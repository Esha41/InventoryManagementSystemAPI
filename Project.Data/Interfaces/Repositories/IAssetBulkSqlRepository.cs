using System.Threading;
using System.Threading.Tasks;
using Ettad.Data.Entities;

namespace Ettad.Data.Interfaces.Repositories
{
    /// <summary>
    /// SQL Server bulk insert for <see cref="Asset"/> rows via <c>SqlBulkCopy</c> (no EF change tracking).
    /// Lives in the repository/infrastructure layer intentionally.
    /// </summary>
    public interface IAssetBulkSqlRepository
    {
        /// <summary>
        /// Inserts <paramref name="totalQuantity"/> asset rows from <paramref name="templateAsset"/> column values.
        /// After the first bulk chunk, returns the smallest <see cref="Asset.Id"/> among non-deleted rows for
        /// <see cref="Asset.BatchId"/> (same legacy contract as before refactor).
        /// </summary>
        Task<long> BulkInsertTemplateAssetsAsync(
            Asset templateAsset,
            int totalQuantity,
            CancellationToken cancellationToken = default);
    }
}
