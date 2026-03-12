using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Stores per-item batch selections for weapon supply per order.
    /// Each row = one (Order, Depot, Batch, Item, Quantity) combination.
    /// </summary>
    public class WeaponSupplySelection : AuditEntity<long>
    {
        public long OrderId { get; set; }

        public long DepotId { get; set; }

        public long BatchId { get; set; }

        public long ItemId { get; set; }

        public int Quantity { get; set; }

        #region Navigation Properties

        public Order Order { get; set; }
        public Depot Depot { get; set; }
        public Batch Batch { get; set; }
        public BaseItem Item { get; set; }

        #endregion
    }
}
