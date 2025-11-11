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
        private readonly ICrossCuttingRepository<Order> _orderRepository;
        private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateAllowanceItemDto> _validator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AllowanceItemService> _logger;

        public AllowanceItemService(
            ICrossCuttingRepository<AllowanceItem> allowanceItemRepository,
            ICrossCuttingRepository<Department> departmentRepository,
            ICrossCuttingRepository<Order> orderRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            IMapper mapper,
            IValidator<CreateUpdateAllowanceItemDto> validator,
            ICurrentUserService currentUserService,
            ILogger<AllowanceItemService> logger)
        {
            _allowanceItemRepository = allowanceItemRepository;
            _departmentRepository = departmentRepository;
            _orderRepository = orderRepository;
            _requestItemRepository = requestItemRepository;
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

        public async Task<APIOperationResponse<long>> CreateAsync(CreateUpdateAllowanceItemDto inputDto)
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
                   
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                // Map DTO to entity
                var allowanceItem = _mapper.Map<AllowanceItem>(inputDto);
                allowanceItem.CreationDate = DateTime.UtcNow;
                allowanceItem.CreatedBy = _currentUserService.UserId;

                // Add to repository
                var createdAllowanceItem = await _allowanceItemRepository.AddAsync(allowanceItem);

                // Reload with navigation properties
               

                _logger.LogInformation("Allowance item created successfully. AllowanceItemId: {AllowanceItemId}, ItemId: {ItemId}, User: {UserId}", 
                    createdAllowanceItem.Id, inputDto.ItemId, _currentUserService.UserId);

              
               
                return APIOperationResponse<long>.Success(createdAllowanceItem.Id, "Allowance item created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating allowance item. ItemId: {ItemId}, DepartmentId: {DepartmentId}, User: {UserId}", 
                    inputDto?.ItemId, inputDto?.DepartmentId, _currentUserService.UserId);
             
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateAllowanceItemDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                // Check if allowance item exists
                var existingAllowanceItem = await _allowanceItemRepository.FindOneAsync(a => a.Id == id);
                if (existingAllowanceItem == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Allowance item not found");

                // Map updates to entity
                _mapper.Map(inputDto, existingAllowanceItem);
                existingAllowanceItem.ModificationDate = DateTime.UtcNow;
                existingAllowanceItem.ModifiedBy = _currentUserService.UserId;

                // Update in repository
                await _allowanceItemRepository.UpdateAsync(existingAllowanceItem);
                return APIOperationResponse<bool>.Success(true, "Allowance item updated successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
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

        public async Task<APIOperationResponse<AllowanceReserveDetailsDto>> GetReserveDetailsAsync(long departmentId, int year)
        {
            _logger.LogInformation("Getting reserve details. DepartmentId: {DepartmentId}, Year: {Year}, User: {UserId}", 
                departmentId, year, _currentUserService.UserId);
            
            try
            {
                // Validate department
                var department = await _departmentRepository.FindOneAsync(d => d.Id == departmentId && !d.IsDeleted);
                if (department == null)
                {
                    _logger.LogWarning("Department not found. DepartmentId: {DepartmentId}, User: {UserId}", 
                        departmentId, _currentUserService.UserId);
                    return APIOperationResponse<AllowanceReserveDetailsDto>.Fail(ResponseType.NotFound, "Department not found");
                }

                // Get all allowance items for the department and year
                var allowanceItems = await _allowanceItemRepository.FindAsync(
                    a => a.DepartmentId == departmentId && a.Year == year && !a.IsDeleted);

                // Calculate total reserve (sum of all allowance quantities)
                var totalReserve = allowanceItems.Sum(a => a.Quantity);

                // Get all orders from allowance for this department and year
                var ordersFromAllowance = await _orderRepository.FindAsync(
                    o => o.DepartmentId == departmentId && 
                         o.IsFromAllowance && 
                         o.UsageDate.Year == year &&
                         !o.IsDeleted);

                // Calculate ordered quantity (New and UnderProcess orders)
                var orderedOrderIds = ordersFromAllowance
                    .Where(o => o.Status == RequestStatus.New || o.Status == RequestStatus.UnderProcess)
                    .Select(o => o.Id)
                    .ToList();

                var orderedItems = await _requestItemRepository.FindAsync(
                    ri => orderedOrderIds.Contains(ri.RequestId) && !ri.IsDeleted);
                var orderedQuantity = (int)orderedItems.Sum(ri => ri.Quantity);

                // Calculate utilized quantity (Approved orders)
                var utilizedOrderIds = ordersFromAllowance
                    .Where(o => o.Status == RequestStatus.Approved)
                    .Select(o => o.Id)
                    .ToList();

                var utilizedItems = await _requestItemRepository.FindAsync(
                    ri => utilizedOrderIds.Contains(ri.RequestId) && !ri.IsDeleted);
                var utilizedQuantity = (int)utilizedItems.Sum(ri => ri.Quantity);

                // Calculate available reserve
                var availableReserve = totalReserve - orderedQuantity - utilizedQuantity;

                var result = new AllowanceReserveDetailsDto
                {
                    DepartmentId = departmentId,
                    Year = year,
                    TotalReserve = totalReserve,
                    AvailableReserve = Math.Max(0, availableReserve), // Ensure non-negative
                    OrderedQuantity = orderedQuantity,
                    UtilizedQuantity = utilizedQuantity
                };

                _logger.LogInformation("Reserve details calculated. DepartmentId: {DepartmentId}, Year: {Year}, Total: {Total}, Available: {Available}, Ordered: {Ordered}, Utilized: {Utilized}", 
                    departmentId, year, totalReserve, availableReserve, orderedQuantity, utilizedQuantity);

                return APIOperationResponse<AllowanceReserveDetailsDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reserve details. DepartmentId: {DepartmentId}, Year: {Year}, User: {UserId}", 
                    departmentId, year, _currentUserService.UserId);
                return APIOperationResponse<AllowanceReserveDetailsDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<AllowanceReserveDetailsByItemDto>> GetReserveDetailsByItemAsync(long departmentId, int year)
        {
            _logger.LogInformation("Getting reserve details by item. DepartmentId: {DepartmentId}, Year: {Year}, User: {UserId}", 
                departmentId, year, _currentUserService.UserId);
            
            try
            {
                // Validate department
                var department = await _departmentRepository.FindOneAsync(d => d.Id == departmentId && !d.IsDeleted);
                if (department == null)
                {
                    _logger.LogWarning("Department not found. DepartmentId: {DepartmentId}, User: {UserId}", 
                        departmentId, _currentUserService.UserId);
                    return APIOperationResponse<AllowanceReserveDetailsByItemDto>.Fail(ResponseType.NotFound, "Department not found");
                }

                // Get all allowance items for the department and year with item details
                var allowanceItems = await _allowanceItemRepository.FindAsync(
                    a => a.DepartmentId == departmentId && a.Year == year && !a.IsDeleted,
                    false,
                    nameof(AllowanceItem.Item));

                if (!allowanceItems.Any())
                {
                    _logger.LogInformation("No allowance items found for department. DepartmentId: {DepartmentId}, Year: {Year}", 
                        departmentId, year);
                    
                    return APIOperationResponse<AllowanceReserveDetailsByItemDto>.Success(new AllowanceReserveDetailsByItemDto
                    {
                        DepartmentId = departmentId,
                        DepartmentCode = department.Code,
                        DepartmentNameAr = department.NameAr,
                        DepartmentNameEn = department.NameEn,
                        Year = year,
                        TotalReserve = 0,
                        TotalAvailableReserve = 0,
                        TotalOrderedQuantity = 0,
                        TotalUtilizedQuantity = 0,
                        Items = new List<AllowanceItemReserveDetailsDto>()
                    });
                }

                // Get all orders from allowance for this department and year
                var ordersFromAllowance = await _orderRepository.FindAsync(
                    o => o.DepartmentId == departmentId && 
                         o.IsFromAllowance && 
                         o.UsageDate.Year == year &&
                         !o.IsDeleted);

                var allOrderIds = ordersFromAllowance.Select(o => o.Id).ToList();
                
                // Get all request items for these orders
                var allRequestItems = await _requestItemRepository.FindAsync(
                    ri => allOrderIds.Contains(ri.RequestId) && !ri.IsDeleted);

                // Calculate per-item details
                var itemDetailsList = new List<AllowanceItemReserveDetailsDto>();
                int totalReserve = 0;
                int totalAvailableReserve = 0;
                int totalOrderedQuantity = 0;
                int totalUtilizedQuantity = 0;

                foreach (var allowanceItem in allowanceItems)
                {
                    var itemId = allowanceItem.ItemId;
                    var itemTotalReserve = allowanceItem.Quantity;

                    // Calculate ordered quantity for this item (New and UnderProcess orders)
                    var orderedOrderIds = ordersFromAllowance
                        .Where(o => o.Status == RequestStatus.New || o.Status == RequestStatus.UnderProcess)
                        .Select(o => o.Id)
                        .ToList();

                    var itemOrderedQuantity = (int)allRequestItems
                        .Where(ri => orderedOrderIds.Contains(ri.RequestId) && ri.ItemId == itemId)
                        .Sum(ri => ri.Quantity);

                    // Calculate utilized quantity for this item (Approved orders)
                    var utilizedOrderIds = ordersFromAllowance
                        .Where(o => o.Status == RequestStatus.Approved)
                        .Select(o => o.Id)
                        .ToList();

                    var itemUtilizedQuantity = (int)allRequestItems
                        .Where(ri => utilizedOrderIds.Contains(ri.RequestId) && ri.ItemId == itemId)
                        .Sum(ri => ri.Quantity);

                    // Calculate available reserve for this item
                    var itemAvailableReserve = Math.Max(0, itemTotalReserve - itemOrderedQuantity - itemUtilizedQuantity);

                    // Get item details
                    var item = allowanceItem.Item;
                    var itemName = item?.Name ?? "Unknown Item";
                    var itemNo = item?.ItemNo ?? "";
                    var batchNo = item?.BatchNo ?? "";

                    itemDetailsList.Add(new AllowanceItemReserveDetailsDto
                    {
                        ItemId = itemId,
                        ItemName = itemName,
                        ItemNo = itemNo,
                        BatchNo = batchNo,
                        TotalReserve = itemTotalReserve,
                        AvailableReserve = itemAvailableReserve,
                        OrderedQuantity = itemOrderedQuantity,
                        UtilizedQuantity = itemUtilizedQuantity
                    });

                    // Accumulate totals
                    totalReserve += itemTotalReserve;
                    totalAvailableReserve += itemAvailableReserve;
                    totalOrderedQuantity += itemOrderedQuantity;
                    totalUtilizedQuantity += itemUtilizedQuantity;
                }

                var result = new AllowanceReserveDetailsByItemDto
                {
                    DepartmentId = departmentId,
                    DepartmentCode = department.Code,
                    DepartmentNameAr = department.NameAr,
                    DepartmentNameEn = department.NameEn,
                    Year = year,
                    TotalReserve = totalReserve,
                    TotalAvailableReserve = totalAvailableReserve,
                    TotalOrderedQuantity = totalOrderedQuantity,
                    TotalUtilizedQuantity = totalUtilizedQuantity,
                    Items = itemDetailsList
                };

                _logger.LogInformation("Reserve details by item calculated. DepartmentId: {DepartmentId}, Year: {Year}, ItemCount: {ItemCount}, TotalReserve: {Total}, TotalAvailable: {Available}", 
                    departmentId, year, itemDetailsList.Count, totalReserve, totalAvailableReserve);

                return APIOperationResponse<AllowanceReserveDetailsByItemDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reserve details by item. DepartmentId: {DepartmentId}, Year: {Year}, User: {UserId}", 
                    departmentId, year, _currentUserService.UserId);
                return APIOperationResponse<AllowanceReserveDetailsByItemDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}

