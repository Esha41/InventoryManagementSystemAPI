namespace Ettad.Data.Enums
{
    /// <summary>
    /// Represents all possible actions that can be tracked for order items
    /// </summary>
    public enum OrderItemActionType
    {
        /// <summary>
        /// New item added to order
        /// </summary>
        Added = 1,

        /// <summary>
        /// Quantity changed (can happen during workflow)
        /// </summary>
        QuantityModified = 2,

        /// <summary>
        /// Item removed from order
        /// </summary>
        Deleted = 3,

        /// <summary>
        /// Final order approval - snapshot of final approved quantities (status = Approved)
        /// </summary>
        FinalApproved = 4,

        /// <summary>
        /// Item supplied (from Supply)
        /// </summary>
        Supplied = 5,

        /// <summary>
        /// Asset supplied (from AssetSupply)
        /// </summary>
        AssetSupplied = 6
    }
}
