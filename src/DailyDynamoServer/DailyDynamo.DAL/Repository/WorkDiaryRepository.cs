using DailyDynamo.DAL.Context;
using DailyDynamo.Services.Interfaces;
using DailyDynamo.Shared.Common;
using DailyDynamo.Shared.Models.DTO;
using DailyDynamo.Shared.Models.DTO.WorkDiary;
using DailyDynamo.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DailyDynamo.DAL.Repository;

public class WorkDiaryRepository : GenericRepository<WorkDiary>, IWorkDiaryRepository
{
    private readonly DailyDynamoDatabaseContext context;

    public WorkDiaryRepository(DailyDynamoDatabaseContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<PaginatedResponse<IEnumerable<WorkDiaryGetResponse>>> GetAllWorkDiariesAsync<T>(PaginatedRequest<T> model, ClaimsModel claims)
    {
        var workDiaries = context.WorkDiaries.Select(w => new WorkDiaryGetResponse
        {
            ChallengesFaced = w.ChallengesFaced,
            CreatedBy = w.CreatedBy,
            CreatedOn = w.CreatedOn,
            Id = w.Id,
            NextDayPlan = w.NextDayPlan,
            ReportDate = w.ReportDate,
            TaskAccomplished = w.TaskAccomplished
        });


        int totalItems = 0;
        List<WorkDiaryGetResponse> data = new();

        if(claims.UserRole == Shared.Common.UserRole.Employee)
        {
            totalItems = await workDiaries.Where(w => w.CreatedBy == claims.Id).CountAsync();
            data = await workDiaries
                .Where(w => w.CreatedBy == claims.Id)
                .Skip((model.Page - 1) * model.Offset)
                .Take(model.Offset)
                .OrderByDescending(w => w.ReportDate)
                .ToListAsync();

        }

        else
        {
            totalItems = await workDiaries.CountAsync();
            data = await workDiaries
                    .Skip((model.Page - 1) * model.Offset)
                    .Take(model.Offset)
                    .OrderByDescending(w => w.ReportDate)
                    .ToListAsync();
        }

        return new PaginatedResponse<IEnumerable<WorkDiaryGetResponse>>
        {
            Data = data,
            TotalItems = totalItems,
        };
    }

    public async Task<PaginatedResponse<IEnumerable<WorkDiaryDashboardResponse>>> GetWorkDiariesForDashboardAsync<T>(PaginatedRequest<T> model, ClaimsModel claims)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
      
        int totalItems = 0;
        List<WorkDiaryDashboardResponse> userDSRStatusList = new();
        List<WorkDiaryDashboardResponse> data = new();

        var getUsers = await context.Profiles
            .Where(user => ((claims.UserRole == UserRole.Admin && user.RoleId != (int)UserRole.Admin) || (user.ManagerId == claims.Id)) && user.IsActive)
            .Select(user => new
            {
                user.Id,
                FullName = user.FirstName + " " + user.LastName,
            })
            .ToListAsync();

        if (getUsers.Any())
        {
            var workDiaries = await context.WorkDiaries
           .Where(w => w.ReportDate == today)
           .ToListAsync();

            foreach (var user in getUsers)
            {
                var userEntryToday = workDiaries.FirstOrDefault(w => w.CreatedBy == user.Id);
                var userEntry = new WorkDiaryDashboardResponse
                {
                    Name = user.FullName,
                };

                if (userEntryToday != null)
                {
                    userEntry.DSRId = userEntryToday.Id;
                    userEntry.Status = "Submitted";
                }
                else
                {
                    userEntry.Status = "Not Submitted";
                }

                userDSRStatusList.Add(userEntry);
            }

            userDSRStatusList = userDSRStatusList
                .OrderByDescending(entry => entry.Status == "Submitted")
                .ToList();

            totalItems = userDSRStatusList.Count;
            data = userDSRStatusList
                .Skip((model.Page - 1) * model.Offset)
                .Take(model.Offset)
                .ToList();
        }
        else
        {
            var workDiaries = await context.WorkDiaries
           .Where(w => w.CreatedBy == claims.Id)
           .ToListAsync();

            userDSRStatusList = workDiaries.Select(w => new WorkDiaryDashboardResponse
            {
                DSRId = w.Id,
                ReportDate = w.ReportDate,
                TaskAccomplished = w.TaskAccomplished,
                NextDayPlan = w.NextDayPlan
            }).OrderByDescending(w=>w.ReportDate).ToList();

            totalItems = workDiaries.Count();
            data = userDSRStatusList
                .Skip((model.Page - 1) * model.Offset)
                .Take(model.Offset)
                .ToList();
        }


        return new PaginatedResponse<IEnumerable<WorkDiaryDashboardResponse>>
        {
            Data = data,
            TotalItems = totalItems,
        };
    }

}
