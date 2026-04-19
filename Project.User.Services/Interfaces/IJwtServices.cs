using Ettad.User.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.Interfaces
{
    public interface IJwtServices
    {
        string GenerateRefreshToken();
        Task<AuthenticatedResponse> GenerateJWTokenAsync(string userId);
        Task<AuthenticatedResponse> GenerateAzureJWTokenAsync(LoginWithAzureInformation information);
        Task<AuthenticatedResponse> RefreshAsync(UserRefreshToken userRefreshToken);

        /// <summary>JWT with claim purpose=RoleSelection; used only for POST Account/select-role.</summary>
        string GenerateRoleSelectionToken(string userId);

        /// <summary>Returns user id if valid role-selection token; otherwise null.</summary>
        string? ValidateRoleSelectionTokenAndGetUserId(string token);
    }

}
