using AutoMapper;
using FluentValidation;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.AllowanceItems.Dtos;
using Ettad.Inventory.Service.AllowanceItems.Validators;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Ettad.Data.Enums;
using Microsoft.Extensions.Logging;

namespace Ettad.Inventory.Service.AllowanceItems
{
    public class AllowanceItemService : IAllowanceItemService
    {
        private readonly ICrossCuttingRepository<AllowanceItem> _allowanceItemRepository;
        private readonly ICrossCuttingRepository<Department> _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateAllowanceItemDto> _validator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AllowanceItemService> _logger;

        public AllowanceItemService(
            ICrossCuttingRepository<AllowanceItem> allowanceItemRepository,
            ICrossCuttingRepository<Department> departmentRepository,
            IMapper mapper,
            IValidator<CreateUpdateAllowanceItemDto> validator,
            ICurrentUserService currentUserService,
            ILogger<AllowanceItemService> logger)
        {
            _allowanceItemRepository = allowanceItemRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<APIOperationResponse<AllowanceItemDto>> GetByIdAsync(long id)
        {
            try
            {
                var allowanceItem = await _allowanceItemRepository.FindOneAsync(x => x.Id == id);

                if (allowanceItem == null)
                    return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.NotFound, "Allowance item not found");

                var dto = _mapper.Map<AllowanceItemDto>(allowanceItem);
                return APIOperationResponse<AllowanceItemDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<AllowanceItemDto>>> GetAllAsync()
        {
            try
            {
                // Only get non-deleted items
                var allowanceItems = await _allowanceItemRepository.FindAsync(a => !a.IsDeleted);

                var dtos = _mapper.Map<List<AllowanceItemDto>>(allowanceItems);
                return APIOperationResponse<List<AllowanceItemDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<AllowanceItemDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<AllowanceItemDto>> CreateAsync(CreateUpdateAllowanceItemDto inputDto)
        {
            _logger.LogInformation("Creating allowance item. ItemId: {ItemId}, DepartmentId: {DepartmentId}, Year: {Year}, Quantity: {Quantity}, User: {UserId}", 
                inputDto?.ItemId, inputDto?.DepartmentId, inputDto?.Year, inputDto?.Quantity, _currentUserService.UserId);
            
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning("Allowance item validation failed. Errors: {ValidationErrors}, User: {UserId}", 
                        errors, _currentUserService.UserId);
                    return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.BadRequest, errors);
                }

                // Map DTO to entity
                var allowanceItem = _mapper.Map<AllowanceItem>(inputDto);
                allowanceItem.CreationDate = DateTime.UtcNow;
                allowanceItem.CreatedBy = _currentUserService.UserId;

                // Add to repository
                var createdAllowanceItem = await _allowanceItemRepository.AddAsync(allowanceItem);

                // Reload with navigation properties
                var result = await _allowanceItemRepository.FindOneAsync( a => a.Id == createdAllowanceItem.Id);

                _logger.LogInformation("Allowance item created successfully. AllowanceItemId: {AllowanceItemId}, ItemId: {ItemId}, User: {UserId}", 
                    createdAllowanceItem.Id, inputDto.ItemId, _currentUserService.UserId);

                var dto = _mapper.Map<AllowanceItemDto>(result);
                return APIOperationResponse<AllowanceItemDto>.Success(dto, "Allowance item created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating allowance item. ItemId: {ItemId}, DepartmentId: {DepartmentId}, User: {UserId}", 
                    inputDto?.ItemId, inputDto?.DepartmentId, _currentUserService.UserId);
                return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<AllowanceItemDto>> UpdateAsync(long id, CreateUpdateAllowanceItemDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.BadRequest, errors);
                }

                // Check if allowance item exists
                var existingAllowanceItem = await _allowanceItemRepository.FindOneAsync(a => a.Id == id);
                if (existingAllowanceItem == null)
                    return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.NotFound, "Allowance item not found");

                // Map updates to entity
                _mapper.Map(inputDto, existingAllowanceItem);
                existingAllowanceItem.ModificationDate = DateTime.UtcNow;
                existingAllowanceItem.ModifiedBy = _currentUserService.UserId;

                // Update in repository
                await _allowanceItemRepository.UpdateAsync(existingAllowanceItem);

                // Reload with navigation properties
                var result = await _allowanceItemRepository.FindOneAsync(a => a.Id == id);

                var dto = _mapper.Map<AllowanceItemDto>(result);
                return APIOperationResponse<AllowanceItemDto>.Success(dto, "Allowance item updated successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting allowance item. AllowanceItemId: {AllowanceItemId}, User: {UserId}", 
                id, _currentUserService.UserId);
            
            try
            {
                var allowanceItem = await _allowanceItemRepository.FindOneAsync(a => a.Id == id && !a.IsDeleted);
                if (allowanceItem == null)
                {
                    _logger.LogWarning("Allowance item not found for deletion. AllowanceItemId: {AllowanceItemId}, User: {UserId}", 
                        id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Allowance item not found");
                }

                var itemId = allowanceItem.ItemId;
                var quantity = allowanceItem.Quantity;
                
                // Soft delete - interceptor will handle IsDeleted, DeletionDate, and DeletedBy automatically
                await _allowanceItemRepository.DeleteAsync(allowanceItem);

                _logger.LogInformation("Allowance item deleted successfully. AllowanceItemId: {AllowanceItemId}, ItemId: {ItemId}, Quantity: {Quantity}, User: {UserId}", 
                    id, itemId, quantity, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true, "Allowance item deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting allowance item. AllowanceItemId: {AllowanceItemId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<AllowanceItemByDepartmentDto>> GetByDepartmentAndYearAsync(long departmentId, int year)
        {
            try
            {
                var department = await _departmentRepository.FindOneAsync(d => d.Id == departmentId && !d.IsDeleted);
                if (department == null)
                    return APIOperationResponse<AllowanceItemByDepartmentDto>.Fail(ResponseType.NotFound, "Department not found");

                var allowanceItems = await _allowanceItemRepository.FindAsync(
                    a => a.DepartmentId == departmentId && a.Year == year && !a.IsDeleted,
                    false,
                    nameof(AllowanceItem.Item),
                    nameof(AllowanceItem.Department)
                );

                var itemDetails = _mapper.Map<List<AllowanceItemDetailDto>>(allowanceItems);

                var result = new AllowanceItemByDepartmentDto
                {
                    DepartmentId = department.Id,
                    DepartmentCode = department.Code,
                    DepartmentNameAr = department.NameAr,
                    DepartmentNameEn = department.NameEn,
                    Year = year,
                    Items = itemDetails
                };

                return APIOperationResponse<AllowanceItemByDepartmentDto>.Success(result);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<AllowanceItemByDepartmentDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<AllowanceItemByDepartmentDto>>> GetByDepartmentAsync(long departmentId)
        {
            try
            {
                var department = await _departmentRepository.FindOneAsync(d => d.Id == departmentId && !d.IsDeleted);
                if (department == null)
                    return APIOperationResponse<List<AllowanceItemByDepartmentDto>>.Fail(ResponseType.NotFound, "Department not found");

                var allowanceItems = await _allowanceItemRepository.FindAsync(
                    a => a.DepartmentId == departmentId && !a.IsDeleted,
                    false,
                    nameof(AllowanceItem.Item),
                    nameof(AllowanceItem.Department)
                );

                var groupedByYear = allowanceItems.GroupBy(a => a.Year).ToList();

                var result = groupedByYear.Select(group => new AllowanceItemByDepartmentDto
                {
                    DepartmentId = department.Id,
                    DepartmentCode = department.Code,
                    DepartmentNameAr = department.NameAr,
                    DepartmentNameEn = department.NameEn,
                    Year = group.Key,
                    Items = _mapper.Map<List<AllowanceItemDetailDto>>(group.ToList())
                }).ToList();

                return APIOperationResponse<List<AllowanceItemByDepartmentDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<AllowanceItemByDepartmentDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<AllowanceItemDto>>> BulkCreateAsync(BulkCreateAllowanceItemDto inputDto)
        {
            _logger.LogInformation("Bulk creating allowance items. DepartmentId: {DepartmentId}, Year: {Year}, ItemCount: {ItemCount}, User: {UserId}", 
                inputDto?.DepartmentId, inputDto?.Year, inputDto?.Items?.Count ?? 0, _currentUserService.UserId);
            
            try
            {
                var bulkValidator = new BulkCreateAllowanceItemDtoValidator();
                var validationResult = await bulkValidator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning("Bulk allowance items validation failed. Errors: {ValidationErrors}, User: {UserId}", 
                        errors, _currentUserService.UserId);
                    return APIOperationResponse<List<AllowanceItemDto>>.Fail(ResponseType.BadRequest, errors);
                }

                var createdItems = new List<AllowanceItemDto>();
                int createdCount = 0;
                int updatedCount = 0;

                foreach (var itemDto in inputDto.Items)
                {
                    var createDto = new CreateUpdateAllowanceItemDto
                    {
                        ItemId = itemDto.ItemId,
                        DepartmentId = inputDto.DepartmentId,
                        Year = inputDto.Year,
                        Quantity = itemDto.Quantity,
                    };

                    // Validate individual item
                    var itemValidationResult = await _validator.ValidateAsync(createDto);
                    if (!itemValidationResult.IsValid)
                    {
                        var errors = string.Join(", ", itemValidationResult.Errors.Select(e => e.ErrorMessage));
                        _logger.LogWarning("Item validation failed in bulk create. ItemId: {ItemId}, Errors: {Errors}", 
                            itemDto.ItemId, errors);
                        return APIOperationResponse<List<AllowanceItemDto>>.Fail(ResponseType.BadRequest, $"Item {itemDto.ItemId}: {errors}");
                    }

                    // Check if already exists
                    var existing = await _allowanceItemRepository.FindOneAsync(
                        a => a.ItemId == itemDto.ItemId &&
                             a.DepartmentId == inputDto.DepartmentId &&
                             a.Year == inputDto.Year &&
                             !a.IsDeleted);

                    if (existing != null)
                    {
                        // Update existing quantity
                        existing.Quantity = itemDto.Quantity;
                        existing.ModificationDate = DateTime.UtcNow;
                        existing.ModifiedBy = _currentUserService.UserId;
                        await _allowanceItemRepository.UpdateAsync(existing);
                        createdItems.Add(_mapper.Map<AllowanceItemDto>(existing));
                        updatedCount++;
                    }
                    else
                    {
                        // Create new
                        var allowanceItem = _mapper.Map<AllowanceItem>(createDto);
                        allowanceItem.CreationDate = DateTime.UtcNow;
                        allowanceItem.CreatedBy = _currentUserService.UserId;

                        var created = await _allowanceItemRepository.AddAsync(allowanceItem);
                        createdItems.Add(_mapper.Map<AllowanceItemDto>(created));
                        createdCount++;
                    }
                }

                _logger.LogInformation("Bulk allowance items operation completed. DepartmentId: {DepartmentId}, Year: {Year}, Created: {CreatedCount}, Updated: {UpdatedCount}, User: {UserId}", 
                    inputDto.DepartmentId, inputDto.Year, createdCount, updatedCount, _currentUserService.UserId);

                return APIOperationResponse<List<AllowanceItemDto>>.Success(createdItems, "Allowance items created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in bulk create allowance items. DepartmentId: {DepartmentId}, Year: {Year}, User: {UserId}", 
                    inputDto?.DepartmentId, inputDto?.Year, _currentUserService.UserId);
                return APIOperationResponse<List<AllowanceItemDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}

