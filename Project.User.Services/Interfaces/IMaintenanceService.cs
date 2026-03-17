using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace Ettad.User.Services.Interfaces;

/// <summary>
/// Service for managing application maintenance mode.
/// </summary>
public interface IMaintenanceService
{
    /// <summary>
    /// Gets the current maintenance mode status. Public endpoint - no auth required.
    /// </summary>
    Task<APIOperationResponse<MaintenanceStatusDto>> GetStatusAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets maintenance mode on or off. Requires admin permission.
    /// </summary>
    Task<APIOperationResponse<bool>> SetEnabledAsync(bool isEnabled, CancellationToken cancellationToken = default);
}
