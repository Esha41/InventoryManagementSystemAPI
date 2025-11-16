using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.RequestManagement.Service.SupplyManagement.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.RequestManagement.Service.SupplyManagement
{
    public interface ISupplyService
    {
        Task<APIOperationResponse<OrderSupplySuggestionDto>> GetSupplySuggestionAsync(long orderId);
        Task<APIOperationResponse<long>> CreateSupplyAsync(CreateSupplyDto inputDto);
        Task<APIOperationResponse<SupplyDto>> GetSupplyByOrderIdAsync(long orderId);
    }
}

