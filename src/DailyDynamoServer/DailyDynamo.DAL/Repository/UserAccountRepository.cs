using DailyDynamo.DAL.Context;
using DailyDynamo.Services.Interfaces;
using DailyDynamo.Shared.Models.DTO;
using DailyDynamo.Shared.Models.DTO.Employee;
using DailyDynamo.Shared.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DailyDynamo.DAL.Repository;

public class UserAccountRepository : GenericRepository<Account>, IUserAccountRepository
{
    private readonly DailyDynamoDatabaseContext context;

    public UserAccountRepository(DailyDynamoDatabaseContext context) : base(context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<EmployeeResponse>> GetEmployeesAsync(PaginatedRequest<EmployeeRequest> model)
    {
        var userQuery = context.Accounts
            .Include(user => user.Profile)
            .Include(user => user.Profile.Manager)
            .Include(user => user.Profile.Designation);

        var users = await userQuery.Where(user => user.Profile.RoleId != (int)Shared.Common.UserRole.Admin).Select(user => new EmployeeResponse
        {
            Id = user.Profile.Id,
            UserEmail = user.UserName,
            MobileNo = user.Profile.MobileNo,
            ProfileImage = user.Profile.ProfileImageUrl,
            Department = user.Profile.Department.DepartmentCode,
            Designation = user.Profile.Designation.DesignationName,
            FullName = $"{user.Profile.FirstName} {user.Profile.LastName}",
            ManagerName = $"{user.Profile.Manager.FirstName} {user.Profile.Manager.LastName}",
        })
            .Skip((model.Page - 1) * model.Offset)
            .Take(model.Offset)
            .ToListAsync();

        return users;

    }
}
