using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.CrossCutting.Comman.Models;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Enums;
using AutoMapper;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Inventory.Service.Monitoring.Interfaces;
using Ettad.Application.Common.Interfaces;
using Ettad.Module.lookup.Interfaces;

namespace Ettad.Inventory.Service.Monitoring.Services
{
    public class ExpiringLotMonitoringService : IExpiringLotMonitoringService
    {
        private static readonly string[] ExpiringLotInventoryDetailIncludes =
        [
            nameof(InventoryDetail.Inventory),
            $"{nameof(InventoryDetail.Inventory)}.{nameof(Ettad.Data.Entities.Inventory.Depo)}",
            nameof(InventoryDetail.Item),
            nameof(InventoryDetail.Supplier),
            nameof(InventoryDetail.Manufacturer),
        ];

        private readonly ILogger<ExpiringLotMonitoringService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;
        private readonly ICrossCuttingRepository<InventoryDetail> _inventoryDetailsRepository;
        private readonly ICrossCuttingRepository<SupplyDetail> _supplyDetailsRepository;
        private readonly ICrossCuttingRepository<Supply> _supplyRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDepotAccessService _depotAccessService;

        public ExpiringLotMonitoringService(
            ILogger<ExpiringLotMonitoringService> logger,
            IDateTimeProvider dateTimeProvider,
            IMapper mapper,
            ICrossCuttingRepository<InventoryDetail> inventoryDetailsRepository,
            ICrossCuttingRepository<SupplyDetail> supplyDetailsRepository,
            ICrossCuttingRepository<Supply> supplyRepository,
            ICurrentUserService currentUserService,
            IDepotAccessService depotAccessService)
        {
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
            _inventoryDetailsRepository = inventoryDetailsRepository;
            _supplyDetailsRepository = supplyDetailsRepository;
            _supplyRepository = supplyRepository;
            _currentUserService = currentUserService;
            _depotAccessService = depotAccessService;
        }

        public async Task<APIOperationResponse<int>> GetExpiringLotsCountAsync(long? depotId = null, List<long>? depotIds = null)
        {
            var effective = new List<long>();
            if (depotIds != null) foreach (var d in depotIds) if (d > 0) effective.Add(d);
            if (depotId.HasValue && depotId.Value > 0) effective.Add(depotId.Value);
            var distinct = effective.Distinct().ToList();

            _logger.LogInformation("Getting expiring lots count (next 30 days). DepotCount: {DepotCount}", distinct.Count > 0 ? distinct.Count : (int?)null);

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
                                _logger.LogWarning("User {UserId} attempted expiring-lot count for unauthorized depot {DepotId}", userId, dId);
                                return APIOperationResponse<int>.Fail(ResponseType.Forbidden, "You do not have access to one or more of the requested depots.");
                            }
                        }
                    }
                }

                var today = _dateTimeProvider.Now.Date;
                var thirtyDaysFromNow = today.AddDays(30);

                var query = _inventoryDetailsRepository.Find(
                    id =>
                        id.ExpiryDate.HasValue &&
                        id.ExpiryDate.Value.Date >= today &&
                        id.ExpiryDate.Value.Date <= thirtyDaysFromNow &&
                        !id.Inventory.IsDeleted,
                    false,
                    ExpiringLotInventoryDetailIncludes);

                if (distinct.Any())
                    query = query.Where(id => distinct.Contains(id.Inventory.DepoId));

                var expiringLots = await query
                    .Select(id => new { id.ItemId, id.Lot, id.ItemQuantity })
                    .ToListAsync()
                    .ConfigureAwait(false);

                if (expiringLots.Count == 0)
                    return APIOperationResponse<int>.Success(0);

                var itemIds = expiringLots.Select(x => x.ItemId).Distinct().ToList();
                var lotNumbers = expiringLots.Select(x => x.Lot).Distinct().ToList();

                var supplyRows = await (
                    from sd in _supplyDetailsRepository.Find(sd => !sd.IsDeleted)
                    join s in _supplyRepository.Find(s => !s.IsDeleted) on sd.SupplyId equals s.Id
                    where itemIds.Contains(sd.ItemId) && lotNumbers.Contains(sd.Lot)
                    select new { sd.ItemId, sd.Lot, sd.Quantity, s.SubmissionStatus }
                ).ToListAsync().ConfigureAwait(false);

                var usedByItemLot = supplyRows
                    .Where(x => x.SubmissionStatus == SupplySubmissionStatus.Submitted)
                    .GroupBy(x => (x.ItemId, x.Lot))
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

                var reservedByItemLot = supplyRows
                    .Where(x => x.SubmissionStatus == SupplySubmissionStatus.Draft)
                    .GroupBy(x => (x.ItemId, x.Lot))
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));

                var expiringCount = 0;
                foreach (var lot in expiringLots)
                {
                    var key = (lot.ItemId, lot.Lot);
                    usedByItemLot.TryGetValue(key, out var used);
                    reservedByItemLot.TryGetValue(key, out var reserved);
                    var remaining = Math.Max(0, lot.ItemQuantity - used - reserved);
                    if (remaining > 0)
                        expiringCount++;
                }

                _logger.LogInformation($"Found {expiringCount} lots expiring in the next 30 days.");

                return APIOperationResponse<int>.Success(expiringCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting expiring lots count.");
                return APIOperationResponse<int>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<PaginatedList<ExpiringLotDto>>> GetExpiringLotsPaginatedAsync(
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
                "Getting expiring lots (paged, next 30 days). Page={Page}, PageSize={PageSize}, DepotFilterCount={DepotCount}",
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
                                _logger.LogWarning("User {UserId} attempted paged expiring lots for unauthorized depot {DepotId}", userId, dId);
                                return APIOperationResponse<PaginatedList<ExpiringLotDto>>.Fail(
                                    ResponseType.Forbidden,
                                    "You do not have access to one or more of the requested depots.");
                            }
                        }
                    }
                }

                HashSet<long>? depotFilter = distinctDepots.Count > 0 ? distinctDepots.ToHashSet() : null;

                var expiringLotDtos = await BuildSortedExpiringLotDtosAsync(depotFilter).ConfigureAwait(false);
                var totalCount = expiringLotDtos.Count;

                var pageItems = expiringLotDtos
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var paginated = new PaginatedList<ExpiringLotDto>(pageItems, totalCount, request.Page, request.PageSize);

                _logger.LogInformation("Paged expiring lots: total {TotalCount}, returning {Returned} rows", totalCount, pageItems.Count);

                return APIOperationResponse<PaginatedList<ExpiringLotDto>>.Success(paginated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paged expiring lots.");
                return APIOperationResponse<PaginatedList<ExpiringLotDto>>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred: {ex.Message}");
            }
        }

        private async Task<List<ExpiringLotDto>> BuildSortedExpiringLotDtosAsync(HashSet<long>? depotDepoIdsFilter)
        {
            var today = _dateTimeProvider.Now.Date;
            var thirtyDaysFromNow = today.AddDays(30);

            var query = _inventoryDetailsRepository.Find(
                id =>
                    id.ExpiryDate.HasValue &&
                    id.ExpiryDate.Value.Date >= today &&
                    id.ExpiryDate.Value.Date <= thirtyDaysFromNow &&
                    !id.Inventory.IsDeleted,
                false,
                ExpiringLotInventoryDetailIncludes);

            if (depotDepoIdsFilter != null && depotDepoIdsFilter.Count > 0)
                query = query.Where(id => depotDepoIdsFilter.Contains(id.Inventory.DepoId));

            var expiringLots = await query.ToListAsync().ConfigureAwait(false);

            var expiringLotDtos = new List<ExpiringLotDto>();

            foreach (var lot in expiringLots)
            {
                var remainingQuantity = await CalculateRemainingQuantityAsync(lot).ConfigureAwait(false);

                if (remainingQuantity <= 0)
                    continue;

                var daysUntilExpiry = lot.ExpiryDate.HasValue
                    ? (int)(lot.ExpiryDate.Value.Date - today).TotalDays
                    : (int?)null;

                var lotDetail = _mapper.Map<LotDetailDto>(lot);

                expiringLotDtos.Add(new ExpiringLotDto
                {
                    InventoryDetailId = lot.Id,
                    ItemId = lot.ItemId,
                    ItemName = lot.Item?.Name ?? string.Empty,
                    ItemNameAr = lot.Item?.NameAr,
                    ItemNo = lot.Item?.ItemNo,
                    Lot = lot.Lot,
                    BatchNo = lot.BatchNo,
                    ExpiryDate = lot.ExpiryDate,
                    DaysUntilExpiry = daysUntilExpiry,
                    RemainingQuantity = remainingQuantity,
                    Depot = lotDetail.Depot,
                    Supplier = lotDetail.Supplier,
                    Manufacturer = lotDetail.Manufacturer
                });
            }

            return expiringLotDtos
                .OrderBy(l => l.ExpiryDate)
                .ThenBy(l => l.Lot)
                .ThenBy(l => l.InventoryDetailId)
                .ToList();
        }

        private async Task<long> CalculateRemainingQuantityAsync(InventoryDetail lot)
        {
            // Get supply details for this lot
            var allSupplyDetails = await _supplyDetailsRepository.FindAsync(
                sd => sd.ItemId == lot.ItemId && sd.Lot == lot.Lot && !sd.IsDeleted
            );

            // Get all supplies to check submission status
            var supplyIds = allSupplyDetails.Select(sd => sd.SupplyId).Distinct().ToList();
            var supplies = supplyIds.Any()
                ? await _supplyRepository.FindAsync(s => supplyIds.Contains(s.Id) && !s.IsDeleted)
                : new List<Supply>();

            var supplyStatusMap = supplies.ToDictionary(s => s.Id, s => s.SubmissionStatus);

            // Calculate used quantity (submitted supplies)
            var usedQuantity = allSupplyDetails
                .Where(sd => supplyStatusMap.ContainsKey(sd.SupplyId) && 
                           supplyStatusMap[sd.SupplyId] == SupplySubmissionStatus.Submitted)
                .Sum(sd => sd.Quantity);

            // Calculate reserved quantity (draft supplies)
            var reservedQuantity = allSupplyDetails
                .Where(sd => supplyStatusMap.ContainsKey(sd.SupplyId) && 
                           supplyStatusMap[sd.SupplyId] == SupplySubmissionStatus.Draft)
                .Sum(sd => sd.Quantity);

            // Calculate remaining quantity
            var remainingQuantity = lot.ItemQuantity - usedQuantity - reservedQuantity;

            return Math.Max(0, remainingQuantity);
        }
    }
}
