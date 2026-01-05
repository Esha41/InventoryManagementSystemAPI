using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Represents a supply operation for assets based on an order.
    /// Manages the process of supplying and assigning assets to departments/custodians.
    /// </summary>
    public class AssetSupply : FullAuditEntity<long>
    {
        /// <summary>
        /// The order that this supply fulfills
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// Date when the supply was created/initiated
        /// </summary>
        public DateTime? SupplyDate { get; set; }

        /// <summary>
        /// Current status of the supply
        /// </summary>
        public AssetSupplyStatus Status { get; set; }

        /// <summary>
        /// Department receiving the assets (from order if not specified)
        /// </summary>
        public long? DepartmentId { get; set; }

        /// <summary>
        /// Custodian receiving the assets (optional)
        /// </summary>
        public long? CustodianId { get; set; }

        /// <summary>
        /// Name of the person receiving the supply
        /// </summary>
        public string? ReceiverName { get; set; }

        /// <summary>
        /// Military ID of the receiver
        /// </summary>
        public string? ReceiverMilitaryId { get; set; }

        /// <summary>
        /// Rank of the receiver
        /// </summary>
        public long? ReceiverRankId { get; set; }

        /// <summary>
        /// Location where assets will be assigned
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// Expected return date for all assets in this supply
        /// </summary>
        public DateTime? ExpectedReturnDate { get; set; }

        /// <summary>
        /// Additional notes about the supply
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Date when supply was approved
        /// </summary>
        public DateTime? ApprovedDate { get; set; }

        /// <summary>
        /// User who approved the supply
        /// </summary>
        public string? ApprovedByUserId { get; set; }

        /// <summary>
        /// Date when supply was completed/delivered
        /// </summary>
        public DateTime? CompletedDate { get; set; }

        #region Navigation Properties

        public Order Order { get; set; }
        public Department Department { get; set; }
        public Employee Custodian { get; set; }
        public Rank ReceiverRank { get; set; }
        public ICollection<AssetSupplyDetail> SupplyDetails { get; set; }
        public ICollection<AssetAssignment> Assignments { get; set; }

        #endregion
    }
}

