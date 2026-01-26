using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Base;
using Ettad.Data.Enums;
using Ettad.Data.Entities.Workflows;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Complete audit trail and history tracking for order items.
    /// Records every modification to order items including additions, quantity changes, deletions,
    /// final approvals, and supply operations.
    /// </summary>
    public class OrderItemHistory : FullAuditEntity<long>
    {
        /// <summary>
        /// The order this history record belongs to
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// The RequestItem that was modified (null for deleted items)
        /// </summary>
        public long? RequestItemId { get; set; }

        /// <summary>
        /// The item being tracked
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// Type of action that occurred
        /// </summary>
        public OrderItemActionType ActionType { get; set; }

        /// <summary>
        /// Date and time when the action occurred
        /// </summary>
        public DateTime ActionDate { get; set; }

        /// <summary>
        /// Order status at time of action
        /// </summary>
        public RequestStatus OrderStatus { get; set; }

        /// <summary>
        /// Previous quantity (for modifications)
        /// </summary>
        public long? PreviousQuantity { get; set; }

        /// <summary>
        /// New quantity after action
        /// </summary>
        public long? NewQuantity { get; set; }

        /// <summary>
        /// Quantity when order was finally approved (snapshot)
        /// </summary>
        public long? ApprovedQuantity { get; set; }

        /// <summary>
        /// Quantity actually supplied
        /// </summary>
        public long? SuppliedQuantity { get; set; }

        /// <summary>
        /// Department of user who made the change
        /// </summary>
        public long DepartmentId { get; set; }

        /// <summary>
        /// User who made the change
        /// </summary>
        public string ModifiedByUserId { get; set; }

        /// <summary>
        /// Name of user who made the change
        /// </summary>
        public string ModifiedByUserName { get; set; }

        /// <summary>
        /// The workflow approval step that was active when modification occurred (for step approvals)
        /// </summary>
        public int? WorkflowApprovalStepId { get; set; }

        /// <summary>
        /// The workflow step definition ID (for reference)
        /// </summary>
        public int? WorkflowStepId { get; set; }

        /// <summary>
        /// Human-readable description of what happened
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Additional notes or details about the action
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Related Supply ID (if action is from supply)
        /// </summary>
        public long? SupplyId { get; set; }

        /// <summary>
        /// Related AssetSupply ID (if action is from asset supply)
        /// </summary>
        public long? AssetSupplyId { get; set; }

        /// <summary>
        /// Related SupplyDetail ID
        /// </summary>
        public long? SupplyDetailId { get; set; }

        /// <summary>
        /// Related AssetSupplyDetail ID
        /// </summary>
        public long? AssetSupplyDetailId { get; set; }

        #region Navigation Properties

        public Order Order { get; set; }
        public RequestItem RequestItem { get; set; }
        public BaseItem Item { get; set; }
        public Department Department { get; set; }
        public ApplicationUser ModifiedByUser { get; set; }
        public WorkflowApprovalStep WorkflowApprovalStep { get; set; }
        public WorkflowStep WorkflowStep { get; set; }
        public Supply Supply { get; set; }
        public AssetSupply AssetSupply { get; set; }
        public SupplyDetail SupplyDetail { get; set; }
        public AssetSupplyDetail AssetSupplyDetail { get; set; }

        #endregion
    }
}
