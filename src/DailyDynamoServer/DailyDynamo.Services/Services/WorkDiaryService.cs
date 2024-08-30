using AutoMapper;
using DailyDynamo.Services.Interfaces;
using DailyDynamo.Shared.Common;
using DailyDynamo.Shared.ExtensionMethods;
using DailyDynamo.Shared.Models.DTO;
using DailyDynamo.Shared.Models.DTO.WorkDiary;
using DailyDynamo.Shared.Models.Entities;
using DailyDynamo.Shared.Utilities;

namespace DailyDynamo.Services.Services;

public class WorkDiaryService : IWorkDiaryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISharedService _sharedService;
    private readonly IMapper mapper;

    public WorkDiaryService(IUnitOfWork unitOfWork, ISharedService sharedService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _sharedService = sharedService;
        this.mapper = mapper;
    }

    public async Task<ServiceResult<WorkDiaryResponse>> Add(WorkDiaryRequest model, Guid userId)
    {
        var user = await _unitOfWork.UserProfile.GetAsync(user => user.Id == userId, user => user.Manager);

        if(user is null)
            return new(null, "DSR submission failed, please login and try again!");


        var workDiary = mapper.Map<WorkDiary>(model);

        workDiary.CreatedBy = userId;
        workDiary.UpdatedBy = userId;
        workDiary.Id = Guid.NewGuid();
        workDiary.IsActive = true;
        workDiary.CreatedOn = DateTime.UtcNow;
        workDiary.UpdatedOn = DateTime.UtcNow;
        workDiary.IsActive = true;

        await _unitOfWork.WorkDiary.AddAsync(workDiary);

        await _unitOfWork.SaveChangesAsync();

        var response = mapper.Map<WorkDiaryResponse>(workDiary);


        if(user.Manager is null)
            return new(response, "DSR Submitted successfully");

        var emailContent = ExtensionMethods
            .ReadHtmlContentFromFile(AppSettingsUtility.Settings.EmailTemplatePaths.DSRSubmittedTemplate)
            .Replace("{ManagerName}", $"{user.Manager?.FirstName} {user.Manager?.LastName}")
            .Replace("{EmployeeName}", $"{user.FirstName} {user.LastName}")
            .Replace("{ReportDate}", workDiary.ReportDate.ToString())
            .Replace("{TaskAccomplished}", workDiary.TaskAccomplished)
            .Replace("{ChallengesFaced}", workDiary.ChallengesFaced)
            .Replace("{NextDayPlan}", workDiary.NextDayPlan)
            .Replace("{DSRLink}", "");

        await _sharedService.EmailSendingAsync($"{user.FirstName} {user.LastName} Submitted DSR", emailContent, [user.Manager?.EmailId]);


        return new(response, "DSR Submitted successfully");

    }

    public async Task<ServiceResult<WorkDiaryGetResponse>> Get(Guid id)
    {
        var workDiary = await _unitOfWork.WorkDiary.GetAsync(w => w.Id == id, w=>w.CreatedByNavigation);

        if(workDiary is null)
            return new(null, "No work diary found");

        var response = mapper.Map<WorkDiaryGetResponse>(workDiary);

        return new(response, "Successful");


    }

    public async Task<ServiceResult<bool>> Delete(Guid id)
    {
        var workItem = await _unitOfWork.WorkDiary.GetAsync(w => w.Id == id);

        if(workItem is null)
            return new(false, "No work item found");

        await _unitOfWork.WorkDiary.DeleteAsync(workItem);

        await _unitOfWork.SaveChangesAsync();
        return new(true, "Work diary deleted successfully");
    }

    // the reason i am passing the claims model from the API layer is
    // to reuse the GetAllWorkDiariesAsync() method of work diary service.
    public async Task<ServiceResult<PaginatedResponse<IEnumerable<WorkDiaryGetResponse>>>> GetAll(PaginatedRequest<WorkDiaryGetRequest> model, ClaimsModel claims)
    {
        var workDiaries = await _unitOfWork.WorkDiary.GetAllWorkDiariesAsync(model, claims);

        if(workDiaries.Data is not null && workDiaries.Data.Any())
            return new(workDiaries, "Successful");

        return new(null, "No Work Diaries found");
    }

    // the reason i am passing the claims model from the API layer is
    // to reuse the GetAllWorkDiariesAsync() method of work diary service.
    public async Task<ServiceResult<PaginatedResponse<IEnumerable<WorkDiaryGetResponse>>>> GetByEmployeeId(PaginatedRequest<ClaimsModel> claims)
    {
        var workDiaries = await _unitOfWork.WorkDiary
            .GetAllWorkDiariesAsync(claims, new ClaimsModel { Id = claims.Model.Id, UserRole = claims.Model.UserRole });

        if(workDiaries.Data is not null && workDiaries.Data.Any())
            return new(workDiaries, "Successful");

        return new(null, "No Work Diaries found");

    }


    public async Task<ServiceResult<PaginatedResponse<IEnumerable<WorkDiaryDashboardResponse>>>> GetWorkDiariesBasedOnRole(PaginatedRequest<WorkDiaryDashboardResponse> model, ClaimsModel claims)
    {
        var workDiaries = await _unitOfWork.WorkDiary
             .GetWorkDiariesForDashboardAsync(model, claims);

        if (workDiaries.Data is not null && workDiaries.Data.Any())
            return new(workDiaries, "Successful");

        return new(null, "No Work Diaries found");

    }

    public async Task<ServiceResult<IEnumerable<WorkDiaryGetResponse>>> GetUserWorkDiaryForSelectedDates(string fromDate, string toDate, Guid userId)
    {
        if (!DateTime.TryParse(fromDate, out DateTime FromDate) ||
            !DateTime.TryParse(toDate, out DateTime ToDate))
            return new(null, "Invalid date format.");

        var workDiaries = await _unitOfWork.WorkDiary.GetAllAsync(dsr => dsr.CreatedBy == userId &&
            dsr.CreatedOn >= FromDate && dsr.CreatedOn <= ToDate);

        var response = workDiaries.Select(d => mapper.Map<WorkDiaryGetResponse>(d));

        return new(response, "Successful");
    }

}
