using Microsoft.AspNetCore.Http;

namespace DailyDynamo.Shared.Models.DTO.Employee;

public class ProfileUpdateRequest
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? EmailId { get; set; }
    public string MobileNo { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? DOB { get; set; }
    public Guid GenderId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? DesignationId { get; set; }
    public Guid? ManagerId { get; set; }
    public IFormFile? ProfileImage { get; set; }
}
