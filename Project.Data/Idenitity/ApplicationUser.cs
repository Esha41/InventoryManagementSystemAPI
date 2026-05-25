
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ettad.Comman.Idenitity
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsLdapUser { get; set; }
        public bool IsSuperAdmin { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryDate { get; set; }

        /// <summary>
        /// The immediately-previous refresh token kept valid for a short grace window
        /// after rotation. Handles the case where the server rotated the token but the
        /// Set-Cookie response was lost before the client received it.
        /// </summary>
        public string? PreviousRefreshToken { get; set; }
        public DateTime? PreviousRefreshTokenExpiresAt { get; set; }

        public string? CurrentTokenId { get; set; }
        public string ExtraEmployeesView { get; set; } = string.Empty;

        public long? DepartmentId { get; set; }

        // Navigation property
        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }      
        public string? FullNameEN { get; set; }
        public string? FullNameAR { get; set; }
        public long? RankId { get; set; }

        [ForeignKey(nameof(RankId))]
        public Rank Rank { get; set; }
        public string? MilitoryId { get; set; }
        public string? LdapUserName { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsOnboardingCompleted { get; set; } = false;

        /// <summary>Active session role (AspNetRoles.Id). Null when multi-role user has not chosen yet.</summary>
        public string? DefaultRoleId { get; set; }

        [ForeignKey(nameof(DefaultRoleId))]
        public ApplicationRole? DefaultRole { get; set; }

        /// <summary>Matches DB column from migration (FK to HelpCenterTermsConditions).</summary>
        public long? LastAcceptedTermsConditionsId { get; set; }

        [ForeignKey(nameof(LastAcceptedTermsConditionsId))]
        public HelpCenterTermsConditions? LastAcceptedTermsConditions { get; set; }

        // Soft delete properties
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletionDate { get; set; }
        public string? DeletedBy { get; set; }
    }
}
