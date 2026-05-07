using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ettad.Inventory.Service.Assets.Interfaces
{
    /// <summary>Runs Hangfire-triggered chunked soft-delete for AssetBulkDeletionJob rows.</summary>
    public interface IAssetBulkDeletionProcessor
    {
        Task ExecuteAsync(Guid jobId, CancellationToken cancellationToken = default);
    }
}
