using DailyDynamo.MinimalAPI.ExtentionMethods;
using DailyDynamo.MinimalAPI.Utilities;
using DailyDynamo.Services.Interfaces;
using DailyDynamo.Shared.Common;
using DailyDynamo.Shared.ExtensionMethods;
using DailyDynamo.Shared.Models;
using DailyDynamo.Shared.Models.DTO;
using DailyDynamo.Shared.Models.DTO.Employee;
using DailyDynamo.Shared.Models.DTO.Password;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DailyDynamo.MinimalAPI.API
{
    public class AuthenticationRoute
    {
        private readonly IUserService _userService;

        public AuthenticationRoute(IUserService userService)
        {
            _userService = userService;
        }

        public void AddRoutes(WebApplication app)
        {
            app.MapPost("/addUser", async ([FromBody] SignUpViewModel newUser) => await SignUp(newUser));

            app.MapPost("/login", async ([FromBody] LoginRequest model) => await Login(model));

            app.MapGet("/employees",
                async ([FromQuery] int page = 1, [FromQuery] int offset = 10) => await GetEmployees(page, offset));

            app.MapPatch("/password",
                async ([FromBody] UpdatePasswordRequest model, HttpContext context) =>
                await UpdatePassword(model, context));

            app.MapPut("/password",
                [Authorize] async ([FromBody] ChangePasswordRequest model, HttpContext context) =>
                await ChangePassword(model, context));

            app.MapPost("/reset-password",
                [RoleAuthorization(("Admin"))]
            async ([FromBody] ResetPasswordRequest model, HttpContext context) =>
                await ResetPassword(model, context));

            app.MapPost("/forgot-password",
                async ([FromBody] ForgotPasswordRequest model) => await ForgotPassword(model));

            app.MapPatch("/deactivateOrActivate-account", [Authorize]
            async ([FromQuery] Guid id, HttpContext context) =>
                    await DeactivateOrActivateAccount(id, context));

            app.MapGet("/profile", async ([FromQuery] Guid? id, HttpContext context) =>
                await GetProfile(id, context));

            app.MapPatch("/profile", async ([FromForm] ProfilePatchRequest model, HttpContext context)
                => await PatchProfile(model, context))
                .DisableAntiforgery();

            app.MapPut("/profile", async ([FromForm] ProfileUpdateRequest model, HttpContext context)
                => await UpdateProfile(model, context))
                .DisableAntiforgery();

            app.MapGet("/getUserActiveState", async ([FromQuery] Guid? id, HttpContext context)
                => await GetUserActiveState(id, context));
        }

        private async Task<APIResponseModel<bool>> ChangePassword(ChangePasswordRequest model, HttpContext context)
        {
            if(model.NewPassword != model.ConfirmPassword)
                return new("Password and confirm password does not match");

            return new(await _userService.ChangePassword(model, context.User.GetUserId()));
        }

        private async Task<APIResponseModel<ProfileResponse>> UpdateProfile(ProfileUpdateRequest model, HttpContext context)
            => new(await _userService.UpdateProfile(model, context.User.GetUserId()));


        private async Task<APIResponseModel<ProfilePatchResponse>> PatchProfile(ProfilePatchRequest model, HttpContext context)
            => new(await _userService.PatchProfile(model, context.User.GetUserId()));

        private async Task<APIResponseModel<ForgotPasswordResponse>> ForgotPassword(ForgotPasswordRequest model)
        {
            if(string.IsNullOrEmpty(model.Email))
            {
                return new APIResponseModel<ForgotPasswordResponse>("Invalid email");
            }

            return new APIResponseModel<ForgotPasswordResponse>(await _userService.ForgotPassword(model));
        }


        private async Task<APIResponseModel<bool>> DeactivateOrActivateAccount(Guid id, HttpContext context)
        {
            Guid logedUserId = context.User.GetUserId();
            return new APIResponseModel<bool>(await _userService.DeactivateOrActivateAccount(id, logedUserId));
        }

        protected async virtual Task<APIResponseModel<bool>> SignUp(SignUpViewModel newUser)
        {
            if(newUser == null)
                return new APIResponseModel<bool>("User is null");

            var validation = ExtensionMethods.ValidateSignUpModel(newUser);

            if(!validation.Item1)
                return new APIResponseModel<bool>(validation.Item2);

            var result = await _userService.SignUp(newUser);

            return new APIResponseModel<bool>(result);
        }


        /// <summary>
        /// To Authenticate user after Login
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="userService"></param>
        /// <returns></returns>
        private static async Task<ClaimDataViewModel> Authenticate(string emailId, IUserService userService)
        {
            ClaimDataViewModel user = null;
            var currentUser = (await userService.GetUserAsync(emailId)).Data;
            if(currentUser != null)
            {
                user = new()
                {
                    Id = currentUser.Id,
                    FirstName = currentUser.FirstName,
                    LastName = currentUser.LastName,
                    EmailId = currentUser.EmailId,
                };
                return user;
            }

            return null;
        }


        private async Task<APIResponseModel<LoginResponse>> Login(LoginRequest model)
            => new APIResponseModel<LoginResponse>(await _userService.Login(model));

        private async Task<APIResponseModel<IEnumerable<EmployeeResponse>>> GetEmployees(int page, int offset)
        {
            if(page <= 0 || offset <= 0)
                return new APIResponseModel<IEnumerable<EmployeeResponse>>("Provide a valid page and offset");

            var model = new PaginatedRequest<EmployeeRequest> { Offset = offset, Page = page };
            return new APIResponseModel<IEnumerable<EmployeeResponse>>(await _userService.GetEmployees(model));
        }

        private async Task<APIResponseModel<bool>> UpdatePassword(UpdatePasswordRequest model, HttpContext context)
        {
            //validate the data using some library or a maintainable method
            if(string.IsNullOrEmpty(model.NewPassword))
                return new APIResponseModel<bool>("Invalid password");

            if(model.NewPassword != model.ConfirmPassword)
                return new APIResponseModel<bool>("Password and confirm password does not match.");


            return new APIResponseModel<bool>(await _userService.UpdatePassword(model));
        }


        private async Task<APIResponseModel<ResetPasswordResponse>> ResetPassword(ResetPasswordRequest request,
            HttpContext context)
            => new APIResponseModel<ResetPasswordResponse>(
                await _userService.ResetPassword(request, context.User.GetUserId()));


        private async Task<APIResponseModel<ProfileResponse>> GetProfile(Guid? id, HttpContext context)
        {
            Guid userId = context.User.GetUserId();
            if(id is not null || !string.IsNullOrEmpty(id.ToString()))
                userId = (Guid)id!;

            return new APIResponseModel<ProfileResponse>(await _userService.GetProfile(userId));
        }

        private async Task<APIResponseModel<GetUserActiveStateResponse>> GetUserActiveState(Guid? id, HttpContext context)
        {
            Guid userId = context.User.GetUserId();
            if (id is not null || !string.IsNullOrEmpty(id.ToString()))
                userId = (Guid)id!;

            return new APIResponseModel<GetUserActiveStateResponse>(await _userService.GetUserActiveState(userId));
        }

    }
}