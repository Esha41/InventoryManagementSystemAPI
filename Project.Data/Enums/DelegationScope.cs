using System;

namespace Ettad.Data.Enums
{
    /// <summary>
    /// Defines the scopes/areas that can be delegated.
    /// Uses Flags attribute to allow multiple scopes per delegation.
    /// Stored as bitwise long in database for efficient querying and future extensibility.
    /// </summary>
    [Flags]
    public enum DelegationScope : long
    {
        /// <summary>
        /// No delegation scope (invalid state)
        /// </summary>
        None = 0,

        /// <summary>
        /// Allows delegatee to approve/reject/return workflow steps on behalf of delegator
        /// </summary>
        WorkflowApproval = 1
    }
}
