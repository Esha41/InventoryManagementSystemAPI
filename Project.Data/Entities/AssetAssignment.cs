using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Represents an asset assignment to a custodian or department.
    /// Tracks the complete assignment lifecycle including dates and status.
    /// </summary>
    public class AssetAssignment : FullAuditEntity<long>
    {
        /// <summary>
        /// The asset being assigned
        /// </summary>
        public long AssetId { get; set; }

        /// <summary>
        /// The order that triggered this assignment (if any)
        /// </summary>
        public long? OrderId { get; set; }

        /// <summary>
        /// The asset supply record that created this assignment (if any)
        /// </summary>
        public long? AssetSupplyId { get; set; }

        /// <summary>
        /// The department the asset is assigned to
        /// </summary>
        public long? DepartmentId { get; set; }

        /// <summary>
        /// The custodian (user) responsible for the asset
        /// </summary>
        public string CustodianId { get; set; }

        /// <summary>
        /// Physical location of the asset during this assignment
        /// </summary>
        public string? Location { get; set; }

        /// <summary>
        /// Date and time when the asset was assigned
        /// </summary>
        public DateTime AssignDate { get; set; }

        /// <summary>
        /// Expected return date (if applicable)
        /// </summary>
        public DateTime? ExpectedReturnDate { get; set; }

        /// <summary>
        /// Actual return date when the asset was returned
        /// </summary>
        public DateTime? ActualReturnDate { get; set; }

        /// <summary>
        /// Current status of this assignment
        /// </summary>
        public AssetAssignmentStatus Status { get; set; }

        /// <summary>
        /// Purpose or reason for this assignment
        /// </summary>
        public string? Purpose { get; set; }

        /// <summary>
        /// Additional notes about the assignment
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Condition of asset when assigned
        /// </summary>
        public string? ConditionOnAssign { get; set; }

        /// <summary>
        /// Condition of asset when returned
        /// </summary>
        public string? ConditionOnReturn { get; set; }

        /// <summary>
        /// Name of the receiver when assignment was made
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

        #region Navigation Properties

        public Asset Asset { get; set; }
        public Order Order { get; set; }
        public AssetSupply AssetSupply { get; set; }
        public Department Department { get; set; }
        public ApplicationUser Custodian { get; set; }
        public Rank ReceiverRank { get; set; }

        #endregion
    }
}

