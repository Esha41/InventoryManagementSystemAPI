using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ettad.Data.Entities
{
    public class UserDelegation : FullAuditEntity<int>
    {
        [Required]
        public string DelegatorUserId { get; set; }

        [ForeignKey("DelegatorUserId")]
        public virtual ApplicationUser DelegatorUser { get; set; }

        [Required]
        public string DelegateeUserId { get; set; }

        [ForeignKey("DelegateeUserId")]
        public virtual ApplicationUser DelegateeUser { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string Reason { get; set; }

        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Status of delegation: 0 = Pending, 1 = Approved, 2 = Rejected
        /// </summary>
        public int DelegationStatus { get; set; } = 0; // 0 = Pending by default

        /// <summary>
        /// Legacy bitwise scope column. Scope-based delegation has been removed; this column is
        /// retained for backward compatibility and is written as 0 for new delegations.
        /// </summary>
        public long DelegationScopes { get; set; }

        /// <summary>
        /// AspNetRoles.Id of the role the delegator was logged in with (active role) when
        /// the delegation was created. The delegatee inherits exactly this role's permissions,
        /// approval authority, and visibility while the delegation is active. Nullable to keep
        /// legacy rows (created before this column) valid.
        /// </summary>
        public string DelegatorRoleId { get; set; }
    }
}
