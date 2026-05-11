using System;
using System.Threading.Tasks;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.Assets.Interfaces
{
    /// <summary>Starts and queries async chunked bulk soft-delete of tracked assets.</summary>
    public interface IBulkAssetDeleteService
    {
        Task<APIOperationResponse<BulkDeleteAssetsEnqueueResultDto>> StartBulkDeleteAsync(
            StartBulkDeleteAssetsDto dto);

        Task<APIOperationResponse<BulkDeleteAssetsStatusDto>> GetBulkDeleteStatusAsync(Guid jobId);
    }
}
