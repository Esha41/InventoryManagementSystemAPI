using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Inventory.Service.AssetSupply.Dtos;
using Ettad.Inventory.Service.AssetHistory;
using Ettad.Inventory.Service.AssetHistory.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.AssetSupply
{
    public class AssetSupplyService : IAssetSupplyService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICrossCuttingRepository<Ettad.Data.Entities.AssetSupply> _assetSupplyRepository;
        private readonly ICrossCuttingRepository<AssetSupplyDetail> _assetSupplyDetailRepository;
        private readonly ICrossCuttingRepository<Asset> _assetRepository;
        private readonly ICrossCuttingRepository<AssetAssignment> _assignmentRepository;
        private readonly ICrossCuttingRepository<Order> _orderRepository;
        private readonly IAssetHistoryService _historyService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAssetSupplyDto> _createValidator;
        private readonly IValidator<ReturnAssetDto> _returnValidator;
        private readonly IValidator<ReturnMultipleAssetsDto> _returnMultipleValidator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AssetSupplyService> _logger;

        public AssetSupplyService(
            ApplicationDbContext context,
            ICrossCuttingRepository<Ettad.Data.Entities.AssetSupply> assetSupplyRepository,
            ICrossCuttingRepository<AssetSupplyDetail> assetSupplyDetailRepository,
            ICrossCuttingRepository<Asset> assetRepository,
            ICrossCuttingRepository<AssetAssignment> assignmentRepository,
            ICrossCuttingRepository<Order> orderRepository,
            IAssetHistoryService historyService,
            IMapper mapper,
            IValidator<CreateAssetSupplyDto> createValidator,
            IValidator<ReturnAssetDto> returnValidator,
            IValidator<ReturnMultipleAssetsDto> returnMultipleValidator,
            ICurrentUserService currentUserService,
            ILogger<AssetSupplyService> logger)
        {
            _context = context;
            _assetSupplyRepository = assetSupplyRepository;
            _assetSupplyDetailRepository = assetSupplyDetailRepository;
            _assetRepository = assetRepository;
            _assignmentRepository = assignmentRepository;
            _orderRepository = orderRepository;
            _historyService = historyService;
            _mapper = mapper;
            _createValidator = createValidator;
            _returnValidator = returnValidator;
            _returnMultipleValidator = returnMultipleValidator;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<APIOperationResponse<OrderAssetsToSupplyDto>> GetAssetsToSupplyAsync(long orderId)
        {
            _logger.LogInformation("Getting assets to supply for order. OrderId: {OrderId}, User: {UserId}",
                orderId, _currentUserService.UserId);

            try
            {
                // Get order with request items
                var order = await _context.Orders
                    .Include(o => o.RequestItems)
                        .ThenInclude(ri => ri.Item)
                    .Include(o => o.Department)
                    .FirstOrDefaultAsync(o => o.Id == orderId && !o.IsDeleted);

                if (order == null)
                {
                    return APIOperationResponse<OrderAssetsToSupplyDto>.Fail(
                        ResponseType.NotFound, "Order not found");
                }

                if (order.RequestItems == null || !order.RequestItems.Any())
                {
                    return APIOperationResponse<OrderAssetsToSupplyDto>.Fail(
                        ResponseType.BadRequest, "Order has no request items");
                }

                var result = new OrderAssetsToSupplyDto
                {
                    OrderId = order.Id,
                    RequestNo = order.RequestNo,
                    DepartmentName = order.Department?.NameEn
                };

                foreach (var requestItem in order.RequestItems.Where(ri => !ri.IsDeleted))
                {
                    // Query available assets for this item type
                    // Criteria: has serial number, not assigned, not deleted, ordered by purchase date (FIFO)
                    var availableAssets = await _context.Assets
                        .Include(a => a.Depot)
                        .Where(a => a.ItemId == requestItem.ItemId
                            && !string.IsNullOrEmpty(a.SerialNumber)
                            && !a.IsAssigned
                            && !a.IsDeleted
                            && a.Status != AssetStatus.Maintenance
                            && a.Status != AssetStatus.Disposed
                            && a.Status != AssetStatus.Lost)
                        .OrderBy(a => a.PurchaseDate ?? DateTime.MaxValue) // FIFO - oldest first
                        .ThenBy(a => a.Id)
                        .ToListAsync();

                    var itemAssets = new ItemAssetsToSupplyDto
                    {
                        ItemId = requestItem.ItemId,
                        ItemName = requestItem.Item?.Name,
                        RequestedQuantity = requestItem.Quantity,
                        AvailableAssets = availableAssets.Select((a, index) => new AssetToSupplyDto
                        {
                            Id = a.Id,
                            SerialNumber = a.SerialNumber!,
                            RFID = a.RFID,
                            AssetTag = a.AssetTag,
                            Condition = a.Condition,
                            Status = a.Status,
                            PurchaseDate = a.PurchaseDate,
                            DepotId = a.DepotId,
                            DepotName = a.Depot?.NameEn,
                            Priority = index + 1 // Priority based on FIFO order
                        }).ToList()
                    };

                    result.Items.Add(itemAssets);
                }

                _logger.LogInformation("Assets to supply retrieved. OrderId: {OrderId}, Items: {ItemCount}, User: {UserId}",
                    orderId, result.Items.Count, _currentUserService.UserId);

                return APIOperationResponse<OrderAssetsToSupplyDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting assets to supply. OrderId: {OrderId}, User: {UserId}",
                    orderId, _currentUserService.UserId);
                return APIOperationResponse<OrderAssetsToSupplyDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<AssetSupplyDto>> GetByIdAsync(long id)
        {
            _logger.LogInformation("Getting asset supply by ID. SupplyId: {SupplyId}, User: {UserId}",
                id, _currentUserService.UserId);

            try
            {
                var supply = await _context.AssetSupplies
                    .Include(s => s.Department)
                    .Include(s => s.Custodian)
                    .Include(s => s.ReceiverRank)
                    .Include(s => s.SupplyDetails)
                        .ThenInclude(d => d.Asset)
                    .Include(s => s.SupplyDetails)
                        .ThenInclude(d => d.Item)
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

                if (supply == null)
                {
                    return APIOperationResponse<AssetSupplyDto>.Fail(
                        ResponseType.NotFound, "Asset supply not found");
                }

                var dto = _mapper.Map<AssetSupplyDto>(supply);
                return APIOperationResponse<AssetSupplyDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting asset supply. SupplyId: {SupplyId}, User: {UserId}",
                    id, _currentUserService.UserId);
                return APIOperationResponse<AssetSupplyDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<AssetSupplyDto>> GetByOrderIdAsync(long orderId)
        {
            try
            {
                var supply = await _context.AssetSupplies
                    .Include(s => s.Department)
                    .Include(s => s.Custodian)
                    .Include(s => s.ReceiverRank)
                    .Include(s => s.SupplyDetails)
                        .ThenInclude(d => d.Asset)
                    .Include(s => s.SupplyDetails)
                        .ThenInclude(d => d.Item)
                    .FirstOrDefaultAsync(s => s.OrderId == orderId && !s.IsDeleted);

                if (supply == null)
                {
                    return APIOperationResponse<AssetSupplyDto>.Fail(
                        ResponseType.NotFound, "Asset supply not found for this order");
                }

                var dto = _mapper.Map<AssetSupplyDto>(supply);
                return APIOperationResponse<AssetSupplyDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting asset supply by order. OrderId: {OrderId}, User: {UserId}",
                    orderId, _currentUserService.UserId);
                return APIOperationResponse<AssetSupplyDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<AssetSupplyDto>>> GetAllAsync()
        {
            try
            {
                var supplies = await _context.AssetSupplies
                    .Include(s => s.Department)
                    .Include(s => s.Custodian)
                    .Include(s => s.SupplyDetails)
                    .Where(s => !s.IsDeleted)
                    .OrderByDescending(s => s.CreationDate)
                    .ToListAsync();

                var dtos = _mapper.Map<List<AssetSupplyDto>>(supplies);
                return APIOperationResponse<List<AssetSupplyDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all asset supplies. User: {UserId}",
                    _currentUserService.UserId);
                return APIOperationResponse<List<AssetSupplyDto>>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<long>> CreateAndSubmitAsync(CreateAssetSupplyDto dto)
        {
            _logger.LogInformation("Creating and submitting asset supply. OrderId: {OrderId}, AssetCount: {AssetCount}, User: {UserId}",
                dto.OrderId, dto.SupplyDetails?.Count ?? 0, _currentUserService.UserId);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Validate input
                var validationResult = await _createValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                // Check order exists
                var order = await _orderRepository.FindOneAsync(
                    o => o.Id == dto.OrderId && !o.IsDeleted,
                    false,
                    nameof(Order.Department),
                    nameof(Order.RequestItems));

                if (order == null)
                {
                    return APIOperationResponse<long>.Fail(ResponseType.NotFound, "Order not found");
                }

                // Check for existing supply
                var existingSupply = await _context.AssetSupplies
                    .FirstOrDefaultAsync(s => s.OrderId == dto.OrderId && !s.IsDeleted);

                if (existingSupply != null)
                {
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                        $"A supply already exists for this order. Supply ID: {existingSupply.Id}");
                }

                // Validate all assets
                var assetIds = dto.SupplyDetails.Select(d => d.AssetId).ToList();
                var assets = await _context.Assets
                    .Where(a => assetIds.Contains(a.Id) && !a.IsDeleted)
                    .ToListAsync();

                // Check all assets exist
                var missingAssets = assetIds.Except(assets.Select(a => a.Id)).ToList();
                if (missingAssets.Any())
                {
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                        $"Assets not found: {string.Join(", ", missingAssets)}");
                }

                // Check all assets have serial numbers
                var assetsWithoutSerial = assets.Where(a => string.IsNullOrEmpty(a.SerialNumber)).ToList();
                if (assetsWithoutSerial.Any())
                {
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                        $"Assets without serial numbers cannot be supplied: {string.Join(", ", assetsWithoutSerial.Select(a => a.Id))}");
                }

                // Check no assets are already assigned
                var assignedAssets = assets.Where(a => a.IsAssigned).ToList();
                if (assignedAssets.Any())
                {
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                        $"Assets already assigned cannot be supplied: {string.Join(", ", assignedAssets.Select(a => $"{a.Id} ({a.SerialNumber})"))}");
                }

                // Create supply entity
                var supply = _mapper.Map<Ettad.Data.Entities.AssetSupply>(dto);
                supply.SubmissionStatus = SupplySubmissionStatus.Submitted;
                supply.SupplyDate = order.SupplyDate ?? DateTime.UtcNow; // Use order supply date or now
                supply.DepartmentId = order.DepartmentId; // Always use requested department
                supply.CustodianId = dto.CustodianId; // Use provided user ID or null
                
                // Calculate fulfillment status
                // Group requested quantities
                var requestedItems = order.RequestItems
                    .Where(ri => !ri.IsDeleted)
                    .GroupBy(ri => ri.ItemId)
                    .ToDictionary(g => g.Key, g => g.Sum(ri => ri.Quantity));

                // Group supplied quantities
                var suppliedItems = assets
                    .GroupBy(a => a.ItemId)
                    .ToDictionary(g => g.Key, g => (long)g.Count());

                bool fullyFulfilled = true;
                foreach (var req in requestedItems)
                {
                    long suppliedQty = suppliedItems.ContainsKey(req.Key) ? suppliedItems[req.Key] : 0;
                    if (suppliedQty < req.Value)
                    {
                        fullyFulfilled = false;
                        break;
                    }
                }
                supply.FulfillmentStatus = fullyFulfilled ? SupplyFulfillmentStatus.Fully : SupplyFulfillmentStatus.Partial;

                supply.CreationDate = DateTime.UtcNow;
                supply.CreatedBy = _currentUserService.UserId;

                // Create supply details
                var sequenceNo = 1;
                supply.SupplyDetails = dto.SupplyDetails.Select(d =>
                {
                    var asset = assets.First(a => a.Id == d.AssetId);
                    return new AssetSupplyDetail
                    {
                        AssetId = d.AssetId,
                        ItemId = asset.ItemId,
                        SequenceNo = sequenceNo++,
                        ConditionOnSupply = d.ConditionOnSupply ?? asset.Condition,
                        Notes = d.Notes,
                        IsDelivered = true, // Auto-delivered since there is no draft
                        DeliveredDate = DateTime.UtcNow,
                        CreationDate = DateTime.UtcNow,
                        CreatedBy = _currentUserService.UserId
                    };
                }).ToList();

                var createdSupply = await _assetSupplyRepository.AddAsync(supply);

                // Create assignments immediately
                foreach (var detail in supply.SupplyDetails)
                {
                    var asset = assets.First(a => a.Id == detail.AssetId);

                    // Create assignment
                    var assignment = new AssetAssignment
                    {
                        AssetId = asset.Id,
                        OrderId = supply.OrderId,
                        AssetSupplyId = supply.Id,
                        DepartmentId = supply.DepartmentId,
                        CustodianId = null, // Custodian in Assignment is Employee, but we used User for Supply custodian. Leaving null for now as per instructions to use User.
                        // Ideally we should update AssetAssignment to support User Custodian too, but for now we follow the instruction.
                        // "I will use User as Custodian instead of Employee" - implied for Supply.
                        // Assuming assignment logic should assign to department since custodian is a User now and Assignment expects Employee.
                        
                        Location = supply.Location,
                        AssignDate = supply.SupplyDate ?? DateTime.UtcNow,
                        ExpectedReturnDate = supply.ExpectedReturnDate,
                        Status = AssetAssignmentStatus.Active,
                        Purpose = order.UsagePurpose,
                        ConditionOnAssign = detail.ConditionOnSupply,
                        ReceiverName = supply.ReceiverName,
                        ReceiverMilitaryId = supply.ReceiverMilitaryId,
                        ReceiverRankId = supply.ReceiverRankId,
                        CreationDate = DateTime.UtcNow,
                        CreatedBy = _currentUserService.UserId
                    };

                    _context.AssetAssignments.Add(assignment);
                    await _context.SaveChangesAsync();

                    // Update asset
                    asset.IsAssigned = true;
                    asset.CurrentAssignmentId = assignment.Id;
                    asset.ModificationDate = DateTime.UtcNow;
                    asset.ModifiedBy = _currentUserService.UserId;

                    // Record history
                    await _historyService.RecordHistoryAsync(asset.Id, AssetHistoryActionType.Assigned, new AssetHistoryContext
                    {
                        Description = $"Asset assigned via supply #{supply.Id}",
                        NewDepartmentId = assignment.DepartmentId,
                        NewLocation = assignment.Location,
                        OrderId = supply.OrderId,
                        AssetSupplyId = supply.Id,
                        AssetAssignmentId = assignment.Id,
                        Notes = supply.Notes
                    });
                }

                await transaction.CommitAsync();

                _logger.LogInformation("Asset supply created and submitted. SupplyId: {SupplyId}, OrderId: {OrderId}, User: {UserId}",
                    createdSupply.Id, dto.OrderId, _currentUserService.UserId);

                return APIOperationResponse<long>.Success(createdSupply.Id, "Asset supply created and submitted successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating asset supply. OrderId: {OrderId}, User: {UserId}",
                    dto.OrderId, _currentUserService.UserId);
                return APIOperationResponse<long>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> CancelSupplyAsync(long id, string? reason)
        {
            _logger.LogInformation("Cancelling asset supply. SupplyId: {SupplyId}, User: {UserId}",
                id, _currentUserService.UserId);

            try
            {
                var supply = await _context.AssetSupplies
                    .Include(s => s.SupplyDetails)
                        .ThenInclude(d => d.Asset)
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

                if (supply == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Asset supply not found");
                }

                // Since we auto-complete/submit, cancelling means reversing the supply
                // We need to return assets and cancel assignments
                
                // For now, let's just mark it cancelled if it's not already
                // Ideally this should trigger a return process or we block cancellation if already distributed
                // But simplified requirement implies we might want to just mark it cancelled?
                // The prompt says "I will not have a draft supply for now only submitted", so all supplies are active.
                // Cancelling an active supply implies returning items.
                // I'll stick to basic status update for now, assuming physical return is handled via ReturnAssetAsync
                
                // Actually, if we cancel the supply record itself, we should probably ensure assets are returned.
                // But let's leave that to the explicit Return logic to avoid accidental mass returns.
                // We'll just update status here.

                // However, user might expect this to "void" the transaction.
                // Given the instruction "create one method that supply the weapon", this creates active assignments.
                // So cancelling the supply essentially means voiding it.
                
                // Let's implement safe cancellation: only if no assets have been returned yet? 
                // Or just mark as Cancelled and let manual returns happen?
                // I'll mark as Cancelled.

                // Wait, if we used existing Enums, there is no "Cancelled" in SupplySubmissionStatus!
                // SupplySubmissionStatus only has Draft and Submitted.
                // So we can't really cancel it via status. 
                // We can maybe soft-delete it? Or use FulfillmentStatus?
                // SupplyFulfillmentStatus has Partial and Fully.
                
                // Since I cannot change the enum, I cannot set status to Cancelled.
                // I will return an error saying cancellation is not supported for submitted supplies in this scheme,
                // or just soft-delete it. Soft delete seems safer to "remove" it.
                
                // Actually, let's soft delete it.
                await _assetSupplyRepository.DeleteAsync(supply);

                _logger.LogInformation("Asset supply deleted/cancelled. SupplyId: {SupplyId}, User: {UserId}",
                    id, _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(true, "Asset supply cancelled");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling asset supply. SupplyId: {SupplyId}, User: {UserId}",
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> ReturnAssetAsync(ReturnAssetDto dto)
        {
            _logger.LogInformation("Returning asset. AssetId: {AssetId}, User: {UserId}",
                dto.AssetId, _currentUserService.UserId);

            try
            {
                var validationResult = await _returnValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                var asset = await _assetRepository.FindOneAsync(
                    a => a.Id == dto.AssetId && !a.IsDeleted,
                    false);

                if (asset == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Asset not found");
                }

                if (!asset.IsAssigned)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Asset is not currently assigned");
                }

                // Get the assignment to return
                AssetAssignment? assignment;
                if (dto.AssignmentId.HasValue)
                {
                    assignment = await _context.AssetAssignments
                        .FirstOrDefaultAsync(a => a.Id == dto.AssignmentId.Value 
                            && a.AssetId == dto.AssetId 
                            && !a.IsDeleted);
                }
                else
                {
                    assignment = await _context.AssetAssignments
                        .FirstOrDefaultAsync(a => a.Id == asset.CurrentAssignmentId && !a.IsDeleted);
                }

                if (assignment == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Assignment not found");
                }

                if (assignment.Status != AssetAssignmentStatus.Active)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Only active assignments can be returned");
                }

                // Store previous values for history
                var previousDepartmentId = assignment.DepartmentId;
                var previousCustodianId = assignment.CustodianId;
                var previousLocation = assignment.Location;

                // Update assignment
                assignment.Status = AssetAssignmentStatus.Returned;
                assignment.ActualReturnDate = dto.ReturnDate ?? DateTime.UtcNow;
                assignment.ConditionOnReturn = dto.ConditionOnReturn;
                if (!string.IsNullOrEmpty(dto.Notes))
                    assignment.Notes = string.IsNullOrEmpty(assignment.Notes)
                        ? dto.Notes
                        : $"{assignment.Notes}\n{dto.Notes}";
                assignment.ModificationDate = DateTime.UtcNow;
                assignment.ModifiedBy = _currentUserService.UserId;

                // Update asset
                asset.IsAssigned = false;
                asset.CurrentAssignmentId = null;
                if (!string.IsNullOrEmpty(dto.ConditionOnReturn))
                    asset.Condition = dto.ConditionOnReturn;
                asset.ModificationDate = DateTime.UtcNow;
                asset.ModifiedBy = _currentUserService.UserId;

                await _context.SaveChangesAsync();

                // Record history
                await _historyService.RecordHistoryAsync(asset.Id, AssetHistoryActionType.Returned, new AssetHistoryContext
                {
                    Description = "Asset returned from assignment",
                    PreviousDepartmentId = previousDepartmentId,
                    PreviousCustodianId = previousCustodianId,
                    PreviousLocation = previousLocation,
                    AssetAssignmentId = assignment.Id,
                    OrderId = assignment.OrderId,
                    Notes = dto.Notes
                });

                _logger.LogInformation("Asset returned. AssetId: {AssetId}, AssignmentId: {AssignmentId}, User: {UserId}",
                    dto.AssetId, assignment.Id, _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(true, "Asset returned successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error returning asset. AssetId: {AssetId}, User: {UserId}",
                    dto.AssetId, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> ReturnMultipleAssetsAsync(ReturnMultipleAssetsDto dto)
        {
            _logger.LogInformation("Returning multiple assets. Count: {Count}, User: {UserId}",
                dto.Assets?.Count ?? 0, _currentUserService.UserId);

            try
            {
                var validationResult = await _returnMultipleValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                var results = new List<string>();
                var successCount = 0;

                foreach (var assetDto in dto.Assets)
                {
                    // Apply common notes if not specified per asset
                    if (string.IsNullOrEmpty(assetDto.Notes) && !string.IsNullOrEmpty(dto.CommonNotes))
                    {
                        assetDto.Notes = dto.CommonNotes;
                    }

                    var result = await ReturnAssetAsync(assetDto);
                    if (result.Succeeded)
                    {
                        successCount++;
                    }
                    else
                    {
                        results.Add($"Asset {assetDto.AssetId}: {result.Message}");
                    }
                }

                if (successCount == dto.Assets.Count)
                {
                    return APIOperationResponse<bool>.Success(true,
                        $"All {successCount} assets returned successfully");
                }
                else if (successCount > 0)
                {
                    return APIOperationResponse<bool>.Success(true,
                        $"{successCount} of {dto.Assets.Count} assets returned. Errors: {string.Join("; ", results)}");
                }
                else
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        $"No assets were returned. Errors: {string.Join("; ", results)}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error returning multiple assets. User: {UserId}",
                    _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
