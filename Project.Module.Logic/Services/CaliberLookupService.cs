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
    public class CaliberLookupService : LookupService<Caliber, CreateUpdateCaliberDto>, ICaliberLookupService
    {
        public CaliberLookupService(
            ICrossCuttingRepository<Caliber> repository,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider,
            ILogger<LookupService<Caliber, CreateUpdateCaliberDto>> logger)
            : base(repository, mapper, currentUserService, dateTimeProvider, logger)
        {
        }

        public async Task<APIOperationResponse<List<CaliberDto>>> GetByItemTypeAsync(ItemType? itemType)
        {
            try
            {
                _logger.LogInformation(
                    "Retrieving Calibers filtered by ItemType {ItemType} for Organization {OrganizationId}",
                    itemType,
                    _currentUserService.OrganizationId);

                var query = _repository.Find(c => !c.IsDeleted);
                if (itemType.HasValue)
                {
                    query = query.Where(c => c.ItemType == itemType.Value);
                }

                var items = await query
                    .OrderBy(c => c.NameEn)
                    .ThenBy(c => c.NameAr)
                    .ToListAsync();
                var dtos = _mapper.Map<List<CaliberDto>>(items);

                return APIOperationResponse<List<CaliberDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Calibers by ItemType {ItemType}", itemType);
                return APIOperationResponse<List<CaliberDto>>.Fail(
                    ResponseType.InternalServerError,
                    CommonErrorCodes.OPERATION_FAILED,
                    $"Error retrieving calibers: {ex.Message}");
            }
        }
    }
}
