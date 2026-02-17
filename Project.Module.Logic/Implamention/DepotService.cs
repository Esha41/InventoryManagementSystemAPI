using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Lookups.Services.Contracts;
using Ettad.Module.lookup.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;

namespace Ettad.Lookups.Services.Implementation
{
    public class DepotService : LookupService<Depot, CreateUpdateDepotDto>, IDepotService
    {
        private readonly ICrossCuttingRepository<Inventory> _inventoryRepository;
        private readonly ICrossCuttingRepository<UserDepot> _userDepotRepository;
        private readonly IPermissionService _permissionService;

        public DepotService(
            ICrossCuttingRepository<Depot> repository,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider,
            ILogger<LookupService<Depot, CreateUpdateDepotDto>> logger,
            ICrossCuttingRepository<Inventory> inventoryRepository,
            ICrossCuttingRepository<UserDepot> userDepotRepository,
            IPermissionService permissionService)
            : base(repository, mapper, currentUserService, dateTimeProvider, logger)
        {
            _inventoryRepository = inventoryRepository;
            _userDepotRepository = userDepotRepository;
            _permissionService = permissionService;
        }

        public override async Task<APIOperationResponse<List<Depot>>> GetLookupItems(bool includeDeleted = false)
        {
            try
            {
                _logger.LogInformation("Retrieving Depot lookup items (IncludeDeleted: {IncludeDeleted}) by User {UserName}",
                    includeDeleted, _currentUserService.UserName);

                var items = _repository.Find(x => includeDeleted || !x.IsDeleted);
                var allDepots = await items.ToListAsync();

                // SuperAdmin sees all depots
                if (_currentUserService.IsSuperAdmin)
                {
                    _logger.LogInformation("SuperAdmin user - returning all {Count} depots", allDepots.Count);
                    return APIOperationResponse<List<Depot>>.Success(allDepots);
                }

                // Users with depot management permission (Depots.Page only) see all depots (for depot management page).
                // Depots.View allows viewing depot info but list is filtered by UserDepot assignments.
                var hasDepotManagement = await _permissionService.HasPermissionAsync("Permissions.Depots.Page");
                if (hasDepotManagement)
                {
                    _logger.LogInformation("User has depot management permission - returning all {Count} depots", allDepots.Count);
                    return APIOperationResponse<List<Depot>>.Success(allDepots);
                }

                // Filter by UserDepot assignments
                var userId = _currentUserService.UserId;
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("UserId is null - returning empty depot list");
                    return APIOperationResponse<List<Depot>>.Success(new List<Depot>());
                }

                var assignedDepotIds = await _userDepotRepository
                    .Find(ud => ud.UserId == userId)
                    .Select(ud => ud.DepotId)
                    .ToListAsync();

                var filteredDepots = allDepots.Where(d => assignedDepotIds.Contains(d.Id)).ToList();
                _logger.LogInformation("User {UserId} has access to {Count} depots", userId, filteredDepots.Count);

                return APIOperationResponse<List<Depot>>.Success(filteredDepots);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Depot lookup items");
                return APIOperationResponse<List<Depot>>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.OPERATION_FAILED,
                    $"Error retrieving depot items: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<Depot>> DeleteDepotAsync(long id, CreateUpdateDepotDto item)
        {
            try
            {
                _logger.LogInformation("User {UserName} attempting to delete Depot with Id {Id}", 
                    _currentUserService.UserName, id);

                // Check if depot has any inventory records
                var hasInventory = await _inventoryRepository
                    .Find(i => i.DepoId == id && !i.IsDeleted)
                    .AnyAsync();

                if (hasInventory)
                {
                    _logger.LogWarning("Cannot delete Depot with Id {Id} because it has associated inventory records", id);
                    return APIOperationResponse<Depot>.Fail(
                        ResponseType.BadRequest,
                        CommonErrorCodes.OPERATION_FAILED,
                        "Cannot delete depot because it has associated inventory records. Please remove or transfer the inventory first.");
                }

                // If no inventory, proceed with soft delete
                return await SoftDeleteLookupItem(id, item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Depot with Id {Id}", id);
                return APIOperationResponse<Depot>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.OPERATION_FAILED,
                    $"Error deleting depot: {ex.Message}");
            }
        }
    }
}
