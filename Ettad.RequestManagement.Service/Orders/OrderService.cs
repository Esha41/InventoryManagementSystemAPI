using AutoMapper;
using FluentValidation;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.RequestManagement.Service.Orders.Dto;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;

namespace Ettad.RequestManagement.Service.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ICrossCuttingRepository<Order> _orderRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateOrderDto> _validator;
        private readonly ICurrentUserService _currentUserService;

        public OrderService(
            ICrossCuttingRepository<Order> orderRepository,
            IMapper mapper,
            IValidator<CreateUpdateOrderDto> validator,
            ICurrentUserService currentUserService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _validator = validator;
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

        public async Task<APIOperationResponse<OrderDto>> CreateAsync(CreateUpdateOrderDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<OrderDto>.Fail(ResponseType.BadRequest, errors);
                }

                // Map DTO to entity
                var order = _mapper.Map<Order>(inputDto);
                order.CreationDate = DateTime.UtcNow;
                order.CreatedBy = _currentUserService.UserId;

                // Add to repository
                var createdOrder = await _orderRepository.AddAsync(order);

                // Reload with navigation properties
                var result = await _orderRepository.FindOneAsync(
                    o => o.Id == createdOrder.Id,
                    false,
                    nameof(Order.Department),
                    nameof(Order.Requester),
                    nameof(Order.Reciever),
                    nameof(Order.Depot),
                    nameof(Order.RequestPurpose),
                    nameof(Order.RequestItems)
                );

                var dto = _mapper.Map<OrderDto>(result);
                return APIOperationResponse<OrderDto>.Success(dto, "Order created successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<OrderDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<OrderDto>> UpdateAsync(long id, CreateUpdateOrderDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<OrderDto>.Fail(ResponseType.BadRequest, errors);
                }

                // Check if order exists
                var existingOrder = await _orderRepository.FindOneAsync(o => o.Id == id && !o.IsDeleted);
                if (existingOrder == null)
                    return APIOperationResponse<OrderDto>.Fail(ResponseType.NotFound, "Order not found");

                // Map updates to entity
                _mapper.Map(inputDto, existingOrder);
                existingOrder.ModificationDate = DateTime.UtcNow;
                existingOrder.ModifiedBy = _currentUserService.UserId;

                // Update in repository
                await _orderRepository.UpdateAsync(existingOrder);

                // Reload with navigation properties
                var result = await _orderRepository.FindOneAsync(
                    o => o.Id == id,
                    false,
                    nameof(Order.Department),
                    nameof(Order.Requester),
                    nameof(Order.Reciever),
                    nameof(Order.Depot),
                    nameof(Order.RequestPurpose),
                    nameof(Order.RequestItems)
                );

                var dto = _mapper.Map<OrderDto>(result);
                return APIOperationResponse<OrderDto>.Success(dto, "Order updated successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<OrderDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
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

