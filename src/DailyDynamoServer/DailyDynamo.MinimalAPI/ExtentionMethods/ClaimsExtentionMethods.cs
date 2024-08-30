using System.Security.Claims;
using DailyDynamo.Shared.Common;

namespace DailyDynamo.MinimalAPI.ExtentionMethods;

public static class ClaimsExtentionMethods
{
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        GetClaim(claimsPrincipal, "Id", out var id);

        return Guid.TryParse(id, out var userId) ? userId : Guid.Empty;
    }

    public static UserRole GetUserRole(this ClaimsPrincipal claimsPrincipal)
    {
        var roleClaim = claimsPrincipal.FindFirst("Role");
        return (UserRole)Convert.ToInt32(roleClaim.Value);
    }

    private static void GetClaim(ClaimsPrincipal claimsPrincipal, string typeName, out string? value)
    {
        value = claimsPrincipal.Claims.FirstOrDefault(claim => claim.Type == typeName)?.Value;
    }
}
