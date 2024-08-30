namespace DailyDynamo.Shared.Models.DTO.Employee;

public class EmployeeResponse
{
    public Guid Id { get; set; } 
    public string? ProfileImage { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Designation { get; set; } = string.Empty;
    public string? Department { get; set; } = string.Empty;
    public string? ManagerName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string? MobileNo { get; set; }
}
