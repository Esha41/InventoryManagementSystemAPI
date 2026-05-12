using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.Monitoring.Interfaces
{
    public interface ICriticalStockMonitoringService
    {
        Task<APIOperationResponse<int>> GetCriticalStockItemsCountAsync(long? depotId = null, List<long>? depotIds = null);

        Task<APIOperationResponse<List<CriticalStockItemDto>>> GetCriticalStockItemsAsync();
    }
}
