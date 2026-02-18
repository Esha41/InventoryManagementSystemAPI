using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Lookups.Services.Contracts;

namespace Ettad.Lookups.Services.Implementation
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

            // Users with depot management permission (Depots.Page only) have access to all depots.
            // Depots.View allows viewing depot info but access is still filtered by UserDepot assignments.
            var hasDepotManagement = await _permissionService.HasPermissionAsync("Permissions.Depots.Page");
            if (hasDepotManagement)
            {
                return true;
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
    }
}
