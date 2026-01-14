using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities.Settings;
using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.Monitoring
{
    public class LowStockMonitorSettingsService : ILowStockMonitorSettingsService
    {
        private readonly ICrossCuttingRepository<Settings> _settingsRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<LowStockMonitorSettingsService> _logger;
        private readonly IRecurringJobManager? _recurringJobManager;

        public LowStockMonitorSettingsService(
            ICrossCuttingRepository<Settings> settingsRepository,
            ICurrentUserService currentUserService,
            ILogger<LowStockMonitorSettingsService> logger,
            IRecurringJobManager? recurringJobManager = null)
        {
            _settingsRepository = settingsRepository;
            _currentUserService = currentUserService;
            _logger = logger;
            _recurringJobManager = recurringJobManager;
        }

        public async Task<APIOperationResponse<LowStockNotificationSettingsDto>> GetSettingsAsync()
        {
            try
            {
                var setting = await _settingsRepository.FindOneAsync(
                    s => s.Key == LowStockMonitorConstants.SETTINGS_KEY && s.Group == LowStockMonitorConstants.SETTINGS_GROUP);

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
                    s => s.Key == LowStockMonitorConstants.SETTINGS_KEY && s.Group == LowStockMonitorConstants.SETTINGS_GROUP);

                var jsonValue = JsonSerializer.Serialize(dto);

                if (setting == null)
                {
                    setting = new Settings
                    {
                        Key = LowStockMonitorConstants.SETTINGS_KEY,
                        Group = LowStockMonitorConstants.SETTINGS_GROUP,
                        Value = jsonValue,
                        CreationDate = DateTime.Now,
                        CreatedBy = _currentUserService.UserId
                    };
                    await _settingsRepository.AddAsync(setting);
                }
                else
                {
                    setting.Value = jsonValue;
                    setting.ModificationDate = DateTime.Now;
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
                    s => s.Key == LowStockMonitorConstants.SCHEDULE_SETTINGS_KEY && s.Group == LowStockMonitorConstants.SCHEDULE_SETTINGS_GROUP);

                var cronExpression = setting?.Value ?? LowStockMonitorConstants.DEFAULT_CRON_EXPRESSION;
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
                // Use local time directly for cron expression
                // Convert to cron expression format: "minute hour * * *" (daily at specified time)
                var cronExpression = $"{scheduleTime.Minute} {scheduleTime.Hour} * * *";

                var setting = await _settingsRepository.FindOneAsync(
                    s => s.Key == LowStockMonitorConstants.SCHEDULE_SETTINGS_KEY && s.Group == LowStockMonitorConstants.SCHEDULE_SETTINGS_GROUP);

                if (setting == null)
                {
                    setting = new Settings
                    {
                        Key = LowStockMonitorConstants.SCHEDULE_SETTINGS_KEY,
                        Group = LowStockMonitorConstants.SCHEDULE_SETTINGS_GROUP,
                        Value = cronExpression,
                        CreationDate = DateTime.Now,
                        CreatedBy = _currentUserService.UserId
                    };
                    await _settingsRepository.AddAsync(setting);
                }
                else
                {
                    setting.Value = cronExpression;
                    setting.ModificationDate = DateTime.Now;
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
                            cronExpression);
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

