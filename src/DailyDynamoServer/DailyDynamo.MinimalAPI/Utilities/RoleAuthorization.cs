using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace DailyDynamo.MinimalAPI.Utilities;

[AttributeUsage(AttributeTargets.Method)]
/*public class RoleAuthorization : AuthorizeAttribute
{
    public RoleAuthorization(params object[] roles)
    {
        if(roles.Any(r => r.GetType().BaseType != typeof(Enum)))
            throw new ArgumentException("invalid roles");

        Roles = string.Join(",", roles.Select(r => Convert.ToInt32(r)));
    }
}*/
public class RoleAuthorizationAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;

    public RoleAuthorizationAttribute(params string[] roles)
    {
        _roles = roles ?? throw new ArgumentNullException(nameof(roles));
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userRole = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserRole")?.Value;
        if (userRole == null || !_roles.Contains(userRole))
        {
            context.Result = new ForbidResult();
        }
    }
}
