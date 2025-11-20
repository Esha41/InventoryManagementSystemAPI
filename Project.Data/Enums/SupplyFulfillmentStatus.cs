namespace Ettad.Data.Enums
{
    /// <summary>
    /// Represents the fulfillment status of a Supply (how much of the order is fulfilled)
    /// </summary>
    public enum SupplyFulfillmentStatus
    {
        /// <summary>
        /// Supply is partially fulfilled (some items supplied but not all)
        /// </summary>
        Partial = 1,
        
        /// <summary>
        /// Supply is fully fulfilled (all requested items have been supplied)
        /// </summary>
        Fully = 2
    }
}

