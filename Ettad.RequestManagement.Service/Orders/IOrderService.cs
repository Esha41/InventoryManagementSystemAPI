using Ettad.RequestManagement.Service.Orders.Dto;
using Ettad.ResponseHandler.Models;

namespace Ettad.RequestManagement.Service.Orders
{
    public interface IOrderService
    {
        Task<APIOperationResponse<OrderDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<OrderDto>>> GetAllAsync();
        Task<APIOperationResponse<long>> CreateAsync(CreateOrderDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        
        // Order Item Management
        Task<APIOperationResponse<long>> AddOrderItemAsync(long orderId, CreateUpdateRequestItemDto itemDto);
        Task<APIOperationResponse<bool>> UpdateOrderItemQuantityAsync(long orderId, long itemId, long newQuantity);
        Task<APIOperationResponse<bool>> DeleteOrderItemAsync(long orderId, long itemId);
    }
}

