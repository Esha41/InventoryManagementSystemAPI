namespace Ettad.User.Services.DTO
{
    /// <summary>
    /// Lightweight projection of an active delegation: the delegator and the role they
    /// were logged in with (captured at creation). Used to resolve delegated permissions,
    /// approval authority, and request visibility for the delegatee.
    /// </summary>
    public class ActiveDelegationInfo
    {
        public string DelegatorUserId { get; set; }

        /// <summary>
        /// AspNetRoles.Id captured at delegation creation. May be null for legacy rows.
        /// </summary>
        public string DelegatorRoleId { get; set; }
    }
}
