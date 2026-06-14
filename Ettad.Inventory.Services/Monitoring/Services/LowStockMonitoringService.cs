using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.CrossCutting.Comman.Models;
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
    public class LowStockMonitoringService : ILowStockMonitoringService
    {
        private readonly ILogger<LowStockMonitoringService> _logger;
        private readonly ICrossCuttingRepository<BaseItem> _baseItemRepository;
        private readonly ICrossCuttingRepository<InventoryDetail> _inventoryDetailsRepository;
        private readonly ICrossCuttingRepository<SupplyDetail> _supplyDetailsRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDepotAccessService _depotAccessService;

        public LowStockMonitoringService(
            ILogger<LowStockMonitoringService> logger,
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

        public async Task<APIOperationResponse<int>> GetLowStockItemsCountAsync(long? depotId = null, List<long>? depotIds = null)
        {
            var effective = new List<long>();
            if (depotIds != null) foreach (var d in depotIds) if (d > 0) effective.Add(d);
            if (depotId.HasValue && depotId.Value > 0) effective.Add(depotId.Value);
            var distinct = effective.Distinct().ToList();

            _logger.LogInformation("Getting low stock items count. DepotCount: {DepotCount}", distinct.Count > 0 ? distinct.Count : (int?)null);

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
                                _logger.LogWarning("User {UserId} attempted low-stock count for unauthorized depot {DepotId}", userId, dId);
                                return APIOperationResponse<int>.Fail(ResponseType.Forbidden, "You do not have access to one or more of the requested depots.");
                            }
                        }
                    }
                }

                IReadOnlyList<long>? depotFilter = distinct.Count > 0 ? distinct : null;

                var itemsToCheck = await _baseItemRepository
                    .Find(i => i.MinimumQuantity.HasValue && i.MinimumQuantity.Value > 0 && !i.IsDeleted)
                    .ToListAsync();

                _logger.LogInformation($"Found {itemsToCheck.Count} items with minimum quantity configured.");

                if (itemsToCheck.Count == 0)
                    return APIOperationResponse<int>.Success(0);

                var itemIds = itemsToCheck.Select(i => i.Id).ToList();
                var stockMetrics = await GetStockMetricsByItemAsync(itemIds, depotFilter).ConfigureAwait(false);

                int lowStockCount = 0;
                foreach (var item in itemsToCheck)
                {
                    if (!stockMetrics.TryGetValue(item.Id, out var metrics))
                        continue;

                    var remaining = metrics.TotalStock - (metrics.HoldQuantity + metrics.SuppliedQuantity);
                    if (remaining <= item.MinimumQuantity)
                        lowStockCount++;
                }

                _logger.LogInformation($"Found {lowStockCount} items below minimum stock level.");
                return APIOperationResponse<int>.Success(lowStockCount);
            }
            catch (Exception ex)
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
                var itemsToCheck = await _baseItemRepository
                    .Find(i => i.MinimumQuantity.HasValue && i.MinimumQuantity.Value > 0 && !i.IsDeleted)
                    .OrderBy(i => i.Name)
                    .ThenBy(i => i.Id)
                    .ToListAsync();

                _logger.LogInformation($"Found {itemsToCheck.Count} items with minimum quantity configured.");

                var lowStockItems = new List<LowStockItemDto>();

                foreach (var item in itemsToCheck)
                {
                    var lowStockInfo = await CheckItemStockAsync(item, null);
                    if (lowStockInfo != null)
                    {
                        lowStockItems.Add(new LowStockItemDto
                        {
                            ItemId = item.Id,
                            ItemName = item.Name ?? string.Empty,
                            ItemNameAr = item.NameAr,
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting low stock items.");
                return APIOperationResponse<List<LowStockItemDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<PaginatedList<LowStockItemDto>>> GetLowStockItemsPaginatedAsync(
            PagedListRequest request,
            long? depotId = null,
            List<long>? depotIds = null)
        {
            request ??= new PagedListRequest();
            PagedListRequestNormalizer.Normalize(request);

            var effective = new List<long>();
            if (depotIds != null) foreach (var d in depotIds) if (d > 0) effective.Add(d);
            if (depotId.HasValue && depotId.Value > 0) effective.Add(depotId.Value);
            var distinctDepots = effective.Distinct().ToList();

            _logger.LogInformation(
                "Getting low stock items (paged). Page={Page}, PageSize={PageSize}, DepotFilterCount={DepotCount}",
                request.Page,
                request.PageSize,
                distinctDepots.Count > 0 ? distinctDepots.Count : (int?)null);

            try
            {
                if (distinctDepots.Any())
                {
                    var userId = _currentUserService.UserId;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        foreach (var dId in distinctDepots)
                        {
                            if (!await _depotAccessService.HasDepotAccessAsync(userId, dId))
                            {
                                _logger.LogWarning("User {UserId} attempted paged low-stock for unauthorized depot {DepotId}", userId, dId);
                                return APIOperationResponse<PaginatedList<LowStockItemDto>>.Fail(
                                    ResponseType.Forbidden,
                                    "You do not have access to one or more of the requested depots.");
                            }
                        }
                    }
                }

                IReadOnlyList<long>? depotFilter = distinctDepots.Count > 0 ? distinctDepots : null;

                var itemsToCheck = await _baseItemRepository
                    .Find(i => i.MinimumQuantity.HasValue && i.MinimumQuantity.Value > 0 && !i.IsDeleted)
                    .OrderBy(i => i.Name)
                    .ThenBy(i => i.Id)
                    .ToListAsync();

                var lowStockItems = new List<LowStockItemDto>();

                foreach (var item in itemsToCheck)
                {
                    var lowStockInfo = await CheckItemStockAsync(item, depotFilter).ConfigureAwait(false);
                    if (lowStockInfo != null)
                    {
                        lowStockItems.Add(new LowStockItemDto
                        {
                            ItemId = item.Id,
                            ItemName = item.Name ?? string.Empty,
                            ItemNameAr = item.NameAr,
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

                var totalCount = lowStockItems.Count;

                var pageItems = lowStockItems
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var paginated = new PaginatedList<LowStockItemDto>(pageItems, totalCount, request.Page, request.PageSize);

                _logger.LogInformation("Paged low-stock: total {TotalCount}, returning {Returned} rows", totalCount, pageItems.Count);
                return APIOperationResponse<PaginatedList<LowStockItemDto>>.Success(paginated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paged low stock items.");
                return APIOperationResponse<PaginatedList<LowStockItemDto>>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred: {ex.Message}");
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

        private async Task<LowStockItemInfo?> CheckItemStockAsync(BaseItem item, IReadOnlyList<long>? effectiveDepotIds)
        {
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
