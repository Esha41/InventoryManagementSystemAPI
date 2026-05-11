using Ettad.Data.Entities;
using Ettad.Data.Interfaces.Repositories;
using Ettad.EntityFramework.DataBaseContext;
using Microsoft.EntityFrameworkCore;

namespace Ettad.Repository.Repositories
{
    public sealed class AssetBulkDeletionPersistence : IAssetBulkDeletionPersistence
    {
        private readonly ApplicationDbContext _context;

        public AssetBulkDeletionPersistence(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Asset> Assets => _context.Assets;

        public void SetBulkCommandTimeout(TimeSpan timeout)
        {
            _context.Database.SetCommandTimeout(timeout);
        }

        public Task<AssetBulkDeletionJob?> GetTrackedJobAsync(Guid jobId, CancellationToken cancellationToken = default)
        {
            return _context.AssetBulkDeletionJobs.AsTracking()
                .FirstOrDefaultAsync(j => j.Id == jobId, cancellationToken);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
