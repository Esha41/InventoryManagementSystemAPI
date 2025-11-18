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
        private readonly IMapper _mapper;
        private readonly IValidator<CreateInventoryDto> _createValidator;
        private readonly IValidator<UpdateInventoryDto> _updateValidator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(
            ICrossCuttingRepository<InventoryEntity> inventoryRepository,
            ICrossCuttingRepository<InventoryDetailEntity> inventoryDetailRepository,
            IMapper mapper,
            IValidator<CreateInventoryDto> createValidator,
            IValidator<UpdateInventoryDto> updateValidator,
            ICurrentUserService currentUserService,
            ILogger<InventoryService> logger)
        {
            _inventoryRepository = inventoryRepository;
            _inventoryDetailRepository = inventoryDetailRepository;
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

             
                foreach (var detailDto in inputDto.InventoryDetails)
                {
                    if (detailDto.Id.HasValue)
                    {
                     
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

               
                existingInventory.DepoId = inputDto.DepoId;
                existingInventory.InvoiceNumber = inputDto.InvoiceNumber;
                existingInventory.InvoiceDate = inputDto.InvoiceDate;
                existingInventory.RecievedDate = inputDto.RecievedDate;
                existingInventory.Notes = inputDto.Notes;
                existingInventory.ModificationDate = DateTime.UtcNow;
                existingInventory.ModifiedBy = _currentUserService.UserId;

                var parentUpdate = new InventoryEntity
                {
                    Id = existingInventory.Id,
                    DepoId = existingInventory.DepoId,
                    InvoiceNumber = existingInventory.InvoiceNumber,
                    InvoiceDate = existingInventory.InvoiceDate,
                    RecievedDate = existingInventory.RecievedDate,
                    Notes = existingInventory.Notes,
                    ModificationDate = existingInventory.ModificationDate,
                    ModifiedBy = existingInventory.ModifiedBy,
                    CreationDate = existingInventory.CreationDate,
                    CreatedBy = existingInventory.CreatedBy,
                    IsDeleted = existingInventory.IsDeleted
                };
                
                await _inventoryRepository.UpdateAsync(parentUpdate);
             
                foreach (var detailDto in inputDto.InventoryDetails)
                {
                    if (detailDto.Id.HasValue)
                    {
                        var existingDetail = existingInventory.InventoryDetails.FirstOrDefault(d => d.Id == detailDto.Id.Value);
                        if (existingDetail != null)
                        {
                 
                            var detailUpdate = new InventoryDetailEntity
                            {
                                Id = existingDetail.Id,
                                InventoryId = existingDetail.InventoryId,
                                ItemId = existingDetail.ItemId,
                                Lot = existingDetail.Lot,
                                SupplierId = existingDetail.SupplierId,
                                ManufacturerId = existingDetail.ManufacturerId,
                                CountryId = existingDetail.CountryId,
                                ItemQuantity = existingDetail.ItemQuantity
                            };
                            await _inventoryDetailRepository.UpdateAsync(detailUpdate);
                        }
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
    }
}

