using System;
using System.Threading;
using System.Threading.Tasks;
using Ettad.Inventory.Service.Assets.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ettad.Inventory.Service.Assets.Background
{
    /// <summary>Hangfire entry point — keep method signature stable for serialization.</summary>
    public class AssetBulkDeleteHangfireJob
    {
        private readonly IAssetBulkDeletionProcessor _processor;
        private readonly ILogger<AssetBulkDeleteHangfireJob> _logger;

        public AssetBulkDeleteHangfireJob(
            IAssetBulkDeletionProcessor processor,
            ILogger<AssetBulkDeleteHangfireJob> logger)
        {
            _processor = processor;
            _logger = logger;
        }

        public async Task ExecuteAsync(Guid jobId)
        {
            _logger.LogInformation("AssetBulkDeleteHangfireJob started. JobSessionId={JobId}", jobId);
            await _processor.ExecuteAsync(jobId, CancellationToken.None).ConfigureAwait(false);
            _logger.LogInformation("AssetBulkDeleteHangfireJob finished. JobSessionId={JobId}", jobId);
        }
    }
}
