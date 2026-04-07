using Ettad.Data.Entities;
using Ettad.Inventory.Service.Batches.Dtos;
using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Comman.Models;
using Microsoft.AspNetCore.Http;

namespace Ettad.Inventory.Service.Batches
{
    public interface IBatchService
    {
        Task<Batch> GetOrCreateAsync(string batchNumber, long depotId, long? primaryPurposId = null);
        Task<APIOperationResponse<BatchDto>> UpdateAsync(long id, UpdateBatchDto dto);
        Task<APIOperationResponse<BatchDto>> GetByIdAsync(long id, bool? serialNumberOnly = null, int? quantity = null, bool? filterByIsAssigned = null);
        Task<APIOperationResponse<BatchDto>> GetByBatchNumberAsync(string batchNumber, bool? serialNumberOnly = null, int? quantity = null, bool? filterByIsAssigned = null);
        Task<APIOperationResponse<List<BatchDto>>> GetAllAsync(long? depotId = null);
        Task<APIOperationResponse<List<BatchSummaryDto>>> GetSummaryAsync(long depotId);
        Task<APIOperationResponse<PaginatedList<BatchDto>>> SearchAsync(long? depotId, PagedListRequest request);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        Task<APIOperationResponse<bool>> BulkUpdateAssetsAsync(long batchId, BulkUpdateBatchAssetsDto inputDto, List<IFormFile>? files = null);
        Task<APIOperationResponse<bool>> UpdateBatchAsync(long id, UpdateBatchDto dto);
        Task<APIOperationResponse<bool>> RemoveAssetFromBatchAsync(long batchId, long assetId);
    }
}
