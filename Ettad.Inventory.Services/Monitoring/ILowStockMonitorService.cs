using System.Collections.Generic;
using System.Threading.Tasks;
using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.Monitoring
{
    public interface ILowStockMonitorService
    {
        Task CheckAndNotifyAsync();
        
        Task<APIOperationResponse<LowStockNotificationSettingsDto>> GetSettingsAsync();
        Task<APIOperationResponse<bool>> UpdateSettingsAsync(LowStockNotificationSettingsDto dto);
    }
}
