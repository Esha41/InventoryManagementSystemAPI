using Ettad.RequestManagement.Service.Orders.Dto;
using Ettad.ResponseHandler.Models;

namespace Ettad.RequestManagement.Service.Orders
{
    public interface IOrderService
    {
        Task<APIOperationResponse<OrderDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<OrderDto>>> GetAllAsync();
        Task<APIOperationResponse<long>> CreateAsync(CreateOrderDto inputDto);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, UpdateOrderDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
    }
}

