using System;

namespace Ettad.Data.Enums
{
    /// <summary>
    /// Defines the scopes/areas that can be delegated.
    /// Uses Flags attribute to allow multiple scopes per delegation.
    /// Stored as bitwise long in database for efficient querying and future extensibility.
    /// 
    /// IMPORTANT: When adding new enum values, they MUST follow powers of 2:
    /// - WorkflowApproval = 1 (2^0)
    /// - Next value = 2 (2^1)
    /// - Next value = 4 (2^2)
    /// - Next value = 8 (2^3)
    /// - Next value = 16 (2^4)
    /// - Next value = 32 (2^5)
    /// - And so on: 64, 128, 256, 512, 1024, etc.
    /// 
    /// This ensures each scope has a unique bit position for bitwise operations.
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
