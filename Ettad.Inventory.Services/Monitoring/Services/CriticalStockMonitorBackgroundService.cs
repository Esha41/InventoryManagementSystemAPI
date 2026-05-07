using Microsoft.Extensions.Logging;
using Ettad.Data.Entities;
using Ettad.Data.Entities.Settings;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.Data.Constants;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Inventory.Service.Monitoring.Interfaces;
using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.Notification.Service.Interfaces;

namespace Ettad.Inventory.Service.Monitoring.Services
{
    public class CriticalStockMonitorBackgroundService : ICriticalStockMonitorBackgroundService
    {
        private readonly ICrossCuttingRepository<BaseItem> _baseItemRepository;
        private readonly ICrossCuttingRepository<Settings> _settingsRepository;
        private readonly INotificationHelperService _notificationHelperService;
        private readonly ICriticalStockMonitoringService _criticalStockMonitoringService;
        private readonly ILogger<CriticalStockMonitorBackgroundService> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly CriticalStockEmailTemplateService _emailTemplateService;

        public CriticalStockMonitorBackgroundService(
            ICrossCuttingRepository<BaseItem> baseItemRepository,
            INotificationHelperService notificationHelperService,
            ILogger<CriticalStockMonitorBackgroundService> logger,
            ICrossCuttingRepository<Settings> settingsRepository,
            RoleManager<ApplicationRole> roleManager,
            CriticalStockEmailTemplateService emailTemplateService,
            ICriticalStockMonitoringService criticalStockMonitoringService)
        {
            _baseItemRepository = baseItemRepository;
            _notificationHelperService = notificationHelperService;
            _logger = logger;
            _settingsRepository = settingsRepository;
            _roleManager = roleManager;
            _emailTemplateService = emailTemplateService;
            _criticalStockMonitoringService = criticalStockMonitoringService;
        }

        public async Task CheckAndNotifyAsync()
        {
            _logger.LogInformation("Starting Critical Stock Check...");

            try
            {
                var result = await _criticalStockMonitoringService.GetCriticalStockItemsAsync();

                if (!result.Succeeded || result.Data == null)
                {
                    _logger.LogError("Failed to retrieve critical stock items from monitoring service.");
                    return;
                }

                var dtos = result.Data;

                if (!dtos.Any())
                {
                    _logger.LogInformation("No items found at or below critical stock level.");
                    return;
                }

                var criticalItems = new List<LowStockItemInfo>();
                foreach (var dto in dtos)
                {
                    var item = await _baseItemRepository.FindOneAsync(i => i.Id == dto.ItemId && !i.IsDeleted);
                    if (item != null)
                    {
                        criticalItems.Add(new LowStockItemInfo
                        {
                            Item = item,
                            TotalStock = dto.TotalStock,
                            HoldQuantity = dto.HoldQuantity,
                            SuppliedQuantity = dto.SuppliedQuantity,
                            Remaining = dto.Remaining
                        });
                    }
                }

                if (criticalItems.Any())
                {
                    await NotifyCriticalStockBatchAsync(criticalItems);
                }

                _logger.LogInformation("Critical Stock Check completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during Critical Stock Check.");
                throw;
            }
        }

        private async Task NotifyCriticalStockBatchAsync(List<LowStockItemInfo> criticalStockItems)
        {
            var setting = await _settingsRepository.FindOneAsync(
                s => s.Key == CriticalStockMonitorConstants.SETTINGS_KEY && s.Group == CriticalStockMonitorConstants.SETTINGS_GROUP);

            List<string> userIds = new List<string>();
            List<string> roleIds = new List<string>();

            if (setting != null && !string.IsNullOrEmpty(setting.Value))
            {
                try
                {
                    var config = JsonSerializer.Deserialize<ItemNotificationSettingsDto>(setting.Value);
                    if (config != null)
                    {
                        userIds = config.Users ?? new List<string>();
                        roleIds = config.Roles ?? new List<string>();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to deserialize critical stock notification settings.");
                }
            }

            if (!userIds.Any() && !roleIds.Any())
            {
                var defaultRole = await _roleManager.FindByNameAsync(CriticalStockMonitorConstants.DEFAULT_ROLE_NAME);
                if (defaultRole != null)
                {
                    roleIds.Add(defaultRole.Id);
                    _logger.LogInformation("No critical stock notification settings found. Using default role: {Role}",
                        CriticalStockMonitorConstants.DEFAULT_ROLE_NAME);
                }
                else
                {
                    _logger.LogWarning("Default role '{Role}' not found. No critical stock notifications will be sent.",
                        CriticalStockMonitorConstants.DEFAULT_ROLE_NAME);
                    return;
                }
            }

            var itemCount = criticalStockItems.Count;
            var title = $"🚨 تنبيه المخزون الحرج: {itemCount} {(itemCount > 1 ? "مواد" : "مادة")} عند أو تحت الحد الحرج";
            var notificationMessage = $"{itemCount} {(itemCount > 1 ? "مواد وصلت" : "مادة وصلت")} إلى الحد الحرج للمخزون. يرجى التحقق من بريدك الإلكتروني للحصول على معلومات مفصلة.";
            var emailMessage = "المواد التالية عند أو تحت الحد الحرج للمخزون. يرجى مراجعة التفاصيل أدناه.";

            var htmlContent = _emailTemplateService.GenerateEmailHtmlContent(criticalStockItems);

            _logger.LogInformation("Sending Critical Stock Notification for {Count} items to {UserCount} users and {RoleCount} roles.",
                itemCount, userIds.Count, roleIds.Count);

            try
            {
                await _notificationHelperService.SendNotificationAsync(
                    title,
                    notificationMessage,
                    entityType: null,
                    entityId: null,
                    userIds: userIds.Any() ? userIds : null,
                    roleIds: roleIds.Any() ? roleIds : null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send critical stock push notification.");
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
                    htmlContent: htmlContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send critical stock email notification.");
            }
        }
    }
}
