namespace DailyDynamo.Shared.Models.DTO.Password;

public class UpdatePasswordRequest
{
    public Guid AccountId { get; set; }
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;

    public bool IsCheckAdded { get; set; }
}