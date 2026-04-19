using Microsoft.AspNetCore.Http;
using Ettad.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.Implementation
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string? UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        public long? DepartmentId
        {
            get
            {
                var departmentIdClaim = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(item => item.Type == "DepartmentId")?.Value;
                if (string.IsNullOrEmpty(departmentIdClaim))
                    return null;
                
                if (long.TryParse(departmentIdClaim, out long departmentId))
                    return departmentId;
                
                return null;
            }
        }
        public bool IsAdminRole => Convert.ToBoolean(_httpContextAccessor
            .HttpContext?
            .User?
            .Claims
            .FirstOrDefault(item => item.Type == "isAdminRole")?.Value);
        public bool IsHrRole => Convert.ToBoolean(_httpContextAccessor
            .HttpContext?
            .User?
            .Claims
            .FirstOrDefault(item => item.Type == "isHrRole")?.Value);
        public bool IsReportingManager => Convert.ToBoolean(_httpContextAccessor
            .HttpContext?
            .User?
            .Claims
            .FirstOrDefault(item => item.Type == "isReportingManager")?.Value);
        public bool IsHrEmployee => Convert.ToBoolean(_httpContextAccessor
            .HttpContext?
            .User?
            .Claims
            .FirstOrDefault(item => item.Type == "isHrEmployee")?.Value);
        public bool IsTrainingEmployee => Convert.ToBoolean(_httpContextAccessor
            .HttpContext?
            .User?
            .Claims
            .FirstOrDefault(item => item.Type == "isTrainingEmployee")?.Value);
        public bool IsResponsibleEmployee => Convert.ToBoolean(_httpContextAccessor
            .HttpContext?
            .User?
            .Claims
            .FirstOrDefault(item => item.Type == "isResponsibleEmployee")?.Value);
        public bool IncludeEmployeeViewInManagerDashboard => Convert.ToBoolean(_httpContextAccessor
            .HttpContext?
            .User?
            .Claims
            .FirstOrDefault(item => item.Value == "IncludeEmployeeViewInManagerDashboard")?.Value != null);
   
        public List<string>? Roles => _httpContextAccessor
            .HttpContext?
            .User?
            .Claims
            .Where(item => item.Type == ClaimTypes.Role)
            .Select(item => item.Value)
            .ToList();
        public List<long> EntityIds => _httpContextAccessor
            .HttpContext?
            .User?
            .Claims
            .FirstOrDefault(item => item.Type == "Permissions-EnityList")?
            .Value?
            .Split(',')
            .Select(long.Parse)
            .OrderBy(item => item)
            .ToList() ?? new List<long>();
        public List<long> ExtraEmployeesView => _httpContextAccessor
            .HttpContext?
            .User?
            .Claims
            .FirstOrDefault(item => item.Type == "extraEmployeesView")?
            .Value?
            .Split(',')
            .Select(long.Parse)
            .OrderBy(item => item)
            .ToList() ?? new List<long>();
        public List<long> ExtraRoleEmployeesView => _httpContextAccessor
            .HttpContext?
            .User?
            .Claims.FirstOrDefault(item => item.Type == "Permissions-ExtraEmployeesView")?
            .Value?
            .Split(',')
            .Select(long.Parse)
            .OrderBy(item => item)
            .ToList() ?? new List<long>();
        public List<long> AllowedLeaveClausesIds => _httpContextAccessor
            .HttpContext?
            .User?
            .Claims.FirstOrDefault(item => item.Type == "Permissions-AllowedLeaveClauses")?
            .Value?
            .Split(',')
            .Select(long.Parse)
            .ToList() ?? new List<long>();
        public List<long> AllowedPermissionClausesIds => _httpContextAccessor
            .HttpContext?
            .User?
            .Claims.FirstOrDefault(item => item.Type == "Permissions-AllowedPermissionClauses")?
            .Value?
            .Split(',')
            .Select(long.Parse)
            .ToList() ?? new List<long>();

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsUserHasClaim(string claimName)
        {
            // Check both Type and Value to support different claim formats
            // Plain permissions are typically stored as Type
            return _httpContextAccessor.HttpContext?.User?.Claims.Any(item => 
                item.Type == claimName || 
                item.Value == claimName ||
                item.Type.EndsWith(claimName, StringComparison.OrdinalIgnoreCase)) ?? false;
        }
        public int? OrganizationId => Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(item => item.Type == "OrgId")?.Value);

        public bool IsSuperAdmin => Convert.ToBoolean(_httpContextAccessor
         .HttpContext?
         .User?
         .Claims
         .FirstOrDefault(item => item.Type == "IsSuperAdmin")?.Value);

        public string? ActiveRoleId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("ActiveRoleId");
    }
}
