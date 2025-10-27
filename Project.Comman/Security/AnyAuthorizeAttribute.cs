namespace Ettad.CrossCutting.Common.Security;

using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ettad.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Ettad.Common.Interfaces;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class CheckAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public string[] RequiredPolicies { get; set; }

    public CheckAuthorizeAttribute(params string[] requiredPolicies)
    {
        RequiredPolicies = requiredPolicies;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        var user = httpContext.User;

        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Example: using a custom permission service or UserManager to get roles/claims from DB
        var permissionService = httpContext.RequestServices.GetService<IPermissionService>();
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userPolicies = permissionService?.GetUserPermissions(userId).Result; // Assume it returns List<string>

        if (userPolicies == null || !userPolicies.Intersect(RequiredPolicies).Any())
        {
            context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
        }
    }
}

