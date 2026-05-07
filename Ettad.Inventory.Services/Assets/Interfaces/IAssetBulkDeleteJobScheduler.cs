using System;

namespace Ettad.Inventory.Service.Assets.Interfaces
{
    public interface IAssetBulkDeleteJobScheduler
    {
        string Enqueue(Guid jobId);
    }
}
