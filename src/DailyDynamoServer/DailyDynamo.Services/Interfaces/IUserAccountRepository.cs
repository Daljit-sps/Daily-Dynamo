using DailyDynamo.Shared.Models.DTO;
using DailyDynamo.Shared.Models.DTO.Employee;
using DailyDynamo.Shared.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyDynamo.Services.Interfaces;

public interface IUserAccountRepository : IGenericRepository<Account>
{
    Task<IEnumerable<EmployeeResponse>> GetEmployeesAsync(PaginatedRequest<EmployeeRequest> model);
}
