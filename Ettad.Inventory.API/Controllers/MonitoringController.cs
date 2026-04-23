using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Monitoring.Dtos;
using System.Collections.Generic;
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
        private readonly IExpiringLotMonitoringService _expiringLotMonitoringService;
        private readonly IInventoryDashboardMonitoringService _inventoryDashboardMonitoringService;

        public MonitoringController(
            ILowStockMonitoringService lowStockMonitoringService,
            IExpiringLotMonitoringService expiringLotMonitoringService,
            IInventoryDashboardMonitoringService inventoryDashboardMonitoringService)
        {
            _lowStockMonitoringService = lowStockMonitoringService;
            _expiringLotMonitoringService = expiringLotMonitoringService;
            _inventoryDashboardMonitoringService = inventoryDashboardMonitoringService;
        }

        /// <summary>
        /// Get the count of items that are below minimum stock level, optionally filtered by depot.
        /// </summary>
        [HttpGet("low-stock/count")]
        [ProducesResponseType(typeof(APIOperationResponse<int>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetLowStockItemsCount([FromQuery] long? depotId = null)
        {
            var result = await _lowStockMonitoringService.GetLowStockItemsCountAsync(depotId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get the list of items that are below minimum stock level with their details
        /// </summary>
        [HttpGet("low-stock")]
        [ProducesResponseType(typeof(APIOperationResponse<List<LowStockItemDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetLowStockItems()
        {
            var result = await _lowStockMonitoringService.GetLowStockItemsAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get the count of lots that are about to expire in the next 30 days, optionally filtered by depot.
        /// </summary>
        [HttpGet("expiring-lots/count")]
        [ProducesResponseType(typeof(APIOperationResponse<int>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetExpiringLotsCount([FromQuery] long? depotId = null)
        {
            var result = await _expiringLotMonitoringService.GetExpiringLotsCountAsync(depotId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get the list of lots that are about to expire in the next 30 days with their details (item and depot information)
        /// </summary>
        [HttpGet("expiring-lots")]
        [ProducesResponseType(typeof(APIOperationResponse<List<ExpiringLotDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
        public async Task<IActionResult> GetExpiringLots()
        {
            var result = await _expiringLotMonitoringService.GetExpiringLotsAsync();
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
        /// Combined weapon + pipeline dashboard metrics in one round-trip.
        /// </summary>
        [HttpGet("dashboard/inventory-summary")]
        [ProducesResponseType(typeof(APIOperationResponse<InventoryDashboardSummaryDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View", "Permissions.Inventory.Page")]
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
