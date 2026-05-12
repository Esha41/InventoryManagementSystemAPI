using AutoMapper;
using FluentValidation;
using System;
using System.Linq;
using System.Linq.Expressions;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Comman.Models;
using Ettad.Inventory.Service.Batches.Dtos;
using Ettad.Inventory.Service.AssetHistory.Dtos;
using System.Text.Json;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Module.lookup.Interfaces;
using Ettad.Inventory.Service.AssetHistory.Interfaces;
using Ettad.Inventory.Service.Batches.Interfaces;
using Ettad.Inventory.Service.Common.Interfaces;
using Ettad.Inventory.Service.Assets.Interfaces;
using Ettad.Data.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Ettad.Inventory.Service.Assets.Implementation
{
    public class AssetService : IAssetService
    {
        private readonly ICrossCuttingRepository<Asset> _assetRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAssetDto> _createValidator;
        private readonly IValidator<UpdateAssetDto> _updateValidator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AssetService> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly IExcelImportService _excelImportService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IDepotAccessService _depotAccessService;
        private readonly IBatchService _batchService;
        private readonly IValidator<CreateBulkAssetsFromTemplateDto> _bulkTemplateValidator;
        private readonly IAssetHistoryService _historyService;
        private readonly ITransactionManager _transactionManager;
        private readonly ICrossCuttingRepository<Employee> _employeeRepository;
        private readonly ICrossCuttingRepository<Department> _departmentRepository;
        private readonly ICrossCuttingRepository<AssetAssignment> _assetAssignmentRepository;
        private readonly ICrossCuttingRepository<Weapon> _weaponRepository;
        private readonly ICrossCuttingRepository<BaseItemPrimaryPurpos> _baseItemPrimaryPurposRepository;
        private readonly ICrossCuttingRepository<Supplier> _supplierRepository;
        private readonly ICrossCuttingRepository<Manufacturer> _manufacturerRepository;
        private readonly IAssetBulkSqlRepository _assetBulkSqlRepository;
        private readonly ICrossCuttingRepository<AssetSupplyDetail> _assetSupplyDetailRepository;

        public AssetService(
            ICrossCuttingRepository<Asset> assetRepository,
            IMapper mapper,
            IValidator<CreateAssetDto> createValidator,
            IValidator<UpdateAssetDto> updateValidator,
            IValidator<CreateBulkAssetsFromTemplateDto> bulkTemplateValidator,
            ICurrentUserService currentUserService,
            ILogger<AssetService> logger,
            IFileUploadService fileUploadService,
            IExcelImportService excelImportService,
            IDateTimeProvider dateTimeProvider,
            IDepotAccessService depotAccessService,
            IBatchService batchService,
            IAssetHistoryService historyService,
            ITransactionManager transactionManager,
            ICrossCuttingRepository<Employee> employeeRepository,
            ICrossCuttingRepository<Department> departmentRepository,
            ICrossCuttingRepository<AssetAssignment> assetAssignmentRepository,
            ICrossCuttingRepository<Weapon> weaponRepository,
            ICrossCuttingRepository<BaseItemPrimaryPurpos> baseItemPrimaryPurposRepository,
            ICrossCuttingRepository<Supplier> supplierRepository,
            ICrossCuttingRepository<Manufacturer> manufacturerRepository,
            IAssetBulkSqlRepository assetBulkSqlRepository,
            ICrossCuttingRepository<AssetSupplyDetail> assetSupplyDetailRepository)
        {
            _assetRepository = assetRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _bulkTemplateValidator = bulkTemplateValidator;
            _currentUserService = currentUserService;
            _logger = logger;
            _fileUploadService = fileUploadService;
            _excelImportService = excelImportService;
            _dateTimeProvider = dateTimeProvider;
            _depotAccessService = depotAccessService;
            _batchService = batchService;
            _historyService = historyService;
            _transactionManager = transactionManager;
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _assetAssignmentRepository = assetAssignmentRepository;
            _weaponRepository = weaponRepository;
            _baseItemPrimaryPurposRepository = baseItemPrimaryPurposRepository;
            _supplierRepository = supplierRepository;
            _manufacturerRepository = manufacturerRepository;
            _assetBulkSqlRepository = assetBulkSqlRepository;
            _assetSupplyDetailRepository = assetSupplyDetailRepository;
        }

        private static bool WantsIntakeAssignment(CreateAssetDto dto) =>
            dto.AssignToEmployeeId.HasValue || dto.AssignToDepartmentId.HasValue;

        /// <summary>
        /// EF include paths for <see cref="Asset.Item"/> weapon/ammunition caliber data (same pattern as inventory summaries).
        /// </summary>
        private static readonly string AssetItemLookupCaliberInclude =
            $"{nameof(Asset.Item)}.{nameof(Weapon.LookupCaliber)}";

        private static readonly string AssetItemCaliberUnitInclude =
            $"{nameof(Asset.Item)}.{nameof(Weapon.CaliberUnit)}";

        /// <summary>Includes used when mapping <see cref="Asset"/> to <see cref="AssetDto"/> with catalog item details.</summary>
        private static readonly string[] AssetReadMapIncludes =
        {
            nameof(Asset.Item),
            AssetItemLookupCaliberInclude,
            AssetItemCaliberUnitInclude,
            nameof(Asset.Depot),
            nameof(Asset.Batch),
            nameof(Asset.Supplier),
            nameof(Asset.Manufacturer),
            nameof(Asset.PrimaryPurpos),
            $"{nameof(Asset.CurrentAssignment)}.{nameof(AssetAssignment.Custodian)}",
            $"{nameof(Asset.CurrentAssignment)}.{nameof(AssetAssignment.Department)}"
        };

        public async Task<APIOperationResponse<PaginatedList<AssetDto>>> GetAssetsPaginatedAsync(long? depotId, PagedListRequest request)
        {
            _logger.LogInformation("Getting assets paginated. DepotId: {DepotId}, Page: {Page}, PageSize: {PageSize}, User: {UserId}",
                depotId, request.Page, request.PageSize, _currentUserService.UserId);

            try
            {
                if (depotId.HasValue && depotId.Value > 0)
                {
                    var userId = _currentUserService.UserId;
                    if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, depotId.Value))
                    {
                        _logger.LogWarning("User {UserId} attempted to access assets for unauthorized depot {DepotId}", userId, depotId);
                        return APIOperationResponse<PaginatedList<AssetDto>>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
                    }
                }

                var query = _assetRepository.Find(
                    a => !a.IsDeleted && (!depotId.HasValue || a.DepotId == depotId.Value),
                    false,
                    AssetReadMapIncludes);

                var paginatedEntities = await PaginatedList<Asset>.CreateAsyncForTableBinding(query, request);

                // Map to DTOs
                var dtos = new List<AssetDto>();
                if (paginatedEntities.Items.Any())
                {
                    dtos = _mapper.Map<List<AssetDto>>(paginatedEntities.Items);

                    // Fetch images for the visible page only
                    var entityIds = dtos.Select(d => d.Id).ToList();
                    var imagesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Asset, entityIds);

                    if (imagesResult.Succeeded && imagesResult.Data != null)
                    {
                        foreach (var dto in dtos)
                        {
                            if (imagesResult.Data.ContainsKey(dto.Id))
                            {
                                dto.Images = imagesResult.Data[dto.Id];
                            }
                        }
                    }
                }

                var result = new PaginatedList<AssetDto>(
                    dtos,
                    paginatedEntities.TotalCount,
                    paginatedEntities.PageIndex,
                    request.PageSize // Use requested page size
                );

                return APIOperationResponse<PaginatedList<AssetDto>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting assets paginated. DepotId: {DepotId}, User: {UserId}",
                    depotId, _currentUserService.UserId);
                return APIOperationResponse<PaginatedList<AssetDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<APIOperationResponse<PaginatedList<AssetItemCatalogSummaryDto>>> GetAssetCatalogItemSummariesPagedAsync(
            PagedListRequest request,
            long? depotId = null,
            List<long>? depotIds = null,
            ItemType? itemType = null)
        {
            request ??= new PagedListRequest();
            PagedListRequestNormalizer.Normalize(request);

            var effective = new HashSet<long>();
            if (depotIds != null) foreach (var d in depotIds) if (d > 0) effective.Add(d);
            if (depotId.HasValue && depotId.Value > 0) effective.Add(depotId.Value);

            _logger.LogInformation(
                "Getting asset catalog item summaries (paged). DepotId: {DepotId}, DepotCount: {DepotCount}, ItemType: {ItemType}, Page: {Page}, PageSize: {PageSize}, User: {UserId}",
                depotId, effective.Count, itemType, request.Page, request.PageSize, _currentUserService.UserId);

            try
            {
                if (effective.Count > 0)
                {
                    var userId = _currentUserService.UserId;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        foreach (var dId in effective)
                        {
                            if (!await _depotAccessService.HasDepotAccessAsync(userId, dId))
                            {
                                _logger.LogWarning("User {UserId} attempted to access catalog summaries for unauthorized depot {DepotId}", userId, dId);
                                return APIOperationResponse<PaginatedList<AssetItemCatalogSummaryDto>>.Fail(ResponseType.Forbidden, "You do not have access to one or more of the requested depots.");
                            }
                        }
                    }
                }

                Expression<Func<Asset, bool>> depotFilter;
                if (effective.Count == 0)
                    depotFilter = a => !a.IsDeleted;
                else if (effective.Count == 1)
                {
                    var one = effective.First();
                    depotFilter = a => !a.IsDeleted && a.DepotId == one;
                }
                else
                {
                    var setDepots = effective.ToArray();
                    depotFilter = a => !a.IsDeleted && setDepots.Contains(a.DepotId);
                }

                var assetQuery = _assetRepository
                    .Find(depotFilter, false, nameof(Asset.Item))
                    .Where(a => a.Item != null && !a.Item.IsDeleted);

                if (itemType.HasValue)
                {
                    var it = itemType.Value;
                    assetQuery = assetQuery.Where(a => a.Item.ItemType == it);
                }

                var distinctItemIdsQuery = assetQuery.Select(a => a.ItemId).Distinct();
                var totalCount = await distinctItemIdsQuery.CountAsync();

                var pageItemIds = await distinctItemIdsQuery
                    .OrderBy(id => id)
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                if (pageItemIds.Count == 0)
                {
                    var empty = new PaginatedList<AssetItemCatalogSummaryDto>(
                        new List<AssetItemCatalogSummaryDto>(),
                        totalCount,
                        request.Page,
                        request.PageSize);
                    return APIOperationResponse<PaginatedList<AssetItemCatalogSummaryDto>>.Success(empty);
                }

                var grouped = await assetQuery
                    .Where(a => pageItemIds.Contains(a.ItemId))
                    .GroupBy(a => a.ItemId)
                    .Select(g => new
                    {
                        ItemId = g.Key,
                        TotalAssets = g.Count(),
                        ItemName = g.Select(x => x.Item.Name).FirstOrDefault(),
                        ItemNo = g.Select(x => x.Item.ItemNo).FirstOrDefault(),
                        Nsn = g.Select(x => x.Item.Nsn).FirstOrDefault(),
                        PartNo = g.Select(x => x.Item.PartNo).FirstOrDefault(),
                        ItemType = g.Select(x => x.Item.ItemType).FirstOrDefault()
                    })
                    .ToListAsync();

                var rowById = grouped.ToDictionary(x => x.ItemId);
                var items = new List<AssetItemCatalogSummaryDto>();
                foreach (var id in pageItemIds)
                {
                    if (!rowById.TryGetValue(id, out var row))
                        continue;
                    items.Add(new AssetItemCatalogSummaryDto
                    {
                        ItemId = row.ItemId,
                        ItemName = row.ItemName ?? string.Empty,
                        ItemNo = row.ItemNo ?? string.Empty,
                        Nsn = row.Nsn ?? string.Empty,
                        PartNo = row.PartNo ?? string.Empty,
                        ItemType = row.ItemType,
                        TotalAssets = row.TotalAssets
                    });
                }

                var page = new PaginatedList<AssetItemCatalogSummaryDto>(items, totalCount, request.Page, request.PageSize);
                return APIOperationResponse<PaginatedList<AssetItemCatalogSummaryDto>>.Success(page);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting asset catalog item summaries paged. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<PaginatedList<AssetItemCatalogSummaryDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private async Task<(bool Ok, string? Error)> TryApplyIntakeAssignmentAsync(Asset asset, CreateAssetDto dto)
        {
            if (!WantsIntakeAssignment(dto))
                return (true, null);

            long? custodianId = null;
            long? departmentId = null;

            if (dto.AssignToEmployeeId.HasValue)
            {
                var emp = await _employeeRepository.FindOneAsync(e => e.Id == dto.AssignToEmployeeId.Value && !e.IsDeleted);
                if (emp == null)
                    return (false, $"Employee not found: {dto.AssignToEmployeeId.Value}");
                custodianId = emp.Id;
                // Always use the employee's department for assignments to a person (no separate department override).
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

            var deptExists = await _departmentRepository.FindOneAsync(d => d.Id == departmentId.Value && !d.IsDeleted) != null;
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

            await _assetAssignmentRepository.AddAsync(assignment);

            asset.IsAssigned = true;
            asset.CurrentAssignmentId = assignment.Id;
            asset.ModificationDate = now;
            asset.ModifiedBy = _currentUserService.UserId;           
            await _assetRepository.UpdateAsync(asset);

            await _historyService.RecordHistoryAsync(asset.Id, AssetHistoryActionType.Assigned, new AssetHistoryContext
            {
                Description = $"Asset assigned on depot intake (asset id {asset.Id})",
                NewDepartmentId = departmentId,
                NewCustodianId = custodianId,
                AssetAssignmentId = assignment.Id,
                Notes = dto.AssignmentNotes
            });

            return (true, null);
        }

        public async Task<APIOperationResponse<AssetDto>> GetByIdAsync(long id)
        {
            _logger.LogInformation("Getting asset by ID. AssetId: {AssetId}, User: {UserId}", 
                id, _currentUserService.UserId);
            
            try
            {
                var asset = await _assetRepository.FindOneAsync(
                    a => a.Id == id && !a.IsDeleted,
                    false,
                    AssetReadMapIncludes);

                if (asset == null)
                    return APIOperationResponse<AssetDto>.Fail(ResponseType.NotFound, "Asset not found");

                var userId = _currentUserService.UserId;
                if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, asset.DepotId))
                {
                    _logger.LogWarning("User {UserId} attempted to access asset {AssetId} in unauthorized depot {DepotId}", userId, id, asset.DepotId);
                    return APIOperationResponse<AssetDto>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
                }

                var dto = _mapper.Map<AssetDto>(asset);
                
                var imagesResult = await _fileUploadService.GetByEntityAsync(FileEntityType.Asset, asset.Id);
                dto.Images = imagesResult.Succeeded && imagesResult.Data != null ? imagesResult.Data : new List<FileUploadDto>();
                
                _logger.LogInformation("Asset retrieved successfully. AssetId: {AssetId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                
                return APIOperationResponse<AssetDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving asset by ID. AssetId: {AssetId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<AssetDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<AssetDto>> GetBySerialNumberAsync(string serialNumber)
        {
            _logger.LogInformation("Getting asset by SerialNumber. SerialNumber: {SerialNumber}, User: {UserId}", 
                serialNumber, _currentUserService.UserId);
            
            try
            {
                if (string.IsNullOrWhiteSpace(serialNumber))
                    return APIOperationResponse<AssetDto>.Fail(ResponseType.BadRequest, "Serial number is required");

                var trimmedSerialNumber = serialNumber.Trim();
                
                var asset = await _assetRepository.FindOneAsync(
                    a => !a.IsDeleted && a.SerialNumber != null && a.SerialNumber == trimmedSerialNumber,
                    false,
                    AssetReadMapIncludes);

                if (asset == null)
                {
                    _logger.LogWarning("Asset not found by SerialNumber. SerialNumber: {SerialNumber}, User: {UserId}", 
                        trimmedSerialNumber, _currentUserService.UserId);
                    return APIOperationResponse<AssetDto>.Fail(ResponseType.NotFound, "Asset not found");
                }

                var userId = _currentUserService.UserId;
                if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, asset.DepotId))
                {
                    _logger.LogWarning("User {UserId} attempted to access asset by SerialNumber in unauthorized depot {DepotId}", userId, asset.DepotId);
                    return APIOperationResponse<AssetDto>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
                }

                var dto = _mapper.Map<AssetDto>(asset);
                
                // Get images for this asset
                var imagesResult = await _fileUploadService.GetByEntityAsync(FileEntityType.Asset, asset.Id);
                dto.Images = imagesResult.Succeeded && imagesResult.Data != null ? imagesResult.Data : new List<FileUploadDto>();
                
                _logger.LogInformation("Asset retrieved successfully by SerialNumber. AssetId: {AssetId}, SerialNumber: {SerialNumber}, User: {UserId}", 
                    asset.Id, trimmedSerialNumber, _currentUserService.UserId);
                
                return APIOperationResponse<AssetDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving asset by SerialNumber. SerialNumber: {SerialNumber}, User: {UserId}", 
                    serialNumber, _currentUserService.UserId);
                return APIOperationResponse<AssetDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<AssetDto>>> GetAllAsync(long? depotId = null, List<long>? depotIds = null)
        {
            var effective = new HashSet<long>();
            if (depotIds != null) foreach (var d in depotIds) if (d > 0) effective.Add(d);
            if (depotId.HasValue && depotId.Value > 0) effective.Add(depotId.Value);
            _logger.LogInformation("Getting all assets. DepotId: {DepotId}, DepotCount: {DepotCount}, User: {UserId}",
                depotId, effective.Count, _currentUserService.UserId);
            
            try
            {
                if (effective.Count > 0)
                {
                    var userId = _currentUserService.UserId;
                    if (!string.IsNullOrEmpty(userId))
                    {
                        foreach (var dId in effective)
                        {
                            if (!await _depotAccessService.HasDepotAccessAsync(userId, dId))
                            {
                                _logger.LogWarning("User {UserId} attempted to access assets for unauthorized depot {DepotId}", userId, dId);
                                return APIOperationResponse<List<AssetDto>>.Fail(ResponseType.Forbidden, "You do not have access to one or more of the requested depots.");
                            }
                        }
                    }
                }

                // Build filter predicate
                Expression<Func<Asset, bool>> filter = a => !a.IsDeleted;
                if (effective.Count == 1)
                {
                    var one = effective.First();
                    filter = a => !a.IsDeleted && a.DepotId == one;
                }
                else if (effective.Count > 1)
                {
                    var set = effective.ToArray();
                    filter = a => !a.IsDeleted && set.Contains(a.DepotId);
                }

                var assets = await _assetRepository.FindAsync(
                    filter,
                    false,
                    AssetReadMapIncludes);

                // Map assets individually to handle any mapping issues gracefully
                var dtos = new List<AssetDto>();
                foreach (var asset in assets)
                {
                    try
                    {
                        var dto = _mapper.Map<AssetDto>(asset);
                        if (dto != null)
                        {
                            dtos.Add(dto);
                        }
                    }
                    catch (AutoMapperMappingException mapEx)
                    {
                        _logger.LogWarning(mapEx, "Failed to map asset. AssetId: {AssetId}, SerialNumber: {SerialNumber}, User: {UserId}", 
                            asset.Id, asset.SerialNumber, _currentUserService.UserId);
                        // Continue with other assets - don't fail entire request
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unexpected error mapping asset. AssetId: {AssetId}, SerialNumber: {SerialNumber}, User: {UserId}", 
                            asset.Id, asset.SerialNumber, _currentUserService.UserId);
                        // Continue with other assets
                    }
                }
                
                // Populate images for all assets in a single database query
                var entityIds = dtos.Select(d => d.Id).ToList();
                var imagesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Asset, entityIds);
                if (imagesResult.Succeeded && imagesResult.Data != null)
                {
                    foreach (var dto in dtos)
                    {
                        dto.Images = imagesResult.Data.ContainsKey(dto.Id) ? imagesResult.Data[dto.Id] : new List<FileUploadDto>();
                    }
                }
                
                _logger.LogInformation("All assets retrieved successfully. Count: {Count}, User: {UserId}", 
                    dtos.Count, _currentUserService.UserId);
                
                return APIOperationResponse<List<AssetDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all assets. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<AssetDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<AssetDto>>> GetAssetsByItemIdAsync(long itemId, long? depotId = null, List<long>? depotIds = null)
        {
            _logger.LogInformation(
                "Getting assets by itemId. ItemId: {ItemId}, DepotId: {DepotId}, DepotIdsCount: {DepotIdsCount}, User: {UserId}",
                itemId,
                depotId?.ToString() ?? "none",
                depotIds?.Count ?? 0,
                _currentUserService.UserId);

            try
            {
                var userDepotIds = await _depotAccessService.GetUserAccessibleDepotIdsAsync();
                if (userDepotIds != null && userDepotIds.Count == 0)
                    return APIOperationResponse<List<AssetDto>>.Success(new List<AssetDto>());

                var query = QueryAssetsByItemIdWithDepotScope(itemId, depotId, depotIds, userDepotIds, out var emptyBecauseScope);
                if (emptyBecauseScope)
                    return APIOperationResponse<List<AssetDto>>.Success(new List<AssetDto>());

                var assets = await query.ToListAsync();

                var dtos = _mapper.Map<List<AssetDto>>(assets);

                _logger.LogInformation("Assets by itemId retrieved. ItemId: {ItemId}, Count: {Count}, User: {UserId}",
                    itemId, dtos.Count, _currentUserService.UserId);

                return APIOperationResponse<List<AssetDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting assets by itemId. ItemId: {ItemId}, User: {UserId}", itemId, _currentUserService.UserId);
                return APIOperationResponse<List<AssetDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        /// <inheritdoc />
        public async Task<APIOperationResponse<PaginatedList<AssetDto>>> GetAssetsByItemIdPagedAsync(
            long itemId,
            PagedListRequest request,
            long? depotId = null,
            List<long>? depotIds = null)
        {
            request ??= new PagedListRequest();
            PagedListRequestNormalizer.Normalize(request);

            _logger.LogInformation(
                "Getting assets by itemId (paged). ItemId: {ItemId}, DepotId: {DepotId}, DepotIdsCount: {DepotIdsCount}, Page: {Page}, PageSize: {PageSize}, User: {UserId}",
                itemId,
                depotId?.ToString() ?? "none",
                depotIds?.Count ?? 0,
                request.Page,
                request.PageSize,
                _currentUserService.UserId);

            try
            {
                var userDepotIds = await _depotAccessService.GetUserAccessibleDepotIdsAsync();
                if (userDepotIds != null && userDepotIds.Count == 0)
                {
                    var empty = new PaginatedList<AssetDto>(new List<AssetDto>(), 0, request.Page, request.PageSize);
                    return APIOperationResponse<PaginatedList<AssetDto>>.Success(empty);
                }

                var query = QueryAssetsByItemIdWithDepotScope(itemId, depotId, depotIds, userDepotIds, out var emptyBecauseScope);
                if (emptyBecauseScope)
                {
                    var empty = new PaginatedList<AssetDto>(new List<AssetDto>(), 0, request.Page, request.PageSize);
                    return APIOperationResponse<PaginatedList<AssetDto>>.Success(empty);
                }

                var paginatedEntities = await PaginatedList<Asset>.CreateAsyncForTableBinding(query, request);

                var dtos = new List<AssetDto>();
                if (paginatedEntities.Items.Count > 0)
                {
                    dtos = _mapper.Map<List<AssetDto>>(paginatedEntities.Items);

                    var entityIds = dtos.Select(d => d.Id).ToList();
                    var imagesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Asset, entityIds);
                    if (imagesResult.Succeeded && imagesResult.Data != null)
                    {
                        foreach (var dto in dtos)
                        {
                            if (imagesResult.Data.ContainsKey(dto.Id))
                                dto.Images = imagesResult.Data[dto.Id];
                        }
                    }
                }

                var result = new PaginatedList<AssetDto>(
                    dtos,
                    paginatedEntities.TotalCount,
                    paginatedEntities.PageIndex,
                    request.PageSize);

                return APIOperationResponse<PaginatedList<AssetDto>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting assets by itemId (paged). ItemId: {ItemId}, User: {UserId}", itemId, _currentUserService.UserId);
                return APIOperationResponse<PaginatedList<AssetDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Base query for assets of one catalog item, scoped by optional depot filter(s). When <paramref name="depotIds"/> is non-empty it wins over <paramref name="depotId"/>.
        /// Intersects requested depots with <paramref name="userDepotIds"/> when that list is non-null.
        /// </summary>
        private IQueryable<Asset> QueryAssetsByItemIdWithDepotScope(
            long itemId,
            long? depotId,
            List<long>? depotIds,
            List<long>? userDepotIds,
            out bool emptyBecauseScope)
        {
            emptyBecauseScope = false;
            var query = _assetRepository.Find(
                a => !a.IsDeleted && a.ItemId == itemId,
                false,
                AssetReadMapIncludes);

            List<long>? scope = null;
            if (depotIds != null && depotIds.Count > 0)
                scope = depotIds.Where(x => x > 0).Distinct().ToList();
            else if (depotId.HasValue && depotId.Value > 0)
                scope = new List<long> { depotId.Value };

            if (scope != null && scope.Count > 0)
            {
                if (userDepotIds != null)
                    scope = scope.Where(id => userDepotIds.Contains(id)).ToList();
                if (scope.Count == 0)
                {
                    emptyBecauseScope = true;
                    return query.Where(_ => false);
                }

                return query.Where(a => scope.Contains(a.DepotId));
            }

            if (userDepotIds != null)
                return query.Where(a => userDepotIds.Contains(a.DepotId));

            return query;
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateAssetDto inputDto, List<IFormFile>? files = null)
        {
            _logger.LogInformation("Creating new asset. ItemId: {ItemId}, DepotId: {DepotId}, User: {UserId}", 
                inputDto?.ItemId, inputDto?.DepotId, _currentUserService.UserId);
            
            try
            {
                var userId = _currentUserService.UserId;
                if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, inputDto.DepotId))
                {
                    _logger.LogWarning("User {UserId} attempted to create asset in unauthorized depot {DepotId}", userId, inputDto.DepotId);
                    return APIOperationResponse<long>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
                }

                // Validate input
                var validationResult = await _createValidator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning("Asset validation failed. Errors: {ValidationErrors}, User: {UserId}", 
                        errors, _currentUserService.UserId);
                    
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                // Check for duplicate SerialNumber if provided
                if (!string.IsNullOrWhiteSpace(inputDto.SerialNumber))
                {
                    var existingWithSameSerial = await _assetRepository.FindOneAsync(
                        a => !a.IsDeleted && a.SerialNumber == inputDto.SerialNumber.Trim());

                    if (existingWithSameSerial != null)
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "Serial number already exists");
                }

                var lookupError = await ValidateAssetLookupIdsAsync(inputDto.ItemId, inputDto.SupplierId, inputDto.ManufacturerId, inputDto.PrimaryPurposId);
                if (lookupError != null)
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, lookupError);

                // Resolve BatchNumber into BatchId (get-or-create)
                var batch = await _batchService.GetOrCreateAsync(inputDto.BatchNumber, inputDto.DepotId);

                // Map DTO to entity
                var asset = _mapper.Map<Asset>(inputDto);
                asset.BatchId = batch.Id;
                asset.CreationDate = _dateTimeProvider.Now;
                asset.CreatedBy = _currentUserService.UserId;
                asset.Status = AssetStatus.ReadyToIssue;
                asset.SerialNumber = string.IsNullOrWhiteSpace(inputDto.SerialNumber) ? null : inputDto.SerialNumber.Trim();
                asset.RFID = string.IsNullOrWhiteSpace(inputDto.RFID) ? null : inputDto.RFID.Trim();

                await using var transaction = await _transactionManager.BeginAsync();
                Asset createdAsset;
                try
                {
                    createdAsset = await _assetRepository.AddAsync(asset);
                    var (assignOk, assignError) = await TryApplyIntakeAssignmentAsync(createdAsset, inputDto);
                    if (!assignOk)
                    {
                        await _transactionManager.RollbackAsync();
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest, assignError ?? "Intake assignment failed");
                    }

                    await _transactionManager.CommitAsync();
                }
                catch
                {
                    await _transactionManager.RollbackAsync();
                    throw;
                }

                _logger.LogInformation("Asset created successfully. AssetId: {AssetId}, User: {UserId}",
                                createdAsset.Id, _currentUserService.UserId);

                // Upload files and link them to the created asset
                if (files != null && files.Any())
                {
                    var uploadFilesResult = await _fileUploadService.UploadFilesForEntityAsync(
                        files, 
                        FileEntityType.Weapon,
                        batch.Id);
                    
                    if (!uploadFilesResult.Succeeded)
                    {
                        _logger.LogWarning("File upload failed during asset creation. Error: {Error}, User: {UserId}", 
                            uploadFilesResult.Message, _currentUserService.UserId);
                    }
                    else
                    {
                        _logger.LogInformation("Files uploaded and linked to asset. AssetId: {AssetId}, FileCount: {FileCount}, User: {UserId}",
                            createdAsset.Id, files.Count, _currentUserService.UserId);
                    }
                }

                _logger.LogInformation("Asset creation completed successfully. AssetId: {AssetId}, User: {UserId}", 
                    createdAsset.Id, _currentUserService.UserId);
                
                return APIOperationResponse<long>.Success(createdAsset.Id, "Asset created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating asset. ItemId: {ItemId}, User: {UserId}", 
                    inputDto?.ItemId, _currentUserService.UserId);
             
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<long>>> CreateBulkAsync(string? dtosJson, List<IFormFile>? files = null)
        {
            if (string.IsNullOrWhiteSpace(dtosJson))
                return APIOperationResponse<List<long>>.Fail(ResponseType.BadRequest, "dtosJson is required");

            List<CreateAssetDto>? inputDtos;
            await using var transaction = await _transactionManager.BeginAsync();
            try
            {
                inputDtos = JsonSerializer.Deserialize<List<CreateAssetDto>>(dtosJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException ex)
            {
                return APIOperationResponse<List<long>>.Fail(ResponseType.BadRequest, $"Invalid dtosJson payload: {ex.Message}");
            }

            if (inputDtos == null || !inputDtos.Any())
                return APIOperationResponse<List<long>>.Fail(ResponseType.BadRequest, "No assets provided in dtosJson");

            _logger.LogInformation("Creating bulk assets. Count: {Count}, User: {UserId}",
                inputDtos.Count, _currentUserService.UserId);

            try
            {
                var createdIds = new List<long>();
                var errorMessages = new List<string>();

                // Pre-check for duplicates within the input list
                var duplicateSerials = inputDtos
                    .Where(x => !string.IsNullOrWhiteSpace(x.SerialNumber))
                    .GroupBy(x => x.SerialNumber)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicateSerials.Any())
                    return APIOperationResponse<List<long>>.Fail(ResponseType.BadRequest, $"Duplicate serial numbers in request: {string.Join(", ", duplicateSerials)}");

                var duplicateRfids = inputDtos
                    .Where(x => !string.IsNullOrWhiteSpace(x.RFID))
                    .GroupBy(x => x.RFID)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                if (duplicateRfids.Any())
                    return APIOperationResponse<List<long>>.Fail(ResponseType.BadRequest, $"Duplicate RFIDs in request: {string.Join(", ", duplicateRfids)}");

                // Get all relevant serials and RFIDs to check against DB in one go
                var serialsToCheck = inputDtos.Select(x => x.SerialNumber).Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                var rfidsToCheck = inputDtos.Select(x => x.RFID).Where(r => !string.IsNullOrWhiteSpace(r)).ToList();

                if (serialsToCheck.Any())
                {
                    var existingSerials = await _assetRepository.FindAsync(a => !a.IsDeleted && serialsToCheck.Contains(a.SerialNumber));
                    if (existingSerials.Any())
                    {
                        var foundSerials = existingSerials.Select(a => a.SerialNumber).Distinct();
                        return APIOperationResponse<List<long>>.Fail(ResponseType.BadRequest, $"Serial numbers already exist in database: {string.Join(", ", foundSerials)}");
                    }
                }

                if (rfidsToCheck.Any())
                {
                    var existingRfids = await _assetRepository.FindAsync(a => !a.IsDeleted && rfidsToCheck.Contains(a.RFID));
                    if (existingRfids.Any())
                    {
                        var foundRfids = existingRfids.Select(a => a.RFID).Distinct();
                        return APIOperationResponse<List<long>>.Fail(ResponseType.BadRequest, $"RFIDs already exist in database: {string.Join(", ", foundRfids)}");
                    }
                }

                // Pre-resolve all unique (depot, batch number) pairs
                var batchCache = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
                long batchId = 0L;
                foreach (var dto in inputDtos)
                {
                    var validationResult = await _createValidator.ValidateAsync(dto);
                    if (!validationResult.IsValid)
                    {
                        var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                        errorMessages.Add($"Item {inputDtos.IndexOf(dto) + 1}: {errors}");
                        continue;
                    }

                    var lookupErr = await ValidateAssetLookupIdsAsync(dto.ItemId, dto.SupplierId, dto.ManufacturerId, dto.PrimaryPurposId);
                    if (lookupErr != null)
                    {
                        errorMessages.Add($"Item {inputDtos.IndexOf(dto) + 1}: {lookupErr}");
                        continue;
                    }

                    var batchKey = dto.BatchNumber.Trim();
                    var cacheKey = $"{dto.DepotId}:{batchKey}";

                    var batch = await _batchService.GetOrCreateAsync(dto.BatchNumber, dto.DepotId);
                    batchId=batch.Id;
                    if (!batchCache.ContainsKey(cacheKey))
                        batchCache[cacheKey] = batch.Id;

                    var asset = _mapper.Map<Asset>(dto);
                    asset.BatchId = batchCache[cacheKey];
                    asset.CreationDate = _dateTimeProvider.Now;
                    asset.CreatedBy = _currentUserService.UserId;
                    asset.Status = AssetStatus.ReadyToIssue;
                    asset.SerialNumber = string.IsNullOrWhiteSpace(dto.SerialNumber) ? null : dto.SerialNumber.Trim();
                    asset.RFID = string.IsNullOrWhiteSpace(dto.RFID) ? null : dto.RFID.Trim();

                    await _assetRepository.AddAsync(asset);

                    var (assignOk, assignError) = await TryApplyIntakeAssignmentAsync(asset, dto);
                    if (!assignOk)
                    {
                        errorMessages.Add($"Item {inputDtos.IndexOf(dto) + 1}: {assignError}");
                        continue;
                    }

                    createdIds.Add(asset.Id);
                }

                if (errorMessages.Any())
                {
                    await _transactionManager.RollbackAsync();
                    return APIOperationResponse<List<long>>.Fail(ResponseType.BadRequest, string.Join("; ", errorMessages));
                }

                await _transactionManager.CommitAsync();

                _logger.LogInformation("Bulk asset creation completed successfully. Created: {Count}, User: {UserId}",
                    createdIds.Count, _currentUserService.UserId);

                // If files provided, upload them for each created asset
                if (files != null && files.Any())
                {
                        var uploadFilesResult = await _fileUploadService.UploadFilesForEntityAsync(
                            files,
                            FileEntityType.Weapon,
                            batchId);

                        if (!uploadFilesResult.Succeeded)
                        {
                            _logger.LogWarning("File upload failed during bulk creation for AssetId {AssetId}: {Error}", inputDtos.First().ItemId, uploadFilesResult.Message);
                        }
                }

                return APIOperationResponse<List<long>>.Success(createdIds, "Assets created successfully");
            }
            catch (Exception ex)
            {
                await _transactionManager.RollbackAsync();
                _logger.LogError(ex, "Error creating bulk assets. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<long>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<BulkCreateFromTemplateResultDto>> CreateBulkFromTemplateAsync(CreateBulkAssetsFromTemplateDto dto, List<IFormFile>? files = null)
        {
            if (dto == null)
                return APIOperationResponse<BulkCreateFromTemplateResultDto>.Fail(ResponseType.BadRequest, "Request body is required.");

            _logger.LogInformation("Creating bulk assets from template. Quantity: {Quantity}, ItemId: {ItemId}, DepotId: {DepotId}, User: {UserId}",
                dto.Quantity, dto.ItemId, dto.DepotId, _currentUserService.UserId);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                await using var transaction = await _transactionManager.BeginAsync();

                var validationResult = await _bulkTemplateValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    await _transactionManager.RollbackAsync();
                    return APIOperationResponse<BulkCreateFromTemplateResultDto>.Fail(ResponseType.BadRequest, errors);
                }

                var userId = _currentUserService.UserId;
                if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, dto.DepotId))
                {
                    _logger.LogWarning("User {UserId} attempted bulk template create in unauthorized depot {DepotId}", userId, dto.DepotId);
                    await _transactionManager.RollbackAsync();
                    return APIOperationResponse<BulkCreateFromTemplateResultDto>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
                }

                var templateLookupErr = await ValidateAssetLookupIdsAsync(dto.ItemId, dto.SupplierId, dto.ManufacturerId, dto.PrimaryPurposId);
                if (templateLookupErr != null)
                {
                    await _transactionManager.RollbackAsync();
                    return APIOperationResponse<BulkCreateFromTemplateResultDto>.Fail(ResponseType.BadRequest, templateLookupErr);
                }

                var batch = await _batchService.GetOrCreateAsync(dto.BatchNumber.Trim(), dto.DepotId);

                var templateAsset = _mapper.Map<Asset>(dto);
                templateAsset.ItemId = dto.ItemId;
                templateAsset.SupplierId = dto.SupplierId;
                templateAsset.ManufacturerId = dto.ManufacturerId;
                templateAsset.PrimaryPurposId = dto.PrimaryPurposId;
                templateAsset.DepotId = dto.DepotId;
                templateAsset.BatchId = batch.Id;
                templateAsset.CreationDate = _dateTimeProvider.Now;
                templateAsset.CreatedBy = userId;
                templateAsset.Status = AssetStatus.ReadyToIssue;
                templateAsset.SerialNumber = null;
                templateAsset.RFID = null;

                long? firstAssetId = await _assetBulkSqlRepository.BulkInsertTemplateAssetsAsync(templateAsset, dto.Quantity)
                    .ConfigureAwait(false);
                await _transactionManager.CommitAsync();

                if (files != null && files.Any())
                {
                    var uploadFilesResult = await _fileUploadService.UploadFilesForEntityAsync(
                        files,
                        FileEntityType.Weapon,
                        batch.Id);

                    if (!uploadFilesResult.Succeeded)
                    {
                        _logger.LogWarning(
                            "File upload failed after bulk template creation. BatchId: {BatchId}, Error: {Error}",
                            batch.Id,
                            uploadFilesResult.Message);
                    }
                }

                stopwatch.Stop();

                _logger.LogInformation("Bulk template asset creation completed. Created: {Count}, FirstId: {FirstId}, Duration: {DurationMs}ms, User: {UserId}",
                    dto.Quantity, firstAssetId, stopwatch.ElapsedMilliseconds, userId);

                return APIOperationResponse<BulkCreateFromTemplateResultDto>.Success(
                    new BulkCreateFromTemplateResultDto
                    {
                        CreatedCount = dto.Quantity,
                        FirstAssetId = firstAssetId
                    },
                    $"{dto.Quantity:N0} assets created successfully in {stopwatch.Elapsed.TotalSeconds:F2} seconds.");
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                if (_transactionManager.HasActiveTransaction)
                    await _transactionManager.RollbackAsync();
                _logger.LogError(ex, "Error in CreateBulkFromTemplateAsync. User: {UserId}, Duration: {DurationMs}ms", _currentUserService.UserId, stopwatch.ElapsedMilliseconds);
                return APIOperationResponse<BulkCreateFromTemplateResultDto>.Fail(
                    ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, UpdateAssetDto inputDto, List<IFormFile>? files = null)
        {
            _logger.LogInformation("Updating asset. AssetId: {AssetId}, User: {UserId}", 
                id, _currentUserService.UserId);
            
            try
            {
                // Validate input
                var validationResult = await _updateValidator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                // Check if asset exists
                var existingAsset = await _assetRepository.FindOneAsync(a => a.Id == id && !a.IsDeleted);
                if (existingAsset == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Asset not found");

                // Check for duplicate SerialNumber if provided
                if (!string.IsNullOrWhiteSpace(inputDto.SerialNumber))
                {
                    var duplicate = await _assetRepository.FindOneAsync(
                        a => !a.IsDeleted && a.Id != id && a.SerialNumber == inputDto.SerialNumber.Trim());

                    if (duplicate != null)
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Serial number already exists");
                }

                var updateLookupErr = await ValidateAssetLookupIdsAsync(inputDto.ItemId, inputDto.SupplierId, inputDto.ManufacturerId, inputDto.PrimaryPurposId);
                if (updateLookupErr != null)
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, updateLookupErr);

                // Map updates to entity
                _mapper.Map(inputDto, existingAsset);
                existingAsset.ModificationDate = _dateTimeProvider.Now;
                existingAsset.ModifiedBy = _currentUserService.UserId;
                existingAsset.SerialNumber = string.IsNullOrWhiteSpace(inputDto.SerialNumber) ? null : inputDto.SerialNumber.Trim();
                existingAsset.RFID = string.IsNullOrWhiteSpace(inputDto.RFID) ? null : inputDto.RFID.Trim();

                // Update in repository
                await _assetRepository.UpdateAsync(existingAsset);

                // Upload files (if any) and link them to the asset
                if (files != null && files.Any())
                {
                    var uploadFilesResult = await _fileUploadService.UploadFilesForEntityAsync(
                        files,
                        FileEntityType.Weapon,
                        existingAsset.BatchId);

                    if (!uploadFilesResult.Succeeded)
                    {
                        _logger.LogWarning("File upload failed during asset update. Error: {Error}, User: {UserId}",
                            uploadFilesResult.Message, _currentUserService.UserId);
                    }
                    else
                    {
                        _logger.LogInformation("Files uploaded and linked to asset. AssetId: {AssetId}, FileCount: {FileCount}, User: {UserId}",
                            existingAsset.Id, files.Count, _currentUserService.UserId);
                    }
                }
                
                _logger.LogInformation("Asset updated successfully. AssetId: {AssetId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                
                return APIOperationResponse<bool>.Success(true, "Asset updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating asset. AssetId: {AssetId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateSerialNumberAsync(long assetId, string? serialNumber)
        {
            _logger.LogInformation("Updating serial number for asset. AssetId: {AssetId}, User: {UserId}",
                assetId, _currentUserService.UserId);

            try
            {
                var existingAsset = await _assetRepository.FindOneAsync(a => a.Id == assetId && !a.IsDeleted);
                if (existingAsset == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Asset not found");

                var userId = _currentUserService.UserId;
                if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, existingAsset.DepotId))
                {
                    _logger.LogWarning("User {UserId} attempted to update serial number for asset in unauthorized depot {DepotId}", userId, existingAsset.DepotId);
                    return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
                }

                serialNumber = string.IsNullOrWhiteSpace(serialNumber) ? null : serialNumber.Trim();

                if (!string.IsNullOrEmpty(serialNumber))
                {
                    if (serialNumber.Length > 500)
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Serial number must not exceed 500 characters.");

                    var duplicate = await _assetRepository.FindOneAsync(
                        a => !a.IsDeleted && a.Id != assetId && a.SerialNumber == serialNumber);

                    if (duplicate != null)
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Serial number already exists.");
                }

                existingAsset.SerialNumber = serialNumber;
                existingAsset.ModificationDate = _dateTimeProvider.Now;
                existingAsset.ModifiedBy = _currentUserService.UserId;

                await _assetRepository.UpdateAsync(existingAsset);

                _logger.LogInformation("Serial number updated successfully. AssetId: {AssetId}, SerialNumber: {SerialNumber}, User: {UserId}",
                    assetId, serialNumber ?? "(cleared)", _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(true, "Serial number updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating serial number. AssetId: {AssetId}, User: {UserId}",
                    assetId, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting asset. AssetId: {AssetId}, User: {UserId}", 
                id, _currentUserService.UserId);
            
            try
            {
                var asset = await _assetRepository.FindOneAsync(a => a.Id == id && !a.IsDeleted);
                if (asset == null)
                {
                    _logger.LogWarning("Asset not found for deletion. AssetId: {AssetId}, User: {UserId}", 
                        id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Asset not found");
                }

                var userIdDel = _currentUserService.UserId;
                if (!string.IsNullOrEmpty(userIdDel) &&
                    !await _depotAccessService.HasDepotAccessAsync(userIdDel, asset.DepotId))
                {
                    _logger.LogWarning("User {UserId} attempted to delete asset {AssetId} in unauthorized depot {DepotId}",
                        userIdDel, id, asset.DepotId);
                    return APIOperationResponse<bool>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
                }

                if (asset.IsAssigned)
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Cannot delete assigned asset. Unassign it first.");

                var inSupply = await _assetSupplyDetailRepository
                    .Find(sd => !sd.IsDeleted && sd.AssetId == id)
                    .AnyAsync();

                if (inSupply)
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Cannot delete asset that is in a supply order.");

                // Soft delete - interceptor applies IsDeleted, DeletionDate, DeletedBy
                await _assetRepository.DeleteAsync(asset);

                _logger.LogInformation("Asset deleted successfully. AssetId: {AssetId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true, "Asset deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting asset. AssetId: {AssetId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<ImportResult<CreateAssetDto>>> ImportAsync(IFormFile file, long depotId, string language = "en")
        {
            if (file == null || file.Length == 0)
                return APIOperationResponse<ImportResult<CreateAssetDto>>.Fail(ResponseType.BadRequest, "File is required");

            if (depotId <= 0)
                return APIOperationResponse<ImportResult<CreateAssetDto>>.Fail(ResponseType.BadRequest, "Valid depot ID is required");

            _logger.LogInformation("Starting asset import. DepotId: {DepotId}, Language: {Language}, User: {UserId}", 
                depotId, language, _currentUserService.UserId);

            var userId = _currentUserService.UserId;
            if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, depotId))
            {
                _logger.LogWarning("User {UserId} attempted to import assets to unauthorized depot {DepotId}", userId, depotId);
                return APIOperationResponse<ImportResult<CreateAssetDto>>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
            }
 
            try
            {
                // Parse Excel file
                var mappings = GetColumnMappings(language);
                var importResult = await _excelImportService.ImportFromExcelAsync<AssetImportDto>(file, mappings);

                if (importResult.SuccessCount == 0)
                {
                    _logger.LogWarning("No valid rows found in Excel file. DepotId: {DepotId}, User: {UserId}", 
                        depotId, _currentUserService.UserId);
                    return APIOperationResponse<ImportResult<CreateAssetDto>>.Success(
                        new ImportResult<CreateAssetDto> { Errors = importResult.Errors }, 
                        "Import processed with no valid records");
                }

                // Load all items for ItemNo/ItemName lookup
                var allItems = await LoadAllItemsAsync();

                // Load existing assets for duplicate checking
                var existingAssets = await _assetRepository.FindAsync(
                    a => a.DepotId == depotId && !a.IsDeleted);

                var existingSerialNumbers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var existingRFIDs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var asset in existingAssets)
                {
                    if (!string.IsNullOrWhiteSpace(asset.SerialNumber))
                        existingSerialNumbers.Add(asset.SerialNumber);
                    if (!string.IsNullOrWhiteSpace(asset.RFID))
                        existingRFIDs.Add(asset.RFID);
                }

                await using var transaction = await _transactionManager.BeginAsync();
                try
                {
                    var createAssetDtos = new List<CreateAssetDto>();
                    int processedCount = 0;
                    int errorCount = 0;

                    // Use ToList() to avoid modification during iteration
                    foreach (var row in importResult.SuccessfulRecords.ToList())
                    {
                        var rowErrors = new List<string>();

                        // Resolve ItemId from ItemName or ItemNo
                        long? itemId = row.ItemId;
                        if (!itemId.HasValue)
                        {
                            if (!string.IsNullOrEmpty(row.ItemNo))
                            {
                                var foundItem = allItems.FirstOrDefault(i => i.ItemNo == row.ItemNo);
                                if (foundItem != null)
                                {
                                    itemId = foundItem.Id;
                                }
                            }
                            
                            if (!itemId.HasValue && !string.IsNullOrEmpty(row.ItemName))
                            {
                                // Try to parse "ItemName (ItemNo)" format
                                var itemNameValue = row.ItemName.Trim();
                                if (itemNameValue.Contains("(") && itemNameValue.Contains(")"))
                                {
                                    var startIndex = itemNameValue.LastIndexOf("(");
                                    var endIndex = itemNameValue.LastIndexOf(")");
                                    if (startIndex > 0 && endIndex > startIndex)
                                    {
                                        var extractedItemNo = itemNameValue.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                                        var foundItem = allItems.FirstOrDefault(i => i.ItemNo == extractedItemNo);
                                        if (foundItem != null)
                                        {
                                            itemId = foundItem.Id;
                                        }
                                    }
                                }
                                
                                // If still not found, try matching by name
                                if (!itemId.HasValue)
                                {
                                    var foundItem = allItems.FirstOrDefault(i => 
                                        i.Name != null && i.Name.Equals(itemNameValue, StringComparison.OrdinalIgnoreCase));
                                    if (foundItem != null)
                                    {
                                        itemId = foundItem.Id;
                                    }
                                }
                            }
                        }

                        if (!itemId.HasValue)
                        {
                            rowErrors.Add($"Item not found: ItemName={row.ItemName}, ItemNo={row.ItemNo}, ItemId={row.ItemId}");
                        }

                        // Check for duplicate SerialNumber
                        if (!string.IsNullOrWhiteSpace(row.SerialNumber))
                        {
                            if (existingSerialNumbers.Contains(row.SerialNumber))
                            {
                                rowErrors.Add($"Serial number already exists: {row.SerialNumber}");
                            }
                        }

                        // Check for duplicate RFID
                        if (!string.IsNullOrWhiteSpace(row.RFID))
                        {
                            if (existingRFIDs.Contains(row.RFID))
                            {
                                rowErrors.Add($"RFID already exists: {row.RFID}");
                            }
                        }

                        // If validation failed, move from successful to errors
                        if (rowErrors.Any())
                        {
                            importResult.SuccessfulRecords.Remove(row);
                            importResult.Errors.Add(new ImportError
                            {
                                ErrorMessage = string.Join("; ", rowErrors),
                                ColumnName = "N/A"
                            });
                            errorCount++;
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(row.BatchNumber))
                        {
                            rowErrors.Add("Batch number is required");
                        }

                        if (rowErrors.Any())
                        {
                            importResult.SuccessfulRecords.Remove(row);
                            importResult.Errors.Add(new ImportError
                            {
                                ErrorMessage = string.Join("; ", rowErrors),
                                ColumnName = "N/A"
                            });
                            errorCount++;
                            continue;
                        }

                        var createDto = new CreateAssetDto
                        {
                            ItemId = itemId.Value,
                            BatchNumber = row.BatchNumber!.Trim(),
                            DepotId = depotId,
                            SerialNumber = string.IsNullOrWhiteSpace(row.SerialNumber) ? null : row.SerialNumber.Trim(),
                            RFID = string.IsNullOrWhiteSpace(row.RFID) ? null : row.RFID.Trim(),
                            PurchaseDate = row.PurchaseDate,
                            WarrantyExpiryDate = row.WarrantyExpiryDate,
                            PurchasePrice = row.PurchasePrice,
                            Notes = string.IsNullOrWhiteSpace(row.Notes) ? null : row.Notes.Trim(),
                            SupplierId = row.SupplierId,
                            ManufacturerId = row.ManufacturerId,
                            PrimaryPurposId = row.PrimaryPurposId
                        };

                        var validationResult = await _createValidator.ValidateAsync(createDto);
                        if (!validationResult.IsValid)
                        {
                            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                            importResult.SuccessfulRecords.Remove(row);
                            importResult.Errors.Add(new ImportError
                            {
                                ErrorMessage = $"Validation failed: {errors}",
                                ColumnName = "N/A"
                            });
                            errorCount++;
                            continue;
                        }

                        var importLookupErr = await ValidateAssetLookupIdsAsync(createDto.ItemId, createDto.SupplierId, createDto.ManufacturerId, createDto.PrimaryPurposId);
                        if (importLookupErr != null)
                        {
                            importResult.SuccessfulRecords.Remove(row);
                            importResult.Errors.Add(new ImportError
                            {
                                ErrorMessage = importLookupErr,
                                ColumnName = "N/A"
                            });
                            errorCount++;
                            continue;
                        }

                        var batch = await _batchService.GetOrCreateAsync(createDto.BatchNumber, depotId);
                        var asset = _mapper.Map<Asset>(createDto);
                        asset.BatchId = batch.Id;
                        asset.CreationDate = _dateTimeProvider.Now;
                        asset.CreatedBy = _currentUserService.UserId;
                        asset.Status = AssetStatus.ReadyToIssue;

                        await _assetRepository.AddAsync(asset);
                        createAssetDtos.Add(createDto);
                        processedCount++;

                        // Add to existing sets to prevent duplicates within import
                        if (!string.IsNullOrWhiteSpace(asset.SerialNumber))
                            existingSerialNumbers.Add(asset.SerialNumber);
                        if (!string.IsNullOrWhiteSpace(asset.RFID))
                            existingRFIDs.Add(asset.RFID);

                        _logger.LogInformation("Created asset from import. AssetId: {AssetId}, SerialNumber: {SerialNumber}, DepotId: {DepotId}, User: {UserId}",
                            asset.Id, asset.SerialNumber, depotId, _currentUserService.UserId);
                    }

                    if (processedCount > 0)
                    {
                        await _transactionManager.CommitAsync();
                        _logger.LogInformation("Asset import completed successfully. Processed: {ProcessedCount}, Errors: {ErrorCount}, DepotId: {DepotId}, User: {UserId}",
                            processedCount, errorCount, depotId, _currentUserService.UserId);
                    }
                    else
                    {
                        await _transactionManager.RollbackAsync();
                        _logger.LogWarning("Asset import rolled back - no valid records. DepotId: {DepotId}, User: {UserId}",
                            depotId, _currentUserService.UserId);
                    }

                    // Create result with CreateAssetDto
                    var result = new ImportResult<CreateAssetDto>
                    {
                        SuccessfulRecords = createAssetDtos,
                        Errors = importResult.Errors,
                        TotalProcessed = processedCount + errorCount
                    };

                    return APIOperationResponse<ImportResult<CreateAssetDto>>.Success(result, "Import processed");
                }
                catch (Exception ex)
                {
                    await _transactionManager.RollbackAsync();
                    _logger.LogError(ex, "Error during asset import transaction. DepotId: {DepotId}, User: {UserId}",
                        depotId, _currentUserService.UserId);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing assets. DepotId: {DepotId}, User: {UserId}",
                    depotId, _currentUserService.UserId);
                return APIOperationResponse<ImportResult<CreateAssetDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<ImportResult<AssetImportDto>>> ImportPreviewAsync(IFormFile file, long depotId, string language = "en")
        {
            if (file == null || file.Length == 0)
                return APIOperationResponse<ImportResult<AssetImportDto>>.Fail(ResponseType.BadRequest, "File is required");

            if (depotId <= 0)
                return APIOperationResponse<ImportResult<AssetImportDto>>.Fail(ResponseType.BadRequest, "Valid depot ID is required");

            _logger.LogInformation("Starting asset import preview. DepotId: {DepotId}, Language: {Language}, User: {UserId}", 
                depotId, language, _currentUserService.UserId);

            var userId = _currentUserService.UserId;
            if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, depotId))
            {
                _logger.LogWarning("User {UserId} attempted to preview asset import for unauthorized depot {DepotId}", userId, depotId);
                return APIOperationResponse<ImportResult<AssetImportDto>>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
            }
 
            try
            {
                var mappings = GetColumnMappings(language);
                var importResult = await _excelImportService.ImportFromExcelAsync<AssetImportDto>(file, mappings);

                if (importResult.SuccessCount == 0)
                {
                    _logger.LogWarning("No valid rows found in Excel file for preview. DepotId: {DepotId}, User: {UserId}", 
                        depotId, _currentUserService.UserId);
                    return APIOperationResponse<ImportResult<AssetImportDto>>.Success(importResult, "Preview processed with no valid records");
                }

                var allItems = await LoadAllItemsAsync();

                var existingAssets = await _assetRepository.FindAsync(
                    a => a.DepotId == depotId && !a.IsDeleted);

                var existingSerialNumbers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var existingRFIDs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var asset in existingAssets)
                {
                    if (!string.IsNullOrWhiteSpace(asset.SerialNumber))
                        existingSerialNumbers.Add(asset.SerialNumber);
                    if (!string.IsNullOrWhiteSpace(asset.RFID))
                        existingRFIDs.Add(asset.RFID);
                }

                foreach (var row in importResult.SuccessfulRecords.ToList())
                {
                    var rowErrors = new List<string>();

                    long? itemId = row.ItemId;
                    if (!itemId.HasValue)
                    {
                        if (!string.IsNullOrEmpty(row.ItemNo))
                        {
                            var foundItem = allItems.FirstOrDefault(i => i.ItemNo == row.ItemNo);
                            if (foundItem != null)
                            {
                                itemId = foundItem.Id;
                                row.ItemId = itemId;
                            }
                        }
                        
                        if (!itemId.HasValue && !string.IsNullOrEmpty(row.ItemName))
                        {
                            var itemNameValue = row.ItemName.Trim();
                            if (itemNameValue.Contains("(") && itemNameValue.Contains(")"))
                            {
                                var startIndex = itemNameValue.LastIndexOf("(");
                                var endIndex = itemNameValue.LastIndexOf(")");
                                if (startIndex > 0 && endIndex > startIndex)
                                {
                                    var extractedItemNo = itemNameValue.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
                                    var foundItem = allItems.FirstOrDefault(i => i.ItemNo == extractedItemNo);
                                    if (foundItem != null)
                                    {
                                        itemId = foundItem.Id;
                                        row.ItemId = itemId;
                                    }
                                }
                            }
                            
                            if (!itemId.HasValue)
                            {
                                var foundItem = allItems.FirstOrDefault(i => 
                                    i.Name != null && i.Name.Equals(itemNameValue, StringComparison.OrdinalIgnoreCase));
                                if (foundItem != null)
                                {
                                    itemId = foundItem.Id;
                                    row.ItemId = itemId;
                                }
                            }
                        }
                    }

                    if (!itemId.HasValue)
                    {
                        rowErrors.Add($"Item not found: ItemName={row.ItemName}, ItemNo={row.ItemNo}, ItemId={row.ItemId}");
                    }

                    if (!string.IsNullOrWhiteSpace(row.SerialNumber))
                    {
                        if (existingSerialNumbers.Contains(row.SerialNumber))
                        {
                            rowErrors.Add($"Serial number already exists: {row.SerialNumber}");
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(row.RFID))
                    {
                        if (existingRFIDs.Contains(row.RFID))
                        {
                            rowErrors.Add($"RFID already exists: {row.RFID}");
                        }
                    }

                    if (string.IsNullOrWhiteSpace(row.BatchNumber))
                    {
                        rowErrors.Add("Batch number is required");
                    }

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
                        continue;
                    }

                    var createDto = new CreateAssetDto
                    {
                        ItemId = itemId!.Value,
                        BatchNumber = row.BatchNumber!.Trim(),
                        DepotId = depotId,
                        SerialNumber = string.IsNullOrWhiteSpace(row.SerialNumber) ? null : row.SerialNumber.Trim(),
                        RFID = string.IsNullOrWhiteSpace(row.RFID) ? null : row.RFID.Trim(),
                        PurchaseDate = row.PurchaseDate,
                        WarrantyExpiryDate = row.WarrantyExpiryDate,
                        PurchasePrice = row.PurchasePrice,
                        Notes = string.IsNullOrWhiteSpace(row.Notes) ? null : row.Notes.Trim(),
                        SupplierId = row.SupplierId,
                        ManufacturerId = row.ManufacturerId,
                        PrimaryPurposId = row.PrimaryPurposId
                    };

                    var validationResult = await _createValidator.ValidateAsync(createDto);
                    if (!validationResult.IsValid)
                    {
                        var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                        importResult.SuccessfulRecords.Remove(row);
                        importResult.Errors.Add(new ImportError
                        {
                            RowNumber = row.RowNumber > 0 ? row.RowNumber : 0,
                            ErrorMessage = $"Validation failed: {errors}",
                            ColumnName = "N/A",
                            RowData = row
                        });
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(row.SerialNumber))
                        existingSerialNumbers.Add(row.SerialNumber);
                    if (!string.IsNullOrWhiteSpace(row.RFID))
                        existingRFIDs.Add(row.RFID);
                }

                importResult.TotalProcessed = importResult.SuccessfulRecords.Count + importResult.Errors.Count;

                _logger.LogInformation("Asset import preview completed. Valid: {ValidCount}, Errors: {ErrorCount}, DepotId: {DepotId}, User: {UserId}",
                    importResult.SuccessfulRecords.Count, importResult.Errors.Count, depotId, _currentUserService.UserId);

                return APIOperationResponse<ImportResult<AssetImportDto>>.Success(importResult, "Preview processed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error previewing asset import. DepotId: {DepotId}, User: {UserId}",
                    depotId, _currentUserService.UserId);
                return APIOperationResponse<ImportResult<AssetImportDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<byte[]>> GenerateImportTemplateAsync(long depotId, string language = "en")
        {
            try
            {
                _logger.LogInformation("Generating asset import template. DepotId: {DepotId}, Language: {Language}",
                    depotId, language);

                var userId = _currentUserService.UserId;
                if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, depotId))
                {
                    _logger.LogWarning("User {UserId} attempted to generate import template for unauthorized depot {DepotId}", userId, depotId);
                    return APIOperationResponse<byte[]>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
                }

                var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);

                var weapons = (await _weaponRepository.FindAsync(w => !w.IsDeleted)).ToList();
                var itemNames = weapons.Cast<BaseItem>()
                    .Where(i => !string.IsNullOrWhiteSpace(i.Name) && !string.IsNullOrWhiteSpace(i.ItemNo))
                    .Select(i => $"{i.Name!.Trim()} ({i.ItemNo!.Trim()})")
                    .OrderBy(n => n)
                    .ToList();

                var statusLabels = BatchAssetExcelStatusLabels.GetLabelsForLanguage(language).ToList();
                var assignModeLabels = BatchAssetExcelAssignmentModes.GetLabelsForLanguage(language).ToList();

                var departments = (await _departmentRepository.FindAsync(d => !d.IsDeleted))
                    .OrderBy(d => d.Id)
                    .ToList();
                var departmentLabels = departments
                    .Select(d => isAr
                        ? !string.IsNullOrWhiteSpace(d.NameAr) ? d.NameAr.Trim() : (d.NameEn ?? "").Trim()
                        : !string.IsNullOrWhiteSpace(d.NameEn) ? d.NameEn.Trim() : (d.NameAr ?? "").Trim())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .GroupBy(s => s, StringComparer.OrdinalIgnoreCase).Select(g => g.First())
                    .OrderBy(s => s).ToList();

                var employees = (await _employeeRepository.FindAsync(e => !e.IsDeleted))
                    .OrderBy(e => e.Id)
                    .ToList();
                var employeeLabels = employees
                    .Select(e => FormatEmployeeDisplayForLanguage(e, language))
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .GroupBy(s => s, StringComparer.OrdinalIgnoreCase).Select(g => g.First())
                    .OrderBy(s => s).ToList();

                using var package = new ExcelPackage();
                var ws = package.Workbook.Worksheets.Add("Asset Import");

                var headers = isAr
                    ? new[]
                    {
                        "اسم الصنف", "رقم الدفعة", "رقم التسلسل", "RFID", "الحالة التشغيلية",
                        "تاريخ الشراء", "تاريخ انتهاء الضمان", "سعر الشراء", "إيصال التسليم", "ملاحظات",
                        "وضع التعيين", "القسم", "الموظف", "ملاحظات التخصيص"
                    }
                    : new[]
                    {
                        "Item Name", "Batch Number", "Serial Number", "RFID", "Status",
                        "Purchase Date", "Warranty Expiry Date", "Purchase Price", "Delivery Receipt", "Notes",
                        "Assignment Mode", "Department", "Employee", "Assignment Notes"
                    };

                for (int col = 0; col < headers.Length; col++)
                {
                    ws.Cells[1, col + 1].Value = headers[col];
                    ws.Cells[1, col + 1].Style.Font.Bold = true;
                    ws.Cells[1, col + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Cells[1, col + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    ws.Cells[1, col + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                // Hidden lookup sheets
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

                var assignModesSheet = package.Workbook.Worksheets.Add("AssignmentModes");
                assignModesSheet.Hidden = eWorkSheetHidden.Hidden;
                FillLookupSheet(assignModesSheet, assignModeLabels);

                var departmentsSheet = package.Workbook.Worksheets.Add("Departments");
                departmentsSheet.Hidden = eWorkSheetHidden.Hidden;
                FillLookupSheet(departmentsSheet, departmentLabels);

                var employeesSheet = package.Workbook.Worksheets.Add("Employees");
                employeesSheet.Hidden = eWorkSheetHidden.Hidden;
                FillLookupSheet(employeesSheet, employeeLabels);

                // Data validation dropdowns via dynamic header lookup
                var headerIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < headers.Length; i++)
                    headerIndex[headers[i]] = i + 1;

                void AddListValidation(int colIdx, ExcelWorksheet lookup, string sheetName)
                {
                    var colLetter = GetColumnLetter(colIdx);
                    var range = $"{colLetter}2:{colLetter}10000";
                    var v = ws.DataValidations.AddListValidation(range);
                    var lr = lookup.Dimension?.End.Row ?? 1;
                    v.Formula.ExcelFormula = $"'{sheetName}'!$A$1:$A${lr}";
                    v.ShowErrorMessage = true;
                    v.ErrorTitle = "Invalid Value";
                    v.Error = "Please select a value from the dropdown list";
                    v.ShowInputMessage = true;
                    v.PromptTitle = "Select";
                    v.Prompt = "Choose from the list";
                }

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

                for (int c = 1; c <= headers.Length; c++)
                    ws.Column(c).AutoFit();

                ws.View.FreezePanes(2, 1);

                var excelData = package.GetAsByteArray();

                _logger.LogInformation("Asset import template generated successfully. DepotId: {DepotId}, FileSize: {FileSize} bytes, ItemCount: {ItemCount}",
                    depotId, excelData.Length, itemNames.Count);

                return APIOperationResponse<byte[]>.Success(excelData, "Template generated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating asset import template. DepotId: {DepotId}", depotId);
                return APIOperationResponse<byte[]>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private static string FormatEmployeeDisplayForLanguage(Employee? e, string language)
        {
            if (e == null) return "";
            var isAr = string.Equals(language, "ar", StringComparison.OrdinalIgnoreCase);
            var name = isAr
                ? !string.IsNullOrWhiteSpace(e.NameAr) ? e.NameAr : e.NameEn
                : !string.IsNullOrWhiteSpace(e.NameEn) ? e.NameEn : e.NameAr;
            name = string.IsNullOrWhiteSpace(name) ? "" : name.Trim();
            if (!string.IsNullOrWhiteSpace(e.MilitaryId))
                return $"{name} ({e.MilitaryId.Trim()})".Trim();
            return name;
        }

        private Dictionary<string, string> GetColumnMappings(string language = "en")
        {
            var mappings = new Dictionary<string, string>
            {
                // English headers
                { "Item Name", nameof(AssetImportDto.ItemName) },
                { "Item No", nameof(AssetImportDto.ItemNo) },
                { "Item ID", nameof(AssetImportDto.ItemId) },
                { "Batch Number", nameof(AssetImportDto.BatchNumber) },
                { "Serial Number", nameof(AssetImportDto.SerialNumber) },
                { "RFID", nameof(AssetImportDto.RFID) },
                { "Purchase Date", nameof(AssetImportDto.PurchaseDate) },
                { "Warranty Expiry Date", nameof(AssetImportDto.WarrantyExpiryDate) },
                { "Purchase Price", nameof(AssetImportDto.PurchasePrice) },
                { "Notes", nameof(AssetImportDto.Notes) },
                { "Supplier ID", nameof(AssetImportDto.SupplierId) },
                { "Manufacturer ID", nameof(AssetImportDto.ManufacturerId) },
                { "Primary Purpose ID", nameof(AssetImportDto.PrimaryPurposId) },
 
                // Arabic headers
                { "اسم الصنف", nameof(AssetImportDto.ItemName) },
                { "رقم الصنف", nameof(AssetImportDto.ItemNo) },
                { "رقم التعريف", nameof(AssetImportDto.ItemId) },
                { "رقم الدفعة", nameof(AssetImportDto.BatchNumber) },
                { "رقم التسلسل", nameof(AssetImportDto.SerialNumber) },
                { "RFID*", nameof(AssetImportDto.RFID) }, // Sometimes templates have RFID in English even in Arabic template
                { "تاريخ الشراء", nameof(AssetImportDto.PurchaseDate) },
                { "تاريخ انتهاء الضمان", nameof(AssetImportDto.WarrantyExpiryDate) },
                { "سعر الشراء", nameof(AssetImportDto.PurchasePrice) },
                { "ملاحظات", nameof(AssetImportDto.Notes) },
                { "معرف المورد", nameof(AssetImportDto.SupplierId) },
                { "معرف المصنع", nameof(AssetImportDto.ManufacturerId) },
                { "معرف الغرض الأساسي", nameof(AssetImportDto.PrimaryPurposId) }
            };
 
            // Add RFID without asterisk if needed
            if (!mappings.ContainsKey("RFID")) mappings.Add("RFID", nameof(AssetImportDto.RFID));
 
            return mappings;
        }

        /// <summary>
        /// Load weapon items only – asset import/export targets weapons exclusively.
        /// This keeps import aligned with the UI add-weapon flow which only lists weapons.
        /// </summary>
        private async Task<List<BaseItem>> LoadAllItemsAsync()
        {
            var weapons = (await _weaponRepository.FindAsync(w => !w.IsDeleted)).ToList();

            return weapons.Cast<BaseItem>().ToList();
        }

        private async Task<string?> ValidatePrimaryPurposForItemAsync(long itemId, long? primaryPurposId)
        {
            if (!primaryPurposId.HasValue)
                return null;

            var ok = await _baseItemPrimaryPurposRepository.FindOneAsync(
                x => x.BaseItemId == itemId && x.PrimaryPurposId == primaryPurposId.Value) != null;
            return ok ? null : "Primary purpose is not configured for this catalog item.";
        }

        private async Task<string?> ValidateAssetLookupIdsAsync(long itemId, long? supplierId, long? manufacturerId, long? primaryPurposId)
        {
            if (supplierId.HasValue)
            {
                var okSupplier = await _supplierRepository.FindOneAsync(s => s.Id == supplierId.Value && !s.IsDeleted) != null;
                if (!okSupplier)
                    return "Supplier is invalid or deleted.";
            }

            if (manufacturerId.HasValue)
            {
                var okManufacturer = await _manufacturerRepository.FindOneAsync(m => m.Id == manufacturerId.Value && !m.IsDeleted) != null;
                if (!okManufacturer)
                    return "Manufacturer is invalid or deleted.";
            }

            return await ValidatePrimaryPurposForItemAsync(itemId, primaryPurposId);
        }

        /// <summary>
        /// Convert column number to Excel column letter (1 = A, 2 = B, etc.)
        /// </summary>
        private string GetColumnLetter(int columnNumber)
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
    }
}
