using Ettad.RequestManagement.Service.Orders.Dto;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;

namespace Ettad.RequestManagement.Service.Orders
{
    public interface IOrderService
    {
        Task<APIOperationResponse<OrderDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<OrderDto>>> GetAllAsync();
        Task<APIOperationResponse<long>> CreateAsync(CreateOrderDto inputDto);
        Task<APIOperationResponse<long>> CreateAsync(CreateOrderDto inputDto, List<IFormFile> files);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);

        // Order Item Management
        Task<APIOperationResponse<long>> AddOrderItemAsync(long orderId, CreateUpdateRequestItemDto itemDto);
        Task<APIOperationResponse<bool>> UpdateOrderItemQuantityAsync(long orderId, long itemId, long newQuantity);
        Task<APIOperationResponse<bool>> DeleteOrderItemAsync(long orderId, long itemId);
        Task<APIOperationResponse<AllowanceVerificationDto>> VerifyItemAllowanceAsync(long itemId, long requestedQuantity);
    }
}
