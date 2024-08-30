using DailyDynamo.Shared.Common;
using DailyDynamo.Shared.Models;
using DailyDynamo.Shared.Models.DTO;
using DailyDynamo.Shared.Models.DTO.Employee;
using DailyDynamo.Shared.Models.DTO.Password;

namespace DailyDynamo.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<bool>> SignUp(SignUpViewModel user);
        Task<ServiceResult<LoginResponse>> Login(LoginRequest model);
        Task<ServiceResult<ClaimDataViewModel>> GetUserAsync(string emailId);
        Task<ServiceResult<IEnumerable<EmployeeResponse>>> GetEmployees(PaginatedRequest<EmployeeRequest> model);
        Task<ServiceResult<bool>> UpdatePassword(UpdatePasswordRequest model);
        Task<ServiceResult<bool>> ChangePassword(ChangePasswordRequest model, Guid userId);
        Task<ServiceResult<ResetPasswordResponse>> ResetPassword(ResetPasswordRequest model, Guid userId);
        Task<ServiceResult<ForgotPasswordResponse>> ForgotPassword(ForgotPasswordRequest model);
        Task<ServiceResult<bool>> DeactivateOrActivateAccount(Guid id, Guid logedUserId);
        Task<ServiceResult<ProfileResponse>> GetProfile(Guid userId);
        Task<ServiceResult<ProfilePatchResponse>> PatchProfile(ProfilePatchRequest model, Guid userId);
        Task<ServiceResult<ProfileResponse>> UpdateProfile(ProfileUpdateRequest model, Guid userId);
        Task<ServiceResult<GetUserActiveStateResponse>> GetUserActiveState(Guid userId);
    }
}
