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

        /// <summary>
        /// Sliding-window refresh expiry capped at sessionStartedAt + AbsoluteSessionLifetimeMinutes,
        /// so a session cannot outlive its absolute lifetime no matter how often it refreshes.
        /// </summary>
        DateTime CalculateRefreshTokenExpiry(DateTime sessionStartedAt);

        /// <summary>
        /// Issues an access token. <paramref name="startNewSession"/> = true (login/select-role)
        /// rotates the session id, invalidating any prior session's tokens. False (refresh) reuses
        /// the existing session id, so multiple tabs refreshing independently keep one shared session.
        /// </summary>
        Task<AuthenticatedResponse> GenerateJWTokenAsync(string userId, bool startNewSession = true);
        Task<AuthenticatedResponse> GenerateAzureJWTokenAsync(LoginWithAzureInformation information);
        Task<AuthenticatedResponse> RefreshAsync(UserRefreshToken userRefreshToken);

        /// <summary>JWT with claim purpose=RoleSelection; used only for POST Account/select-role.</summary>
        string GenerateRoleSelectionToken(string userId);

        /// <summary>Returns user id if valid role-selection token; otherwise null.</summary>
        string? ValidateRoleSelectionTokenAndGetUserId(string token);
    }

}
