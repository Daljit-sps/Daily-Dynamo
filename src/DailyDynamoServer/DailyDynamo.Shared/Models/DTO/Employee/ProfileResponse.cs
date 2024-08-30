namespace DailyDynamo.Shared.Models.DTO.Employee;

public class ProfileResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;
    public string? MobileNo { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? DateOfBirth { get; set; }
    public Guid? GenderId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? DesignationId { get; set; }
    public Guid? ManagerId { get; set; }
    public bool IsAccountActive { get; set; }
    public string? ProfileImageUrl { get; set; } = string.Empty;
}
