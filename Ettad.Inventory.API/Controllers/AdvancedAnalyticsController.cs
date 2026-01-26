using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.AdvancedAnalytics;
using Ettad.Inventory.Service.AdvancedAnalytics.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Inventory.API.Controllers
{
    /// <summary>
    /// Advanced Analytics Controller
    /// Provides comprehensive KPIs and analytics for command staff decision-making
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdvancedAnalyticsController : ApiControllerBase
    {
        private readonly IAdvancedAnalyticsService _advancedAnalyticsService;

        public AdvancedAnalyticsController(IAdvancedAnalyticsService advancedAnalyticsService)
        {
            _advancedAnalyticsService = advancedAnalyticsService;
        }

        /// <summary>
        /// Get complete Advanced Analytics Dashboard data
        /// </summary>
        [HttpGet("dashboard")]
        [ProducesResponseType(typeof(APIOperationResponse<AdvancedAnalyticsDashboardDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("analytics.page", "analytics.view", "advancedAnalytics.page", "advancedAnalytics.view")]
        public async Task<IActionResult> GetDashboard(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int? warehouseId = null)
        {
            var result = await _advancedAnalyticsService.GetDashboardAsync(startDate, endDate, warehouseId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get Mission Readiness Rate KPI
        /// </summary>
        [HttpGet("mission-readiness")]
        [ProducesResponseType(typeof(APIOperationResponse<MissionReadinessDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("analytics.page", "analytics.view", "advancedAnalytics.page", "advancedAnalytics.view")]
        public async Task<IActionResult> GetMissionReadiness(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var result = await _advancedAnalyticsService.GetMissionReadinessAsync(startDate, endDate);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get Stock Availability KPI
        /// </summary>
        [HttpGet("stock-availability")]
        [ProducesResponseType(typeof(APIOperationResponse<StockAvailabilityDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("analytics.page", "analytics.view", "advancedAnalytics.page", "advancedAnalytics.view")]
        public async Task<IActionResult> GetStockAvailability([FromQuery] int? warehouseId = null)
        {
            var result = await _advancedAnalyticsService.GetStockAvailabilityAsync(warehouseId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get Order Cycle Time KPI
        /// </summary>
        [HttpGet("order-cycle-time")]
        [ProducesResponseType(typeof(APIOperationResponse<OrderCycleTimeDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("analytics.page", "analytics.view", "advancedAnalytics.page", "advancedAnalytics.view")]
        public async Task<IActionResult> GetOrderCycleTime(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var result = await _advancedAnalyticsService.GetOrderCycleTimeAsync(startDate, endDate);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get Consumption Rate vs Forecast KPI
        /// </summary>
        [HttpGet("consumption-forecast")]
        [ProducesResponseType(typeof(APIOperationResponse<ConsumptionForecastDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("analytics.page", "analytics.view", "advancedAnalytics.page", "advancedAnalytics.view")]
        public async Task<IActionResult> GetConsumptionForecast(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var result = await _advancedAnalyticsService.GetConsumptionForecastAsync(startDate, endDate);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Drill down into specific KPI data for detailed analysis
        /// </summary>
        [HttpPost("drill-down")]
        [ProducesResponseType(typeof(APIOperationResponse<object>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("analytics.page", "analytics.view", "advancedAnalytics.page", "advancedAnalytics.view")]
        public async Task<IActionResult> DrillDown([FromBody] DrillDownRequestDto request)
        {
            var result = await _advancedAnalyticsService.DrillDownAsync(request);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get Order Status Distribution Chart
        /// </summary>
        [HttpGet("order-status-distribution")]
        [ProducesResponseType(typeof(APIOperationResponse<OrderStatusDistributionDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("analytics.page", "analytics.view", "advancedAnalytics.page", "advancedAnalytics.view")]
        public async Task<IActionResult> GetOrderStatusDistribution(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var result = await _advancedAnalyticsService.GetOrderStatusDistributionAsync(startDate, endDate);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get Request Trends Over Time
        /// </summary>
        [HttpGet("request-trends")]
        [ProducesResponseType(typeof(APIOperationResponse<RequestTrendsDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("analytics.page", "analytics.view", "advancedAnalytics.page", "advancedAnalytics.view")]
        public async Task<IActionResult> GetRequestTrends(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var result = await _advancedAnalyticsService.GetRequestTrendsAsync(startDate, endDate);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get Inventory Value by Warehouse
        /// </summary>
        [HttpGet("inventory-value")]
        [ProducesResponseType(typeof(APIOperationResponse<InventoryValueDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("analytics.page", "analytics.view", "advancedAnalytics.page", "advancedAnalytics.view")]
        public async Task<IActionResult> GetInventoryValue([FromQuery] int? warehouseId = null)
        {
            var result = await _advancedAnalyticsService.GetInventoryValueAsync(warehouseId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get Asset Assignment Status
        /// </summary>
        [HttpGet("asset-assignment-status")]
        [ProducesResponseType(typeof(APIOperationResponse<AssetAssignmentStatusDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("analytics.page", "analytics.view", "advancedAnalytics.page", "advancedAnalytics.view")]
        public async Task<IActionResult> GetAssetAssignmentStatus()
        {
            var result = await _advancedAnalyticsService.GetAssetAssignmentStatusAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get Supply Fulfillment Status
        /// </summary>
        [HttpGet("supply-fulfillment-status")]
        [ProducesResponseType(typeof(APIOperationResponse<SupplyFulfillmentStatusDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("analytics.page", "analytics.view", "advancedAnalytics.page", "advancedAnalytics.view")]
        public async Task<IActionResult> GetSupplyFulfillmentStatus(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var result = await _advancedAnalyticsService.GetSupplyFulfillmentStatusAsync(startDate, endDate);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get Department Request Volume
        /// </summary>
        [HttpGet("department-request-volume")]
        [ProducesResponseType(typeof(APIOperationResponse<DepartmentRequestVolumeDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("analytics.page", "analytics.view", "advancedAnalytics.page", "advancedAnalytics.view")]
        public async Task<IActionResult> GetDepartmentRequestVolume(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var result = await _advancedAnalyticsService.GetDepartmentRequestVolumeAsync(startDate, endDate);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get Notification Statistics
        /// </summary>
        [HttpGet("notification-statistics")]
        [ProducesResponseType(typeof(APIOperationResponse<NotificationStatisticsDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("analytics.page", "analytics.view", "advancedAnalytics.page", "advancedAnalytics.view")]
        public async Task<IActionResult> GetNotificationStatistics(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var result = await _advancedAnalyticsService.GetNotificationStatisticsAsync(startDate, endDate);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get Import/Export Statistics
        /// </summary>
        [HttpGet("import-export-statistics")]
        [ProducesResponseType(typeof(APIOperationResponse<ImportExportStatisticsDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("analytics.page", "analytics.view", "advancedAnalytics.page", "advancedAnalytics.view")]
        public async Task<IActionResult> GetImportExportStatistics(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var result = await _advancedAnalyticsService.GetImportExportStatisticsAsync(startDate, endDate);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get User Login Analytics
        /// </summary>
        [HttpGet("user-login-analytics")]
        [ProducesResponseType(typeof(APIOperationResponse<UserLoginAnalyticsDto>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("analytics.page", "analytics.view", "advancedAnalytics.page", "advancedAnalytics.view")]
        public async Task<IActionResult> GetUserLoginAnalytics(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var result = await _advancedAnalyticsService.GetUserLoginAnalyticsAsync(startDate, endDate);
            return ProcessResponse(result);
        }
    }
}
