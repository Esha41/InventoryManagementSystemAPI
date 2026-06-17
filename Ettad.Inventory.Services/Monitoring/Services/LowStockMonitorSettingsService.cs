using System.Text.Json;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Application.Common.Interfaces;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.Data.Entities.Settings;
using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Constants;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Inventory.Service.Monitoring.Interfaces;
using Ettad.Inventory.Service.Monitoring.BackgroundJobs;

namespace Ettad.Inventory.Service.Monitoring.Services
{
    public class LowStockMonitorSettingsService : ILowStockMonitorSettingsService
    {
        private readonly ICrossCuttingRepository<Settings> _settingsRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<LowStockMonitorSettingsService> _logger;
        private readonly IRecurringJobManager? _recurringJobManager;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LowStockMonitorSettingsService(
            ICrossCuttingRepository<Settings> settingsRepository,
            ICurrentUserService currentUserService,
            ILogger<LowStockMonitorSettingsService> logger,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IRecurringJobManager? recurringJobManager = null,
            IDateTimeProvider dateTimeProvider = null)
        {
            _settingsRepository = settingsRepository;
            _currentUserService = currentUserService;
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
            _recurringJobManager = recurringJobManager;
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public async Task<APIOperationResponse<ItemNotificationSettingsDto>> GetSettingsAsync()
        {
            try
            {
                var setting = await _settingsRepository.FindOneAsync(
                    s => s.Key == LowStockMonitorConstants.SETTINGS_KEY && s.Group == LowStockMonitorConstants.SETTINGS_GROUP);

                if (setting == null)
                {
                    return APIOperationResponse<ItemNotificationSettingsDto>.Success(
                        new ItemNotificationSettingsDto { Roles = new List<string>(), Users = new List<string>() });
                }

                var dto = JsonSerializer.Deserialize<ItemNotificationSettingsDto>(setting.Value ?? "{}");
                return APIOperationResponse<ItemNotificationSettingsDto>.Success(dto ?? new ItemNotificationSettingsDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving notification settings.");
                return APIOperationResponse<ItemNotificationSettingsDto>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateSettingsAsync(ItemNotificationSettingsDto dto)
        {
            try
            {
                // Reject the request if any supplied role/user ID does not exist, instead of
                // blindly persisting unvalidated references into the settings blob.
                if (dto.Roles != null && dto.Roles.Any())
                {
                    var existingRoleIds = await _roleManager.Roles
                        .Where(r => dto.Roles.Contains(r.Id))
                        .Select(r => r.Id)
                        .ToListAsync();

                    var invalidRoleIds = dto.Roles.Distinct()
                        .Where(id => !existingRoleIds.Contains(id, StringComparer.OrdinalIgnoreCase))
                        .ToList();

                    if (invalidRoleIds.Any())
                        return APIOperationResponse<bool>.BadRequest(
                            $"Invalid role IDs: {string.Join(", ", invalidRoleIds)}");
                }

                if (dto.Users != null && dto.Users.Any())
                {
                    var existingUserIds = await _userManager.Users
                        .Where(u => dto.Users.Contains(u.Id))
                        .Select(u => u.Id)
                        .ToListAsync();

                    var invalidUserIds = dto.Users.Distinct()
                        .Where(id => !existingUserIds.Contains(id, StringComparer.OrdinalIgnoreCase))
                        .ToList();

                    if (invalidUserIds.Any())
                        return APIOperationResponse<bool>.BadRequest(
                            $"Invalid user IDs: {string.Join(", ", invalidUserIds)}");
                }

                var setting = await _settingsRepository.FindOneAsync(
                    s => s.Key == LowStockMonitorConstants.SETTINGS_KEY && s.Group == LowStockMonitorConstants.SETTINGS_GROUP);

                var jsonValue = JsonSerializer.Serialize(dto);

                if (setting == null)
                {
                    setting = new Settings
                    {
                        Key = LowStockMonitorConstants.SETTINGS_KEY,
                        Group = LowStockMonitorConstants.SETTINGS_GROUP,
                        Value = jsonValue,
                        CreationDate = _dateTimeProvider.Now,
                        CreatedBy = _currentUserService.UserId
                    };
                    await _settingsRepository.AddAsync(setting);
                }
                else
                {
                    setting.Value = jsonValue;
                    setting.ModificationDate = _dateTimeProvider.Now;
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

        public async Task<APIOperationResponse<DateTime?>> GetScheduleAsync()
        {
            try
            {
                var setting = await _settingsRepository.FindOneAsync(
                    s => s.Key == LowStockMonitorConstants.SCHEDULE_SETTINGS_KEY && s.Group == LowStockMonitorConstants.SCHEDULE_SETTINGS_GROUP);

                var cronExpression = setting?.Value ?? LowStockMonitorConstants.DEFAULT_CRON_EXPRESSION;
                
                // Parse cron expression format: "minute hour * * *" to DateTime
                // Use today's date with the time from cron expression
                var parts = cronExpression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2 && int.TryParse(parts[0], out int minute) && int.TryParse(parts[1], out int hour))
                {
                    var today = _dateTimeProvider.Now.Date;
                    var scheduleTime = DateTime.SpecifyKind(
                        new DateTime(today.Year, today.Month, today.Day, hour, minute, 0),
                        DateTimeKind.Local);
                    return APIOperationResponse<DateTime?>.Success(scheduleTime);
                }
                
                // If parsing fails, return null
                _logger.LogWarning("Failed to parse cron expression: {CronExpression}", cronExpression);
                return APIOperationResponse<DateTime?>.Success(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving schedule.");
                return APIOperationResponse<DateTime?>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateScheduleAsync(DateTimeOffset scheduleTime)
        {
            try
            {
                // Cron stores minute/hour for server-local wall clock; Hangfire must use the same zone (defaults to UTC otherwise).
                var onServerLocal = TimeZoneInfo.ConvertTime(scheduleTime, TimeZoneInfo.Local);
                var cronExpression = $"{onServerLocal.Minute} {onServerLocal.Hour} * * *";
                var hangfireLocalTz = new RecurringJobOptions { TimeZone = TimeZoneInfo.Local };

                var setting = await _settingsRepository.FindOneAsync(
                    s => s.Key == LowStockMonitorConstants.SCHEDULE_SETTINGS_KEY && s.Group == LowStockMonitorConstants.SCHEDULE_SETTINGS_GROUP);

                if (setting == null)
                {
                    setting = new Settings
                    {
                        Key = LowStockMonitorConstants.SCHEDULE_SETTINGS_KEY,
                        Group = LowStockMonitorConstants.SCHEDULE_SETTINGS_GROUP,
                        Value = cronExpression,
                        CreationDate = _dateTimeProvider.Now,
                        CreatedBy = _currentUserService.UserId
                    };
                    await _settingsRepository.AddAsync(setting);
                }
                else
                {
                    setting.Value = cronExpression;
                    setting.ModificationDate = _dateTimeProvider.Now;
                    setting.ModifiedBy = _currentUserService.UserId;
                    await _settingsRepository.UpdateAsync(setting);
                }

                _logger.LogInformation("Low Stock Monitor schedule updated to: {ScheduleTime} (Cron: {CronExpression})", 
                    scheduleTime, cronExpression);
                
                // Update Hangfire recurring job immediately if available
                if (_recurringJobManager != null)
                {
                    try
                    {
                        // Register recurring job - Hangfire resolves LowStockMonitorJob from DI at execution time
                        _recurringJobManager.AddOrUpdate<LowStockMonitorJob>(
                            LowStockMonitorConstants.JOB_ID,
                            job => job.ExecuteAsync(),
                            cronExpression,
                            hangfireLocalTz);
                        _logger.LogInformation("Hangfire recurring job '{JobId}' updated with new schedule: {CronExpression}", 
                            LowStockMonitorConstants.JOB_ID, cronExpression);
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
                    _logger.LogWarning("IRecurringJobManager not available. Schedule saved to database but job will use new schedule on next application restart.");
                    return APIOperationResponse<bool>.Success(true, "Schedule saved successfully. Note: The recurring job will be updated on next application restart.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating schedule.");
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }
    }
}

