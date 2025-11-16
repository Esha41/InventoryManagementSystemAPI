using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using InventoryEntity = Ettad.Data.Entities.Inventory;
using InventoryDetailEntity = Ettad.Data.Entities.InventoryDetail;

namespace Ettad.Inventory.Service.Inventories
{
    public class InventoryService : IInventoryService
    {
        private readonly ICrossCuttingRepository<InventoryEntity> _inventoryRepository;
        private readonly ICrossCuttingRepository<InventoryDetailEntity> _inventoryDetailRepository;
        private readonly ICrossCuttingRepository<Order> _orderRepository;
        private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateInventoryDto> _createValidator;
        private readonly IValidator<UpdateInventoryDto> _updateValidator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(
            ICrossCuttingRepository<InventoryEntity> inventoryRepository,
            ICrossCuttingRepository<InventoryDetailEntity> inventoryDetailRepository,
            ICrossCuttingRepository<Order> orderRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            IMapper mapper,
            IValidator<CreateInventoryDto> createValidator,
            IValidator<UpdateInventoryDto> updateValidator,
            ICurrentUserService currentUserService,
            ILogger<InventoryService> logger)
        {
            _inventoryRepository = inventoryRepository;
            _inventoryDetailRepository = inventoryDetailRepository;
            _orderRepository = orderRepository;
            _requestItemRepository = requestItemRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<APIOperationResponse<InventoryDto>> GetByIdAsync(long id)
        {
            _logger.LogInformation("Getting inventory by ID. InventoryId: {InventoryId}, User: {UserId}", 
                id, _currentUserService.UserId);
            
            try
            {
                var inventory = await _inventoryRepository.FindOneAsync(
                    i => i.Id == id && !i.IsDeleted,
                    false,
                    nameof(InventoryEntity.Depo),
                    $"{nameof(InventoryEntity.InventoryDetails)}.{nameof(InventoryDetailEntity.Item)}",
                    $"{nameof(InventoryEntity.InventoryDetails)}.{nameof(InventoryDetailEntity.Item)}.Hcc",
                    $"{nameof(InventoryEntity.InventoryDetails)}.{nameof(InventoryDetailEntity.Supplier)}",
                    $"{nameof(InventoryEntity.InventoryDetails)}.{nameof(InventoryDetailEntity.Manufacturer)}",
                    $"{nameof(InventoryEntity.InventoryDetails)}.{nameof(InventoryDetailEntity.Country)}"
                );

                if (inventory == null)
                {
                    _logger.LogWarning("Inventory not found. InventoryId: {InventoryId}, User: {UserId}", 
                        id, _currentUserService.UserId);
                    return APIOperationResponse<InventoryDto>.Fail(ResponseType.NotFound, "Inventory not found");
                }

                _logger.LogInformation("Inventory retrieved successfully. InventoryId: {InventoryId}, DetailCount: {DetailCount}", 
                    id, inventory.InventoryDetails?.Count ?? 0);
                
                var dto = _mapper.Map<InventoryDto>(inventory);
                return APIOperationResponse<InventoryDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting inventory by ID. InventoryId: {InventoryId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<InventoryDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<InventoryDto>>> GetAllAsync()
        {
            _logger.LogInformation("Getting all inventories. User: {UserId}", _currentUserService.UserId);
            
            try
            {
                var inventories = await _inventoryRepository.FindAsync(
                    i => !i.IsDeleted,
                    false,
                    nameof(InventoryEntity.Depo),
                    $"{nameof(InventoryEntity.InventoryDetails)}.{nameof(InventoryDetailEntity.Item)}",
                    $"{nameof(InventoryEntity.InventoryDetails)}.{nameof(InventoryDetailEntity.Item)}.Hcc",
                    $"{nameof(InventoryEntity.InventoryDetails)}.{nameof(InventoryDetailEntity.Supplier)}",
                    $"{nameof(InventoryEntity.InventoryDetails)}.{nameof(InventoryDetailEntity.Manufacturer)}",
                    $"{nameof(InventoryEntity.InventoryDetails)}.{nameof(InventoryDetailEntity.Country)}"
                );

                var dtos = _mapper.Map<List<InventoryDto>>(inventories);
                _logger.LogInformation("Successfully retrieved {InventoryCount} inventories. User: {UserId}", 
                    dtos.Count, _currentUserService.UserId);
                return APIOperationResponse<List<InventoryDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all inventories. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<InventoryDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateInventoryDto inputDto)
        {
            _logger.LogInformation("Creating new inventory. DepoId: {DepoId}, DetailCount: {DetailCount}, User: {UserId}", 
                inputDto?.DepoId, inputDto?.InventoryDetails?.Count ?? 0, _currentUserService.UserId);
            
            try
            {
                // Validate input
                var validationResult = await _createValidator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning("Inventory validation failed. Errors: {ValidationErrors}, User: {UserId}", 
                        errors, _currentUserService.UserId);
                   
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                // Map DTO to entity
                var inventory = _mapper.Map<InventoryEntity>(inputDto);
                inventory.CreationDate = DateTime.UtcNow;
                inventory.CreatedBy = _currentUserService.UserId;

                // Map inventory details
                inventory.InventoryDetails = inputDto.InventoryDetails
                    .Select(d =>
                    {
                        var detail = _mapper.Map<InventoryDetailEntity>(d);
                        return detail;
                    })
                    .ToList();

                _logger.LogInformation("Adding {DetailCount} inventory details. User: {UserId}", 
                    inventory.InventoryDetails.Count, _currentUserService.UserId);

                // Add to repository
                var createdInventory = await _inventoryRepository.AddAsync(inventory);
                _logger.LogInformation("Inventory created successfully. InventoryId: {InventoryId}, DetailCount: {DetailCount}, User: {UserId}",
                       createdInventory.Id, inventory.InventoryDetails.Count, _currentUserService.UserId);

                return APIOperationResponse<long>.Success(createdInventory.Id, "Inventory created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating inventory. DepoId: {DepoId}, User: {UserId}", 
                    inputDto?.DepoId, _currentUserService.UserId);
             
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, UpdateInventoryDto inputDto)
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

                // Check if inventory exists
                var existingInventory = await _inventoryRepository.FindOneAsync(
                    i => i.Id == id && !i.IsDeleted,
                    false,
                    nameof(InventoryEntity.InventoryDetails)
                );

                if (existingInventory == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Inventory not found");

                // Map updates to entity (excluding InventoryDetails)
                _mapper.Map(inputDto, existingInventory);
                existingInventory.ModificationDate = DateTime.UtcNow;
                existingInventory.ModifiedBy = _currentUserService.UserId;

                // Handle inventory details updates
                // Remove old details that are not in the update DTO
                var existingDetailIds = inputDto.InventoryDetails
                    .Where(d => d.Id.HasValue)
                    .Select(d => d.Id.Value)
                    .ToList();

                var detailsToRemove = existingInventory.InventoryDetails
                    .Where(d => !existingDetailIds.Contains(d.Id))
                    .ToList();

                foreach (var detail in detailsToRemove)
                {
                    await _inventoryDetailRepository.DeleteAsync(detail);
                }

                // Update existing details and add new ones
                foreach (var detailDto in inputDto.InventoryDetails)
                {
                    if (detailDto.Id.HasValue)
                    {
                        // Update existing detail
                        var existingDetail = existingInventory.InventoryDetails.FirstOrDefault(d => d.Id == detailDto.Id.Value);
                        if (existingDetail != null)
                        {
                            _mapper.Map(detailDto, existingDetail);
                        }
                    }
                    else
                    {
                        // Add new detail
                        var newDetail = _mapper.Map<InventoryDetailEntity>(detailDto);
                        newDetail.InventoryId = id;
                        existingInventory.InventoryDetails.Add(newDetail);
                    }
                }

                // Update in repository
                await _inventoryRepository.UpdateAsync(existingInventory);
                return APIOperationResponse<bool>.Success(true, "Inventory updated successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting inventory. InventoryId: {InventoryId}, User: {UserId}", 
                id, _currentUserService.UserId);
            
            try
            {
                var inventory = await _inventoryRepository.FindOneAsync(i => i.Id == id && !i.IsDeleted);
                if (inventory == null)
                {
                    _logger.LogWarning("Inventory not found for deletion. InventoryId: {InventoryId}, User: {UserId}", 
                        id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Inventory not found");
                }

                // Soft delete - interceptor will handle IsDeleted, DeletionDate, and DeletedBy automatically
                await _inventoryRepository.DeleteAsync(inventory);

                _logger.LogInformation("Inventory deleted successfully. InventoryId: {InventoryId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true, "Inventory deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting inventory. InventoryId: {InventoryId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<OrderSupplySuggestionDto>> SuggestSupplyForOrderAsync(long orderId)
        {
            _logger.LogInformation("Generating supply suggestion for order. OrderId: {OrderId}, User: {UserId}", 
                orderId, _currentUserService.UserId);

            try
            {
                // Fetch the order with its items and depot
                var order = await _orderRepository.FindOneAsync(
                    o => o.Id == orderId && !o.IsDeleted,
                    false,
                    nameof(Order.RequestItems),
                    $"{nameof(Order.RequestItems)}.{nameof(RequestItem.Item)}"
                );

                if (order == null)
                {
                    _logger.LogWarning("Order not found. OrderId: {OrderId}, User: {UserId}", 
                        orderId, _currentUserService.UserId);
                    return APIOperationResponse<OrderSupplySuggestionDto>.Fail(ResponseType.NotFound, "Order not found");
                }

                if (order.RequestItems == null || !order.RequestItems.Any())
                {
                    _logger.LogWarning("Order has no items. OrderId: {OrderId}, User: {UserId}", 
                        orderId, _currentUserService.UserId);
                    return APIOperationResponse<OrderSupplySuggestionDto>.Fail(ResponseType.BadRequest, "Order has no items to supply");
                }

                var suggestion = new OrderSupplySuggestionDto
                {
                    OrderId = order.Id,
                    OrderNo = order.RequestNo,
                    DepartmentId = order.DepartmentId,
                    ItemSuggestions = new List<OrderItemSupplySuggestionDto>()
                };

                bool allItemsCanBeFulfilled = true;

                _logger.LogInformation("Processing {ItemCount} items for supply suggestion. OrderId: {OrderId}", 
                    order.RequestItems.Count, orderId);

                // Process each request item
                foreach (var requestItem in order.RequestItems)
                {
                    var itemSuggestion = new OrderItemSupplySuggestionDto
                    {
                        RequestItemId = requestItem.Id,
                        ItemId = requestItem.ItemId,
                        ItemName = requestItem.Item.Name,
                        RequestedQuantity = requestItem.Quantity,
                        SuggestedQuantity = 0,
                        LotSuggestions = new List<SupplyLotSuggestionDto>()
                    };

                    // Query available inventory details for this item across all depots
                    // Order by ExpiryDate ASC (FEFO - First Expiry First Out), with nulls last
                    var availableLotsQuery = await _inventoryDetailRepository.FindAsync(
                        id => id.ItemId == requestItem.ItemId &&
                              id.ItemQuantity > 0,
                        false,
                        nameof(InventoryDetailEntity.Inventory),
                        $"{nameof(InventoryDetailEntity.Inventory)}.{nameof(InventoryEntity.Depo)}",
                        nameof(InventoryDetailEntity.Item),
                        nameof(InventoryDetailEntity.Supplier),
                        nameof(InventoryDetailEntity.Manufacturer)
                    );

                    // Sort in memory: items with expiry dates first (sorted by date), then items without expiry dates
                    var availableLots = availableLotsQuery
                        .Where(id => !id.Inventory.IsDeleted)
                        .OrderBy(id => id.Item.ExpiryDate.HasValue ? 0 : 1)  // Non-null expiry dates first
                        .ThenBy(id => id.Item.ExpiryDate)                    // Then sort by expiry date
                        .ThenBy(id => id.Lot)                                // Then by lot number
                        .ToList();

                    _logger.LogInformation("Found {LotCount} available lots for item. ItemId: {ItemId}, OrderId: {OrderId}", 
                        availableLots.Count, requestItem.ItemId, orderId);

                    long remainingQuantity = requestItem.Quantity;

                    // Allocate quantities from lots using FEFO logic
                    foreach (var lot in availableLots)
                    {
                        if (remainingQuantity <= 0)
                            break;

                        long quantityToAllocate = Math.Min(remainingQuantity, lot.ItemQuantity);

                        itemSuggestion.LotSuggestions.Add(new SupplyLotSuggestionDto
                        {
                            InventoryDetailId = lot.Id,
                            ItemId = lot.ItemId,
                            ItemName = lot.Item.Name,
                            Lot = lot.Lot,
                            AvailableQuantity = lot.ItemQuantity,
                            SuggestedQuantity = quantityToAllocate,
                            ExpiryDate = lot.Item.ExpiryDate,
                            InventoryId = lot.InventoryId,
                            Depot = _mapper.Map<Module.lookup.Dtos.DepotDto>(lot.Inventory.Depo),
                            Supplier = lot.Supplier != null ? _mapper.Map<Module.lookup.Dtos.SupplierDto>(lot.Supplier) : null,
                            Manufacturer = lot.Manufacturer != null ? _mapper.Map<Module.lookup.Dtos.ManufacturerDto>(lot.Manufacturer) : null
                        });

                        itemSuggestion.SuggestedQuantity += quantityToAllocate;
                        remainingQuantity -= quantityToAllocate;
                    }

                    itemSuggestion.CanFulfillCompletely = (itemSuggestion.SuggestedQuantity >= itemSuggestion.RequestedQuantity);
                    
                    if (!itemSuggestion.CanFulfillCompletely)
                    {
                        allItemsCanBeFulfilled = false;
                        _logger.LogWarning("Insufficient inventory for item. ItemId: {ItemId}, Requested: {Requested}, Available: {Available}, OrderId: {OrderId}", 
                            requestItem.ItemId, requestItem.Quantity, itemSuggestion.SuggestedQuantity, orderId);
                    }

                    suggestion.ItemSuggestions.Add(itemSuggestion);
                }

                suggestion.CanFulfillCompletely = allItemsCanBeFulfilled;
                suggestion.Message = allItemsCanBeFulfilled 
                    ? "All items can be fulfilled from available inventory" 
                    : "Some items cannot be fully fulfilled due to insufficient inventory";

                _logger.LogInformation("Supply suggestion generated. OrderId: {OrderId}, CanFulfillCompletely: {CanFulfill}, ItemCount: {ItemCount}", 
                    orderId, suggestion.CanFulfillCompletely, suggestion.ItemSuggestions.Count);

                return APIOperationResponse<OrderSupplySuggestionDto>.Success(suggestion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating supply suggestion. OrderId: {OrderId}, User: {UserId}", 
                    orderId, _currentUserService.UserId);
                return APIOperationResponse<OrderSupplySuggestionDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
