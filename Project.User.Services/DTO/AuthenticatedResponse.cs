using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.DTO
{
    public class AuthenticatedResponse
    {
        public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; init; }

        /// <summary>True when the user must pick a role before receiving a full session (multi-role, no default).</summary>
        public bool RequiresRoleSelection { get; init; }

        public IReadOnlyList<RoleForSelectionDto> AvailableRoles { get; init; } = Array.Empty<RoleForSelectionDto>();

        /// <summary>Short-lived JWT used only for POST select-role.</summary>
        public string? RoleSelectionToken { get; init; }
    }

    public class RoleForSelectionDto
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string? NameAr { get; init; }
    }

    public class SelectRoleDto
    {
        public string RoleId { get; init; } = string.Empty;

        /// <summary>Required for first login after credentials; omit when switching role with an active session token.</summary>
        public string? RoleSelectionToken { get; init; }
    }
}
