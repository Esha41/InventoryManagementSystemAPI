using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Inventory.Service.Monitoring.Interfaces;
using Ettad.Application.Common.Interfaces;
using Ettad.Module.lookup.Interfaces;

namespace Ettad.Inventory.Service.Monitoring.Services
{
    public class CriticalStockMonitoringService : ICriticalStockMonitoringService
    {
        private readonly ILogger<CriticalStockMonitoringService> _logger;
        private readonly ICrossCuttingRepository<BaseItem> _baseItemRepository;
        private readonly ICrossCuttingRepository<InventoryDetail> _inventoryDetailsRepository;
        private readonly ICrossCuttingRepository<SupplyDetail> _supplyDetailsRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDepotAccessService _depotAccessService;

        public CriticalStockMonitoringService(
            ILogger<CriticalStockMonitoringService> logger,
            ICrossCuttingRepository<BaseItem> baseItemRepository,
            ICrossCuttingRepository<InventoryDetail> inventoryDetailsRepository,
            ICrossCuttingRepository<SupplyDetail> supplyDetailsRepository,
            ICurrentUserService currentUserService,
            IDepotAccessService depotAccessService)
        {
            _logger = logger;
            _baseItemRepository = baseItemRepository;
            _inventoryDetailsRepository = inventoryDetailsRepository;
            _supplyDetailsRepository = supplyDetailsRepository;
            _currentUserService = currentUserService;
            _depotAccessService = depotAccessService;
        }

        public async Task<APIOperationResponse<int>> GetCriticalStockItemsCountAsync(long? depotId = null, List<long>? depotIds = null)
        {
            var effective = new List<long>();
            if (depotIds != null) foreach (var d in depotIds) if (d > 0) effective.Add(d);
            if (depotId.HasValue && depotId.Value > 0) effective.Add(depotId.Value);
            var distinct = effective.Distinct().ToList();

            _logger.LogInformation("Getting critical stock items count. DepotCount: {DepotCount}", distinct.Count > 0 ? distinct.Count : (int?)null);

            try
            {
                if (distinct.Any())
                {
                    var userId = _currentUserService.UserId;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        foreach (var dId in distinct)
                        {
                            if (!await _depotAccessService.HasDepotAccessAsync(userId, dId))
                            {
                                _logger.LogWarning("User {UserId} attempted critical-stock count for unauthorized depot {DepotId}", userId, dId);
                                return APIOperationResponse<int>.Fail(ResponseType.Forbidden, "You do not have access to one or more of the requested depots.");
                            }
                        }
                    }
                }

                IReadOnlyList<long>? depotFilter = distinct.Count > 0 ? distinct : null;

                var itemsToCheck = await _baseItemRepository
                    .Find(i => i.CriticalQuantity.HasValue && i.CriticalQuantity.Value > 0 && !i.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Found {Count} items with critical quantity configured.", itemsToCheck.Count);

                if (itemsToCheck.Count == 0)
                    return APIOperationResponse<int>.Success(0);

                var itemIds = itemsToCheck.Select(i => i.Id).ToList();
                var stockMetrics = await GetStockMetricsByItemAsync(itemIds, depotFilter).ConfigureAwait(false);

                int criticalCount = 0;
                foreach (var item in itemsToCheck)
                {
                    if (!stockMetrics.TryGetValue(item.Id, out var metrics))
                        continue;

                    var remaining = metrics.TotalStock - (metrics.HoldQuantity + metrics.SuppliedQuantity);
                    if (remaining <= item.CriticalQuantity)
                        criticalCount++;
                }

                _logger.LogInformation("Found {Count} items at or below critical stock level.", criticalCount);
                return APIOperationResponse<int>.Success(criticalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting critical stock items count.");
                return APIOperationResponse<int>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<CriticalStockItemDto>>> GetCriticalStockItemsAsync()
        {
            _logger.LogInformation("Getting critical stock items with details");

            try
            {
                var itemsToCheck = await _baseItemRepository
                    .Find(i => i.CriticalQuantity.HasValue && i.CriticalQuantity.Value > 0 && !i.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation("Found {Count} items with critical quantity configured.", itemsToCheck.Count);

                var criticalItems = new List<CriticalStockItemDto>();

                foreach (var item in itemsToCheck)
                {
                    var info = await CheckItemStockAsync(item, null);
                    if (info != null)
                    {
                        criticalItems.Add(new CriticalStockItemDto
                        {
                            ItemId = item.Id,
                            ItemName = item.Name ?? string.Empty,
                            ItemNameAr = item.NameAr,
                            ItemNo = item.ItemNo,
                            Nsn = item.Nsn,
                            CriticalQuantity = item.CriticalQuantity,
                            TotalStock = info.TotalStock,
                            HoldQuantity = info.HoldQuantity,
                            SuppliedQuantity = info.SuppliedQuantity,
                            Remaining = info.Remaining
                        });
                    }
                }

                _logger.LogInformation("Found {Count} items at or below critical stock level.", criticalItems.Count);
                return APIOperationResponse<List<CriticalStockItemDto>>.Success(criticalItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting critical stock items.");
                return APIOperationResponse<List<CriticalStockItemDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private sealed class ItemStockMetrics
        {
            public long TotalStock { get; init; }
            public long HoldQuantity { get; init; }
            public long SuppliedQuantity { get; init; }
        }

        private async Task<Dictionary<long, ItemStockMetrics>> GetStockMetricsByItemAsync(
            IReadOnlyList<long> itemIds,
            IReadOnlyList<long>? effectiveDepotIds)
        {
            if (itemIds.Count == 0)
                return new Dictionary<long, ItemStockMetrics>();

            var inventoryQuery = _inventoryDetailsRepository.Find(
                id => itemIds.Contains(id.ItemId) && !id.Inventory.IsDeleted,
                false,
                nameof(InventoryDetail.Inventory));

            if (effectiveDepotIds != null && effectiveDepotIds.Count > 0)
                inventoryQuery = inventoryQuery.Where(id => effectiveDepotIds.Contains(id.Inventory.DepoId));

            var totalStockByItem = await inventoryQuery
                .GroupBy(id => id.ItemId)
                .Select(g => new { ItemId = g.Key, TotalStock = g.Sum(x => x.ItemQuantity) })
                .ToListAsync()
                .ConfigureAwait(false);

            var supplyRows = await _supplyDetailsRepository
                .Find(
                    sd => itemIds.Contains(sd.ItemId) && !sd.IsDeleted && !sd.Supply.IsDeleted,
                    false,
                    nameof(SupplyDetail.Supply))
                .Select(sd => new { sd.ItemId, sd.Quantity, sd.Supply.SubmissionStatus })
                .ToListAsync()
                .ConfigureAwait(false);

            var holdByItem = supplyRows
                .Where(sd => sd.SubmissionStatus == SupplySubmissionStatus.Draft)
                .GroupBy(sd => sd.ItemId)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

            var suppliedByItem = supplyRows
                .Where(sd => sd.SubmissionStatus == SupplySubmissionStatus.Submitted)
                .GroupBy(sd => sd.ItemId)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

            var result = new Dictionary<long, ItemStockMetrics>();
            foreach (var row in totalStockByItem)
            {
                holdByItem.TryGetValue(row.ItemId, out var hold);
                suppliedByItem.TryGetValue(row.ItemId, out var supplied);
                result[row.ItemId] = new ItemStockMetrics
                {
                    TotalStock = row.TotalStock,
                    HoldQuantity = hold,
                    SuppliedQuantity = supplied
                };
            }

            return result;
        }

        private sealed class StockSnapshot
        {
            public long TotalStock { get; set; }
            public long HoldQuantity { get; set; }
            public long SuppliedQuantity { get; set; }
            public long Remaining { get; set; }
        }

        private async Task<StockSnapshot?> CheckItemStockAsync(BaseItem item, IReadOnlyList<long>? effectiveDepotIds)
        {
            if (!item.CriticalQuantity.HasValue || item.CriticalQuantity.Value <= 0)
                return null;

            var inventoryQuery = _inventoryDetailsRepository.Find(
                id => id.ItemId == item.Id && !id.Inventory.IsDeleted,
                false,
                nameof(InventoryDetail.Inventory));

            if (effectiveDepotIds != null && effectiveDepotIds.Count > 0)
                inventoryQuery = inventoryQuery.Where(id => effectiveDepotIds.Contains(id.Inventory.DepoId));

            var totalStock = await inventoryQuery.SumAsync(id => id.ItemQuantity);

            var supplyDetails = await _supplyDetailsRepository
                .Find(
                    sd => sd.ItemId == item.Id && !sd.IsDeleted && !sd.Supply.IsDeleted,
                    false,
                    nameof(SupplyDetail.Supply))
                .Select(sd => new { sd.Quantity, sd.Supply.SubmissionStatus })
                .ToListAsync();

            var holdQuantity = supplyDetails
                .Where(sd => sd.SubmissionStatus == SupplySubmissionStatus.Draft)
                .Sum(sd => sd.Quantity);

            var suppliedQuantity = supplyDetails
                .Where(sd => sd.SubmissionStatus == SupplySubmissionStatus.Submitted)
                .Sum(sd => sd.Quantity);

            var remaining = totalStock - (holdQuantity + suppliedQuantity);

            _logger.LogDebug(
                "Item {Name} (ID: {ItemId}): Critical={Critical}, Total={Total}, Hold={Hold}, Supplied={Supplied}, Remaining={Remaining}",
                item.Name, item.Id, item.CriticalQuantity, totalStock, holdQuantity, suppliedQuantity, remaining);

            if (remaining <= item.CriticalQuantity.Value)
            {
                return new StockSnapshot
                {
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
