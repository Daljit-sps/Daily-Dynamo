using System;
using System.Globalization;
using System.Reflection;
using AutoMapper;
using DailyDynamo.Services.Interfaces;
using DailyDynamo.Shared.Common;
using DailyDynamo.Shared.ExtensionMethods;
using DailyDynamo.Shared.Models;
using DailyDynamo.Shared.Models.DTO;
using DailyDynamo.Shared.Models.DTO.Employee;
using DailyDynamo.Shared.Models.DTO.JWTModels;
using DailyDynamo.Shared.Models.DTO.Password;
using DailyDynamo.Shared.Models.Entities;
using DailyDynamo.Shared.Utilities;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Tls;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static DailyDynamo.Shared.Utilities.AppSettingsUtility;

namespace DailyDynamo.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISharedService _sharedService;
        private readonly FileManagerService _filesService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ISharedService sharedService, FileManagerService filesService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sharedService = sharedService;
            this._filesService = filesService;
        }


        public async Task<ServiceResult<bool>> SignUp(SignUpViewModel user)
        {
            try
            {
                // Check if email already exists
                var isEmailExists =
                    await _unitOfWork.UserAccount.IsExistsAsync(x => x.UserName.ToLower() == user.EmailId.ToLower());
                if(isEmailExists)
                    return new ServiceResult<bool>(false, "Email Already Exists!");

                // Check if mobile number already exists
                var isMobileNoExists = await _unitOfWork.UserProfile.IsExistsAsync(x => x.MobileNo == user.MobileNo);
                if(isMobileNoExists)
                    return new ServiceResult<bool>(false, "Mobile Number Already Exists!");

                // else generate Id, default password
                var hashPassword = ExtensionMethods.HashPassword(Settings.Password.DefaultPassword);
                var userAccountId = Guid.NewGuid();
                var userProfileId = Guid.NewGuid();

                var userAccount = new Account
                {
                    Id = userAccountId,
                    UserName = user.EmailId.ToLower(),
                    PasswordHash = hashPassword,
                    CreatedBy = userAccountId,
                    UpdatedBy = userAccountId,
                };

                await _unitOfWork.UserAccount.AddAsync(userAccount);

                var userProfile = _mapper.Map<Shared.Models.Entities.Profile>(user);
                userProfile.CreatedBy = userProfile.UpdatedBy = userProfile.Id;
                userProfile.RoleId = (int)UserRole.Employee;
                userProfile.Id = userProfileId;
                userProfile.AccountId = userAccountId;


                await _unitOfWork.UserProfile.AddAsync(userProfile);
                await _unitOfWork.SaveChangesAsync();

                // Read HTML content from file
                string htmlString = ExtensionMethods.ReadHtmlContentFromFile("wwwroot/EmailVerificationTemplate.html");
                bool isCheckedAdded
                    = true;

                if(htmlString == null)
                    return new ServiceResult<bool>(false, "Error reading HTML template");

                var emailVerificationLink =
                    ExtensionMethods.EncodeEmailLinkAsync(Settings.AppURL.NewPasswordPageLink, userAccount.Id,
                        userAccount.UserName, isCheckedAdded);

                // Replace placeholders in the email template with actual values
                var emailBody = htmlString
                    .Replace("{UserName}", $"{userProfile.FirstName} {userProfile.LastName}")
                    .Replace("{emailVerficationURL}", emailVerificationLink);

                //send verfication email
                List<string> emailTo = new() { userProfile.EmailId };
                await _sharedService.EmailSendingAsync("Welcome, please verify your email", emailBody, emailTo, null,
                    null);

                return new ServiceResult<bool>(true, "User Added Successfully!");
            }
            catch(Exception ex)
            {
                return new ServiceResult<bool>(ex, ex.Message);
            }
        }

        public async Task<ServiceResult<ClaimDataViewModel>> GetUserAsync(string emailId)
        {
            try
            {
                // Check if user exists
                var user = await _unitOfWork.UserProfile.GetAsync(x => x.EmailId == emailId);
                if(user != null)
                {
                    // Map entity to DTO
                    var userDto = _mapper.Map<ClaimDataViewModel>(user);
                    return new ServiceResult<ClaimDataViewModel>(userDto, "user details");
                }

                else
                {
                    return new ServiceResult<ClaimDataViewModel>(new ClaimDataViewModel(),
                        "user with this email doesn't exists.");
                }
            }
            catch(Exception ex)
            {
                return new ServiceResult<ClaimDataViewModel>(ex, ex.Message);
            }
        }

        public async Task<ServiceResult<LoginResponse>> Login(LoginRequest model)
        {
            try
            {
                var user = await _unitOfWork.UserAccount.GetAsync(
                    user => user.UserName == model.Email,
                    user => user.Profile,
                    user => user.Profile.Department,
                    user => user.Profile.Designation
                );

                if(user is null || !ExtensionMethods.VerifyHash(model.Password, user.PasswordHash))
                    return new ServiceResult<LoginResponse>(
                        null,
                        "Invalid Credentials"
                    );


                if (!user.IsVerified)
                    return new ServiceResult<LoginResponse>(
                        null,
                        "Please verify you email before logging in."
                    );

                if (!user.IsActive)
                    return new ServiceResult<LoginResponse>(
                        null,
                        "Your account has been deactivated, kindly contact admin."
                    );

                var token = JWTHelper.GenerateToken(new JWTClaimsModel
                {
                    FirstName = user.Profile?.FirstName,
                    LastName = user.Profile?.LastName,
                    Email = user.UserName,
                    Department = user.Profile?.Department?.DepartmentName,
                    Designation = user.Profile?.Designation?.DesignationName,
                    Id = user.Profile.Id,
                    Role = user.Profile?.RoleId
                });

                return new ServiceResult<LoginResponse>(
                    new LoginResponse { Token = token },
                    "Login successful"
                );
            }
            catch(Exception)
            {
                return new ServiceResult<LoginResponse>(
                    null,
                    "Something went wrong"
                );
            }
        }

        public async Task<ServiceResult<IEnumerable<EmployeeResponse>>> GetEmployees(
            PaginatedRequest<EmployeeRequest> model)
        {
            var response = await _unitOfWork.UserAccount.GetEmployeesAsync(model);
            if(response.Any())
                return new ServiceResult<IEnumerable<EmployeeResponse>>(response, "Successful");

            return new ServiceResult<IEnumerable<EmployeeResponse>>(null, "No employees found");
        }

        public async Task<ServiceResult<bool>> UpdatePassword(UpdatePasswordRequest model)
        {
            var user = await _unitOfWork.UserAccount.GetAsync(user => user.Id == model.AccountId);
            if(user is null)
                return new ServiceResult<bool>(null, "No user found");


            var newPasswordHash = ExtensionMethods.HashPassword(model.NewPassword);

            if(newPasswordHash == user.PasswordHash)
                return new ServiceResult<bool>(null, "This password has been used please set a new password.");

            user.PasswordHash = newPasswordHash;
            user.IsVerified = true;
            user.IsActive = true;

            if(model.IsCheckAdded)
            {
                var userProfile = await _unitOfWork.UserProfile.GetAsync(user => user.AccountId == model.AccountId);
                if(userProfile != null)
                    userProfile.IsActive = true;
            }


            int returnValue = await _unitOfWork.SaveChangesAsync();

            if(returnValue > 0)
                return new ServiceResult<bool>(true, "Password set successfully");

            return new ServiceResult<bool>(null, "Something went wrong, Please try again later.");
        }

        public async Task<ServiceResult<ResetPasswordResponse>> ResetPassword(ResetPasswordRequest model, Guid userId)
        {
            var user = await _unitOfWork.UserProfile.GetAsync(user => user.Id == model.UserId);
            if (user is null)
                return new ServiceResult<ResetPasswordResponse>(null, "User not found");

            var userAccount = await _unitOfWork.UserAccount.GetAsync(account=>account.Id == user.AccountId);

          
            userAccount.PasswordHash = ExtensionMethods.HashPassword(AppSettingsUtility.Settings.Password.DefaultPassword);
            userAccount.UpdatedOn = DateTime.Now;
            userAccount.UpdatedBy = userId;

            int returnValue = await _unitOfWork.SaveChangesAsync();

            if(returnValue <= 0)
                return new ServiceResult<ResetPasswordResponse>(null, "Something went wrong, Try again!");

            bool isCheckAdded = false;
            var passwordResetLink =
                ExtensionMethods.EncodeEmailLinkAsync(Settings.AppURL.NewPasswordPageLink, userAccount.Id, userAccount.UserName, isCheckAdded);

            var emailContent = ExtensionMethods
                .ReadHtmlContentFromFile(Settings.EmailTemplatePaths.PasswordResetTemplate)
                .Replace("{NewPasswordLink}", passwordResetLink)
                .Replace("{UserName}", $"{user.FirstName} {user.LastName}");

            var (isEmailSent, message) =
                await _sharedService.EmailSendingAsync("Password Reset Successful", emailContent, [userAccount.UserName]);

            if(!isEmailSent)
                return new ServiceResult<ResetPasswordResponse>(null, "Error sending email to user");


            return new ServiceResult<ResetPasswordResponse>(new ResetPasswordResponse { },
                "Password reset successfull");
        }

        public async Task<ServiceResult<ForgotPasswordResponse>> ForgotPassword(ForgotPasswordRequest model)
        {
            var user = await _unitOfWork.UserAccount.GetAsync(user => user.UserName == model.Email && user.IsActive,
                user => user.Profile!);

            if(user is null)
                return new ServiceResult<ForgotPasswordResponse>(null, "Please check you email and try again.");

            if(!user.IsActive)
                return new ServiceResult<ForgotPasswordResponse>(null, "This account is not Active please contact admin.");

            user.IsVerified = false;
            user.UpdatedOn = DateTime.Now;

            await _unitOfWork.SaveChangesAsync();

            bool isCheckAdded = false;
            string passwordResetLink =
                ExtensionMethods.EncodeEmailLinkAsync(Settings.AppURL.NewPasswordPageLink, user.Id, user.UserName, isCheckAdded);

            string emailContent = ExtensionMethods
                .ReadHtmlContentFromFile(Settings.EmailTemplatePaths.ForgotPasswordTemplate)
                .Replace("{NewPasswordLink}", passwordResetLink)
                .Replace("{UserName}", $"{user.Profile?.FirstName} {user.Profile?.LastName}");

            var (isEmailSent, message) =
                await _sharedService.EmailSendingAsync("Set new password", emailContent, [user.UserName]);

            if(!isEmailSent)
                return new ServiceResult<ForgotPasswordResponse>(null,
                    "Something went wrong while sending email, Try again.");

            return new ServiceResult<ForgotPasswordResponse>(new ForgotPasswordResponse() { },
                "Password reset link sent to your email successfully");
        }

        public async Task<ServiceResult<ProfileResponse>> GetProfile(Guid userId)
        {
            var profile = await _unitOfWork.UserProfile
                .GetAsync(
                user => user.Id == userId);

            if(profile is null)
                return new(null, "Please login again");

            var isAccountActive = (await _unitOfWork.UserAccount.GetAsync(account => account.Id == profile.AccountId)).IsActive;
            var profileResponse = _mapper.Map<ProfileResponse>(profile);
            profileResponse.IsAccountActive = isAccountActive;

            return new(profileResponse, "Successful");
        }

        public async Task<ServiceResult<ProfilePatchResponse>> PatchProfile(ProfilePatchRequest model, Guid userId)
        {
            if(await _unitOfWork.UserProfile.IsExistsAsync(user => user.MobileNo == model.MobileNo && user.Id != userId))
                return new(null, "Mobile number already registered");

            var profile = await _unitOfWork.UserProfile.GetAsync(user => user.Id == userId);

            // Convert string date to DateOnly
            if (!string.IsNullOrEmpty(model.DOB))
            {
                if (DateTime.TryParse(model.DOB, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    DateOnly dob = DateOnly.FromDateTime(parsedDate.Date);
                    profile.DateOfBirth = dob;
                }
                else
                {
                    return new(null, "Invalid date format for date of birth");
                }
            }
            _mapper.Map(model, profile);

            if(model.ProfileImage is not null)
                await UpsertProfileImage(profile, model.ProfileImage);


            await _unitOfWork.SaveChangesAsync();

            return new(_mapper.Map<ProfilePatchResponse>(profile), "Profile updated successfully");
        }

        public async Task<ServiceResult<ProfileResponse>> UpdateProfile(ProfileUpdateRequest model, Guid userId)
        {
            var profile = await _unitOfWork.UserProfile.GetAsync(user => user.Id == model.Id);

            if(profile is null)
                return new(null, "User not found");

            // Convert string date to DateOnly
            if (!string.IsNullOrEmpty(model.DOB))
            {
                if (DateTime.TryParse(model.DOB, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    DateOnly dob = DateOnly.FromDateTime(parsedDate.Date);
                    profile.DateOfBirth = dob;
                }
                else
                {
                    return new(null, "Invalid date format for date of birth");
                }
            }

            _mapper.Map(model, profile);

            if(model.ProfileImage is not null)
                await UpsertProfileImage(profile, model.ProfileImage);

            await _unitOfWork.SaveChangesAsync();

            return new(_mapper.Map<ProfileResponse>(profile), "User updated successfully");
        }


        private async Task UpsertProfileImage(Shared.Models.Entities.Profile profile, IFormFile file)
        {
            if(!string.IsNullOrEmpty(profile.ProfileImageUrl))
                _filesService.Delete(profile.ProfileImageUrl);

            profile.ProfileImageUrl = await _filesService.SaveFile(
                file, ExtensionMethods.GenerateRandomFileName(file.FileName));
        }

        public async Task<ServiceResult<bool>> ChangePassword(ChangePasswordRequest model, Guid userId)
        {
            var user = await _unitOfWork.UserProfile.GetAsync(user => user.Id == userId);
            if (user is null)
                return new ServiceResult<bool>(null, "Please re-login & try again.");

            var userAccount = await _unitOfWork.UserAccount.GetAsync(account => account.Id == user.AccountId);

            if (ExtensionMethods.HashPassword(model.OldPassword) != userAccount.PasswordHash)
                return new(false, "Old password is incorrect");

            var newPasswordHash = ExtensionMethods.HashPassword(model.NewPassword);

            if (newPasswordHash == userAccount.PasswordHash)
                return new ServiceResult<bool>(null, "This password has been used please set a new password.");

            userAccount.PasswordHash = newPasswordHash;

            await _unitOfWork.SaveChangesAsync();

            return new ServiceResult<bool>(true, "Password set successfully");

        }

        public async Task<ServiceResult<bool>> DeactivateOrActivateAccount(Guid id, Guid logedUserId)
        {
            try
            {
                var profile = await _unitOfWork.UserProfile.GetAsync(user => user.Id == id);
                if (profile is null)
                    return new ServiceResult<bool>(false, "No user found");

                var userAccount = await _unitOfWork.UserAccount.GetAsync(user => user.Id == profile.AccountId);

                if (userAccount.IsActive)
                    userAccount.IsActive = false;
                else
                    userAccount.IsActive = true;

                userAccount.UpdatedOn = DateTime.UtcNow;
                userAccount.UpdatedBy = logedUserId;

                int returnValue = await _unitOfWork.SaveChangesAsync();

                if (returnValue <= 0)
                    return new ServiceResult<bool>(false, "Something went wrong, Try again!");

                else
                {
                    if (userAccount.IsActive)
                        return new ServiceResult<bool>(true, "Account activated successfully.");

                    else
                        return new ServiceResult<bool>(true, "Account deactivated successfully.");
                }


            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(ex, ex.Message);
            }
        }

        public async Task<ServiceResult<GetUserActiveStateResponse>> GetUserActiveState(Guid userId)
        {
            var profile = await _unitOfWork.UserProfile
                .GetAsync(
                user => user.Id == userId);

            if (profile is null)
                return new ServiceResult<GetUserActiveStateResponse>(new GetUserActiveStateResponse { isSuccess = false, isUserActive = false }, "No user found");

            var userAccount = await _unitOfWork.UserAccount.GetAsync(user => user.Id == profile.AccountId);

            return new ServiceResult<GetUserActiveStateResponse>(new GetUserActiveStateResponse { isSuccess = true, isUserActive = userAccount.IsActive }, "User get successfully.");
        }
    }
}