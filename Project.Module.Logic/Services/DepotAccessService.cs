using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Application.Common.Interfaces;
using Ettad.Data.Entities;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Module.lookup.Interfaces;

namespace Ettad.Module.lookup.Services
{
    /// <summary>
    /// Service for checking depot access based on UserDepot assignments.
    /// </summary>
    public class DepotAccessService : IDepotAccessService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ICrossCuttingRepository<UserDepot> _userDepotRepository;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<DepotAccessService> _logger;

        public DepotAccessService(
            ICurrentUserService currentUserService,
            ICrossCuttingRepository<UserDepot> userDepotRepository,
            IPermissionService permissionService,
            ILogger<DepotAccessService> logger)
        {
            _currentUserService = currentUserService;
            _userDepotRepository = userDepotRepository;
            _permissionService = permissionService;
            _logger = logger;
        }

        public async Task<bool> HasDepotAccessAsync(string userId, long depotId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("HasDepotAccessAsync called with null or empty userId");
                return false;
            }

            // SuperAdmin always has access
            if (_currentUserService.IsSuperAdmin)
            {
                return true;
            }

            // Depots.ViewAll: read any depot (no UserDepot filter).
            var hasViewAll = await _permissionService.HasPermissionAsync("Permissions.Depots.ViewAll");
            if (hasViewAll)
            {
                return true;
            }

            // Scoped read: Depots.View (read depots) or Inventory.View (read inventory in assigned depots). Depots.Page alone does not grant access.
            var canReadScoped = await _permissionService.HasPermissionAsync("Permissions.Depots.View")
                || await _permissionService.HasPermissionAsync("Permissions.Inventory.View");
            if (!canReadScoped)
            {
                _logger.LogDebug("User {UserId} lacks Depots.View/Inventory.View — denying access to depot {DepotId}", userId, depotId);
                return false;
            }

            var hasAccess = await _userDepotRepository
                .Find(ud => ud.UserId == userId && ud.DepotId == depotId)
                .AnyAsync(cancellationToken);

            if (!hasAccess)
            {
                _logger.LogDebug("User {UserId} does not have access to depot {DepotId}", userId, depotId);
            }

            return hasAccess;
        }

        public async Task<List<long>?> GetUserAccessibleDepotIdsAsync(CancellationToken cancellationToken = default)
        {
            if (_currentUserService.IsSuperAdmin)
            {
                _logger.LogDebug("SuperAdmin — unrestricted depot access");
                return null;
            }

            var hasViewAll = await _permissionService.HasPermissionAsync("Permissions.Depots.ViewAll");
            if (hasViewAll)
            {
                _logger.LogDebug("User has Depots.ViewAll — unrestricted depot access");
                return null;
            }

            var canReadScoped = await _permissionService.HasPermissionAsync("Permissions.Depots.View")
                || await _permissionService.HasPermissionAsync("Permissions.Inventory.View");
            if (!canReadScoped)
            {
                _logger.LogDebug("User {UserId} lacks Depots.View/Inventory.View — no depot access", _currentUserService.UserId);
                return new List<long>();
            }

            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("UserId is null — returning empty depot list");
                return new List<long>();
            }

            var depotIds = await _userDepotRepository
                .Find(ud => ud.UserId == userId)
                .Select(ud => ud.DepotId)
                .ToListAsync(cancellationToken);

            _logger.LogDebug("User {UserId} has access to {Count} depots: [{DepotIds}]",
                userId, depotIds.Count, string.Join(", ", depotIds));

            return depotIds;
        }
    }
}
