using Ettad.Module.lookup.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Lookups.Services.Contracts
{
    /// <summary>
    /// Service for managing user-depot assignments.
    /// </summary>
    public interface IUserDepotService
    {
        /// <summary>
        /// Gets the list of users assigned to a depot.
        /// </summary>
        Task<APIOperationResponse<List<DepotUserDto>>> GetUsersByDepotIdAsync(long depotId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets the user assignments for a depot. Replaces existing assignments.
        /// </summary>
        Task<APIOperationResponse<bool>> SetDepotUserAssignmentsAsync(long depotId, IEnumerable<string> userIds, CancellationToken cancellationToken = default);
    }
}
