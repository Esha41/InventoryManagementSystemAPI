using AutoMapper;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.ItemDepartmentAssignments.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Inventory.Service.ItemDepartmentAssignments.Interfaces;

namespace Ettad.Inventory.Service.ItemDepartmentAssignments.Services
{
    public class ItemDepartmentAssignmentService : IItemDepartmentAssignmentService
    {
        private readonly ICrossCuttingRepository<ItemDepartmentAssignment> _assignmentRepository;
        private readonly ICrossCuttingRepository<BaseItem> _itemRepository;
        private readonly ICrossCuttingRepository<Department> _departmentRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ItemDepartmentAssignmentService> _logger;

        public ItemDepartmentAssignmentService(
            ICrossCuttingRepository<ItemDepartmentAssignment> assignmentRepository,
            ICrossCuttingRepository<BaseItem> itemRepository,
            ICrossCuttingRepository<Department> departmentRepository,
            IMapper mapper,
            ICurrentUserService currentUserService,
            ILogger<ItemDepartmentAssignmentService> logger)
        {
            _assignmentRepository = assignmentRepository;
            _itemRepository = itemRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<APIOperationResponse<ItemDepartmentAssignmentDto>> GetByIdAsync(long id)
        {
            try
            {
                var assignment = await _assignmentRepository.FindOneAsync(
                    x => x.Id == id,
                    false,
                    nameof(ItemDepartmentAssignment.Item),
                    nameof(ItemDepartmentAssignment.Department));

                if (assignment == null)
                    return APIOperationResponse<ItemDepartmentAssignmentDto>.Fail(ResponseType.NotFound, "Assignment not found");

                var dto = await MapToDtoAsync(assignment);
                return APIOperationResponse<ItemDepartmentAssignmentDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving assignment by ID. Id: {Id}", id);
                return APIOperationResponse<ItemDepartmentAssignmentDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<ItemDepartmentAssignmentDto>>> GetAllAsync()
        {
            try
            {
                var assignments = await _assignmentRepository.FindAsync(
                    x => true,
                    false,
                    nameof(ItemDepartmentAssignment.Item),
                    nameof(ItemDepartmentAssignment.Department));

                var dtos = new List<ItemDepartmentAssignmentDto>();
                foreach (var assignment in assignments)
                {
                    dtos.Add(await MapToDtoAsync(assignment));
                }

                return APIOperationResponse<List<ItemDepartmentAssignmentDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all assignments");
                return APIOperationResponse<List<ItemDepartmentAssignmentDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<ItemDepartmentAssignmentDto>>> GetByDepartmentIdAsync(long departmentId)
        {
            try
            {
                var assignments = await _assignmentRepository.FindAsync(
                    x => x.DepartmentId == departmentId,
                    false,
                    nameof(ItemDepartmentAssignment.Item),
                    nameof(ItemDepartmentAssignment.Department));

                var dtos = new List<ItemDepartmentAssignmentDto>();
                foreach (var assignment in assignments)
                {
                    dtos.Add(await MapToDtoAsync(assignment));
                }

                return APIOperationResponse<List<ItemDepartmentAssignmentDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving assignments by department. DepartmentId: {DepartmentId}", departmentId);
                return APIOperationResponse<List<ItemDepartmentAssignmentDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<ItemDepartmentAssignmentDto>>> GetByItemIdAsync(long itemId)
        {
            try
            {
                var assignments = await _assignmentRepository.FindAsync(
                    x => x.ItemId == itemId,
                    false,
                    nameof(ItemDepartmentAssignment.Item),
                    nameof(ItemDepartmentAssignment.Department));

                var dtos = new List<ItemDepartmentAssignmentDto>();
                foreach (var assignment in assignments)
                {
                    dtos.Add(await MapToDtoAsync(assignment));
                }

                return APIOperationResponse<List<ItemDepartmentAssignmentDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving assignments by item. ItemId: {ItemId}", itemId);
                return APIOperationResponse<List<ItemDepartmentAssignmentDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateUpdateItemDepartmentAssignmentDto inputDto)
        {
            try
            {
                // Check if assignment already exists
                var existing = await _assignmentRepository.FindOneAsync(
                    x => x.ItemId == inputDto.ItemId && 
                         x.DepartmentId == inputDto.DepartmentId);

                if (existing != null)
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "This item is already assigned to this department");

                // Verify item exists
                var item = await _itemRepository.FindOneAsync(x => x.Id == inputDto.ItemId && !x.IsDeleted);
                if (item == null)
                    return APIOperationResponse<long>.Fail(ResponseType.NotFound, "Item not found");

                // Verify department exists
                var department = await _departmentRepository.FindOneAsync(x => x.Id == inputDto.DepartmentId && !x.IsDeleted);
                if (department == null)
                    return APIOperationResponse<long>.Fail(ResponseType.NotFound, "Department not found");

                var assignment = new ItemDepartmentAssignment
                {
                    ItemId = inputDto.ItemId,
                    DepartmentId = inputDto.DepartmentId,
                    Notes = inputDto.Notes,
                    CreatedBy = _currentUserService.UserId
                };

                await _assignmentRepository.AddAsync(assignment);

                _logger.LogInformation("Created item-department assignment. ItemId: {ItemId}, DepartmentId: {DepartmentId}, User: {UserId}",
                    inputDto.ItemId, inputDto.DepartmentId, _currentUserService.UserId);

                return APIOperationResponse<long>.Success(assignment.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating assignment. ItemId: {ItemId}, DepartmentId: {DepartmentId}",
                    inputDto.ItemId, inputDto.DepartmentId);
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateItemDepartmentAssignmentDto inputDto)
        {
            try
            {
                var assignment = await _assignmentRepository.FindOneAsync(x => x.Id == id);
                if (assignment == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Assignment not found");

                // Check if another assignment with same item-department combination exists
                var existing = await _assignmentRepository.FindOneAsync(
                    x => x.ItemId == inputDto.ItemId && 
                         x.DepartmentId == inputDto.DepartmentId && 
                         x.Id != id);

                if (existing != null)
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "This item is already assigned to this department");

                // Verify item exists
                var item = await _itemRepository.FindOneAsync(x => x.Id == inputDto.ItemId && !x.IsDeleted);
                if (item == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Item not found");

                // Verify department exists
                var department = await _departmentRepository.FindOneAsync(x => x.Id == inputDto.DepartmentId && !x.IsDeleted);
                if (department == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Department not found");

                assignment.ItemId = inputDto.ItemId;
                assignment.DepartmentId = inputDto.DepartmentId;
                assignment.Notes = inputDto.Notes;
                assignment.ModifiedBy = _currentUserService.UserId;
                assignment.ModificationDate = DateTime.UtcNow;

                await _assignmentRepository.UpdateAsync(assignment);

                _logger.LogInformation("Updated item-department assignment. Id: {Id}, User: {UserId}", id, _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating assignment. Id: {Id}", id);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            try
            {
                var assignment = await _assignmentRepository.FindOneAsync(x => x.Id == id);
                if (assignment == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Assignment not found");

                await _assignmentRepository.DeleteAsync(assignment);

                _logger.LogInformation("Deleted item-department assignment. Id: {Id}, User: {UserId}", id, _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting assignment. Id: {Id}", id);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> BulkAssignAsync(List<CreateUpdateItemDepartmentAssignmentDto> assignments)
        {
            try
            {
                var createdCount = 0;
                var skippedCount = 0;

                foreach (var dto in assignments)
                {
                    // Check if assignment already exists
                    var existing = await _assignmentRepository.FindOneAsync(
                        x => x.ItemId == dto.ItemId && 
                             x.DepartmentId == dto.DepartmentId);

                    if (existing != null)
                    {
                        skippedCount++;
                        continue;
                    }

                    // Verify item exists
                    var item = await _itemRepository.FindOneAsync(x => x.Id == dto.ItemId && !x.IsDeleted);
                    if (item == null)
                    {
                        skippedCount++;
                        continue;
                    }

                    // Verify department exists
                    var department = await _departmentRepository.FindOneAsync(x => x.Id == dto.DepartmentId && !x.IsDeleted);
                    if (department == null)
                    {
                        skippedCount++;
                        continue;
                    }

                    var assignment = new ItemDepartmentAssignment
                    {
                        ItemId = dto.ItemId,
                        DepartmentId = dto.DepartmentId,
                        Notes = dto.Notes,
                        CreatedBy = _currentUserService.UserId
                    };

                    await _assignmentRepository.AddAsync(assignment);
                    createdCount++;
                }

                _logger.LogInformation("Bulk assigned items to departments. Created: {Created}, Skipped: {Skipped}, User: {UserId}",
                    createdCount, skippedCount, _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk assigning items to departments");
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<DepartmentAssignmentSummaryDto>>> GetDepartmentSummariesAsync()
        {
            try
            {
                var assignments = await _assignmentRepository.FindAsync(
                    x => true,
                    false,
                    nameof(ItemDepartmentAssignment.Item),
                    nameof(ItemDepartmentAssignment.Department));

                var grouped = assignments
                    .GroupBy(a => a.DepartmentId)
                    .Select(g =>
                    {
                        var first = g.First();
                        var dept = first.Department;
                        if (dept == null)
                            return null;

                        int ammo = 0, weapons = 0, explosives = 0;
                        foreach (var a in g)
                        {
                            var item = a.Item;
                            if (item == null) continue;
                            switch (item.ItemType)
                            {
                                case ItemType.Ammunition: ammo++; break;
                                case ItemType.Weapon: weapons++; break;
                                case ItemType.Explosive: explosives++; break;
                                default: break;
                            }
                        }

                        return new DepartmentAssignmentSummaryDto
                        {
                            DepartmentId = first.DepartmentId,
                            DepartmentCode = dept.Code,
                            DepartmentNameAr = dept.NameAr,
                            DepartmentNameEn = dept.NameEn,
                            AmmunitionCount = ammo,
                            ExplosivesCount = explosives,
                            WeaponsCount = weapons
                        };
                    })
                    .Where(x => x != null)
                    .Cast<DepartmentAssignmentSummaryDto>()
                    .OrderBy(x => x.DepartmentNameEn ?? x.DepartmentNameAr ?? "")
                    .ToList();

                return APIOperationResponse<List<DepartmentAssignmentSummaryDto>>.Success(grouped);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving department assignment summaries");
                return APIOperationResponse<List<DepartmentAssignmentSummaryDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private async Task<ItemDepartmentAssignmentDto> MapToDtoAsync(ItemDepartmentAssignment assignment)
        {
            var dto = new ItemDepartmentAssignmentDto
            {
                Id = assignment.Id,
                ItemId = assignment.ItemId,
                DepartmentId = assignment.DepartmentId,
                Notes = assignment.Notes
            };

            // Load item details if not loaded
            if (assignment.Item == null)
            {
                assignment.Item = await _itemRepository.FindOneAsync(x => x.Id == assignment.ItemId);
            }

            if (assignment.Item != null)
            {
                dto.ItemName = assignment.Item.Name;
                dto.ItemNo = assignment.Item.ItemNo;
                dto.ItemType = assignment.Item.ItemType;
            }

            // Load department details if not loaded
            if (assignment.Department == null)
            {
                assignment.Department = await _departmentRepository.FindOneAsync(x => x.Id == assignment.DepartmentId);
            }

            if (assignment.Department != null)
            {
                dto.DepartmentCode = assignment.Department.Code;
                dto.DepartmentNameAr = assignment.Department.NameAr;
                dto.DepartmentNameEn = assignment.Department.NameEn;
            }

            return dto;
        }
    }
}
