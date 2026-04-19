using Ettad.Comman.Idenitity;
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

                if (order.Status is RequestStatus.Rejected or RequestStatus.Cancelled)
                    return APIOperationResponse<WorkflowSupplySummaryDto>.Fail(ResponseType.Forbidden, "Order is not eligible for supply summary");

                var weaponOnly = IsWeaponOnlyOrder(order);

                if (weaponOnly)
                {
                    var assetResult = await _assetSupplyService.GetByOrderIdAsync(orderId);
                    if (assetResult.Succeeded && assetResult.Data != null)
                        return APIOperationResponse<WorkflowSupplySummaryDto>.Success(await BuildFromAssetSupplyAsync(order, assetResult.Data));

                    // No asset supply yet — check if depot/batch selections exist
                    var selectionDto = await BuildFromWeaponSelectionsAsync(order);
                    if (selectionDto != null)
                        return APIOperationResponse<WorkflowSupplySummaryDto>.Success(selectionDto);
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

        private async Task<WorkflowSupplySummaryDto?> BuildFromWeaponSelectionsAsync(Order order)
        {
            var selections = await _context.Set<WeaponSupplySelection>()
                .AsNoTracking()
                .Include(ws => ws.Item)
                .Include(ws => ws.Depot)
                .Include(ws => ws.Batch)
                .Where(ws => ws.OrderId == order.Id)
                .ToListAsync();

            if (selections.Count == 0)
                return null;

            var dto = new WorkflowSupplySummaryDto
            {
                OrderId = order.Id,
                OrderSupplyDate = order.SupplyDate,
                IsWeaponOrder = true,
                IsOrderCompleted = order.Status == RequestStatus.Approved,
                Phase = "Selection"
            };

            foreach (var s in selections)
            {
                dto.SelectionLines.Add(new WeaponSelectionLineDto
                {
                    ItemId = s.ItemId,
                    ItemName = s.Item?.Name ?? string.Empty,
                    DepotId = s.DepotId,
                    DepotName = s.Depot?.NameEn,
                    DepotCode = s.Depot?.Code,
                    BatchId = s.BatchId,
                    BatchNumber = s.Batch?.BatchNumber ?? string.Empty,
                    SelectedQuantity = s.Quantity
                });
            }

            return dto;
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
                ReceiverName = data.ReceiverEmployee?.NameEn ?? data.ReceiverEmployee?.NameAr,
                ReceiverMilitaryId = data.ReceiverEmployee?.MilitaryId,
                ReceiverRankName = data.ReceiverEmployee?.Rank?.NameEn ?? data.ReceiverEmployee?.Rank?.NameAr,
                Notes = data.Notes,
                IsWeaponOrder = true,
                IsOrderCompleted = order.Status == RequestStatus.Approved,
                Phase = "Supplied"
            };

            // Keep depot/batch selections visible alongside the supplied assets
            var selections = await _context.Set<WeaponSupplySelection>()
                .AsNoTracking()
                .Include(ws => ws.Item)
                .Include(ws => ws.Depot)
                .Include(ws => ws.Batch)
                .Where(ws => ws.OrderId == order.Id)
                .ToListAsync();

            foreach (var s in selections)
            {
                dto.SelectionLines.Add(new WeaponSelectionLineDto
                {
                    ItemId = s.ItemId,
                    ItemName = s.Item?.Name ?? string.Empty,
                    DepotId = s.DepotId,
                    DepotName = s.Depot?.NameEn,
                    DepotCode = s.Depot?.Code,
                    BatchId = s.BatchId,
                    BatchNumber = s.Batch?.BatchNumber ?? string.Empty,
                    SelectedQuantity = s.Quantity
                });
            }

            var details = data.SupplyDetails ?? new List<AssetSupplyDetailDto>();
            if (details.Count == 0)
                return dto;

            var assetIds = details.Select(d => d.AssetId).Distinct().ToList();
            var assetInfo = await (
                from a in _context.Set<Asset>().AsNoTracking()
                join d in _context.Set<Depot>().AsNoTracking() on a.DepotId equals d.Id
                join b in _context.Set<Batch>().AsNoTracking() on a.BatchId equals b.Id into batches
                from b in batches.DefaultIfEmpty()
                where assetIds.Contains(a.Id) && !a.IsDeleted && !d.IsDeleted
                select new { a.Id, a.SerialNumber, DepotId = a.DepotId, DepotName = d.NameEn, DepotCode = d.Code, BatchNumber = b != null ? b.BatchNumber : null }
            ).ToDictionaryAsync(x => x.Id);

            // Load custodian (Employee) names for each detail line
            var custodianIds = details
                .Where(dl => dl.CustodianId != null)
                .Select(dl => long.TryParse(dl.CustodianId, out var cid) ? cid : 0)
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            var employeeNames = custodianIds.Count > 0
                ? await _context.Set<Employee>()
                    .AsNoTracking()
                    .Where(e => custodianIds.Contains(e.Id) && !e.IsDeleted)
                    .ToDictionaryAsync(e => e.Id, e => (e.NameEn ?? e.NameAr ?? string.Empty))
                : new Dictionary<long, string>();

            // Fallback: if no per-line custodian resolves, use the order requester name
            var requesterFallbackName = await ResolveRequesterNameAsync(order);

            foreach (var line in details)
            {
                assetInfo.TryGetValue(line.AssetId, out var asset);

                string? assigneeName = null;
                if (line.CustodianId != null && long.TryParse(line.CustodianId, out var empId) && employeeNames.TryGetValue(empId, out var name))
                    assigneeName = name;
                assigneeName ??= requesterFallbackName;

                dto.WeaponLines.Add(new WeaponSuppliedLineDto
                {
                    ItemId = line.ItemId,
                    ItemName = line.ItemName ?? string.Empty,
                    AssetId = line.AssetId,
                    SerialNumber = asset?.SerialNumber ?? line.AssetSerialNumber,
                    DepotId = asset?.DepotId,
                    DepotName = asset?.DepotName,
                    DepotCode = asset?.DepotCode,
                    BatchNumber = asset?.BatchNumber,
                    AssigneeName = assigneeName,
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
                IsWeaponOrder = isWeaponOrderFlag,
                IsOrderCompleted = order.Status == RequestStatus.Approved
            };

            var supply = await _context.Set<Supply>()
                .AsNoTracking()
                .Include(s => s.ReceiverEmployee)
                    .ThenInclude(e => e.Rank)
                .Include(s => s.SupplyDetails)
                .ThenInclude(sd => sd.Item)
                .FirstOrDefaultAsync(s => s.OrderId == orderId && !s.IsDeleted);

            if (supply == null)
                return dto;

            dto.SupplyDate = supply.SupplyDate;
            dto.SubmissionStatus = supply.SubmissionStatus;
            dto.FulfillmentStatus = supply.FulfillmentStatus;
            dto.ReceiverName = supply.ReceiverEmployee?.NameEn ?? supply.ReceiverEmployee?.NameAr;
            dto.ReceiverMilitaryId = supply.ReceiverEmployee?.MilitaryId;
            dto.ReceiverRankName = supply.ReceiverEmployee?.Rank?.NameEn ?? supply.ReceiverEmployee?.Rank?.NameAr;
            dto.Notes = supply.Notes;

            var details = supply.SupplyDetails?.Where(sd => !sd.IsDeleted).ToList() ?? new List<SupplyDetail>();
            if (details.Count == 0)
                return dto;

            var qtyByItem = await BuildRequestedApprovedByItemAsync(orderId, order.RequestItems?.Where(ri => !ri.IsDeleted).ToList() ?? new List<RequestItem>());

            var depotLookup = await BuildDepotLookupForLinesAsync(details);

            foreach (var d in details)
            {
                var lotKey = (d.Lot ?? string.Empty).Trim();
                var mapKey = $"{d.ItemId}|{lotKey}";
                depotLookup.TryGetValue(mapKey, out var dep);
                qtyByItem.TryGetValue(d.ItemId, out var qty);

                dto.Lines.Add(new WorkflowSupplySummaryLineDto
                {
                    ItemId = d.ItemId,
                    ItemName = d.Item?.Name ?? string.Empty,
                    ItemNo = d.Item?.ItemNo,
                    RequestedQuantity = qty.RequestedOriginal,
                    ApprovedQuantity = qty.Approved,
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

        /// <summary>
        /// Per item: original requested (from history when quantity was reduced) and approved quantity (current request lines).
        /// </summary>
        private async Task<Dictionary<long, (long RequestedOriginal, long Approved)>> BuildRequestedApprovedByItemAsync(
            long orderId,
            List<RequestItem> requestItems)
        {
            var result = new Dictionary<long, (long, long)>();
            if (requestItems.Count == 0)
                return result;

            var histories = await _context.OrderItemHistory
                .AsNoTracking()
                .Where(h => h.OrderId == orderId && !h.IsDeleted && h.RequestItemId != null)
                .ToListAsync();

            long MaxSnapshotForRequestItem(RequestItem ri)
            {
                long m = ri.Quantity;
                foreach (var h in histories.Where(x => x.RequestItemId == ri.Id))
                {
                    if (h.PreviousQuantity.HasValue)
                        m = Math.Max(m, h.PreviousQuantity.Value);
                    if (h.NewQuantity.HasValue)
                        m = Math.Max(m, h.NewQuantity.Value);
                    if (h.ApprovedQuantity.HasValue)
                        m = Math.Max(m, h.ApprovedQuantity.Value);
                }
                return m;
            }

            foreach (var g in requestItems.GroupBy(ri => ri.ItemId))
            {
                var approved = g.Sum(ri => ri.Quantity);
                var requested = g.Sum(ri => MaxSnapshotForRequestItem(ri));
                result[g.Key] = (requested, approved);
            }

            return result;
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

        private async Task<string?> ResolveRequesterNameAsync(Order order)
        {
            if (string.IsNullOrEmpty(order.RequesterId))
                return null;
            var user = await _context.Set<ApplicationUser>()
                .AsNoTracking()
                .Where(u => u.Id == order.RequesterId)
                .Select(u => new { u.FullNameEN, u.FullNameAR, u.UserName })
                .FirstOrDefaultAsync();
            return user?.FullNameEN ?? user?.FullNameAR ?? user?.UserName;
        }

        private sealed record DepotRow(long DepotId, string? DepotName, string? DepotCode);
    }
}
