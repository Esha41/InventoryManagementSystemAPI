using System.Collections.Generic;
using System.Net;
using Ettad.CrossCutting.Common.Security;
using Ettad.CrossCutting.Comman.Models;
using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.Inventory.Service.Monitoring.Interfaces;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ettad.Inventory.Service.Monitoring.Interfaces;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MonitoringController : ApiControllerBase
    {
        private readonly ILowStockMonitoringService _lowStockMonitoringService;
        private readonly ICriticalStockMonitoringService _criticalStockMonitoringService;
        private readonly IExpiringLotMonitoringService _expiringLotMonitoringService;
        private readonly IInventoryDashboardMonitoringService _inventoryDashboardMonitoringService;

        public MonitoringController(
            ILowStockMonitoringService lowStockMonitoringService,
            ICriticalStockMonitoringService criticalStockMonitoringService,
            IExpiringLotMonitoringService expiringLotMonitoringService,
            IInventoryDashboardMonitoringService inventoryDashboardMonitoringService)
        {
            _lowStockMonitoringService = lowStockMonitoringService;
            _criticalStockMonitoringService = criticalStockMonitoringService;
            _expiringLotMonitoringService = expiringLotMonitoringService;
            _inventoryDashboardMonitoringService = inventoryDashboardMonitoringService;
        }

        /// <summary>
        /// Get the count of items that are below minimum stock level, optionally filtered by depot.
        /// </summary>
        [HttpGet("low-stock/count")]
        [ProducesResponseType(typeof(APIOperationResponse<int>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetLowStockItemsCount([FromQuery] long? depotId = null, [FromQuery] List<long>? depotIds = null)
        {
            var result = await _lowStockMonitoringService.GetLowStockItemsCountAsync(depotId, depotIds);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Low-stock items with server-side paging (same contract as <c>api/Ammunition/Paginated</c>).
        /// Body: <see cref="PagedListRequest"/> (page, pageSize, optional filter). Depot query matches <c>low-stock/count</c>.
        /// </summary>
        [HttpPost("low-stock/Paginated")]
        [ProducesResponseType(typeof(APIOperationResponse<PaginatedList<LowStockItemDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetLowStockItemsPaginated(
            [FromBody] PagedListRequest? request,
            [FromQuery] long? depotId = null,
            [FromQuery] List<long>? depotIds = null)
        {
            request ??= new PagedListRequest();
            var result = await _lowStockMonitoringService.GetLowStockItemsPaginatedAsync(request, depotId, depotIds);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get the count of items at or below configured critical stock level, optionally filtered by depot.
        /// </summary>
        [HttpGet("critical-stock/count")]
        [ProducesResponseType(typeof(APIOperationResponse<int>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetCriticalStockItemsCount([FromQuery] long? depotId = null, [FromQuery] List<long>? depotIds = null)
        {
            var result = await _criticalStockMonitoringService.GetCriticalStockItemsCountAsync(depotId, depotIds);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get the list of items at or below configured critical stock level with their details.
        /// </summary>
        [HttpGet("critical-stock")]
        [ProducesResponseType(typeof(APIOperationResponse<List<CriticalStockItemDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetCriticalStockItems()
        {
            var result = await _criticalStockMonitoringService.GetCriticalStockItemsAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get the count of lots that are about to expire in the next 30 days, optionally filtered by depot.
        /// </summary>
        [HttpGet("expiring-lots/count")]
        [ProducesResponseType(typeof(APIOperationResponse<int>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetExpiringLotsCount([FromQuery] long? depotId = null, [FromQuery] List<long>? depotIds = null)
        {
            var result = await _expiringLotMonitoringService.GetExpiringLotsCountAsync(depotId, depotIds);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Expiring lots (next 30 days) with server-side paging (same contract as <c>api/Ammunition/Paginated</c>).
        /// Body: <see cref="PagedListRequest"/>. Depot query matches <c>expiring-lots/count</c>.
        /// </summary>
        [HttpPost("expiring-lots/Paginated")]
        [ProducesResponseType(typeof(APIOperationResponse<PaginatedList<ExpiringLotDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetExpiringLotsPaginated(
            [FromBody] PagedListRequest? request,
            [FromQuery] long? depotId = null,
            [FromQuery] List<long>? depotIds = null)
        {
            request ??= new PagedListRequest();
            var result = await _expiringLotMonitoringService.GetExpiringLotsPaginatedAsync(request, depotId, depotIds);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Weapon asset headline metrics (depot-scoped), aligned with inventory summary depot filtering.
        /// </summary>
        [HttpGet("dashboard/weapon-assets")]
        [ProducesResponseType(typeof(APIOperationResponse<WeaponAssetDashboardDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetWeaponAssetDashboard([FromQuery] long? depotId = null, [FromQuery] List<long>? depotIds = null)
        {
            var result = await _inventoryDashboardMonitoringService.GetWeaponAssetDashboardAsync(depotId, depotIds);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Supply pipeline headline metrics (draft supplies, orders awaiting fulfillment).
        /// </summary>
        [HttpGet("dashboard/pipeline")]
        [ProducesResponseType(typeof(APIOperationResponse<InventoryPipelineDashboardDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetPipelineDashboard([FromQuery] long? depotId = null, [FromQuery] List<long>? depotIds = null)
        {
            var result = await _inventoryDashboardMonitoringService.GetPipelineDashboardAsync(depotId, depotIds);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Headline counts for inventory dashboard stat cards (alerts, totals, items-by-type).
        /// </summary>
        [HttpGet("dashboard/inventory-headline-metrics")]
        [ProducesResponseType(typeof(APIOperationResponse<InventoryHeadlineMetricsDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize(
            "Permissions.Inventory.View",
            "Permissions.Inventory.Page",
            "Permissions.Analytics.View",
            "Permissions.Analytics.Page")]
        public async Task<IActionResult> GetInventoryHeadlineMetrics([FromQuery] long? depotId = null, [FromQuery] List<long>? depotIds = null)
        {
            var result = await _inventoryDashboardMonitoringService.GetInventoryHeadlineMetricsAsync(depotId, depotIds);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Combined weapon + pipeline dashboard metrics in one round-trip.
        /// </summary>
        [HttpGet("dashboard/inventory-summary")]
        [ProducesResponseType(typeof(APIOperationResponse<InventoryDashboardSummaryDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize(
            "Permissions.Inventory.View",
            "Permissions.Inventory.Page",
            "Permissions.Analytics.View",
            "Permissions.Analytics.Page")]
        public async Task<IActionResult> GetInventoryDashboardSummary([FromQuery] long? depotId = null, [FromQuery] List<long>? depotIds = null)
        {
            var result = await _inventoryDashboardMonitoringService.GetInventoryDashboardSummaryAsync(depotId, depotIds);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Draft Supply and AssetSupply rows (same depot rules as pipeline draft count).
        /// </summary>
        [HttpGet("dashboard/draft-supplies")]
        [ProducesResponseType(typeof(APIOperationResponse<List<DraftSupplyListItemDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetDraftSuppliesList([FromQuery] long? depotId = null, [FromQuery] List<long>? depotIds = null)
        {
            var result = await _inventoryDashboardMonitoringService.GetDraftSuppliesListAsync(depotId, depotIds);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Approved orders not fully fulfilled (same rules as pipeline awaiting count).
        /// </summary>
        [HttpGet("dashboard/orders-awaiting-fulfillment")]
        [ProducesResponseType(typeof(APIOperationResponse<List<OrderAwaitingFulfillmentListItemDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetOrdersAwaitingFulfillmentList([FromQuery] long? depotId = null, [FromQuery] List<long>? depotIds = null)
        {
            var result = await _inventoryDashboardMonitoringService.GetOrdersAwaitingFulfillmentListAsync(depotId, depotIds);
            return ProcessResponse(result);
        }
    }
}
