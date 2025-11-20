namespace Ettad.Data.Enums
{
    /// <summary>
    /// Represents the status of a Supply
    /// </summary>
    public enum SupplyStatus
    {
        /// <summary>
        /// Supply is in draft state, can be modified
        /// </summary>
        Draft = 1,
        
        /// <summary>
        /// Supply is partially fulfilled (some items supplied but not all)
        /// </summary>
        Partial = 2,
        
        /// <summary>
        /// Supply is fully completed and all items have been supplied
        /// </summary>
        Completed = 3,
        
        /// <summary>
        /// Supply has been cancelled
        /// </summary>
        Cancelled = 4
    }
}
