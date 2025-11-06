using AutoMapper;
using FluentValidation;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.Orders.Dto;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;

namespace Ettad.RequestManagement.Service.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ICrossCuttingRepository<Order> _orderRepository;
        private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateOrderDto> _createValidator;
        private readonly IValidator<UpdateOrderDto> _updateValidator;
        private readonly ICurrentUserService _currentUserService;

        public OrderService(
            ICrossCuttingRepository<Order> orderRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            IMapper mapper,
            IValidator<CreateOrderDto> createValidator,
            IValidator<UpdateOrderDto> updateValidator,
            ICurrentUserService currentUserService)
        {
            _orderRepository = orderRepository;
            _requestItemRepository = requestItemRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _currentUserService = currentUserService;
        }

        public async Task<APIOperationResponse<OrderDto>> GetByIdAsync(long id)
        {
            try
            {
                var order = await _orderRepository.FindOneAsync(
                    o => o.Id == id && !o.IsDeleted,
                    false,
                    nameof(Order.Department),
                    nameof(Order.Requester),
                    nameof(Order.Reciever),
                    nameof(Order.Depot),
                    nameof(Order.RequestPurpose),
                    nameof(Order.RequestItems)
                );

                if (order == null)
                    return APIOperationResponse<OrderDto>.Fail(ResponseType.NotFound, "Order not found");

                var dto = _mapper.Map<OrderDto>(order);
                return APIOperationResponse<OrderDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<OrderDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<OrderDto>>> GetAllAsync()
        {
            try
            {
                var orders = await _orderRepository.FindAsync(
                    o => !o.IsDeleted,
                    false,
                    nameof(Order.Department),
                    nameof(Order.Requester),
                    nameof(Order.Reciever),
                    nameof(Order.Depot),
                    nameof(Order.RequestPurpose),
                    nameof(Order.RequestItems)
                );

                var dtos = _mapper.Map<List<OrderDto>>(orders);
                return APIOperationResponse<List<OrderDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<OrderDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateOrderDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _createValidator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                // Map DTO to entity (exclude RequestItems for now)
                var order = _mapper.Map<Order>(inputDto);
                order.RequestNo = inputDto.OrderNo; // Explicitly set RequestNo from OrderNo
                order.RequestType = RequestType.Order; // Always set request type to Order
                order.Status = RequestStatus.New; // Always set initial status to New
                order.CreationDate = DateTime.UtcNow;
                order.CreatedBy = _currentUserService.UserId;
                
                // Initialize RequestItems collection if null
                if (order.RequestItems == null)
                {
                    order.RequestItems = new List<RequestItem>();
                }

                // Set audit fields for request items and establish relationship
                if (inputDto.RequestItems != null && inputDto.RequestItems.Any())
                {
                    var requestItems = _mapper.Map<List<RequestItem>>(inputDto.RequestItems);
                    foreach (var item in requestItems)
                    {
                        item.CreationDate = DateTime.UtcNow;
                        item.CreatedBy = _currentUserService.UserId;
                        order.RequestItems.Add(item);
                    }
                }

                // Add to repository (this will cascade save RequestItems)
                var createdOrder = await _orderRepository.AddAsync(order);

                return APIOperationResponse<long>.Success(createdOrder.Id, "Order created successfully");
            }
            catch (Exception ex)
            {
                var errorMessage = $"An error occurred: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $" | Inner Exception: {ex.InnerException.Message}";
                }
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, errorMessage);
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, UpdateOrderDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _updateValidator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                // Check if order exists
                var existingOrder = await _orderRepository.FindOneAsync(o => o.Id == id && !o.IsDeleted);
                if (existingOrder == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Order not found");

                // Map updates to entity
                _mapper.Map(inputDto, existingOrder);
                existingOrder.ModificationDate = DateTime.UtcNow;
                existingOrder.ModifiedBy = _currentUserService.UserId;

                // Handle request items update
                if (inputDto.RequestItems != null)
                {
                    // Get existing request items
                    var existingItems = await _requestItemRepository.FindAsync(ri => ri.RequestId == id && !ri.IsDeleted);
                    
                    // Soft delete all existing items
                    foreach (var existingItem in existingItems)
                    {
                        await _requestItemRepository.DeleteAsync(existingItem);
                    }

                    // Add new items
                    var newItems = _mapper.Map<List<RequestItem>>(inputDto.RequestItems);
                    foreach (var item in newItems)
                    {
                        item.RequestId = id;
                        item.CreationDate = DateTime.UtcNow;
                        item.CreatedBy = _currentUserService.UserId;
                        await _requestItemRepository.AddAsync(item);
                    }
                }

                // Update in repository
                await _orderRepository.UpdateAsync(existingOrder);

                return APIOperationResponse<bool>.Success(true, "Order updated successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            try
            {
                var order = await _orderRepository.FindOneAsync(o => o.Id == id && !o.IsDeleted);
                if (order == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Order not found");

                // Soft delete - interceptor will handle IsDeleted, DeletionDate, and DeletedBy automatically
                await _orderRepository.DeleteAsync(order);

                return APIOperationResponse<bool>.Success(true, "Order deleted successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}

