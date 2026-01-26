using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ettad.Application.Common.Interfaces;
using Ettad.Comman.Enums;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.CrossCutting.Comman.Models;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Notification.Service;
using Ettad.RequestManagement.Service.Common;
using Ettad.RequestManagement.Service.Orders.Dto;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.Interface;
using Ettad.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq;
using Ettad.CrossCutting.Comman.Time;

namespace Ettad.RequestManagement.Service.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ICrossCuttingRepository<Order> _orderRepository;
        private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;
        private readonly ICrossCuttingRepository<RequestPurpose> _requestPurposeRepository;
        private readonly ICrossCuttingRepository<AllowanceItem> _allowanceItemRepository;
        private readonly ICrossCuttingRepository<Supply> _supplyRepository;
        private readonly ICrossCuttingRepository<SupplyDetail> _supplyDetailRepository;
        private readonly ICrossCuttingRepository<FileUplodDetails> _fileDetailsRepository;
        private readonly ICrossCuttingRepository<BaseItem> _baseItemRepository;
        private readonly IWorkflowApprovalService _workflowApprovalService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateOrderDto> _createValidator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRequestNoGeneratorService _requestNoGeneratorService;
        private readonly INotificationHelperService _notificationHelperService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<OrderService> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IOrderItemTrackingService _orderItemTrackingService;

        public OrderService(
            ICrossCuttingRepository<Order> orderRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            ICrossCuttingRepository<RequestPurpose> requestPurposeRepository,
            ICrossCuttingRepository<AllowanceItem> allowanceItemRepository,
            ICrossCuttingRepository<Supply> supplyRepository,
            ICrossCuttingRepository<SupplyDetail> supplyDetailRepository,
            ICrossCuttingRepository<FileUplodDetails> fileDetailsRepository,
            ICrossCuttingRepository<BaseItem> baseItemRepository,
            IWorkflowApprovalService workflowApprovalService,
            IMapper mapper,
            IValidator<CreateOrderDto> createValidator,
            ICurrentUserService currentUserService,
            IRequestNoGeneratorService requestNoGeneratorService,
            INotificationHelperService notificationHelperService,
            UserManager<ApplicationUser> userManager,
            ILogger<OrderService> logger,
            IFileUploadService fileUploadService,
            IDateTimeProvider dateTimeProvider,
            IOrderItemTrackingService orderItemTrackingService)
        {
            _orderRepository = orderRepository;
            _requestItemRepository = requestItemRepository;
            _requestPurposeRepository = requestPurposeRepository;
            _allowanceItemRepository = allowanceItemRepository;
            _supplyRepository = supplyRepository;
            _supplyDetailRepository = supplyDetailRepository;
            _fileDetailsRepository = fileDetailsRepository;
            _baseItemRepository = baseItemRepository;
            _workflowApprovalService = workflowApprovalService;
            _mapper = mapper;
            _createValidator = createValidator;
            _currentUserService = currentUserService;
            _requestNoGeneratorService = requestNoGeneratorService;
            _notificationHelperService = notificationHelperService;
            _userManager = userManager;
            _logger = logger;
            _fileUploadService = fileUploadService;
            _dateTimeProvider = dateTimeProvider;
            _orderItemTrackingService = orderItemTrackingService;
        }

        // ... existing methods omitted for brevity until SetSupplyDateAsync ...
        
        // Include full file content implementation here...
        // For brevity in this tool call, I will include the full implementation but focusing on adding the new method
        
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
                    nameof(Order.RequestPurpose),
                    $"{nameof(Order.RequestItems)}.{nameof(RequestItem.Item)}"
                );

                if (order == null)
                {
                    _logger.LogWarning("Order not found. OrderId: {OrderId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<OrderDto>.Fail(ResponseType.NotFound, "Order not found");
                }

                // Filter out soft-deleted request items
                if (order.RequestItems != null)
                {
                    order.RequestItems = order.RequestItems.Where(ri => !ri.IsDeleted).ToList();
                }

                var dto = _mapper.Map<OrderDto>(order);
                
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
            _logger.LogInformation("Getting all orders. User: {UserId}", _currentUserService.UserId);
            
            try
            {
                var orders = await _orderRepository.FindAsync(
                    o => !o.IsDeleted,
                    false,
                    nameof(Order.Department),
                    nameof(Order.Requester),
                    nameof(Order.RequestPurpose),
                    $"{nameof(Order.RequestItems)}.{nameof(RequestItem.Item)}"
                );

                // Filter out soft-deleted request items from all orders
                foreach (var order in orders)
                {
                    if (order.RequestItems != null)
                    {
                        order.RequestItems = order.RequestItems.Where(ri => !ri.IsDeleted).ToList();
                    }
                }

                var dtos = _mapper.Map<List<OrderDto>>(orders);
                
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
            return await CreateAsync(inputDto, null);
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateOrderDto inputDto, List<IFormFile> files)
        {
            var currentUserId = _currentUserService.UserId;
            var departmentId = _currentUserService.DepartmentId;

            if (!departmentId.HasValue || departmentId.Value <= 0)
            {
                _logger.LogError("User has no department assigned. Cannot create order. User: {UserId}", currentUserId);
                return APIOperationResponse<long>.Fail(ResponseType.BadRequest, 
                    "Department not found for current user. Cannot create order.");
            }

            _logger.LogInformation("Creating new order. DepartmentId: {DepartmentId}, IsFromAllowance: {IsFromAllowance}, User: {UserId}, HasFiles: {HasFiles}",
                departmentId.Value, inputDto.IsFromAllowance, currentUserId, files != null && files.Count > 0);
            
            try
            {
                // Validate input
                var validationResult = await _createValidator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning("Order validation failed. Errors: {ValidationErrors}, User: {UserId}",
                        errors, currentUserId);
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                // Additional validation for orders from allowance
                if (inputDto.IsFromAllowance)
                {
                    var allowanceValidationResult = await ValidateAllowanceForOrderAsync(inputDto.RequestItems, departmentId.Value);
                    if (!allowanceValidationResult.IsValid)
                    {
                        var errorMessage = string.Join("; ", allowanceValidationResult.Errors);
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errorMessage);
                    }
                }

                // Ensure request purpose is for orders
                var requestPurpose = await _requestPurposeRepository.FindOneAsync(
                    rp => rp.Id == inputDto.RequestPurposeId && rp.RequestType == RequestType.Order && !rp.IsDeleted);

                if (requestPurpose == null)
                {
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "Request purpose must be of type Order");
                }

                // Validate item type combinations and determine if this is a weapon order
                bool isWeaponOrder = false;
                if (inputDto.RequestItems != null && inputDto.RequestItems.Any())
                {
                    var itemIds = inputDto.RequestItems.Select(ri => ri.ItemId).Distinct().ToList();
                    var items = await _baseItemRepository.FindAsync(
                        item => itemIds.Contains(item.Id) && !item.IsDeleted);

                    if (items.Count() != itemIds.Count)
                    {
                        var foundIds = items.Select(i => i.Id).ToList();
                        var missingIds = itemIds.Except(foundIds).ToList();
                        _logger.LogWarning("Some items not found. Missing ItemIds: {MissingIds}, User: {UserId}",
                            string.Join(", ", missingIds), currentUserId);
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest, 
                            $"One or more items not found. Item IDs: {string.Join(", ", missingIds)}");
                    }

                    var itemTypes = items.Select(i => i.ItemType).Distinct().ToList();
                    var hasWeapon = itemTypes.Contains(ItemType.Weapon);
                    var hasAmmunition = itemTypes.Contains(ItemType.Ammunition);
                    var hasExplosive = itemTypes.Contains(ItemType.Explosive);
                    var hasOtherTypes = itemTypes.Any(t => t != ItemType.Weapon && t != ItemType.Ammunition && t != ItemType.Explosive);

                    // Rule 1: Weapons cannot be ordered with anything else
                    if (hasWeapon && (hasAmmunition || hasExplosive || hasOtherTypes))
                    {
                        _logger.LogWarning("Invalid order: Weapons cannot be ordered with other item types. ItemTypes: {ItemTypes}, User: {UserId}",
                            string.Join(", ", itemTypes), currentUserId);
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest, 
                            "Weapons cannot be ordered with other item types. All items in a weapon order must be weapons.");
                    }

                    // Determine if this is a weapon order (all items are weapons)
                    isWeaponOrder = items.All(i => i.ItemType == ItemType.Weapon);

                    // Rule 2: Ammunition and Explosive can be ordered together (already satisfied by Rule 1)
                    // Rule 3: All items must be of the same type if ordering weapons (enforced by Rule 1)
                }

                // Step 1: Save files first (before creating order) to create FileUplodMaster records
                List<long> savedFileMasterIds = null;
                if (files != null && files.Count > 0)
                {
                    try
                    {
                        var saveFilesResult = await _fileUploadService.SaveFilesAsync(files, FileEntityType.Order);
                        if (!saveFilesResult.Succeeded || saveFilesResult.Data == null)
                        {
                            _logger.LogWarning("Failed to save files before creating order. Error: {Error}, User: {UserId}",
                                saveFilesResult.Message ?? "Unknown error", currentUserId);
                            // Continue without files - files are optional
                        }
                        else
                        {
                            savedFileMasterIds = saveFilesResult.Data;
                            _logger.LogInformation("Files saved successfully before order creation. FileCount: {FileCount}, MasterIds: {MasterIds}, User: {UserId}",
                                files.Count, string.Join(", ", savedFileMasterIds), currentUserId);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error but don't fail order creation - files are optional
                        _logger.LogError(ex, "Exception occurred while saving files before order creation. User: {UserId}",
                            currentUserId);
                    }
                }

                // Step 2: Map DTO to entity (exclude RequestItems for now)
                var order = _mapper.Map<Order>(inputDto);
                order.RequestType = RequestType.Order; // Always set request type to Order
                order.Status = RequestStatus.New; // Always set initial status to New
                order.CreationDate = _dateTimeProvider.Now;
                order.CreatedBy = currentUserId;
                order.DepartmentId = departmentId.Value;
                order.RequesterId = currentUserId;

                // Generate request number
                order.RequestNo = await _requestNoGeneratorService.GenerateRequestNoAsync(RequestType.Order, departmentId.Value);
                
                // Initialize RequestItems collection if null
                if (order.RequestItems == null)
                {
                    order.RequestItems = new List<RequestItem>();
                }

                // Set audit fields for request items and establish relationship
                if (inputDto.RequestItems != null && inputDto.RequestItems.Any())
                {
                    var requestItems = _mapper.Map<List<RequestItem>>(inputDto.RequestItems);
                    _logger.LogInformation("Adding {ItemCount} request items to order.",
                        requestItems.Count);
                    
                    foreach (var item in requestItems)
                    {
                        item.CreationDate = _dateTimeProvider.Now;
                        item.CreatedBy = _currentUserService.UserId;
                        order.RequestItems.Add(item);
                    }
                }

                // Step 3: Add to repository (this will cascade save RequestItems)
                var createdOrder = await _orderRepository.AddAsync(order);

                // Record history for initial items
                if (createdOrder.RequestItems != null && createdOrder.RequestItems.Any())
                {
                    try
                    {
                        var departmentIdForHistory = _currentUserService.DepartmentId ?? createdOrder.DepartmentId;
                        var userName = _currentUserService.UserName ?? "System";

                        foreach (var item in createdOrder.RequestItems.Where(ri => !ri.IsDeleted))
                        {
                            var historyContext = new OrderItemHistoryContext
                            {
                                OrderId = createdOrder.Id,
                                RequestItemId = item.Id,
                                ItemId = item.ItemId,
                                ActionType = OrderItemActionType.Added,
                                OrderStatus = createdOrder.Status,
                                NewQuantity = item.Quantity,
                                DepartmentId = departmentIdForHistory,
                                ModifiedByUserId = _currentUserService.UserId,
                                ModifiedByUserName = userName,
                                WorkflowApprovalStepId = null, // No workflow step yet for new orders
                                WorkflowStepId = null,
                                Description = $"Item added to order (Order Status: {createdOrder.Status})"
                            };

                            await _orderItemTrackingService.RecordHistoryAsync(historyContext);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to record history for order creation. OrderId: {OrderId}", 
                            createdOrder.Id);
                    }
                }

                // Step 4: Link files to the order (create FileUplodDetails records) if files were saved
                if (savedFileMasterIds != null && savedFileMasterIds.Count > 0)
                {
                    try
                    {
                        foreach (var masterId in savedFileMasterIds)
                        {
                            var detail = new FileUplodDetails
                            {
                                FileUplodMasterId = masterId,
                                Entity = FileEntityType.Order,
                                EntityId = createdOrder.Id
                            };

                            await _fileDetailsRepository.AddAsync(detail);
                        }

                        _logger.LogInformation("Files linked successfully to order. OrderId: {OrderId}, FileCount: {FileCount}, User: {UserId}",
                            createdOrder.Id, savedFileMasterIds.Count, currentUserId);
                    }
                    catch (Exception ex)
                    {
                        // Log error but don't fail order creation - files are optional
                        _logger.LogError(ex, "Exception occurred while linking files to order. OrderId: {OrderId}, User: {UserId}",
                            createdOrder.Id, currentUserId);
                    }
                }

                // Determine which workflow to start for the created order.
                // Priority and rules:
                // 1) If the order was created from an allowance (reserved items), always use
                //    `WorkflowType.OrderFromAllowance` (or `OrderFromAllowance_Weapon` for weapons) because allowance-based orders follow a different approval path.
                // 2) Otherwise, if the request purpose represents a "Training Order" (currently coded as Id == 4),
                //    use `WorkflowType.NormalOrderForTrainingPurpose` (or `NormalOrderForTrainingPurpose_Weapon` for weapons) - training orders have a specific workflow.
                // 3) For all other non-allowance orders, use the default `WorkflowType.NormalOrder` (or `NormalOrder_Weapon` for weapons).
                //
                // Note:
                // - The numeric literal `4` is a magic number that represents the seeded RequestPurpose for "Training Order".
                //   Replace this with a named constant (e.g. `RequestPurposeIds.TrainingOrder`) and keep the seed/migration in sync
                //   to avoid brittle code and accidental mismatches.
                // - The allowance check takes precedence: if an order is both "from allowance" and a training purpose,
                //   it will use the allowance workflow.
                // - Weapon orders use weapon-specific workflows: if all items are weapons, use weapon workflow variants.
                
                WorkflowType workflowType;
                if (createdOrder.IsFromAllowance)
                {
                    workflowType = isWeaponOrder 
                        ? WorkflowType.OrderFromAllowance_Weapon 
                        : WorkflowType.OrderFromAllowance;
                }
                else if (createdOrder.RequestPurposeId == 4)
                {
                    workflowType = isWeaponOrder 
                        ? WorkflowType.NormalOrderForTrainingPurpose_Weapon 
                        : WorkflowType.NormalOrderForTrainingPurpose;
                }
                else
                {
                    workflowType = isWeaponOrder 
                        ? WorkflowType.NormalOrder_Weapon 
                        : WorkflowType.NormalOrder;
                }

                // Start workflow for the order
                var workflowStarted = await _workflowApprovalService.StartWorkflowAsync(createdOrder.Id, workflowType);

                if (workflowStarted)
                {
                    _logger.LogInformation("Workflow started successfully for order. OrderId: {OrderId}, WorkflowType: {WorkflowType}, IsFromAllowance: {IsFromAllowance}, IsWeaponOrder: {IsWeaponOrder}, User: {UserId}",
                        createdOrder.Id, workflowType, createdOrder.IsFromAllowance, isWeaponOrder, currentUserId);
                }
                else
                {
                    _logger.LogWarning("Failed to start workflow for order. OrderId: {OrderId}, WorkflowType: {WorkflowType}, IsFromAllowance: {IsFromAllowance}, IsWeaponOrder: {IsWeaponOrder}, User: {UserId}",
                        createdOrder.Id, workflowType, createdOrder.IsFromAllowance, isWeaponOrder, currentUserId);
                }

                // Send notification
                await NotifyOrderAsync(
                    "Order Created",
                    $"Order request {createdOrder.RequestNo} has been created{(createdOrder.IsFromAllowance ? " from allowance" : "")}.",
                    createdOrder.Id);

                _logger.LogInformation("Order created successfully. OrderId: {OrderId}, OrderNo: {OrderNo}, ItemCount: {ItemCount}, IsFromAllowance: {IsFromAllowance}, User: {UserId}", 
                    createdOrder.Id, createdOrder.RequestNo, createdOrder.RequestItems?.Count ?? 0, createdOrder.IsFromAllowance, currentUserId);
                
                return APIOperationResponse<long>.Success(createdOrder.Id, "Order created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order. User: {UserId}",
                    currentUserId);
                
                var errorMessage = $"An error occurred: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $" | Inner Exception: {ex.InnerException.Message}";
                }
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, errorMessage);
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

        public async Task<APIOperationResponse<bool>> SetSupplyDateAsync(long orderId, DateTime supplyDate)
        {
            _logger.LogInformation("Setting supply date for order. OrderId: {OrderId}, SupplyDate: {SupplyDate}, User: {UserId}",
                orderId, supplyDate, _currentUserService.UserId);

            try
            {
                var order = await _orderRepository.FindOneAsync(o => o.Id == orderId && !o.IsDeleted);
                if (order == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Order not found");
                }

                if(order.Status == RequestStatus.Rejected || order.Status == RequestStatus.Cancelled)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, $"Cannot set supply date. The order has been {order.Status.ToString().ToLower()}.");
                }

                if(order.Status == RequestStatus.Approved)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Cannot set supply date. The order has already been approved.");
                }

                order.SupplyDate = supplyDate;
                order.ModificationDate = _dateTimeProvider.Now;
                order.ModifiedBy = _currentUserService.UserId;

                await _orderRepository.UpdateAsync(order);

                // Notify the order requester
                if (order.RequesterId != null)
                {
                    try
                    {
                        // Get current user details for contact information
                        var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId ?? string.Empty);
                        var currentUserName = currentUser?.FullNameEN ?? currentUser?.UserName ?? "the administrator";
                        var currentUserEmail = currentUser?.Email;
                        var currentUserPhone = currentUser?.PhoneNumber;

                        // Build contact information string
                        var contactParts = new List<string>();
                        if (!string.IsNullOrEmpty(currentUserEmail))
                            contactParts.Add($"email: {currentUserEmail}");
                        if (!string.IsNullOrEmpty(currentUserPhone))
                            contactParts.Add($"phone: {currentUserPhone}");

                        var contactInfo = contactParts.Any()
                            ? $"If this date is not suitable, please contact {currentUserName} ({string.Join(" or ", contactParts)}) to arrange an alternative."
                            : $"If this date is not suitable, please contact {currentUserName} to arrange an alternative.";

                        await _notificationHelperService.SendNotificationAsync(
                            title: "Supply Pickup Date Set",
                            message: $"The supply pickup date for Order #{order.RequestNo} has been set to {order.SupplyDate:yyyy-MM-dd}. {contactInfo}",
                            entityType: "Supply",
                            entityId: order.Id,
                            userIds: new List<string> { order.RequesterId },
                            senderId: _currentUserService.UserId
                        );
                    }
                    catch (Exception ex)
                    {
                        // Log error but don't fail the operation - notifications are non-critical
                        _logger.LogError(ex, "Error sending notification to requester. OrderId: {OrderId}, RequesterId: {RequesterId}",
                            order.Id, order.RequesterId);
                    }

                }

                return APIOperationResponse<bool>.Success(true, "Supply date set successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting supply date. OrderId: {OrderId}", orderId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private async Task NotifyOrderAsync(string title, string message, long entityId)
        {
            try
            {
                var userId = _currentUserService.UserId;
                var userIds = string.IsNullOrWhiteSpace(userId) ? null : new List<string> { userId };

                await _notificationHelperService.SendNotificationAndEmailAsync(
                    title,
                    message,
                    entityType: nameof(Order),
                    entityId: entityId,
                    userIds: userIds,
                    senderId: userId,
                    includeSuperAdmins: true);
            }
            catch (Exception ex)
            {
                // Log but don't fail the operation
                _logger.LogWarning(ex, "Failed to send notification and email for order. OrderId: {OrderId}", entityId);
            }
        }

        public async Task<APIOperationResponse<long>> AddOrderItemAsync(long orderId, CreateUpdateRequestItemDto itemDto)
        {
            _logger.LogInformation("Adding item to order. OrderId: {OrderId}, ItemId: {ItemId}, User: {UserId}", 
                orderId, itemDto.ItemId, _currentUserService.UserId);

            try
            {
                // Check if order exists
                var order = await _orderRepository.FindOneAsync(
                    o => o.Id == orderId && !o.IsDeleted,
                    false,
                    nameof(Order.RequestItems));

                if (order == null)
                {
                    _logger.LogWarning("Order not found. OrderId: {OrderId}, User: {UserId}", 
                        orderId, _currentUserService.UserId);
                    return APIOperationResponse<long>.Fail(ResponseType.NotFound, "Order not found");
                }

                // Check if item already exists in order
                var existingItem = order.RequestItems?.FirstOrDefault(ri => ri.ItemId == itemDto.ItemId && !ri.IsDeleted);
                if (existingItem != null)
                {
                    _logger.LogWarning("Item already exists in order. OrderId: {OrderId}, ItemId: {ItemId}, User: {UserId}", 
                        orderId, itemDto.ItemId, _currentUserService.UserId);
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, 
                        "Item already exists in this order. Use update quantity instead.");
                }

                // Create new request item
                var newItem = _mapper.Map<RequestItem>(itemDto);
                newItem.RequestId = orderId;
                newItem.CreationDate = _dateTimeProvider.Now;
                newItem.CreatedBy = _currentUserService.UserId;

                var createdItem = await _requestItemRepository.AddAsync(newItem);

                // Record history
                try
                {
                    var currentStep = await _workflowApprovalService.GetCurrentApprovalStepByRequestIdAsync((int)orderId);
                    var departmentId = _currentUserService.DepartmentId ?? order.DepartmentId;
                    var userName = _currentUserService.UserName ?? "System";

                    var historyContext = new OrderItemHistoryContext
                    {
                        OrderId = orderId,
                        RequestItemId = createdItem.Id,
                        ItemId = itemDto.ItemId,
                        ActionType = OrderItemActionType.Added,
                        OrderStatus = order.Status,
                        NewQuantity = itemDto.Quantity,
                        DepartmentId = departmentId,
                        ModifiedByUserId = _currentUserService.UserId,
                        ModifiedByUserName = userName,
                        WorkflowApprovalStepId = currentStep?.Id,
                        WorkflowStepId = currentStep?.WorkflowStepId,
                        Description = $"Item added to order (Order Status: {order.Status})"
                    };

                    await _orderItemTrackingService.RecordHistoryAsync(historyContext);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to record history for added item. OrderId: {OrderId}, ItemId: {ItemId}", 
                        orderId, itemDto.ItemId);
                }

                _logger.LogInformation("Item added to order successfully. OrderId: {OrderId}, ItemId: {ItemId}, RequestItemId: {RequestItemId}, User: {UserId}", 
                    orderId, itemDto.ItemId, createdItem.Id, _currentUserService.UserId);

                return APIOperationResponse<long>.Success(createdItem.Id, "Item added to order successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to order. OrderId: {OrderId}, ItemId: {ItemId}, User: {UserId}", 
                    orderId, itemDto.ItemId, _currentUserService.UserId);
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateOrderItemQuantityAsync(long orderId, long itemId, long newQuantity)
        {
            _logger.LogInformation("Updating order item quantity. OrderId: {OrderId}, ItemId: {ItemId}, NewQuantity: {NewQuantity}, User: {UserId}", 
                orderId, itemId, newQuantity, _currentUserService.UserId);

            try
            {
                if (newQuantity <= 0)
                {
                    _logger.LogWarning("Invalid quantity. OrderId: {OrderId}, ItemId: {ItemId}, Quantity: {Quantity}", 
                        orderId, itemId, newQuantity);
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Quantity must be greater than zero");
                }

                // Check if order exists
                var order = await _orderRepository.FindOneAsync(o => o.Id == orderId && !o.IsDeleted);
                if (order == null)
                {
                    _logger.LogWarning("Order not found. OrderId: {OrderId}, User: {UserId}", 
                        orderId, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Order not found");
                }

                // Find the request item
                var requestItem = await _requestItemRepository.FindOneAsync(
                    ri => ri.Id == itemId && ri.RequestId == orderId && !ri.IsDeleted);

                if (requestItem == null)
                {
                    _logger.LogWarning("Order item not found. OrderId: {OrderId}, ItemId: {ItemId}, User: {UserId}", 
                        orderId, itemId, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Order item not found");
                }

                var oldQuantity = requestItem.Quantity;
                requestItem.Quantity = newQuantity;
                requestItem.ModificationDate = _dateTimeProvider.Now;
                requestItem.ModifiedBy = _currentUserService.UserId;

                await _requestItemRepository.UpdateAsync(requestItem);

                // Record history
                try
                {
                    var currentStep = await _workflowApprovalService.GetCurrentApprovalStepByRequestIdAsync((int)orderId);
                    var departmentId = _currentUserService.DepartmentId ?? order.DepartmentId;
                    var userName = _currentUserService.UserName ?? "System";
                    var stepName = currentStep?.WorkflowStep?.ApplicationRole?.Name ?? "Unknown";

                    var historyContext = new OrderItemHistoryContext
                    {
                        OrderId = orderId,
                        RequestItemId = itemId,
                        ItemId = requestItem.ItemId,
                        ActionType = OrderItemActionType.QuantityModified,
                        OrderStatus = order.Status,
                        PreviousQuantity = oldQuantity,
                        NewQuantity = newQuantity,
                        DepartmentId = departmentId,
                        ModifiedByUserId = _currentUserService.UserId,
                        ModifiedByUserName = userName,
                        WorkflowApprovalStepId = currentStep?.Id,
                        WorkflowStepId = currentStep?.WorkflowStepId,
                        Description = $"Quantity changed from {oldQuantity} to {newQuantity} (Order Status: {order.Status})" +
                            (currentStep != null ? $" - Workflow Step: {stepName}" : "")
                    };

                    await _orderItemTrackingService.RecordHistoryAsync(historyContext);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to record history for quantity update. OrderId: {OrderId}, ItemId: {ItemId}", 
                        orderId, itemId);
                }

                _logger.LogInformation("Order item quantity updated. OrderId: {OrderId}, ItemId: {ItemId}, OldQuantity: {OldQuantity}, NewQuantity: {NewQuantity}, User: {UserId}", 
                    orderId, itemId, oldQuantity, newQuantity, _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(true, "Order item quantity updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order item quantity. OrderId: {OrderId}, ItemId: {ItemId}, User: {UserId}", 
                    orderId, itemId, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteOrderItemAsync(long orderId, long itemId)
        {
            _logger.LogInformation("Deleting order item. OrderId: {OrderId}, ItemId: {ItemId}, User: {UserId}", 
                orderId, itemId, _currentUserService.UserId);

            try
            {
                // Check if order exists and get its items
                var order = await _orderRepository.FindOneAsync(
                    o => o.Id == orderId && !o.IsDeleted,
                    false,
                    nameof(Order.RequestItems));

                if (order == null)
                {
                    _logger.LogWarning("Order not found. OrderId: {OrderId}, User: {UserId}", 
                        orderId, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Order not found");
                }

                // Count active items in the order
                var activeItemsCount = order.RequestItems?.Count(ri => !ri.IsDeleted) ?? 0;

                if (activeItemsCount <= 1)
                {
                    _logger.LogWarning("Cannot delete last item from order. OrderId: {OrderId}, ItemId: {ItemId}, User: {UserId}", 
                        orderId, itemId, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, 
                        "Cannot delete the last item from an order. An order must have at least one item.");
                }

                // Find the request item to delete
                var requestItem = await _requestItemRepository.FindOneAsync(
                    ri => ri.Id == itemId && ri.RequestId == orderId && !ri.IsDeleted);

                if (requestItem == null)
                {
                    _logger.LogWarning("Order item not found. OrderId: {OrderId}, ItemId: {ItemId}, User: {UserId}", 
                        orderId, itemId, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Order item not found");
                }

                // Record history before deletion
                try
                {
                    var currentStep = await _workflowApprovalService.GetCurrentApprovalStepByRequestIdAsync((int)orderId);
                    var departmentId = _currentUserService.DepartmentId ?? order.DepartmentId;
                    var userName = _currentUserService.UserName ?? "System";

                    var historyContext = new OrderItemHistoryContext
                    {
                        OrderId = orderId,
                        RequestItemId = itemId,
                        ItemId = requestItem.ItemId,
                        ActionType = OrderItemActionType.Deleted,
                        OrderStatus = order.Status,
                        PreviousQuantity = requestItem.Quantity,
                        DepartmentId = departmentId,
                        ModifiedByUserId = _currentUserService.UserId,
                        ModifiedByUserName = userName,
                        WorkflowApprovalStepId = currentStep?.Id,
                        WorkflowStepId = currentStep?.WorkflowStepId,
                        Description = $"Item removed from order (Order Status: {order.Status})"
                    };

                    await _orderItemTrackingService.RecordHistoryAsync(historyContext);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to record history for deleted item. OrderId: {OrderId}, ItemId: {ItemId}", 
                        orderId, itemId);
                }

                // Soft delete the item
                await _requestItemRepository.DeleteAsync(requestItem);

                _logger.LogInformation("Order item deleted successfully. OrderId: {OrderId}, ItemId: {ItemId}, User: {UserId}", 
                    orderId, itemId, _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(true, "Order item deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order item. OrderId: {OrderId}, ItemId: {ItemId}, User: {UserId}", 
                    orderId, itemId, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<AllowanceVerificationDto>> VerifyItemAllowanceAsync(long itemId, long requestedQuantity)
        {
            try
            {
                var departmentId = _currentUserService.DepartmentId;
                if (!departmentId.HasValue || departmentId.Value <= 0)
                {
                    return APIOperationResponse<AllowanceVerificationDto>.Fail(ResponseType.BadRequest, 
                        "Department not found for current user. Cannot verify allowance.");
                }

                var currentYear = _dateTimeProvider.Now.Year;
                var verification = await CalculateAllowanceAvailabilityAsync(itemId, departmentId.Value, currentYear);
                verification.RequestedQuantity = requestedQuantity;

                return APIOperationResponse<AllowanceVerificationDto>.Success(verification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verifying item allowance. ItemId: {ItemId}", itemId);
                return APIOperationResponse<AllowanceVerificationDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates allowance for all items in an order
        /// </summary>
        private async Task<(bool IsValid, List<string> Errors)> ValidateAllowanceForOrderAsync(
            List<CreateUpdateRequestItemDto> requestItems, 
            long departmentId)
        {
            var errors = new List<string>();
            var currentYear = _dateTimeProvider.Now.Year;

            if (requestItems == null || !requestItems.Any())
            {
                errors.Add("Order must have at least one item");
                return (false, errors);
            }

            // Get all unique item IDs
            var itemIds = requestItems.Select(ri => ri.ItemId).Distinct().ToList();

            // Get all allowance items for this department and year in one query
            var allowanceItems = await _allowanceItemRepository.FindAsync(
                ai => ai.DepartmentId == departmentId && 
                      ai.Year == currentYear && 
                      !ai.IsDeleted
            );

            var allowanceByItemId = allowanceItems.ToDictionary(ai => ai.ItemId, ai => ai.Quantity);

            // Validate each item
            foreach (var requestItem in requestItems)
            {
                // Check if item exists in allowance
                if (!allowanceByItemId.ContainsKey(requestItem.ItemId))
                {
                    errors.Add($"Item {requestItem.ItemId} is not found in the department's allowance for year {currentYear}");
                    continue;
                }

                // Calculate allowance availability
                var verification = await CalculateAllowanceAvailabilityAsync(requestItem.ItemId, departmentId, currentYear);

                // Check if requested quantity can be fulfilled
                if (!verification.CanFulfillRequest || verification.AvailableQuantity < requestItem.Quantity)
                {
                    errors.Add($"Item {requestItem.ItemId}: Requested quantity ({requestItem.Quantity}) exceeds available allowance. " +
                              $"Available: {verification.AvailableQuantity}, " +
                              $"Original Allowance: {verification.OriginalAllowanceQuantity}, " +
                              $"Reserved: {verification.ReservedByOrdersUnderProcessing}, " +
                              $"Used: {verification.UsedQuantity}");
                }
            }

            return (errors.Count == 0, errors);
        }

        /// <summary>
        /// Calculates allowance availability details for a specific item
        /// </summary>
        private async Task<AllowanceVerificationDto> CalculateAllowanceAvailabilityAsync(
            long itemId, 
            long departmentId, 
            int year)
        {
            var verification = new AllowanceVerificationDto
            {
                ItemId = itemId,
                DepartmentId = departmentId,
                Year = year
            };

            // Get original allowance quantity
            var allowanceItem = await _allowanceItemRepository.FindOneAsync(
                ai => ai.ItemId == itemId && 
                      ai.DepartmentId == departmentId && 
                      ai.Year == year && 
                      !ai.IsDeleted
            );

            if (allowanceItem == null)
            {
                verification.ItemExistsInAllowance = false;
                verification.OriginalAllowanceQuantity = 0;
                verification.ReservedByOrdersUnderProcessing = 0;
                verification.UsedQuantity = 0;
                return verification;
            }

            verification.ItemExistsInAllowance = true;
            verification.OriginalAllowanceQuantity = allowanceItem.Quantity;

            // Get all orders for this department that are from allowance
            var departmentOrders = await _orderRepository.FindAsync(
                o => o.DepartmentId == departmentId && 
                     o.IsFromAllowance && 
                     o.CreationDate.Year == year &&
                     !o.IsDeleted
            );

            var orderIds = departmentOrders.Select(o => o.Id).ToList();

            if (!orderIds.Any())
            {
                verification.ReservedByOrdersUnderProcessing = 0;
                verification.UsedQuantity = 0;
                return verification;
            }

            // Get all supplies for these orders
            var supplies = await _supplyRepository.FindAsync(
                s => orderIds.Contains(s.OrderId) && !s.IsDeleted,
                false,
                nameof(Supply.SupplyDetails)
            );

            // Separate Draft and Submitted supplies
            var draftSupplies = supplies.Where(s => s.SubmissionStatus == SupplySubmissionStatus.Draft).ToList();
            var submittedSupplies = supplies.Where(s => s.SubmissionStatus == SupplySubmissionStatus.Submitted).ToList();

            // Calculate ReservedByOrdersUnderProcessing from Draft supplies
            var draftSupplyIds = draftSupplies.Select(s => s.Id).ToList();
            if (draftSupplyIds.Any())
            {
                var draftSupplyDetails = await _supplyDetailRepository.FindAsync(
                    sd => draftSupplyIds.Contains(sd.SupplyId) && 
                          sd.ItemId == itemId && 
                          !sd.IsDeleted
                );

                verification.ReservedByOrdersUnderProcessing = draftSupplyDetails.Sum(sd => sd.Quantity);
            }
            else
            {
                verification.ReservedByOrdersUnderProcessing = 0;
            }

            // Calculate UsedQuantity from Submitted supplies
            var submittedSupplyIds = submittedSupplies.Select(s => s.Id).ToList();
            if (submittedSupplyIds.Any())
            {
                var submittedSupplyDetails = await _supplyDetailRepository.FindAsync(
                    sd => submittedSupplyIds.Contains(sd.SupplyId) && 
                          sd.ItemId == itemId && 
                          !sd.IsDeleted
                );

                verification.UsedQuantity = submittedSupplyDetails.Sum(sd => sd.Quantity);
            }
            else
            {
                verification.UsedQuantity = 0;
            }

            return verification;
        }
    }
}
