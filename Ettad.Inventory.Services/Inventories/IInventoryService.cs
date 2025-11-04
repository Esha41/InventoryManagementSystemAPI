using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.Inventories
{
    public interface IInventoryService
    {
        Task<APIOperationResponse<InventoryDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<InventoryDto>>> GetAllAsync();
        Task<APIOperationResponse<InventoryDto>> CreateAsync(CreateInventoryDto inputDto);
        Task<APIOperationResponse<InventoryDto>> UpdateAsync(long id, UpdateInventoryDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
    }
}

