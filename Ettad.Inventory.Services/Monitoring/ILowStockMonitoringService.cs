using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.Monitoring
{
    public interface ILowStockMonitoringService
    {
        /// <summary>
        /// Gets the count of items that are below minimum stock level
        /// </summary>
        Task<APIOperationResponse<int>> GetLowStockItemsCountAsync();

        /// <summary>
        /// Gets the list of items that are below minimum stock level with their details
        /// </summary>
        Task<APIOperationResponse<List<LowStockItemDto>>> GetLowStockItemsAsync();
    }
}
