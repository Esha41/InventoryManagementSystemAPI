using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using InventoryRecord = Ettad.Data.Entities.Inventory;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Inventory.Service.AssetSupply.Dtos;
using Ettad.Inventory.Service.AssetSupply.Interfaces;
using Ettad.RequestManagement.Service.SupplyManagement.Dtos;
using Ettad.RequestManagement.Service.SupplyManagement.Interfaces;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.RequestManagement.Service.SupplyManagement.Services
{
    public class WorkflowSupplySummaryService : IWorkflowSupplySummaryService
    {
        private static readonly string[] OrderSummaryIncludes =
        [
            nameof(Order.RequestItems),
            $"{nameof(Order.RequestItems)}.{nameof(RequestItem.Item)}",
        ];

        private static readonly string[] WeaponSupplySelectionIncludes =
        [
            nameof(WeaponSupplySelection.Item),
            nameof(WeaponSupplySelection.Depot),
            nameof(WeaponSupplySelection.Batch),
        ];

        private static readonly string[] SupplySummaryIncludes =
        [
            nameof(Supply.ReceiverEmployee),
            $"{nameof(Supply.ReceiverEmployee)}.{nameof(Employee.Rank)}",
            nameof(Supply.SupplyDetails),
            $"{nameof(Supply.SupplyDetails)}.{nameof(SupplyDetail.Item)}",
        ];

        private static readonly string[] AssetDepotBatchIncludes =
        [
            nameof(Asset.Depot),
            nameof(Asset.Batch),
        ];

        private static readonly string[] InventoryDetailDepotIncludes =
        [
            nameof(InventoryDetail.Inventory),
            $"{nameof(InventoryDetail.Inventory)}.{nameof(InventoryRecord.Depo)}",
        ];

        private readonly ICrossCuttingRepository<Order> _orderRepository;
        private readonly ICrossCuttingRepository<WeaponSupplySelection> _weaponSupplySelectionRepository;
        private readonly ICrossCuttingRepository<Asset> _assetRepository;
        private readonly ICrossCuttingRepository<Employee> _employeeRepository;
        private readonly ICrossCuttingRepository<Supply> _supplyRepository;
        private readonly ICrossCuttingRepository<OrderItemHistory> _orderItemHistoryRepository;
        private readonly ICrossCuttingRepository<InventoryDetail> _inventoryDetailRepository;
        private readonly ICrossCuttingRepository<ApplicationUser> _applicationUserRepository;
        private readonly IAssetSupplyService _assetSupplyService;
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<WorkflowSupplySummaryService> _logger;

        public WorkflowSupplySummaryService(
            ICrossCuttingRepository<Order> orderRepository,
            ICrossCuttingRepository<WeaponSupplySelection> weaponSupplySelectionRepository,
            ICrossCuttingRepository<Asset> assetRepository,
            ICrossCuttingRepository<Employee> employeeRepository,
            ICrossCuttingRepository<Supply> supplyRepository,
            ICrossCuttingRepository<OrderItemHistory> orderItemHistoryRepository,
            ICrossCuttingRepository<InventoryDetail> inventoryDetailRepository,
            ICrossCuttingRepository<ApplicationUser> applicationUserRepository,
            IAssetSupplyService assetSupplyService,
            IFileUploadService fileUploadService,
            ILogger<WorkflowSupplySummaryService> logger)
        {
            _orderRepository = orderRepository;
            _weaponSupplySelectionRepository = weaponSupplySelectionRepository;
            _assetRepository = assetRepository;
            _employeeRepository = employeeRepository;
            _supplyRepository = supplyRepository;
            _orderItemHistoryRepository = orderItemHistoryRepository;
            _inventoryDetailRepository = inventoryDetailRepository;
            _applicationUserRepository = applicationUserRepository;
            _assetSupplyService = assetSupplyService;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        public async Task<APIOperationResponse<WorkflowSupplySummaryDto>> GetSummaryForOrderAsync(long orderId)
        {
            try
            {
                var order = await _orderRepository.FindOneAsync(
                    o => o.Id == orderId && !o.IsDeleted,
                    false,
                    OrderSummaryIncludes);

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
            var selections = await _weaponSupplySelectionRepository
                .Find(ws => ws.OrderId == order.Id, false, WeaponSupplySelectionIncludes)
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
                    DepotNameEn = s.Depot?.NameEn,
                    DepotNameAr = s.Depot?.NameAr,
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
                Phase = "Supplied",
                Files = data.Files ?? new List<FileUploadDto>()
            };

            var selections = await _weaponSupplySelectionRepository
                .Find(ws => ws.OrderId == order.Id, false, WeaponSupplySelectionIncludes)
                .ToListAsync();

            foreach (var s in selections)
            {
                dto.SelectionLines.Add(new WeaponSelectionLineDto
                {
                    ItemId = s.ItemId,
                    ItemName = s.Item?.Name ?? string.Empty,
                    DepotId = s.DepotId,
                    DepotName = s.Depot?.NameEn,
                    DepotNameEn = s.Depot?.NameEn,
                    DepotNameAr = s.Depot?.NameAr,
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
            var assetRows = await _assetRepository
                .Find(a => assetIds.Contains(a.Id) && !a.IsDeleted, false, AssetDepotBatchIncludes)
                .ToListAsync();

            var assetInfo = assetRows.ToDictionary(
                a => a.Id,
                a => new
                {
                    a.Id,
                    a.SerialNumber,
                    a.DepotId,
                    DepotName = a.Depot?.NameEn,
                    DepotNameEn = a.Depot?.NameEn,
                    DepotNameAr = a.Depot?.NameAr,
                    DepotCode = a.Depot?.Code,
                    BatchNumber = a.Batch?.BatchNumber
                });

            var custodianIds = details
                .Where(dl => dl.CustodianId != null)
                .Select(dl => long.TryParse(dl.CustodianId, out var cid) ? cid : 0)
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            var employeeNames = new Dictionary<long, string>();
            if (custodianIds.Count > 0)
            {
                var employees = await _employeeRepository
                    .Find(e => custodianIds.Contains(e.Id) && !e.IsDeleted)
                    .ToListAsync();
                foreach (var e in employees)
                    employeeNames[e.Id] = e.NameEn ?? e.NameAr ?? string.Empty;
            }

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
                    DepotName = asset?.DepotNameEn,
                    DepotNameEn = asset?.DepotNameEn,
                    DepotNameAr = asset?.DepotNameAr,
                    DepotCode = asset?.DepotCode,
                    BatchNumber = asset?.BatchNumber,
                    AssigneeName = assigneeName,
                    Notes = line.Notes,
                    Accessories = (line.Accessories ?? new List<AssetSupplyAccessoryDetailDto>())
                        .Select(a => new WeaponSuppliedAccessoryDto
                        {
                            AccessoryId = a.AccessoryId,
                            ItemNo = a.ItemNo,
                            Name = a.Name,
                            NameAr = a.NameAr,
                            DefaultQuantity = a.DefaultQuantity,
                            SuppliedQuantity = a.SuppliedQuantity
                        })
                        .ToList()
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

            var supply = await _supplyRepository.FindOneAsync(
                s => s.OrderId == orderId && !s.IsDeleted,
                false,
                SupplySummaryIncludes);

            if (supply == null)
                return dto;

            dto.SupplyDate = supply.SupplyDate;
            dto.SubmissionStatus = supply.SubmissionStatus;
            dto.FulfillmentStatus = supply.FulfillmentStatus;
            dto.ReceiverName = supply.ReceiverEmployee?.NameEn ?? supply.ReceiverEmployee?.NameAr;
            dto.ReceiverMilitaryId = supply.ReceiverEmployee?.MilitaryId;
            dto.ReceiverRankName = supply.ReceiverEmployee?.Rank?.NameEn ?? supply.ReceiverEmployee?.Rank?.NameAr;
            dto.Notes = supply.Notes;

            var filesResult = await _fileUploadService.GetByEntityAsync(FileEntityType.Supply, supply.Id);
            if (filesResult.Succeeded && filesResult.Data != null)
                dto.Files = filesResult.Data;

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
                    DepotName = dep?.DepotNameEn,
                    DepotNameEn = dep?.DepotNameEn,
                    DepotNameAr = dep?.DepotNameAr,
                    DepotCode = dep?.DepotCode,
                    Notes = d.Notes
                });
            }

            return dto;
        }

        private async Task<Dictionary<long, (long RequestedOriginal, long Approved)>> BuildRequestedApprovedByItemAsync(
            long orderId,
            List<RequestItem> requestItems)
        {
            var result = new Dictionary<long, (long, long)>();
            if (requestItems.Count == 0)
                return result;

            var histories = await _orderItemHistoryRepository
                .Find(h => h.OrderId == orderId && !h.IsDeleted && h.RequestItemId != null)
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

            var candidates = await _inventoryDetailRepository
                .Find(id => itemIds.Contains(id.ItemId), false, InventoryDetailDepotIncludes)
                .Where(id =>
                    id.Inventory != null &&
                    !id.Inventory.IsDeleted &&
                    id.Inventory.Depo != null &&
                    !id.Inventory.Depo.IsDeleted)
                .Select(id => new
                {
                    id.ItemId,
                    Lot = (id.Lot ?? string.Empty).Trim(),
                    id.Inventory!.DepoId,
                    DepotNameEn = id.Inventory.Depo!.NameEn,
                    DepotNameAr = id.Inventory.Depo!.NameAr,
                    DepotCode = id.Inventory.Depo.Code
                })
                .ToListAsync();

            var result = new Dictionary<string, DepotRow>(StringComparer.Ordinal);
            foreach (var k in keys)
            {
                var keyStr = $"{k.ItemId}|{k.Lot}";
                if (result.ContainsKey(keyStr))
                    continue;
                var match = candidates.FirstOrDefault(c => c.ItemId == k.ItemId && string.Equals(c.Lot, k.Lot, StringComparison.Ordinal));
                if (match != null)
                    result[keyStr] = new DepotRow(match.DepoId, match.DepotNameEn, match.DepotNameAr, match.DepotCode);
            }

            return result;
        }

        private async Task<string?> ResolveRequesterNameAsync(Order order)
        {
            if (string.IsNullOrEmpty(order.RequesterId))
                return null;
            var user = await _applicationUserRepository.FindOneAsync(u => u.Id == order.RequesterId);
            return user?.FullNameEN ?? user?.FullNameAR ?? user?.UserName;
        }

        private sealed record DepotRow(long DepotId, string? DepotNameEn, string? DepotNameAr, string? DepotCode);
    }
}
