using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Models;
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

                // Depots.ViewAll: read all depots (no UserDepot filter).
                var hasViewAll = await _permissionService.HasPermissionAsync("Permissions.Depots.ViewAll");
                if (hasViewAll)
                {
                    _logger.LogInformation("User has depot ViewAll permission - returning all {Count} depots", allDepots.Count);
                    return APIOperationResponse<List<Depot>>.Success(allDepots);
                }

                // Scoped read: Depots.View (read depot list) or Inventory.View (warehouse flows). Depots.Page alone does not return rows.
                var canReadScoped = await _permissionService.HasPermissionAsync("Permissions.Depots.View")
                    || await _permissionService.HasPermissionAsync("Permissions.Inventory.View");
                if (!canReadScoped)
                {
                    _logger.LogInformation("User lacks Depots.View or Inventory.View — returning empty depot list");
                    return APIOperationResponse<List<Depot>>.Success(new List<Depot>());
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

        public async Task<APIOperationResponse<PaginatedList<Depot>>> GetDepotsPaginatedAsync(PagedListRequest request)
        {
            try
            {
                request ??= new PagedListRequest();
                if (request.Page <= 0)
                {
                    request.Page = 1;
                }

                var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;
                if (pageSize > 1000)
                {
                    pageSize = 1000;
                }
                request.PageSize = pageSize;

                var baseQuery = _repository.Find(x => !x.IsDeleted);
                IQueryable<Depot> query;

                if (_currentUserService.IsSuperAdmin)
                {
                    query = baseQuery;
                }
                else if (await _permissionService.HasPermissionAsync("Permissions.Depots.ViewAll"))
                {
                    query = baseQuery;
                }
                else
                {
                    var canReadScoped = await _permissionService.HasPermissionAsync("Permissions.Depots.View")
                        || await _permissionService.HasPermissionAsync("Permissions.Inventory.View");
                    if (!canReadScoped)
                    {
                        var empty = new PaginatedList<Depot>(new List<Depot>(), 0, 1, request.PageSize);
                        return APIOperationResponse<PaginatedList<Depot>>.Success(empty);
                    }

                    var userId = _currentUserService.UserId;
                    if (string.IsNullOrEmpty(userId))
                    {
                        var emptyUser = new PaginatedList<Depot>(new List<Depot>(), 0, 1, request.PageSize);
                        return APIOperationResponse<PaginatedList<Depot>>.Success(emptyUser);
                    }

                    var assignedDepotIds = await _userDepotRepository
                        .Find(ud => ud.UserId == userId)
                        .Select(ud => ud.DepotId)
                        .ToListAsync();

                    query = baseQuery.Where(d => assignedDepotIds.Contains(d.Id));
                }

                if (request.Filter == null || string.IsNullOrEmpty(request.Filter.sortField))
                {
                    query = query.OrderBy(d => d.NameEn).ThenBy(d => d.Id);
                }

                var paginated = await PaginatedList<Depot>.CreateAsyncForTableBinding(query, request);
                return APIOperationResponse<PaginatedList<Depot>>.Success(paginated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paginated depots");
                return APIOperationResponse<PaginatedList<Depot>>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.OPERATION_FAILED,
                    $"Error retrieving depots: {ex.Message}");
            }
        }
    }
}
