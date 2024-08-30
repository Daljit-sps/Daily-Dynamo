
using DailyDynamo.Shared.Models.DTO;
using DailyDynamo.Shared.Models.DTO.WorkDiary;
using DailyDynamo.Shared.Models.Entities;

namespace DailyDynamo.Services.Interfaces;

public interface IWorkDiaryRepository : IGenericRepository<WorkDiary>
{
    Task<PaginatedResponse<IEnumerable<WorkDiaryGetResponse>>> GetAllWorkDiariesAsync<T>(PaginatedRequest<T> model,ClaimsModel claimsModel);
    Task<PaginatedResponse<IEnumerable<WorkDiaryDashboardResponse>>> GetWorkDiariesForDashboardAsync<T>(PaginatedRequest<T> model, ClaimsModel claims);
}
