using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Enums;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Complete audit trail and history tracking for assets.
    /// Records every significant action/event in the asset lifecycle.
    /// </summary>
    public class AssetHistory : FullAuditEntity<long>
    {
        /// <summary>
        /// The asset this history record belongs to
        /// </summary>
        public long AssetId { get; set; }

        /// <summary>
        /// Type of action that occurred
        /// </summary>
        public AssetHistoryActionType ActionType { get; set; }

        /// <summary>
        /// Date and time when the action occurred
        /// </summary>
        public DateTime ActionDate { get; set; }

        /// <summary>
        /// Description of what happened
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Previous status before the action (if status changed)
        /// </summary>
        public AssetStatus? PreviousStatus { get; set; }

        /// <summary>
        /// New status after the action (if status changed)
        /// </summary>
        public AssetStatus? NewStatus { get; set; }

        /// <summary>
        /// Previous department (for transfers)
        /// </summary>
        public long? PreviousDepartmentId { get; set; }

        /// <summary>
        /// New department (for assignments/transfers)
        /// </summary>
        public long? NewDepartmentId { get; set; }

        /// <summary>
        /// Previous custodian (for transfers)
        /// </summary>
        public string? PreviousCustodianId { get; set; }

        /// <summary>
        /// New custodian (for assignments/transfers)
        /// </summary>
        public string? NewCustodianId { get; set; }

        /// <summary>
        /// Previous location (for location changes)
        /// </summary>
        public string? PreviousLocation { get; set; }

        /// <summary>
        /// New location (for location changes)
        /// </summary>
        public string? NewLocation { get; set; }

        /// <summary>
        /// Related order ID (if action was triggered by an order)
        /// </summary>
        public long? OrderId { get; set; }

        /// <summary>
        /// Related asset supply ID (if action was part of a supply)
        /// </summary>
        public long? AssetSupplyId { get; set; }

        /// <summary>
        /// Related assignment ID (if action involves an assignment)
        /// </summary>
        public long? AssetAssignmentId { get; set; }

        /// <summary>
        /// ID of the user who performed the action
        /// </summary>
        public string? PerformedByUserId { get; set; }

        /// <summary>
        /// Name of the user who performed the action
        /// </summary>
        public string? PerformedByUserName { get; set; }

        /// <summary>
        /// Additional notes or details about the action
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// JSON metadata for storing additional context
        /// </summary>
        public string? Metadata { get; set; }

        #region Navigation Properties

        public Asset Asset { get; set; }
        public Order Order { get; set; }
        public AssetSupply AssetSupply { get; set; }
        public AssetAssignment AssetAssignment { get; set; }
        public Department PreviousDepartment { get; set; }
        public Department NewDepartment { get; set; }
        public ApplicationUser PreviousCustodian { get; set; }
        public ApplicationUser NewCustodian { get; set; }

        #endregion
    }
}

