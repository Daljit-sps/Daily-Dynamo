using DailyDynamo.Shared.Common;
using DailyDynamo.Shared.Models.DTO;
using DailyDynamo.Shared.Models.DTO.WorkDiary;

namespace DailyDynamo.Services.Interfaces;

public interface IWorkDiaryService
{
    Task<ServiceResult<WorkDiaryResponse>> Add(WorkDiaryRequest model, Guid userId);
    Task<ServiceResult<PaginatedResponse<IEnumerable<WorkDiaryGetResponse>>>> GetAll(PaginatedRequest<WorkDiaryGetRequest> model, ClaimsModel claimsModel);
    Task<ServiceResult<WorkDiaryGetResponse>> Get(Guid id);
    Task<ServiceResult<PaginatedResponse<IEnumerable<WorkDiaryGetResponse>>>> GetByEmployeeId(PaginatedRequest<ClaimsModel> model);
    Task<ServiceResult<bool>> Delete(Guid id);
    Task<ServiceResult<PaginatedResponse<IEnumerable<WorkDiaryDashboardResponse>>>> GetWorkDiariesBasedOnRole(PaginatedRequest<WorkDiaryDashboardResponse> model, ClaimsModel claims);
    Task<ServiceResult<IEnumerable<WorkDiaryGetResponse>>> GetUserWorkDiaryForSelectedDates(string fromDate, string toDate, Guid userId);
}
