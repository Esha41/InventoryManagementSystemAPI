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
        private readonly ICrossCuttingRepository<Supply> _supplyRepository;
        private readonly ICrossCuttingRepository<SupplyDetail> _supplyDetailRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateAllowanceItemDto> _validator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AllowanceItemService> _logger;
        private readonly IPermissionService _permissionService;

        public AllowanceItemService(
            ICrossCuttingRepository<AllowanceItem> allowanceItemRepository,
            ICrossCuttingRepository<Department> departmentRepository,
            ICrossCuttingRepository<Order> orderRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            ICrossCuttingRepository<Supply> supplyRepository,
            ICrossCuttingRepository<SupplyDetail> supplyDetailRepository,
            IMapper mapper,
            IValidator<CreateUpdateAllowanceItemDto> validator,
            ICurrentUserService currentUserService,
            ILogger<AllowanceItemService> logger,
            IPermissionService permissionService)
        {
            _allowanceItemRepository = allowanceItemRepository;
            _departmentRepository = departmentRepository;
            _orderRepository = orderRepository;
            _requestItemRepository = requestItemRepository;
            _supplyRepository = supplyRepository;
            _supplyDetailRepository = supplyDetailRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
            _logger = logger;
            _permissionService = permissionService;
        }

        public async Task<APIOperationResponse<AllowanceItemDto>> GetByIdAsync(long id)
        {
            try
            {
                var allowanceItem = await _allowanceItemRepository.FindOneAsync(
                    x => x.Id == id,
                    false,
                    nameof(AllowanceItem.Item));

                if (allowanceItem == null)
                    return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.NotFound, "Allowance item not found");

                // Check authorization: users with ViewAllDepartments permission or admin can access all departments
                var canViewAll = _currentUserService.IsSuperAdmin;
                if (!canViewAll)
                {
                    canViewAll = await _permissionService.HasPermissionAsync("AllowanceItemViewAllDepartments");
                }
                var userDepartmentId = _currentUserService.DepartmentId;

                if (!canViewAll)
                {
                    if (!userDepartmentId.HasValue)
                    {
                        _logger.LogWarning("User has no department assigned. UserId: {UserId}", _currentUserService.UserId);
                        return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.Forbidden, "You do not have permission to access this allowance item");
                    }

                    if (allowanceItem.DepartmentId != userDepartmentId.Value)
                    {
                        _logger.LogWarning("User attempted to access allowance from different department. UserDepartmentId: {UserDeptId}, AllowanceDepartmentId: {AllowanceDeptId}, UserId: {UserId}", 
                            userDepartmentId.Value, allowanceItem.DepartmentId, _currentUserService.UserId);
                        return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.Forbidden, "You do not have permission to access this allowance item");
                    }
                }

                var dto = _mapper.Map<AllowanceItemDto>(allowanceItem);
                await PopulateCalculatedQuantitiesAsync(dto, allowanceItem);
                return APIOperationResponse<AllowanceItemDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving allowance item by ID. Id: {Id}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<AllowanceItemDto>>> GetAllAsync()
        {
            try
            {
                var userDepartmentId = _currentUserService.DepartmentId;
                
                // Check if user can view all departments
                // First check SuperAdmin (from JWT)
                var canViewAll = _currentUserService.IsSuperAdmin;
                
                // If not super admin, check for the specific permission using the permission service
                if (!canViewAll)
                {
                    canViewAll = await _permissionService.HasPermissionAsync("AllowanceItemViewAllDepartments");
                }

                _logger.LogInformation("GetAllAsync - User: {UserId}, IsSuperAdmin: {IsSuperAdmin}, IsAdminRole: {IsAdminRole}, HasPermission: {HasPermission}, CanViewAll: {CanViewAll}, DepartmentId: {DepartmentId}", 
                    _currentUserService.UserId,
                    _currentUserService.IsSuperAdmin,
                    _currentUserService.IsAdminRole,
                    await _permissionService.HasPermissionAsync("AllowanceItemViewAllDepartments"),
                    canViewAll,
                    userDepartmentId);

                // Build filter: non-deleted items, and filter by department if user can't view all
                System.Linq.Expressions.Expression<System.Func<AllowanceItem, bool>> filter;
                
                if (canViewAll)
                {
                    // User can view all departments - no department filter
                    filter = a => !a.IsDeleted;
                    _logger.LogInformation("Returning allowances for ALL departments. User: {UserId}", _currentUserService.UserId);
                }
                else if (userDepartmentId.HasValue)
                {
                    // Non-admin users can only see allowances for their own department
                    filter = a => !a.IsDeleted && a.DepartmentId == userDepartmentId.Value;
                    _logger.LogInformation("Filtering allowances by department. DepartmentId: {DepartmentId}, User: {UserId}", 
                        userDepartmentId.Value, _currentUserService.UserId);
                }
                else
                {
                    // User has no department assigned - return empty list
                    _logger.LogWarning("User has no department assigned and no view all permission. UserId: {UserId}", _currentUserService.UserId);
                    return APIOperationResponse<List<AllowanceItemDto>>.Success(new List<AllowanceItemDto>());
                }

                // Only get non-deleted items, include Item navigation property for names
                var allowanceItems = await _allowanceItemRepository.FindAsync(
                    filter,
                    false,
                    nameof(AllowanceItem.Item));

                var dtos = _mapper.Map<List<AllowanceItemDto>>(allowanceItems);
                
                // Populate calculated quantities for each DTO
                foreach (var dto in dtos)
                {
                    var allowanceItem = allowanceItems.FirstOrDefault(a => a.Id == dto.Id);
                    if (allowanceItem != null)
                    {
                        await PopulateCalculatedQuantitiesAsync(dto, allowanceItem);
                    }
                }
                
                _logger.LogInformation("Retrieved {Count} allowance items. CanViewAll: {CanViewAll}, DepartmentId: {DepartmentId}, User: {UserId}", 
                    dtos.Count, canViewAll, userDepartmentId, _currentUserService.UserId);
                
                return APIOperationResponse<List<AllowanceItemDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving allowance items. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<AllowanceItemDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateUpdateAllowanceItemDto inputDto)
        {
            _logger.LogInformation("Creating allowance item. ItemId: {ItemId}, DepartmentId: {DepartmentId}, Year: {Year}, Quantity: {Quantity}, User: {UserId}", 
                inputDto?.ItemId, inputDto?.DepartmentId, inputDto?.Year, inputDto?.Quantity, _currentUserService.UserId);
            
            try
            {
                // Check authorization: users with ViewAllDepartments permission or admin can create for all departments
                var canViewAll = _currentUserService.IsSuperAdmin;
                if (!canViewAll)
                {
                    canViewAll = await _permissionService.HasPermissionAsync("AllowanceItemViewAllDepartments");
                }
                var userDepartmentId = _currentUserService.DepartmentId;

                if (!canViewAll)
                {
                    if (!userDepartmentId.HasValue)
                    {
                        _logger.LogWarning("User has no department assigned. UserId: {UserId}", _currentUserService.UserId);
                        return APIOperationResponse<long>.Fail(ResponseType.Forbidden, "You do not have permission to create allowances");
                    }

                    if (inputDto.DepartmentId != userDepartmentId.Value)
                    {
                        _logger.LogWarning("User attempted to create allowance for different department. UserDepartmentId: {UserDeptId}, RequestedDepartmentId: {RequestedDeptId}, UserId: {UserId}", 
                            userDepartmentId.Value, inputDto.DepartmentId, _currentUserService.UserId);
                        return APIOperationResponse<long>.Fail(ResponseType.Forbidden, "You can only create allowances for your own department");
                    }
                }

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
                // Check if allowance item exists
                var existingAllowanceItem = await _allowanceItemRepository.FindOneAsync(a => a.Id == id);
                if (existingAllowanceItem == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Allowance item not found");

                // Check authorization: users with ViewAllDepartments permission or admin can update for all departments
                var canViewAll = _currentUserService.IsSuperAdmin;
                if (!canViewAll)
                {
                    canViewAll = await _permissionService.HasPermissionAsync("AllowanceItemViewAllDepartments");
                }
                var userDepartmentId = _currentUserService.DepartmentId;

                if (!canViewAll)
                {
                    if (!userDepartmentId.HasValue)
                    {
                        _logger.LogWarning("User has no department assigned. UserId: {UserId}", _currentUserService.UserId);
                        return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You do not have permission to update this allowance item");
                    }

                    if (existingAllowanceItem.DepartmentId != userDepartmentId.Value || inputDto.DepartmentId != userDepartmentId.Value)
                    {
                        _logger.LogWarning("User attempted to update allowance from different department. UserDepartmentId: {UserDeptId}, ExistingDepartmentId: {ExistingDeptId}, RequestedDepartmentId: {RequestedDeptId}, UserId: {UserId}", 
                            userDepartmentId.Value, existingAllowanceItem.DepartmentId, inputDto.DepartmentId, _currentUserService.UserId);
                        return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You can only update allowances for your own department");
                    }
                }

                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

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

                // Check authorization: users with ViewAllDepartments permission or admin can delete for all departments
                var canViewAll = _currentUserService.IsSuperAdmin;
                if (!canViewAll)
                {
                    canViewAll = await _permissionService.HasPermissionAsync("AllowanceItemViewAllDepartments");
                }
                var userDepartmentId = _currentUserService.DepartmentId;

                if (!canViewAll)
                {
                    if (!userDepartmentId.HasValue)
                    {
                        _logger.LogWarning("User has no department assigned. UserId: {UserId}", _currentUserService.UserId);
                        return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You do not have permission to delete this allowance item");
                    }

                    if (allowanceItem.DepartmentId != userDepartmentId.Value)
                    {
                        _logger.LogWarning("User attempted to delete allowance from different department. UserDepartmentId: {UserDeptId}, AllowanceDepartmentId: {AllowanceDeptId}, UserId: {UserId}", 
                            userDepartmentId.Value, allowanceItem.DepartmentId, _currentUserService.UserId);
                        return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You can only delete allowances for your own department");
                    }
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
                // Check authorization: users with ViewAllDepartments permission or admin can access all departments
                var canViewAll = _currentUserService.IsSuperAdmin;
                if (!canViewAll)
                {
                    canViewAll = await _permissionService.HasPermissionAsync("AllowanceItemViewAllDepartments");
                }
                var userDepartmentId = _currentUserService.DepartmentId;

                if (!canViewAll)
                {
                    if (!userDepartmentId.HasValue)
                    {
                        _logger.LogWarning("User has no department assigned. UserId: {UserId}", _currentUserService.UserId);
                        return APIOperationResponse<AllowanceItemByDepartmentDto>.Fail(ResponseType.Forbidden, "You do not have permission to access this department's allowances");
                    }

                    if (departmentId != userDepartmentId.Value)
                    {
                        _logger.LogWarning("User attempted to access allowances from different department. UserDepartmentId: {UserDeptId}, RequestedDepartmentId: {RequestedDeptId}, UserId: {UserId}", 
                            userDepartmentId.Value, departmentId, _currentUserService.UserId);
                        return APIOperationResponse<AllowanceItemByDepartmentDto>.Fail(ResponseType.Forbidden, "You do not have permission to access this department's allowances");
                    }
                }

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

                // Populate calculated quantities
                var quantities = await GetCalculatedQuantitiesForDepartmentYearAsync(departmentId, year);
                
                foreach (var detail in itemDetails)
                {
                    if (quantities.TryGetValue(detail.ItemId, out var qty))
                    {
                        detail.UsedQuantityFromAllowance = qty.Used;
                        detail.ReservedQuantityByOrdersOnProcessing = qty.Reserved;
                    }
                    else
                    {
                        detail.UsedQuantityFromAllowance = 0;
                        detail.ReservedQuantityByOrdersOnProcessing = 0;
                    }
                    detail.RemainingQuantityFromAllowance = Math.Max(0, detail.Quantity - detail.UsedQuantityFromAllowance - detail.ReservedQuantityByOrdersOnProcessing);
                }

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
                // Check authorization: users with ViewAllDepartments permission or admin can access all departments
                var canViewAll = _currentUserService.IsSuperAdmin;
                if (!canViewAll)
                {
                    canViewAll = await _permissionService.HasPermissionAsync("AllowanceItemViewAllDepartments");
                }
                var userDepartmentId = _currentUserService.DepartmentId;

                if (!canViewAll)
                {
                    if (!userDepartmentId.HasValue)
                    {
                        _logger.LogWarning("User has no department assigned. UserId: {UserId}", _currentUserService.UserId);
                        return APIOperationResponse<List<AllowanceItemByDepartmentDto>>.Fail(ResponseType.Forbidden, "You do not have permission to access this department's allowances");
                    }

                    if (departmentId != userDepartmentId.Value)
                    {
                        _logger.LogWarning("User attempted to access allowances from different department. UserDepartmentId: {UserDeptId}, RequestedDepartmentId: {RequestedDeptId}, UserId: {UserId}", 
                            userDepartmentId.Value, departmentId, _currentUserService.UserId);
                        return APIOperationResponse<List<AllowanceItemByDepartmentDto>>.Fail(ResponseType.Forbidden, "You do not have permission to access this department's allowances");
                    }
                }

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
                var result = new List<AllowanceItemByDepartmentDto>();

                foreach (var group in groupedByYear)
                {
                    var year = group.Key;
                    var details = _mapper.Map<List<AllowanceItemDetailDto>>(group.ToList());
                    
                    // Batch calculate for this year
                    var quantities = await GetCalculatedQuantitiesForDepartmentYearAsync(departmentId, year);
                    
                    foreach (var detail in details)
                    {
                        if (quantities.TryGetValue(detail.ItemId, out var qty))
                        {
                            detail.UsedQuantityFromAllowance = qty.Used;
                            detail.ReservedQuantityByOrdersOnProcessing = qty.Reserved;
                        }
                        else
                        {
                            detail.UsedQuantityFromAllowance = 0;
                            detail.ReservedQuantityByOrdersOnProcessing = 0;
                        }
                        detail.RemainingQuantityFromAllowance = Math.Max(0, detail.Quantity - detail.UsedQuantityFromAllowance - detail.ReservedQuantityByOrdersOnProcessing);
                    }

                    result.Add(new AllowanceItemByDepartmentDto
                    {
                        DepartmentId = department.Id,
                        DepartmentCode = department.Code,
                        DepartmentNameAr = department.NameAr,
                        DepartmentNameEn = department.NameEn,
                        Year = year,
                        Items = details
                    });
                }

                return APIOperationResponse<List<AllowanceItemByDepartmentDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<AllowanceItemByDepartmentDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private async Task<Dictionary<long, (int Used, int Reserved)>> GetCalculatedQuantitiesForDepartmentYearAsync(long departmentId, int year)
        {
            // Get all orders for this department and year
            var orders = await _orderRepository.FindAsync(
                o => o.DepartmentId == departmentId &&
                     o.CreationDate.Year == year &&
                     !o.IsDeleted);

            var orderIds = orders.Select(o => o.Id).ToList();

            if (!orderIds.Any())
            {
                return new Dictionary<long, (int, int)>();
            }

            // Get all supplies for orders that have supplies
            var supplies = await _supplyRepository.FindAsync(
                s => orderIds.Contains(s.OrderId) && !s.IsDeleted);

            // Separate Draft and Submitted supplies
            var draftSupplyIds = supplies
                .Where(s => s.SubmissionStatus == SupplySubmissionStatus.Draft)
                .Select(s => s.Id)
                .ToList();

            var submittedSupplyIds = supplies
                .Where(s => s.SubmissionStatus == SupplySubmissionStatus.Submitted)
                .Select(s => s.Id)
                .ToList();
            
            var allSupplyIds = draftSupplyIds.Concat(submittedSupplyIds).ToList();
            
            var quantities = new Dictionary<long, (int Used, int Reserved)>();

            if (allSupplyIds.Any())
            {
                var supplyDetails = await _supplyDetailRepository.FindAsync(
                    sd => allSupplyIds.Contains(sd.SupplyId) && !sd.IsDeleted);

                var usedByItem = supplyDetails
                    .Where(sd => submittedSupplyIds.Contains(sd.SupplyId))
                    .GroupBy(sd => sd.ItemId)
                    .ToDictionary(g => g.Key, g => (int)g.Sum(sd => sd.Quantity));

                var reservedByItem = supplyDetails
                    .Where(sd => draftSupplyIds.Contains(sd.SupplyId))
                    .GroupBy(sd => sd.ItemId)
                    .ToDictionary(g => g.Key, g => (int)g.Sum(sd => sd.Quantity));
                
                var allItemIds = usedByItem.Keys.Union(reservedByItem.Keys);
                
                foreach(var itemId in allItemIds)
                {
                    quantities[itemId] = (
                        usedByItem.ContainsKey(itemId) ? usedByItem[itemId] : 0,
                        reservedByItem.ContainsKey(itemId) ? reservedByItem[itemId] : 0
                    );
                }
            }
            
            return quantities;
        }

        public async Task<APIOperationResponse<bool>> BulkCreateAsync(BulkCreateAllowanceItemDto inputDto)
        {
            _logger.LogInformation("Bulk creating allowance items. DepartmentId: {DepartmentId}, Year: {Year}, ItemCount: {ItemCount}, User: {UserId}", 
                inputDto?.DepartmentId, inputDto?.Year, inputDto?.Items?.Count ?? 0, _currentUserService.UserId);
            
            try
            {
                // Check authorization: non-admin users can only create allowances for their own department
                var canViewAll = _currentUserService.IsSuperAdmin;
                if (!canViewAll)
                {
                    canViewAll = await _permissionService.HasPermissionAsync("AllowanceItemViewAllDepartments");
                }
                var userDepartmentId = _currentUserService.DepartmentId;

                if (!canViewAll)
                {
                    if (!userDepartmentId.HasValue)
                    {
                        _logger.LogWarning("User has no department assigned. UserId: {UserId}", _currentUserService.UserId);
                        return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You do not have permission to create allowances");
                    }

                    if (inputDto.DepartmentId != userDepartmentId.Value)
                    {
                        _logger.LogWarning("User attempted to bulk create allowances for different department. UserDepartmentId: {UserDeptId}, RequestedDepartmentId: {RequestedDeptId}, UserId: {UserId}", 
                            userDepartmentId.Value, inputDto.DepartmentId, _currentUserService.UserId);
                        return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You can only create allowances for your own department");
                    }
                }

                var bulkValidator = new BulkCreateAllowanceItemDtoValidator();
                var validationResult = await bulkValidator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning("Bulk allowance items validation failed. Errors: {ValidationErrors}, User: {UserId}", 
                        errors, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

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
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, $"Item {itemDto.ItemId}: {errors}");
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
                        updatedCount++;
                    }
                    else
                    {
                        // Create new
                        var allowanceItem = _mapper.Map<AllowanceItem>(createDto);
                        allowanceItem.CreationDate = DateTime.UtcNow;
                        allowanceItem.CreatedBy = _currentUserService.UserId;

                        await _allowanceItemRepository.AddAsync(allowanceItem);
                        createdCount++;
                    }
                }

                _logger.LogInformation("Bulk allowance items operation completed. DepartmentId: {DepartmentId}, Year: {Year}, Created: {CreatedCount}, Updated: {UpdatedCount}, User: {UserId}", 
                    inputDto.DepartmentId, inputDto.Year, createdCount, updatedCount, _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(true, "Allowance items created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in bulk create allowance items. DepartmentId: {DepartmentId}, Year: {Year}, User: {UserId}", 
                    inputDto?.DepartmentId, inputDto?.Year, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<AllowanceReserveDetailsDto>> GetReserveDetailsAsync(long departmentId, int year)
        {
            _logger.LogInformation("Getting reserve details. DepartmentId: {DepartmentId}, Year: {Year}, User: {UserId}", 
                departmentId, year, _currentUserService.UserId);
            
            try
            {
                // Check authorization: users with ViewAllDepartments permission or admin can access all departments
                var canViewAll = _currentUserService.IsSuperAdmin;
                if (!canViewAll)
                {
                    canViewAll = await _permissionService.HasPermissionAsync("AllowanceItemViewAllDepartments");
                }
                var userDepartmentId = _currentUserService.DepartmentId;

                if (!canViewAll)
                {
                    if (!userDepartmentId.HasValue)
                    {
                        _logger.LogWarning("User has no department assigned. UserId: {UserId}", _currentUserService.UserId);
                        return APIOperationResponse<AllowanceReserveDetailsDto>.Fail(ResponseType.Forbidden, "You do not have permission to access this department's reserve details");
                    }

                    if (departmentId != userDepartmentId.Value)
                    {
                        _logger.LogWarning("User attempted to access reserve details from different department. UserDepartmentId: {UserDeptId}, RequestedDepartmentId: {RequestedDeptId}, UserId: {UserId}", 
                            userDepartmentId.Value, departmentId, _currentUserService.UserId);
                        return APIOperationResponse<AllowanceReserveDetailsDto>.Fail(ResponseType.Forbidden, "You do not have permission to access this department's reserve details");
                    }
                }

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
                var totalOriginalQuantity = allowanceItems.Sum(a => a.Quantity);

                // Get all orders from allowance for this department and year
                var ordersFromAllowance = await _orderRepository.FindAsync(
                    o => o.DepartmentId == departmentId && 
                         o.IsFromAllowance && 
                         o.CreationDate.Year == year &&
                         !o.IsDeleted);
                
                var orderIds = ordersFromAllowance.Select(o => o.Id).ToList();
                int totalUsedQuantity = 0;
                int totalReservedQuantity = 0;

                if (orderIds.Any())
                {
                    // Get all supplies for these orders
                    var supplies = await _supplyRepository.FindAsync(
                        s => orderIds.Contains(s.OrderId) && !s.IsDeleted);

                    var draftSupplyIds = supplies
                        .Where(s => s.SubmissionStatus == SupplySubmissionStatus.Draft)
                        .Select(s => s.Id)
                        .ToList();

                    var submittedSupplyIds = supplies
                        .Where(s => s.SubmissionStatus == SupplySubmissionStatus.Submitted)
                        .Select(s => s.Id)
                        .ToList();
                    
                    var allSupplyIds = draftSupplyIds.Concat(submittedSupplyIds).ToList();
                    
                    if (allSupplyIds.Any())
                    {
                        var supplyDetails = await _supplyDetailRepository.FindAsync(
                            sd => allSupplyIds.Contains(sd.SupplyId) && !sd.IsDeleted);

                        // Calculate Used (Submitted)
                        totalUsedQuantity = (int)supplyDetails
                            .Where(sd => submittedSupplyIds.Contains(sd.SupplyId))
                            .Sum(sd => sd.Quantity);

                        // Calculate Reserved (Draft)
                        totalReservedQuantity = (int)supplyDetails
                            .Where(sd => draftSupplyIds.Contains(sd.SupplyId))
                            .Sum(sd => sd.Quantity);
                    }
                }

                // Calculate remaining
                var totalRemainingQuantity = totalOriginalQuantity - totalUsedQuantity - totalReservedQuantity;

                var result = new AllowanceReserveDetailsDto
                {
                    DepartmentId = departmentId,
                    Year = year,
                    TotalOriginalQuantity = totalOriginalQuantity,
                    TotalRemainingQuantity = Math.Max(0, totalRemainingQuantity),
                    TotalReservedQuantityByOrdersOnProcessing = totalReservedQuantity,
                    TotalUsedQuantity = totalUsedQuantity
                };

                _logger.LogInformation("Reserve details calculated. DepartmentId: {DepartmentId}, Year: {Year}, Original: {Original}, Remaining: {Remaining}, Reserved: {Reserved}, Used: {Used}", 
                    departmentId, year, totalOriginalQuantity, totalRemainingQuantity, totalReservedQuantity, totalUsedQuantity);

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
                // Check authorization: users with ViewAllDepartments permission or admin can access all departments
                var canViewAll = _currentUserService.IsSuperAdmin;
                if (!canViewAll)
                {
                    canViewAll = await _permissionService.HasPermissionAsync("AllowanceItemViewAllDepartments");
                }
                var userDepartmentId = _currentUserService.DepartmentId;

                if (!canViewAll)
                {
                    if (!userDepartmentId.HasValue)
                    {
                        _logger.LogWarning("User has no department assigned. UserId: {UserId}", _currentUserService.UserId);
                        return APIOperationResponse<AllowanceReserveDetailsByItemDto>.Fail(ResponseType.Forbidden, "You do not have permission to access this department's reserve details");
                    }

                    if (departmentId != userDepartmentId.Value)
                    {
                        _logger.LogWarning("User attempted to access reserve details from different department. UserDepartmentId: {UserDeptId}, RequestedDepartmentId: {RequestedDeptId}, UserId: {UserId}", 
                            userDepartmentId.Value, departmentId, _currentUserService.UserId);
                        return APIOperationResponse<AllowanceReserveDetailsByItemDto>.Fail(ResponseType.Forbidden, "You do not have permission to access this department's reserve details");
                    }
                }

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
                        TotalOriginalQuantity = 0,
                        TotalRemainingQuantity = 0,
                        TotalReservedQuantityByOrdersOnProcessing = 0,
                        TotalUsedQuantity = 0,
                        Items = new List<AllowanceItemReserveDetailsDto>()
                    });
                }

                // Get all orders from allowance for this department and year
                var ordersFromAllowance = await _orderRepository.FindAsync(
                    o => o.DepartmentId == departmentId && 
                         o.IsFromAllowance && 
                         o.CreationDate.Year == year &&
                         !o.IsDeleted);

                var orderIds = ordersFromAllowance.Select(o => o.Id).ToList();
                
                // Prepare dictionaries for quantities
                var usedQuantityByItem = new Dictionary<long, int>();
                var reservedQuantityByItem = new Dictionary<long, int>();

                if (orderIds.Any())
                {
                    var supplies = await _supplyRepository.FindAsync(
                        s => orderIds.Contains(s.OrderId) && !s.IsDeleted);

                    var draftSupplyIds = supplies.Where(s => s.SubmissionStatus == SupplySubmissionStatus.Draft).Select(s => s.Id).ToList();
                    var submittedSupplyIds = supplies.Where(s => s.SubmissionStatus == SupplySubmissionStatus.Submitted).Select(s => s.Id).ToList();
                    var allSupplyIds = draftSupplyIds.Concat(submittedSupplyIds).ToList();

                    if (allSupplyIds.Any())
                    {
                        var supplyDetails = await _supplyDetailRepository.FindAsync(
                            sd => allSupplyIds.Contains(sd.SupplyId) && !sd.IsDeleted);

                        // Group by ItemId
                        usedQuantityByItem = supplyDetails
                            .Where(sd => submittedSupplyIds.Contains(sd.SupplyId))
                            .GroupBy(sd => sd.ItemId)
                            .ToDictionary(g => g.Key, g => (int)g.Sum(sd => sd.Quantity));

                        reservedQuantityByItem = supplyDetails
                            .Where(sd => draftSupplyIds.Contains(sd.SupplyId))
                            .GroupBy(sd => sd.ItemId)
                            .ToDictionary(g => g.Key, g => (int)g.Sum(sd => sd.Quantity));
                    }
                }

                // Calculate per-item details
                var itemDetailsList = new List<AllowanceItemReserveDetailsDto>();
                int totalOriginal = 0;
                int totalRemaining = 0;
                int totalReserved = 0;
                int totalUsed = 0;

                foreach (var allowanceItem in allowanceItems)
                {
                    var itemId = allowanceItem.ItemId;
                    var itemOriginalQuantity = allowanceItem.Quantity;
                    var itemUsedQuantity = usedQuantityByItem.TryGetValue(itemId, out var used) ? used : 0;
                    var itemReservedQuantity = reservedQuantityByItem.TryGetValue(itemId, out var reserved) ? reserved : 0;
                    var itemRemainingQuantity = Math.Max(0, itemOriginalQuantity - itemUsedQuantity - itemReservedQuantity);

                    // Get item details
                    var item = allowanceItem.Item;
                    var itemName = item?.Name ?? "Unknown Item";
                    var itemNo = item?.ItemNo ?? "";
                    // BatchNo is now on InventoryDetail, not BaseItem - set to null for allowance items
                    string? batchNo = null;

                    itemDetailsList.Add(new AllowanceItemReserveDetailsDto
                    {
                        ItemId = itemId,
                        ItemName = itemName,
                        ItemNo = itemNo,
                        BatchNo = batchNo,
                        OriginalQuantity = itemOriginalQuantity,
                        RemainingQuantity = itemRemainingQuantity,
                        ReservedQuantityByOrdersOnProcessing = itemReservedQuantity,
                        UsedQuantity = itemUsedQuantity
                    });

                    // Accumulate totals
                    totalOriginal += itemOriginalQuantity;
                    totalRemaining += itemRemainingQuantity;
                    totalReserved += itemReservedQuantity;
                    totalUsed += itemUsedQuantity;
                }

                var result = new AllowanceReserveDetailsByItemDto
                {
                    DepartmentId = departmentId,
                    DepartmentCode = department.Code,
                    DepartmentNameAr = department.NameAr,
                    DepartmentNameEn = department.NameEn,
                    Year = year,
                    TotalOriginalQuantity = totalOriginal,
                    TotalRemainingQuantity = totalRemaining,
                    TotalReservedQuantityByOrdersOnProcessing = totalReserved,
                    TotalUsedQuantity = totalUsed,
                    Items = itemDetailsList
                };

                _logger.LogInformation("Reserve details by item calculated. DepartmentId: {DepartmentId}, Year: {Year}, ItemCount: {ItemCount}, TotalOriginal: {Total}, TotalRemaining: {Remaining}", 
                    departmentId, year, itemDetailsList.Count, totalOriginal, totalRemaining);

                return APIOperationResponse<AllowanceReserveDetailsByItemDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting reserve details by item. DepartmentId: {DepartmentId}, Year: {Year}, User: {UserId}", 
                    departmentId, year, _currentUserService.UserId);
                return APIOperationResponse<AllowanceReserveDetailsByItemDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Populates calculated quantities (UsedQuantityFromAllowance, ReservedQuantityByOrdersUnderProccessing, RemainingQuantity) for an AllowanceItemDto
        /// </summary>
        private async Task PopulateCalculatedQuantitiesAsync(AllowanceItemDto dto, AllowanceItem allowanceItem)
        {
            try
            {
                // Get all orders from this department and year
                var orders = await _orderRepository.FindAsync(
                    o => o.DepartmentId == allowanceItem.DepartmentId &&
                         o.CreationDate.Year == allowanceItem.Year &&
                         !o.IsDeleted);

                var orderIds = orders.Select(o => o.Id).ToList();

                if (!orderIds.Any())
                {
                    dto.UsedQuantityFromAllowance = 0;
                    dto.ReservedQuantityByOrdersOnProcessing = 0;
                    dto.RemainingQuantityFromAllowance = allowanceItem.Quantity;
                    return;
                }

                // Get all supplies for orders that have supplies (only consider orders with supplies)
                var supplies = await _supplyRepository.FindAsync(
                    s => orderIds.Contains(s.OrderId) && !s.IsDeleted);

                // Separate Draft and Submitted supplies
                var draftSupplyIds = supplies
                    .Where(s => s.SubmissionStatus == SupplySubmissionStatus.Draft)
                    .Select(s => s.Id)
                    .ToList();

                var submittedSupplyIds = supplies
                    .Where(s => s.SubmissionStatus == SupplySubmissionStatus.Submitted)
                    .Select(s => s.Id)
                    .ToList();

                // Calculate UsedQuantityFromAllowance from Submitted supplies
                if (submittedSupplyIds.Any())
                {
                    var submittedSupplyDetails = await _supplyDetailRepository.FindAsync(
                        sd => submittedSupplyIds.Contains(sd.SupplyId) &&
                              sd.ItemId == allowanceItem.ItemId &&
                              !sd.IsDeleted);

                    dto.UsedQuantityFromAllowance = (int)submittedSupplyDetails.Sum(sd => sd.Quantity);
                }
                else
                {
                    dto.UsedQuantityFromAllowance = 0;
                }

                // Calculate ReservedQuantityByOrdersUnderProccessing from Draft supplies
                if (draftSupplyIds.Any())
                {
                    var draftSupplyDetails = await _supplyDetailRepository.FindAsync(
                        sd => draftSupplyIds.Contains(sd.SupplyId) &&
                              sd.ItemId == allowanceItem.ItemId &&
                              !sd.IsDeleted);

                    dto.ReservedQuantityByOrdersOnProcessing = (int)draftSupplyDetails.Sum(sd => sd.Quantity);
                }
                else
                {
                    dto.ReservedQuantityByOrdersOnProcessing = 0;
                }

                // Calculate RemainingQuantity
                dto.RemainingQuantityFromAllowance = Math.Max(0, allowanceItem.Quantity - dto.UsedQuantityFromAllowance - dto.ReservedQuantityByOrdersOnProcessing);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating quantities for AllowanceItem. Id: {Id}, ItemId: {ItemId}, DepartmentId: {DepartmentId}, Year: {Year}",
                    allowanceItem.Id, allowanceItem.ItemId, allowanceItem.DepartmentId, allowanceItem.Year);
                
                // Set defaults on error
                dto.UsedQuantityFromAllowance = 0;
                dto.ReservedQuantityByOrdersOnProcessing = 0;
                dto.RemainingQuantityFromAllowance = allowanceItem.Quantity;
            }
        }
    }
}
