using Ettad.Data.Enums;
using Ettad.ResponseHandler.Models;

namespace Ettad.Application.Common.Interfaces
{
    public interface IOrderItemTrackingService
    {
        /// <summary>
        /// Record a history entry for order item modifications
        /// </summary>
        Task RecordHistoryAsync(OrderItemHistoryContext context);

        /// <summary>
        /// Record final approval history when order status becomes Approved
        /// </summary>
        Task RecordFinalApprovalHistoryAsync(long orderId, int workflowApprovalStepId);

        /// <summary>
        /// Get all history for an order (by orderId or RequestNo)
        /// </summary>
        Task<APIOperationResponse<List<OrderItemHistoryDto>>> GetOrderItemHistoryAsync(long? orderId = null, string? requestNo = null);

        /// <summary>
        /// Get history for specific item in order
        /// </summary>
        Task<APIOperationResponse<List<OrderItemHistoryDto>>> GetItemHistoryAsync(long orderId, long itemId);

        /// <summary>
        /// Get final approved quantities snapshot (by orderId or RequestNo)
        /// </summary>
        Task<APIOperationResponse<List<OrderItemHistoryDto>>> GetApprovedQuantitiesAsync(long? orderId = null, string? requestNo = null);
    }

    /// <summary>
    /// Context object for recording order item history
    /// </summary>
    public class OrderItemHistoryContext
    {
        public long OrderId { get; set; }
        public long? RequestItemId { get; set; }
        public long ItemId { get; set; }
        public OrderItemActionType ActionType { get; set; }
        public RequestStatus OrderStatus { get; set; }
        public long? PreviousQuantity { get; set; }
        public long? NewQuantity { get; set; }
        public long? ApprovedQuantity { get; set; }
        public long? SuppliedQuantity { get; set; }
        public long DepartmentId { get; set; }
        public string ModifiedByUserId { get; set; }
        public string ModifiedByUserName { get; set; }
        public int? WorkflowApprovalStepId { get; set; }
        public int? WorkflowStepId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public long? SupplyId { get; set; }
        public long? AssetSupplyId { get; set; }
        public long? SupplyDetailId { get; set; }
        public long? AssetSupplyDetailId { get; set; }
    }

    /// <summary>
    /// DTO for order item history records
    /// </summary>
    public class OrderItemHistoryDto
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public string? OrderRequestNo { get; set; }
        public long? RequestItemId { get; set; }
        public long ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? ItemNo { get; set; }
        public OrderItemActionType ActionType { get; set; }
        public DateTime ActionDate { get; set; }
        public RequestStatus OrderStatus { get; set; }

        public long? PreviousQuantity { get; set; }
        public long? NewQuantity { get; set; }
        public long? ApprovedQuantity { get; set; }
        public long? SuppliedQuantity { get; set; }

        public long DepartmentId { get; set; }
        public string DepartmentNameAr { get; set; }
        public string DepartmentNameEn { get; set; }

        public string ModifiedByUserId { get; set; }
        public string ModifiedByUserName { get; set; }
        public string ModifiedByUserNameEn { get; set; }
        public string ModifiedByUserNameAr { get; set; }

        public int? WorkflowApprovalStepId { get; set; }
        public int? WorkflowStepId { get; set; }
        public string? WorkflowStepName { get; set; }

        public string Description { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string? Notes { get; set; }

        public long? SupplyId { get; set; }
        public long? AssetSupplyId { get; set; }
        public long? SupplyDetailId { get; set; }
        public long? AssetSupplyDetailId { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
