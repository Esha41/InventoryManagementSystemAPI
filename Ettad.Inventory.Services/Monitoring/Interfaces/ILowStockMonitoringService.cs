using Ettad.CrossCutting.Comman.Models;
using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Models;
using System.Collections.Generic;

namespace Ettad.Inventory.Service.Monitoring.Interfaces
{
    public interface ILowStockMonitoringService
    {
        /// <summary>
        /// Gets the count of items that are below minimum stock level, optionally filtered by depot.
        /// </summary>
        Task<APIOperationResponse<int>> GetLowStockItemsCountAsync(long? depotId = null, List<long>? depotIds = null);

        /// <summary>
        /// Gets all low-stock rows (background jobs / integrations). Prefer <see cref="GetLowStockItemsPaginatedAsync"/> for APIs.
        /// </summary>
        Task<APIOperationResponse<List<LowStockItemDto>>> GetLowStockItemsAsync();

        /// <summary>
        /// Low-stock table rows with server-side paging, optionally filtered by depot (same rules as count).
        /// </summary>
        Task<APIOperationResponse<PaginatedList<LowStockItemDto>>> GetLowStockItemsPaginatedAsync(
            PagedListRequest request,
            long? depotId = null,
            List<long>? depotIds = null);
    }
}
