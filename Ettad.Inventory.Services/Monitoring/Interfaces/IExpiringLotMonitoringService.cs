using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.CrossCutting.Comman.Models;
using Ettad.ResponseHandler.Models;
using System.Collections.Generic;

namespace Ettad.Inventory.Service.Monitoring.Interfaces
{
    public interface IExpiringLotMonitoringService
    {
        /// <summary>
        /// Gets the count of lots that are about to expire in the next 30 days, optionally filtered by depot.
        /// </summary>
        Task<APIOperationResponse<int>> GetExpiringLotsCountAsync(long? depotId = null, List<long>? depotIds = null);

        /// <summary>
        /// Expiring-lot rows (next 30 days) with server-side paging. Same depot rules as <see cref="GetExpiringLotsCountAsync"/>.
        /// </summary>
        Task<APIOperationResponse<PaginatedList<ExpiringLotDto>>> GetExpiringLotsPaginatedAsync(
            PagedListRequest request,
            long? depotId = null,
            List<long>? depotIds = null);
    }
}
