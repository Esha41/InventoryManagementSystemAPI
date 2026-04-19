using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Monitoring;
using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MonitoringController : ApiControllerBase
    {
        private readonly ILowStockMonitoringService _lowStockMonitoringService;
        private readonly IExpiringLotMonitoringService _expiringLotMonitoringService;

        public MonitoringController(
            ILowStockMonitoringService lowStockMonitoringService,
            IExpiringLotMonitoringService expiringLotMonitoringService)
        {
            _lowStockMonitoringService = lowStockMonitoringService;
            _expiringLotMonitoringService = expiringLotMonitoringService;
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
    }
}
