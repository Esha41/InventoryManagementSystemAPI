using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Lookups.Services.Contracts;
using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.Inventory.Service.Monitoring
{
    /// <summary>
    /// Inventory dashboard monitoring: weapon assets (depot-scoped like <see cref="Inventories.InventoryService"/>)
    /// and pipeline counts (draft Supply + AssetSupply; orders approved but not fully fulfilled).
    /// </summary>
    public class InventoryDashboardMonitoringService : IInventoryDashboardMonitoringService
    {
        private readonly ICrossCuttingRepository<Asset> _assetRepository;
        private readonly ICrossCuttingRepository<BaseItem> _baseItemRepository;
        private readonly ICrossCuttingRepository<Supply> _supplyRepository;
        private readonly ICrossCuttingRepository<Order> _orderRepository;
        private readonly ICrossCuttingRepository<Ettad.Data.Entities.AssetSupply> _assetSupplyRepository;
        private readonly ICrossCuttingRepository<SupplyDetail> _supplyDetailRepository;
        private readonly ICrossCuttingRepository<InventoryDetail> _inventoryDetailRepository;
        private readonly ICrossCuttingRepository<WeaponSupplySelection> _weaponSupplySelectionRepository;
        private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;

        /// <summary>Base queryables for correlated subqueries (same DbContext as repositories; EF translates via captured IQueryable).</summary>
        private readonly IQueryable<SupplyDetail> _supplyDetailsAll;
        private readonly IQueryable<InventoryDetail> _inventoryDetailsAll;
        private readonly IQueryable<WeaponSupplySelection> _weaponSupplySelectionsAll;
        private readonly IQueryable<Supply> _suppliesAll;

        private readonly IDepotAccessService _depotAccessService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<InventoryDashboardMonitoringService> _logger;

        public InventoryDashboardMonitoringService(
            ICrossCuttingRepository<Asset> assetRepository,
            ICrossCuttingRepository<BaseItem> baseItemRepository,
            ICrossCuttingRepository<Supply> supplyRepository,
            ICrossCuttingRepository<Order> orderRepository,
            ICrossCuttingRepository<Ettad.Data.Entities.AssetSupply> assetSupplyRepository,
            ICrossCuttingRepository<SupplyDetail> supplyDetailRepository,
            ICrossCuttingRepository<InventoryDetail> inventoryDetailRepository,
            ICrossCuttingRepository<WeaponSupplySelection> weaponSupplySelectionRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            IDepotAccessService depotAccessService,
            ICurrentUserService currentUserService,
            ILogger<InventoryDashboardMonitoringService> logger)
        {
            _assetRepository = assetRepository;
            _baseItemRepository = baseItemRepository;
            _supplyRepository = supplyRepository;
            _orderRepository = orderRepository;
            _assetSupplyRepository = assetSupplyRepository;
            _supplyDetailRepository = supplyDetailRepository;
            _inventoryDetailRepository = inventoryDetailRepository;
            _weaponSupplySelectionRepository = weaponSupplySelectionRepository;
            _requestItemRepository = requestItemRepository;

            _supplyDetailsAll = _supplyDetailRepository.Find(x => true);
            _inventoryDetailsAll = _inventoryDetailRepository.Find(x => true);
            _weaponSupplySelectionsAll = _weaponSupplySelectionRepository.Find(x => true);
            _suppliesAll = _supplyRepository.Find(x => true);

            _depotAccessService = depotAccessService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<APIOperationResponse<InventoryDashboardSummaryDto>> GetInventoryDashboardSummaryAsync(
            long? depotId = null,
            List<long>? depotIds = null)
        {
            var weaponResult = await GetWeaponAssetDashboardAsync(depotId, depotIds);
            if (!weaponResult.Succeeded || weaponResult.Data == null)
                return APIOperationResponse<InventoryDashboardSummaryDto>.Fail(
                    (ResponseType)weaponResult.StatusCode,
                    weaponResult.Message ?? "Weapon dashboard failed");

            var pipelineResult = await GetPipelineDashboardAsync(depotId, depotIds);
            if (!pipelineResult.Succeeded || pipelineResult.Data == null)
                return APIOperationResponse<InventoryDashboardSummaryDto>.Fail(
                    (ResponseType)pipelineResult.StatusCode,
                    pipelineResult.Message ?? "Pipeline dashboard failed");

            return APIOperationResponse<InventoryDashboardSummaryDto>.Success(new InventoryDashboardSummaryDto
            {
                WeaponAssets = weaponResult.Data,
                Pipeline = pipelineResult.Data
            });
        }

        public async Task<APIOperationResponse<WeaponAssetDashboardDto>> GetWeaponAssetDashboardAsync(
            long? depotId = null,
            List<long>? depotIds = null)
        {
            _logger.LogInformation("Weapon asset dashboard. User: {UserId}", _currentUserService.UserId);

            try
            {
                var scope = await BuildDepotScopeAsync(depotId, depotIds);
                if (scope == null)
                    return APIOperationResponse<WeaponAssetDashboardDto>.Fail(ResponseType.Forbidden, "You do not have access to one or more of the requested depots.");

                var weaponAssets = BaseWeaponAssetQuery(scope);

                var total = await weaponAssets.CountAsync();
                var assigned = await weaponAssets.CountAsync(a => a.IsAssigned);
                var inDepot = await weaponAssets.CountAsync(a => !a.IsAssigned);

                var statusGroups = await weaponAssets
                    .GroupBy(a => a.Status)
                    .Select(g => new { g.Key, Count = g.Count() })
                    .ToListAsync();

                var unknown = statusGroups.Where(x => x.Key == null).Sum(x => x.Count);
                var byStatus = statusGroups
                    .Where(x => x.Key != null)
                    .Select(x => new WeaponAssetStatusCountDto { Status = x.Key!.Value, Count = x.Count })
                    .OrderBy(x => x.Status)
                    .ToList();

                return APIOperationResponse<WeaponAssetDashboardDto>.Success(new WeaponAssetDashboardDto
                {
                    TotalAssets = total,
                    AssignedCount = assigned,
                    InDepotCount = inDepot,
                    UnknownStatusCount = unknown,
                    ByStatus = byStatus
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Weapon asset dashboard failed");
                return APIOperationResponse<WeaponAssetDashboardDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<InventoryPipelineDashboardDto>> GetPipelineDashboardAsync(
            long? depotId = null,
            List<long>? depotIds = null)
        {
            _logger.LogInformation("Pipeline dashboard. User: {UserId}", _currentUserService.UserId);

            try
            {
                var scope = await BuildDepotScopeAsync(depotId, depotIds);
                if (scope == null)
                    return APIOperationResponse<InventoryPipelineDashboardDto>.Fail(ResponseType.Forbidden, "You do not have access to one or more of the requested depots.");

                var draftSupplyCount = await CountDraftSuppliesAsync(scope);
                var awaitingCount = await CountOrdersAwaitingFulfillmentAsync(scope);

                return APIOperationResponse<InventoryPipelineDashboardDto>.Success(new InventoryPipelineDashboardDto
                {
                    DraftSupplyCount = draftSupplyCount,
                    OrdersAwaitingFulfillmentCount = awaitingCount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Pipeline dashboard failed");
                return APIOperationResponse<InventoryPipelineDashboardDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<DraftSupplyListItemDto>>> GetDraftSuppliesListAsync(
            long? depotId = null,
            List<long>? depotIds = null)
        {
            _logger.LogInformation("Draft supplies list. User: {UserId}", _currentUserService.UserId);

            try
            {
                var scope = await BuildDepotScopeAsync(depotId, depotIds);
                if (scope == null)
                    return APIOperationResponse<List<DraftSupplyListItemDto>>.Fail(ResponseType.Forbidden, "You do not have access to one or more of the requested depots.");

                var supplyRows = await ListSupplyDraftRowsAsync(scope);
                var assetRows = await ListAssetSupplyDraftRowsAsync(scope);
                var merged = supplyRows.Concat(assetRows)
                    .OrderBy(x => x.OrderId)
                    .ThenBy(x => x.RowKind)
                    .ThenBy(x => x.RowId)
                    .ToList();

                return APIOperationResponse<List<DraftSupplyListItemDto>>.Success(merged);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Draft supplies list failed");
                return APIOperationResponse<List<DraftSupplyListItemDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<OrderAwaitingFulfillmentListItemDto>>> GetOrdersAwaitingFulfillmentListAsync(
            long? depotId = null,
            List<long>? depotIds = null)
        {
            _logger.LogInformation("Orders awaiting fulfillment list. User: {UserId}", _currentUserService.UserId);

            try
            {
                var scope = await BuildDepotScopeAsync(depotId, depotIds);
                if (scope == null)
                    return APIOperationResponse<List<OrderAwaitingFulfillmentListItemDto>>.Fail(ResponseType.Forbidden, "You do not have access to one or more of the requested depots.");

                var list = await BuildOrdersAwaitingFulfillmentListAsync(scope);
                return APIOperationResponse<List<OrderAwaitingFulfillmentListItemDto>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Orders awaiting fulfillment list failed");
                return APIOperationResponse<List<OrderAwaitingFulfillmentListItemDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private IQueryable<Asset> BaseWeaponAssetQuery(DepotScope scope)
        {
            var q =
                from a in _assetRepository.Find(a => true)
                join i in _baseItemRepository.Find(i => true) on a.ItemId equals i.Id
                where !a.IsDeleted && i.ItemType == ItemType.Weapon
                select a;

            if (scope.UserDepotIds != null)
                q = q.Where(a => scope.UserDepotIds.Contains(a.DepotId));

            if (scope.EffectiveDepotIds.Any())
                q = q.Where(a => scope.EffectiveDepotIds.Contains(a.DepotId));

            return q;
        }

        private async Task<int> CountDraftSuppliesAsync(DepotScope scope)
        {
            var supplyDraft = await CountSupplyDraftsAsync(scope);
            var assetSupplyDraft = await CountAssetSupplyDraftsAsync(scope);
            return supplyDraft + assetSupplyDraft;
        }

        private IQueryable<Supply> SupplyDraftBaseQueryable(DepotScope scope)
        {
            var q =
                from s in _supplyRepository.Find(s => true)
                join o in _orderRepository.Find(o => true) on s.OrderId equals o.Id
                where !s.IsDeleted && !o.IsDeleted
                      && s.SubmissionStatus == SupplySubmissionStatus.Draft
                      && o.RequestType == RequestType.Order
                select s;

            return ApplyPipelineDepotFilterToSupplyQuery(q, scope);
        }

        private IQueryable<Ettad.Data.Entities.AssetSupply> AssetSupplyDraftBaseQueryable(DepotScope scope)
        {
            var q =
                from a in _assetSupplyRepository.Find(a => true)
                join o in _orderRepository.Find(o => true) on a.OrderId equals o.Id
                where !a.IsDeleted && !o.IsDeleted
                      && a.SubmissionStatus == SupplySubmissionStatus.Draft
                      && o.RequestType == RequestType.Order
                select a;

            return ApplyPipelineDepotFilterToAssetSupplyQuery(q, scope);
        }

        private async Task<int> CountSupplyDraftsAsync(DepotScope scope)
        {
            return await SupplyDraftBaseQueryable(scope).Select(s => s.Id).Distinct().CountAsync();
        }

        private async Task<int> CountAssetSupplyDraftsAsync(DepotScope scope)
        {
            return await AssetSupplyDraftBaseQueryable(scope).Select(a => a.Id).Distinct().CountAsync();
        }

        private async Task<List<DraftSupplyListItemDto>> ListSupplyDraftRowsAsync(DepotScope scope)
        {
            return await SupplyDraftBaseQueryable(scope)
                .Join(
                    _orderRepository.Find(o => true),
                    s => s.OrderId,
                    o => o.Id,
                    (s, o) => new DraftSupplyListItemDto
                    {
                        RowKind = "Supply",
                        RowId = s.Id,
                        OrderId = o.Id,
                        OrderNumber = o.RequestNo,
                        SubmissionStatus = s.SubmissionStatus.ToString()
                    })
                .OrderBy(x => x.OrderId)
                .ThenBy(x => x.RowId)
                .ToListAsync();
        }

        private async Task<List<DraftSupplyListItemDto>> ListAssetSupplyDraftRowsAsync(DepotScope scope)
        {
            return await AssetSupplyDraftBaseQueryable(scope)
                .Join(
                    _orderRepository.Find(o => true),
                    a => a.OrderId,
                    o => o.Id,
                    (a, o) => new DraftSupplyListItemDto
                    {
                        RowKind = "AssetSupply",
                        RowId = a.Id,
                        OrderId = o.Id,
                        OrderNumber = o.RequestNo,
                        SubmissionStatus = a.SubmissionStatus.ToString()
                    })
                .OrderBy(x => x.OrderId)
                .ThenBy(x => x.RowId)
                .ToListAsync();
        }

        private IQueryable<Supply> ApplyPipelineDepotFilterToSupplyQuery(IQueryable<Supply> q, DepotScope scope)
        {
            if (!scope.RequiresPipelineDepotFilter)
                return q;
            if (!scope.PipelineDepotIds.Any())
                return q.Where(_ => false);

            var depots = scope.PipelineDepotIds;
            return q.Where(s =>
                _supplyDetailsAll.Any(sd =>
                    sd.SupplyId == s.Id && !sd.IsDeleted &&
                    _inventoryDetailsAll.Any(id =>
                        id.ItemId == sd.ItemId && id.ItemQuantity > 0 &&
                        !id.Inventory.IsDeleted &&
                        depots.Contains(id.Inventory.DepoId))));
        }

        private IQueryable<Ettad.Data.Entities.AssetSupply> ApplyPipelineDepotFilterToAssetSupplyQuery(IQueryable<Ettad.Data.Entities.AssetSupply> q, DepotScope scope)
        {
            if (!scope.RequiresPipelineDepotFilter)
                return q;
            if (!scope.PipelineDepotIds.Any())
                return q.Where(_ => false);

            var depots = scope.PipelineDepotIds;
            return q.Where(a =>
                _weaponSupplySelectionsAll.Any(ws =>
                    ws.OrderId == a.OrderId && depots.Contains(ws.DepotId)));
        }

        private IQueryable<Order> ApprovedOrdersForPipelineQueryable(DepotScope scope)
        {
            var approvedOrders =
                from o in _orderRepository.Find(o => true)
                where !o.IsDeleted
                      && o.Status == RequestStatus.Approved
                      && o.RequestType == RequestType.Order
                select o;

            return ApplyPipelineDepotFilterToOrdersQuery(approvedOrders, scope);
        }

        private async Task<int> CountOrdersAwaitingFulfillmentAsync(DepotScope scope)
        {
            var list = await BuildOrdersAwaitingFulfillmentListAsync(scope);
            return list.Count;
        }

        private async Task<List<OrderAwaitingFulfillmentListItemDto>> BuildOrdersAwaitingFulfillmentListAsync(DepotScope scope)
        {
            var orderRows = await ApprovedOrdersForPipelineQueryable(scope)
                .Select(o => new { o.Id, o.RequestNo, o.Status })
                .OrderBy(x => x.Id)
                .ToListAsync();

            var orderIds = orderRows.Select(x => x.Id).ToList();
            if (orderIds.Count == 0)
                return new List<OrderAwaitingFulfillmentListItemDto>();

            var requestItems = await _requestItemRepository.Find(ri => true)
                .Where(ri => orderIds.Contains(ri.RequestId) && !ri.IsDeleted)
                .Join(_baseItemRepository.Find(i => true), ri => ri.ItemId, i => i.Id, (ri, i) => new { ri.RequestId, i.ItemType })
                .ToListAsync();

            var itemsByOrder = requestItems.GroupBy(x => x.RequestId).ToDictionary(g => g.Key, g => g.Select(x => x.ItemType).ToList());

            var supplyByOrder = await _supplyRepository.Find(s => true)
                .Where(s => orderIds.Contains(s.OrderId) && !s.IsDeleted)
                .ToListAsync();

            var assetSupplyByOrder = await _assetSupplyRepository.Find(s => true)
                .Where(s => orderIds.Contains(s.OrderId) && !s.IsDeleted)
                .ToListAsync();

            var result = new List<OrderAwaitingFulfillmentListItemDto>();
            foreach (var row in orderRows)
            {
                var orderId = row.Id;
                itemsByOrder.TryGetValue(orderId, out var types);
                types ??= new List<ItemType>();
                var hasNonWeapon = types.Any(t => t is ItemType.Ammunition or ItemType.Explosive);
                var hasWeapon = types.Any(t => t == ItemType.Weapon);

                var supplies = supplyByOrder.Where(s => s.OrderId == orderId).ToList();
                var assetSupplies = assetSupplyByOrder.Where(s => s.OrderId == orderId).ToList();

                if (!IsOrderAwaitingFulfillment(hasNonWeapon, hasWeapon, supplies, assetSupplies))
                    continue;

                result.Add(new OrderAwaitingFulfillmentListItemDto
                {
                    OrderId = row.Id,
                    OrderNumber = row.RequestNo,
                    Status = row.Status.ToString()
                });
            }

            return result;
        }

        /// <summary>
        /// Approved order still needs work if any applicable path is not fully submitted+fulfilled.
        /// </summary>
        private static bool IsOrderAwaitingFulfillment(
            bool hasNonWeapon,
            bool hasWeapon,
            List<Supply> supplies,
            List<Ettad.Data.Entities.AssetSupply> assetSupplies)
        {
            bool ammoDone = !hasNonWeapon || supplies.Any(s =>
                s.SubmissionStatus == SupplySubmissionStatus.Submitted &&
                s.FulfillmentStatus == SupplyFulfillmentStatus.Fully);

            bool weaponDone = !hasWeapon || assetSupplies.Any(a =>
                a.SubmissionStatus == SupplySubmissionStatus.Submitted &&
                a.FulfillmentStatus == SupplyFulfillmentStatus.Fully);

            return !(ammoDone && weaponDone);
        }

        private IQueryable<Order> ApplyPipelineDepotFilterToOrdersQuery(IQueryable<Order> q, DepotScope scope)
        {
            if (!scope.RequiresPipelineDepotFilter)
                return q;
            if (!scope.PipelineDepotIds.Any())
                return q.Where(_ => false);

            var depots = scope.PipelineDepotIds;
            return q.Where(o =>
                _weaponSupplySelectionsAll.Any(ws => ws.OrderId == o.Id && depots.Contains(ws.DepotId))
                || _suppliesAll.Any(s => s.OrderId == o.Id && !s.IsDeleted &&
                    _supplyDetailsAll.Any(sd => sd.SupplyId == s.Id && !sd.IsDeleted &&
                        _inventoryDetailsAll.Any(id => id.ItemId == sd.ItemId && id.ItemQuantity > 0 &&
                            !id.Inventory.IsDeleted && depots.Contains(id.Inventory.DepoId)))));
        }

        private async Task<DepotScope?> BuildDepotScopeAsync(long? depotId, List<long>? depotIds)
        {
            var effective = new HashSet<long>();
            if (depotIds?.Any() == true)
                foreach (var d in depotIds)
                    effective.Add(d);
            if (depotId.HasValue)
                effective.Add(depotId.Value);

            var userId = _currentUserService.UserId;
            if (effective.Any() && !string.IsNullOrEmpty(userId))
            {
                foreach (var dId in effective)
                {
                    if (!await _depotAccessService.HasDepotAccessAsync(userId, dId))
                        return null;
                }
            }

            var userDepotIds = await _depotAccessService.GetUserAccessibleDepotIdsAsync();

            var pipelineDepots = new List<long>();
            if (effective.Any())
                pipelineDepots.AddRange(effective);
            else if (userDepotIds != null)
                pipelineDepots.AddRange(userDepotIds);

            return new DepotScope(effective, userDepotIds, pipelineDepots);
        }

        private sealed class DepotScope
        {
            public DepotScope(HashSet<long> effectiveDepotIds, IReadOnlyCollection<long>? userDepotIds, List<long> pipelineDepotIds)
            {
                EffectiveDepotIds = effectiveDepotIds;
                UserDepotIds = userDepotIds;
                PipelineDepotIds = pipelineDepotIds;
            }

            public HashSet<long> EffectiveDepotIds { get; }
            public IReadOnlyCollection<long>? UserDepotIds { get; }
            public List<long> PipelineDepotIds { get; }

            /// <summary>When false, pipeline counts are global (super-admin / unrestricted depot access).</summary>
            public bool RequiresPipelineDepotFilter => UserDepotIds != null || PipelineDepotIds.Count > 0;
        }
    }
}
