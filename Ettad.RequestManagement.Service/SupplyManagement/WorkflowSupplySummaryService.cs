using Ettad.Data.Entities;
using Ettad.Data.Enums;
using InventoryRecord = Ettad.Data.Entities.Inventory;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Inventory.Service.AssetSupply;
using Ettad.Inventory.Service.AssetSupply.Dtos;
using Ettad.RequestManagement.Service.SupplyManagement.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.RequestManagement.Service.SupplyManagement
{
    public class WorkflowSupplySummaryService : IWorkflowSupplySummaryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAssetSupplyService _assetSupplyService;
        private readonly ILogger<WorkflowSupplySummaryService> _logger;

        public WorkflowSupplySummaryService(
            ApplicationDbContext context,
            IAssetSupplyService assetSupplyService,
            ILogger<WorkflowSupplySummaryService> logger)
        {
            _context = context;
            _assetSupplyService = assetSupplyService;
            _logger = logger;
        }

        public async Task<APIOperationResponse<WorkflowSupplySummaryDto>> GetSummaryForOrderAsync(long orderId)
        {
            try
            {
                var order = await _context.Set<Order>()
                    .AsNoTracking()
                    .Include(o => o.RequestItems)
                    .ThenInclude(ri => ri.Item)
                    .FirstOrDefaultAsync(o => o.Id == orderId && !o.IsDeleted);

                if (order == null)
                    return APIOperationResponse<WorkflowSupplySummaryDto>.Fail(ResponseType.NotFound, "Order not found");

                if (order.RequestType != RequestType.Order)
                    return APIOperationResponse<WorkflowSupplySummaryDto>.Fail(ResponseType.BadRequest, "Not an order request");

                if (order.Status != RequestStatus.Approved)
                    return APIOperationResponse<WorkflowSupplySummaryDto>.Fail(ResponseType.Forbidden, "Order is not completed");

                var weaponOnly = IsWeaponOnlyOrder(order);

                if (weaponOnly)
                {
                    var assetResult = await _assetSupplyService.GetByOrderIdAsync(orderId);
                    if (assetResult.Succeeded && assetResult.Data != null)
                        return APIOperationResponse<WorkflowSupplySummaryDto>.Success(await BuildFromAssetSupplyAsync(order, assetResult.Data));
                }

                return APIOperationResponse<WorkflowSupplySummaryDto>.Success(await BuildFromAmmunitionSupplyAsync(orderId, order, weaponOnly));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Workflow supply summary failed for OrderId {OrderId}", orderId);
                return APIOperationResponse<WorkflowSupplySummaryDto>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        private static bool IsWeaponOnlyOrder(Order order)
        {
            var items = order.RequestItems?.Where(ri => !ri.IsDeleted).ToList() ?? new List<RequestItem>();
            if (items.Count == 0)
                return false;
            return items.All(ri => ri.Item != null && ri.Item.ItemType == ItemType.Weapon);
        }

        private async Task<WorkflowSupplySummaryDto> BuildFromAssetSupplyAsync(Order order, AssetSupplyDto data)
        {
            var dto = new WorkflowSupplySummaryDto
            {
                OrderId = order.Id,
                OrderSupplyDate = order.SupplyDate,
                SupplyDate = data.SupplyDate,
                SubmissionStatus = data.SubmissionStatus,
                FulfillmentStatus = data.FulfillmentStatus,
                ReceiverName = data.ReceiverName,
                ReceiverMilitaryId = data.ReceiverMilitaryId,
                ReceiverRankName = data.ReceiverRank?.NameEn ?? data.ReceiverRank?.NameAr,
                Notes = data.Notes,
                IsWeaponOrder = true
            };

            var details = data.SupplyDetails ?? new List<AssetSupplyDetailDto>();
            if (details.Count == 0)
                return dto;

            var assetIds = details.Select(d => d.AssetId).Distinct().ToList();
            var depotByAsset = await (
                from a in _context.Set<Asset>().AsNoTracking()
                join d in _context.Set<Depot>().AsNoTracking() on a.DepotId equals d.Id
                where assetIds.Contains(a.Id) && !a.IsDeleted && !d.IsDeleted
                select new { a.Id, DepotId = a.DepotId, DepotName = d.NameEn, DepotCode = d.Code }
            ).ToDictionaryAsync(x => x.Id);

            var requestedByItem = order.RequestItems?
                .Where(ri => !ri.IsDeleted)
                .GroupBy(ri => ri.ItemId)
                .ToDictionary(g => g.Key, g => g.Sum(ri => ri.Quantity)) ?? new Dictionary<long, long>();

            foreach (var line in details)
            {
                depotByAsset.TryGetValue(line.AssetId, out var dep);
                requestedByItem.TryGetValue(line.ItemId, out var reqQty);

                dto.Lines.Add(new WorkflowSupplySummaryLineDto
                {
                    ItemId = line.ItemId,
                    ItemName = line.ItemName ?? string.Empty,
                    ItemNo = null,
                    RequestedQuantity = reqQty,
                    SuppliedQuantity = 1,
                    Lot = line.AssetSerialNumber ?? line.AssetTag ?? "-",
                    DepotId = dep?.DepotId,
                    DepotName = dep?.DepotName,
                    DepotCode = dep?.DepotCode,
                    Notes = line.Notes
                });
            }

            return dto;
        }

        private async Task<WorkflowSupplySummaryDto> BuildFromAmmunitionSupplyAsync(long orderId, Order order, bool isWeaponOrderFlag)
        {
            var dto = new WorkflowSupplySummaryDto
            {
                OrderId = order.Id,
                OrderSupplyDate = order.SupplyDate,
                IsWeaponOrder = isWeaponOrderFlag
            };

            var supply = await _context.Set<Supply>()
                .AsNoTracking()
                .Include(s => s.ReceiverRank)
                .Include(s => s.SupplyDetails)
                .ThenInclude(sd => sd.Item)
                .FirstOrDefaultAsync(s => s.OrderId == orderId && !s.IsDeleted);

            if (supply == null)
                return dto;

            dto.SupplyDate = supply.SupplyDate;
            dto.SubmissionStatus = supply.SubmissionStatus;
            dto.FulfillmentStatus = supply.FulfillmentStatus;
            dto.ReceiverName = supply.RecieverName;
            dto.ReceiverMilitaryId = supply.RecieverMilitaryId;
            dto.ReceiverRankName = supply.ReceiverRank?.NameEn ?? supply.ReceiverRank?.NameAr;
            dto.Notes = supply.Notes;

            var details = supply.SupplyDetails?.Where(sd => !sd.IsDeleted).ToList() ?? new List<SupplyDetail>();
            if (details.Count == 0)
                return dto;

            var requestedByItem = order.RequestItems?
                .Where(ri => !ri.IsDeleted)
                .GroupBy(ri => ri.ItemId)
                .ToDictionary(g => g.Key, g => g.Sum(ri => ri.Quantity)) ?? new Dictionary<long, long>();

            var depotLookup = await BuildDepotLookupForLinesAsync(details);

            foreach (var d in details)
            {
                var lotKey = (d.Lot ?? string.Empty).Trim();
                var mapKey = $"{d.ItemId}|{lotKey}";
                depotLookup.TryGetValue(mapKey, out var dep);
                requestedByItem.TryGetValue(d.ItemId, out var reqQty);

                dto.Lines.Add(new WorkflowSupplySummaryLineDto
                {
                    ItemId = d.ItemId,
                    ItemName = d.Item?.Name ?? string.Empty,
                    ItemNo = d.Item?.ItemNo,
                    RequestedQuantity = reqQty,
                    SuppliedQuantity = d.Quantity,
                    Lot = d.Lot ?? string.Empty,
                    DepotId = dep?.DepotId,
                    DepotName = dep?.DepotName,
                    DepotCode = dep?.DepotCode,
                    Notes = d.Notes
                });
            }

            return dto;
        }

        private async Task<Dictionary<string, DepotRow>> BuildDepotLookupForLinesAsync(List<SupplyDetail> details)
        {
            var keys = details
                .Select(d => new { d.ItemId, Lot = (d.Lot ?? string.Empty).Trim() })
                .Distinct()
                .ToList();
            if (keys.Count == 0)
                return new Dictionary<string, DepotRow>();

            var itemIds = keys.Select(k => k.ItemId).Distinct().ToList();

            var candidates = await (
                from id in _context.Set<InventoryDetail>().AsNoTracking()
                join inv in _context.Set<InventoryRecord>().AsNoTracking() on id.InventoryId equals inv.Id
                join dep in _context.Set<Depot>().AsNoTracking() on inv.DepoId equals dep.Id
                where itemIds.Contains(id.ItemId) && !inv.IsDeleted && !dep.IsDeleted
                select new { id.ItemId, Lot = (id.Lot ?? string.Empty).Trim(), inv.DepoId, DepotName = dep.NameEn, DepotCode = dep.Code }
            ).ToListAsync();

            var result = new Dictionary<string, DepotRow>(StringComparer.Ordinal);
            foreach (var k in keys)
            {
                var keyStr = $"{k.ItemId}|{k.Lot}";
                if (result.ContainsKey(keyStr))
                    continue;
                var match = candidates.FirstOrDefault(c => c.ItemId == k.ItemId && string.Equals(c.Lot, k.Lot, StringComparison.Ordinal));
                if (match != null)
                    result[keyStr] = new DepotRow(match.DepoId, match.DepotName, match.DepotCode);
            }

            return result;
        }

        private sealed record DepotRow(long DepotId, string? DepotName, string? DepotCode);
    }
}
