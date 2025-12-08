using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Data.Entities;
using Ettad.Data.Entities.Settings;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Notification.Service;
using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Data.Repository;
using Ettad.ResponseHandler.Consts;
using Microsoft.AspNetCore.Identity;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Idenitity;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;

namespace Ettad.Inventory.Service.Monitoring
{
    public class LowStockMonitorService : ILowStockMonitorService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationHelperService _notificationHelperService;
        private readonly ILogger<LowStockMonitorService> _logger;
        private readonly ICrossCuttingRepository<Settings> _settingsRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRecurringJobManager? _recurringJobManager;
        private readonly IServiceProvider? _serviceProvider;
        private const string SETTINGS_KEY = "LowStockNotificationRecipients";
        private const string SETTINGS_GROUP = "LowStockNotifications";
        private const string SCHEDULE_SETTINGS_KEY = "LowStockMonitorSchedule";
        private const string SCHEDULE_SETTINGS_GROUP = "BackgroundJobs";
        private const string JOB_ID = "LowStockMonitor";
        private const string DEFAULT_ROLE_NAME = "Head of Depo Division (Inventory)";
        private const string DEFAULT_CRON_EXPRESSION = "15 6 * * *"; // Default: 6:15 AM UTC (9:15 AM Qatar time, UTC+3)

        public LowStockMonitorService(
            ApplicationDbContext context,
            INotificationHelperService notificationHelperService,
            ILogger<LowStockMonitorService> logger,
            ICrossCuttingRepository<Settings> settingsRepository,
            ICurrentUserService currentUserService,
            RoleManager<ApplicationRole> roleManager,
            IRecurringJobManager? recurringJobManager = null,
            IServiceProvider? serviceProvider = null)
        {
            _context = context;
            _notificationHelperService = notificationHelperService;
            _logger = logger;
            _settingsRepository = settingsRepository;
            _currentUserService = currentUserService;
            _roleManager = roleManager;
            _recurringJobManager = recurringJobManager;
            _serviceProvider = serviceProvider;
        }

        public async Task CheckAndNotifyAsync()
        {
            _logger.LogInformation("Starting Low Stock Check...");

            try
            {
                var itemsToCheck = await _context.BaseItems
                    .Where(i => i.MinimumQuantity.HasValue && i.MinimumQuantity.Value > 0 && !i.IsDeleted)
                    .AsNoTracking()
                    .ToListAsync();

                _logger.LogInformation($"Found {itemsToCheck.Count} items with minimum quantity configured.");

                var lowStockItems = new List<LowStockItemInfo>();

                foreach (var item in itemsToCheck)
                {
                    var lowStockInfo = await CheckItemStockAsync(item);
                    if (lowStockInfo != null)
                    {
                        lowStockItems.Add(lowStockInfo);
                    }
                }

                if (lowStockItems.Any())
                {
                    await NotifyLowStockBatchAsync(lowStockItems);
                }
                else
                {
                    _logger.LogInformation("No items found below minimum stock level.");
                }

                _logger.LogInformation("Low Stock Check completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during Low Stock Check.");
                throw;
            }
        }

        public async Task<APIOperationResponse<LowStockNotificationSettingsDto>> GetSettingsAsync()
        {
            try
            {
                var setting = await _settingsRepository.FindOneAsync(
                    s => s.Key == SETTINGS_KEY && s.Group == SETTINGS_GROUP);

                if (setting == null)
                {
                    return APIOperationResponse<LowStockNotificationSettingsDto>.Success(
                        new LowStockNotificationSettingsDto { Roles = new List<string>(), Users = new List<string>() });
                }

                var dto = JsonSerializer.Deserialize<LowStockNotificationSettingsDto>(setting.Value ?? "{}");
                return APIOperationResponse<LowStockNotificationSettingsDto>.Success(dto ?? new LowStockNotificationSettingsDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving notification settings.");
                return APIOperationResponse<LowStockNotificationSettingsDto>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateSettingsAsync(LowStockNotificationSettingsDto dto)
        {
            try
            {
                var setting = await _settingsRepository.FindOneAsync(
                    s => s.Key == SETTINGS_KEY && s.Group == SETTINGS_GROUP);

                var jsonValue = JsonSerializer.Serialize(dto);

                if (setting == null)
                {
                    setting = new Settings
                    {
                        Key = SETTINGS_KEY,
                        Group = SETTINGS_GROUP,
                        Value = jsonValue,
                        CreationDate = DateTime.UtcNow,
                        CreatedBy = _currentUserService.UserId
                    };
                    await _settingsRepository.AddAsync(setting);
                }
                else
                {
                    setting.Value = jsonValue;
                    setting.ModificationDate = DateTime.UtcNow;
                    setting.ModifiedBy = _currentUserService.UserId;
                    await _settingsRepository.UpdateAsync(setting);
                }

                return APIOperationResponse<bool>.Success(true, "Settings updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating notification settings.");
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<string>> GetScheduleAsync()
        {
            try
            {
                var setting = await _settingsRepository.FindOneAsync(
                    s => s.Key == SCHEDULE_SETTINGS_KEY && s.Group == SCHEDULE_SETTINGS_GROUP);

                var cronExpression = setting?.Value ?? DEFAULT_CRON_EXPRESSION;
                return APIOperationResponse<string>.Success(cronExpression);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving schedule.");
                return APIOperationResponse<string>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateScheduleAsync(DateTime scheduleTime)
        {
            try
            {
                // Convert local time to UTC (assuming scheduleTime is in Qatar time UTC+3)
                // If scheduleTime.Kind is Unspecified, assume it's Qatar local time
                DateTime utcTime;
                if (scheduleTime.Kind == DateTimeKind.Unspecified)
                {
                    // Assume Qatar time, convert to UTC
                    // Use platform-specific timezone ID (Windows: "Arab Standard Time", Linux: "Asia/Riyadh")
                    TimeZoneInfo qatarTimeZone;
                    try
                    {
                        qatarTimeZone = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                            ? TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time")
                            : TimeZoneInfo.FindSystemTimeZoneById("Asia/Riyadh");
                        utcTime = TimeZoneInfo.ConvertTimeToUtc(scheduleTime, qatarTimeZone);
                    }
                    catch (TimeZoneNotFoundException)
                    {
                        // Fallback: assume UTC+3 offset if timezone not found
                        _logger.LogWarning("Qatar timezone not found, using UTC+3 offset as fallback");
                        utcTime = scheduleTime.AddHours(-3);
                    }
                }
                else if (scheduleTime.Kind == DateTimeKind.Local)
                {
                    utcTime = scheduleTime.ToUniversalTime();
                }
                else
                {
                    utcTime = scheduleTime; // Already UTC
                }

                // Convert to cron expression format: "minute hour * * *" (daily at specified time)
                var cronExpression = $"{utcTime.Minute} {utcTime.Hour} * * *";

                var setting = await _settingsRepository.FindOneAsync(
                    s => s.Key == SCHEDULE_SETTINGS_KEY && s.Group == SCHEDULE_SETTINGS_GROUP);

                if (setting == null)
                {
                    setting = new Settings
                    {
                        Key = SCHEDULE_SETTINGS_KEY,
                        Group = SCHEDULE_SETTINGS_GROUP,
                        Value = cronExpression,
                        CreationDate = DateTime.UtcNow,
                        CreatedBy = _currentUserService.UserId
                    };
                    await _settingsRepository.AddAsync(setting);
                }
                else
                {
                    setting.Value = cronExpression;
                    setting.ModificationDate = DateTime.UtcNow;
                    setting.ModifiedBy = _currentUserService.UserId;
                    await _settingsRepository.UpdateAsync(setting);
                }

                _logger.LogInformation("Low Stock Monitor schedule updated to: {ScheduleTime} (UTC: {UtcTime}, Cron: {CronExpression})", 
                    scheduleTime, utcTime, cronExpression);
                
                // Update Hangfire recurring job immediately if available
                if (_recurringJobManager != null && _serviceProvider != null)
                {
                    try
                    {
                        // Use the same pattern as Program.cs - Hangfire will resolve the service from DI when executing
                        _recurringJobManager.AddOrUpdate(
                            JOB_ID,
                            () => _serviceProvider.GetRequiredService<ILowStockMonitorService>().CheckAndNotifyAsync(),
                            cronExpression);
                        _logger.LogInformation("Hangfire recurring job '{JobId}' updated with new schedule: {CronExpression}", JOB_ID, cronExpression);
                        return APIOperationResponse<bool>.Success(true, "Schedule updated successfully. The recurring job has been updated immediately.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to update Hangfire recurring job. The schedule was saved to database but the job will use the new schedule on next application restart.");
                        return APIOperationResponse<bool>.Success(true, "Schedule saved successfully. Note: The recurring job will be updated on next application restart.");
                    }
                }
                else
                {
                    _logger.LogWarning("IRecurringJobManager or IServiceProvider not available. Schedule saved to database but job will use new schedule on next application restart.");
                    return APIOperationResponse<bool>.Success(true, "Schedule saved successfully. Note: The recurring job will be updated on next application restart.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating schedule.");
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        private async Task<LowStockItemInfo?> CheckItemStockAsync(BaseItem item)
        {
            var totalStock = await _context.InventoryDetails
                .Where(id => id.ItemId == item.Id && !id.Inventory.IsDeleted)
                .SumAsync(id => id.ItemQuantity);

            var supplyDetails = await _context.SupplyDetails
                .Include(sd => sd.Supply)
                .Where(sd => sd.ItemId == item.Id && !sd.IsDeleted && !sd.Supply.IsDeleted)
                .Select(sd => new { sd.Quantity, sd.Supply.SubmissionStatus })
                .ToListAsync();

            var holdQuantity = supplyDetails
                .Where(sd => sd.SubmissionStatus == SupplySubmissionStatus.Draft)
                .Sum(sd => sd.Quantity);

            var suppliedQuantity = supplyDetails
                .Where(sd => sd.SubmissionStatus == SupplySubmissionStatus.Submitted)
                .Sum(sd => sd.Quantity);

            var remaining = totalStock - (holdQuantity + suppliedQuantity);

            _logger.LogDebug($"Item {item.Name} (ID: {item.Id}): Min={item.MinimumQuantity}, Total={totalStock}, Hold={holdQuantity}, Supplied={suppliedQuantity}, Remaining={remaining}");

            if (remaining <= item.MinimumQuantity)
            {
                return new LowStockItemInfo
                {
                    Item = item,
                    TotalStock = totalStock,
                    HoldQuantity = holdQuantity,
                    SuppliedQuantity = suppliedQuantity,
                    Remaining = remaining
                };
            }

            return null;
        }

        private async Task NotifyLowStockBatchAsync(List<LowStockItemInfo> lowStockItems)
        {
            // Get configured recipients from Settings
            var setting = await _settingsRepository.FindOneAsync(
                s => s.Key == SETTINGS_KEY && s.Group == SETTINGS_GROUP);

            List<string> userIds = new List<string>();
            List<string> roleIds = new List<string>();

            if (setting != null && !string.IsNullOrEmpty(setting.Value))
            {
                try
                {
                    var config = JsonSerializer.Deserialize<LowStockNotificationSettingsDto>(setting.Value);
                    if (config != null)
                    {
                        userIds = config.Users ?? new List<string>();
                        roleIds = config.Roles ?? new List<string>();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to deserialize notification settings.");
                }
            }

            // Fallback to default role if no configuration
            if (!userIds.Any() && !roleIds.Any())
            {
                var defaultRole = await _roleManager.FindByNameAsync(DEFAULT_ROLE_NAME);
                if (defaultRole != null)
                {
                    roleIds.Add(defaultRole.Id);
                    _logger.LogInformation($"No notification settings found. Using default role: {DEFAULT_ROLE_NAME}");
                }
                else
                {
                    _logger.LogWarning($"Default role '{DEFAULT_ROLE_NAME}' not found. No notifications will be sent.");
                    return;
                }
            }

            var itemCount = lowStockItems.Count;
            var title = $"⚠️ تنبيه المخزون المنخفض: {itemCount} {(itemCount > 1 ? "مواد" : "مادة")} تحت الحد الأدنى";
            var notificationMessage = $"{itemCount} {(itemCount > 1 ? "مواد انخفضت" : "مادة انخفض")} تحت الحد الأدنى للمخزون. يرجى التحقق من بريدك الإلكتروني للحصول على معلومات مفصلة.";
            var emailMessage = $"المواد التالية تحت الحد الأدنى للمخزون. يرجى مراجعة التفاصيل أدناه.";

            // Build HTML table for email content
            var htmlContent = $@"
                <div style='margin-top: 20px; margin-bottom: 30px; direction: rtl; text-align: right;'>
                    <h3 style='color: #1F3A5F; font-size: 18px; font-weight: 600; margin-bottom: 15px;'>تفاصيل المواد منخفضة المخزون</h3>
                    <table border='1' cellpadding='8' cellspacing='0' style='border-collapse: collapse; width: 100%; direction: rtl;'>
                        <thead>
                            <tr style='background-color: #f2f2f2;'>
                                <th style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>اسم المادة</th>
                                <th style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>رقم المادة</th>
                                <th style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>NSN</th>
                                <th style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>الحد الأدنى</th>
                                <th style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>إجمالي المخزون</th>
                                <th style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>إجمالي الكمية المحجوزة</th>
                                <th style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>إجمالي المصروف سابقا</th>
                                <th style='text-align: right; padding: 8px; border: 1px solid #6B6B6B; background-color: #ffe6e6;'>المتبقي</th>
                            </tr>
                        </thead>
                        <tbody>
            ";

            foreach (var itemInfo in lowStockItems)
            {
                var item = itemInfo.Item;
                htmlContent += $@"
                            <tr>
                                <td style='padding: 8px; border: 1px solid #6B6B6B;'>{WebUtility.HtmlEncode(item.Name)}</td>
                                <td style='padding: 8px; border: 1px solid #6B6B6B;'>{WebUtility.HtmlEncode(item.ItemNo)}</td>
                                <td style='padding: 8px; border: 1px solid #6B6B6B;'>{WebUtility.HtmlEncode(item.Nsn ?? "N/A")}</td>
                                <td style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>{item.MinimumQuantity}</td>
                                <td style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>{itemInfo.TotalStock}</td>
                                <td style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>{itemInfo.HoldQuantity}</td>
                                <td style='text-align: right; padding: 8px; border: 1px solid #6B6B6B;'>{itemInfo.SuppliedQuantity}</td>
                                <td style='text-align: right; padding: 8px; border: 1px solid #6B6B6B; font-weight: bold; background-color: #ffe6e6;'>{itemInfo.Remaining}</td>
                            </tr>
                ";
            }

            htmlContent += @"
                        </tbody>
                    </table>
                </div>
            ";

            _logger.LogInformation($"Sending Low Stock Notification for {itemCount} items to {userIds.Count} users and {roleIds.Count} roles.");

            try
            {
                await _notificationHelperService.SendNotificationAsync(
                    title,
                    notificationMessage,
                    entityType: null,
                    entityId: null,
                    userIds: userIds.Any() ? userIds : null,
                    roleIds: roleIds.Any() ? roleIds : null
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send push notification.");
            }

            try
            {
                await _notificationHelperService.SendEmailAsync(
                    title,
                    emailMessage,
                    entityType: null,
                    entityId: null,
                    userIds: userIds.Any() ? userIds : null,
                    roleIds: roleIds.Any() ? roleIds : null,
                    htmlContent: htmlContent
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email notification.");
            }
        }

        private class LowStockItemInfo
        {
            public BaseItem Item { get; set; } = null!;
            public long TotalStock { get; set; }
            public long HoldQuantity { get; set; }
            public long SuppliedQuantity { get; set; }
            public long Remaining { get; set; }
        }
    }
}
