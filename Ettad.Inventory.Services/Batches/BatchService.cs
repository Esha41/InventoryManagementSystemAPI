using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.CrossCutting.Data.Repository;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Comman.Enums;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.Inventory.Service.Batches.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Ettad.Lookups.Services.Contracts;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Comman.Models;

namespace Ettad.Inventory.Service.Batches
{
    public class BatchService : IBatchService
    {
        private readonly ICrossCuttingRepository<Batch> _batchRepository;
        private readonly ICrossCuttingRepository<Asset> _assetRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<BatchService> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly ApplicationDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IDepotAccessService _depotAccessService;
        private readonly IValidator<BulkUpdateBatchAssetsDto> _bulkUpdateValidator;

        public BatchService(
            ICrossCuttingRepository<Batch> batchRepository,
            ICrossCuttingRepository<Asset> assetRepository,
            IMapper mapper,
            ICurrentUserService currentUserService,
            ILogger<BatchService> logger,
            IFileUploadService fileUploadService,
            ApplicationDbContext context,
            IDateTimeProvider dateTimeProvider,
            IDepotAccessService depotAccessService,
            IValidator<BulkUpdateBatchAssetsDto> bulkUpdateValidator)
        {
            _batchRepository = batchRepository;
            _assetRepository = assetRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _logger = logger;
            _fileUploadService = fileUploadService;
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _depotAccessService = depotAccessService;
            _bulkUpdateValidator = bulkUpdateValidator;
        }

        public async Task<Batch> GetOrCreateAsync(string batchNumber, long depotId)
        {
            var trimmedBatchNumber = batchNumber.Trim();

            var existing = await _batchRepository.FindOneAsync(
                p => !p.IsDeleted && p.BatchNumber == trimmedBatchNumber);

            if (existing != null)
                return existing;

            var batch = new Batch
            {
                BatchNumber = trimmedBatchNumber,
                DepotId = depotId,
                CreationDate = _dateTimeProvider.Now,
                CreatedBy = _currentUserService.UserId
            };

            var created = await _batchRepository.AddAsync(batch);

            _logger.LogInformation("Batch created implicitly. BatchId: {BatchId}, BatchNumber: {BatchNumber}, DepotId: {DepotId}, User: {UserId}",
                created.Id, trimmedBatchNumber, depotId, _currentUserService.UserId);

            return created;
        }

        public async Task<APIOperationResponse<BatchDto>> GetByIdAsync(long id, bool? serialNumberOnly = null, int? quantity = null, bool? filterByIsAssigned = null)
        {
            _logger.LogInformation("Getting batch by ID. BatchId: {BatchId}, SerialNumberOnly: {SerialNumberOnly}, Quantity: {Quantity}, FilterByIsAssigned: {FilterByIsAssigned}, User: {UserId}",
                id, serialNumberOnly, quantity, filterByIsAssigned, _currentUserService.UserId);

            try
            {
                var batch = await _batchRepository.FindOneAsync(
                    p => p.Id == id && !p.IsDeleted,
                    false,
                    nameof(Batch.Depot),
                    nameof(Batch.PrimaryPurpos));

                if (batch == null)
                    return APIOperationResponse<BatchDto>.Fail(ResponseType.NotFound, "Batch not found");

                var userId = _currentUserService.UserId;
                if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, batch.DepotId))
                {
                    return APIOperationResponse<BatchDto>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
                }

                var dto = _mapper.Map<BatchDto>(batch);

                var assetQuery = _context.Assets
                    .AsNoTracking()
                    .Where(a => !a.IsDeleted && a.BatchId == id)
                    .Include(nameof(Asset.Item))
                    .Include(nameof(Asset.Depot))
                    .Include($"{nameof(Asset.CurrentAssignment)}.{nameof(AssetAssignment.Department)}")
                    .AsSplitQuery();

                if (serialNumberOnly == true)
                    assetQuery = assetQuery.Where(a => !string.IsNullOrEmpty(a.SerialNumber));

                if (filterByIsAssigned.HasValue)
                    assetQuery = assetQuery.Where(a => a.IsAssigned == filterByIsAssigned.Value);

                if (quantity.HasValue && quantity.Value > 0)
                    assetQuery = assetQuery.Take(quantity.Value);

                var assets = await assetQuery.ToListAsync();

                dto.Assets = _mapper.Map<List<AssetDto>>(assets);
                dto.AssetCount = dto.Assets.Count;

                var entityIds = dto.Assets.Select(a => a.Id).ToList();
                if (entityIds.Any())
                {
                    var imagesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Asset, entityIds);
                    if (imagesResult.Succeeded && imagesResult.Data != null)
                    {
                        foreach (var assetDto in dto.Assets)
                        {
                            if (imagesResult.Data.ContainsKey(assetDto.Id))
                                assetDto.Images = imagesResult.Data[assetDto.Id];
                        }
                    }
                }

                return APIOperationResponse<BatchDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving batch by ID. BatchId: {BatchId}, User: {UserId}",
                    id, _currentUserService.UserId);
                return APIOperationResponse<BatchDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<BatchDto>> GetByBatchNumberAsync(string batchNumber, bool? serialNumberOnly = null, int? quantity = null, bool? filterByIsAssigned = null)
        {
            if (string.IsNullOrWhiteSpace(batchNumber))
                return APIOperationResponse<BatchDto>.Fail(ResponseType.BadRequest, "Batch number is required.");

            var trimmedBatchNumber = batchNumber.Trim();

            _logger.LogInformation("Getting batch by BatchNumber. BatchNumber: {BatchNumber}, User: {UserId}",
                trimmedBatchNumber, _currentUserService.UserId);

            var batch = await _batchRepository.FindOneAsync(
                p => p.BatchNumber == trimmedBatchNumber && !p.IsDeleted);

            if (batch == null)
                return APIOperationResponse<BatchDto>.Fail(ResponseType.NotFound, "Batch not found");

            return await GetByIdAsync(batch.Id, serialNumberOnly, quantity, filterByIsAssigned);
        }

        public async Task<APIOperationResponse<List<BatchDto>>> GetAllAsync(long? depotId = null)
        {
            _logger.LogInformation("Getting all batches. DepotId: {DepotId}, User: {UserId}",
                depotId, _currentUserService.UserId);

            try
            {
                if (depotId.HasValue && depotId.Value > 0)
                {
                    var userId = _currentUserService.UserId;
                    if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, depotId.Value))
                    {
                        return APIOperationResponse<List<BatchDto>>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
                    }
                }

                var batches = await _batchRepository.FindAsync(
                    p => !p.IsDeleted && (!depotId.HasValue || p.DepotId == depotId.Value),
                    false,
                    nameof(Batch.Depot),
                    nameof(Batch.PrimaryPurpos));

                var dtos = _mapper.Map<List<BatchDto>>(batches.ToList());

                var batchIds = dtos.Select(d => d.Id).ToList();
                if (batchIds.Any())
                {
                    var assetCounts = await _context.Assets
                        .Where(a => !a.IsDeleted && batchIds.Contains(a.BatchId))
                        .GroupBy(a => a.BatchId)
                        .Select(g => new { BatchId = g.Key, Count = g.Count() })
                        .ToListAsync();

                    foreach (var dto in dtos)
                    {
                        var match = assetCounts.FirstOrDefault(c => c.BatchId == dto.Id);
                        dto.AssetCount = match?.Count ?? 0;
                    }
                }

                return APIOperationResponse<List<BatchDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all batches. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<BatchDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<BatchSummaryDto>>> GetSummaryAsync(long depotId)
        {
            _logger.LogInformation("Getting batch summary. DepotId: {DepotId}, User: {UserId}",
                depotId, _currentUserService.UserId);

            try
            {
                var userId = _currentUserService.UserId;
                if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, depotId))
                {
                    return APIOperationResponse<List<BatchSummaryDto>>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
                }

                var batches = await _batchRepository.FindAsync(
                    p => !p.IsDeleted && p.DepotId == depotId);

                var batchIds = batches.Select(b => b.Id).ToList();
                Dictionary<long, int> countDict = new();
                if (batchIds.Any())
                {
                    var assetCounts = await _context.Assets
                        .Where(a => !a.IsDeleted && batchIds.Contains(a.BatchId))
                        .GroupBy(a => a.BatchId)
                        .Select(g => new { BatchId = g.Key, Count = g.Count() })
                        .ToListAsync();
                    countDict = assetCounts.ToDictionary(c => c.BatchId, c => c.Count);
                }

                var summaries = batches
                    .OrderByDescending(b => b.CreationDate)
                    .ThenByDescending(b => b.Id)
                    .Select(b => new BatchSummaryDto
                    {
                        Id = b.Id,
                        BatchNumber = b.BatchNumber,
                        Quantity = countDict.TryGetValue(b.Id, out var count) ? count : 0
                    })
                    .ToList();

                return APIOperationResponse<List<BatchSummaryDto>>.Success(summaries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting batch summary. DepotId: {DepotId}, User: {UserId}",
                    depotId, _currentUserService.UserId);
                return APIOperationResponse<List<BatchSummaryDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<PaginatedList<BatchDto>>> SearchAsync(long? depotId, PagedListRequest request)
        {
            _logger.LogInformation("Searching batches. DepotId: {DepotId}, Page: {Page}, PageSize: {PageSize}, User: {UserId}",
                depotId, request.Page, request.PageSize, _currentUserService.UserId);

            try
            {
                if (depotId.HasValue && depotId.Value > 0)
                {
                    var userId = _currentUserService.UserId;
                    if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, depotId.Value))
                    {
                        return APIOperationResponse<PaginatedList<BatchDto>>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
                    }
                }

                var query = _batchRepository.Find(
                    p => !p.IsDeleted && (!depotId.HasValue || p.DepotId == depotId.Value),
                    false,
                    nameof(Batch.Depot),
                    nameof(Batch.PrimaryPurpos));

                var paginatedEntities = await PaginatedList<Batch>.CreateAsyncForTableBinding(query, request);

                var dtos = _mapper.Map<List<BatchDto>>(paginatedEntities.Items);

                var batchIds = dtos.Select(d => d.Id).ToList();
                if (batchIds.Any())
                {
                    var assetCounts = await _context.Assets
                        .Where(a => !a.IsDeleted && batchIds.Contains(a.BatchId))
                        .GroupBy(a => a.BatchId)
                        .Select(g => new { BatchId = g.Key, Count = g.Count() })
                        .ToListAsync();

                    foreach (var dto in dtos)
                    {
                        var match = assetCounts.FirstOrDefault(c => c.BatchId == dto.Id);
                        dto.AssetCount = match?.Count ?? 0;
                    }
                }

                var result = new PaginatedList<BatchDto>(
                    dtos,
                    paginatedEntities.TotalCount,
                    paginatedEntities.PageIndex,
                    request.PageSize);

                return APIOperationResponse<PaginatedList<BatchDto>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching batches. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<PaginatedList<BatchDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting batch. BatchId: {BatchId}, User: {UserId}",
                id, _currentUserService.UserId);

            try
            {
                var batch = await _batchRepository.FindOneAsync(p => p.Id == id && !p.IsDeleted);
                if (batch == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Batch not found");

                var userId = _currentUserService.UserId;
                if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, batch.DepotId))
                    return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");

                var assetIdsInBatch = await _context.Assets
                    .Where(a => !a.IsDeleted && a.BatchId == id)
                    .Select(a => a.Id)
                    .ToListAsync();

                if (assetIdsInBatch.Count > 0)
                {
                    var assignedCount = await _context.Assets
                        .Where(a => assetIdsInBatch.Contains(a.Id) && a.IsAssigned)
                        .CountAsync();

                    var inSupplyCount = await _context.AssetSupplyDetails
                        .Where(sd => !sd.IsDeleted && assetIdsInBatch.Contains(sd.AssetId))
                        .CountAsync();

                    if (assignedCount > 0 || inSupplyCount > 0)
                    {
                        var reasons = new List<string>();
                        if (assignedCount > 0) reasons.Add($"{assignedCount} assigned");
                        if (inSupplyCount > 0) reasons.Add($"{inSupplyCount} in supply orders");
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                            $"Cannot delete batch: {assetIdsInBatch.Count} asset(s) are in use ({string.Join(", ", reasons)}). Remove or unassign them first.");
                    }

                    await using var transaction = await _context.Database.BeginTransactionAsync();
                    try
                    {
                        foreach (var assetId in assetIdsInBatch)
                        {
                            var asset = await _assetRepository.FindOneAsync(a => a.Id == assetId, false);
                            if (asset != null)
                                await _assetRepository.DeleteAsync(asset);
                        }

                        await _batchRepository.DeleteAsync(batch);
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
                else
                {
                    await _batchRepository.DeleteAsync(batch);
                }

                _logger.LogInformation("Batch and {AssetCount} assets deleted successfully. BatchId: {BatchId}, User: {UserId}",
                    assetIdsInBatch.Count, id, _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(true, "Batch deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting batch. BatchId: {BatchId}, User: {UserId}",
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> BulkUpdateAssetsAsync(long batchId, BulkUpdateBatchAssetsDto inputDto)
        {
            _logger.LogInformation("Bulk updating assets in batch. BatchId: {BatchId}, ItemCount: {ItemCount}, User: {UserId}",
                batchId, inputDto?.Items?.Count, _currentUserService.UserId);

            try
            {
                var validationResult = await _bulkUpdateValidator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                var batch = await _batchRepository.FindOneAsync(p => p.Id == batchId && !p.IsDeleted);
                if (batch == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Batch not found");

                var assets = await _assetRepository.FindAsync(
                    a => !a.IsDeleted && a.BatchId == batchId);
                var assetDict = assets.ToDictionary(a => a.Id);

                var requestedIds = inputDto.Items.Select(i => i.AssetId).ToHashSet();
                var missingIds = requestedIds.Except(assetDict.Keys).ToList();
                if (missingIds.Any())
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, $"Assets not found in this batch: {string.Join(", ", missingIds)}");

                var serialNumbers = inputDto.Items
                    .Where(i => !string.IsNullOrWhiteSpace(i.SerialNumber))
                    .GroupBy(i => i.SerialNumber!.Trim())
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (serialNumbers.Any())
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, $"Duplicate serial numbers in request: {string.Join(", ", serialNumbers)}");

                var serialsToCheck = inputDto.Items
                    .Where(i => !string.IsNullOrWhiteSpace(i.SerialNumber))
                    .Select(i => new { i.AssetId, Serial = i.SerialNumber!.Trim() })
                    .ToList();

                if (serialsToCheck.Any())
                {
                    var serialValues = serialsToCheck.Select(s => s.Serial).ToList();
                    var assetIdsInRequest = serialsToCheck.Select(s => s.AssetId).ToList();

                    var existingDuplicates = await _context.Assets
                        .Where(a => !a.IsDeleted && !assetIdsInRequest.Contains(a.Id) && serialValues.Contains(a.SerialNumber))
                        .Select(a => a.SerialNumber)
                        .ToListAsync();

                    if (existingDuplicates.Any())
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, $"Serial numbers already exist: {string.Join(", ", existingDuplicates)}");
                }

                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    foreach (var item in inputDto.Items)
                    {
                        var asset = assetDict[item.AssetId];
                        asset.ItemId = item.ItemId;
                        asset.SerialNumber = string.IsNullOrWhiteSpace(item.SerialNumber) ? null : item.SerialNumber.Trim();
                        asset.RFID = string.IsNullOrWhiteSpace(item.RFID) ? null : item.RFID.Trim();
                        asset.Status = item.Status;
                        asset.AssetTag = string.IsNullOrWhiteSpace(item.AssetTag) ? null : item.AssetTag.Trim();
                        asset.PurchaseDate = item.PurchaseDate;
                        asset.WarrantyExpiryDate = item.WarrantyExpiryDate;
                        asset.Condition = string.IsNullOrWhiteSpace(item.Condition) ? null : item.Condition.Trim();
                        asset.PurchasePrice = item.PurchasePrice;
                        asset.Notes = string.IsNullOrWhiteSpace(item.Notes) ? null : item.Notes.Trim();
                        asset.ModificationDate = _dateTimeProvider.Now;
                        asset.ModifiedBy = _currentUserService.UserId;

                        await _assetRepository.UpdateAsync(asset);
                    }

                    await transaction.CommitAsync();

                    _logger.LogInformation("Bulk update completed. BatchId: {BatchId}, UpdatedCount: {Count}, User: {UserId}",
                        batchId, inputDto.Items.Count, _currentUserService.UserId);

                    return APIOperationResponse<bool>.Success(true, "All assets updated successfully");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Error during bulk update transaction. BatchId: {BatchId}, User: {UserId}",
                        batchId, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error bulk updating assets. BatchId: {BatchId}, User: {UserId}",
                    batchId, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> RemoveAssetFromBatchAsync(long batchId, long assetId)
        {
            _logger.LogInformation("Removing asset from batch. BatchId: {BatchId}, AssetId: {AssetId}, User: {UserId}",
                batchId, assetId, _currentUserService.UserId);

            try
            {
                var batch = await _batchRepository.FindOneAsync(p => p.Id == batchId && !p.IsDeleted);
                if (batch == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Batch not found");

                var userId = _currentUserService.UserId;
                if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, batch.DepotId))
                    return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");

                var asset = await _assetRepository.FindOneAsync(a => a.Id == assetId && !a.IsDeleted && a.BatchId == batchId);
                if (asset == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Asset not found in this batch");

                if (asset.IsAssigned)
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Cannot remove assigned asset. Unassign it first.");

                var inSupply = await _context.AssetSupplyDetails
                    .AnyAsync(sd => !sd.IsDeleted && sd.AssetId == assetId);
                if (inSupply)
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Cannot remove asset that is in a supply order.");

                await _assetRepository.DeleteAsync(asset);

                _logger.LogInformation("Asset removed from batch successfully. BatchId: {BatchId}, AssetId: {AssetId}, User: {UserId}",
                    batchId, assetId, _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(true, "Asset removed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing asset from batch. BatchId: {BatchId}, AssetId: {AssetId}, User: {UserId}",
                    batchId, assetId, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
