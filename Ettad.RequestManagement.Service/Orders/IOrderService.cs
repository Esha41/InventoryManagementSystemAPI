using Ettad.RequestManagement.Service.Orders.Dto;
using Ettad.ResponseHandler.Models;

namespace Ettad.RequestManagement.Service.Orders
{
    public interface IOrderService
    {
        Task<APIOperationResponse<OrderDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<OrderDto>>> GetAllAsync();
        Task<APIOperationResponse<OrderDto>> CreateAsync(CreateUpdateOrderDto inputDto);
        Task<APIOperationResponse<OrderDto>> UpdateAsync(long id, CreateUpdateOrderDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
    }
}

