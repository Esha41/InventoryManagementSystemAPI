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

        public LowStockMonitorBackgroundService(
            ApplicationDbContext context,
            INotificationHelperService notificationHelperService,
            ILogger<LowStockMonitorBackgroundService> logger,
            ICrossCuttingRepository<Settings> settingsRepository,
            RoleManager<ApplicationRole> roleManager,
            LowStockEmailTemplateService emailTemplateService)
        {
            _context = context;
            _notificationHelperService = notificationHelperService;
            _logger = logger;
            _settingsRepository = settingsRepository;
            _roleManager = roleManager;
            _emailTemplateService = emailTemplateService;
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
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error occurred during Low Stock Check.");
                throw;
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

