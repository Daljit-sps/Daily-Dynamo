namespace DailyDynamo.Shared.Models.DTO.JWTModels;

public class JWTClaimsModel
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? Role { get; set; }
    public string? Department { get; set; } = string.Empty;
    public string? Designation { get; set; } = string.Empty;
}
