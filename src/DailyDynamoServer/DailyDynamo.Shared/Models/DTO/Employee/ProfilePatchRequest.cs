using Microsoft.AspNetCore.Http;

namespace DailyDynamo.Shared.Models.DTO.Employee;

public class ProfilePatchRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;
    public string MobileNo { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? DOB { get; set; }
    public Guid GenderId { get; set; }
    public IFormFile? ProfileImage { get; set; }
}
