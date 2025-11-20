using Ettad.RequestManagement.Service.Orders.Dto;
using Ettad.ResponseHandler.Models;

namespace Ettad.RequestManagement.Service.Orders
{
    public interface IOrderService
    {
        Task<APIOperationResponse<OrderDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<OrderDto>>> GetAllAsync();
			Task<APIOperationResponse<Ettad.CrossCutting.Comman.Models.PaginatedList<OrderDto>>> GetAsync(
				int? status,
				long? departmentId,
				int page,
				int pageSize,
				string? sortBy,
				string? sortDir);
			Task<APIOperationResponse<Ettad.CrossCutting.Comman.Models.PaginatedList<Ettad.Data.Enums.RequestStatus>>> GetSummariesAsync(
				int? status,
				long? departmentId,
				int page,
				int pageSize,
				string? sortBy,
				string? sortDir);
        Task<APIOperationResponse<long>> CreateAsync(CreateOrderDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        
        // Order Item Management
        Task<APIOperationResponse<long>> AddOrderItemAsync(long orderId, CreateUpdateRequestItemDto itemDto);
        Task<APIOperationResponse<bool>> UpdateOrderItemQuantityAsync(long orderId, long itemId, long newQuantity);
        Task<APIOperationResponse<bool>> DeleteOrderItemAsync(long orderId, long itemId);
    }
}

