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
        private readonly IValidator<UpdateAssetSupplyDto> _updateValidator;
        private readonly IValidator<SubmitAssetSupplyDto> _submitValidator;
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
            IValidator<UpdateAssetSupplyDto> updateValidator,
            IValidator<SubmitAssetSupplyDto> submitValidator,
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
            _updateValidator = updateValidator;
            _submitValidator = submitValidator;
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

        public async Task<APIOperationResponse<AssetSupplyDto>> GetDraftByOrderIdAsync(long orderId)
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
                    .FirstOrDefaultAsync(s => s.OrderId == orderId 
                        && s.Status == AssetSupplyStatus.Draft 
                        && !s.IsDeleted);

                if (supply == null)
                {
                    return APIOperationResponse<AssetSupplyDto>.Fail(
                        ResponseType.NotFound, "No draft asset supply found for this order");
                }

                var dto = _mapper.Map<AssetSupplyDto>(supply);
                return APIOperationResponse<AssetSupplyDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting draft asset supply. OrderId: {OrderId}, User: {UserId}",
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

        public async Task<APIOperationResponse<long>> CreateAsync(CreateAssetSupplyDto dto)
        {
            _logger.LogInformation("Creating asset supply. OrderId: {OrderId}, AssetCount: {AssetCount}, User: {UserId}",
                dto.OrderId, dto.SupplyDetails?.Count ?? 0, _currentUserService.UserId);

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
                    nameof(Order.Department));

                if (order == null)
                {
                    return APIOperationResponse<long>.Fail(ResponseType.NotFound, "Order not found");
                }

                // Check for existing draft
                var existingDraft = await _context.AssetSupplies
                    .FirstOrDefaultAsync(s => s.OrderId == dto.OrderId 
                        && s.Status == AssetSupplyStatus.Draft 
                        && !s.IsDeleted);

                if (existingDraft != null)
                {
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                        $"A draft supply already exists for this order. Supply ID: {existingDraft.Id}");
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
                supply.Status = AssetSupplyStatus.Draft;
                supply.DepartmentId = dto.DepartmentId ?? order.DepartmentId;
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
                        IsDelivered = false,
                        CreationDate = DateTime.UtcNow,
                        CreatedBy = _currentUserService.UserId
                    };
                }).ToList();

                var created = await _assetSupplyRepository.AddAsync(supply);

                _logger.LogInformation("Asset supply created. SupplyId: {SupplyId}, OrderId: {OrderId}, User: {UserId}",
                    created.Id, dto.OrderId, _currentUserService.UserId);

                return APIOperationResponse<long>.Success(created.Id, "Asset supply created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating asset supply. OrderId: {OrderId}, User: {UserId}",
                    dto.OrderId, _currentUserService.UserId);
                return APIOperationResponse<long>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, UpdateAssetSupplyDto dto)
        {
            _logger.LogInformation("Updating asset supply. SupplyId: {SupplyId}, User: {UserId}",
                id, _currentUserService.UserId);

            try
            {
                var validationResult = await _updateValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                var supply = await _context.AssetSupplies
                    .Include(s => s.SupplyDetails)
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

                if (supply == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Asset supply not found");
                }

                if (supply.Status != AssetSupplyStatus.Draft)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Only draft supplies can be updated");
                }

                // Update basic fields
                if (dto.CustodianId.HasValue)
                    supply.CustodianId = dto.CustodianId;
                if (dto.DepartmentId.HasValue)
                    supply.DepartmentId = dto.DepartmentId;
                if (!string.IsNullOrEmpty(dto.Location))
                    supply.Location = dto.Location;
                if (dto.ExpectedReturnDate.HasValue)
                    supply.ExpectedReturnDate = dto.ExpectedReturnDate;
                if (!string.IsNullOrEmpty(dto.Notes))
                    supply.Notes = dto.Notes;

                supply.ModificationDate = DateTime.UtcNow;
                supply.ModifiedBy = _currentUserService.UserId;

                // Update supply details if provided
                if (dto.SupplyDetails != null && dto.SupplyDetails.Any())
                {
                    // Validate new assets
                    var assetIds = dto.SupplyDetails.Select(d => d.AssetId).ToList();
                    var assets = await _context.Assets
                        .Where(a => assetIds.Contains(a.Id) && !a.IsDeleted)
                        .ToListAsync();

                    // Check all assets have serial numbers
                    var assetsWithoutSerial = assets.Where(a => string.IsNullOrEmpty(a.SerialNumber)).ToList();
                    if (assetsWithoutSerial.Any())
                    {
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                            $"Assets without serial numbers cannot be supplied: {string.Join(", ", assetsWithoutSerial.Select(a => a.Id))}");
                    }

                    // Check no assets are already assigned
                    var assignedAssets = assets.Where(a => a.IsAssigned).ToList();
                    if (assignedAssets.Any())
                    {
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                            $"Assets already assigned cannot be supplied: {string.Join(", ", assignedAssets.Select(a => $"{a.Id} ({a.SerialNumber})"))}");
                    }

                    // Remove old details
                    _context.AssetSupplyDetails.RemoveRange(supply.SupplyDetails);

                    // Add new details
                    var sequenceNo = 1;
                    foreach (var detail in dto.SupplyDetails)
                    {
                        var asset = assets.First(a => a.Id == detail.AssetId);
                        supply.SupplyDetails.Add(new AssetSupplyDetail
                        {
                            AssetSupplyId = supply.Id,
                            AssetId = detail.AssetId,
                            ItemId = asset.ItemId,
                            SequenceNo = sequenceNo++,
                            ConditionOnSupply = detail.ConditionOnSupply ?? asset.Condition,
                            Notes = detail.Notes,
                            IsDelivered = false,
                            CreationDate = DateTime.UtcNow,
                            CreatedBy = _currentUserService.UserId
                        });
                    }
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Asset supply updated. SupplyId: {SupplyId}, User: {UserId}",
                    id, _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(true, "Asset supply updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating asset supply. SupplyId: {SupplyId}, User: {UserId}",
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> SubmitSupplyAsync(long id, SubmitAssetSupplyDto dto)
        {
            _logger.LogInformation("Submitting asset supply. SupplyId: {SupplyId}, User: {UserId}",
                id, _currentUserService.UserId);

            try
            {
                var validationResult = await _submitValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                var supply = await _context.AssetSupplies
                    .Include(s => s.SupplyDetails)
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

                if (supply == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Asset supply not found");
                }

                if (supply.Status != AssetSupplyStatus.Draft)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Only draft supplies can be submitted");
                }

                if (supply.SupplyDetails == null || !supply.SupplyDetails.Any())
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Cannot submit supply with no assets");
                }

                // Update supply with submission info
                supply.Status = AssetSupplyStatus.Submitted;
                supply.SupplyDate = dto.SupplyDate;
                supply.ReceiverName = dto.ReceiverName;
                supply.ReceiverMilitaryId = dto.ReceiverMilitaryId;
                supply.ReceiverRankId = dto.ReceiverRankId;
                if (!string.IsNullOrEmpty(dto.Notes))
                    supply.Notes = dto.Notes;
                supply.ModificationDate = DateTime.UtcNow;
                supply.ModifiedBy = _currentUserService.UserId;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Asset supply submitted. SupplyId: {SupplyId}, User: {UserId}",
                    id, _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(true, "Asset supply submitted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting asset supply. SupplyId: {SupplyId}, User: {UserId}",
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> CompleteSupplyAsync(long id)
        {
            _logger.LogInformation("Completing asset supply. SupplyId: {SupplyId}, User: {UserId}",
                id, _currentUserService.UserId);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var supply = await _context.AssetSupplies
                    .Include(s => s.SupplyDetails)
                        .ThenInclude(d => d.Asset)
                    .Include(s => s.Order)
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

                if (supply == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Asset supply not found");
                }

                if (supply.Status != AssetSupplyStatus.Submitted && supply.Status != AssetSupplyStatus.Approved)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Only submitted or approved supplies can be completed");
                }

                // Re-validate assets before completing
                foreach (var detail in supply.SupplyDetails.Where(d => !d.IsDeleted))
                {
                    var asset = detail.Asset;
                    if (asset == null || asset.IsDeleted)
                    {
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                            $"Asset {detail.AssetId} no longer exists");
                    }

                    if (string.IsNullOrEmpty(asset.SerialNumber))
                    {
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                            $"Asset {asset.Id} does not have a serial number");
                    }

                    if (asset.IsAssigned)
                    {
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                            $"Asset {asset.Id} ({asset.SerialNumber}) is already assigned");
                    }
                }

                // Create assignments for each asset
                foreach (var detail in supply.SupplyDetails.Where(d => !d.IsDeleted))
                {
                    var asset = detail.Asset;

                    // Create assignment
                    var assignment = new AssetAssignment
                    {
                        AssetId = asset.Id,
                        OrderId = supply.OrderId,
                        AssetSupplyId = supply.Id,
                        DepartmentId = supply.DepartmentId ?? supply.Order?.DepartmentId,
                        CustodianId = supply.CustodianId,
                        Location = supply.Location,
                        AssignDate = supply.SupplyDate ?? DateTime.UtcNow,
                        ExpectedReturnDate = supply.ExpectedReturnDate,
                        Status = AssetAssignmentStatus.Active,
                        Purpose = supply.Order?.UsagePurpose,
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

                    // Mark detail as delivered
                    detail.IsDelivered = true;
                    detail.DeliveredDate = DateTime.UtcNow;

                    // Record history
                    await _historyService.RecordHistoryAsync(asset.Id, AssetHistoryActionType.Assigned, new AssetHistoryContext
                    {
                        Description = $"Asset assigned via supply #{supply.Id}",
                        NewDepartmentId = assignment.DepartmentId,
                        NewCustodianId = assignment.CustodianId,
                        NewLocation = assignment.Location,
                        OrderId = supply.OrderId,
                        AssetSupplyId = supply.Id,
                        AssetAssignmentId = assignment.Id,
                        Notes = supply.Notes
                    });
                }

                // Update supply status
                supply.Status = AssetSupplyStatus.Completed;
                supply.CompletedDate = DateTime.UtcNow;
                supply.ModificationDate = DateTime.UtcNow;
                supply.ModifiedBy = _currentUserService.UserId;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Asset supply completed. SupplyId: {SupplyId}, AssetsAssigned: {Count}, User: {UserId}",
                    id, supply.SupplyDetails.Count, _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(true, "Asset supply completed and assets assigned");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error completing asset supply. SupplyId: {SupplyId}, User: {UserId}",
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(
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
                    .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

                if (supply == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Asset supply not found");
                }

                if (supply.Status == AssetSupplyStatus.Completed)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Completed supplies cannot be cancelled. Use return functionality instead.");
                }

                supply.Status = AssetSupplyStatus.Cancelled;
                supply.Notes = string.IsNullOrEmpty(supply.Notes)
                    ? $"Cancelled: {reason}"
                    : $"{supply.Notes}\nCancelled: {reason}";
                supply.ModificationDate = DateTime.UtcNow;
                supply.ModifiedBy = _currentUserService.UserId;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Asset supply cancelled. SupplyId: {SupplyId}, User: {UserId}",
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
