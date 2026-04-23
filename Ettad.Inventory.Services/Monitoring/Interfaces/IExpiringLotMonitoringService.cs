using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.Monitoring.Interfaces
{
    public interface IExpiringLotMonitoringService
    {
        /// <summary>
        /// Gets the count of lots that are about to expire in the next 30 days, optionally filtered by depot.
        /// </summary>
        Task<APIOperationResponse<int>> GetExpiringLotsCountAsync(long? depotId = null);

        /// <summary>
        /// Gets the list of lots that are about to expire in the next 30 days with their details
        /// </summary>
        Task<APIOperationResponse<List<ExpiringLotDto>>> GetExpiringLotsAsync();
    }
}
