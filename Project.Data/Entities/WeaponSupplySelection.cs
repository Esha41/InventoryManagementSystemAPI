using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Stores depot and batch selections for weapon supply per order (request).
    /// Table: Id, OrderId (request id), DepotId, BatchId, AuditEntity (CreationDate, CreatedBy, ModificationDate, ModifiedBy).
    /// </summary>
    public class WeaponSupplySelection : FullAuditEntity<long>
    {
        /// <summary>
        /// The order (request) this selection belongs to
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Selected depot
        /// </summary>
        public long DepotId { get; set; }

        /// <summary>
        /// Selected batch within the depot (null when depot is selected without a specific batch)
        /// </summary>
        public long? BatchId { get; set; }

        #region Navigation Properties

        public Order Order { get; set; }
        public Depot Depot { get; set; }
        public Batch Batch { get; set; }

        #endregion
    }
}
