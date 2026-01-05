namespace Ettad.Data.Enums
{
    /// <summary>
    /// Represents the status of an asset assignment
    /// </summary>
    public enum AssetAssignmentStatus
    {
        /// <summary>
        /// Asset is currently assigned and active
        /// </summary>
        Active = 1,

        /// <summary>
        /// Asset has been returned from assignment
        /// </summary>
        Returned = 2,

        /// <summary>
        /// Assignment was cancelled before delivery
        /// </summary>
        Cancelled = 3,

        /// <summary>
        /// Asset was transferred to another custodian/department
        /// </summary>
        Transferred = 4
    }
}

