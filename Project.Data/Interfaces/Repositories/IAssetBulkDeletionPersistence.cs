using System.Linq;
using Ettad.Data.Entities;

namespace Ettad.Data.Interfaces.Repositories
{
    /// <summary>
    /// Hangfire bulk soft-delete access to <see cref="Asset"/> and <see cref="AssetBulkDeletionJob"/> without coupling the inventory layer to EF <c>DbContext</c>.
    /// </summary>
    public interface IAssetBulkDeletionPersistence
    {
        void SetBulkCommandTimeout(TimeSpan timeout);

        Task<AssetBulkDeletionJob?> GetTrackedJobAsync(Guid jobId, CancellationToken cancellationToken = default);

        IQueryable<Asset> Assets { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
