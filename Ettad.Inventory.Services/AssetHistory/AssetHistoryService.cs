using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Inventory.Service.AssetHistory.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Comman.Time;

namespace Ettad.Inventory.Service.AssetHistory
{
    public class AssetHistoryService : IAssetHistoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICrossCuttingRepository<Ettad.Data.Entities.AssetHistory> _historyRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AssetHistoryService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AssetHistoryService(
            ApplicationDbContext context,
            ICrossCuttingRepository<Ettad.Data.Entities.AssetHistory> historyRepository,
            IMapper mapper,
            ICurrentUserService currentUserService,
            ILogger<AssetHistoryService> logger,
            IDateTimeProvider dateTimeProvider)
        {
            _context = context;
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
                var history = new Ettad.Data.Entities.AssetHistory
                {
                    AssetId = assetId,
                    ActionType = actionType,
                    ActionDate = _dateTimeProvider.Now,
                    Description = context.Description,
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
                var history = await _context.AssetHistory
                    .Include(h => h.Asset)
                    .Include(h => h.Order)
                    .Include(h => h.PreviousDepartment)
                    .Include(h => h.NewDepartment)
                    .Include(h => h.PreviousCustodian)
                    .Include(h => h.NewCustodian)
                    .Where(h => h.AssetId == assetId && !h.IsDeleted)
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
                var history = await _context.AssetHistory
                    .Include(h => h.Asset)
                    .Include(h => h.Order)
                    .Include(h => h.PreviousDepartment)
                    .Include(h => h.NewDepartment)
                    .Include(h => h.PreviousCustodian)
                    .Include(h => h.NewCustodian)
                    .Where(h => h.OrderId == orderId && !h.IsDeleted)
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
                var history = await _context.AssetHistory
                    .Include(h => h.Asset)
                    .Include(h => h.Order)
                    .Include(h => h.PreviousDepartment)
                    .Include(h => h.NewDepartment)
                    .Include(h => h.PreviousCustodian)
                    .Include(h => h.NewCustodian)
                    .Where(h => h.AssetSupplyId == supplyId && !h.IsDeleted)
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
                var query = _context.AssetHistory
                    .Include(h => h.Asset)
                    .Include(h => h.Order)
                    .Include(h => h.PreviousDepartment)
                    .Include(h => h.NewDepartment)
                    .Include(h => h.PreviousCustodian)
                    .Include(h => h.NewCustodian)
                    .Where(h => h.ActionType == actionType && !h.IsDeleted);

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

