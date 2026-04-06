using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Inventories.Dtos;
using Ettad.Inventory.Services.Common;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Lookups.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using Ettad.CrossCutting.Comman.Models;
using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Comman.Enums;
using InventoryEntity = Ettad.Data.Entities.Inventory;
using InventoryDetailEntity = Ettad.Data.Entities.InventoryDetail;

namespace Ettad.Inventory.Service.Inventories
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICrossCuttingRepository<InventoryEntity> _inventoryRepository;
        private readonly ICrossCuttingRepository<InventoryDetailEntity> _inventoryDetailRepository;
        private readonly ICrossCuttingRepository<Order> _orderRepository;
        private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;
        private readonly ICrossCuttingRepository<SupplyDetail> _supplyDetailsRepository;
        private readonly ICrossCuttingRepository<Supply> _supplyRepository;
        private readonly ICrossCuttingRepository<BaseItem> _baseItemRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateInventoryDto> _createValidator;
        private readonly IValidator<UpdateInventoryDto> _updateValidator;
        private readonly IExcelImportService _excelImportService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<InventoryService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IDepotAccessService _depotAccessService;
        private readonly IFileUploadService _fileUploadService;

        public InventoryService(
            ApplicationDbContext context,
            ICrossCuttingRepository<InventoryEntity> inventoryRepository,
            ICrossCuttingRepository<InventoryDetailEntity> inventoryDetailRepository,
            ICrossCuttingRepository<Order> orderRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            ICrossCuttingRepository<SupplyDetail> supplyDetailsRepository,
            ICrossCuttingRepository<Supply> supplyRepository,
            ICrossCuttingRepository<BaseItem> baseItemRepository,
            IMapper mapper,
            IValidator<CreateInventoryDto> createValidator,
            IValidator<UpdateInventoryDto> updateValidator,
            IExcelImportService excelImportService,
            ICurrentUserService currentUserService,
            ILogger<InventoryService> logger,
            IDateTimeProvider dateTimeProvider,
            IDepotAccessService depotAccessService,
            IFileUploadService fileUploadService)
        {
            _context = context;
            _inventoryRepository = inventoryRepository;
            _inventoryDetailRepository = inventoryDetailRepository;
            _orderRepository = orderRepository;
            _requestItemRepository = requestItemRepository;
            _supplyDetailsRepository = supplyDetailsRepository;
            _supplyRepository = supplyRepository;
            _baseItemRepository = baseItemRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _excelImportService = excelImportService;
            _currentUserService = currentUserService;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _depotAccessService = depotAccessService;
            _fileUploadService = fileUploadService;
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

                var userId = _currentUserService.UserId;
                if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, inventory.DepoId))
                {
                    _logger.LogWarning("User {UserId} attempted to access inventory {InventoryId} in unauthorized depot {DepotId}", userId, id, inventory.DepoId);
                    return APIOperationResponse<InventoryDto>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
                }

                _logger.LogInformation("Inventory retrieved successfully. InventoryId: {InventoryId}, DetailCount: {DetailCount}",
                    id, inventory.InventoryDetails?.Count ?? 0);

                var dto = _mapper.Map<InventoryDto>(inventory);

                if (dto.InventoryDetails != null && dto.InventoryDetails.Any())
                {
                    await PopulateInventoryDetailsQuantitiesAsync(dto.InventoryDetails);

                    // Populate files per inventory using file service in a batched way
                    var ammoInventoryIds = dto.InventoryDetails
                        .Where(d => d.Item != null && d.Item.ItemType != ItemType.Explosive)
                        .Select(d => d.InventoryId)
                        .Distinct()
                        .ToList();
                    var explosiveInventoryIds = dto.InventoryDetails
                        .Where(d => d.Item != null && d.Item.ItemType == ItemType.Explosive)
                        .Select(d => d.InventoryId)
                        .Distinct()
                        .ToList();

                    Dictionary<long, List<FileUploadDto>> ammoFiles = new();
                    Dictionary<long, List<FileUploadDto>> explosiveFiles = new();

                    if (ammoInventoryIds.Any())
                    {
                        var filesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Ammunition, ammoInventoryIds);
                        ammoFiles = filesResult.Data ?? new Dictionary<long, List<FileUploadDto>>();
                    }
                    if (explosiveInventoryIds.Any())
                    {
                        var filesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Explosive, explosiveInventoryIds);
                        explosiveFiles = filesResult.Data ?? new Dictionary<long, List<FileUploadDto>>();
                    }

                    foreach (var detail in dto.InventoryDetails)
                    {
                        if (detail.Item != null && detail.Item.ItemType == ItemType.Explosive)
                        {
                            detail.Files = explosiveFiles.TryGetValue(detail.InventoryId, out var list) ? list : new List<FileUploadDto>();
                        }
                        else
                        {
                            detail.Files = ammoFiles.TryGetValue(detail.InventoryId, out var list) ? list : new List<FileUploadDto>();
                        }
                    }
                }

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
                    $"{nameof(InventoryEntity.InventoryDetails)}.{nameof(InventoryDetailEntity.Supplier)}",
                    $"{nameof(InventoryEntity.InventoryDetails)}.{nameof(InventoryDetailEntity.Manufacturer)}",
                    $"{nameof(InventoryEntity.InventoryDetails)}.{nameof(InventoryDetailEntity.Country)}"
                );

                var dtos = _mapper.Map<List<InventoryDto>>(inventories);

                var allDetails = dtos.SelectMany(d => d.InventoryDetails ?? new List<InventoryDetailDto>()).ToList();
                if (allDetails.Any())
                {
                    await PopulateInventoryDetailsQuantitiesAsync(allDetails);
                }

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

        public async Task<APIOperationResponse<long>> CreateAsync(CreateInventoryDto inputDto, List<IFormFile>? files = null)
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
                inventory.CreationDate = _dateTimeProvider.Now;
                inventory.CreatedBy = _currentUserService.UserId;

                // Map inventory details
                inventory.InventoryDetails = inputDto.InventoryDetails
                    .Select(d =>
                    {
                        var detail = _mapper.Map<InventoryDetailEntity>(d);
                        detail.Lot = (d.Lot ?? string.Empty).Trim();
                        return detail;
                    })
                    .ToList();

                _logger.LogInformation("Adding {DetailCount} inventory details. User: {UserId}",
                    inventory.InventoryDetails.Count, _currentUserService.UserId);

                // Add to repository
                var createdInventory = await _inventoryRepository.AddAsync(inventory);
                _logger.LogInformation("Inventory created successfully. InventoryId: {InventoryId}, DetailCount: {DetailCount}, User: {UserId}",
                       createdInventory.Id, inventory.InventoryDetails.Count, _currentUserService.UserId);

                // If files were provided, attach them to related item entities (Ammunition/Explosive)
                if (files != null && files.Any())
                {
                    try
                    {
                        // Determine unique item IDs and their types
                        var detailItemIds = inventory.InventoryDetails.Select(d => d.ItemId).Distinct().ToList();
                        foreach (var itemId in detailItemIds)
                        {
                            // Resolve item type by probing weapons/ammunitions/explosives tables; default to Ammunition if not explosive
                            var itemType = _context.BaseItems.Where(e => e.Id == itemId && !e.IsDeleted).Select(e => e.ItemType)
                                                             .FirstOrDefault();

                            var entityType = itemType == ItemType.Explosive? FileEntityType.Explosive : FileEntityType.Ammunition;
                            var uploadResult = await _fileUploadService.UploadFilesForEntityAsync(files, entityType, createdInventory.Id);
                            if (!uploadResult.Succeeded)
                            {
                                _logger.LogWarning("File upload failed for Inventory create. ItemId: {ItemId}, Error: {Error}", createdInventory.Id, uploadResult.Message);
                            }
                        }
                    }
                    catch (Exception exUpload)
                    {
                        _logger.LogWarning(exUpload, "Error uploading files for Inventory create. InventoryId: {InventoryId}", createdInventory.Id);
                    }
                }

                return APIOperationResponse<long>.Success(createdInventory.Id, "Inventory created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating inventory. DepoId: {DepoId}, User: {UserId}",
                    inputDto?.DepoId, _currentUserService.UserId);

                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, UpdateInventoryDto inputDto, List<IFormFile>? files = null, long? filesItemId = null)
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
                existingInventory.ModificationDate = _dateTimeProvider.Now;
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
                        var existingDetail = existingInventory.InventoryDetails.FirstOrDefault(d => d.Id == detailDto.Id.Value);
                        if (existingDetail != null)
                        {
                            // Update the existing detail
                            _mapper.Map(detailDto, existingDetail);
                            existingDetail.Lot = (detailDto.Lot ?? string.Empty).Trim();
                            // Update in repository - use the already-mapped entity
                            await _inventoryDetailRepository.UpdateAsync(existingDetail);
                        }
                    }
                    else
                    {
                        // Add new detail
                        var newDetail = _mapper.Map<InventoryDetailEntity>(detailDto);
                        newDetail.Lot = (detailDto.Lot ?? string.Empty).Trim();
                        newDetail.InventoryId = id;
                        existingInventory.InventoryDetails.Add(newDetail);
                    }
                }

                // Update the parent inventory entity - use the already-mapped entity
                await _inventoryRepository.UpdateAsync(existingInventory);

                // Handle file uploads after successful update
                if (files != null && files.Any() && filesItemId.HasValue)
                {
                    try
                    {
                        var targetDetail = existingInventory.InventoryDetails.FirstOrDefault(d => d.ItemId == filesItemId.Value);
                        if (targetDetail != null)
                        {
                            // Resolve item type by probing weapons/ammunitions/explosives tables; default to Ammunition if not explosive
                            var itemType = _context.BaseItems.Where(e => e.Id == targetDetail.ItemId && !e.IsDeleted).Select(e => e.ItemType)
                                                             .FirstOrDefault();

                            var entityType = itemType == ItemType.Explosive ? FileEntityType.Explosive : FileEntityType.Ammunition;

                            // Attach files to the specific itemId (Ammunition/Explosive)
                            await _fileUploadService.UploadFilesForEntityAsync(files, entityType, targetDetail.InventoryId);
                        }
                    }
                    catch (Exception exUpload)
                    {
                        _logger.LogWarning(exUpload, "Error uploading files for Inventory update. InventoryId: {InventoryId}", id);
                    }
                }

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

                // Delete any files linked to this inventory for Ammunition and Explosive entity types
                try
                {
                    var entityTypes = new[] { FileEntityType.Ammunition, FileEntityType.Explosive };
                    foreach (var entityType in entityTypes)
                    {
                        var filesResp = await _fileUploadService.GetByEntityAsync(entityType, id);
                        if (filesResp.Succeeded && filesResp.Data != null && filesResp.Data.Any())
                        {
                            foreach (var file in filesResp.Data)
                            {
                                try
                                {
                                    await _fileUploadService.DeleteAsync(file.Id);
                                }
                                catch (Exception exDel)
                                {
                                    _logger.LogWarning(exDel, "Failed deleting file {FileId} for Inventory {InventoryId}", file.Id, id);
                                }
                            }
                        }
                    }
                }
                catch (Exception exFiles)
                {
                    _logger.LogWarning(exFiles, "Error while cleaning up files for Inventory delete. InventoryId: {InventoryId}", id);
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

                // Get all existing SUBMITTED supplies for this order (excluding deleted and drafts)
                // Draft supplies should not be counted because user can still modify them
                var existingSuppliesForOrder = await _supplyRepository.FindAsync(
                    s => s.OrderId == orderId && !s.IsDeleted && s.SubmissionStatus == SupplySubmissionStatus.Submitted,
                    false,
                    nameof(Supply.SupplyDetails)
                );

                // Get all supply details for these SUBMITTED supplies only
                var supplyIds = existingSuppliesForOrder.Select(s => s.Id).ToList();
                var existingSupplyDetails = supplyIds.Any()
                    ? await _supplyDetailsRepository.FindAsync(
                        sd => supplyIds.Contains(sd.SupplyId) && !sd.IsDeleted)
                    : new List<SupplyDetail>();

                // Calculate already supplied quantities per item (only from SUBMITTED supplies)
                var suppliedQuantitiesByItem = existingSupplyDetails
                    .GroupBy(sd => sd.ItemId)
                    .ToDictionary(g => g.Key, g => g.Sum(sd => sd.Quantity));

                _logger.LogInformation("Found existing SUBMITTED supplies for order (drafts excluded). OrderId: {OrderId}, SuppliedItemsCount: {Count}",
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

                        // Use AutoMapper to map from LotDetailDto to SupplyLotSuggestionDto
                        var lotSuggestion = _mapper.Map<SupplyLotSuggestionDto>(lotDetail);
                        lotSuggestion.SuggestedQuantity = quantityToAllocate; // Set calculated value
                        itemSuggestion.LotSuggestions.Add(lotSuggestion);

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

                // Restrict to the user's assigned depots (null = unrestricted)
                var userDepotIds = await _depotAccessService.GetUserAccessibleDepotIdsAsync();
                if (userDepotIds != null)
                {
                    lots = lots.Where(l => userDepotIds.Contains(l.Inventory.DepoId)).ToList();
                }

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
                    sd => sd.ItemId == itemId && !sd.IsDeleted
                );

                // Get all supplies to check submission status
                var supplyIds = allSupplyDetails.Select(sd => sd.SupplyId).Distinct().ToList();
                var supplies = supplyIds.Any()
                    ? await _supplyRepository.FindAsync(s => supplyIds.Contains(s.Id) && !s.IsDeleted)
                    : new List<Supply>();

                var supplyStatusMap = supplies.ToDictionary(s => s.Id, s => s.SubmissionStatus);

                // Separate supply details by submission status and group by lot
                var usedQuantityByLot = allSupplyDetails
                    .Where(sd => supplyStatusMap.ContainsKey(sd.SupplyId) &&
                                 supplyStatusMap[sd.SupplyId] == SupplySubmissionStatus.Submitted)
                    .GroupBy(sd => sd.Lot)
                    .ToDictionary(g => g.Key, g => g.Sum(sd => sd.Quantity));

                var reservedQuantityByLot = allSupplyDetails
                    .Where(sd => supplyStatusMap.ContainsKey(sd.SupplyId) &&
                                 supplyStatusMap[sd.SupplyId] == SupplySubmissionStatus.Draft)
                    .GroupBy(sd => sd.Lot)
                    .ToDictionary(g => g.Key, g => g.Sum(sd => sd.Quantity));

                _logger.LogInformation("Calculated usage for {LotCount} lots. ItemId: {ItemId}",
                    usedQuantityByLot.Count + reservedQuantityByLot.Count, itemId);

                var lotDetails = new List<LotDetailDto>();

                foreach (var lot in lots)
                {
                    // Get used and reserved quantities from the grouped data (O(1) lookup)
                    long usedQuantity = usedQuantityByLot.TryGetValue(lot.Lot, out var usedQty) ? usedQty : 0;
                    long reservedQuantity = reservedQuantityByLot.TryGetValue(lot.Lot, out var reservedQty) ? reservedQty : 0;
                    long remainingQuantity = lot.ItemQuantity - usedQuantity - reservedQuantity;

                    // A lot is empty if remaining quantity is 0 or less
                    bool isEmptyLot = remainingQuantity <= 0;

                    // Check if the lot is expired (expiry date is in the past)
                    bool isExpired = lot.ExpiryDate.HasValue && lot.ExpiryDate.Value.Date < _dateTimeProvider.Now.Date;

                    // Use AutoMapper to create the base mapping
                    var lotDetail = _mapper.Map<LotDetailDto>(lot);

                    // Set calculated properties
                    lotDetail.UsedQuantity = usedQuantity;
                    lotDetail.ReservedQuantityByOrdersOnProcessing = reservedQuantity;
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

        public async Task<APIOperationResponse<List<LotDetailDto>>> GetAvailableLotsForQuantityAsync(long itemId, long requiredQuantity, List<long>? depotIds = null, long? excludeSupplyId = null)
        {
            _logger.LogInformation("Getting available lots for quantity. ItemId: {ItemId}, RequiredQuantity: {RequiredQuantity}, DepotIds: {DepotIds}, ExcludeSupplyId: {ExcludeSupplyId}, User: {UserId}",
                itemId, requiredQuantity, depotIds != null ? string.Join(", ", depotIds) : "All", excludeSupplyId?.ToString() ?? "None", _currentUserService.UserId);

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

                // Enforce depot access: merge any caller-supplied depotIds with the user's assigned depots
                var userDepotIds = await _depotAccessService.GetUserAccessibleDepotIdsAsync();
                if (userDepotIds != null)
                {
                    depotIds = (depotIds != null && depotIds.Any())
                        ? depotIds.Intersect(userDepotIds).ToList()
                        : userDepotIds;

                    if (!depotIds.Any())
                    {
                        _logger.LogInformation("User {UserId} has no accessible depots for item {ItemId}. Returning empty.",
                            _currentUserService.UserId, itemId);
                        return APIOperationResponse<List<LotDetailDto>>.Success(new List<LotDetailDto>());
                    }
                }

                // Filter by depot IDs if provided (or derived from user access)
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

                // Get ALL supply details for this item in ONE query (don't exclude at query level)
                var allSupplyDetails = await _supplyDetailsRepository.FindAsync(
                    sd => sd.ItemId == itemId && !sd.IsDeleted
                );

                // Get all supplies to check submission status
                // If excludeSupplyId is provided, we need to fetch it separately to check if it's Draft
                var supplyIds = allSupplyDetails.Select(sd => sd.SupplyId).Distinct().ToList();

                // Check if excluded supply is Draft (only exclude Draft supplies, not Submitted)
                // Business Rule: You can only replace Draft supplies, so we only exclude Draft supplies
                bool shouldExcludeDraftSupply = false;
                if (excludeSupplyId.HasValue)
                {
                    var excludedSupply = await _supplyRepository.FindOneAsync(
                        s => s.Id == excludeSupplyId.Value && !s.IsDeleted
                    );
                    // Only exclude if the supply is Draft (you can only replace Draft supplies)
                    shouldExcludeDraftSupply = excludedSupply != null && excludedSupply.SubmissionStatus == SupplySubmissionStatus.Draft;

                    if (shouldExcludeDraftSupply)
                    {
                        _logger.LogInformation("Excluding Draft supply from availability calculations. SupplyId: {SupplyId}, ItemId: {ItemId}",
                            excludeSupplyId.Value, itemId);
                    }

                    // Add to supplyIds list if not already present (for status map)
                    if (!supplyIds.Contains(excludeSupplyId.Value))
                    {
                        supplyIds.Add(excludeSupplyId.Value);
                    }
                }

                var supplies = supplyIds.Any()
                    ? await _supplyRepository.FindAsync(s => supplyIds.Contains(s.Id) && !s.IsDeleted)
                    : new List<Supply>();

                var supplyStatusMap = supplies.ToDictionary(s => s.Id, s => s.SubmissionStatus);

                // Separate supply details by submission status and group by lot
                // Used quantities: Always count ALL Submitted supplies (never exclude - they're finalized)
                var usedQuantityByLot = allSupplyDetails
                    .Where(sd => supplyStatusMap.ContainsKey(sd.SupplyId) &&
                                 supplyStatusMap[sd.SupplyId] == SupplySubmissionStatus.Submitted)
                    .GroupBy(sd => sd.Lot)
                    .ToDictionary(g => g.Key, g => g.Sum(sd => sd.Quantity));

                // Reserved quantities: Only exclude Draft supplies if excludeSupplyId is provided and it's Draft
                var reservedQuantityByLot = allSupplyDetails
                    .Where(sd => supplyStatusMap.ContainsKey(sd.SupplyId) &&
                                 supplyStatusMap[sd.SupplyId] == SupplySubmissionStatus.Draft &&
                                 (!shouldExcludeDraftSupply || sd.SupplyId != excludeSupplyId.Value))
                    .GroupBy(sd => sd.Lot)
                    .ToDictionary(g => g.Key, g => g.Sum(sd => sd.Quantity));

                _logger.LogInformation("Calculated usage for {LotCount} lots. ItemId: {ItemId}",
                    usedQuantityByLot.Count + reservedQuantityByLot.Count, itemId);

                // Filter and sort available lots (not expired, not empty, FEFO order)
                var currentDate = _dateTimeProvider.Now.Date;
                var availableLots = new List<LotDetailDto>();
                long remainingQuantityNeeded = requiredQuantity;

                // Sort lots by FEFO: items with expiry dates first (sorted by date), then items without expiry dates
                var sortedLots = lots
                    .OrderBy(l => l.ExpiryDate.HasValue ? 0 : 1)  // Non-null expiry dates first
                    .ThenBy(l => l.ExpiryDate)                    // Then sort by expiry date
                    .ThenBy(l => l.Lot)                                // Then by lot number
                    .ToList();

                foreach (var lot in sortedLots)
                {
                    // Skip expired lots
                    bool isExpired = lot.ExpiryDate.HasValue && lot.ExpiryDate.Value.Date < currentDate;
                    if (isExpired)
                    {
                        _logger.LogDebug("Skipping expired lot. Lot: {Lot}, ExpiryDate: {ExpiryDate}, ItemId: {ItemId}",
                            lot.Lot, lot.ExpiryDate, itemId);
                        continue;
                    }

                    // Calculate used, reserved, and remaining quantities
                    long usedQuantity = usedQuantityByLot.TryGetValue(lot.Lot, out var usedQty) ? usedQty : 0;
                    long reservedQuantity = reservedQuantityByLot.TryGetValue(lot.Lot, out var reservedQty) ? reservedQty : 0;
                    long remainingQuantity = lot.ItemQuantity - usedQuantity - reservedQuantity;

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
                    lotDetail.UsedQuantity = usedQuantity;
                    lotDetail.ReservedQuantityByOrdersOnProcessing = reservedQuantity;
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

        public async Task<APIOperationResponse<LotDetailDto>> GetLotByNumberAsync(string lotNumber)
        {
            var lotKey = lotNumber?.Trim() ?? string.Empty;
            _logger.LogInformation("Getting lot details by lot number. Lot: {Lot}, User: {UserId}",
                lotKey, _currentUserService.UserId);

            try
            {
                if (string.IsNullOrWhiteSpace(lotKey))
                {
                    return APIOperationResponse<LotDetailDto>.Fail(ResponseType.BadRequest, "Lot is required");
                }

                var inventoryDetail = await _inventoryDetailRepository.FindOneAsync(
                    id => id.Lot == lotKey,
                    false,
                    nameof(InventoryDetailEntity.Inventory),
                    $"{nameof(InventoryDetailEntity.Inventory)}.{nameof(InventoryEntity.Depo)}",
                    nameof(InventoryDetailEntity.Item),
                    nameof(InventoryDetailEntity.Supplier),
                    nameof(InventoryDetailEntity.Manufacturer),
                    nameof(InventoryDetailEntity.Country)
                );

                if (inventoryDetail == null || inventoryDetail.Inventory == null || inventoryDetail.Inventory.IsDeleted)
                {
                    _logger.LogWarning("Lot not found or inventory deleted. Lot: {Lot}, User: {UserId}",
                        lotKey, _currentUserService.UserId);
                    return APIOperationResponse<LotDetailDto>.Fail(ResponseType.NotFound, "Lot not found");
                }

                // Verify the user has access to the depot that holds this lot
                var userDepotIds = await _depotAccessService.GetUserAccessibleDepotIdsAsync();
                if (userDepotIds != null && !userDepotIds.Contains(inventoryDetail.Inventory.DepoId))
                {
                    _logger.LogWarning("User {UserId} does not have access to depot {DepotId} for lot {Lot}",
                        _currentUserService.UserId, inventoryDetail.Inventory.DepoId, lotKey);
                    return APIOperationResponse<LotDetailDto>.Fail(ResponseType.NotFound, "Lot not found");
                }

                // Get all supply details for this lot to calculate usage
                var supplyDetails = await _supplyDetailsRepository.FindAsync(
                    sd => sd.Lot == lotKey && !sd.IsDeleted
                );

                // Get all supplies to check submission status
                var supplyIds = supplyDetails.Select(sd => sd.SupplyId).Distinct().ToList();
                var supplies = supplyIds.Any()
                    ? await _supplyRepository.FindAsync(s => supplyIds.Contains(s.Id) && !s.IsDeleted)
                    : new List<Supply>();

                var supplyStatusMap = supplies.ToDictionary(s => s.Id, s => s.SubmissionStatus);

                // Calculate used and reserved quantities
                long usedQuantity = supplyDetails
                    .Where(sd => supplyStatusMap.ContainsKey(sd.SupplyId) &&
                                 supplyStatusMap[sd.SupplyId] == SupplySubmissionStatus.Submitted)
                    .Sum(sd => sd.Quantity);

                long reservedQuantity = supplyDetails
                    .Where(sd => supplyStatusMap.ContainsKey(sd.SupplyId) &&
                                 supplyStatusMap[sd.SupplyId] == SupplySubmissionStatus.Draft)
                    .Sum(sd => sd.Quantity);

                long remainingQuantity = inventoryDetail.ItemQuantity - usedQuantity - reservedQuantity;

                var lotDetail = _mapper.Map<LotDetailDto>(inventoryDetail);
                lotDetail.UsedQuantity = usedQuantity;
                lotDetail.ReservedQuantityByOrdersOnProcessing = reservedQuantity;
                lotDetail.RemainingQuantity = Math.Max(0, remainingQuantity);
                lotDetail.IsEmptyLot = remainingQuantity <= 0;
                lotDetail.IsExpired = inventoryDetail.ExpiryDate.HasValue == true &&
                                      inventoryDetail.ExpiryDate.Value.Date < _dateTimeProvider.Now.Date;

                return APIOperationResponse<LotDetailDto>.Success(lotDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting lot details by number. Lot: {Lot}, User: {UserId}",
                    lotKey, _currentUserService.UserId);
                return APIOperationResponse<LotDetailDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<PaginatedList<InventoryDetailDto>>> GetInventoryDetailsByDepotIdPaginatedAsync(long depotId, PagedListRequest request)
        {
            _logger.LogInformation("Getting paginated inventory details by depot ID. DepotId: {DepotId}, Page: {Page}, PageSize: {PageSize}, User: {UserId}",
                depotId, request.Page, request.PageSize, _currentUserService.UserId);

            try
            {
                var userId = _currentUserService.UserId;
                if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, depotId))
                {
                    _logger.LogWarning("User {UserId} attempted to access inventory for unauthorized depot {DepotId}", userId, depotId);
                    return APIOperationResponse<PaginatedList<InventoryDetailDto>>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
                }

                var query = _inventoryDetailRepository.Find(
                        x => x.Inventory.DepoId == depotId && !x.Inventory.IsDeleted,
                        false,
                        nameof(InventoryDetailEntity.Item),
                        nameof(InventoryDetailEntity.Supplier),
                        nameof(InventoryDetailEntity.Manufacturer),
                        nameof(InventoryDetailEntity.Country),
                        nameof(InventoryDetailEntity.Inventory)
                    )
                    // Newest inventory entries first so recently added ammunition/explosives/weapon lots appear at the top
                    .OrderByDescending(x => x.Inventory.CreationDate)
                    .ThenByDescending(x => x.Id);

                // Create paginated list of entities first to apply filtering and paging on database
                var paginatedEntities = await PaginatedList<InventoryDetailEntity>.CreateAsyncForTableBinding(query, request);

                // Map entities to DTOs
                var dtos = _mapper.Map<List<InventoryDetailDto>>(paginatedEntities.Items);

                // Create paginated list of DTOs
                var result = new PaginatedList<InventoryDetailDto>(
                    dtos,
                    paginatedEntities.TotalCount,
                    paginatedEntities.PageIndex,
                    request.PageSize
                );

                return APIOperationResponse<PaginatedList<InventoryDetailDto>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting paginated inventory details. DepotId: {DepotId}, User: {UserId}",
                    depotId, _currentUserService.UserId);
                return APIOperationResponse<PaginatedList<InventoryDetailDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<ItemInventorySummaryDto>>> GetInventorySummaryForAllItemsAsync()
        {
            _logger.LogInformation("Getting inventory summary for all items. User: {UserId}", _currentUserService.UserId);

            try
            {
                // 1. Get all valid inventory details
                var inventoryDetails = await _inventoryDetailRepository.FindAsync(
                    id => id.ItemQuantity > 0, // We only care about lots that were created with quantity
                    false,
                    nameof(InventoryDetailEntity.Inventory),
                    nameof(InventoryDetailEntity.Item)
                );

                // Filter out deleted inventory parent records
                var activeDetails = inventoryDetails
                    .Where(id => !id.Inventory.IsDeleted)
                    .ToList();

                if (!activeDetails.Any())
                {
                    return APIOperationResponse<List<ItemInventorySummaryDto>>.Success(new List<ItemInventorySummaryDto>());
                }

                // 2. Get all supply details (to calculate usage)
                var allSupplyDetails = await _supplyDetailsRepository.FindAsync(
                    sd => !sd.IsDeleted
                );

                // 3. Get all supplies to check submission status
                var supplyIds = allSupplyDetails.Select(sd => sd.SupplyId).Distinct().ToList();
                var supplies = supplyIds.Any()
                    ? await _supplyRepository.FindAsync(s => supplyIds.Contains(s.Id) && !s.IsDeleted)
                    : new List<Supply>();

                var supplyStatusMap = supplies.ToDictionary(s => s.Id, s => s.SubmissionStatus);

                // 4. Group Supply Details by ItemId
                var supplyDetailsByItem = allSupplyDetails
                    .GroupBy(sd => sd.ItemId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                // 5. Group Inventory Details by ItemId
                var inventoryDetailsByItem = activeDetails
                    .GroupBy(id => id.ItemId)
                    .ToList();

                var result = new List<ItemInventorySummaryDto>();

                foreach (var itemGroup in inventoryDetailsByItem)
                {
                    long itemId = itemGroup.Key;
                    var lots = itemGroup.ToList();
                    var firstLotItem = lots.FirstOrDefault()?.Item;
                    var itemName = firstLotItem?.Name ?? "Unknown Item";
                    var itemNo = firstLotItem?.ItemNo ?? string.Empty;
                    var itemType = firstLotItem?.ItemType ?? default(ItemType);
                    var nsn = firstLotItem?.Nsn ?? string.Empty;
                    var partNo = firstLotItem?.PartNo ?? string.Empty;

                    // Calculate total entered quantity (sum of Original Quantities in lots)
                    long totalQuantity = lots.Sum(l => l.ItemQuantity);

                    // Get supplies for this item
                    var itemSupplyDetails = supplyDetailsByItem.TryGetValue(itemId, out var supplyList)
                        ? supplyList
                        : new List<SupplyDetail>();

                    // Calculate used quantity (Submitted supplies)
                    long usedQuantity = itemSupplyDetails
                        .Where(sd => supplyStatusMap.ContainsKey(sd.SupplyId) &&
                                     supplyStatusMap[sd.SupplyId] == SupplySubmissionStatus.Submitted)
                        .Sum(sd => sd.Quantity);

                    // Calculate reserved quantity (Draft supplies)
                    long reservedQuantity = itemSupplyDetails
                        .Where(sd => supplyStatusMap.ContainsKey(sd.SupplyId) &&
                                     supplyStatusMap[sd.SupplyId] == SupplySubmissionStatus.Draft)
                        .Sum(sd => sd.Quantity);

                    // Calculate remaining quantity
                    long remainingQuantity = totalQuantity - usedQuantity - reservedQuantity;

                    result.Add(new ItemInventorySummaryDto
                    {
                        ItemId = itemId,
                        ItemName = itemName,
                        ItemNo = itemNo,
                        ItemType = itemType,
                        Nsn = nsn,
                        PartNo = partNo,
                        TotalQuantity = totalQuantity,
                        UsedQuantity = usedQuantity,
                        ReservedQuantityByOrdersOnProcessing = reservedQuantity,
                        RemainingQuantity = Math.Max(0, remainingQuantity),
                        TotalLots = lots.Count
                    });
                }

                _logger.LogInformation("Inventory summary calculated for {ItemCount} items. User: {UserId}",
                    result.Count, _currentUserService.UserId);

                return APIOperationResponse<List<ItemInventorySummaryDto>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting inventory summary for all items. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<ItemInventorySummaryDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<ItemInventorySummaryDto>> GetItemInventorySummaryAsync(long itemId)
        {
            _logger.LogInformation("Getting item inventory summary. ItemId: {ItemId}, User: {UserId}",
                itemId, _currentUserService.UserId);

            try
            {
                // Get all inventory details for this item
                var inventoryDetails = await _inventoryDetailRepository.FindAsync(
                    id => id.ItemId == itemId && id.ItemQuantity > 0,
                    false,
                    nameof(InventoryDetailEntity.Inventory),
                    nameof(InventoryDetailEntity.Item)
                );

                var lots = inventoryDetails
                    .Where(id => !id.Inventory.IsDeleted)
                    .ToList();

                // Get item properties from first lot
                var firstLotItem = lots.FirstOrDefault()?.Item;
                var itemName = firstLotItem?.Name ?? "Unknown Item";
                var itemNo = firstLotItem?.ItemNo ?? string.Empty;
                var itemType = firstLotItem?.ItemType ?? default(ItemType);
                var nsn = firstLotItem?.Nsn ?? string.Empty;
                var partNo = firstLotItem?.PartNo ?? string.Empty;

                if (lots.Count == 0)
                {
                    _logger.LogInformation("No lots found for item. ItemId: {ItemId}, User: {UserId}",
                        itemId, _currentUserService.UserId);

                    // Return empty summary
                    var emptySummary = new ItemInventorySummaryDto
                    {
                        ItemId = itemId,
                        ItemName = itemName,
                        ItemNo = itemNo,
                        ItemType = itemType,
                        Nsn = nsn,
                        PartNo = partNo,
                        TotalQuantity = 0,
                        UsedQuantity = 0,
                        ReservedQuantityByOrdersOnProcessing = 0,
                        RemainingQuantity = 0,
                        TotalLots = 0
                    };

                    return APIOperationResponse<ItemInventorySummaryDto>.Success(emptySummary);
                }

                // Calculate total quantity across all lots
                long totalQuantity = lots.Sum(l => l.ItemQuantity);

                // Get ALL supply details for this item
                var allSupplyDetails = await _supplyDetailsRepository.FindAsync(
                    sd => sd.ItemId == itemId && !sd.IsDeleted
                );

                // Get all supplies to check submission status
                var supplyIds = allSupplyDetails.Select(sd => sd.SupplyId).Distinct().ToList();
                var supplies = supplyIds.Any()
                    ? await _supplyRepository.FindAsync(s => supplyIds.Contains(s.Id) && !s.IsDeleted)
                    : new List<Supply>();

                var supplyStatusMap = supplies.ToDictionary(s => s.Id, s => s.SubmissionStatus);

                // Calculate used and reserved quantities across all lots
                long usedQuantity = allSupplyDetails
                    .Where(sd => supplyStatusMap.ContainsKey(sd.SupplyId) &&
                                 supplyStatusMap[sd.SupplyId] == SupplySubmissionStatus.Submitted)
                    .Sum(sd => sd.Quantity);

                long reservedQuantity = allSupplyDetails
                    .Where(sd => supplyStatusMap.ContainsKey(sd.SupplyId) &&
                                 supplyStatusMap[sd.SupplyId] == SupplySubmissionStatus.Draft)
                    .Sum(sd => sd.Quantity);

                long remainingQuantity = totalQuantity - usedQuantity - reservedQuantity;

                var summary = new ItemInventorySummaryDto
                {
                    ItemId = itemId,
                    ItemName = itemName,
                    ItemNo = itemNo,
                    ItemType = itemType,
                    Nsn = nsn,
                    PartNo = partNo,
                    TotalQuantity = totalQuantity,
                    UsedQuantity = usedQuantity,
                    ReservedQuantityByOrdersOnProcessing = reservedQuantity,
                    RemainingQuantity = Math.Max(0, remainingQuantity),
                    TotalLots = lots.Count
                };

                _logger.LogInformation("Item inventory summary calculated. ItemId: {ItemId}, Total: {Total}, Used: {Used}, Reserved: {Reserved}, Remaining: {Remaining}, Lots: {Lots}, User: {UserId}",
                    itemId, totalQuantity, usedQuantity, reservedQuantity, remainingQuantity, lots.Count, _currentUserService.UserId);

                return APIOperationResponse<ItemInventorySummaryDto>.Success(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting item inventory summary. ItemId: {ItemId}, User: {UserId}",
                    itemId, _currentUserService.UserId);
                return APIOperationResponse<ItemInventorySummaryDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private async Task PopulateInventoryDetailsQuantitiesAsync(List<InventoryDetailDto> details)
        {
            if (details == null || !details.Any()) return;

            // Get unique item IDs and lots for batch processing
            var itemLotPairs = details
                .Select(d => new { d.ItemId, d.Lot })
                .Distinct()
                .ToList();

            var itemIds = itemLotPairs.Select(p => p.ItemId).Distinct().ToList();
            var lotNumbers = itemLotPairs.Select(p => p.Lot).Distinct().ToList();

            // Fetch all supply details for these items and lots in ONE query
            var supplyDetails = await _supplyDetailsRepository.FindAsync(
                sd => itemIds.Contains(sd.ItemId) && lotNumbers.Contains(sd.Lot) && !sd.IsDeleted
            );

            // Fetch submission status for the associated supplies
            var uniqueSupplyIds = supplyDetails.Select(sd => sd.SupplyId).Distinct().ToList();
            var supplies = uniqueSupplyIds.Any()
                ? await _supplyRepository.FindAsync(s => uniqueSupplyIds.Contains(s.Id) && !s.IsDeleted)
                : new List<Supply>();

            var supplyStatusMap = supplies.ToDictionary(s => s.Id, s => s.SubmissionStatus);

            // Group supply details by ItemId and Lot for fast O(1) lookup
            var usedQuantitiesByItemLot = supplyDetails
                .Where(sd => supplyStatusMap.ContainsKey(sd.SupplyId) &&
                             supplyStatusMap[sd.SupplyId] == SupplySubmissionStatus.Submitted)
                .GroupBy(sd => new { sd.ItemId, sd.Lot })
                .ToDictionary(g => g.Key, g => g.Sum(sd => sd.Quantity));

            var reservedQuantitiesByItemLot = supplyDetails
                .Where(sd => supplyStatusMap.ContainsKey(sd.SupplyId) &&
                             supplyStatusMap[sd.SupplyId] == SupplySubmissionStatus.Draft)
                .GroupBy(sd => new { sd.ItemId, sd.Lot })
                .ToDictionary(g => g.Key, g => g.Sum(sd => sd.Quantity));

            // Update DTO objects with calculated values
            foreach (var detail in details)
            {
                var key = new { detail.ItemId, detail.Lot };

                long used = usedQuantitiesByItemLot.TryGetValue(key, out var usedQty) ? usedQty : 0;
                long reserved = reservedQuantitiesByItemLot.TryGetValue(key, out var reservedQty) ? reservedQty : 0;

                detail.UsedQuantity = used;
                detail.ReservedQuantityByOrdersOnProcessing = reserved;
                detail.RemainingQuantity = Math.Max(0, detail.OriginalQuantity - used - reserved);
                detail.CurrentQuantity = Math.Max(0, detail.OriginalQuantity - used);
                detail.IsLotEmpty = detail.RemainingQuantity <= 0;
            }
        }

        public async Task<APIOperationResponse<bool>> ToggleReadyForIssueAsync(long inventoryDetailId)
        {
            _logger.LogInformation("Toggling ReadyForIssue status. InventoryDetailId: {InventoryDetailId}, User: {UserId}",
                inventoryDetailId, _currentUserService.UserId);

            try
            {
                var inventoryDetail = await _inventoryDetailRepository.FindOneAsync(id => id.Id == inventoryDetailId);

                if (inventoryDetail == null)
                {
                    _logger.LogWarning("Inventory detail not found. InventoryDetailId: {InventoryDetailId}, User: {UserId}",
                        inventoryDetailId, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Inventory detail not found");
                }

                inventoryDetail.ReadyForIssue = !inventoryDetail.ReadyForIssue;
                await _inventoryDetailRepository.UpdateAsync(inventoryDetail);

                _logger.LogInformation("ReadyForIssue status toggled successfully. InventoryDetailId: {InventoryDetailId}, NewStatus: {NewStatus}, User: {UserId}",
                    inventoryDetailId, inventoryDetail.ReadyForIssue, _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(inventoryDetail.ReadyForIssue, "Status updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling ReadyForIssue status. InventoryDetailId: {InventoryDetailId}, User: {UserId}",
                    inventoryDetailId, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<ImportResult<InventoryImportRowDto>>> ImportAsync(IFormFile file, long depotId, string language = "en")
        {
            _logger.LogInformation("Starting inventory import. DepotId: {DepotId}, Language: {Language}, User: {UserId}",
                depotId, language, _currentUserService.UserId);

            var userId = _currentUserService.UserId;
            if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, depotId))
            {
                _logger.LogWarning("User {UserId} attempted to import inventory to unauthorized depot {DepotId}", userId, depotId);
                return APIOperationResponse<ImportResult<InventoryImportRowDto>>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
            }

            try
            {
                // Parse Excel file
                var mappings = GetColumnMappings(language);
                var importResult = await _excelImportService.ImportFromExcelAsync<InventoryImportRowDto>(file, mappings);

                if (importResult.SuccessCount == 0)
                {
                    _logger.LogWarning("No valid rows found in Excel file. DepotId: {DepotId}, User: {UserId}",
                        depotId, _currentUserService.UserId);
                    return APIOperationResponse<ImportResult<InventoryImportRowDto>>.Success(importResult, "Import processed with no valid records");
                }

                // Load all items for ItemNo lookup
                var allItems = await LoadAllItemsAsync();

                // Load lookup tables for name-to-ID resolution
                var suppliers = await _context.Suppliers.Where(s => !s.IsDeleted).ToListAsync();
                var manufacturers = await _context.Manufacturers.Where(m => !m.IsDeleted).ToListAsync();
                var countries = await _context.Countries.Where(c => !c.IsDeleted).ToListAsync();

                // Load existing inventory for duplicate checking
                var existingInventoryDetails = await _inventoryDetailRepository.FindAsync(
                    id => id.Inventory.DepoId == depotId && !id.Inventory.IsDeleted,
                    false,
                    nameof(InventoryDetailEntity.Inventory)
                );

                var existingKeys = new HashSet<string>();
                foreach (var detail in existingInventoryDetails)
                {
                    var key = $"{detail.ItemId}_{detail.Lot}_{detail.BatchNo ?? ""}";
                    existingKeys.Add(key);
                }

                // Process valid records with transaction support
                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    // Group rows by invoice number
                    var invoiceGroups = importResult.SuccessfulRecords
                        .GroupBy(r => r.InvoiceNumber ?? $"IMPORT-{Guid.NewGuid()}")
                        .ToList();

                    int processedCount = 0;
                    int errorCount = 0;

                    foreach (var invoiceGroup in invoiceGroups)
                    {
                        var invoiceNumber = invoiceGroup.Key;
                        var rows = invoiceGroup.ToList();

                        try
                        {
                            // Validate all rows in invoice first (per-invoice atomicity)
                            var invoiceErrors = new List<string>();
                            var inventoryDetails = new List<CreateInventoryDetailDto>();
                            DateTime? invoiceDate = null;
                            DateTime? receivedDate = null;
                            string? notes = null;

                            foreach (var row in rows)
                            {
                                // Extract ItemNo from ItemName if provided (format: "ItemName (ItemNo)")
                                if (string.IsNullOrEmpty(row.ItemNo) && !string.IsNullOrEmpty(row.ItemName))
                                {
                                    // Parse "ItemName (ItemNo)" format to extract ItemNo
                                    var itemNameValue = row.ItemName.Trim();
                                    if (itemNameValue.Contains("(") && itemNameValue.Contains(")"))
                                    {
                                        var startIndex = itemNameValue.LastIndexOf("(");
                                        var endIndex = itemNameValue.LastIndexOf(")");
                                        if (startIndex > 0 && endIndex > startIndex)
                                        {
                                            row.ItemNo = itemNameValue.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                                        }
                                    }
                                    else
                                    {
                                        // If no parentheses, try to match by item name
                                        var foundItem = allItems.FirstOrDefault(i =>
                                            i.Name != null && i.Name.Equals(itemNameValue, StringComparison.OrdinalIgnoreCase));
                                        if (foundItem != null)
                                        {
                                            row.ItemNo = foundItem.ItemNo ?? "";
                                        }
                                    }
                                }

                                // Resolve ItemId from ItemNo
                                long? itemId = row.ItemId;
                                if (!itemId.HasValue && !string.IsNullOrEmpty(row.ItemNo))
                                {
                                    var foundItem = allItems.FirstOrDefault(i => i.ItemNo == row.ItemNo);
                                    if (foundItem != null)
                                    {
                                        itemId = foundItem.Id;
                                    }
                                }

                                if (!itemId.HasValue)
                                {
                                    invoiceErrors.Add($"Item not found: ItemName={row.ItemName}, ItemNo={row.ItemNo}, ItemId={row.ItemId}");
                                    continue;
                                }

                                // Resolve Supplier name to ID
                                if (!string.IsNullOrWhiteSpace(row.Supplier) && !row.SupplierId.HasValue)
                                {
                                    var supplier = suppliers.FirstOrDefault(s =>
                                        s.NameEn.Equals(row.Supplier, StringComparison.OrdinalIgnoreCase) ||
                                        s.NameAr.Equals(row.Supplier, StringComparison.OrdinalIgnoreCase));

                                    if (supplier != null)
                                    {
                                        row.SupplierId = supplier.Id;
                                    }
                                    else
                                    {
                                        invoiceErrors.Add($"Supplier not found: {row.Supplier}");
                                        continue;
                                    }
                                }

                                // Resolve Manufacturer name to ID
                                if (!string.IsNullOrWhiteSpace(row.Manufacturer) && !row.ManufacturerId.HasValue)
                                {
                                    var manufacturer = manufacturers.FirstOrDefault(m =>
                                        m.NameEn.Equals(row.Manufacturer, StringComparison.OrdinalIgnoreCase) ||
                                        m.NameAr.Equals(row.Manufacturer, StringComparison.OrdinalIgnoreCase));

                                    if (manufacturer != null)
                                    {
                                        row.ManufacturerId = manufacturer.Id;
                                    }
                                    else
                                    {
                                        invoiceErrors.Add($"Manufacturer not found: {row.Manufacturer}");
                                        continue;
                                    }
                                }

                                // Resolve Country name to ID
                                if (!string.IsNullOrWhiteSpace(row.Country) && !row.CountryId.HasValue)
                                {
                                    var country = countries.FirstOrDefault(c =>
                                        c.NameEn.Equals(row.Country, StringComparison.OrdinalIgnoreCase) ||
                                        c.NameAr.Equals(row.Country, StringComparison.OrdinalIgnoreCase));

                                    if (country != null)
                                    {
                                        row.CountryId = country.Id;
                                    }
                                    else
                                    {
                                        invoiceErrors.Add($"Country not found: {row.Country}");
                                        continue;
                                    }
                                }

                                // Check for duplicates
                                var duplicateKey = $"{itemId.Value}_{row.Lot}_{row.BatchNo ?? ""}";
                                if (existingKeys.Contains(duplicateKey))
                                {
                                    invoiceErrors.Add($"Duplicate entry: ItemId={itemId.Value}, Lot={row.Lot}, BatchNo={row.BatchNo ?? "N/A"}");
                                    continue;
                                }

                                if (row.OriginalQuantity <= 0)
                                {
                                    invoiceErrors.Add($"Original Quantity must be greater than 0: ItemId={itemId.Value}");
                                    continue;
                                }

                                // Add to details
                                inventoryDetails.Add(new CreateInventoryDetailDto
                                {
                                    ItemId = itemId.Value,
                                    Lot = row.Lot,
                                    SupplierId = row.SupplierId,
                                    ManufacturerId = row.ManufacturerId,
                                    CountryId = row.CountryId,
                                    OriginalQuantity = row.OriginalQuantity,
                                    BatchNo = row.BatchNo,
                                    ExpiryDate = row.ExpiryDate,
                                    ReadyForIssue = row.ReadyForIssue
                                });

                                // Add to existing keys to prevent duplicates within import
                                existingKeys.Add(duplicateKey);

                                // Capture invoice-level data from first row
                                if (rows.IndexOf(row) == 0)
                                {
                                    invoiceDate = row.InvoiceDate;
                                    receivedDate = row.ReceivedDate;
                                    notes = row.Notes;
                                }
                            }

                            // If any errors in invoice, skip entire invoice
                            if (invoiceErrors.Any())
                            {
                                foreach (var error in invoiceErrors)
                                {
                                    importResult.Errors.Add(new ImportError
                                    {
                                        RowNumber = 0,
                                        ErrorMessage = $"Invoice {invoiceNumber}: {error}",
                                        ColumnName = "N/A"
                                    });
                                }
                                errorCount += rows.Count;
                                continue;
                            }

                            if (inventoryDetails.Count == 0)
                            {
                                importResult.Errors.Add(new ImportError
                                {
                                    RowNumber = 0,
                                    ErrorMessage = $"Invoice {invoiceNumber}: No valid inventory details",
                                    ColumnName = "N/A"
                                });
                                errorCount += rows.Count;
                                continue;
                            }

                            // Create inventory
                            var createDto = new CreateInventoryDto
                            {
                                DepoId = depotId,
                                InvoiceNumber = invoiceNumber.StartsWith("IMPORT-") ? null : invoiceNumber,
                                InvoiceDate = invoiceDate,
                                RecievedDate = receivedDate,
                                Notes = notes,
                                InventoryDetails = inventoryDetails
                            };

                            // Validate
                            var validationResult = await _createValidator.ValidateAsync(createDto);
                            if (!validationResult.IsValid)
                            {
                                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                                importResult.Errors.Add(new ImportError
                                {
                                    RowNumber = 0,
                                    ErrorMessage = $"Invoice {invoiceNumber}: Validation failed - {errors}",
                                    ColumnName = "N/A"
                                });
                                errorCount += rows.Count;
                                continue;
                            }

                            // Create inventory entity
                            var inventory = _mapper.Map<InventoryEntity>(createDto);
                            inventory.CreationDate = _dateTimeProvider.Now;
                            inventory.CreatedBy = _currentUserService.UserId;
                            inventory.InventoryDetails = inventoryDetails
                                .Select(d => _mapper.Map<InventoryDetailEntity>(d))
                                .ToList();

                            await _inventoryRepository.AddAsync(inventory);
                            processedCount += inventoryDetails.Count;

                            _logger.LogInformation("Created inventory from import. InvoiceNumber: {InvoiceNumber}, DetailCount: {DetailCount}, DepotId: {DepotId}, User: {UserId}",
                                invoiceNumber, inventoryDetails.Count, depotId, _currentUserService.UserId);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error processing invoice group. InvoiceNumber: {InvoiceNumber}, DepotId: {DepotId}, User: {UserId}",
                                invoiceNumber, depotId, _currentUserService.UserId);

                            importResult.Errors.Add(new ImportError
                            {
                                RowNumber = 0,
                                ErrorMessage = $"Invoice {invoiceNumber}: {ex.Message}",
                                ColumnName = "N/A"
                            });
                            errorCount += rows.Count;
                        }
                    }

                    // Commit transaction if all successful
                    if (processedCount > 0)
                    {
                        await transaction.CommitAsync();
                        _logger.LogInformation("Inventory import completed successfully. Processed: {ProcessedCount}, Errors: {ErrorCount}, DepotId: {DepotId}, User: {UserId}",
                            processedCount, errorCount, depotId, _currentUserService.UserId);
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        _logger.LogWarning("Inventory import rolled back - no valid records. DepotId: {DepotId}, User: {UserId}",
                            depotId, _currentUserService.UserId);
                    }

                    // Update import result counts
                    importResult.SuccessfulRecords = importResult.SuccessfulRecords.Take(processedCount).ToList();
                    importResult.TotalProcessed = importResult.SuccessfulRecords.Count + importResult.Errors.Count;

                    return APIOperationResponse<ImportResult<InventoryImportRowDto>>.Success(importResult, "Import processed");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Error during inventory import transaction. DepotId: {DepotId}, User: {UserId}",
                        depotId, _currentUserService.UserId);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing inventory. DepotId: {DepotId}, User: {UserId}",
                    depotId, _currentUserService.UserId);
                return APIOperationResponse<ImportResult<InventoryImportRowDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<ImportResult<InventoryImportRowDto>>> ImportPreviewAsync(IFormFile file, long depotId, string language = "en")
        {
            _logger.LogInformation("Starting inventory import preview. DepotId: {DepotId}, Language: {Language}, User: {UserId}",
                depotId, language, _currentUserService.UserId);

            var userId = _currentUserService.UserId;
            if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, depotId))
            {
                _logger.LogWarning("User {UserId} attempted to preview inventory import for unauthorized depot {DepotId}", userId, depotId);
                return APIOperationResponse<ImportResult<InventoryImportRowDto>>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
            }

            try
            {
                // Parse Excel file
                var mappings = GetColumnMappings(language);
                var importResult = await _excelImportService.ImportFromExcelAsync<InventoryImportRowDto>(file, mappings);

                if (importResult.SuccessCount == 0)
                {
                    _logger.LogWarning("No valid rows found in Excel file for preview. DepotId: {DepotId}, User: {UserId}",
                        depotId, _currentUserService.UserId);
                    return APIOperationResponse<ImportResult<InventoryImportRowDto>>.Success(importResult, "Preview processed with no valid records");
                }

                // Load all items for ItemNo lookup
                var allItems = await LoadAllItemsAsync();

                // Load lookup tables for name-to-ID resolution
                var suppliers = await _context.Suppliers.Where(s => !s.IsDeleted).ToListAsync();
                var manufacturers = await _context.Manufacturers.Where(m => !m.IsDeleted).ToListAsync();
                var countries = await _context.Countries.Where(c => !c.IsDeleted).ToListAsync();

                // Load existing inventory for duplicate checking
                var existingInventoryDetails = await _inventoryDetailRepository.FindAsync(
                    id => id.Inventory.DepoId == depotId && !id.Inventory.IsDeleted,
                    false,
                    nameof(InventoryDetailEntity.Inventory)
                );

                var existingKeys = new HashSet<string>();
                foreach (var detail in existingInventoryDetails)
                {
                    var key = $"{detail.ItemId}_{detail.Lot}_{detail.BatchNo ?? ""}";
                    existingKeys.Add(key);
                }

                // Validate records WITHOUT saving to database
                // Use ToList() to avoid modification during iteration (following asset pattern)
                foreach (var row in importResult.SuccessfulRecords.ToList())
                {
                    var rowErrors = new List<string>();

                    // Extract ItemNo from ItemName if provided (format: "ItemName (ItemNo)")
                    if (string.IsNullOrEmpty(row.ItemNo) && !string.IsNullOrEmpty(row.ItemName))
                    {
                        // Parse "ItemName (ItemNo)" format to extract ItemNo
                        var itemNameValue = row.ItemName.Trim();
                        if (itemNameValue.Contains("(") && itemNameValue.Contains(")"))
                        {
                            var startIndex = itemNameValue.LastIndexOf("(");
                            var endIndex = itemNameValue.LastIndexOf(")");
                            if (startIndex > 0 && endIndex > startIndex)
                            {
                                row.ItemNo = itemNameValue.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                            }
                        }
                        else
                        {
                            // If no parentheses, try to match by item name
                            var foundItem = allItems.FirstOrDefault(i =>
                                i.Name != null && i.Name.Equals(itemNameValue, StringComparison.OrdinalIgnoreCase));
                            if (foundItem != null)
                            {
                                row.ItemNo = foundItem.ItemNo ?? "";
                            }
                        }
                    }

                    // Resolve ItemId from ItemNo
                    long? itemId = row.ItemId;
                    if (!itemId.HasValue && !string.IsNullOrEmpty(row.ItemNo))
                    {
                        var foundItem = allItems.FirstOrDefault(i => i.ItemNo == row.ItemNo);
                        if (foundItem != null)
                        {
                            itemId = foundItem.Id;
                            row.ItemId = itemId; // Update the row
                        }
                    }

                    if (!itemId.HasValue)
                    {
                        rowErrors.Add($"Item not found: ItemName={row.ItemName}, ItemNo={row.ItemNo}, ItemId={row.ItemId}");
                    }
                    else
                    {
                        // Resolve Supplier name to ID
                        if (!string.IsNullOrWhiteSpace(row.Supplier))
                        {
                            var supplier = suppliers.FirstOrDefault(s =>
                                s.NameEn.Equals(row.Supplier, StringComparison.OrdinalIgnoreCase) ||
                                s.NameAr.Equals(row.Supplier, StringComparison.OrdinalIgnoreCase));

                            if (supplier != null)
                            {
                                row.SupplierId = supplier.Id;
                            }
                            else
                            {
                                rowErrors.Add($"Supplier not found: {row.Supplier}");
                            }
                        }

                        // Resolve Manufacturer name to ID
                        if (!string.IsNullOrWhiteSpace(row.Manufacturer))
                        {
                            var manufacturer = manufacturers.FirstOrDefault(m =>
                                m.NameEn.Equals(row.Manufacturer, StringComparison.OrdinalIgnoreCase) ||
                                m.NameAr.Equals(row.Manufacturer, StringComparison.OrdinalIgnoreCase));

                            if (manufacturer != null)
                            {
                                row.ManufacturerId = manufacturer.Id;
                            }
                            else
                            {
                                rowErrors.Add($"Manufacturer not found: {row.Manufacturer}");
                            }
                        }

                        // Resolve Country name to ID
                        if (!string.IsNullOrWhiteSpace(row.Country))
                        {
                            var country = countries.FirstOrDefault(c =>
                                c.NameEn.Equals(row.Country, StringComparison.OrdinalIgnoreCase) ||
                                c.NameAr.Equals(row.Country, StringComparison.OrdinalIgnoreCase));

                            if (country != null)
                            {
                                row.CountryId = country.Id;
                            }
                            else
                            {
                                rowErrors.Add($"Country not found: {row.Country}");
                            }
                        }

                        // Check for duplicates
                        var duplicateKey = $"{itemId.Value}_{row.Lot}_{row.BatchNo ?? ""}";
                        if (existingKeys.Contains(duplicateKey))
                        {
                            rowErrors.Add($"Duplicate entry: ItemId={itemId.Value}, Lot={row.Lot}, BatchNo={row.BatchNo ?? "N/A"}");
                        }
                        else
                        {
                            // Add to existing keys to prevent duplicates within preview
                            existingKeys.Add(duplicateKey);
                        }

                        if (row.OriginalQuantity <= 0)
                        {
                            rowErrors.Add($"Original Quantity must be greater than 0");
                        }

                        // Validate Expiry Date
                        if (row.ExpiryDate.HasValue && row.ExpiryDate.Value <= _dateTimeProvider.Now)
                        {
                            rowErrors.Add($"Expiry date must be in the future");
                        }
                    }

                    // If validation failed, move from successful to errors (following asset pattern)
                    if (rowErrors.Any())
                    {
                        importResult.SuccessfulRecords.Remove(row);
                        importResult.Errors.Add(new ImportError
                        {
                            RowNumber = row.RowNumber > 0 ? row.RowNumber : 0,
                            ErrorMessage = string.Join("; ", rowErrors),
                            ColumnName = "N/A",
                            RowData = row
                        });
                    }
                }

                // Update counts
                importResult.TotalProcessed = importResult.SuccessfulRecords.Count + importResult.Errors.Count;

                _logger.LogInformation("Inventory import preview completed. Valid: {ValidCount}, Errors: {ErrorCount}, DepotId: {DepotId}, User: {UserId}",
                    importResult.SuccessfulRecords.Count, importResult.Errors.Count, depotId, _currentUserService.UserId);

                return APIOperationResponse<ImportResult<InventoryImportRowDto>>.Success(importResult, "Preview processed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error previewing inventory import. DepotId: {DepotId}, User: {UserId}",
                    depotId, _currentUserService.UserId);
                return APIOperationResponse<ImportResult<InventoryImportRowDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        private Dictionary<string, string> GetColumnMappings(string language = "en")
        {
            var mappings = new Dictionary<string, string>
            {
                // English headers
                { "Item Name", nameof(InventoryImportRowDto.ItemName) },
                { "Item No", nameof(InventoryImportRowDto.ItemNo) },
                { "Item ID", nameof(InventoryImportRowDto.ItemId) },
                { "Lot", nameof(InventoryImportRowDto.Lot) },
                { "Supplier", nameof(InventoryImportRowDto.Supplier) },
                { "Manufacturer", nameof(InventoryImportRowDto.Manufacturer) },
                { "Country", nameof(InventoryImportRowDto.Country) },
                { "Original Quantity", nameof(InventoryImportRowDto.OriginalQuantity) },
                { "Batch No", nameof(InventoryImportRowDto.BatchNo) },
                { "Expiry Date", nameof(InventoryImportRowDto.ExpiryDate) },
                { "Ready For Issue", nameof(InventoryImportRowDto.ReadyForIssue) },
                { "Invoice Number", nameof(InventoryImportRowDto.InvoiceNumber) },
                { "Invoice Date", nameof(InventoryImportRowDto.InvoiceDate) },
                { "Received Date", nameof(InventoryImportRowDto.ReceivedDate) },
                { "Notes", nameof(InventoryImportRowDto.Notes) },
 
                // Arabic headers
                { "اسم الصنف", nameof(InventoryImportRowDto.ItemName) },
                { "رقم الصنف", nameof(InventoryImportRowDto.ItemNo) },
                { "رقم التعريف", nameof(InventoryImportRowDto.ItemId) },
                { "الدفعة", nameof(InventoryImportRowDto.Lot) },
                { "المورد", nameof(InventoryImportRowDto.Supplier) },
                { "المصنع", nameof(InventoryImportRowDto.Manufacturer) },
                { "بلد المنشأ", nameof(InventoryImportRowDto.Country) },
                { "الكمية الأصلية", nameof(InventoryImportRowDto.OriginalQuantity) },
                { "رقم التشغيلة", nameof(InventoryImportRowDto.BatchNo) },
                { "تاريخ الانتهاء", nameof(InventoryImportRowDto.ExpiryDate) },
                { "جاهز للصرف", nameof(InventoryImportRowDto.ReadyForIssue) },
                { "رقم الفاتورة", nameof(InventoryImportRowDto.InvoiceNumber) },
                { "تاريخ الفاتورة", nameof(InventoryImportRowDto.InvoiceDate) },
                { "تاريخ الاستلام", nameof(InventoryImportRowDto.ReceivedDate) },
                { "ملاحظات", nameof(InventoryImportRowDto.Notes) }
            };

            return mappings;
        }

        private async Task<List<BaseItem>> LoadAllItemsAsync()
        {
            var items = new List<BaseItem>();

            // Load all ammunition, weapons, and explosives
            var ammunitions = await _context.Ammunitions
                .Where(a => !a.IsDeleted)
                .ToListAsync();

            var weapons = await _context.Weapons
                .Where(w => !w.IsDeleted)
                .ToListAsync();

            var explosives = await _context.Explosives
                .Where(e => !e.IsDeleted)
                .ToListAsync();

            items.AddRange(ammunitions.Cast<BaseItem>());
            items.AddRange(weapons.Cast<BaseItem>());
            items.AddRange(explosives.Cast<BaseItem>());

            return items;
        }
    }
}
