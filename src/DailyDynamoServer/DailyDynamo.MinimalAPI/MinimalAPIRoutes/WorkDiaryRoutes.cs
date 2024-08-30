
using AutoMapper;
using DailyDynamo.MinimalAPI.ExtentionMethods;
using DailyDynamo.MinimalAPI.Utilities;
using DailyDynamo.Services.Interfaces;
using DailyDynamo.Shared.Common;
using DailyDynamo.Shared.Models.DTO;
using DailyDynamo.Shared.Models.DTO.WorkDiary;
using Microsoft.AspNetCore.Mvc;

namespace DailyDynamo.MinimalAPI.MinimalAPIRoutes;

public static class WorkDiaryRoutes
{
    public static void AddWorkDiaryRoutes(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("work-diary").RequireAuthorization();

        group.MapPost("", async ([FromBody] WorkDiaryRequest model, IWorkDiaryService service, HttpContext context) 
            => await (Add(model, service, context)));

        group.MapGet("", async (IWorkDiaryService service, HttpContext context, [FromQuery] int page = 1, [FromQuery] int offset = 10)
            => await GetAll(page, offset, service, context));

        group.MapGet("{id:guid}", (Guid id, IWorkDiaryService service, HttpContext context) 
            => Get(id, service, context));

        group.MapGet("/employee/{id:guid}", ([FromRoute] Guid id, IWorkDiaryService service, HttpContext context, [FromQuery] int page = 1, [FromQuery] int offset = 30)
            => GetByEmployee(id, page, offset, service, context));

        group.MapGet("/employee", ([FromQuery] string fromDate, [FromQuery] string toDate, IWorkDiaryService service, HttpContext context)
            => GetWorkDiaryForSelectedDates(fromDate, toDate, service, context));

        group.MapDelete("{id:guid}", (Guid id, IWorkDiaryService service) 
            => Delete(id, service));

        group.MapGet("/dashboard", async (IWorkDiaryService service, HttpContext context, [FromQuery] int page = 1, [FromQuery] int offset = 10)
     => await GetAllWorkDiariesForDashboard(page, offset, service, context));

    }

    private static async Task<APIResponseModel<WorkDiaryResponse>> Add(WorkDiaryRequest model, IWorkDiaryService service, HttpContext context)
        => new(await service.Add(model, context.User.GetUserId()));


    private async static Task<APIResponseModel<WorkDiaryGetResponse>> Get(Guid id, IWorkDiaryService service, HttpContext context)
        => new(await service.Get(id));


    private static async Task<APIResponseModel<bool>> Delete(Guid id, IWorkDiaryService service)
        => new(await service.Delete(id));


    private static async Task<APIResponseModel<PaginatedResponse<IEnumerable<WorkDiaryGetResponse>>>> GetAll(int page, int offset, IWorkDiaryService service, HttpContext context)
    {
        var claims = new ClaimsModel { Id = context.User.GetUserId(), UserRole = context.User.GetUserRole() };
        return new(await service.GetAll(new PaginatedRequest<WorkDiaryGetRequest> { Offset = offset, Page = page }, claims));
    }


    private static async Task<APIResponseModel<PaginatedResponse<IEnumerable<WorkDiaryGetResponse>>>> GetByEmployee(Guid id, int page, int offset, IWorkDiaryService service, HttpContext context)
    {
        var model = new PaginatedRequest<ClaimsModel>
        {
            Model = new() { Id = id, UserRole = UserRole.Employee },
            Offset = offset,
            Page = page
        };

        return new(await service.GetByEmployeeId(model));
    }

    private static async Task<APIResponseModel<PaginatedResponse<IEnumerable<WorkDiaryDashboardResponse>>>> GetAllWorkDiariesForDashboard(int page, int offset, IWorkDiaryService service, HttpContext context)
    {
        var claims = new ClaimsModel { Id = context.User.GetUserId(), UserRole = context.User.GetUserRole() };
        return new(await service.GetWorkDiariesBasedOnRole(new PaginatedRequest<WorkDiaryDashboardResponse> { Offset = offset, Page = page }, claims));
    }

    private static async Task<APIResponseModel<IEnumerable<WorkDiaryGetResponse>>> GetWorkDiaryForSelectedDates(string fromDate, string toDate, IWorkDiaryService service, HttpContext context)
    {
        return new(await service.GetUserWorkDiaryForSelectedDates(fromDate, toDate, context.User.GetUserId()));
    }

}
