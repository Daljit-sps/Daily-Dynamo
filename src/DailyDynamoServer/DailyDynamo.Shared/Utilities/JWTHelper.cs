using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DailyDynamo.Shared.Models.DTO.JWTModels;
using Microsoft.IdentityModel.Tokens;
using static DailyDynamo.Shared.Utilities.AppSettingsUtility;

namespace DailyDynamo.Shared.Utilities;

public static class JWTHelper
{
    public static string GenerateToken(JWTClaimsModel user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.JWT.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var Claims = new[]
        {
            new Claim("Email", user.Email),
            new Claim("Id", user.Id.ToString()),
            new Claim("Name", user.FirstName + " " + user.LastName), 
            new Claim("Role", user.Role is null ? "" : user.Role.ToString()!),
            new Claim("Department", user.Department is null ? "": user.Department),
            new Claim("Designation", user.Designation is null ? "": user.Designation),
        };

        var token = new JwtSecurityToken(
            issuer: Settings.JWT.ValidIssuer,
            audience: Settings.JWT.ValidAudience,
            expires: DateTime.Now.AddHours(6),
            claims: Claims,
            signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}
