using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Notification.Service;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities.Settings;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Ettad.CrossCutting.Comman.Idenitity;

namespace Ettad.Inventory.Service.Monitoring
{
    public class LowStockMonitorBackgroundService : ILowStockMonitorBackgroundService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationHelperService _notificationHelperService;
        private readonly ILogger<LowStockMonitorBackgroundService> _logger;
        private readonly ICrossCuttingRepository<Settings> _settingsRepository;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly LowStockEmailTemplateService _emailTemplateService;
        private readonly ILowStockMonitoringService _lowStockMonitoringService;

        public LowStockMonitorBackgroundService(
            ApplicationDbContext context,
            INotificationHelperService notificationHelperService,
            ILogger<LowStockMonitorBackgroundService> logger,
            ICrossCuttingRepository<Settings> settingsRepository,
            RoleManager<ApplicationRole> roleManager,
            LowStockEmailTemplateService emailTemplateService,
            ILowStockMonitoringService lowStockMonitoringService)
        {
            _context = context;
            _notificationHelperService = notificationHelperService;
            _logger = logger;
            _settingsRepository = settingsRepository;
            _roleManager = roleManager;
            _emailTemplateService = emailTemplateService;
            _lowStockMonitoringService = lowStockMonitoringService;
        }

        public async Task CheckAndNotifyAsync()
        {
            _logger.LogInformation("Starting Low Stock Check...");

            try
            {
                // Use the monitoring service to get low stock items
                var lowStockItemsResult = await _lowStockMonitoringService.GetLowStockItemsAsync();
                
                if (!lowStockItemsResult.Succeeded || lowStockItemsResult.Data == null)
                {
                    _logger.LogError("Failed to retrieve low stock items from monitoring service.");
                    return;
                }

                var lowStockItemDtos = lowStockItemsResult.Data;

                if (!lowStockItemDtos.Any())
                {
                    _logger.LogInformation("No items found below minimum stock level.");
                    return;
                }

                // Convert DTOs to LowStockItemInfo for notification
                var lowStockItems = new List<LowStockItemInfo>();
                foreach (var dto in lowStockItemDtos)
                {
                    var item = await _context.BaseItems
                        .FirstOrDefaultAsync(i => i.Id == dto.ItemId && !i.IsDeleted);
                    
                    if (item != null)
                    {
                        lowStockItems.Add(new LowStockItemInfo
                        {
                            Item = item,
                            TotalStock = dto.TotalStock,
                            HoldQuantity = dto.HoldQuantity,
                            SuppliedQuantity = dto.SuppliedQuantity,
                            Remaining = dto.Remaining
                        });
                    }
                }

                if (lowStockItems.Any())
                {
                    await NotifyLowStockBatchAsync(lowStockItems);
                }

                _logger.LogInformation("Low Stock Check completed.");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error occurred during Low Stock Check.");
                throw;
            }
        }

        private async Task NotifyLowStockBatchAsync(List<LowStockItemInfo> lowStockItems)
        {
            // Get configured recipients from Settings
            var setting = await _settingsRepository.FindOneAsync(
                s => s.Key == LowStockMonitorConstants.SETTINGS_KEY && s.Group == LowStockMonitorConstants.SETTINGS_GROUP);

            List<string> userIds = new List<string>();
            List<string> roleIds = new List<string>();

            if (setting != null && !string.IsNullOrEmpty(setting.Value))
            {
                try
                {
                    var config = JsonSerializer.Deserialize<Dtos.LowStockNotificationSettingsDto>(setting.Value);
                    if (config != null)
                    {
                        userIds = config.Users ?? new List<string>();
                        roleIds = config.Roles ?? new List<string>();
                    }
                }
                catch (System.Exception ex)
                {
                    _logger.LogError(ex, "Failed to deserialize notification settings.");
                }
            }

            // Fallback to default role if no configuration
            if (!userIds.Any() && !roleIds.Any())
            {
                var defaultRole = await _roleManager.FindByNameAsync(LowStockMonitorConstants.DEFAULT_ROLE_NAME);
                if (defaultRole != null)
                {
                    roleIds.Add(defaultRole.Id);
                    _logger.LogInformation($"No notification settings found. Using default role: {LowStockMonitorConstants.DEFAULT_ROLE_NAME}");
                }
                else
                {
                    _logger.LogWarning($"Default role '{LowStockMonitorConstants.DEFAULT_ROLE_NAME}' not found. No notifications will be sent.");
                    return;
                }
            }

            var itemCount = lowStockItems.Count;
            var title = $"⚠️ تنبيه المخزون المنخفض: {itemCount} {(itemCount > 1 ? "مواد" : "مادة")} تحت الحد الأدنى";
            var notificationMessage = $"{itemCount} {(itemCount > 1 ? "مواد انخفضت" : "مادة انخفض")} تحت الحد الأدنى للمخزون. يرجى التحقق من بريدك الإلكتروني للحصول على معلومات مفصلة.";
            var emailMessage = $"المواد التالية تحت الحد الأدنى للمخزون. يرجى مراجعة التفاصيل أدناه.";

            // Generate HTML content using template service
            var htmlContent = _emailTemplateService.GenerateEmailHtmlContent(lowStockItems);

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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to send email notification.");
            }
        }
    }
}
