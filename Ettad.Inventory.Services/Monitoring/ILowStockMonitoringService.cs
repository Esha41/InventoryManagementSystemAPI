using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Models;
using System.Collections.Generic;

namespace Ettad.Inventory.Service.Monitoring
{
    public interface ILowStockMonitoringService
    {
        /// <summary>
        /// Gets the count of items that are below minimum stock level, optionally filtered by depot.
        /// </summary>
        Task<APIOperationResponse<int>> GetLowStockItemsCountAsync(long? depotId = null, List<long>? depotIds = null);

        /// <summary>
        /// Gets the list of items that are below minimum stock level with their details
        /// </summary>
        Task<APIOperationResponse<List<LowStockItemDto>>> GetLowStockItemsAsync();
    }
}
