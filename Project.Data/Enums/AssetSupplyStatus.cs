namespace Ettad.Data.Enums
{
    /// <summary>
    /// Represents the status of an asset supply operation
    /// </summary>
    public enum AssetSupplyStatus
    {
        /// <summary>
        /// Supply is being prepared (draft state)
        /// </summary>
        Draft = 1,

        /// <summary>
        /// Supply has been submitted and awaiting approval
        /// </summary>
        Submitted = 2,

        /// <summary>
        /// Supply has been approved
        /// </summary>
        Approved = 3,

        /// <summary>
        /// Supply is ready for pickup/delivery
        /// </summary>
        Ready = 4,

        /// <summary>
        /// Supply has been delivered/completed
        /// </summary>
        Completed = 5,

        /// <summary>
        /// Supply was cancelled
        /// </summary>
        Cancelled = 6,

        /// <summary>
        /// Supply is partially fulfilled (some assets returned)
        /// </summary>
        PartiallyReturned = 7
    }
}

