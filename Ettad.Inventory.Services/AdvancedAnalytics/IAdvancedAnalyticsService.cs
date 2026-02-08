using Ettad.Inventory.Service.AdvancedAnalytics.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.AdvancedAnalytics
{
    /// <summary>
    /// Advanced Analytics Service Interface
    /// Provides comprehensive KPIs and analytics for command staff
    /// </summary>
    public interface IAdvancedAnalyticsService
    {
        /// <summary>
        /// Get complete Advanced Analytics Dashboard data
        /// </summary>
        Task<APIOperationResponse<AdvancedAnalyticsDashboardDto>> GetDashboardAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? warehouseId = null);

        /// <summary>
        /// Get Mission Readiness Rate KPI
        /// </summary>
        Task<APIOperationResponse<MissionReadinessDto>> GetMissionReadinessAsync(
            DateTime? startDate = null,
            DateTime? endDate = null);

        /// <summary>
        /// Get Stock Availability KPI
        /// </summary>
        Task<APIOperationResponse<StockAvailabilityDto>> GetStockAvailabilityAsync(
            int? warehouseId = null);

        /// <summary>
        /// Get Order Cycle Time KPI
        /// </summary>
        Task<APIOperationResponse<OrderCycleTimeDto>> GetOrderCycleTimeAsync(
            DateTime? startDate = null,
            DateTime? endDate = null);

        /// <summary>
        /// Get Consumption Rate vs Forecast KPI
        /// </summary>
        Task<APIOperationResponse<ConsumptionForecastDto>> GetConsumptionForecastAsync(
            DateTime? startDate = null,
            DateTime? endDate = null);

        /// <summary>
        /// Drill down into specific KPI data
        /// </summary>
        Task<APIOperationResponse<object>> DrillDownAsync(DrillDownRequestDto request);

        /// <summary>
        /// Get Order Status Distribution
        /// </summary>
        Task<APIOperationResponse<OrderStatusDistributionDto>> GetOrderStatusDistributionAsync(
            DateTime? startDate = null,
            DateTime? endDate = null);

        /// <summary>
        /// Get Request Trends Over Time
        /// </summary>
        Task<APIOperationResponse<RequestTrendsDto>> GetRequestTrendsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null);

        /// <summary>
        /// Get Inventory Value by Warehouse
        /// </summary>
        Task<APIOperationResponse<InventoryValueDto>> GetInventoryValueAsync(int? warehouseId = null);

        /// <summary>
        /// Get Asset Assignment Status
        /// </summary>
        Task<APIOperationResponse<AssetAssignmentStatusDto>> GetAssetAssignmentStatusAsync();

        /// <summary>
        /// Get Supply Fulfillment Status
        /// </summary>
        Task<APIOperationResponse<SupplyFulfillmentStatusDto>> GetSupplyFulfillmentStatusAsync(
            DateTime? startDate = null,
            DateTime? endDate = null);

        /// <summary>
        /// Get Department Request Volume
        /// </summary>
        Task<APIOperationResponse<DepartmentRequestVolumeDto>> GetDepartmentRequestVolumeAsync(
            DateTime? startDate = null,
            DateTime? endDate = null);

        /// <summary>
        /// Get Notification Statistics
        /// </summary>
        Task<APIOperationResponse<NotificationStatisticsDto>> GetNotificationStatisticsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null);

        /// <summary>
        /// Get Import/Export Statistics
        /// </summary>
        Task<APIOperationResponse<ImportExportStatisticsDto>> GetImportExportStatisticsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null);

        /// <summary>
        /// Get User Login Analytics
        /// </summary>
        Task<APIOperationResponse<UserLoginAnalyticsDto>> GetUserLoginAnalyticsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null);
    }
}
