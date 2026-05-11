using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.Monitoring.Interfaces
{
    public interface ICriticalStockMonitorSettingsService
    {
        Task<APIOperationResponse<ItemNotificationSettingsDto>> GetSettingsAsync();
        Task<APIOperationResponse<bool>> UpdateSettingsAsync(ItemNotificationSettingsDto dto);
        Task<APIOperationResponse<DateTime?>> GetScheduleAsync();
        Task<APIOperationResponse<bool>> UpdateScheduleAsync(DateTimeOffset scheduleTime);
    }
}
