namespace Ettad.Data.Enums
{
    /// <summary>
    /// Represents all possible actions that can be tracked in asset history
    /// </summary>
    public enum AssetHistoryActionType
    {
        /// <summary>
        /// Asset was created/registered in the system
        /// </summary>
        Created = 1,

        /// <summary>
        /// Asset was assigned to a custodian or department
        /// </summary>
        Assigned = 2,

        /// <summary>
        /// Asset was returned from assignment
        /// </summary>
        Returned = 3,

        /// <summary>
        /// Asset was transferred to another custodian/department
        /// </summary>
        Transferred = 4,

        /// <summary>
        /// Asset status was changed (e.g., Active -> Maintenance)
        /// </summary>
        StatusChanged = 5,

        /// <summary>
        /// Asset was supplied as part of an order
        /// </summary>
        Supplied = 6,

        /// <summary>
        /// Asset location was changed
        /// </summary>
        LocationChanged = 7,

        /// <summary>
        /// Asset was sent for maintenance
        /// </summary>
        MaintenanceStarted = 8,

        /// <summary>
        /// Asset maintenance was completed
        /// </summary>
        MaintenanceCompleted = 9,

        /// <summary>
        /// Asset was disposed/discarded
        /// </summary>
        Disposed = 10,

        /// <summary>
        /// Asset was marked as lost
        /// </summary>
        Lost = 11,

        /// <summary>
        /// Asset was marked as damaged
        /// </summary>
        Damaged = 12,

        /// <summary>
        /// Asset information was updated
        /// </summary>
        Updated = 13,

        /// <summary>
        /// Asset was received from supply
        /// </summary>
        Received = 14
    }
}

