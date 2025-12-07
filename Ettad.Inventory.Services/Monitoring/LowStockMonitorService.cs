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
        private const string SETTINGS_KEY = "LowStockNotificationRecipients";
        private const string SETTINGS_GROUP = "LowStockNotifications";
        private const string DEFAULT_ROLE_NAME = "Head of Depo Division (Inventory)";

        public LowStockMonitorService(
            ApplicationDbContext context,
            INotificationHelperService notificationHelperService,
            ILogger<LowStockMonitorService> logger,
            ICrossCuttingRepository<Settings> settingsRepository,
            ICurrentUserService currentUserService,
            RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _notificationHelperService = notificationHelperService;
            _logger = logger;
            _settingsRepository = settingsRepository;
            _currentUserService = currentUserService;
            _roleManager = roleManager;
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

                foreach (var item in itemsToCheck)
                {
                    await CheckItemStockAsync(item);
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

        private async Task CheckItemStockAsync(BaseItem item)
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
                await NotifyLowStockAsync(item, totalStock, holdQuantity, suppliedQuantity, remaining);
            }
        }

        private async Task NotifyLowStockAsync(BaseItem item, long totalStock, long holdQuantity, long suppliedQuantity, long remaining)
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

            var title = $"⚠️ Low Stock Alert: {item.Name}";
            var message = $"Item {item.Name} has dropped below minimum level. Remaining: {remaining}. Check email for details.";

            var emailBody = $@"
                <h3>Low Stock Warning - {item.Name}</h3>
                <p><strong>Item No:</strong> {item.ItemNo}</p>
                <p><strong>Part No:</strong> {item.PartNo ?? "N/A"}</p>
                <p><strong>NSN:</strong> {item.Nsn ?? "N/A"}</p>
                <p><strong>Minimum Level:</strong> {item.MinimumQuantity}</p>
                <br/>
                <table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse;'>
                    <tr>
                        <th style='background-color: #f2f2f2;'>Metric</th>
                        <th style='background-color: #f2f2f2;'>Quantity</th>
                    </tr>
                    <tr>
                        <td>📥 Total In Stock</td>
                        <td>{totalStock}</td>
                    </tr>
                    <tr>
                        <td>✋ On Hold (Drafts)</td>
                        <td>{holdQuantity}</td>
                    </tr>
                    <tr>
                        <td>📤 Supplied (Submitted)</td>
                        <td>{suppliedQuantity}</td>
                    </tr>
                    <tr style='font-weight: bold; background-color: #ffe6e6;'>
                        <td>✅ Remaining Available</td>
                        <td>{remaining}</td>
                    </tr>
                </table>
            ";

            _logger.LogInformation($"Sending Low Stock Notification for Item {item.Id} to {userIds.Count} users and {roleIds.Count} roles.");

            try
            {
                await _notificationHelperService.SendNotificationAsync(
                    title,
                    message,
                    entityType: "Item",
                    entityId: item.Id,
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
                    emailBody,
                    entityType: "Item",
                    entityId: item.Id,
                    userIds: userIds.Any() ? userIds : null,
                    roleIds: roleIds.Any() ? roleIds : null
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email notification.");
            }
        }
    }
}
