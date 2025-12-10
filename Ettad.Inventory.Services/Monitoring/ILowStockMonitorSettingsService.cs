using System;
using System.Threading.Tasks;
using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.Monitoring
{
    public interface ILowStockMonitorSettingsService
    {
        Task<APIOperationResponse<LowStockNotificationSettingsDto>> GetSettingsAsync();
        Task<APIOperationResponse<bool>> UpdateSettingsAsync(LowStockNotificationSettingsDto dto);
        
        Task<APIOperationResponse<string>> GetScheduleAsync();
        Task<APIOperationResponse<bool>> UpdateScheduleAsync(DateTime scheduleTime);
    }
}

