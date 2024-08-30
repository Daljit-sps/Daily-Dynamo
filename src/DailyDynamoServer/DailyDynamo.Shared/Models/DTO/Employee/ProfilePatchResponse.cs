using Microsoft.AspNetCore.Http;

namespace DailyDynamo.Shared.Models.DTO.Employee;

public class ProfilePatchResponse
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;
    public string MobileNo { get; set; } = string.Empty;
    public DateOnly DOB { get; set; }
    public Guid GenderId { get; set; }
    public string ProfileImageUrl { get; set; } = string.Empty;

}
