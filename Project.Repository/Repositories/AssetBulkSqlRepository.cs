using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.EntityFramework.DataBaseContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Ettad.Repository.Repositories
{
    /// <summary>
    /// Encapsulates SqlBulkCopy + DataTable materialization for template-based asset creation.
    /// </summary>
    public class AssetBulkSqlRepository : IAssetBulkSqlRepository
    {
        /// <summary>Aligned with previous <see cref="AssetService"/> bulk-template implementation.</summary>
        private const int ChunkSize = 50_000;

        private readonly ApplicationDbContext _context;
        private readonly ILogger<AssetBulkSqlRepository> _logger;

        public AssetBulkSqlRepository(ApplicationDbContext context, ILogger<AssetBulkSqlRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<long> BulkInsertTemplateAssetsAsync(
            Asset templateAsset,
            int totalQuantity,
            CancellationToken cancellationToken = default)
        {
            long firstAssetId = 0;
            var batchId = templateAsset.BatchId;

            var dbConnection = _context.Database.GetDbConnection();
            var shouldCloseConnection = dbConnection.State != ConnectionState.Open;
            if (shouldCloseConnection)
                await _context.Database.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                if (dbConnection is not SqlConnection sqlConnection)
                    throw new InvalidOperationException("Bulk template insert requires a SQL Server connection.");

                var sqlTransaction = _context.Database.CurrentTransaction?.GetDbTransaction() as SqlTransaction;

                for (var i = 0; i < totalQuantity; i += ChunkSize)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var take = System.Math.Min(ChunkSize, totalQuantity - i);
                    var dataTable = BuildAssetDataTable(templateAsset, take);

                    using (var bulkCopy = new SqlBulkCopy(
                               sqlConnection,
                               SqlBulkCopyOptions.CheckConstraints | SqlBulkCopyOptions.TableLock,
                               sqlTransaction))
                    {
                        bulkCopy.DestinationTableName = "dbo.Assets";
                        bulkCopy.BatchSize = 10_000;
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.EnableStreaming = true;
                        bulkCopy.ColumnMappings.Add("ItemId", "ItemId");
                        bulkCopy.ColumnMappings.Add("SupplierId", "SupplierId");
                        bulkCopy.ColumnMappings.Add("ManufacturerId", "ManufacturerId");
                        bulkCopy.ColumnMappings.Add("PrimaryPurposId", "PrimaryPurposId");
                        bulkCopy.ColumnMappings.Add("DepotId", "DepotId");
                        bulkCopy.ColumnMappings.Add("BatchId", "BatchId");
                        bulkCopy.ColumnMappings.Add("CreationDate", "CreationDate");
                        bulkCopy.ColumnMappings.Add("CreatedBy", "CreatedBy");
                        bulkCopy.ColumnMappings.Add("Status", "Status");
                        bulkCopy.ColumnMappings.Add("IsAssigned", "IsAssigned");
                        bulkCopy.ColumnMappings.Add("PurchasePrice", "PurchasePrice");
                        bulkCopy.ColumnMappings.Add("IsDeleted", "IsDeleted");

                        await bulkCopy.WriteToServerAsync(dataTable, cancellationToken).ConfigureAwait(false);
                    }

                    if (i == 0)
                    {
                        firstAssetId = await _context.Set<Asset>()
                            .AsNoTracking()
                            .Where(a => a.BatchId == batchId && !a.IsDeleted)
                            .OrderBy(a => a.Id)
                            .Select(a => a.Id)
                            .FirstAsync(cancellationToken)
                            .ConfigureAwait(false);
                    }

                    _logger.LogInformation(
                        "SqlBulkCopy partition completed. Records: {Current}/{Total}",
                        System.Math.Min(i + ChunkSize, totalQuantity),
                        totalQuantity);
                }
            }
            finally
            {
                if (shouldCloseConnection)
                    await _context.Database.CloseConnectionAsync().ConfigureAwait(false);
            }

            return firstAssetId;
        }

        private static DataTable BuildAssetDataTable(Asset template, int quantity)
        {
            var dt = new DataTable("Assets");

            dt.Columns.Add("ItemId", typeof(long));
            dt.Columns.Add("SupplierId", typeof(long));
            dt.Columns.Add("ManufacturerId", typeof(long));
            dt.Columns.Add("PrimaryPurposId", typeof(long));
            dt.Columns.Add("DepotId", typeof(long));
            dt.Columns.Add("BatchId", typeof(long));
            dt.Columns.Add("CreationDate", typeof(DateTime));
            dt.Columns.Add("CreatedBy", typeof(string));
            dt.Columns.Add("Status", typeof(int));
            dt.Columns.Add("IsAssigned", typeof(bool));
            dt.Columns.Add("PurchasePrice", typeof(decimal));
            dt.Columns.Add("IsDeleted", typeof(bool));

            for (var i = 0; i < quantity; i++)
            {
                var row = dt.NewRow();
                row["ItemId"] = template.ItemId;
                row["SupplierId"] = template.SupplierId ?? (object)DBNull.Value;
                row["ManufacturerId"] = template.ManufacturerId ?? (object)DBNull.Value;
                row["PrimaryPurposId"] = template.PrimaryPurposId ?? (object)DBNull.Value;
                row["DepotId"] = template.DepotId;
                row["BatchId"] = template.BatchId;
                row["CreationDate"] = template.CreationDate;
                row["CreatedBy"] = template.CreatedBy ?? (object)DBNull.Value;
                row["Status"] = (int)(template.Status ?? AssetStatus.ReadyToIssue);
                row["IsAssigned"] = false;
                row["PurchasePrice"] = template.PurchasePrice ?? (object)DBNull.Value;
                row["IsDeleted"] = false;

                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
