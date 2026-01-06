using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Represents an individual asset included in a supply operation.
    /// Links a specific asset to a supply record.
    /// </summary>
    public class AssetSupplyDetail : FullAuditEntity<long>
    {
        /// <summary>
        /// The parent supply record
        /// </summary>
        public long AssetSupplyId { get; set; }

        /// <summary>
        /// The specific asset being supplied
        /// </summary>
        public long AssetId { get; set; }

        /// <summary>
        /// The item type (for reference/grouping)
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// Sequence/order number within the supply
        /// </summary>
        public int SequenceNo { get; set; }

        /// <summary>
        /// Condition of the asset when supplied
        /// </summary>
        public string? ConditionOnSupply { get; set; }

        /// <summary>
        /// Additional notes for this specific asset
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Whether this asset was actually delivered
        /// </summary>
        public bool IsDelivered { get; set; }

        /// <summary>
        /// Date when this specific asset was delivered
        /// </summary>
        public DateTime? DeliveredDate { get; set; }

        /// <summary>
        /// Custodian (User) responsible for this specific asset (can override supply-level custodian)
        /// </summary>
        public string CustodianId { get; set; }

        #region Navigation Properties

        public AssetSupply AssetSupply { get; set; }
        public Asset Asset { get; set; }
        public BaseItem Item { get; set; }
        public ApplicationUser Custodian { get; set; }

        #endregion
    }
}
