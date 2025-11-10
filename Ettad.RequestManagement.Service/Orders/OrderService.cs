using AutoMapper;
using FluentValidation;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.Orders.Dto;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Ettad.RequestManagement.Service.Common;
using Microsoft.AspNetCore.Identity;
using Ettad.Comman.Idenitity;
using Microsoft.Extensions.Logging;

namespace Ettad.RequestManagement.Service.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ICrossCuttingRepository<Order> _orderRepository;
        private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;
        private readonly ICrossCuttingRepository<RequestPurpose> _requestPurposeRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateOrderDto> _createValidator;
        private readonly IValidator<UpdateOrderDto> _updateValidator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRequestNoGeneratorService _requestNoGeneratorService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            ICrossCuttingRepository<Order> orderRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            ICrossCuttingRepository<RequestPurpose> requestPurposeRepository,
            IMapper mapper,
            IValidator<CreateOrderDto> createValidator,
            IValidator<UpdateOrderDto> updateValidator,
            ICurrentUserService currentUserService,
            IRequestNoGeneratorService requestNoGeneratorService,
            UserManager<ApplicationUser> userManager,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _requestItemRepository = requestItemRepository;
            _requestPurposeRepository = requestPurposeRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _currentUserService = currentUserService;
            _requestNoGeneratorService = requestNoGeneratorService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<APIOperationResponse<OrderDto>> GetByIdAsync(long id)
        {
            _logger.LogInformation("Getting order by ID: {OrderId}. User: {UserId}", id, _currentUserService.UserId);
            
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
                    $"{nameof(Order.RequestItems)}.{nameof(RequestItem.Item)}"
                );

                if (order == null)
                {
                    _logger.LogWarning("Order not found. OrderId: {OrderId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<OrderDto>.Fail(ResponseType.NotFound, "Order not found");
                }

                var dto = _mapper.Map<OrderDto>(order);
                
                // Fallback: If RequesterName is null but we have a CreatedBy user, use that user's name
                if (string.IsNullOrEmpty(dto.RequesterName) && !string.IsNullOrEmpty(order.CreatedBy))
                {
                    dto.RequesterName = await GetUserNameByIdAsync(order.CreatedBy);
                }
                
                _logger.LogInformation("Successfully retrieved order. OrderId: {OrderId}, OrderNo: {OrderNo}", id, order.RequestNo);
                return APIOperationResponse<OrderDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order by ID. OrderId: {OrderId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<OrderDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<OrderDto>>> GetAllAsync()
        {
            //throw new NotImplementedException();
            _logger.LogInformation("Getting all orders. User: {UserId}", _currentUserService.UserId);
            
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
                    $"{nameof(Order.RequestItems)}.{nameof(RequestItem.Item)}"
                );

                var dtos = _mapper.Map<List<OrderDto>>(orders);
                
                // Fallback: Populate RequesterName from CreatedBy user if not set
                foreach (var dto in dtos)
                {
                    var order = orders.FirstOrDefault(o => o.Id == dto.Id);
                    if (order != null && string.IsNullOrEmpty(dto.RequesterName) && !string.IsNullOrEmpty(order.CreatedBy))
                    {
                        dto.RequesterName = await GetUserNameByIdAsync(order.CreatedBy);
                    }
                }
                
                _logger.LogInformation("Successfully retrieved {OrderCount} orders. User: {UserId}", dtos.Count, _currentUserService.UserId);
                return APIOperationResponse<List<OrderDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all orders. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<OrderDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateOrderDto inputDto)
        {
            _logger.LogInformation("Creating new order. OrderNo: {OrderNo}, DepartmentId: {DepartmentId}, User: {UserId}", 
                inputDto.OrderNo, inputDto.DepartmentId, _currentUserService.UserId);
            
            try
            {
                // Validate input
                var validationResult = await _createValidator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning("Order validation failed. OrderNo: {OrderNo}, Errors: {ValidationErrors}, User: {UserId}", 
                        inputDto.OrderNo, errors, _currentUserService.UserId);
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                // Ensure request purpose is for orders
                var requestPurpose = await _requestPurposeRepository.FindOneAsync(
                    rp => rp.Id == inputDto.RequestPurposeId && rp.RequestType == RequestType.Order && !rp.IsDeleted);

                if (requestPurpose == null)
                {
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "Request purpose must be of type Order");
                }

                // Map DTO to entity (exclude RequestItems for now)
                var order = _mapper.Map<Order>(inputDto);
                order.RequestNo = inputDto.OrderNo; // Explicitly set RequestNo from OrderNo
                order.RequestType = RequestType.Order; // Always set request type to Order
                order.Status = RequestStatus.New; // Always set initial status to New
                order.CreationDate = DateTime.UtcNow;
                order.CreatedBy = _currentUserService.UserId;

                // Generate request number
                order.RequestNo = await _requestNoGeneratorService.GenerateRequestNoAsync(RequestType.Order, inputDto.DepartmentId);
                
                // Initialize RequestItems collection if null
                if (order.RequestItems == null)
                {
                    order.RequestItems = new List<RequestItem>();
                }

                // Set audit fields for request items and establish relationship
                if (inputDto.RequestItems != null && inputDto.RequestItems.Any())
                {
                    var requestItems = _mapper.Map<List<RequestItem>>(inputDto.RequestItems);
                    _logger.LogInformation("Adding {ItemCount} request items to order. OrderNo: {OrderNo}", 
                        requestItems.Count, inputDto.OrderNo);
                    
                    foreach (var item in requestItems)
                    {
                        item.CreationDate = DateTime.UtcNow;
                        item.CreatedBy = _currentUserService.UserId;
                        order.RequestItems.Add(item);
                    }
                }

                // Add to repository (this will cascade save RequestItems)
                var createdOrder = await _orderRepository.AddAsync(order);

                _logger.LogInformation("Order created successfully. OrderId: {OrderId}, OrderNo: {OrderNo}, ItemCount: {ItemCount}, User: {UserId}", 
                    createdOrder.Id, createdOrder.RequestNo, createdOrder.RequestItems?.Count ?? 0, _currentUserService.UserId);
                
                return APIOperationResponse<long>.Success(createdOrder.Id, "Order created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order. OrderNo: {OrderNo}, User: {UserId}", 
                    inputDto.OrderNo, _currentUserService.UserId);
                
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
            _logger.LogInformation("Updating order. OrderId: {OrderId}, User: {UserId}", id, _currentUserService.UserId);
            
            try
            {
                // Validate input
                var validationResult = await _updateValidator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning("Order update validation failed. OrderId: {OrderId}, Errors: {ValidationErrors}, User: {UserId}", 
                        id, errors, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                // Check if order exists
                var existingOrder = await _orderRepository.FindOneAsync(o => o.Id == id && !o.IsDeleted);
                if (existingOrder == null)
                {
                    _logger.LogWarning("Order not found for update. OrderId: {OrderId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Order not found");
                }

                // Map updates to entity
                _mapper.Map(inputDto, existingOrder);
                existingOrder.ModificationDate = DateTime.UtcNow;
                existingOrder.ModifiedBy = _currentUserService.UserId;

                // Handle request items update
                if (inputDto.RequestItems != null)
                {
                    // Get existing request items
                    var existingItems = await _requestItemRepository.FindAsync(ri => ri.RequestId == id && !ri.IsDeleted);
                    
                    _logger.LogInformation("Updating request items. OrderId: {OrderId}, ExistingItemsCount: {ExistingCount}, NewItemsCount: {NewCount}", 
                        id, existingItems.Count(), inputDto.RequestItems.Count);
                    
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

                _logger.LogInformation("Order updated successfully. OrderId: {OrderId}, OrderNo: {OrderNo}, User: {UserId}", 
                    id, existingOrder.RequestNo, _currentUserService.UserId);
                
                return APIOperationResponse<bool>.Success(true, "Order updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order. OrderId: {OrderId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting order. OrderId: {OrderId}, User: {UserId}", id, _currentUserService.UserId);
            
            try
            {
                var order = await _orderRepository.FindOneAsync(o => o.Id == id && !o.IsDeleted);
                if (order == null)
                {
                    _logger.LogWarning("Order not found for deletion. OrderId: {OrderId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Order not found");
                }

                // Soft delete - interceptor will handle IsDeleted, DeletionDate, and DeletedBy automatically
                await _orderRepository.DeleteAsync(order);

                _logger.LogInformation("Order deleted successfully. OrderId: {OrderId}, OrderNo: {OrderNo}, User: {UserId}", 
                    id, order.RequestNo, _currentUserService.UserId);
                
                return APIOperationResponse<bool>.Success(true, "Order deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order. OrderId: {OrderId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Get user name by user ID from Identity system
        /// </summary>
        private async Task<string> GetUserNameByIdAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    // Try FullNameEN first, then FullNameAR, then UserName
                    if (!string.IsNullOrEmpty(user.FullNameEN))
                        return user.FullNameEN;
                    if (!string.IsNullOrEmpty(user.FullNameAR))
                        return user.FullNameAR;
                    if (!string.IsNullOrEmpty(user.UserName))
                        return user.UserName;
                }
                
                return "System User";
            }
            catch
            {
                return "System User";
            }
        }
    }
}



