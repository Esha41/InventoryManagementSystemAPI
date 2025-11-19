using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
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
        private readonly ICrossCuttingRepository<SupplyDetail> _supplyDetailsRepository;
        private readonly ICrossCuttingRepository<Supply> _supplyRepository;
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
            ICrossCuttingRepository<SupplyDetail> supplyDetailsRepository,
            ICrossCuttingRepository<Supply> supplyRepository,
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
            _supplyDetailsRepository = supplyDetailsRepository;
            _supplyRepository = supplyRepository;
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

        public async Task<APIOperationResponse<OrderSupplySuggestionDto>> SuggestSupplyForOrderAsync(long orderId, List<long>? depotIds = null)
        {
            _logger.LogInformation("Generating supply suggestion for order. OrderId: {OrderId}, DepotIds: {DepotIds}, User: {UserId}", 
                orderId, depotIds != null ? string.Join(", ", depotIds) : "All", _currentUserService.UserId);

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

				// Get all existing supplies for this order (excluding deleted)
				var existingSuppliesForOrder = await _supplyRepository.FindAsync(
					s => s.OrderId == orderId && !s.IsDeleted,
					false,
					nameof(Supply.SupplyDetails)
				);

                // Get all supply details for these supplies
                var supplyIds = existingSuppliesForOrder.Select(s => s.Id).ToList();
                var existingSupplyDetails = supplyIds.Any()
                    ? await _supplyDetailsRepository.FindAsync(
                        sd => supplyIds.Contains(sd.SupplyId) && !sd.IsDeleted)
                    : new List<SupplyDetail>();

                // Calculate already supplied quantities per item
                var suppliedQuantitiesByItem = existingSupplyDetails
                    .GroupBy(sd => sd.ItemId)
                    .ToDictionary(g => g.Key, g => g.Sum(sd => sd.Quantity));

                _logger.LogInformation("Found existing supplies for order. OrderId: {OrderId}, SuppliedItemsCount: {Count}", 
                    orderId, suppliedQuantitiesByItem.Count);

                var suggestion = new OrderSupplySuggestionDto
                {
                    OrderId = order.Id,
                    OrderNo = order.RequestNo,
                    DepartmentId = order.DepartmentId,
                    ItemSuggestions = new List<OrderItemSupplySuggestionDto>()
                };

                bool allItemsCanBeFulfilled = true;
                bool hasItemsNeedingSupply = false;

                _logger.LogInformation("Processing {ItemCount} items for supply suggestion. OrderId: {OrderId}", 
                    order.RequestItems.Count, orderId);

                // Process each request item
                foreach (var requestItem in order.RequestItems.Where(ri => !ri.IsDeleted))
                {
                    // Calculate already supplied quantity for this item
                    var alreadySuppliedQuantity = suppliedQuantitiesByItem.TryGetValue(requestItem.ItemId, out var supplied) ? supplied : 0;
                    var remainingQuantityNeeded = requestItem.Quantity - alreadySuppliedQuantity;

                    _logger.LogInformation("Item supply status. ItemId: {ItemId}, Requested: {Requested}, AlreadySupplied: {Supplied}, RemainingNeeded: {Remaining}, OrderId: {OrderId}",
                        requestItem.ItemId, requestItem.Quantity, alreadySuppliedQuantity, remainingQuantityNeeded, orderId);

                    // Skip items that are already fully supplied
                    if (remainingQuantityNeeded <= 0)
                    {
                        _logger.LogInformation("Item already fully supplied. ItemId: {ItemId}, Requested: {Requested}, Supplied: {Supplied}, OrderId: {OrderId}",
                            requestItem.ItemId, requestItem.Quantity, alreadySuppliedQuantity, orderId);
                        continue;
                    }

                    hasItemsNeedingSupply = true;

                    var itemSuggestion = new OrderItemSupplySuggestionDto
                    {
                        RequestItemId = requestItem.Id,
                        ItemId = requestItem.ItemId,
                        ItemName = requestItem.Item.Name,
                        RequestedQuantity = requestItem.Quantity,
                        SuggestedQuantity = 0,
                        LotSuggestions = new List<SupplyLotSuggestionDto>()
                    };

                    // Get available lots for the remaining quantity needed
                    var availableLotsResponse = await GetAvailableLotsForQuantityAsync(requestItem.ItemId, remainingQuantityNeeded, depotIds);

                    if (!availableLotsResponse.Succeeded)
                    {
                        _logger.LogError("Failed to get available lots for item. ItemId: {ItemId}, Error: {Error}",
                            requestItem.ItemId, availableLotsResponse.Message);
                        return APIOperationResponse<OrderSupplySuggestionDto>.Fail((ResponseType)availableLotsResponse.StatusCode, availableLotsResponse.Message);
                    }

                    var availableLots = availableLotsResponse.Data;

                    _logger.LogInformation("Found {LotCount} available lots for item. ItemId: {ItemId}, RemainingNeeded: {Remaining}, OrderId: {OrderId}",
                        availableLots.Count, requestItem.ItemId, remainingQuantityNeeded, orderId);

                    long remainingQuantity = remainingQuantityNeeded;

                    // Allocate quantities from available lots using FEFO logic
                    foreach (var lotDetail in availableLots)
                    {
                        if (remainingQuantity <= 0)
                            break;

                        long quantityToAllocate = Math.Min(remainingQuantity, lotDetail.RemainingQuantity);

                        itemSuggestion.LotSuggestions.Add(new SupplyLotSuggestionDto
                        {
                            InventoryDetailId = lotDetail.InventoryDetailId,
                            ItemId = lotDetail.ItemId,
                            ItemName = lotDetail.ItemName,
                            Lot = lotDetail.Lot,
                            AvailableQuantity = lotDetail.RemainingQuantity,
                            SuggestedQuantity = quantityToAllocate,
                            ExpiryDate = lotDetail.ExpiryDate,
                            InventoryId = lotDetail.InventoryId,
                            Depot = lotDetail.Depot,
                            Supplier = lotDetail.Supplier,
                            Manufacturer = lotDetail.Manufacturer
                        });

                        itemSuggestion.SuggestedQuantity += quantityToAllocate;
                        remainingQuantity -= quantityToAllocate;
                    }

                    // Check if we can fulfill the remaining quantity needed
                    itemSuggestion.CanFulfillCompletely = (itemSuggestion.SuggestedQuantity >= remainingQuantityNeeded);

                    if (!itemSuggestion.CanFulfillCompletely)
                    {
                        allItemsCanBeFulfilled = false;
                        _logger.LogWarning("Insufficient available inventory for remaining quantity. ItemId: {ItemId}, RemainingNeeded: {Remaining}, Available: {Available}, OrderId: {OrderId}",
                            requestItem.ItemId, remainingQuantityNeeded, itemSuggestion.SuggestedQuantity, orderId);
                    }

                    suggestion.ItemSuggestions.Add(itemSuggestion);
                }

                // If order is fully supplied, return empty suggestion
                if (!hasItemsNeedingSupply)
                {
                    suggestion.CanFulfillCompletely = true;
                    suggestion.Message = "Order is already fully supplied. No suggestions needed.";
                    _logger.LogInformation("Order is fully supplied. OrderId: {OrderId}, User: {UserId}", 
                        orderId, _currentUserService.UserId);
                    return APIOperationResponse<OrderSupplySuggestionDto>.Success(suggestion);
                }

                suggestion.CanFulfillCompletely = allItemsCanBeFulfilled;
                suggestion.Message = allItemsCanBeFulfilled 
                    ? "All remaining items can be fulfilled from available inventory" 
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

        public async Task<APIOperationResponse<List<LotDetailDto>>> GetLotsByItemIdAsync(long itemId)
        {
            _logger.LogInformation("Getting lots for item. ItemId: {ItemId}, User: {UserId}",
                itemId, _currentUserService.UserId);

            try
            {
                // Get all inventory details for this item in ONE query
                var inventoryDetails = await _inventoryDetailRepository.FindAsync(
                    id => id.ItemId == itemId && id.ItemQuantity > 0,
                    false,
                    nameof(InventoryDetailEntity.Inventory),
                    $"{nameof(InventoryDetailEntity.Inventory)}.{nameof(InventoryEntity.Depo)}",
                    nameof(InventoryDetailEntity.Item),
                    nameof(InventoryDetailEntity.Supplier),
                    nameof(InventoryDetailEntity.Manufacturer),
                    nameof(InventoryDetailEntity.Country)
                );

                var lots = inventoryDetails
                    .Where(id => !id.Inventory.IsDeleted)
                    .ToList();

                _logger.LogInformation("Found {LotCount} lots for item. ItemId: {ItemId}",
                    lots.Count, itemId);

                if (lots.Count == 0)
                {
                    _logger.LogInformation("No lots found for item. ItemId: {ItemId}, User: {UserId}",
                        itemId, _currentUserService.UserId);
                    return APIOperationResponse<List<LotDetailDto>>.Success(new List<LotDetailDto>());
                }

                // Get ALL supply details for this item in ONE query (not per lot)
                var allSupplyDetails = await _supplyDetailsRepository.FindAsync(
                    sd => sd.ItemId == itemId
                );

                // Group supply details by lot for efficient lookup
                var supplyDetailsByLot = allSupplyDetails
                    .GroupBy(sd => sd.Lot)
                    .ToDictionary(g => g.Key, g => g.Sum(sd => sd.Quantity));

                _logger.LogInformation("Calculated usage for {LotCount} lots. ItemId: {ItemId}",
                    supplyDetailsByLot.Count, itemId);

                var lotDetails = new List<LotDetailDto>();

                foreach (var lot in lots)
                {
                    // Get used quantity from the grouped data (O(1) lookup)
                    long totalUsedQuantity = supplyDetailsByLot.TryGetValue(lot.Lot, out var usedQty) ? usedQty : 0;
                    long remainingQuantity = lot.ItemQuantity - totalUsedQuantity;

                    // A lot is empty if remaining quantity is 0 or less
                    bool isEmptyLot = remainingQuantity <= 0;

                    // Check if the lot is expired (expiry date is in the past)
                    bool isExpired = lot.Item.ExpiryDate.HasValue && lot.Item.ExpiryDate.Value.Date < DateTime.UtcNow.Date;

                    // Use AutoMapper to create the base mapping
                    var lotDetail = _mapper.Map<LotDetailDto>(lot);

                    // Set calculated properties
                    lotDetail.UsedQuantity = totalUsedQuantity;
                    lotDetail.RemainingQuantity = Math.Max(0, remainingQuantity);
                    lotDetail.IsEmptyLot = isEmptyLot;
                    lotDetail.IsExpired = isExpired;

                    lotDetails.Add(lotDetail);
                }

                // Sort by expiry date (FEFO - First Expiry First Out), then by lot number
                lotDetails = lotDetails
                    .OrderBy(l => l.ExpiryDate.HasValue ? 0 : 1)  // Non-null expiry dates first
                    .ThenBy(l => l.ExpiryDate)                    // Then sort by expiry date
                    .ThenBy(l => l.Lot)                           // Then by lot number
                    .ToList();

                _logger.LogInformation("Successfully retrieved {LotCount} lots for item. ItemId: {ItemId}, User: {UserId}",
                    lotDetails.Count, itemId, _currentUserService.UserId);

                return APIOperationResponse<List<LotDetailDto>>.Success(lotDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting lots for item. ItemId: {ItemId}, User: {UserId}",
                    itemId, _currentUserService.UserId);
                return APIOperationResponse<List<LotDetailDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<LotDetailDto>>> GetAvailableLotsForQuantityAsync(long itemId, long requiredQuantity, List<long>? depotIds = null)
        {
            _logger.LogInformation("Getting available lots for quantity. ItemId: {ItemId}, RequiredQuantity: {RequiredQuantity}, DepotIds: {DepotIds}, User: {UserId}",
                itemId, requiredQuantity, depotIds != null ? string.Join(", ", depotIds) : "All", _currentUserService.UserId);

            try
            {
                if (requiredQuantity <= 0)
                {
                    _logger.LogWarning("Invalid required quantity: {RequiredQuantity}. ItemId: {ItemId}, User: {UserId}",
                        requiredQuantity, itemId, _currentUserService.UserId);
                    return APIOperationResponse<List<LotDetailDto>>.Fail(ResponseType.BadRequest, "Required quantity must be greater than 0");
                }

                // Get all inventory details for this item in ONE query
                var inventoryDetails = await _inventoryDetailRepository.FindAsync(
                    id => id.ItemId == itemId && id.ItemQuantity > 0,
                    false,
                    nameof(InventoryDetailEntity.Inventory),
                    $"{nameof(InventoryDetailEntity.Inventory)}.{nameof(InventoryEntity.Depo)}",
                    nameof(InventoryDetailEntity.Item),
                    nameof(InventoryDetailEntity.Supplier),
                    nameof(InventoryDetailEntity.Manufacturer),
                    nameof(InventoryDetailEntity.Country)
                );

                var lots = inventoryDetails
                    .Where(id => !id.Inventory.IsDeleted)
                    .ToList();

                // Filter by depot IDs if provided
                if (depotIds != null && depotIds.Any())
                {
                    lots = lots
                        .Where(l => depotIds.Contains(l.Inventory.DepoId))
                        .ToList();
                }

                _logger.LogInformation("Found {LotCount} total lots for item. ItemId: {ItemId}",
                    lots.Count, itemId);

                if (lots.Count == 0)
                {
                    _logger.LogInformation("No lots found for item. ItemId: {ItemId}, User: {UserId}",
                        itemId, _currentUserService.UserId);
                    return APIOperationResponse<List<LotDetailDto>>.Success(new List<LotDetailDto>());
                }

                // Get ALL supply details for this item in ONE query
                var allSupplyDetails = await _supplyDetailsRepository.FindAsync(sd => sd.ItemId == itemId);

                // Group supply details by lot for efficient lookup
                var supplyDetailsByLot = allSupplyDetails
                    .GroupBy(sd => sd.Lot)
                    .ToDictionary(g => g.Key, g => g.Sum(sd => sd.Quantity));

                _logger.LogInformation("Calculated usage for {LotCount} lots. ItemId: {ItemId}",
                    supplyDetailsByLot.Count, itemId);

                // Filter and sort available lots (not expired, not empty, FEFO order)
                var currentDate = DateTime.UtcNow.Date;
                var availableLots = new List<LotDetailDto>();
                long remainingQuantityNeeded = requiredQuantity;

                // Sort lots by FEFO: items with expiry dates first (sorted by date), then items without expiry dates
                var sortedLots = lots
                    .OrderBy(l => l.Item.ExpiryDate.HasValue ? 0 : 1)  // Non-null expiry dates first
                    .ThenBy(l => l.Item.ExpiryDate)                    // Then sort by expiry date
                    .ThenBy(l => l.Lot)                                // Then by lot number
                    .ToList();

                foreach (var lot in sortedLots)
                {
                    // Skip expired lots
                    bool isExpired = lot.Item.ExpiryDate.HasValue && lot.Item.ExpiryDate.Value.Date < currentDate;
                    if (isExpired)
                    {
                        _logger.LogDebug("Skipping expired lot. Lot: {Lot}, ExpiryDate: {ExpiryDate}, ItemId: {ItemId}",
                            lot.Lot, lot.Item.ExpiryDate, itemId);
                        continue;
                    }

                    // Calculate used quantity and remaining quantity
                    long totalUsedQuantity = supplyDetailsByLot.TryGetValue(lot.Lot, out var usedQty) ? usedQty : 0;
                    long remainingQuantity = lot.ItemQuantity - totalUsedQuantity;

                    // Skip empty lots
                    if (remainingQuantity <= 0)
                    {
                        _logger.LogDebug("Skipping empty lot. Lot: {Lot}, RemainingQuantity: {RemainingQuantity}, ItemId: {ItemId}",
                            lot.Lot, remainingQuantity, itemId);
                        continue;
                    }

                    // Use AutoMapper to create the base mapping
                    var lotDetail = _mapper.Map<LotDetailDto>(lot);

                    // Set calculated properties
                    lotDetail.UsedQuantity = totalUsedQuantity;
                    lotDetail.RemainingQuantity = remainingQuantity;
                    lotDetail.IsEmptyLot = false; // We already filtered out empty lots
                    lotDetail.IsExpired = false;  // We already filtered out expired lots

                    availableLots.Add(lotDetail);

                    // Check if we have enough quantity now
                    remainingQuantityNeeded -= remainingQuantity;
                    if (remainingQuantityNeeded <= 0)
                    {
                        _logger.LogInformation("Found sufficient lots for required quantity. Required: {Required}, Found: {Found}, LotCount: {LotCount}",
                            requiredQuantity, requiredQuantity - remainingQuantityNeeded, availableLots.Count);
                        break; // We have enough, no need to check more lots
                    }
                }

                if (remainingQuantityNeeded > 0)
                {
                    _logger.LogWarning("Insufficient available inventory for item. ItemId: {ItemId}, Required: {Required}, Available: {Available}",
                        itemId, requiredQuantity, requiredQuantity - remainingQuantityNeeded);
                }

                _logger.LogInformation("Found {LotCount} available lots for quantity. ItemId: {ItemId}, RequiredQuantity: {RequiredQuantity}, User: {UserId}",
                    availableLots.Count, itemId, requiredQuantity, _currentUserService.UserId);

                return APIOperationResponse<List<LotDetailDto>>.Success(availableLots);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting available lots for quantity. ItemId: {ItemId}, RequiredQuantity: {RequiredQuantity}, User: {UserId}",
                    itemId, requiredQuantity, _currentUserService.UserId);
                return APIOperationResponse<List<LotDetailDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
