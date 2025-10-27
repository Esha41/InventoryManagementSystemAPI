using Serilog.Core;
using Serilog.Events;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Ettad.CrossCutting.Comman.Monitoring
{
    /// <summary>
    /// Custom Serilog enricher to add user and organization context to logs
    /// </summary>
    public class UserEnricher : ILogEventEnricher
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserEnricher(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User?.Identity?.IsAuthenticated == true)
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = httpContext.User.Identity.Name;
                var organizationId = httpContext.User.FindFirst("OrgId")?.Value;
                var employeeId = httpContext.User.FindFirst("employeeId")?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("UserId", userId));
                }

                if (!string.IsNullOrEmpty(userName))
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("User", userName));
                }

                if (!string.IsNullOrEmpty(organizationId))
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("OrganizationId", organizationId));
                }

                if (!string.IsNullOrEmpty(employeeId))
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("EmployeeId", employeeId));
                }

                // Add request path if available
                var requestPath = httpContext.Request?.Path.Value;
                if (!string.IsNullOrEmpty(requestPath))
                {
                    logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("RequestPath", requestPath));
                }
            }
        }
    }
}

