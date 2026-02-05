using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.Monitoring
{
    public class LowStockMonitoringService : ILowStockMonitoringService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LowStockMonitoringService> _logger;

        public LowStockMonitoringService(
            ApplicationDbContext context,
            ILogger<LowStockMonitoringService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<APIOperationResponse<int>> GetLowStockItemsCountAsync()
        {
            _logger.LogInformation("Getting low stock items count");

            try
            {
                var itemsToCheck = await _context.BaseItems
                    .Where(i => i.MinimumQuantity.HasValue && i.MinimumQuantity.Value > 0 && !i.IsDeleted)
                    .AsNoTracking()
                    .ToListAsync();

                _logger.LogInformation($"Found {itemsToCheck.Count} items with minimum quantity configured.");

                int lowStockCount = 0;

                foreach (var item in itemsToCheck)
                {
                    var lowStockInfo = await CheckItemStockAsync(item);
                    if (lowStockInfo != null)
                    {
                        lowStockCount++;
                    }
                }

                _logger.LogInformation($"Found {lowStockCount} items below minimum stock level.");

                return APIOperationResponse<int>.Success(lowStockCount);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting low stock items count.");
                return APIOperationResponse<int>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<LowStockItemDto>>> GetLowStockItemsAsync()
        {
            _logger.LogInformation("Getting low stock items with details");

            try
            {
                var itemsToCheck = await _context.BaseItems
                    .Where(i => i.MinimumQuantity.HasValue && i.MinimumQuantity.Value > 0 && !i.IsDeleted)
                    .AsNoTracking()
                    .ToListAsync();

                _logger.LogInformation($"Found {itemsToCheck.Count} items with minimum quantity configured.");

                var lowStockItems = new List<LowStockItemDto>();

                foreach (var item in itemsToCheck)
                {
                    var lowStockInfo = await CheckItemStockAsync(item);
                    if (lowStockInfo != null)
                    {
                        lowStockItems.Add(new LowStockItemDto
                        {
                            ItemId = item.Id,
                            ItemName = item.Name ?? string.Empty,
                            ItemNo = item.ItemNo,
                            Nsn = item.Nsn,
                            MinimumQuantity = item.MinimumQuantity,
                            TotalStock = lowStockInfo.TotalStock,
                            HoldQuantity = lowStockInfo.HoldQuantity,
                            SuppliedQuantity = lowStockInfo.SuppliedQuantity,
                            Remaining = lowStockInfo.Remaining
                        });
                    }
                }

                _logger.LogInformation($"Found {lowStockItems.Count} items below minimum stock level.");

                return APIOperationResponse<List<LowStockItemDto>>.Success(lowStockItems);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting low stock items.");
                return APIOperationResponse<List<LowStockItemDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private async Task<LowStockItemInfo?> CheckItemStockAsync(BaseItem item)
        {
            var totalStock = await _context.InventoryDetails
                .Where(id => id.ItemId == item.Id && !id.Inventory.IsDeleted)
                .SumAsync(id => id.ItemQuantity);

            var supplyDetails = await _context.SupplyDetails
                .Include(sd => sd.Supply)
                .Where(sd => sd.ItemId == item.Id && !sd.IsDeleted && !sd.Supply.IsDeleted)
                .Select(sd => new { sd.Quantity, sd.Supply.SubmissionStatus })
                .ToListAsync();

            var holdQuantity = supplyDetails
                .Where(sd => sd.SubmissionStatus == SupplySubmissionStatus.Draft)
                .Sum(sd => sd.Quantity);

            var suppliedQuantity = supplyDetails
                .Where(sd => sd.SubmissionStatus == SupplySubmissionStatus.Submitted)
                .Sum(sd => sd.Quantity);

            var remaining = totalStock - (holdQuantity + suppliedQuantity);

            _logger.LogDebug($"Item {item.Name} (ID: {item.Id}): Min={item.MinimumQuantity}, Total={totalStock}, Hold={holdQuantity}, Supplied={suppliedQuantity}, Remaining={remaining}");

            if (remaining <= item.MinimumQuantity)
            {
                return new LowStockItemInfo
                {
                    Item = item,
                    TotalStock = totalStock,
                    HoldQuantity = holdQuantity,
                    SuppliedQuantity = suppliedQuantity,
                    Remaining = remaining
                };
            }

            return null;
        }
    }
}
