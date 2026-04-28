using AutoMapper;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Module.lookup.Dtos;
using Ettad.Module.lookup.Interfaces;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.Module.lookup.Services
{
    public class UnitLookupService : LookupService<Unit, CreateUpdateUnitDto>, IUnitLookupService
    {
        public UnitLookupService(
            ICrossCuttingRepository<Unit> repository,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider,
            ILogger<LookupService<Unit, CreateUpdateUnitDto>> logger)
            : base(repository, mapper, currentUserService, dateTimeProvider, logger)
        {
        }

        public async Task<APIOperationResponse<List<UnitDto>>> GetByItemTypeAsync(ItemType? itemType)
        {
            try
            {
                _logger.LogInformation(
                    "Retrieving Units filtered by ItemType {ItemType} for Organization {OrganizationId}",
                    itemType,
                    _currentUserService.OrganizationId);

                var query = _repository.Find(u => !u.IsDeleted);
                if (itemType.HasValue)
                {
                    query = query.Where(u => u.ItemType == itemType.Value);
                }

                var units = await query.ToListAsync();
                var dtos = _mapper.Map<List<UnitDto>>(units);

                return APIOperationResponse<List<UnitDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Units by ItemType {ItemType}", itemType);
                return APIOperationResponse<List<UnitDto>>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.OPERATION_FAILED,
                    $"Error retrieving units: {ex.Message}");
            }
        }
    }
}
