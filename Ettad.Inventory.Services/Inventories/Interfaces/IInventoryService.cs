using Ettad.Data.Enums;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Comman.Models;
using Microsoft.AspNetCore.Http;
using Ettad.Inventory.Service.Common.Services;

namespace Ettad.Inventory.Service.Inventories.Interfaces
{
    public interface IInventoryService
    {
        Task<APIOperationResponse<InventoryDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<InventoryDto>>> GetAllAsync();
        Task<APIOperationResponse<long>> CreateAsync(CreateInventoryDto inputDto, List<IFormFile>? files = null);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, UpdateInventoryDto inputDto, List<IFormFile>? files = null, long? filesItemId = null);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        Task<APIOperationResponse<OrderSupplySuggestionDto>> SuggestSupplyForOrderAsync(long orderId, List<long>? depotIds = null);
        Task<APIOperationResponse<List<LotDetailDto>>> GetLotsByItemIdAsync(long itemId, long? depotId = null);
        Task<APIOperationResponse<List<LotDetailDto>>> GetAvailableLotsForQuantityAsync(long itemId, long requiredQuantity, List<long>? depotIds = null, long? excludeSupplyId = null);
        Task<APIOperationResponse<LotDetailDto>> GetLotByNumberAsync(string lotNumber);
        Task<APIOperationResponse<ItemInventorySummaryDto>> GetItemInventorySummaryAsync(long itemId);
        Task<APIOperationResponse<List<ItemInventorySummaryDto>>> GetInventorySummaryForAllItemsAsync(long? depotId = null, List<long>? depotIds = null);
        Task<APIOperationResponse<PaginatedList<ItemInventorySummaryDto>>> GetInventorySummaryForAllItemsPaginatedAsync(PagedListRequest request, long? depotId = null, List<long>? depotIds = null, ItemType? itemType = null);
        Task<APIOperationResponse<bool>> ToggleReadyForIssueAsync(long inventoryDetailId);
        Task<APIOperationResponse<ImportResult<InventoryImportRowDto>>> ImportAsync(IFormFile file, long depotId, string language = "en");
        Task<APIOperationResponse<ImportResult<InventoryImportRowDto>>> ImportPreviewAsync(IFormFile file, long depotId, string language = "en");
        Task<APIOperationResponse<PaginatedList<InventoryDetailDto>>> GetInventoryDetailsByDepotIdPaginatedAsync(long depotId, PagedListRequest request);
        Task<APIOperationResponse<byte[]>> ExportToExcelAsync(ItemType? itemType = null);
        Task<APIOperationResponse<byte[]>> GenerateImportTemplateAsync(long depotId, string language = "en");
    }
}
