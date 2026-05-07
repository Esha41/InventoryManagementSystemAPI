using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Application.Common.Interfaces;
using Ettad.Data.Entities;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Interfaces.Services;
using Ettad.Data.Interfaces.Repositories;

namespace Ettad.RequestManagement.Service.OrderItemTracking
{
    public class OrderItemTrackingService : IOrderItemTrackingService
    {
        private static readonly string[] OrderWithRequestItemsInclude =
        [
            nameof(Order.RequestItems),
        ];

        private static readonly string[] OrderItemHistoryDetailIncludes =
        [
            nameof(OrderItemHistory.Order),
            nameof(OrderItemHistory.Item),
            nameof(OrderItemHistory.Department),
            nameof(OrderItemHistory.ModifiedByUser),
            nameof(OrderItemHistory.WorkflowStep),
            $"{nameof(OrderItemHistory.WorkflowStep)}.{nameof(WorkflowStep.ApplicationRole)}",
        ];

        private readonly ICrossCuttingRepository<Order> _orderRepository;
        private readonly ICrossCuttingRepository<WorkflowApprovalStep> _workflowApprovalStepRepository;
        private readonly ICrossCuttingRepository<OrderItemHistory> _historyRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<OrderItemTrackingService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public OrderItemTrackingService(
            ICrossCuttingRepository<Order> orderRepository,
            ICrossCuttingRepository<WorkflowApprovalStep> workflowApprovalStepRepository,
            ICrossCuttingRepository<OrderItemHistory> historyRepository,
            IMapper mapper,
            ICurrentUserService currentUserService,
            ILogger<OrderItemTrackingService> logger,
            IDateTimeProvider dateTimeProvider)
        {
            _orderRepository = orderRepository;
            _workflowApprovalStepRepository = workflowApprovalStepRepository;
            _historyRepository = historyRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task RecordHistoryAsync(OrderItemHistoryContext context)
        {
            try
            {
                var history = new OrderItemHistory
                {
                    OrderId = context.OrderId,
                    RequestItemId = context.RequestItemId,
                    ItemId = context.ItemId,
                    ActionType = context.ActionType,
                    ActionDate = _dateTimeProvider.Now,
                    OrderStatus = context.OrderStatus,
                    PreviousQuantity = context.PreviousQuantity,
                    NewQuantity = context.NewQuantity,
                    ApprovedQuantity = context.ApprovedQuantity,
                    SuppliedQuantity = context.SuppliedQuantity,
                    DepartmentId = context.DepartmentId,
                    ModifiedByUserId = context.ModifiedByUserId,
                    ModifiedByUserName = context.ModifiedByUserName,
                    WorkflowApprovalStepId = context.WorkflowApprovalStepId,
                    WorkflowStepId = context.WorkflowStepId,
                    Description = context.Description ?? string.Empty,
                    Notes = context.Notes,
                    SupplyId = context.SupplyId,
                    AssetSupplyId = context.AssetSupplyId,
                    SupplyDetailId = context.SupplyDetailId,
                    AssetSupplyDetailId = context.AssetSupplyDetailId,
                    CreationDate = _dateTimeProvider.Now,
                    CreatedBy = _currentUserService.UserId
                };

                await _historyRepository.AddAsync(history);

                _logger.LogInformation("Order item history recorded. OrderId: {OrderId}, ItemId: {ItemId}, ActionType: {ActionType}, User: {UserId}",
                    context.OrderId, context.ItemId, context.ActionType, context.ModifiedByUserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording order item history. OrderId: {OrderId}, ItemId: {ItemId}, ActionType: {ActionType}",
                    context.OrderId, context.ItemId, context.ActionType);
                // Don't throw - history recording should not fail the main operation
            }
        }

        public async Task RecordFinalApprovalHistoryAsync(long orderId, long workflowApprovalStepId)
        {
            try
            {
                // Get the order with its request items
                var order = await _orderRepository.FindOneAsync(
                    o => o.Id == orderId && !o.IsDeleted,
                    false,
                    OrderWithRequestItemsInclude);

                if (order == null)
                {
                    _logger.LogWarning("Order not found for final approval history. OrderId: {OrderId}", orderId);
                    return;
                }

                // Get the workflow approval step to get WorkflowStepId
                var workflowApprovalStep = await _workflowApprovalStepRepository.FindOneAsync(
                    was => was.Id == workflowApprovalStepId,
                    false,
                    nameof(WorkflowApprovalStep.WorkflowStep));

                if (workflowApprovalStep == null)
                {
                    _logger.LogWarning("Workflow approval step not found. WorkflowApprovalStepId: {WorkflowApprovalStepId}", workflowApprovalStepId);
                    return;
                }

                var departmentId = _currentUserService.DepartmentId ?? order.DepartmentId;
                var userId = _currentUserService.UserId ?? "System";
                var userName = _currentUserService.UserName ?? "System";

                // Record history for each request item
                foreach (var requestItem in order.RequestItems.Where(ri => !ri.IsDeleted))
                {
                    var historyContext = new OrderItemHistoryContext
                    {
                        OrderId = orderId,
                        RequestItemId = requestItem.Id,
                        ItemId = requestItem.ItemId,
                        ActionType = OrderItemActionType.FinalApproved,
                        OrderStatus = RequestStatus.Approved,
                        ApprovedQuantity = requestItem.Quantity,
                        DepartmentId = departmentId,
                        ModifiedByUserId = userId,
                        ModifiedByUserName = userName,
                        WorkflowApprovalStepId = workflowApprovalStepId,
                        WorkflowStepId = workflowApprovalStep.WorkflowStepId,
                        Description = $"Final order approved - approved quantity: {requestItem.Quantity}",
                        Notes = null
                    };

                    await RecordHistoryAsync(historyContext);
                }

                _logger.LogInformation("Final approval history recorded. OrderId: {OrderId}, ItemCount: {ItemCount}, User: {UserId}",
                    orderId, order.RequestItems.Count(ri => !ri.IsDeleted), userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recording final approval history. OrderId: {OrderId}", orderId);
                // Don't throw - history recording should not fail the main operation
            }
        }

        public async Task<APIOperationResponse<List<OrderItemHistoryDto>>> GetOrderItemHistoryAsync(long? orderId = null, string? requestNo = null)
        {
            _logger.LogInformation("Getting order item history. OrderId: {OrderId}, RequestNo: {RequestNo}, User: {UserId}",
                orderId, requestNo, _currentUserService.UserId);

            try
            {
                // Resolve orderId from RequestNo if needed
                long resolvedOrderId;
                if (orderId.HasValue)
                {
                    resolvedOrderId = orderId.Value;
                }
                else if (!string.IsNullOrWhiteSpace(requestNo))
                {
                    var order = await _orderRepository.FindOneAsync(
                        o => o.RequestNo == requestNo && !o.IsDeleted);
                    
                    if (order == null)
                    {
                        _logger.LogWarning("Order not found by RequestNo. RequestNo: {RequestNo}, User: {UserId}", 
                            requestNo, _currentUserService.UserId);
                        return APIOperationResponse<List<OrderItemHistoryDto>>.Fail(
                            ResponseType.NotFound, $"Order with RequestNo '{requestNo}' not found");
                    }
                    
                    resolvedOrderId = order.Id;
                }
                else
                {
                    return APIOperationResponse<List<OrderItemHistoryDto>>.Fail(
                        ResponseType.BadRequest, "Either orderId or requestNo must be provided");
                }

                var history = await _historyRepository
                    .Find(h => h.OrderId == resolvedOrderId && !h.IsDeleted, false, OrderItemHistoryDetailIncludes)
                    .OrderByDescending(h => h.ActionDate)
                    .ToListAsync();

                var dtos = _mapper.Map<List<OrderItemHistoryDto>>(history);

                // Map additional properties
                foreach (var dto in dtos)
                {
                    var historyEntity = history.FirstOrDefault(h => h.Id == dto.Id);
                    if (historyEntity != null)
                    {
                        dto.OrderRequestNo = historyEntity.Order?.RequestNo;
                        dto.ItemName = historyEntity.Item?.Name;
                        dto.ItemNo = historyEntity.Item?.ItemNo;
                        dto.DepartmentNameAr = historyEntity.Department?.NameAr ?? string.Empty;
                        dto.DepartmentNameEn = historyEntity.Department?.NameEn ?? string.Empty;
                        dto.ModifiedByUserNameEn = historyEntity.ModifiedByUser?.FullNameEN ?? string.Empty;
                        dto.ModifiedByUserNameAr = historyEntity.ModifiedByUser?.FullNameAR ?? string.Empty;
                        dto.WorkflowStepName = historyEntity.WorkflowStep?.ApplicationRole?.Name;
                    }
                }

                return APIOperationResponse<List<OrderItemHistoryDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order item history. OrderId: {OrderId}, RequestNo: {RequestNo}", 
                    orderId, requestNo);
                return APIOperationResponse<List<OrderItemHistoryDto>>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<OrderItemHistoryDto>>> GetItemHistoryAsync(long orderId, long itemId)
        {
            _logger.LogInformation("Getting item history. OrderId: {OrderId}, ItemId: {ItemId}, User: {UserId}",
                orderId, itemId, _currentUserService.UserId);

            try
            {
                var history = await _historyRepository
                    .Find(h => h.OrderId == orderId && h.ItemId == itemId && !h.IsDeleted, false, OrderItemHistoryDetailIncludes)
                    .OrderByDescending(h => h.ActionDate)
                    .ToListAsync();

                var dtos = _mapper.Map<List<OrderItemHistoryDto>>(history);

                // Map additional properties
                foreach (var dto in dtos)
                {
                    var historyEntity = history.FirstOrDefault(h => h.Id == dto.Id);
                    if (historyEntity != null)
                    {
                        dto.OrderRequestNo = historyEntity.Order?.RequestNo;
                        dto.ItemName = historyEntity.Item?.Name;
                        dto.ItemNo = historyEntity.Item?.ItemNo;
                        dto.DepartmentNameAr = historyEntity.Department?.NameAr ?? string.Empty;
                        dto.DepartmentNameEn = historyEntity.Department?.NameEn ?? string.Empty;
                        dto.ModifiedByUserNameEn = historyEntity.ModifiedByUser?.FullNameEN ?? string.Empty;
                        dto.ModifiedByUserNameAr = historyEntity.ModifiedByUser?.FullNameAR ?? string.Empty;
                        dto.WorkflowStepName = historyEntity.WorkflowStep?.ApplicationRole?.Name;
                    }
                }

                return APIOperationResponse<List<OrderItemHistoryDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting item history. OrderId: {OrderId}, ItemId: {ItemId}", orderId, itemId);
                return APIOperationResponse<List<OrderItemHistoryDto>>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<OrderItemHistoryDto>>> GetApprovedQuantitiesAsync(long? orderId = null, string? requestNo = null)
        {
            _logger.LogInformation("Getting approved quantities. OrderId: {OrderId}, RequestNo: {RequestNo}, User: {UserId}",
                orderId, requestNo, _currentUserService.UserId);

            try
            {
                // Resolve orderId from RequestNo if needed
                long resolvedOrderId;
                if (orderId.HasValue)
                {
                    resolvedOrderId = orderId.Value;
                }
                else if (!string.IsNullOrWhiteSpace(requestNo))
                {
                    var order = await _orderRepository.FindOneAsync(
                        o => o.RequestNo == requestNo && !o.IsDeleted);
                    
                    if (order == null)
                    {
                        _logger.LogWarning("Order not found by RequestNo. RequestNo: {RequestNo}, User: {UserId}", 
                            requestNo, _currentUserService.UserId);
                        return APIOperationResponse<List<OrderItemHistoryDto>>.Fail(
                            ResponseType.NotFound, $"Order with RequestNo '{requestNo}' not found");
                    }
                    
                    resolvedOrderId = order.Id;
                }
                else
                {
                    return APIOperationResponse<List<OrderItemHistoryDto>>.Fail(
                        ResponseType.BadRequest, "Either orderId or requestNo must be provided");
                }

                var history = await _historyRepository
                    .Find(h => h.OrderId == resolvedOrderId
                        && h.ActionType == OrderItemActionType.FinalApproved
                        && !h.IsDeleted, false, OrderItemHistoryDetailIncludes)
                    .OrderByDescending(h => h.ActionDate)
                    .ToListAsync();

                var dtos = _mapper.Map<List<OrderItemHistoryDto>>(history);

                // Map additional properties
                foreach (var dto in dtos)
                {
                    var historyEntity = history.FirstOrDefault(h => h.Id == dto.Id);
                    if (historyEntity != null)
                    {
                        dto.OrderRequestNo = historyEntity.Order?.RequestNo;
                        dto.ItemName = historyEntity.Item?.Name;
                        dto.ItemNo = historyEntity.Item?.ItemNo;
                        dto.DepartmentNameAr = historyEntity.Department?.NameAr ?? string.Empty;
                        dto.DepartmentNameEn = historyEntity.Department?.NameEn ?? string.Empty;
                        dto.ModifiedByUserNameEn = historyEntity.ModifiedByUser?.FullNameEN ?? string.Empty;
                        dto.ModifiedByUserNameAr = historyEntity.ModifiedByUser?.FullNameAR ?? string.Empty;
                        dto.WorkflowStepName = historyEntity.WorkflowStep?.ApplicationRole?.Name;
                    }
                }

                return APIOperationResponse<List<OrderItemHistoryDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting approved quantities. OrderId: {OrderId}, RequestNo: {RequestNo}", 
                    orderId, requestNo);
                return APIOperationResponse<List<OrderItemHistoryDto>>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
