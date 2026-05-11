using System;
using Ettad.Inventory.Service.Assets.Interfaces;
using Hangfire;

namespace Ettad.Inventory.Service.Assets.Background
{
    public class HangfireAssetBulkDeleteJobScheduler : IAssetBulkDeleteJobScheduler
    {
        public string Enqueue(Guid jobId)
        {
            return BackgroundJob.Enqueue<AssetBulkDeleteHangfireJob>(j => j.ExecuteAsync(jobId));
        }
    }
}
