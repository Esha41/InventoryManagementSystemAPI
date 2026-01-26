namespace Ettad.CrossCutting.Common.Security;

using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ettad.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class CheckAuthorizeAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    public string[] RequiredPolicies { get; set; }

    public CheckAuthorizeAttribute(params string[] requiredPolicies)
    {
        RequiredPolicies = requiredPolicies;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpContext = context.HttpContext;
        var user = httpContext.User;

        if (!user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // Check if user is a super admin - super admins bypass all permission checks
        var isSuperAdmin = user.FindFirst("IsSuperAdmin")?.Value;
        if (isSuperAdmin == "true")
        {
            return; // Super admin has access to everything
        }

        // ---------------------------------------------------------
        // DELEGATION CHECK: Block if user is delegating authority
        // ---------------------------------------------------------
        // We use IDelegationAuthorizationService to avoid circular dependency
        var delegationService = httpContext.RequestServices.GetService<IDelegationAuthorizationService>();
        if (delegationService != null)
        {
            // Safer approach: Check HTTP Method.
            var method = httpContext.Request.Method.ToUpper();
            if (method != "GET" && method != "OPTIONS" && method != "HEAD")
            {
                 bool isRestricted = await delegationService.IsUserRestrictedAsync(userId);
                 if (isRestricted)
                 {
                     context.Result = new ContentResult
                     {
                         StatusCode = 403,
                         Content = "Your authority is currently delegated to another user. You cannot perform actions while delegation is active."
                     };
                     return;
                 }
            }
        }

        // ---------------------------------------------------------
        // PERMISSION CHECK
        // ---------------------------------------------------------
        var permissionService = httpContext.RequestServices.GetService<IPermissionService>();
        var userPolicies = await permissionService?.GetUserPermissions(userId); // Use await if async

        if (userPolicies == null || !userPolicies.Intersect(RequiredPolicies).Any())
        {
            context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
        }
    }
}

