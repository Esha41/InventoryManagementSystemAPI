using Ettad.Data.Entities;
using Ettad.Inventory.Service.Batches.Dtos;
using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Comman.Models;
using Microsoft.AspNetCore.Http;
using Ettad.Inventory.Service.Common.Interfaces;

namespace Ettad.Inventory.Service.Batches.Interfaces
{
    public interface IBatchService
    {
        Task<Batch> GetOrCreateAsync(string batchNumber, long depotId);
        Task<APIOperationResponse<BatchDto>> GetByIdAsync(long id, bool? serialNumberOnly = null, bool? filterByIsAssigned = null, int assetsPage = 1, int assetsPageSize = 50, bool includeAllAssets = false, BatchAssetFilterDto? filters = null);
        Task<APIOperationResponse<List<BatchDto>>> GetByBatchNumberAsync(string batchNumber, long? depotId = null, bool? serialNumberOnly = null, bool? filterByIsAssigned = null, int assetsPage = 1, int assetsPageSize = 50, bool includeAllAssets = false);
        Task<APIOperationResponse<List<BatchDto>>> GetAllAsync(long? depotId = null);
        Task<APIOperationResponse<List<BatchSummaryDto>>> GetSummaryAsync(long depotId, BatchAssetFilterDto? filters = null);
        Task<APIOperationResponse<PaginatedList<BatchDto>>> SearchAsync(long? depotId, PagedListRequest request);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        Task<APIOperationResponse<bool>> BulkUpdateAssetsAsync(long batchId, BulkUpdateBatchAssetsDto inputDto, List<IFormFile>? files = null);
        Task<APIOperationResponse<bool>> UpdateBatchAsync(long id, UpdateBatchDto dto);
        Task<APIOperationResponse<bool>> RemoveAssetFromBatchAsync(long batchId, long assetId);

        Task<APIOperationResponse<byte[]>> ExportBatchAssetsExcelAsync(long batchId, string language = "en");

        Task<APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>> ImportBatchAssetsPreviewAsync(long batchId, IFormFile file, string language = "en");

        Task<APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>> ImportBatchAssetsAsync(long batchId, IFormFile file, string language = "en");
    }
}
