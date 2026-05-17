using Ettad.RequestManagement.Service.Orders.Dto;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;

namespace Ettad.RequestManagement.Service.Orders
{
    public interface IOrderService
    {
        Task<APIOperationResponse<OrderDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<OrderDto>>> GetAllAsync();

        /// <summary>
        /// Creates an Order with optional entity-only "other" files (legacy bucket).
        /// No attachment-requirement slot uploads are submitted via this overload.
        /// </summary>
        Task<APIOperationResponse<long>> CreateAsync(
            CreateOrderDto inputDto,
            List<IFormFile>? otherFiles = null);

        /// <summary>
        /// Creates an Order with files grouped per AttachmentRequirement plus optional
        /// entity-only "other" files. Centralized validation enforces required/min/max
        /// per requirement, and persists AttachmentRequirementId on each file row.
        /// </summary>
        Task<APIOperationResponse<long>> CreateAsync(
            CreateOrderDto inputDto,
            IReadOnlyDictionary<long, IReadOnlyList<IFormFile>> filesByAttachmentRequirementId,
            List<IFormFile>? otherFiles = null);

        Task<APIOperationResponse<bool>> DeleteAsync(long id);

        // Order Item Management
        Task<APIOperationResponse<long>> AddOrderItemAsync(long orderId, CreateUpdateRequestItemDto itemDto);
        Task<long?> GetOrderItemCurrentQuantityAsync(long orderId, long itemId);
        Task<APIOperationResponse<bool>> UpdateOrderItemQuantityAsync(long orderId, long itemId, long newQuantity);
        Task<APIOperationResponse<bool>> DeleteOrderItemAsync(long orderId, long itemId);
        Task<APIOperationResponse<AllowanceVerificationDto>> VerifyItemAllowanceAsync(long itemId, long requestedQuantity);
        
        // Supply Date Management
        Task<APIOperationResponse<bool>> SetSupplyDateAsync(long orderId, DateTime supplyDate);
    }
}
