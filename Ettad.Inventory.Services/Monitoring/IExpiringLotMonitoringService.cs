using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Models;
using System.Collections.Generic;

namespace Ettad.Inventory.Service.Monitoring
{
    public interface IExpiringLotMonitoringService
    {
        /// <summary>
        /// Gets the count of lots that are about to expire in the next 30 days, optionally filtered by depot.
        /// </summary>
        Task<APIOperationResponse<int>> GetExpiringLotsCountAsync(long? depotId = null, List<long>? depotIds = null);

        /// <summary>
        /// Gets the list of lots that are about to expire in the next 30 days with their details
        /// </summary>
        Task<APIOperationResponse<List<ExpiringLotDto>>> GetExpiringLotsAsync();
    }
}
