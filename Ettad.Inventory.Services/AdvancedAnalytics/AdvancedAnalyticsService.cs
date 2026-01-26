using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Inventory.Service.AdvancedAnalytics.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Comman.Time;

namespace Ettad.Inventory.Service.AdvancedAnalytics
{
    /// <summary>
    /// Advanced Analytics Service Implementation
    /// Provides comprehensive KPIs for command staff decision-making
    /// </summary>
    public class AdvancedAnalyticsService : IAdvancedAnalyticsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AdvancedAnalyticsService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AdvancedAnalyticsService(
            ApplicationDbContext context,
            ILogger<AdvancedAnalyticsService> logger,
            IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<APIOperationResponse<AdvancedAnalyticsDashboardDto>> GetDashboardAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? warehouseId = null)
        {
            try
            {
                _logger.LogInformation("Fetching Advanced Analytics Dashboard data");

                var dashboard = new AdvancedAnalyticsDashboardDto
                {
                    MissionReadiness = await GetMissionReadinessInternalAsync(startDate, endDate),
                    StockAvailability = await GetStockAvailabilityInternalAsync(warehouseId),
                    OrderCycleTime = await GetOrderCycleTimeInternalAsync(startDate, endDate),
                    ConsumptionForecast = await GetConsumptionForecastInternalAsync(startDate, endDate),
                    OrderStatusDistribution = await GetOrderStatusDistributionInternalAsync(startDate, endDate),
                    RequestTrends = await GetRequestTrendsInternalAsync(startDate, endDate),
                    InventoryValue = await GetInventoryValueInternalAsync(warehouseId),
                    AssetAssignmentStatus = await GetAssetAssignmentStatusInternalAsync(),
                    SupplyFulfillmentStatus = await GetSupplyFulfillmentStatusInternalAsync(startDate, endDate),
                    DepartmentRequestVolume = await GetDepartmentRequestVolumeInternalAsync(startDate, endDate),
                    NotificationStatistics = await GetNotificationStatisticsInternalAsync(startDate, endDate),
                    ImportExportStatistics = await GetImportExportStatisticsInternalAsync(startDate, endDate),
                    LastUpdated = _dateTimeProvider.Now
                };

                return APIOperationResponse<AdvancedAnalyticsDashboardDto>.Success(dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Advanced Analytics Dashboard");
                return APIOperationResponse<AdvancedAnalyticsDashboardDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<MissionReadinessDto>> GetMissionReadinessAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                var result = await GetMissionReadinessInternalAsync(startDate, endDate);
                return APIOperationResponse<MissionReadinessDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Mission Readiness");
                return APIOperationResponse<MissionReadinessDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<StockAvailabilityDto>> GetStockAvailabilityAsync(
            int? warehouseId = null)
        {
            try
            {
                var result = await GetStockAvailabilityInternalAsync(warehouseId);
                return APIOperationResponse<StockAvailabilityDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Stock Availability");
                return APIOperationResponse<StockAvailabilityDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<OrderCycleTimeDto>> GetOrderCycleTimeAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                var result = await GetOrderCycleTimeInternalAsync(startDate, endDate);
                return APIOperationResponse<OrderCycleTimeDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Order Cycle Time");
                return APIOperationResponse<OrderCycleTimeDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<ConsumptionForecastDto>> GetConsumptionForecastAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                var result = await GetConsumptionForecastInternalAsync(startDate, endDate);
                return APIOperationResponse<ConsumptionForecastDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Consumption Forecast");
                return APIOperationResponse<ConsumptionForecastDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<object>> DrillDownAsync(DrillDownRequestDto request)
        {
            try
            {
                _logger.LogInformation($"Drilling down into {request.KpiType} at {request.Level} level");

                object result = request.KpiType switch
                {
                    "MissionReadiness" => await DrillDownMissionReadinessAsync(request),
                    "StockAvailability" => await DrillDownStockAvailabilityAsync(request),
                    "OrderCycleTime" => await DrillDownOrderCycleTimeAsync(request),
                    "ConsumptionForecast" => await DrillDownConsumptionForecastAsync(request),
                    _ => throw new ArgumentException($"Unknown KPI type: {request.KpiType}")
                };

                return APIOperationResponse<object>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in drill-down");
                return APIOperationResponse<object>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        #region Private Methods

        private async Task<MissionReadinessDto> GetMissionReadinessInternalAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            // Get all assets (units)
            var assets = await _context.Assets
                .Where(a => !a.IsDeleted)
                .Include(a => a.Item)
                .ToListAsync();

            var totalUnits = assets.Count;
            var missionReadyUnits = assets.Count(a => 
                a.Status == AssetStatus.Active && !a.IsAssigned);
            var underMaintenanceUnits = assets.Count(a => 
                a.Status == AssetStatus.Maintenance);
            var unavailableUnits = assets.Count(a => 
                a.Status == AssetStatus.Damaged || 
                a.Status == AssetStatus.Lost || 
                a.Status == AssetStatus.Disposed);

            var readinessRate = totalUnits > 0 
                ? (decimal)missionReadyUnits / totalUnits * 100 
                : 0;

            var status = readinessRate >= 95 ? "Green" 
                : readinessRate >= 85 ? "Amber" 
                : "Red";

            // Get trend data (last 90 days)
            var endDateForTrend = endDate ?? _dateTimeProvider.Now;
            var startDateForTrend = startDate ?? endDateForTrend.AddDays(-90);
            
            var trendData = new List<MissionReadinessTrendDto>();
            // Note: This is simplified - in production, you'd track historical readiness data
            for (var date = startDateForTrend.Date; date <= endDateForTrend.Date; date = date.AddDays(1))
            {
                // Simplified: Use current readiness rate for all dates
                // In production, you'd query historical asset status data
                trendData.Add(new MissionReadinessTrendDto
                {
                    Date = date,
                    ReadinessRate = readinessRate,
                    TotalUnits = totalUnits,
                    MissionReadyUnits = missionReadyUnits
                });
            }

            // Breakdown by category (item type)
            var byCategory = assets
                .Where(a => a.Item != null) // Filter out null items
                .GroupBy(a => a.Item!.ItemType.ToString())
                .Select(g => new MissionReadinessByCategoryDto
                {
                    CategoryName = g.Key,
                    TotalUnits = g.Count(),
                    MissionReadyUnits = g.Count(a => a.Status == AssetStatus.Active && !a.IsAssigned),
                    ReadinessRate = g.Count() > 0 
                        ? (decimal)g.Count(a => a.Status == AssetStatus.Active && !a.IsAssigned) / g.Count() * 100 
                        : 0
                })
                .ToList();

            return new MissionReadinessDto
            {
                ReadinessRate = readinessRate,
                TotalUnits = totalUnits,
                MissionReadyUnits = missionReadyUnits,
                UnderMaintenanceUnits = underMaintenanceUnits,
                UnavailableUnits = unavailableUnits,
                Status = status,
                TrendData = trendData,
                ByCategory = byCategory
            };
        }

        private async Task<StockAvailabilityDto> GetStockAvailabilityInternalAsync(int? warehouseId = null)
        {
            // Get inventory items with minimum quantity requirements
            var query = _context.InventoryDetails
                .Include(id => id.Inventory)
                .Include(id => id.Item)
                .Where(id => !id.Inventory.IsDeleted && !id.Item.IsDeleted);

            if (warehouseId.HasValue)
            {
                query = query.Where(id => id.Inventory.DepoId == warehouseId.Value);
            }

            var inventoryDetails = await query.ToListAsync();

            // Get items with minimum quantity set
            var itemsWithMinQty = await _context.BaseItems
                .Where(i => i.MinimumQuantity.HasValue && i.MinimumQuantity.Value > 0 && !i.IsDeleted)
                .ToListAsync();

            var totalRequiredItems = itemsWithMinQty.Count;
            var totalAvailableItems = itemsWithMinQty.Count(item =>
            {
                var totalStock = inventoryDetails
                    .Where(id => id.ItemId == item.Id)
                    .Sum(id => id.ItemQuantity);
                return totalStock >= (item.MinimumQuantity ?? 0);
            });

            var availabilityRate = totalRequiredItems > 0
                ? (decimal)totalAvailableItems / totalRequiredItems * 100
                : 0;

            // Breakdown by warehouse
            var warehouses = await _context.Depots
                .Where(d => !d.IsDeleted)
                .ToListAsync();

            var byWarehouse = warehouses.Select(warehouse =>
            {
                var warehouseItems = itemsWithMinQty.Select(item =>
                {
                    var stock = inventoryDetails
                        .Where(id => id.Inventory.DepoId == warehouse.Id && id.ItemId == item.Id)
                        .Sum(id => id.ItemQuantity);
                    return new { Item = item, Stock = stock, Required = item.MinimumQuantity ?? 0 };
                }).ToList();

                var required = warehouseItems.Count;
                var available = warehouseItems.Count(wi => wi.Stock >= wi.Required);

                return new StockAvailabilityByWarehouseDto
                {
                    WarehouseId = (int)warehouse.Id,
                    WarehouseName = warehouse.NameEn ?? warehouse.NameAr ?? "Unknown",
                    RequiredItems = required,
                    AvailableItems = available,
                    AvailabilityRate = required > 0 ? (decimal)available / required * 100 : 0
                };
            }).ToList();

            // Breakdown by category
            var byCategory = itemsWithMinQty
                .GroupBy(item => item.ItemType.ToString())
                .Select(g =>
                {
                    var items = g.ToList();
                    var available = items.Count(item =>
                    {
                        var stock = inventoryDetails
                            .Where(id => id.ItemId == item.Id)
                            .Sum(id => id.ItemQuantity);
                        return stock >= (item.MinimumQuantity ?? 0);
                    });

                    return new StockAvailabilityByCategoryDto
                    {
                        CategoryName = g.Key,
                        IsCritical = false, // You'd determine this based on business logic
                        RequiredItems = items.Count,
                        AvailableItems = available,
                        AvailabilityRate = items.Count > 0 ? (decimal)available / items.Count * 100 : 0
                    };
                })
                .ToList();

            // Low stock areas for heat map
            var lowStockAreas = warehouses.SelectMany(warehouse =>
            {
                return itemsWithMinQty
                    .GroupBy(item => item.ItemType.ToString())
                    .Select(g =>
                    {
                        var categoryItems = g.ToList();
                        var lowStockCount = categoryItems.Count(item =>
                        {
                            var stock = inventoryDetails
                                .Where(id => id.Inventory.DepoId == warehouse.Id && id.ItemId == item.Id)
                                .Sum(id => id.ItemQuantity);
                            return stock < (item.MinimumQuantity ?? 0);
                        });

                        return new LowStockAreaDto
                        {
                            WarehouseId = (int)warehouse.Id,
                            WarehouseName = warehouse.NameEn ?? warehouse.NameAr ?? "Unknown",
                            CategoryName = g.Key,
                            LowStockItemCount = lowStockCount,
                            Severity = categoryItems.Count > 0 
                                ? (decimal)lowStockCount / categoryItems.Count 
                                : 0
                        };
                    })
                    .Where(area => area.LowStockItemCount > 0);
            }).ToList();

            return new StockAvailabilityDto
            {
                AvailabilityRate = availabilityRate,
                TotalRequiredItems = totalRequiredItems,
                TotalAvailableItems = totalAvailableItems,
                ByWarehouse = byWarehouse,
                ByCategory = byCategory,
                LowStockAreas = lowStockAreas
            };
        }

        private async Task<OrderCycleTimeDto> GetOrderCycleTimeInternalAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var endDateForQuery = endDate ?? _dateTimeProvider.Now;
            var startDateForQuery = startDate ?? endDateForQuery.AddDays(-90);

            // Get completed orders (supplies with fulfillment status)
            var supplies = await _context.Supplies
                .Include(s => s.Order)
                .Include(s => s.SupplyDetails)
                .Where(s => !s.IsDeleted && 
                           s.FulfillmentStatus == SupplyFulfillmentStatus.Fully &&
                           s.CreationDate >= startDateForQuery &&
                           s.CreationDate <= endDateForQuery)
                .ToListAsync();

            if (!supplies.Any())
            {
                return new OrderCycleTimeDto();
            }

            var cycleTimes = supplies.Select(supply =>
            {
                var orderDate = supply.Order.CreationDate;
                var deliveryDate = supply.SupplyDate ?? supply.CreationDate;
                return (deliveryDate.Date - orderDate.Date).TotalDays;
            }).ToList();

            var averageCycleTime = (decimal)cycleTimes.Average();
            var medianCycleTime = (decimal)GetMedian(cycleTimes);
            var minCycleTime = (decimal)cycleTimes.Min();
            var maxCycleTime = (decimal)cycleTimes.Max();

            // Trend data (grouped by week)
            var trendData = supplies
                .GroupBy(s => new { Year = s.CreationDate.Year, Week = GetWeekOfYear(s.CreationDate) })
                .Select(g => new OrderCycleTimeTrendDto
                {
                    Date = g.First().CreationDate.Date,
                    AverageCycleTimeDays = (decimal)g.Average(s => 
                        ((s.SupplyDate ?? s.CreationDate) - s.Order.CreationDate).TotalDays),
                    OrderCount = g.Count()
                })
                .OrderBy(t => t.Date)
                .ToList();

            // Breakdown by component (simplified - you'd track each stage separately)
            var breakdown = new OrderCycleTimeBreakdownDto
            {
                AverageApprovalTimeDays = averageCycleTime * 0.2m, // Estimated
                AverageProcurementTimeDays = averageCycleTime * 0.4m,
                AverageShippingTimeDays = averageCycleTime * 0.2m,
                AverageReceivingTimeDays = averageCycleTime * 0.2m
            };

            // By supplier (if supplier info is available)
            var bySupplier = new List<OrderCycleTimeBySupplierDto>();
            // Note: Add supplier relationship if available in your schema

            // By item type
            var byItemType = supplies
                .SelectMany(s => s.SupplyDetails)
                .Where(sd => sd.Item != null) // Filter out null items
                .GroupBy(sd => sd.Item!.ItemType.ToString())
                .Select(g => new OrderCycleTimeByItemTypeDto
                {
                    ItemType = g.Key,
                    AverageCycleTimeDays = averageCycleTime, // Simplified
                    OrderCount = g.Select(sd => sd.Supply.OrderId).Distinct().Count()
                })
                .ToList();

            // Outliers (orders with cycle time > 2x average)
            var threshold = averageCycleTime * 2;
            var outliers = supplies
                .Where(s =>
                {
                    var cycleTime = ((s.SupplyDate ?? s.CreationDate) - s.Order.CreationDate).TotalDays;
                    return cycleTime > (double)threshold;
                })
                .Select(s => new OrderCycleTimeOutlierDto
                {
                    OrderId = (int)s.OrderId,
                    OrderNumber = s.Order.RequestNo,
                    CycleTimeDays = (decimal)((s.SupplyDate ?? s.CreationDate) - s.Order.CreationDate).TotalDays,
                    OrderDate = s.Order.CreationDate,
                    DeliveryDate = s.SupplyDate ?? s.CreationDate,
                    DelayReason = "Exceeded average cycle time"
                })
                .ToList();

            return new OrderCycleTimeDto
            {
                AverageCycleTimeDays = averageCycleTime,
                MedianCycleTimeDays = medianCycleTime,
                MinCycleTimeDays = minCycleTime,
                MaxCycleTimeDays = maxCycleTime,
                TrendData = trendData,
                Breakdown = breakdown,
                BySupplier = bySupplier,
                ByItemType = byItemType,
                Outliers = outliers
            };
        }

        private async Task<ConsumptionForecastDto> GetConsumptionForecastInternalAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var endDateForQuery = endDate ?? _dateTimeProvider.Now;
            var startDateForQuery = startDate ?? endDateForQuery.AddDays(-90);

            // Get actual consumption from supply details
            var actualConsumption = await _context.SupplyDetails
                .Include(sd => sd.Supply)
                .Where(sd => !sd.IsDeleted && 
                            !sd.Supply.IsDeleted &&
                            sd.Supply.FulfillmentStatus == SupplyFulfillmentStatus.Fully &&
                            sd.Supply.CreationDate >= startDateForQuery &&
                            sd.Supply.CreationDate <= endDateForQuery)
                .GroupBy(sd => sd.ItemId)
                .Select(g => new
                {
                    ItemId = g.Key,
                    ActualConsumption = g.Sum(sd => sd.Quantity)
                })
                .ToListAsync();

            // Get forecasted consumption (from allowance items or forecast data)
            // Note: This is simplified - you'd have a forecast table/entity
            var forecastedConsumption = await _context.AllowanceItems
                .Where(ai => !ai.IsDeleted && ai.Year == _dateTimeProvider.Now.Year)
                .GroupBy(ai => ai.ItemId)
                .Select(g => new
                {
                    ItemId = g.Key,
                    ForecastedConsumption = g.Sum(ai => ai.Quantity)
                })
                .ToListAsync();

            var totalActual = actualConsumption.Sum(ac => ac.ActualConsumption);
            var totalForecast = forecastedConsumption.Sum(fc => fc.ForecastedConsumption);

            var consumptionRatio = totalForecast > 0 
                ? totalActual / totalForecast 
                : 0;
            var variancePercentage = totalForecast > 0 
                ? (totalActual - totalForecast) / totalForecast * 100 
                : 0;

            // Trend data (grouped by month)
            var trendData = new List<ConsumptionForecastTrendDto>();
            for (var date = startDateForQuery.Date; date <= endDateForQuery.Date; date = date.AddMonths(1))
            {
                var monthStart = new DateTime(date.Year, date.Month, 1);
                var monthEnd = monthStart.AddMonths(1).AddDays(-1);

                var monthActual = actualConsumption.Sum(ac => ac.ActualConsumption) / 
                    ((endDateForQuery - startDateForQuery).Days / 30.0); // Simplified
                var monthForecast = totalForecast / 12; // Simplified

                trendData.Add(new ConsumptionForecastTrendDto
                {
                    Date = monthStart,
                    ActualConsumption = (decimal)monthActual,
                    ForecastedConsumption = monthForecast,
                    Variance = (decimal)monthActual - monthForecast,
                    VariancePercentage = monthForecast > 0 
                        ? ((decimal)monthActual - monthForecast) / monthForecast * 100 
                        : 0
                });
            }

            // By category
            var items = await _context.BaseItems
                .Where(i => !i.IsDeleted)
                .ToListAsync();

            var byCategory = items
                .GroupBy(item => item.ItemType.ToString())
                .Select(g =>
                {
                    var categoryItems = g.ToList();
                    var actual = actualConsumption
                        .Where(ac => categoryItems.Any(ci => ci.Id == ac.ItemId))
                        .Sum(ac => ac.ActualConsumption);
                    var forecast = forecastedConsumption
                        .Where(fc => categoryItems.Any(ci => ci.Id == fc.ItemId))
                        .Sum(fc => fc.ForecastedConsumption);

                    return new ConsumptionForecastByCategoryDto
                    {
                        CategoryName = g.Key,
                        ActualConsumption = (decimal)actual,
                        ForecastedConsumption = forecast,
                        Variance = (decimal)actual - forecast,
                        VariancePercentage = forecast > 0 
                            ? ((decimal)actual - forecast) / forecast * 100 
                            : 0
                    };
                })
                .ToList();

            // Alerts (over/under consumption)
            var alerts = items
                .Select(item =>
                {
                    var actual = actualConsumption
                        .FirstOrDefault(ac => ac.ItemId == item.Id)?.ActualConsumption ?? 0;
                    var forecast = forecastedConsumption
                        .FirstOrDefault(fc => fc.ItemId == item.Id)?.ForecastedConsumption ?? 0;

                    if (forecast == 0) return null;

                    var variancePct = (actual - forecast) / forecast * 100;

                    if (variancePct > 10 || variancePct < -10)
                    {
                        return new ConsumptionForecastAlertDto
                        {
                            ItemId = (int)item.Id,
                            ItemName = item.Name ?? "Unknown",
                            CategoryName = item.ItemType.ToString(),
                            ActualConsumption = (decimal)actual,
                            ForecastedConsumption = forecast,
                            VariancePercentage = (decimal)variancePct,
                            AlertType = variancePct > 10 ? "OverConsumption" : "UnderUtilization"
                        };
                    }

                    return null;
                })
                .Where(alert => alert != null)
                .ToList()!;

            return new ConsumptionForecastDto
            {
                ConsumptionRatio = consumptionRatio,
                VariancePercentage = variancePercentage,
                TrendData = trendData,
                ByCategory = byCategory,
                Alerts = alerts
            };
        }

        private async Task<object> DrillDownMissionReadinessAsync(DrillDownRequestDto request)
        {
            // Implementation for drill-down
            return await GetMissionReadinessInternalAsync(request.StartDate, request.EndDate);
        }

        private async Task<object> DrillDownStockAvailabilityAsync(DrillDownRequestDto request)
        {
            return await GetStockAvailabilityInternalAsync(request.WarehouseId);
        }

        private async Task<object> DrillDownOrderCycleTimeAsync(DrillDownRequestDto request)
        {
            return await GetOrderCycleTimeInternalAsync(request.StartDate, request.EndDate);
        }

        private async Task<object> DrillDownConsumptionForecastAsync(DrillDownRequestDto request)
        {
            return await GetConsumptionForecastInternalAsync(request.StartDate, request.EndDate);
        }

        private static double GetMedian(List<double> values)
        {
            if (!values.Any()) return 0;
            var sorted = values.OrderBy(v => v).ToList();
            var mid = sorted.Count / 2;
            return sorted.Count % 2 == 0 
                ? (sorted[mid - 1] + sorted[mid]) / 2 
                : sorted[mid];
        }

        private static int GetWeekOfYear(DateTime date)
        {
            var culture = System.Globalization.CultureInfo.CurrentCulture;
            return culture.Calendar.GetWeekOfYear(date, 
                System.Globalization.CalendarWeekRule.FirstFourDayWeek, 
                DayOfWeek.Monday);
        }

        #endregion

        #region Additional Charts Implementation

        public async Task<APIOperationResponse<OrderStatusDistributionDto>> GetOrderStatusDistributionAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                var result = await GetOrderStatusDistributionInternalAsync(startDate, endDate);
                return APIOperationResponse<OrderStatusDistributionDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Order Status Distribution");
                return APIOperationResponse<OrderStatusDistributionDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<RequestTrendsDto>> GetRequestTrendsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                var result = await GetRequestTrendsInternalAsync(startDate, endDate);
                return APIOperationResponse<RequestTrendsDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Request Trends");
                return APIOperationResponse<RequestTrendsDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<InventoryValueDto>> GetInventoryValueAsync(int? warehouseId = null)
        {
            try
            {
                var result = await GetInventoryValueInternalAsync(warehouseId);
                return APIOperationResponse<InventoryValueDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Inventory Value");
                return APIOperationResponse<InventoryValueDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<AssetAssignmentStatusDto>> GetAssetAssignmentStatusAsync()
        {
            try
            {
                var result = await GetAssetAssignmentStatusInternalAsync();
                return APIOperationResponse<AssetAssignmentStatusDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Asset Assignment Status");
                return APIOperationResponse<AssetAssignmentStatusDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<SupplyFulfillmentStatusDto>> GetSupplyFulfillmentStatusAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                var result = await GetSupplyFulfillmentStatusInternalAsync(startDate, endDate);
                return APIOperationResponse<SupplyFulfillmentStatusDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Supply Fulfillment Status");
                return APIOperationResponse<SupplyFulfillmentStatusDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<DepartmentRequestVolumeDto>> GetDepartmentRequestVolumeAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                var result = await GetDepartmentRequestVolumeInternalAsync(startDate, endDate);
                return APIOperationResponse<DepartmentRequestVolumeDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Department Request Volume");
                return APIOperationResponse<DepartmentRequestVolumeDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<NotificationStatisticsDto>> GetNotificationStatisticsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                var result = await GetNotificationStatisticsInternalAsync(startDate, endDate);
                return APIOperationResponse<NotificationStatisticsDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Notification Statistics");
                return APIOperationResponse<NotificationStatisticsDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private async Task<OrderStatusDistributionDto> GetOrderStatusDistributionInternalAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            // Normalize dates: start at beginning of day, end at end of day
            var startDateForQuery = startDate?.Date ?? (DateTime?)null;
            var endDateForQuery = endDate?.Date.AddDays(1).AddTicks(-1) ?? (DateTime?)null; // End of day

            // Query Orders
            var ordersQuery = _context.Orders.Where(o => !o.IsDeleted);
            if (startDateForQuery.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.CreationDate >= startDateForQuery.Value);
            }
            if (endDateForQuery.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.CreationDate <= endDateForQuery.Value);
            }
            var orders = await ordersQuery.ToListAsync();

            // Query Returns
            var returnsQuery = _context.Set<Return>().Where(r => !r.IsDeleted);
            if (startDateForQuery.HasValue)
            {
                returnsQuery = returnsQuery.Where(r => r.CreationDate >= startDateForQuery.Value);
            }
            if (endDateForQuery.HasValue)
            {
                returnsQuery = returnsQuery.Where(r => r.CreationDate <= endDateForQuery.Value);
            }
            var returns = await returnsQuery.ToListAsync();

            // Query Discards
            var discardsQuery = _context.Set<Discard>().Where(d => !d.IsDeleted);
            if (startDateForQuery.HasValue)
            {
                discardsQuery = discardsQuery.Where(d => d.CreationDate >= startDateForQuery.Value);
            }
            if (endDateForQuery.HasValue)
            {
                discardsQuery = discardsQuery.Where(d => d.CreationDate <= endDateForQuery.Value);
            }
            var discards = await discardsQuery.ToListAsync();

            // Combine all requests
            var allRequests = orders.Cast<object>()
                .Concat(returns.Cast<object>())
                .Concat(discards.Cast<object>())
                .ToList();

            var totalRequests = orders.Count + returns.Count + discards.Count;

            // Helper function to normalize status (map UnderProcess to Pending, keep New as New)
            string NormalizeStatus(RequestStatus status)
            {
                if (status == RequestStatus.UnderProcess)
                {
                    return "Pending";
                }
                return status.ToString();
            }

            // Group orders by normalized status
            var orderStatusGroups = orders
                .GroupBy(o => NormalizeStatus(o.Status))
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToList();

            // Group returns by normalized status
            var returnStatusGroups = returns
                .GroupBy(r => NormalizeStatus(r.Status))
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToList();

            // Group discards by normalized status
            var discardStatusGroups = discards
                .GroupBy(d => NormalizeStatus(d.Status))
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToList();

            // Combine all status groups
            var allStatusGroups = orderStatusGroups
                .Concat(returnStatusGroups)
                .Concat(discardStatusGroups)
                .GroupBy(g => g.Status)
                .Select(g => new OrderStatusDataPointDto
                {
                    Status = g.Key,
                    Count = g.Sum(x => x.Count),
                    Percentage = totalRequests > 0 ? Math.Round((decimal)g.Sum(x => x.Count) / totalRequests * 100, 0) : 0
                })
                .OrderByDescending(s => s.Count)
                .ToList();

            return new OrderStatusDistributionDto
            {
                StatusDistribution = allStatusGroups,
                TotalOrders = totalRequests
            };
        }

        private async Task<RequestTrendsDto> GetRequestTrendsInternalAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var defaultStartDate = startDate ?? _dateTimeProvider.Now.AddDays(-90);
            var defaultEndDate = endDate ?? _dateTimeProvider.Now;

            // Normalize dates: start at beginning of day, end at end of day
            var startDateForQuery = defaultStartDate.Date;
            var endDateForQuery = defaultEndDate.Date.AddDays(1).AddTicks(-1); // End of day

            var orders = await _context.Orders
                .Where(o => !o.IsDeleted && o.CreationDate >= startDateForQuery && o.CreationDate <= endDateForQuery)
                .ToListAsync();

            var returns = await _context.Set<Return>()
                .Where(r => !r.IsDeleted && r.CreationDate >= startDateForQuery && r.CreationDate <= endDateForQuery)
                .ToListAsync();

            var discards = await _context.Set<Discard>()
                .Where(d => !d.IsDeleted && d.CreationDate >= startDateForQuery && d.CreationDate <= endDateForQuery)
                .ToListAsync();

            var allDates = orders.Select(o => o.CreationDate.Date)
                .Union(returns.Select(r => r.CreationDate.Date))
                .Union(discards.Select(d => d.CreationDate.Date))
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            var trendData = allDates.Select(date => new RequestTrendDataPointDto
            {
                Date = date,
                OrderCount = orders.Count(o => o.CreationDate.Date == date),
                ReturnCount = returns.Count(r => r.CreationDate.Date == date),
                DiscardCount = discards.Count(d => d.CreationDate.Date == date),
                TotalCount = orders.Count(o => o.CreationDate.Date == date) +
                            returns.Count(r => r.CreationDate.Date == date) +
                            discards.Count(d => d.CreationDate.Date == date)
            }).ToList();

            return new RequestTrendsDto
            {
                TrendData = trendData,
                TotalRequests = orders.Count + returns.Count + discards.Count,
                PendingRequests = orders.Count(o => o.Status == RequestStatus.New || o.Status == RequestStatus.UnderProcess) +
                                 returns.Count(r => r.Status == RequestStatus.New || r.Status == RequestStatus.UnderProcess) +
                                 discards.Count(d => d.Status == RequestStatus.New || d.Status == RequestStatus.UnderProcess),
                ApprovedRequests = orders.Count(o => o.Status == RequestStatus.Approved) +
                                  returns.Count(r => r.Status == RequestStatus.Approved) +
                                  discards.Count(d => d.Status == RequestStatus.Approved),
                RejectedRequests = orders.Count(o => o.Status == RequestStatus.Rejected) +
                                  returns.Count(r => r.Status == RequestStatus.Rejected) +
                                  discards.Count(d => d.Status == RequestStatus.Rejected)
            };
        }

        private async Task<InventoryValueDto> GetInventoryValueInternalAsync(int? warehouseId = null)
        {
            var query = _context.InventoryDetails
                .Include(id => id.Inventory)
                .Include(id => id.Item)
                .Where(id => !id.Inventory.IsDeleted && !id.Item.IsDeleted);

            if (warehouseId.HasValue)
            {
                query = query.Where(id => id.Inventory.DepoId == warehouseId.Value);
            }

            var inventoryDetails = await query.ToListAsync();
            var warehouses = await _context.Depots.Where(d => !d.IsDeleted).ToListAsync();

            var byWarehouse = warehouses.Select(warehouse =>
            {
                var warehouseItems = inventoryDetails
                    .Where(id => id.Inventory.DepoId == warehouse.Id)
                    .GroupBy(id => id.ItemId)
                    .Select(g => new
                    {
                        Item = g.First().Item,
                        Quantity = g.Sum(id => id.ItemQuantity)
                    })
                    .ToList();

                var totalValue = warehouseItems.Sum(wi => 
                    (wi.Item.Price ?? 0) * wi.Quantity);
                var lowStockItems = warehouseItems.Count(wi => 
                    wi.Item.MinimumQuantity.HasValue && 
                    wi.Quantity < wi.Item.MinimumQuantity.Value);

                return new WarehouseValueDto
                {
                    WarehouseId = (int)warehouse.Id,
                    WarehouseName = warehouse.NameEn ?? warehouse.NameAr ?? "Unknown",
                    TotalValue = totalValue,
                    ItemCount = warehouseItems.Count,
                    LowStockItems = lowStockItems
                };
            }).ToList();

            var totalValue = byWarehouse.Sum(w => w.TotalValue);
            var totalItems = inventoryDetails.Select(id => id.ItemId).Distinct().Count();

            return new InventoryValueDto
            {
                ByWarehouse = byWarehouse,
                TotalValue = totalValue,
                TotalItems = totalItems
            };
        }

        private async Task<AssetAssignmentStatusDto> GetAssetAssignmentStatusInternalAsync()
        {
            var assets = await _context.Assets
                .Where(a => !a.IsDeleted)
                .ToListAsync();

            var assignments = await _context.AssetAssignments
                .Where(aa => aa.Status == AssetAssignmentStatus.Active)
                .ToListAsync();

            var assignedAssetIds = assignments.Select(aa => aa.AssetId).Distinct().ToList();
            var assignedAssets = assets.Count(a => assignedAssetIds.Contains(a.Id));
            var unassignedAssets = assets.Count - assignedAssets;

            var statusGroups = assets
                .GroupBy(a => a.Status)
                .Select(g => new AssetStatusDataPointDto
                {
                    Status = g.Key.ToString(),
                    Count = g.Count(),
                    Percentage = assets.Count > 0 ? Math.Round((decimal)g.Count() / assets.Count * 100, 0) : 0
                })
                .ToList();

            return new AssetAssignmentStatusDto
            {
                StatusDistribution = statusGroups,
                TotalAssets = assets.Count,
                AssignedAssets = assignedAssets,
                UnassignedAssets = unassignedAssets
            };
        }

        private async Task<SupplyFulfillmentStatusDto> GetSupplyFulfillmentStatusInternalAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var query = _context.Supplies.Where(s => !s.IsDeleted);

            if (startDate.HasValue)
            {
                query = query.Where(s => s.CreationDate >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(s => s.CreationDate <= endDate.Value);
            }

            var supplies = await query.ToListAsync();
            var totalSupplies = supplies.Count;

            var statusGroups = supplies
                .GroupBy(s => s.FulfillmentStatus)
                .Select(g => new SupplyStatusDataPointDto
                {
                    Status = g.Key.ToString(),
                    Count = g.Count(),
                    Percentage = totalSupplies > 0 ? Math.Round((decimal)g.Count() / totalSupplies * 100, 0) : 0
                })
                .ToList();

            var fullyFulfilled = supplies.Count(s => s.FulfillmentStatus == SupplyFulfillmentStatus.Fully);
            var averageFulfillmentRate = totalSupplies > 0 ? (decimal)fullyFulfilled / totalSupplies * 100 : 0;

            return new SupplyFulfillmentStatusDto
            {
                StatusDistribution = statusGroups,
                TotalSupplies = totalSupplies,
                AverageFulfillmentRate = averageFulfillmentRate
            };
        }

        private async Task<DepartmentRequestVolumeDto> GetDepartmentRequestVolumeInternalAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            // Normalize dates: start at beginning of day, end at end of day
            var startDateForQuery = startDate?.Date ?? (DateTime?)null;
            var endDateForQuery = endDate?.Date.AddDays(1).AddTicks(-1) ?? (DateTime?)null; // End of day

            // Query Orders
            var ordersQuery = _context.Orders
                .Include(o => o.Department)
                .Where(o => !o.IsDeleted);
            if (startDateForQuery.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.CreationDate >= startDateForQuery.Value);
            }
            if (endDateForQuery.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.CreationDate <= endDateForQuery.Value);
            }
            var orders = await ordersQuery.ToListAsync();

            // Query Returns
            var returnsQuery = _context.Set<Return>()
                .Include(r => r.Department)
                .Where(r => !r.IsDeleted);
            if (startDateForQuery.HasValue)
            {
                returnsQuery = returnsQuery.Where(r => r.CreationDate >= startDateForQuery.Value);
            }
            if (endDateForQuery.HasValue)
            {
                returnsQuery = returnsQuery.Where(r => r.CreationDate <= endDateForQuery.Value);
            }
            var returns = await returnsQuery.ToListAsync();

            // Query Discards
            var discardsQuery = _context.Set<Discard>()
                .Include(d => d.Department)
                .Where(d => !d.IsDeleted);
            if (startDateForQuery.HasValue)
            {
                discardsQuery = discardsQuery.Where(d => d.CreationDate >= startDateForQuery.Value);
            }
            if (endDateForQuery.HasValue)
            {
                discardsQuery = discardsQuery.Where(d => d.CreationDate <= endDateForQuery.Value);
            }
            var discards = await discardsQuery.ToListAsync();

            // Combine all requests for grouping
            var allRequests = orders.Cast<BaseRequest>()
                .Concat(returns.Cast<BaseRequest>())
                .Concat(discards.Cast<BaseRequest>())
                .ToList();

            // Group by department
            var departmentGroups = allRequests
                .GroupBy(r => new { 
                    r.DepartmentId, 
                    DepartmentName = r.Department?.NameEn ?? r.Department?.NameAr ?? "Unknown" 
                })
                .Select(g => new DepartmentVolumeDto
                {
                    DepartmentId = (int)g.Key.DepartmentId,
                    DepartmentName = g.Key.DepartmentName,
                    RequestCount = g.Count(),
                    OrderCount = g.Count(r => r is Order),
                    ReturnCount = g.Count(r => r is Return),
                    DiscardCount = g.Count(r => r is Discard)
                })
                .OrderByDescending(d => d.RequestCount)
                .ToList();

            return new DepartmentRequestVolumeDto
            {
                ByDepartment = departmentGroups,
                TotalRequests = allRequests.Count
            };
        }

        private async Task<NotificationStatisticsDto> GetNotificationStatisticsInternalAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            var defaultStartDate = startDate ?? _dateTimeProvider.Now.AddDays(-90);
            var defaultEndDate = endDate ?? _dateTimeProvider.Now;

            var notifications = await _context.Notifications
                .Where(n => !n.IsDeleted && n.CreationDate >= defaultStartDate && n.CreationDate <= defaultEndDate)
                .Include(n => n.Receivers)
                .ToListAsync();

            var notificationReceivers = await _context.NotificationReceivers
                .Where(nr => notifications.Select(n => n.Id).Contains(nr.NotificationId))
                .ToListAsync();

            var totalNotifications = notifications.Count;
            // Explicitly check IsRead with null safety - treat null as unread
            var readNotifications = notificationReceivers.Count(nr => nr.IsRead == true);
            // Calculate unread as total - read to ensure consistency
            var unreadNotifications = notificationReceivers.Count - readNotifications;

            // Calculate average read time
            var readReceivers = notificationReceivers
                .Where(nr => nr.IsRead && nr.ReadAt.HasValue)
                .ToList();

            var averageReadTimeHours = 0m;
            if (readReceivers.Any())
            {
                var readTimes = readReceivers.Select(nr =>
                {
                    var notification = notifications.FirstOrDefault(n => n.Id == nr.NotificationId);
                    if (notification != null && nr.ReadAt.HasValue)
                    {
                        return (nr.ReadAt.Value - notification.CreationDate).TotalHours;
                    }
                    return 0.0;
                }).Where(h => h > 0).ToList();

                if (readTimes.Any())
                {
                    averageReadTimeHours = (decimal)readTimes.Average();
                }
            }

            // Trend data by date
            var allDates = notifications.Select(n => n.CreationDate.Date)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            var trendData = allDates.Select(date =>
            {
                var dateNotifications = notifications.Where(n => n.CreationDate.Date == date).ToList();
                var dateNotificationIds = dateNotifications.Select(n => n.Id).ToList();
                var dateReceivers = notificationReceivers.Where(nr => dateNotificationIds.Contains(nr.NotificationId)).ToList();

                var sentCount = dateReceivers.Count; // Total receivers (deliveries)
                // Explicitly check IsRead with null safety - treat null as unread
                var readCount = dateReceivers.Count(nr => nr.IsRead == true);
                // Calculate unread as sent - read to ensure consistency
                var unreadCount = sentCount - readCount;

                return new NotificationTrendDataPointDto
                {
                    Date = date,
                    SentCount = sentCount,
                    ReadCount = readCount,
                    UnreadCount = unreadCount
                };
            }).ToList();

            // By entity type
            var byType = notifications
                .GroupBy(n => n.EntityType ?? "System")
                .Select(g => new NotificationByTypeDto
                {
                    EntityType = g.Key,
                    Count = g.Count(),
                    Percentage = totalNotifications > 0 ? Math.Round((decimal)g.Count() / totalNotifications * 100, 0) : 0
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            // By entity with read status
            var byEntity = notifications
                .GroupBy(n => n.EntityType ?? "System")
                .Select(g =>
                {
                    var entityNotificationIds = g.Select(n => n.Id).ToList();
                    var entityReceivers = notificationReceivers.Where(nr => entityNotificationIds.Contains(nr.NotificationId)).ToList();
                    var entityReadCount = entityReceivers.Count(nr => nr.IsRead);
                    var entityTotalReceivers = entityReceivers.Count;

                    return new NotificationByEntityDto
                    {
                        EntityType = g.Key,
                        NotificationCount = g.Count(),
                        ReadCount = entityReadCount,
                        UnreadCount = entityTotalReceivers - entityReadCount,
                        ReadRate = entityTotalReceivers > 0 ? Math.Round((decimal)entityReadCount / entityTotalReceivers * 100, 0) : 0
                    };
                })
                .OrderByDescending(x => x.NotificationCount)
                .ToList();

            var readRate = notificationReceivers.Count > 0
                ? Math.Round((decimal)readNotifications / notificationReceivers.Count * 100, 0)
                : 0;

            return new NotificationStatisticsDto
            {
                TrendData = trendData,
                ByType = byType,
                ByEntity = byEntity,
                ReadStatus = new NotificationReadStatusDto
                {
                    TotalSent = totalNotifications,
                    TotalRead = readNotifications,
                    TotalUnread = unreadNotifications,
                    ReadRate = readRate,
                    AverageReadTimeHours = averageReadTimeHours
                },
                TotalNotifications = totalNotifications,
                UnreadNotifications = unreadNotifications,
                ReadNotifications = readNotifications,
                AverageReadTimeHours = averageReadTimeHours
            };
        }

        /// <summary>
        /// Get Import/Export Statistics (Public Method)
        /// </summary>
        public async Task<APIOperationResponse<ImportExportStatisticsDto>> GetImportExportStatisticsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                var result = await GetImportExportStatisticsInternalAsync(startDate, endDate);
                return APIOperationResponse<ImportExportStatisticsDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Import/Export Statistics");
                return APIOperationResponse<ImportExportStatisticsDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Get Import/Export Statistics (Internal Method)
        /// Tracks file uploads (imports) and calculates export statistics
        /// </summary>
        private async Task<ImportExportStatisticsDto> GetImportExportStatisticsInternalAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            // Default to last 90 days if no date range provided
            var defaultStartDate = startDate ?? _dateTimeProvider.Now.AddDays(-90);
            var defaultEndDate = endDate ?? _dateTimeProvider.Now;

            // Get file uploads (imports) from FileUplodMaster
            // Note: FileUplodMaster only has Id property, no IsDeleted or CreationDate
            // We'll get all file uploads since we can't filter by date
            var fileUploads = await _context.FileUplodMasters.ToListAsync();

            // Get file upload details to determine entity types
            var fileUploadIds = fileUploads.Select(f => f.Id).ToList();
            var fileDetails = await _context.FileUplodDetails
                .Where(fd => fileUploadIds.Contains(fd.FileUplodMasterId))
                .ToListAsync();

            // Calculate imports (file uploads)
            var totalImports = fileUploads.Count;

            // For exports, we'll track based on inventory exports and other export operations
            // Since there's no explicit export log, we'll infer from recent inventory summaries
            // or track export operations if they create records
            // For now, we'll set exports to 0 as there's no explicit export tracking
            var totalExports = 0;

            // Group by date for trend data
            // Since FileUplodMaster doesn't have CreationDate, we'll create a simple trend
            // showing all imports in a single data point, or we can group by a default date
            // For now, we'll create a single trend data point with all imports
            var trendData = new List<ImportExportTrendDataPointDto>
            {
                new ImportExportTrendDataPointDto
                {
                    Date = defaultStartDate.Date,
                    ImportCount = totalImports,
                    ExportCount = totalExports,
                    TotalOperations = totalImports + totalExports
                }
            };

            // By Type (based on file entity types)
            var byType = fileDetails
                .GroupBy(fd => fd.Entity.ToString())
                .Select(g => new ImportExportByTypeDto
                {
                    Type = g.Key,
                    ImportCount = g.Count(),
                    ExportCount = 0,
                    TotalCount = g.Count(),
                    Percentage = totalImports > 0 ? Math.Round((decimal)g.Count() / totalImports * 100, 0) : 0
                })
                .OrderByDescending(x => x.TotalCount)
                .ToList();

            // By Entity (grouped by entity type from file details)
            var byEntity = fileDetails
                .GroupBy(fd => fd.Entity.ToString())
                .Select(g => new ImportExportByEntityDto
                {
                    EntityType = g.Key,
                    ImportCount = g.Count(),
                    ExportCount = 0,
                    TotalCount = g.Count(),
                    ImportPercentage = totalImports > 0 ? Math.Round((decimal)g.Count() / totalImports * 100, 0) : 0,
                    ExportPercentage = 0
                })
                .OrderByDescending(x => x.TotalCount)
                .ToList();

            // Summary
            // totalExports already declared above
            var totalOperations = totalImports + totalExports;

            var summary = new ImportExportSummaryDto
            {
                TotalImports = totalImports,
                TotalExports = totalExports,
                TotalOperations = totalOperations,
                ImportPercentage = totalOperations > 0 ? Math.Round((decimal)totalImports / totalOperations * 100, 0) : 0,
                ExportPercentage = totalOperations > 0 ? Math.Round((decimal)totalExports / totalOperations * 100, 0) : 0,
                LastImportDate = null, // FileUplodMaster doesn't have CreationDate property
                LastExportDate = null // Will be updated when export tracking is implemented
            };

            return new ImportExportStatisticsDto
            {
                TrendData = trendData,
                ByType = byType,
                ByEntity = byEntity,
                Summary = summary,
                TotalImports = totalImports,
                TotalExports = totalExports
            };
        }

        /// <summary>
        /// Get User Login Analytics (Public Method)
        /// </summary>
        public async Task<APIOperationResponse<UserLoginAnalyticsDto>> GetUserLoginAnalyticsAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            try
            {
                var result = await GetUserLoginAnalyticsInternalAsync(startDate, endDate);
                return APIOperationResponse<UserLoginAnalyticsDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching User Login Analytics");
                return APIOperationResponse<UserLoginAnalyticsDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Get User Login Analytics (Internal Method)
        /// Calculates login counts and estimated session durations
        /// </summary>
        private async Task<UserLoginAnalyticsDto> GetUserLoginAnalyticsInternalAsync(
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            // Default to last 30 days if no date range provided
            var defaultStartDate = startDate ?? _dateTimeProvider.Now.AddDays(-30);
            var defaultEndDate = endDate ?? _dateTimeProvider.Now;

            // Get all successful login attempts within the date range
            var loginAttempts = await _context.LoginAttempts
                .Where(la => la.IsSuccessful 
                    && la.AttemptDate >= defaultStartDate 
                    && la.AttemptDate <= defaultEndDate
                    && la.UserId != null)
                .OrderBy(la => la.AttemptDate)
                .ToListAsync();

            // Get user information
            var userIds = loginAttempts.Select(la => la.UserId).Distinct().ToList();
            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new { u.Id, u.UserName, u.FullNameEN, u.FullNameAR })
                .ToListAsync();

            var userDict = users.ToDictionary(u => u.Id, u => new { u.UserName, FullName = u.FullNameEN ?? u.FullNameAR ?? u.UserName });

            // Group logins by user and calculate statistics
            var userLoginStats = loginAttempts
                .GroupBy(la => la.UserId!)
                .Select(g =>
                {
                    var userLogins = g.OrderBy(la => la.AttemptDate).ToList();
                    var loginCount = userLogins.Count;
                    
                    // Calculate total session duration
                    // Estimate: if there's a next login within 24 hours, assume session ended at next login
                    // Otherwise, assume session lasted 8 hours (average work session)
                    decimal totalDurationHours = 0;
                    for (int i = 0; i < userLogins.Count; i++)
                    {
                        var currentLogin = userLogins[i];
                        if (i < userLogins.Count - 1)
                        {
                            var nextLogin = userLogins[i + 1];
                            var timeBetween = (nextLogin.AttemptDate - currentLogin.AttemptDate).TotalHours;
                            
                            // If next login is within 24 hours, assume session ended at next login
                            if (timeBetween <= 24)
                            {
                                totalDurationHours += (decimal)timeBetween;
                            }
                            else
                            {
                                // Otherwise, assume 8-hour session
                                totalDurationHours += 8;
                            }
                        }
                        else
                        {
                            // Last login: if it's today, assume still active (use current time)
                            // Otherwise, assume 8-hour session
                            if (currentLogin.AttemptDate.Date == _dateTimeProvider.Now.Date)
                            {
                                var hoursSinceLogin = (_dateTimeProvider.Now - currentLogin.AttemptDate).TotalHours;
                                totalDurationHours += (decimal)Math.Min(hoursSinceLogin, 8);
                            }
                            else
                            {
                                totalDurationHours += 8;
                            }
                        }
                    }

                    var avgDuration = loginCount > 0 ? totalDurationHours / loginCount : 0;
                    var userInfo = userDict.ContainsKey(g.Key) ? userDict[g.Key] : null;

                    return new UserLoginDataPointDto
                    {
                        UserId = g.Key,
                        Username = userInfo?.UserName ?? g.First().Username,
                        FullName = userInfo?.FullName,
                        LoginCount = loginCount,
                        TotalLoginDurationHours = Math.Round(totalDurationHours, 2),
                        AverageLoginDurationHours = Math.Round(avgDuration, 2),
                        LastLoginDate = userLogins.Last().AttemptDate
                    };
                })
                .OrderByDescending(u => u.TotalLoginDurationHours)
                .ToList();

            // Calculate trend data (daily aggregation)
            var trendData = loginAttempts
                .GroupBy(la => la.AttemptDate.Date)
                .Select(g =>
                {
                    var dayLogins = g.ToList();
                    var uniqueUsers = dayLogins.Select(la => la.UserId).Distinct().Count();
                    
                    // Calculate average duration for this day
                    // For simplicity, estimate 8 hours per login session
                    var avgDuration = 8.0m;

                    return new LoginTrendDataPointDto
                    {
                        Date = g.Key,
                        LoginCount = dayLogins.Count,
                        UniqueUsers = uniqueUsers,
                        AverageDurationHours = avgDuration
                    };
                })
                .OrderBy(t => t.Date)
                .ToList();

            // Calculate summary statistics
            var totalLogins = loginAttempts.Count;
            var uniqueUsers = loginAttempts.Select(la => la.UserId).Distinct().Count();
            var totalDuration = userLoginStats.Sum(u => u.TotalLoginDurationHours);
            var averageLoginDurationHours = totalLogins > 0 
                ? Math.Round(totalDuration / totalLogins, 2) 
                : 0;

            // Active users today
            var today = _dateTimeProvider.Now.Date;
            var activeUsersToday = loginAttempts
                .Where(la => la.AttemptDate.Date == today)
                .Select(la => la.UserId)
                .Distinct()
                .Count();

            return new UserLoginAnalyticsDto
            {
                UserLogins = userLoginStats,
                TrendData = trendData,
                TotalLogins = totalLogins,
                UniqueUsers = uniqueUsers,
                AverageLoginDurationHours = averageLoginDurationHours,
                ActiveUsersToday = activeUsersToday
            };
        }

        #endregion
    }
}
