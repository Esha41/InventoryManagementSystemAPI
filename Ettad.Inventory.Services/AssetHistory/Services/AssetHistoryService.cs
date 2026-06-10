using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Application.Common.Interfaces;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.AssetHistory.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Inventory.Service.AssetHistory.Interfaces;

using AssetHistoryEntity = Ettad.Data.Entities.AssetHistory;

namespace Ettad.Inventory.Service.AssetHistory.Services
{
    public class AssetHistoryService : IAssetHistoryService
    {
        private static readonly string[] HistoryIncludes =
        {
            nameof(AssetHistoryEntity.Asset),
            $"{nameof(AssetHistoryEntity.Asset)}.{nameof(Ettad.Data.Entities.Asset.Batch)}",
            nameof(AssetHistoryEntity.Order),
            nameof(AssetHistoryEntity.AssetSupply),
            nameof(AssetHistoryEntity.AssetAssignment),
            nameof(AssetHistoryEntity.PreviousDepartment),
            nameof(AssetHistoryEntity.NewDepartment),
            nameof(AssetHistoryEntity.PreviousCustodian),
            nameof(AssetHistoryEntity.NewCustodian)
        };

        private readonly ICrossCuttingRepository<Data.Entities.AssetHistory> _historyRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AssetHistoryService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AssetHistoryService(
            ICrossCuttingRepository<Data.Entities.AssetHistory> historyRepository,
            IMapper mapper,
            ICurrentUserService currentUserService,
            ILogger<AssetHistoryService> logger,
            IDateTimeProvider dateTimeProvider)
        {
            _historyRepository = historyRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
        }
        public async Task RecordHistoryAsync(long assetId, AssetHistoryActionType actionType, AssetHistoryContext context)
        {
            try
            {
                var history = new Data.Entities.AssetHistory
                {
                    AssetId = assetId,
                    ActionType = actionType,
                    ActionDate = _dateTimeProvider.Now,
                    Description = string.Empty,
                    PreviousStatus = context.PreviousStatus,
                    NewStatus = context.NewStatus,
                    PreviousDepartmentId = context.PreviousDepartmentId,
                    NewDepartmentId = context.NewDepartmentId,
                    PreviousCustodianId = context.PreviousCustodianId,
                    NewCustodianId = context.NewCustodianId,
                    PreviousLocation = context.PreviousLocation,
                    NewLocation = context.NewLocation,
                    OrderId = context.OrderId,
                    AssetSupplyId = context.AssetSupplyId,
                    AssetAssignmentId = context.AssetAssignmentId,
                    PerformedByUserId = _currentUserService.UserId,
                    PerformedByUserName = _currentUserService.UserName,
                    Notes = context.Notes,
                    Metadata = context.Metadata,
                    CreationDate = _dateTimeProvider.Now,
                    CreatedBy = _currentUserService.UserId
                };

                await _historyRepository.AddAsync(history);

                _logger.LogInformation("Asset history recorded. AssetId: {AssetId}, ActionType: {ActionType}, User: {UserId}",
                    assetId, actionType, _currentUserService.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording asset history. AssetId: {AssetId}, ActionType: {ActionType}",
                    assetId, actionType);
                // Don't throw - history recording should not fail the main operation
            }
        }

        public async Task<APIOperationResponse<List<AssetHistoryDto>>> GetAssetHistoryAsync(long assetId)
        {
            _logger.LogInformation("Getting asset history. AssetId: {AssetId}, User: {UserId}",
                assetId, _currentUserService.UserId);

            try
            {
                var history = await _historyRepository
                    .Find(h => h.AssetId == assetId && !h.IsDeleted, false, HistoryIncludes)
                    .OrderByDescending(h => h.ActionDate)
                    .ToListAsync();

                var dtos = _mapper.Map<List<AssetHistoryDto>>(history);

                return APIOperationResponse<List<AssetHistoryDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting asset history. AssetId: {AssetId}", assetId);
                return APIOperationResponse<List<AssetHistoryDto>>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<AssetHistoryDto>>> GetAssetHistoryByOrderAsync(long orderId)
        {
            _logger.LogInformation("Getting asset history by order. OrderId: {OrderId}, User: {UserId}",
                orderId, _currentUserService.UserId);

            try
            {
                var history = await _historyRepository
                    .Find(h => h.OrderId == orderId && !h.IsDeleted, false, HistoryIncludes)
                    .OrderByDescending(h => h.ActionDate)
                    .ToListAsync();

                var dtos = _mapper.Map<List<AssetHistoryDto>>(history);

                return APIOperationResponse<List<AssetHistoryDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting asset history by order. OrderId: {OrderId}", orderId);
                return APIOperationResponse<List<AssetHistoryDto>>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<AssetHistoryDto>>> GetAssetHistoryBySupplyAsync(long supplyId)
        {
            _logger.LogInformation("Getting asset history by supply. SupplyId: {SupplyId}, User: {UserId}",
                supplyId, _currentUserService.UserId);

            try
            {
                var history = await _historyRepository
                    .Find(h => h.AssetSupplyId == supplyId && !h.IsDeleted, false, HistoryIncludes)
                    .OrderByDescending(h => h.ActionDate)
                    .ToListAsync();

                var dtos = _mapper.Map<List<AssetHistoryDto>>(history);

                return APIOperationResponse<List<AssetHistoryDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting asset history by supply. SupplyId: {SupplyId}", supplyId);
                return APIOperationResponse<List<AssetHistoryDto>>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<AssetHistoryDto>>> GetHistoryByActionTypeAsync(
            AssetHistoryActionType actionType, DateTime? fromDate = null, DateTime? toDate = null)
        {
            _logger.LogInformation("Getting asset history by action type. ActionType: {ActionType}, User: {UserId}",
                actionType, _currentUserService.UserId);

            try
            {
                var query = _historyRepository
                    .Find(h => h.ActionType == actionType && !h.IsDeleted, false, HistoryIncludes);

                if (fromDate.HasValue)
                {
                    query = query.Where(h => h.ActionDate >= fromDate.Value);
                }

                if (toDate.HasValue)
                {
                    query = query.Where(h => h.ActionDate <= toDate.Value);
                }

                var history = await query
                    .OrderByDescending(h => h.ActionDate)
                    .ToListAsync();

                var dtos = _mapper.Map<List<AssetHistoryDto>>(history);

                return APIOperationResponse<List<AssetHistoryDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting asset history by action type. ActionType: {ActionType}", actionType);
                return APIOperationResponse<List<AssetHistoryDto>>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}

