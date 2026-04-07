using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Ettad.CrossCutting.Data.Repository;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Comman.Enums;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.Inventory.Service.Batches.Dtos;
using Ettad.Inventory.Services.Common;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Ettad.Lookups.Services.Contracts;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Comman.Models;
using Ettad.Inventory.Service.AssetHistory;
using Ettad.Inventory.Service.AssetHistory.Dtos;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Text.RegularExpressions;

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
        private readonly IValidator<UpdateBatchDto> _updateBatchValidator;
        private readonly IAssetHistoryService _historyService;
        private readonly IExcelImportService _excelImportService;
        private readonly IValidator<CreateAssetDto> _createAssetValidator;

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
            IValidator<BulkUpdateBatchAssetsDto> bulkUpdateValidator,
            IValidator<UpdateBatchDto> updateBatchValidator,
            IValidator<CreateAssetDto> createAssetValidator,
            IAssetHistoryService historyService,
            IExcelImportService excelImportService)
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
            _updateBatchValidator = updateBatchValidator;
            _createAssetValidator = createAssetValidator;
            _historyService = historyService;
            _excelImportService = excelImportService;
        }

        public async Task<Batch> GetOrCreateAsync(string batchNumber, long depotId)
        {
            var trimmedBatchNumber = batchNumber.Trim();

            var existing = await _batchRepository.FindOneAsync(
                p => !p.IsDeleted && p.BatchNumber == trimmedBatchNumber && p.DepotId == depotId);

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

        public async Task<APIOperationResponse<BatchDto>> GetByIdAsync(long id, bool? serialNumberOnly = null, bool? filterByIsAssigned = null, int assetsPage = 1, int assetsPageSize = 50, bool includeAllAssets = false, BatchAssetFilterDto? filters = null)
        {
            _logger.LogInformation(
                "Getting batch by ID. BatchId: {BatchId}, SerialNumberOnly: {SerialNumberOnly}, FilterByIsAssigned: {FilterByIsAssigned}, AssetsPage: {AssetsPage}, AssetsPageSize: {AssetsPageSize}, IncludeAllAssets: {IncludeAllAssets}, User: {UserId}",
                id, serialNumberOnly, filterByIsAssigned, assetsPage, assetsPageSize, includeAllAssets, _currentUserService.UserId);

            try
            {
                var batch = await _batchRepository.FindOneAsync(
                    p => p.Id == id && !p.IsDeleted,
                    false,
                    nameof(Batch.Depot));

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
                    .Include($"{nameof(Asset.Item)}.{nameof(BaseItem.BaseItemPrimaryPurposes)}.{nameof(BaseItemPrimaryPurpos.PrimaryPurpos)}")
                    .Include(nameof(Asset.Depot))
                    .Include(nameof(Asset.Supplier))
                    .Include(nameof(Asset.Manufacturer))
                    .Include(nameof(Asset.PrimaryPurpos))
                    .Include($"{nameof(Asset.CurrentAssignment)}.{nameof(AssetAssignment.Department)}")
                    .Include($"{nameof(Asset.CurrentAssignment)}.{nameof(AssetAssignment.Custodian)}")
                    .AsSplitQuery();

                assetQuery = ApplyAssetFilters(assetQuery, filters);

                if (serialNumberOnly == true)
                    assetQuery = assetQuery.Where(a => !string.IsNullOrEmpty(a.SerialNumber));

                if (filterByIsAssigned.HasValue)
                    assetQuery = assetQuery.Where(a => a.IsAssigned == filterByIsAssigned.Value);

                var totalCount = await assetQuery.CountAsync();

                var effectivePageSize = NormalizeBatchAssetsPageSize(assetsPageSize);
                int effectivePage;
                int totalPages;

                if (includeAllAssets)
                {
                    effectivePage = 1;
                    totalPages = 1;
                }
                else
                {
                    totalPages = totalCount == 0 ? 1 : (int)Math.Ceiling(totalCount / (double)effectivePageSize);
                    effectivePage = Math.Max(1, Math.Min(assetsPage < 1 ? 1 : assetsPage, totalPages));
                }

                IQueryable<Asset> orderedQuery = assetQuery.OrderBy(a => a.Id);

                List<Asset> assets;
                if (includeAllAssets)
                {
                    assets = await orderedQuery.ToListAsync();
                }
                else
                {
                    assets = await orderedQuery
                        .Skip((effectivePage - 1) * effectivePageSize)
                        .Take(effectivePageSize)
                        .ToListAsync();
                }

                dto.Assets = _mapper.Map<List<AssetDto>>(assets);
                dto.AssetCount = totalCount;
                dto.AssetsPageIndex = includeAllAssets ? 1 : effectivePage;
                dto.AssetsPageSize = includeAllAssets ? totalCount : effectivePageSize;
                dto.AssetsTotalPages = includeAllAssets ? 1 : totalPages;

                var entityIds = dto.Assets.Select(a => a.BatchId).Distinct().ToList();
                if (entityIds.Any())
                {
                    var imagesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Weapon, entityIds);
                    if (imagesResult.Succeeded && imagesResult.Data != null)
                    {
                        foreach (var assetDto in dto.Assets)
                        {
                            if (imagesResult.Data.ContainsKey(assetDto.BatchId))
                                assetDto.Images = imagesResult.Data[assetDto.BatchId];
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

        public async Task<APIOperationResponse<List<BatchDto>>> GetByBatchNumberAsync(string batchNumber, long? depotId = null, bool? serialNumberOnly = null, bool? filterByIsAssigned = null, int assetsPage = 1, int assetsPageSize = 50, bool includeAllAssets = false)
        {
            if (string.IsNullOrWhiteSpace(batchNumber))
                return APIOperationResponse<List<BatchDto>>.Fail(ResponseType.BadRequest, "Batch number is required.");

            var trimmedBatchNumber = batchNumber.Trim();

            _logger.LogInformation("Getting batch(es) by BatchNumber. BatchNumber: {BatchNumber}, DepotId: {DepotId}, User: {UserId}",
                trimmedBatchNumber, depotId, _currentUserService.UserId);

            try
            {
                if (depotId.HasValue && depotId.Value > 0)
                {
                    var userId = _currentUserService.UserId;
                    if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, depotId.Value))
                        return APIOperationResponse<List<BatchDto>>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");

                    var batch = await _batchRepository.FindOneAsync(
                        p => p.BatchNumber == trimmedBatchNumber && !p.IsDeleted && p.DepotId == depotId.Value);

                    if (batch == null)
                        return APIOperationResponse<List<BatchDto>>.Fail(ResponseType.NotFound, "Batch not found");

                    var one = await GetByIdAsync(batch.Id, serialNumberOnly, filterByIsAssigned, assetsPage, assetsPageSize, includeAllAssets);
                    if (!one.Succeeded || one.Data == null)
                        return APIOperationResponse<List<BatchDto>>.Fail(
                            (ResponseType)one.StatusCode,
                            one.Message ?? "Could not load batch.");

                    return APIOperationResponse<List<BatchDto>>.Success(new List<BatchDto> { one.Data });
                }

                var userDepotIds = await _depotAccessService.GetUserAccessibleDepotIdsAsync();
                if (userDepotIds != null && userDepotIds.Count == 0)
                    return APIOperationResponse<List<BatchDto>>.Success(new List<BatchDto>());

                var matchesQuery = _context.Batches
                    .AsNoTracking()
                    .Where(b => !b.IsDeleted && b.BatchNumber == trimmedBatchNumber);
                if (userDepotIds != null)
                    matchesQuery = matchesQuery.Where(b => userDepotIds.Contains(b.DepotId));

                var matches = await matchesQuery
                    .OrderBy(b => b.DepotId)
                    .Select(b => b.Id)
                    .ToListAsync();

                var list = new List<BatchDto>();
                foreach (var id in matches)
                {
                    var item = await GetByIdAsync(id, serialNumberOnly, filterByIsAssigned, assetsPage, assetsPageSize, includeAllAssets);
                    if (item.Succeeded && item.Data != null)
                        list.Add(item.Data);
                }

                return APIOperationResponse<List<BatchDto>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving batch(es) by BatchNumber. BatchNumber: {BatchNumber}, User: {UserId}",
                    trimmedBatchNumber, _currentUserService.UserId);
                return APIOperationResponse<List<BatchDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private static int NormalizeBatchAssetsPageSize(int requested)
        {
            return requested switch
            {
                50 => 50,
                100 => 100,
                200 => 200,
                500 => 500,
                _ => 50
            };
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
                    nameof(Batch.Depot));

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

        public async Task<APIOperationResponse<List<BatchSummaryDto>>> GetSummaryAsync(long depotId, BatchAssetFilterDto? filters = null)
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
                    var assetQuery = _context.Assets
                        .Where(a => !a.IsDeleted && batchIds.Contains(a.BatchId));

                    assetQuery = ApplyAssetFilters(assetQuery, filters);

                    var assetCounts = await assetQuery
                        .GroupBy(a => a.BatchId)
                        .Select(g => new { BatchId = g.Key, Count = g.Count() })
                        .ToListAsync();
                    countDict = assetCounts.ToDictionary(c => c.BatchId, c => c.Count);
                }

                var hasFilters = filters?.HasAnyFilter == true;

                var summaries = batches
                    .OrderByDescending(b => b.CreationDate)
                    .ThenByDescending(b => b.Id)
                    .Select(b => new BatchSummaryDto
                    {
                        Id = b.Id,
                        BatchNumber = b.BatchNumber,
                        Quantity = countDict.TryGetValue(b.Id, out var count) ? count : 0
                    })
                    .Where(s => !hasFilters || s.Quantity > 0)
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
                    nameof(Batch.Depot));

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

                    // Delete any files linked to assets in this batch (FileUplodDetails/FileUplodMaster + disk)
                    try
                    {
                        var filesResp = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Weapon, new List<long> { batch.Id });
                        if (filesResp.Succeeded && filesResp.Data != null && filesResp.Data.Any())
                        {
                            // Flatten master IDs
                            var fileIds = filesResp.Data.Values
                                .Where(v => v != null)
                                .SelectMany(v => v)
                                .Select(f => f.Id)
                                .Distinct()
                                .ToList();

                            foreach (var fileId in fileIds)
                            {
                                try
                                {
                                    await _fileUploadService.DeleteAsync(fileId);
                                }
                                catch (Exception exDel)
                                {
                                    _logger.LogWarning(exDel, "Failed deleting file {FileId} for Batch {BatchId}", fileId, id);
                                }
                            }
                        }
                    }
                    catch (Exception exFiles)
                    {
                        _logger.LogWarning(exFiles, "Error while cleaning up files for Batch delete. BatchId: {BatchId}", id);
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

        public async Task<APIOperationResponse<bool>> BulkUpdateAssetsAsync(long batchId, BulkUpdateBatchAssetsDto inputDto, List<IFormFile>? files = null)
        {
            _logger.LogInformation("Bulk updating assets in batch. BatchId: {BatchId}, ItemCount: {ItemCount}, User: {UserId}",
                batchId, inputDto?.Items?.Count, _currentUserService.UserId);

            try
            {
                var prep = await TryPrepareBulkUpdateForApplyAsync(batchId, inputDto);
                if (!prep.Ok)
                {
                    var notFound = prep.Error?.Contains("Batch not found", StringComparison.Ordinal) == true;
                    return APIOperationResponse<bool>.Fail(
                        notFound ? ResponseType.NotFound : ResponseType.BadRequest,
                        prep.Error ?? "Validation failed");
                }

                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    var apply = await ApplyBulkUpdateItemsCoreAsync(prep.AssetDict!, inputDto);
                    if (!apply.Ok)
                    {
                        await transaction.RollbackAsync();
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, apply.Error ?? "Assignment update failed.");
                    }

                    await transaction.CommitAsync();

                    // Attach uploaded files (if any) to each asset in the request.
                    if (files != null && files.Any())
                    {
                            var uploadResult = await _fileUploadService.UploadFilesForEntityAsync(files, FileEntityType.Weapon, batchId);
                            if (!uploadResult.Succeeded)
                            {
                                _logger.LogWarning("File upload failed during batch bulk update. BatchId: {BatchId}, Message: {Message}",
                                    batchId, uploadResult.Message);
                                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, $"File upload failed: {uploadResult.Message}");
                            }
                    }

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

        public async Task<APIOperationResponse<bool>> UpdateBatchAsync(long id, UpdateBatchDto dto)
        {
            _logger.LogInformation("Updating batch metadata. BatchId: {BatchId}, User: {UserId}", id, _currentUserService.UserId);

            try
            {
                var validationResult = await _updateBatchValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                var trimmed = dto.BatchNumber.Trim();
                var batch = await _batchRepository.FindOneAsync(p => p.Id == id && !p.IsDeleted);
                if (batch == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Batch not found");

                var userId = _currentUserService.UserId;
                if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, batch.DepotId))
                    return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");

                if (!string.Equals(batch.BatchNumber, trimmed, StringComparison.Ordinal))
                {
                    var duplicate = await _batchRepository.FindOneAsync(
                        p => !p.IsDeleted && p.Id != id && p.BatchNumber == trimmed);
                    if (duplicate != null)
                        return APIOperationResponse<bool>.Fail(ResponseType.Conflict, "A batch with this number already exists.");
                }

                batch.BatchNumber = trimmed;
                batch.ModificationDate = _dateTimeProvider.Now;
                batch.ModifiedBy = _currentUserService.UserId;
                await _batchRepository.UpdateAsync(batch);

                _logger.LogInformation("Batch metadata updated. BatchId: {BatchId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true, "Batch updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating batch. BatchId: {BatchId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> BulkUpdateAssetsAsync(long batchId, BulkUpdateBatchAssetsDto inputDto)
        {
            _logger.LogInformation("Bulk updating assets in batch. BatchId: {BatchId}, ItemCount: {ItemCount}, User: {UserId}",
                batchId, inputDto?.Items?.Count, _currentUserService.UserId);

            try
            {
                var prep = await TryPrepareBulkUpdateForApplyAsync(batchId, inputDto);
                if (!prep.Ok)
                {
                    var notFound = prep.Error?.Contains("Batch not found", StringComparison.Ordinal) == true;
                    return APIOperationResponse<bool>.Fail(
                        notFound ? ResponseType.NotFound : ResponseType.BadRequest,
                        prep.Error ?? "Validation failed");
                }

                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    var apply = await ApplyBulkUpdateItemsCoreAsync(prep.AssetDict!, inputDto);
                    if (!apply.Ok)
                    {
                        await transaction.RollbackAsync();
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, apply.Error ?? "Assignment update failed.");
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

        private async Task<(bool Ok, string? Error, Dictionary<long, Asset>? AssetDict)> TryPrepareBulkUpdateForApplyAsync(
            long batchId, BulkUpdateBatchAssetsDto inputDto)
        {
            var validationResult = await _bulkUpdateValidator.ValidateAsync(inputDto);
            if (!validationResult.IsValid)
                return (false, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), null);

            var batch = await _batchRepository.FindOneAsync(p => p.Id == batchId && !p.IsDeleted);
            if (batch == null)
                return (false, "Batch not found", null);

            var assets = await _assetRepository.FindAsync(
                a => !a.IsDeleted && a.BatchId == batchId);
            var assetDict = assets.ToDictionary(a => a.Id);

            var requestedIds = inputDto.Items.Select(i => i.AssetId).ToHashSet();
            var missingIds = requestedIds.Except(assetDict.Keys).ToList();
            if (missingIds.Any())
                return (false, $"Assets not found in this batch: {string.Join(", ", missingIds)}", null);

            var serialNumbers = inputDto.Items
                .Where(i => !string.IsNullOrWhiteSpace(i.SerialNumber))
                .GroupBy(i => i.SerialNumber!.Trim())
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (serialNumbers.Any())
                return (false, $"Duplicate serial numbers in request: {string.Join(", ", serialNumbers)}", null);

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
                    return (false, $"Serial numbers already exist: {string.Join(", ", existingDuplicates)}", null);
            }

            return (true, null, assetDict);
        }

        private async Task<(bool Ok, string? Error)> ApplyBulkUpdateItemsCoreAsync(
            Dictionary<long, Asset> assetDict, BulkUpdateBatchAssetsDto inputDto)
        {
            foreach (var item in inputDto.Items)
            {
                var asset = assetDict[item.AssetId];
                var lookupErr = await ValidateAssetLookupIdsForBulkUpdateAsync(
                    item.ItemId, item.SupplierId, item.ManufacturerId, item.PrimaryPurposId);
                if (lookupErr != null)
                    return (false, lookupErr);

                asset.ItemId = item.ItemId;
                asset.SerialNumber = string.IsNullOrWhiteSpace(item.SerialNumber) ? null : item.SerialNumber.Trim();
                asset.RFID = string.IsNullOrWhiteSpace(item.RFID) ? null : item.RFID.Trim();
                asset.Status = item.Status;
                asset.PurchaseDate = item.PurchaseDate;
                asset.WarrantyExpiryDate = item.WarrantyExpiryDate;
                asset.PurchasePrice = item.PurchasePrice;
                asset.DeliveryReceipt = string.IsNullOrWhiteSpace(item.DeliveryReceipt) ? null : item.DeliveryReceipt.Trim();
                asset.Notes = string.IsNullOrWhiteSpace(item.Notes) ? null : item.Notes.Trim();
                asset.SupplierId = item.SupplierId;
                asset.ManufacturerId = item.ManufacturerId;
                asset.PrimaryPurposId = item.PrimaryPurposId;
                asset.ModificationDate = _dateTimeProvider.Now;
                asset.ModifiedBy = _currentUserService.UserId;

                if (item.UpdateAssignment)
                {
                    var (assignOk, assignError) = await TryApplyBulkItemAssignmentAsync(asset, item);
                    if (!assignOk)
                        return (false, assignError ?? "Assignment update failed.");
                }

                await _assetRepository.UpdateAsync(asset);
            }

            return (true, null);
        }

        private async Task<string?> ValidatePrimaryPurposForItemBulkAsync(long itemId, long? primaryPurposId)
        {
            if (!primaryPurposId.HasValue)
                return null;

            var ok = await _context.BaseItemPrimaryPurposes
                .AnyAsync(x => x.BaseItemId == itemId && x.PrimaryPurposId == primaryPurposId.Value);
            return ok ? null : "Primary purpose is not configured for this catalog item.";
        }

        private async Task<string?> ValidateAssetLookupIdsForBulkUpdateAsync(
            long itemId, long? supplierId, long? manufacturerId, long? primaryPurposId)
        {
            if (supplierId.HasValue)
            {
                var okSupplier = await _context.Suppliers.AnyAsync(s => s.Id == supplierId.Value && !s.IsDeleted);
                if (!okSupplier)
                    return "Supplier is invalid or deleted.";
            }

            if (manufacturerId.HasValue)
            {
                var okManufacturer = await _context.Manufacturers.AnyAsync(m => m.Id == manufacturerId.Value && !m.IsDeleted);
                if (!okManufacturer)
                    return "Manufacturer is invalid or deleted.";
            }

            return await ValidatePrimaryPurposForItemBulkAsync(itemId, primaryPurposId);
        }

        private async Task<(bool Ok, string? Error)> TryApplyBulkItemAssignmentAsync(Asset asset, BatchAssetUpdateItem item)
        {
            if (!item.UpdateAssignment)
                return (true, null);

            var inSupply = await _context.AssetSupplyDetails.AnyAsync(sd => !sd.IsDeleted && sd.AssetId == asset.Id);
            if (inSupply)
                return (false, $"Cannot change assignment for asset {asset.Id} while it is included in a supply order.");

            var notes = string.IsNullOrWhiteSpace(item.AssignmentNotes) ? null : item.AssignmentNotes.Trim();

            if (!item.AssignToEmployeeId.HasValue && !item.AssignToDepartmentId.HasValue)
            {
                if (!asset.IsAssigned || !asset.CurrentAssignmentId.HasValue)
                    return (true, null);

                var assignment = await _context.AssetAssignments
                    .FirstOrDefaultAsync(a => a.Id == asset.CurrentAssignmentId.Value && !a.IsDeleted);
                if (assignment == null || assignment.Status != AssetAssignmentStatus.Active)
                {
                    asset.IsAssigned = false;
                    asset.CurrentAssignmentId = null;
                    return (true, null);
                }

                var prevDept = assignment.DepartmentId;
                var prevCust = assignment.CustodianId;
                assignment.Status = AssetAssignmentStatus.Returned;
                assignment.ActualReturnDate = _dateTimeProvider.Now;
                assignment.ModificationDate = _dateTimeProvider.Now;
                assignment.ModifiedBy = _currentUserService.UserId;

                asset.IsAssigned = false;
                asset.CurrentAssignmentId = null;

                await _context.SaveChangesAsync();
                await _historyService.RecordHistoryAsync(asset.Id, AssetHistoryActionType.Returned, new AssetHistoryContext
                {
                    Description = "Assignment removed from batch edit",
                    PreviousDepartmentId = prevDept,
                    PreviousCustodianId = prevCust,
                    AssetAssignmentId = assignment.Id
                });

                return (true, null);
            }

            long? custodianId = null;
            long? departmentId = null;
            if (item.AssignToEmployeeId.HasValue)
            {
                var emp = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == item.AssignToEmployeeId.Value && !e.IsDeleted);
                if (emp == null)
                    return (false, $"Employee not found: {item.AssignToEmployeeId.Value}");
                custodianId = emp.Id;
                departmentId = emp.DepartmentId;
                if (!departmentId.HasValue || departmentId.Value <= 0)
                    return (false, "Assignee employee has no department on record; update the employee or assign to a department only.");
            }
            else
            {
                departmentId = item.AssignToDepartmentId;
            }

            if (!departmentId.HasValue || departmentId.Value <= 0)
                return (false, "A valid department is required when assigning from batch edit.");

            var deptExists = await _context.Departments.AnyAsync(d => d.Id == departmentId.Value && !d.IsDeleted);
            if (!deptExists)
                return (false, $"Department not found: {departmentId.Value}");

            if (asset.CurrentAssignmentId.HasValue)
            {
                var existing = await _context.AssetAssignments
                    .FirstOrDefaultAsync(a => a.Id == asset.CurrentAssignmentId.Value && !a.IsDeleted);
                if (existing != null && existing.Status == AssetAssignmentStatus.Active
                    && existing.DepartmentId == departmentId
                    && existing.CustodianId == custodianId)
                {
                    if (!string.IsNullOrEmpty(notes) && !string.Equals(existing.Notes, notes, StringComparison.Ordinal))
                    {
                        existing.Notes = string.IsNullOrEmpty(existing.Notes) ? notes : $"{existing.Notes}\n{notes}";
                        existing.ModificationDate = _dateTimeProvider.Now;
                        existing.ModifiedBy = _currentUserService.UserId;
                        await _context.SaveChangesAsync();
                    }

                    return (true, null);
                }
            }

            if (asset.CurrentAssignmentId.HasValue)
            {
                var old = await _context.AssetAssignments
                    .FirstOrDefaultAsync(a => a.Id == asset.CurrentAssignmentId.Value && !a.IsDeleted);
                if (old != null && old.Status == AssetAssignmentStatus.Active)
                {
                    var prevDept = old.DepartmentId;
                    var prevCust = old.CustodianId;
                    old.Status = AssetAssignmentStatus.Returned;
                    old.ActualReturnDate = _dateTimeProvider.Now;
                    old.ModificationDate = _dateTimeProvider.Now;
                    old.ModifiedBy = _currentUserService.UserId;
                    await _context.SaveChangesAsync();
                    await _historyService.RecordHistoryAsync(asset.Id, AssetHistoryActionType.Returned, new AssetHistoryContext
                    {
                        Description = "Previous assignment ended before batch-edit reassignment",
                        PreviousDepartmentId = prevDept,
                        PreviousCustodianId = prevCust,
                        AssetAssignmentId = old.Id
                    });
                }

                asset.CurrentAssignmentId = null;
                asset.IsAssigned = false;
            }

            var now = _dateTimeProvider.Now;
            var newAssignment = new AssetAssignment
            {
                AssetId = asset.Id,
                DepartmentId = departmentId,
                CustodianId = custodianId,
                AssignDate = now,
                Status = AssetAssignmentStatus.Active,
                Notes = notes,
                ConditionOnAssign = null,
                CreationDate = now,
                CreatedBy = _currentUserService.UserId
            };

            _context.AssetAssignments.Add(newAssignment);
            await _context.SaveChangesAsync();

            asset.IsAssigned = true;
            asset.CurrentAssignmentId = newAssignment.Id;

            await _historyService.RecordHistoryAsync(asset.Id, AssetHistoryActionType.Assigned, new AssetHistoryContext
            {
                Description = "Asset assigned from batch edit",
                NewDepartmentId = departmentId,
                NewCustodianId = custodianId,
                AssetAssignmentId = newAssignment.Id,
                Notes = notes
            });

            return (true, null);
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

        public async Task<APIOperationResponse<byte[]>> ExportBatchAssetsExcelAsync(long batchId, string language = "en")
        {
            ExcelPackage.License.SetNonCommercialPersonal("Ettad");
            try
            {
                var batch = await _batchRepository.FindOneAsync(p => p.Id == batchId && !p.IsDeleted);
                if (batch == null)
                    return APIOperationResponse<byte[]>.Fail(ResponseType.NotFound, "Batch not found");

                var userId = _currentUserService.UserId;
                if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, batch.DepotId))
                    return APIOperationResponse<byte[]>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");

                var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);
                var headers = isAr
                    ? BatchAssetExcelColumnMappings.ExportHeaderOrderAr
                    : BatchAssetExcelColumnMappings.ExportHeaderOrderEn;

                var assets = await _context.Assets.AsNoTracking()
                    .Where(a => !a.IsDeleted && a.BatchId == batchId)
                    .Include(a => a.Item)
                    .Include(a => a.CurrentAssignment!)
                        .ThenInclude(ca => ca.Department)
                    .Include(a => a.CurrentAssignment!)
                        .ThenInclude(ca => ca.Custodian)
                    .OrderBy(a => a.Id)
                    .ToListAsync();

                var weapons = await _context.Weapons.AsNoTracking()
                    .Where(w => !w.IsDeleted)
                    .ToListAsync();
                var itemNames = weapons.Cast<BaseItem>()
                    .Where(i => !string.IsNullOrWhiteSpace(i.Name) && !string.IsNullOrWhiteSpace(i.ItemNo))
                    .Select(i => $"{i.Name!.Trim()} ({i.ItemNo!.Trim()})")
                    .OrderBy(n => n)
                    .ToList();

                var statusLabels = BatchAssetExcelStatusLabels.GetLabelsForLanguage(language).ToList();

                var departments = await _context.Departments.AsNoTracking()
                    .Where(d => !d.IsDeleted)
                    .OrderBy(d => d.Id)
                    .ToListAsync();
                var departmentLabels = departments
                    .Select(d => isAr
                        ? (!string.IsNullOrWhiteSpace(d.NameAr) ? d.NameAr.Trim() : (d.NameEn ?? "").Trim())
                        : (!string.IsNullOrWhiteSpace(d.NameEn) ? d.NameEn.Trim() : (d.NameAr ?? "").Trim()))
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .GroupBy(s => s, StringComparer.OrdinalIgnoreCase)
                    .Select(g => g.First())
                    .OrderBy(s => s)
                    .ToList();

                var employees = await _context.Employees.AsNoTracking()
                    .Where(e => !e.IsDeleted)
                    .OrderBy(e => e.Id)
                    .ToListAsync();
                var employeeLabels = employees
                    .Select(e => FormatBatchEmployeeDisplayForLanguage(e, language))
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .GroupBy(s => s, StringComparer.OrdinalIgnoreCase)
                    .Select(g => g.First())
                    .OrderBy(s => s)
                    .ToList();

                using var package = new ExcelPackage();
                var ws = package.Workbook.Worksheets.Add("Batch Assets");

                for (int col = 0; col < headers.Count; col++)
                {
                    ws.Cells[1, col + 1].Value = headers[col];
                    ws.Cells[1, col + 1].Style.Font.Bold = true;
                    ws.Cells[1, col + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[1, col + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    ws.Cells[1, col + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

                int excelRow = 2;
                foreach (var asset in assets)
                {
                    for (int col = 0; col < headers.Count; col++)
                        ws.Cells[excelRow, col + 1].Value = ResolveBatchExportCellValue(asset, headers[col], language);
                    excelRow++;
                }

                static void FillLookupSheet(ExcelWorksheet sheet, IReadOnlyList<string> values)
                {
                    for (int i = 0; i < values.Count; i++)
                        sheet.Cells[i + 1, 1].Value = values[i];
                    if (values.Count == 0)
                        sheet.Cells[1, 1].Value = "";
                }

                var itemsSheet = package.Workbook.Worksheets.Add("Items");
                itemsSheet.Hidden = eWorkSheetHidden.Hidden;
                FillLookupSheet(itemsSheet, itemNames);

                var statusesSheet = package.Workbook.Worksheets.Add("Statuses");
                statusesSheet.Hidden = eWorkSheetHidden.Hidden;
                FillLookupSheet(statusesSheet, statusLabels);

                var departmentsSheet = package.Workbook.Worksheets.Add("Departments");
                departmentsSheet.Hidden = eWorkSheetHidden.Hidden;
                FillLookupSheet(departmentsSheet, departmentLabels);

                var employeesSheet = package.Workbook.Worksheets.Add("Employees");
                employeesSheet.Hidden = eWorkSheetHidden.Hidden;
                FillLookupSheet(employeesSheet, employeeLabels);

                var assignmentModeLabels = BatchAssetExcelAssignmentModes.GetLabelsForLanguage(language).ToList();
                var assignModesSheet = package.Workbook.Worksheets.Add("AssignmentModes");
                assignModesSheet.Hidden = eWorkSheetHidden.Hidden;
                FillLookupSheet(assignModesSheet, assignmentModeLabels);

                static int LookupLastRow(ExcelWorksheet s)
                {
                    var d = s.Dimension;
                    return d == null || d.End.Row < 1 ? 1 : d.End.Row;
                }

                void AddListValidation(int columnIndex1Based, ExcelWorksheet lookup, string sheetName)
                {
                    var colLetter = GetColumnLetter(columnIndex1Based);
                    var range = $"{colLetter}2:{colLetter}10000";
                    var v = ws.DataValidations.AddListValidation(range);
                    var lr = LookupLastRow(lookup);
                    v.Formula.ExcelFormula = $"'{sheetName}'!$A$1:$A${lr}";
                    v.ShowErrorMessage = true;
                    v.ErrorTitle = "Invalid Value";
                    v.Error = "Please select a value from the dropdown list";
                    v.ShowInputMessage = true;
                    v.PromptTitle = "Select";
                    v.Prompt = "Choose from the list";
                }

                // Dropdown columns follow current export headers (avoids drift when columns are added/removed).
                var headerIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < headers.Count; i++)
                    headerIndex[headers[i]] = i + 1;

                var hdrItemName = isAr ? "اسم الصنف" : "Item Name";
                var hdrStatus = isAr ? "الحالة التشغيلية" : "Status";
                var hdrAssignMode = isAr ? "وضع التعيين" : "Assignment Mode";
                var hdrDept = isAr ? "القسم" : "Department";
                var hdrEmployee = isAr ? "الموظف" : "Employee";

                AddListValidation(headerIndex[hdrItemName], itemsSheet, "Items");
                AddListValidation(headerIndex[hdrStatus], statusesSheet, "Statuses");
                AddListValidation(headerIndex[hdrAssignMode], assignModesSheet, "AssignmentModes");
                AddListValidation(headerIndex[hdrDept], departmentsSheet, "Departments");
                AddListValidation(headerIndex[hdrEmployee], employeesSheet, "Employees");

                // AutoFit must not run on technical ID columns: it clears EPPlus's Hidden flag and widens them.
                if (ws.Dimension != null)
                {
                    var lastCol = ws.Dimension.End.Column;
                    for (int c = BatchAssetExcelColumnMappings.HiddenColumnCount + 1; c <= lastCol; c++)
                        ws.Column(c).AutoFit();
                }

                ws.Column(1).Hidden = true;
                ws.Column(2).Hidden = true;
                ws.Column(1).Width = 0;
                ws.Column(2).Width = 0;

                ws.View.FreezePanes(2, 1);

                return APIOperationResponse<byte[]>.Success(package.GetAsByteArray(), "OK");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Export batch assets Excel. BatchId: {BatchId}", batchId);
                return APIOperationResponse<byte[]>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>> ImportBatchAssetsPreviewAsync(
            long batchId, IFormFile file, string language = "en")
        {
            try
            {
                var guard = await GuardBatchImportAsync(batchId, file);
                if (guard != null)
                    return guard;

                var mappings = BatchAssetExcelColumnMappings.Get(language);
                var importResult = await _excelImportService.ImportFromExcelAsync<BatchAssetExcelImportRowDto>(
                    file, mappings, BatchAssetExcelColumnMappings.WorksheetName);
                var assetDict = await LoadBatchAssetsForImportAsync(batchId);
                var batch = await _batchRepository.FindOneAsync(p => p.Id == batchId && !p.IsDeleted);
                if (batch == null)
                    return APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>.Fail(ResponseType.NotFound, "Batch not found");

                await EnrichValidateBatchAssetImportAsync(language, importResult, assetDict, batch);

                var dto = BuildBulkUpdateDtoFromImport(importResult, assetDict);
                if (dto != null && dto.Items.Count > 0)
                {
                    var vr = await _bulkUpdateValidator.ValidateAsync(dto);
                    if (!vr.IsValid)
                        MergeFluentValidationIntoImportPreview(vr, importResult, dto);
                }

                importResult.TotalProcessed = importResult.SuccessfulRecords.Count + importResult.Errors.Count;
                return APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>.Success(importResult, "Preview processed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Batch assets import preview. BatchId: {BatchId}", batchId);
                return APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>> ImportBatchAssetsAsync(long batchId, IFormFile file, string language = "en")
        {
            try
            {
                var guard = await GuardBatchImportAsync(batchId, file);
                if (guard != null)
                    return APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>.Fail((ResponseType)guard.StatusCode, guard.Message ?? "Error");

                var mappings = BatchAssetExcelColumnMappings.Get(language);
                var importResult = await _excelImportService.ImportFromExcelAsync<BatchAssetExcelImportRowDto>(
                    file, mappings, BatchAssetExcelColumnMappings.WorksheetName);
                var assetDict = await LoadBatchAssetsForImportAsync(batchId);
                var batch = await _batchRepository.FindOneAsync(p => p.Id == batchId && !p.IsDeleted);
                if (batch == null)
                    return APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>.Fail(ResponseType.NotFound, "Batch not found");

                await EnrichValidateBatchAssetImportAsync(language, importResult, assetDict, batch);

                var createRows = importResult.SuccessfulRecords.Where(r => r.IsNewRow).ToList();
                var dto = BuildBulkUpdateDtoFromImport(importResult, assetDict);

                if (createRows.Count == 0 && (dto == null || dto.Items.Count == 0))
                {
                    var msg = importResult.Errors.Count > 0
                        ? "No valid rows to import"
                        : "No data rows found";
                    return APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>.Fail(ResponseType.BadRequest, msg);
                }

                Dictionary<long, Asset>? bulkUpdateDict = null;
                if (dto != null && dto.Items.Count > 0)
                {
                    var prep = await TryPrepareBulkUpdateForApplyAsync(batchId, dto);
                    if (!prep.Ok)
                    {
                        var st = prep.Error?.Contains("Batch not found", StringComparison.Ordinal) == true
                            ? ResponseType.NotFound
                            : ResponseType.BadRequest;
                        return APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>.Fail((ResponseType)st, prep.Error ?? "Validation failed");
                    }
                    bulkUpdateDict = prep.AssetDict;
                }

                await using var importTransaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    foreach (var row in createRows)
                    {
                        var createErr = await CreateAssetFromBatchExcelRowAsync(batch, row);
                        if (createErr != null)
                        {
                            await importTransaction.RollbackAsync();
                            return APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>.Fail(ResponseType.BadRequest, createErr);
                        }
                    }

                    if (dto != null && dto.Items.Count > 0 && bulkUpdateDict != null)
                    {
                        var apply = await ApplyBulkUpdateItemsCoreAsync(bulkUpdateDict, dto);
                        if (!apply.Ok)
                        {
                            await importTransaction.RollbackAsync();
                            return APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>.Fail(ResponseType.BadRequest, apply.Error ?? "Update failed");
                        }
                    }

                    await importTransaction.CommitAsync();

                    var total = createRows.Count + (dto?.Items.Count ?? 0);
                    _logger.LogInformation("Batch assets import completed. BatchId: {BatchId}, Created: {Created}, Updated: {Updated}, User: {UserId}",
                        batchId, createRows.Count, dto?.Items.Count ?? 0, _currentUserService.UserId);

                    return APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>.Success(
                        new ImportResult<BatchAssetExcelImportRowDto>
                        {
                            SuccessfulRecords = importResult.SuccessfulRecords.ToList(),
                            TotalProcessed = total
                        },
                        $"Import completed: {createRows.Count} created, {dto?.Items.Count ?? 0} updated.");
                }
                catch (Exception exInner)
                {
                    await importTransaction.RollbackAsync();
                    _logger.LogError(exInner, "Batch assets import transaction failed. BatchId: {BatchId}", batchId);
                    return APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {exInner.Message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Batch assets import. BatchId: {BatchId}", batchId);
                return APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private async Task<APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>?> GuardBatchImportAsync(long batchId, IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>.Fail(ResponseType.BadRequest, "File is required");

            var batch = await _batchRepository.FindOneAsync(p => p.Id == batchId && !p.IsDeleted);
            if (batch == null)
                return APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>.Fail(ResponseType.NotFound, "Batch not found");

            var userId = _currentUserService.UserId;
            if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, batch.DepotId))
                return APIOperationResponse<ImportResult<BatchAssetExcelImportRowDto>>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");

            return null;
        }

        private async Task<Dictionary<long, Asset>> LoadBatchAssetsForImportAsync(long batchId)
        {
            var list = await _context.Assets
                .AsNoTracking()
                .Where(a => !a.IsDeleted && a.BatchId == batchId)
                .ToListAsync();
            return list.ToDictionary(a => a.Id);
        }

        private static object? ResolveBatchExportCellValue(Asset asset, string header, string language)
        {
            var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);
            var ca = asset.CurrentAssignment is { Status: AssetAssignmentStatus.Active } ? asset.CurrentAssignment : null;

            return header switch
            {
                "_AssetId" => asset.Id,
                "_ItemId" => asset.ItemId,
                "Asset ID" or "معرف الأصل" => asset.Id,
                "Item ID" or "معرف الصنف" => asset.ItemId,
                "Item Name" or "اسم الصنف" => FormatBatchItemDisplay(asset.Item),
                "Item No" or "رقم الصنف" => asset.Item?.ItemNo,
                "Serial Number" or "رقم التسلسل" => asset.SerialNumber ?? "",
                "RFID" => asset.RFID ?? "",
                "Status" or "الوضع" or "الحالة التشغيلية" => BatchAssetExcelStatusLabels.ToLabel(asset.Status, language),
                "Purchase Date" or "تاريخ الشراء" => asset.PurchaseDate,
                "Warranty Expiry Date" or "تاريخ انتهاء الضمان" => asset.WarrantyExpiryDate,
                "Purchase Price" or "سعر الشراء" => asset.PurchasePrice,
                "Delivery Receipt" or "إيصال التسليم" => asset.DeliveryReceipt ?? "",
                "Notes" or "ملاحظات" => asset.Notes ?? "",
                "Assignment Mode" or "وضع التعيين" or "Update Assignment" or "تحديث التخصيص" =>
                    ResolveAssignmentModeLabel(ca, language),
                "Assign To Department ID" or "معرف القسم" => ca?.DepartmentId,
                "Assign To Employee ID" or "معرف الموظف" => ca?.CustodianId,
                "Assignment Notes" or "ملاحظات التخصيص" => ca?.Notes ?? "",
                "Department" or "القسم" or "Department Name" or "اسم القسم" => ca?.Department == null
                    ? ""
                    : (isAr
                        ? (!string.IsNullOrWhiteSpace(ca.Department.NameAr) ? ca.Department.NameAr : ca.Department.NameEn) ?? ""
                        : (!string.IsNullOrWhiteSpace(ca.Department.NameEn) ? ca.Department.NameEn : ca.Department.NameAr) ?? ""),
                "Employee" or "الموظف" or "Employee Name" or "اسم الموظف" => ca?.Custodian == null
                    ? ""
                    : FormatBatchEmployeeDisplayForLanguage(ca.Custodian, language),
                _ => ""
            };
        }

        private static string ResolveAssignmentModeLabel(AssetAssignment? ca, string language)
        {
            if (ca == null)
                return BatchAssetExcelAssignmentModes.ToLabel(BatchAssignmentMode.NoChange, language);
            if (ca.CustodianId.HasValue && ca.CustodianId.Value > 0)
                return BatchAssetExcelAssignmentModes.ToLabel(BatchAssignmentMode.AssignEmployee, language);
            if (ca.DepartmentId.HasValue && ca.DepartmentId.Value > 0)
                return BatchAssetExcelAssignmentModes.ToLabel(BatchAssignmentMode.AssignDepartment, language);
            return BatchAssetExcelAssignmentModes.ToLabel(BatchAssignmentMode.NoChange, language);
        }

        private static string FormatBatchItemDisplay(BaseItem? item)
        {
            if (item == null) return "";
            if (!string.IsNullOrWhiteSpace(item.Name) && !string.IsNullOrWhiteSpace(item.ItemNo))
                return $"{item.Name.Trim()} ({item.ItemNo.Trim()})";
            if (!string.IsNullOrWhiteSpace(item.Name)) return item.Name.Trim();
            return item.ItemNo?.Trim() ?? "";
        }

        private static string FormatBatchEmployeeDisplayForLanguage(Employee? e, string language)
        {
            if (e == null) return "";
            var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);
            var name = isAr
                ? (!string.IsNullOrWhiteSpace(e.NameAr) ? e.NameAr : e.NameEn)
                : (!string.IsNullOrWhiteSpace(e.NameEn) ? e.NameEn : e.NameAr);
            name = string.IsNullOrWhiteSpace(name) ? "" : name.Trim();
            if (!string.IsNullOrWhiteSpace(e.MilitaryId))
                return $"{name} ({e.MilitaryId.Trim()})".Trim();
            return name;
        }

        private static bool IsLegacyYesValue(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            var t = value.Trim().ToLowerInvariant();
            return t is "yes" or "y" or "true" or "1";
        }

        private static string GetColumnLetter(int columnNumber)
        {
            string columnLetter = "";
            while (columnNumber > 0)
            {
                columnNumber--;
                columnLetter = (char)('A' + columnNumber % 26) + columnLetter;
                columnNumber /= 26;
            }
            return columnLetter;
        }

        private async Task EnrichValidateBatchAssetImportAsync(
            string language,
            ImportResult<BatchAssetExcelImportRowDto> importResult,
            Dictionary<long, Asset> assetDict,
            Batch batch)
        {
            var weaponItems = (await _context.Weapons.AsNoTracking()
                .Where(w => !w.IsDeleted).ToListAsync()).Cast<BaseItem>().ToList();
            var departments = await _context.Departments.AsNoTracking()
                .Where(d => !d.IsDeleted).ToListAsync();
            var employees = await _context.Employees.AsNoTracking()
                .Where(e => !e.IsDeleted).ToListAsync();

            var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);

            var serialToAssets = assetDict.Values
                .Where(a => !string.IsNullOrWhiteSpace(a.SerialNumber))
                .GroupBy(a => a.SerialNumber!.Trim(), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.OrdinalIgnoreCase);

            // --- Pass 1: resolve identity, item, status, assignment ---
            ResolveAndValidateRowFields(importResult, assetDict, serialToAssets, weaponItems, departments, employees, language, isAr);

            // --- Pass 2: new-row / existing-row structural validation ---
            await ValidateRowStructureAsync(importResult, assetDict, batch);

            // --- Pass 3: in-file duplicate detection ---
            DetectInFileDuplicates(importResult);

            // --- Pass 4: DB-level serial/RFID uniqueness ---
            await DetectDbDuplicatesAsync(importResult);

            RejectSuccessfulRowsWithBothAssignmentTargets(importResult, batch, assetDict);
        }

        private static void ResolveAndValidateRowFields(
            ImportResult<BatchAssetExcelImportRowDto> importResult,
            Dictionary<long, Asset> assetDict,
            Dictionary<string, List<Asset>> serialToAssets,
            List<BaseItem> weaponItems,
            List<Department> departments,
            List<Employee> employees,
            string language,
            bool isAr)
        {
            var invalidRows = new List<(BatchAssetExcelImportRowDto row, string error)>();

            foreach (var row in importResult.SuccessfulRecords.ToList())
            {
                var rowErrors = new List<string>();
                row.IsNewRow = false;

                // Resolve asset identity
                if (row.AssetId > 0)
                {
                    if (!assetDict.ContainsKey(row.AssetId))
                        rowErrors.Add($"Asset {row.AssetId} is not in this batch");
                }
                else if (!string.IsNullOrWhiteSpace(row.SerialNumber))
                {
                    var sn = row.SerialNumber.Trim();
                    if (!serialToAssets.TryGetValue(sn, out var list) || list.Count == 0)
                        row.IsNewRow = true;
                    else if (list.Count > 1)
                        rowErrors.Add($"Multiple assets in this batch share serial number: {sn}");
                    else
                        row.AssetId = list[0].Id;
                }
                else
                    row.IsNewRow = true;

                // Auto-fill ItemId for existing rows when no item columns provided
                if (rowErrors.Count == 0 && !row.IsNewRow &&
                    string.IsNullOrWhiteSpace(row.ItemName) && string.IsNullOrWhiteSpace(row.ItemNo) &&
                    row.AssetId > 0 && row.ItemId <= 0 && assetDict.TryGetValue(row.AssetId, out var aFill))
                    row.ItemId = aFill.ItemId;

                // Resolve item
                if (rowErrors.Count == 0 &&
                    (!string.IsNullOrWhiteSpace(row.ItemName) || !string.IsNullOrWhiteSpace(row.ItemNo)))
                    TryResolveBatchItemIdFromRow(row, weaponItems, rowErrors);

                // Resolve status
                if (rowErrors.Count == 0 && !string.IsNullOrWhiteSpace(row.StatusLabel))
                {
                    if (!BatchAssetExcelStatusLabels.TryParse(row.StatusLabel, language, out var st))
                        rowErrors.Add($"Unknown status: {row.StatusLabel}");
                    else
                        row.Status = st;
                }

                // Resolve assignment mode
                if (rowErrors.Count == 0 && !string.IsNullOrWhiteSpace(row.AssignmentModeLabel))
                    ResolveAssignmentMode(row, isAr, departments, employees, rowErrors);

                if (rowErrors.Count > 0)
                    invalidRows.Add((row, string.Join("; ", rowErrors)));
            }

            FlushInvalidRows(importResult, invalidRows);
        }

        private static void ResolveAssignmentMode(
            BatchAssetExcelImportRowDto row,
            bool isAr,
            List<Department> departments,
            List<Employee> employees,
            List<string> rowErrors)
        {
            if (!BatchAssetExcelAssignmentModes.TryParse(row.AssignmentModeLabel, out var parsedMode))
            {
                rowErrors.Add($"Unknown assignment mode: {row.AssignmentModeLabel}");
                return;
            }

            var hasEmpLabel = !string.IsNullOrWhiteSpace(row.AssignmentEmployee);
            var hasDeptLabel = !string.IsNullOrWhiteSpace(row.AssignmentDepartment);

            if (IsLegacyYesValue(row.AssignmentModeLabel))
                parsedMode = BatchAssetExcelAssignmentModes.InferLegacyYesMode(hasDeptLabel, hasEmpLabel);

            row.AssignmentMode = parsedMode;

            switch (parsedMode)
            {
                case BatchAssignmentMode.AssignEmployee:
                    if (!hasEmpLabel)
                        rowErrors.Add("Assignment Mode is 'Assign to Employee' but the Employee column is empty.");
                    else if (hasDeptLabel)
                        rowErrors.Add("When assigning to an employee, leave the Department column empty (the employee's department is used).");
                    else if (!TryResolveBatchEmployeeId(row.AssignmentEmployee, isAr, employees, out var empId, out var empErr) || !empId.HasValue)
                        rowErrors.Add(empErr ?? "Employee could not be resolved");
                    else
                    {
                        row.AssignToEmployeeId = empId;
                        row.AssignToDepartmentId = null;
                    }
                    break;

                case BatchAssignmentMode.AssignDepartment:
                    if (!hasDeptLabel)
                        rowErrors.Add("Assignment Mode is 'Assign to Department' but the Department column is empty.");
                    else if (hasEmpLabel)
                        rowErrors.Add("When assigning to a department, leave the Employee column empty.");
                    else if (!TryResolveBatchDepartmentId(row.AssignmentDepartment, departments, out var deptId, out var deptErr) || !deptId.HasValue)
                        rowErrors.Add(deptErr ?? "Department could not be resolved");
                    else
                    {
                        row.AssignToDepartmentId = deptId;
                        row.AssignToEmployeeId = null;
                    }
                    break;

                case BatchAssignmentMode.RemoveAssignment:
                    row.AssignToEmployeeId = null;
                    row.AssignToDepartmentId = null;
                    break;
            }
        }

        private async Task ValidateRowStructureAsync(
            ImportResult<BatchAssetExcelImportRowDto> importResult,
            Dictionary<long, Asset> assetDict,
            Batch batch)
        {
            var invalidRows = new List<(BatchAssetExcelImportRowDto row, string error)>();

            foreach (var row in importResult.SuccessfulRecords.ToList())
            {
                var rowErrors = new List<string>();
                if (row.IsNewRow)
                {
                    if (row.ItemId <= 0)
                        rowErrors.Add("Item is required for new rows (item name/code, or unhide _ItemId).");

                    if (rowErrors.Count == 0)
                    {
                        var createDto = MapImportRowToCreateAssetDto(batch, row);
                        var vr = await _createAssetValidator.ValidateAsync(createDto);
                        if (!vr.IsValid)
                            rowErrors.Add(string.Join("; ", vr.Errors.Select(e => e.ErrorMessage)));
                    }
                }
                else
                {
                    if (row.AssetId <= 0)
                        rowErrors.Add("Asset ID is required");
                    if (!assetDict.TryGetValue(row.AssetId, out _))
                        rowErrors.Add($"Asset {row.AssetId} is not in this batch");
                    if (row.ItemId <= 0)
                        rowErrors.Add("Item ID is required");
                }

                if (rowErrors.Count > 0)
                    invalidRows.Add((row, string.Join("; ", rowErrors)));
            }

            FlushInvalidRows(importResult, invalidRows);
        }

        private static void DetectInFileDuplicates(ImportResult<BatchAssetExcelImportRowDto> importResult)
        {
            // Duplicate AssetId
            var seenAsset = new HashSet<long>();
            var invalidRows = new List<(BatchAssetExcelImportRowDto row, string error)>();

            foreach (var row in importResult.SuccessfulRecords)
            {
                if (row.IsNewRow) continue;
                if (!seenAsset.Add(row.AssetId))
                    invalidRows.Add((row, "Duplicate Asset ID in file"));
            }
            FlushInvalidRows(importResult, invalidRows);

            // Duplicate serial numbers
            var dupSerials = importResult.SuccessfulRecords
                .Where(r => !string.IsNullOrWhiteSpace(r.SerialNumber))
                .GroupBy(r => r.SerialNumber!.Trim(), StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            invalidRows.Clear();
            foreach (var row in importResult.SuccessfulRecords)
            {
                if (!string.IsNullOrWhiteSpace(row.SerialNumber) && dupSerials.Contains(row.SerialNumber.Trim()))
                    invalidRows.Add((row, $"Duplicate serial number in file: {row.SerialNumber}"));
            }
            FlushInvalidRows(importResult, invalidRows);

            // Duplicate RFIDs
            var dupRfids = importResult.SuccessfulRecords
                .Where(r => !string.IsNullOrWhiteSpace(r.RFID))
                .GroupBy(r => r.RFID!.Trim(), StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            invalidRows.Clear();
            foreach (var row in importResult.SuccessfulRecords)
            {
                if (!string.IsNullOrWhiteSpace(row.RFID) && dupRfids.Contains(row.RFID.Trim()))
                    invalidRows.Add((row, $"Duplicate RFID in file: {row.RFID}"));
            }
            FlushInvalidRows(importResult, invalidRows);
        }

        private async Task DetectDbDuplicatesAsync(ImportResult<BatchAssetExcelImportRowDto> importResult)
        {
            // Serial numbers already in DB
            var serialsToCheck = importResult.SuccessfulRecords
                .Where(r => !string.IsNullOrWhiteSpace(r.SerialNumber))
                .ToList();

            if (serialsToCheck.Count > 0)
            {
                var serialValues = serialsToCheck.Select(s => s.SerialNumber!.Trim()).ToList();
                var assetIdsInRequest = serialsToCheck
                    .Where(s => s.AssetId > 0).Select(s => s.AssetId).Distinct().ToList();

                var existingSerials = (await _context.Assets
                    .Where(a => !a.IsDeleted && !assetIdsInRequest.Contains(a.Id) && a.SerialNumber != null && serialValues.Contains(a.SerialNumber))
                    .Select(a => a.SerialNumber).ToListAsync())
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                if (existingSerials.Count > 0)
                {
                    var invalid = importResult.SuccessfulRecords
                        .Where(r => !string.IsNullOrWhiteSpace(r.SerialNumber) && existingSerials.Contains(r.SerialNumber!.Trim()))
                        .Select(r => (r, $"Serial number already exists: {r.SerialNumber}"))
                        .ToList();
                    FlushInvalidRows(importResult, invalid);
                }
            }

            // RFIDs already in DB
            var rfidsToCheck = importResult.SuccessfulRecords
                .Where(r => !string.IsNullOrWhiteSpace(r.RFID))
                .ToList();

            if (rfidsToCheck.Count > 0)
            {
                var rfidValues = rfidsToCheck.Select(s => s.RFID!.Trim()).ToList();
                var assetIdsForRfid = rfidsToCheck
                    .Where(s => s.AssetId > 0).Select(s => s.AssetId).Distinct().ToList();

                var existingRfids = (await _context.Assets
                    .Where(a => !a.IsDeleted && a.RFID != null && rfidValues.Contains(a.RFID) && !assetIdsForRfid.Contains(a.Id))
                    .Select(a => a.RFID).ToListAsync())
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                if (existingRfids.Count > 0)
                {
                    var invalid = importResult.SuccessfulRecords
                        .Where(r => !string.IsNullOrWhiteSpace(r.RFID) && existingRfids.Contains(r.RFID!.Trim()))
                        .Select(r => (r, $"RFID already exists: {r.RFID}"))
                        .ToList();
                    FlushInvalidRows(importResult, invalid);
                }
            }
        }

        /// <summary>Batch-remove invalid rows from SuccessfulRecords and add to Errors in O(n).</summary>
        private static void FlushInvalidRows(
            ImportResult<BatchAssetExcelImportRowDto> importResult,
            List<(BatchAssetExcelImportRowDto row, string error)> invalidRows)
        {
            if (invalidRows.Count == 0) return;

            var toRemove = new HashSet<BatchAssetExcelImportRowDto>(invalidRows.Select(x => x.row));
            importResult.SuccessfulRecords.RemoveAll(r => toRemove.Contains(r));

            foreach (var (row, error) in invalidRows)
            {
                importResult.Errors.Add(new ImportError
                {
                    RowNumber = row.RowNumber,
                    ErrorMessage = error,
                    RowData = row
                });
            }
        }

        /// <summary>
        /// Preview/import must reject rows where intake assignment specifies both employee and department IDs
        /// (labels-only conflicts are handled earlier). Ensures preview matches import regardless of FluentValidation property paths.
        /// </summary>
        private void RejectSuccessfulRowsWithBothAssignmentTargets(
            ImportResult<BatchAssetExcelImportRowDto> importResult,
            Batch batch,
            Dictionary<long, Asset> assetDict)
        {
            const string msg = "Specify either assign to employee or assign to department, not both.";
            foreach (var row in importResult.SuccessfulRecords.ToList())
            {
                if (!row.UpdateAssignment)
                    continue;

                if (!row.IsNewRow)
                {
                    if (!assetDict.TryGetValue(row.AssetId, out var asset))
                        continue;
                    var item = MapImportRowToUpdateItem(row, asset);
                    if (item.AssignToEmployeeId.HasValue && item.AssignToDepartmentId.HasValue)
                        MoveRowToImportErrors(importResult, row, msg);
                }
                else
                {
                    var createDto = MapImportRowToCreateAssetDto(batch, row);
                    if (createDto.AssignToEmployeeId.HasValue && createDto.AssignToDepartmentId.HasValue)
                        MoveRowToImportErrors(importResult, row, msg);
                }
            }
        }

        private static void MoveRowToImportErrors(
            ImportResult<BatchAssetExcelImportRowDto> importResult,
            BatchAssetExcelImportRowDto row,
            string message)
        {
            importResult.SuccessfulRecords.Remove(row);
            importResult.Errors.Add(new ImportError
            {
                RowNumber = row.RowNumber,
                ErrorMessage = message,
                RowData = row
            });
        }

        private static CreateAssetDto MapImportRowToCreateAssetDto(Batch batch, BatchAssetExcelImportRowDto row)
        {
            var dto = new CreateAssetDto
            {
                ItemId = row.ItemId,
                BatchNumber = batch.BatchNumber,
                SerialNumber = row.SerialNumber,
                RFID = row.RFID,
                DepotId = batch.DepotId,
                PurchaseDate = row.PurchaseDate,
                WarrantyExpiryDate = row.WarrantyExpiryDate,
                PurchasePrice = row.PurchasePrice,
                DeliveryReceipt = row.DeliveryReceipt,
                Notes = row.Notes,
            };
            if (row.UpdateAssignment)
            {
                dto.AssignToEmployeeId = row.AssignToEmployeeId;
                dto.AssignToDepartmentId = row.AssignToDepartmentId;
                dto.AssignmentNotes = row.AssignmentNotes;
            }
            return dto;
        }

        private static bool WantsBatchImportIntakeAssignment(CreateAssetDto dto) =>
            dto.AssignToEmployeeId.HasValue || dto.AssignToDepartmentId.HasValue;

        private async Task<(bool Ok, string? Error)> TryApplyBatchImportIntakeAssignmentAsync(Asset asset, CreateAssetDto dto)
        {
            if (!WantsBatchImportIntakeAssignment(dto))
                return (true, null);

            long? custodianId = null;
            long? departmentId = null;

            if (dto.AssignToEmployeeId.HasValue)
            {
                var emp = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == dto.AssignToEmployeeId.Value && !e.IsDeleted);
                if (emp == null)
                    return (false, $"Employee not found: {dto.AssignToEmployeeId.Value}");
                custodianId = emp.Id;
                departmentId = emp.DepartmentId;
                if (!departmentId.HasValue || departmentId.Value <= 0)
                    return (false, "Assignee employee has no department on record; update the employee or assign to a department only.");
            }
            else
            {
                departmentId = dto.AssignToDepartmentId;
            }

            if (!departmentId.HasValue || departmentId.Value <= 0)
                return (false, "A valid department is required for intake assignment.");

            var deptExists = await _context.Departments.AnyAsync(d => d.Id == departmentId.Value && !d.IsDeleted);
            if (!deptExists)
                return (false, $"Department not found: {departmentId.Value}");

            var now = _dateTimeProvider.Now;
            var assignment = new AssetAssignment
            {
                AssetId = asset.Id,
                DepartmentId = departmentId,
                CustodianId = custodianId,
                AssignDate = now,
                Status = AssetAssignmentStatus.Active,
                Notes = string.IsNullOrWhiteSpace(dto.AssignmentNotes) ? null : dto.AssignmentNotes.Trim(),
                ConditionOnAssign = null,
                CreationDate = now,
                CreatedBy = _currentUserService.UserId
            };

            _context.AssetAssignments.Add(assignment);
            await _context.SaveChangesAsync();

            asset.IsAssigned = true;
            asset.CurrentAssignmentId = assignment.Id;
            asset.ModificationDate = now;
            asset.ModifiedBy = _currentUserService.UserId;
            await _context.SaveChangesAsync();

            await _historyService.RecordHistoryAsync(asset.Id, AssetHistoryActionType.Assigned, new AssetHistoryContext
            {
                Description = $"Asset assigned on batch Excel import (asset id {asset.Id})",
                NewDepartmentId = departmentId,
                NewCustodianId = custodianId,
                AssetAssignmentId = assignment.Id,
                Notes = dto.AssignmentNotes
            });

            return (true, null);
        }

        private async Task<string?> CreateAssetFromBatchExcelRowAsync(Batch batch, BatchAssetExcelImportRowDto row)
        {
            var createDto = MapImportRowToCreateAssetDto(batch, row);
            var asset = _mapper.Map<Asset>(createDto);
            asset.BatchId = batch.Id;
            asset.CreationDate = _dateTimeProvider.Now;
            asset.CreatedBy = _currentUserService.UserId;
            asset.Status = row.Status ?? AssetStatus.ReadyToIssue;
            asset.SerialNumber = string.IsNullOrWhiteSpace(createDto.SerialNumber) ? null : createDto.SerialNumber.Trim();
            asset.RFID = string.IsNullOrWhiteSpace(createDto.RFID) ? null : createDto.RFID.Trim();
            asset.ItemId = row.ItemId;
            asset.DepotId = batch.DepotId;

            var createdAsset = await _assetRepository.AddAsync(asset);
            var (assignOk, assignError) = await TryApplyBatchImportIntakeAssignmentAsync(createdAsset, createDto);
            return assignOk ? null : assignError;
        }

        private static void TryResolveBatchItemIdFromRow(BatchAssetExcelImportRowDto row, List<BaseItem> allItems, List<string> rowErrors)
        {
            long? itemId = null;
            if (!string.IsNullOrEmpty(row.ItemNo))
            {
                var foundItem = allItems.FirstOrDefault(i => string.Equals(i.ItemNo, row.ItemNo.Trim(), StringComparison.Ordinal));
                if (foundItem != null)
                    itemId = foundItem.Id;
            }

            if (!itemId.HasValue && !string.IsNullOrEmpty(row.ItemName))
            {
                var itemNameValue = row.ItemName.Trim();
                if (itemNameValue.Contains('(') && itemNameValue.Contains(')'))
                {
                    var startIndex = itemNameValue.LastIndexOf('(');
                    var endIndex = itemNameValue.LastIndexOf(')');
                    if (startIndex > 0 && endIndex > startIndex)
                    {
                        var extractedItemNo = itemNameValue.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                        var foundItem = allItems.FirstOrDefault(i => string.Equals(i.ItemNo, extractedItemNo, StringComparison.Ordinal));
                        if (foundItem != null)
                            itemId = foundItem.Id;
                    }
                }

                if (!itemId.HasValue)
                {
                    var foundItem = allItems.FirstOrDefault(i =>
                        i.Name != null && i.Name.Equals(itemNameValue, StringComparison.OrdinalIgnoreCase));
                    if (foundItem != null)
                        itemId = foundItem.Id;
                }
            }

            if (!itemId.HasValue)
                rowErrors.Add($"Item not found: {row.ItemName ?? row.ItemNo ?? "(empty)"}");
            else
                row.ItemId = itemId.Value;
        }

        private static bool TryResolveBatchDepartmentId(
            string? label,
            List<Department> departments,
            out long? deptId,
            out string? error)
        {
            deptId = null;
            error = null;
            if (string.IsNullOrWhiteSpace(label))
                return true;

            var t = label.Trim();
            var matches = departments
                .Where(d =>
                    (!string.IsNullOrWhiteSpace(d.NameEn) && string.Equals(d.NameEn.Trim(), t, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrWhiteSpace(d.NameAr) && string.Equals(d.NameAr.Trim(), t, StringComparison.OrdinalIgnoreCase)))
                .Select(d => d.Id)
                .Distinct()
                .ToList();

            if (matches.Count == 0)
            {
                error = $"Department not found: {t}";
                return false;
            }

            if (matches.Count > 1)
            {
                error = $"Ambiguous department name: {t}";
                return false;
            }

            deptId = matches[0];
            return true;
        }

        private static bool TryResolveBatchEmployeeId(
            string? label,
            bool isArFile,
            List<Employee> employees,
            out long? empId,
            out string? error)
        {
            empId = null;
            error = null;
            if (string.IsNullOrWhiteSpace(label))
                return true;

            var t = label.Trim();
            var matches = employees
                .Where(e =>
                {
                    var primary = FormatBatchEmployeeDisplayForLanguage(e, isArFile ? "ar" : "en");
                    var alternate = FormatBatchEmployeeDisplayForLanguage(e, isArFile ? "en" : "ar");
                    return string.Equals(primary, t, StringComparison.OrdinalIgnoreCase)
                        || string.Equals(alternate, t, StringComparison.OrdinalIgnoreCase);
                })
                .Select(e => e.Id)
                .Distinct()
                .ToList();

            if (matches.Count == 0)
            {
                error = $"Employee not found: {t}";
                return false;
            }

            if (matches.Count > 1)
            {
                error = $"Ambiguous employee: {t}";
                return false;
            }

            empId = matches[0];
            return true;
        }

        private static BatchAssetUpdateItem MapImportRowToUpdateItem(BatchAssetExcelImportRowDto row, Asset assetEntity)
        {
            var updateAssignment = row.UpdateAssignment;
            return new BatchAssetUpdateItem
            {
                AssetId = row.AssetId,
                ItemId = row.ItemId,
                SerialNumber = CoalesceTrimOptional(row.SerialNumber, assetEntity.SerialNumber),
                RFID = CoalesceTrimOptional(row.RFID, assetEntity.RFID),
                Status = row.Status ?? assetEntity.Status,
                PurchaseDate = row.PurchaseDate ?? assetEntity.PurchaseDate,
                WarrantyExpiryDate = row.WarrantyExpiryDate ?? assetEntity.WarrantyExpiryDate,
                PurchasePrice = row.PurchasePrice ?? assetEntity.PurchasePrice,
                DeliveryReceipt = CoalesceTrimOptional(row.DeliveryReceipt, assetEntity.DeliveryReceipt),
                Notes = CoalesceTrimOptional(row.Notes, assetEntity.Notes),
                UpdateAssignment = updateAssignment,
                AssignToDepartmentId = updateAssignment ? row.AssignToDepartmentId : null,
                AssignToEmployeeId = updateAssignment ? row.AssignToEmployeeId : null,
                AssignmentNotes = updateAssignment && !string.IsNullOrWhiteSpace(row.AssignmentNotes)
                    ? row.AssignmentNotes.Trim()
                    : null
            };
        }

        private static string? CoalesceTrimOptional(string? fromRow, string? existing)
        {
            if (fromRow == null)
                return existing;
            return string.IsNullOrWhiteSpace(fromRow) ? null : fromRow.Trim();
        }

        private BulkUpdateBatchAssetsDto? BuildBulkUpdateDtoFromImport(
            ImportResult<BatchAssetExcelImportRowDto> importResult,
            Dictionary<long, Asset> assetDict)
        {
            var updateRows = importResult.SuccessfulRecords.Where(r => !r.IsNewRow).ToList();
            if (updateRows.Count == 0)
                return null;

            var items = updateRows
                .Select(r => MapImportRowToUpdateItem(r, assetDict[r.AssetId]))
                .ToList();

            return new BulkUpdateBatchAssetsDto { Items = items };
        }

        private void MergeFluentValidationIntoImportPreview(
            FluentValidation.Results.ValidationResult vr,
            ImportResult<BatchAssetExcelImportRowDto> importResult,
            BulkUpdateBatchAssetsDto dto)
        {
            var rowsByAssetId = importResult.SuccessfulRecords
                .Where(r => !r.IsNewRow && r.AssetId > 0)
                .ToDictionary(r => r.AssetId, r => r);
            var rowsToRemove = new HashSet<BatchAssetExcelImportRowDto>();

            foreach (var e in vr.Errors)
            {
                var prop = e.PropertyName ?? string.Empty;
                var m = Regex.Match(prop, @"Items\[(\d+)\]", RegexOptions.IgnoreCase);
                if (!m.Success)
                    m = Regex.Match(prop, @"\bItems\[(\d+)\]", RegexOptions.IgnoreCase);
                if (!m.Success)
                    m = Regex.Match(prop, @"\[(\d+)\]");
                if (m.Success && int.TryParse(m.Groups[1].Value, out var ix) && ix >= 0 && ix < dto.Items.Count)
                {
                    var aid = dto.Items[ix].AssetId;
                    if (rowsByAssetId.TryGetValue(aid, out var row))
                    {
                        rowsToRemove.Add(row);
                        importResult.Errors.Add(new ImportError
                        {
                            RowNumber = row.RowNumber,
                            ErrorMessage = e.ErrorMessage,
                            RowData = row
                        });
                    }
                }
                else
                {
                    importResult.Errors.Add(new ImportError
                    {
                        RowNumber = 0,
                        ErrorMessage = e.ErrorMessage
                    });
                }
            }

            foreach (var row in rowsToRemove)
                importResult.SuccessfulRecords.Remove(row);
        }

        private static IQueryable<Asset> ApplyAssetFilters(IQueryable<Asset> query, BatchAssetFilterDto? filters)
        {
            if (filters == null) return query;

            if (filters.ItemIds is { Count: > 0 })
                query = query.Where(a => filters.ItemIds.Contains(a.ItemId));

            if (filters.SupplierIds is { Count: > 0 })
                query = query.Where(a => a.SupplierId.HasValue && filters.SupplierIds.Contains(a.SupplierId.Value));

            if (filters.ManufacturerIds is { Count: > 0 })
                query = query.Where(a => a.ManufacturerId.HasValue && filters.ManufacturerIds.Contains(a.ManufacturerId.Value));

            if (filters.PrimaryPurposeIds is { Count: > 0 })
                query = query.Where(a => a.PrimaryPurposId.HasValue && filters.PrimaryPurposeIds.Contains(a.PrimaryPurposId.Value));

            return query;
        }
    }
}
