using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.Inventory.Services.Common;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;

namespace Ettad.Inventory.Service.Inventories
{
    public interface IInventoryService
    {
        Task<APIOperationResponse<InventoryDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<InventoryDto>>> GetAllAsync();
        Task<APIOperationResponse<long>> CreateAsync(CreateInventoryDto inputDto);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, UpdateInventoryDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        Task<APIOperationResponse<OrderSupplySuggestionDto>> SuggestSupplyForOrderAsync(long orderId, List<long>? depotIds = null);
        Task<APIOperationResponse<List<LotDetailDto>>> GetLotsByItemIdAsync(long itemId);
        Task<APIOperationResponse<List<LotDetailDto>>> GetAvailableLotsForQuantityAsync(long itemId, long requiredQuantity, List<long>? depotIds = null);
        Task<APIOperationResponse<LotDetailDto>> GetLotByNumberAsync(int lotNumber);
        Task<APIOperationResponse<ItemInventorySummaryDto>> GetItemInventorySummaryAsync(long itemId);
        Task<APIOperationResponse<List<ItemInventorySummaryDto>>> GetInventorySummaryForAllItemsAsync();
        Task<APIOperationResponse<bool>> ToggleReadyForIssueAsync(long inventoryDetailId);
        Task<APIOperationResponse<ImportResult<InventoryImportRowDto>>> ImportAsync(IFormFile file, long depotId);
    }
}
