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

        public DepotService(
            ICrossCuttingRepository<Depot> repository,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider,
            ILogger<LookupService<Depot, CreateUpdateDepotDto>> logger,
            ICrossCuttingRepository<Inventory> inventoryRepository)
            : base(repository, mapper, currentUserService, dateTimeProvider, logger)
        {
            _inventoryRepository = inventoryRepository;
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
