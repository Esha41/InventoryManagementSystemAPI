using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Represents a trackable physical asset with a unique identity (serial number).
    /// Assignment information is stored in AssetAssignment entity for proper tracking.
    /// </summary>
    public class Asset : FullAuditEntity<long>
    {
        /// <summary>
        /// The item type this asset belongs to (weapon, ammunition, etc.)
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// Unique serial number for the asset (required for supply/assignment)
        /// </summary>
        public string? SerialNumber { get; set; }

        /// <summary>
        /// RFID tag identifier for tracking
        /// </summary>
        public string? RFID { get; set; }

        /// <summary>
        /// The depot where this asset is stored/registered
        /// </summary>
        public long DepotId { get; set; }

        /// <summary>
        /// Current operational status of the asset
        /// </summary>
        public AssetStatus? Status { get; set; }

        /// <summary>
        /// Date when the asset was purchased
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// Date when warranty expires
        /// </summary>
        public DateTime? WarrantyExpiryDate { get; set; }

        /// <summary>
        /// Original purchase price
        /// </summary>
        public decimal? PurchasePrice { get; set; }

        /// <summary>
        /// Optional delivery receipt reference
        /// </summary>
        public string? DeliveryReceipt { get; set; }

        /// <summary>
        /// General notes about the asset
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Whether this asset is currently assigned to someone
        /// </summary>
        public bool IsAssigned { get; set; }

        /// <summary>
        /// ID of the current active assignment (for quick lookup)
        /// </summary>
        public long? CurrentAssignmentId { get; set; }

        public long BatchId { get; set; }

        public long? SupplierId { get; set; }

        public long? ManufacturerId { get; set; }

        public long? PrimaryPurposId { get; set; }

        #region Navigation Properties

        public BaseItem Item { get; set; }
        public Depot Depot { get; set; }
        public Batch Batch { get; set; }
        public AssetAssignment CurrentAssignment { get; set; }
        public ICollection<AssetAssignment> Assignments { get; set; }
        public ICollection<AssetHistory> History { get; set; }
        public ICollection<AssetSupplyDetail> SupplyDetails { get; set; }

        public Supplier Supplier { get; set; }

        public Manufacturer Manufacturer { get; set; }

        public PrimaryPurpos PrimaryPurpos { get; set; }

        #endregion
    }
}
