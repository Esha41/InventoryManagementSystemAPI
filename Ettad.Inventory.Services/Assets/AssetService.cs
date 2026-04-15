using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using Ettad.CrossCutting.Data.Repository;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Comman.Enums;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.Inventory.Services.Common;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Ettad.Lookups.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Ettad.EntityFramework.DataBaseContext;
using OfficeOpenXml;
using Ettad.CrossCutting.Comman.Time;
using OfficeOpenXml.DataValidation;
using Ettad.CrossCutting.Comman.Models;
using Ettad.Inventory.Service.Batches;
using Ettad.Inventory.Service.Batches.Dtos;
using Ettad.Inventory.Service.AssetHistory;
using Ettad.Inventory.Service.AssetHistory.Dtos;

namespace Ettad.Inventory.Service.Assets
{
    public class AssetService : IAssetService
    {
        // ... (existing fields)

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
                    nameof(Asset.Item),
                    nameof(Asset.Depot),
                    nameof(Asset.Batch),
                    $"{nameof(Asset.Batch)}.{nameof(Batch.Depot)}",
                    nameof(Asset.Supplier),
                    nameof(Asset.Manufacturer),
                    nameof(Asset.PrimaryPurpos),
                    $"{nameof(Asset.CurrentAssignment)}.{nameof(AssetAssignment.Custodian)}",
                    $"{nameof(Asset.CurrentAssignment)}.{nameof(AssetAssignment.Department)}"
                );

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

        private readonly ICrossCuttingRepository<Asset> _assetRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAssetDto> _createValidator;
        private readonly IValidator<UpdateAssetDto> _updateValidator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AssetService> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly IExcelImportService _excelImportService;
        private readonly ApplicationDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IDepotAccessService _depotAccessService;
        private readonly IBatchService _batchService;
        private readonly IValidator<CreateBulkAssetsFromTemplateDto> _bulkTemplateValidator;
        private readonly IAssetHistoryService _historyService;

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
            ApplicationDbContext context,
            IDateTimeProvider dateTimeProvider,
            IDepotAccessService depotAccessService,
            IBatchService batchService,
            IAssetHistoryService historyService)
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
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _depotAccessService = depotAccessService;
            _batchService = batchService;
            _historyService = historyService;
        }

        private static bool WantsIntakeAssignment(CreateAssetDto dto) =>
            dto.AssignToEmployeeId.HasValue || dto.AssignToDepartmentId.HasValue;

        private async Task<(bool Ok, string? Error)> TryApplyIntakeAssignmentAsync(Asset asset, CreateAssetDto dto)
        {
            if (!WantsIntakeAssignment(dto))
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
                    nameof(Asset.Item),
                    nameof(Asset.Depot),
                    nameof(Asset.Batch),
                    $"{nameof(Asset.Batch)}.{nameof(Batch.Depot)}",
                    nameof(Asset.Supplier),
                    nameof(Asset.Manufacturer),
                    nameof(Asset.PrimaryPurpos),
                    $"{nameof(Asset.CurrentAssignment)}.{nameof(AssetAssignment.Custodian)}",
                    $"{nameof(Asset.CurrentAssignment)}.{nameof(AssetAssignment.Department)}"
                );

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
                    nameof(Asset.Item),
                    nameof(Asset.Depot),
                    nameof(Asset.Batch),
                    $"{nameof(Asset.Batch)}.{nameof(Batch.Depot)}",
                    nameof(Asset.Supplier),
                    nameof(Asset.Manufacturer),
                    nameof(Asset.PrimaryPurpos),
                    $"{nameof(Asset.CurrentAssignment)}.{nameof(AssetAssignment.Custodian)}",
                    $"{nameof(Asset.CurrentAssignment)}.{nameof(AssetAssignment.Department)}"
                );

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

        public async Task<APIOperationResponse<List<AssetDto>>> GetAllAsync(long? depotId = null)
        {
            _logger.LogInformation("Getting all assets. DepotId: {DepotId}, User: {UserId}", depotId, _currentUserService.UserId);
            
            try
            {
                if (depotId.HasValue && depotId.Value > 0)
                {
                    var userId = _currentUserService.UserId;
                    if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, depotId.Value))
                    {
                        _logger.LogWarning("User {UserId} attempted to access assets for unauthorized depot {DepotId}", userId, depotId);
                        return APIOperationResponse<List<AssetDto>>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
                    }
                }

                // Build filter predicate
                Expression<Func<Asset, bool>> filter = a => !a.IsDeleted;
                if (depotId.HasValue && depotId.Value > 0)
                {
                    filter = a => !a.IsDeleted && a.DepotId == depotId.Value;
                }

                var assets = await _assetRepository.FindAsync(
                    filter,
                    false,
                    nameof(Asset.Item),
                    nameof(Asset.Depot),
                    nameof(Asset.Batch),
                    $"{nameof(Asset.Batch)}.{nameof(Batch.Depot)}",
                    nameof(Asset.Supplier),
                    nameof(Asset.Manufacturer),
                    nameof(Asset.PrimaryPurpos),
                    $"{nameof(Asset.CurrentAssignment)}.{nameof(AssetAssignment.Custodian)}",
                    $"{nameof(Asset.CurrentAssignment)}.{nameof(AssetAssignment.Department)}"
                );

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

                await using var transaction = await _context.Database.BeginTransactionAsync();
                Asset createdAsset;
                try
                {
                    createdAsset = await _assetRepository.AddAsync(asset);
                    var (assignOk, assignError) = await TryApplyIntakeAssignmentAsync(createdAsset, inputDto);
                    if (!assignOk)
                    {
                        await transaction.RollbackAsync();
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest, assignError ?? "Intake assignment failed");
                    }

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
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

        public async Task<APIOperationResponse<List<long>>> CreateBulkAsync(List<CreateAssetDto> inputDtos, List<IFormFile>? files = null)
        {
            if (inputDtos == null || !inputDtos.Any())
                return APIOperationResponse<List<long>>.Fail(ResponseType.BadRequest, "No assets provided");

            _logger.LogInformation("Creating bulk assets. Count: {Count}, User: {UserId}",
                inputDtos.Count, _currentUserService.UserId);

            await using var transaction = await _context.Database.BeginTransactionAsync();
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
                    await transaction.RollbackAsync();
                    return APIOperationResponse<List<long>>.Fail(ResponseType.BadRequest, string.Join("; ", errorMessages));
                }

                await transaction.CommitAsync();

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
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating bulk assets. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<long>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<BulkCreateFromTemplateResultDto>> CreateBulkFromTemplateAsync(CreateBulkAssetsFromTemplateDto dto)
        {
            if (dto == null)
                return APIOperationResponse<BulkCreateFromTemplateResultDto>.Fail(ResponseType.BadRequest, "Request body is required.");

            _logger.LogInformation("Creating bulk assets from template. Quantity: {Quantity}, ItemId: {ItemId}, DepotId: {DepotId}, User: {UserId}",
                dto.Quantity, dto.ItemId, dto.DepotId, _currentUserService.UserId);

            try
            {
                var validationResult = await _bulkTemplateValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<BulkCreateFromTemplateResultDto>.Fail(ResponseType.BadRequest, errors);
                }

                var userId = _currentUserService.UserId;
                if (!string.IsNullOrEmpty(userId) && !await _depotAccessService.HasDepotAccessAsync(userId, dto.DepotId))
                {
                    _logger.LogWarning("User {UserId} attempted bulk template create in unauthorized depot {DepotId}", userId, dto.DepotId);
                    return APIOperationResponse<BulkCreateFromTemplateResultDto>.Fail(ResponseType.Forbidden, "You do not have access to this depot.");
                }

                var templateLookupErr = await ValidateAssetLookupIdsAsync(dto.ItemId, dto.SupplierId, dto.ManufacturerId, dto.PrimaryPurposId);
                if (templateLookupErr != null)
                    return APIOperationResponse<BulkCreateFromTemplateResultDto>.Fail(ResponseType.BadRequest, templateLookupErr);

                var batch = await _batchService.GetOrCreateAsync(dto.BatchNumber.Trim(), dto.DepotId);
                var now = _dateTimeProvider.Now;
                long? firstAssetId = null;
                const int chunkSize = 1000;

                await using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var remaining = dto.Quantity;
                    while (remaining > 0)
                    {
                        var take = Math.Min(chunkSize, remaining);
                        var chunk = new List<Asset>(take);

                        for (var i = 0; i < take; i++)
                        {
                            var asset = _mapper.Map<Asset>(dto);
                            asset.BatchId = batch.Id;
                            asset.CreationDate = now;
                            asset.CreatedBy = userId;
                            asset.Status = AssetStatus.ReadyToIssue;
                            asset.SerialNumber = null;
                            asset.RFID = null;
                            chunk.Add(asset);
                        }

                        await _context.Assets.AddRangeAsync(chunk);
                        await _context.SaveChangesAsync();

                        firstAssetId ??= chunk[0].Id;
                        remaining -= take;
                    }

                    await transaction.CommitAsync();

                    _logger.LogInformation("Bulk template asset creation completed. Created: {Count}, FirstId: {FirstId}, User: {UserId}",
                        dto.Quantity, firstAssetId, userId);

                    return APIOperationResponse<BulkCreateFromTemplateResultDto>.Success(
                        new BulkCreateFromTemplateResultDto
                        {
                            CreatedCount = dto.Quantity,
                            FirstAssetId = firstAssetId
                        },
                        $"{dto.Quantity} assets created successfully.");
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateBulkFromTemplateAsync. User: {UserId}", _currentUserService.UserId);
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

                // Soft delete - interceptor will handle IsDeleted, DeletionDate, and DeletedBy automatically
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

                // Process valid records with transaction support
                await using var transaction = await _context.Database.BeginTransactionAsync();
                
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

                    // Commit transaction if all successful
                    if (processedCount > 0)
                    {
                        await transaction.CommitAsync();
                        _logger.LogInformation("Asset import completed successfully. Processed: {ProcessedCount}, Errors: {ErrorCount}, DepotId: {DepotId}, User: {UserId}",
                            processedCount, errorCount, depotId, _currentUserService.UserId);
                    }
                    else
                    {
                        await transaction.RollbackAsync();
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
                    await transaction.RollbackAsync();
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

                var weapons = await _context.Weapons.AsNoTracking()
                    .Where(w => !w.IsDeleted).ToListAsync();
                var itemNames = weapons.Cast<BaseItem>()
                    .Where(i => !string.IsNullOrWhiteSpace(i.Name) && !string.IsNullOrWhiteSpace(i.ItemNo))
                    .Select(i => $"{i.Name!.Trim()} ({i.ItemNo!.Trim()})")
                    .OrderBy(n => n)
                    .ToList();

                var statusLabels = BatchAssetExcelStatusLabels.GetLabelsForLanguage(language).ToList();
                var assignModeLabels = BatchAssetExcelAssignmentModes.GetLabelsForLanguage(language).ToList();

                var departments = await _context.Departments.AsNoTracking()
                    .Where(d => !d.IsDeleted).OrderBy(d => d.Id).ToListAsync();
                var departmentLabels = departments
                    .Select(d => isAr
                        ? (!string.IsNullOrWhiteSpace(d.NameAr) ? d.NameAr.Trim() : (d.NameEn ?? "").Trim())
                        : (!string.IsNullOrWhiteSpace(d.NameEn) ? d.NameEn.Trim() : (d.NameAr ?? "").Trim()))
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .GroupBy(s => s, StringComparer.OrdinalIgnoreCase).Select(g => g.First())
                    .OrderBy(s => s).ToList();

                var employees = await _context.Employees.AsNoTracking()
                    .Where(e => !e.IsDeleted).OrderBy(e => e.Id).ToListAsync();
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
                ? (!string.IsNullOrWhiteSpace(e.NameAr) ? e.NameAr : e.NameEn)
                : (!string.IsNullOrWhiteSpace(e.NameEn) ? e.NameEn : e.NameAr);
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
            var weapons = await _context.Weapons
                .Where(w => !w.IsDeleted)
                .ToListAsync();

            return weapons.Cast<BaseItem>().ToList();
        }

        private async Task<string?> ValidatePrimaryPurposForItemAsync(long itemId, long? primaryPurposId)
        {
            if (!primaryPurposId.HasValue)
                return null;

            var ok = await _context.BaseItemPrimaryPurposes
                .AnyAsync(x => x.BaseItemId == itemId && x.PrimaryPurposId == primaryPurposId.Value);
            return ok ? null : "Primary purpose is not configured for this catalog item.";
        }

        private async Task<string?> ValidateAssetLookupIdsAsync(long itemId, long? supplierId, long? manufacturerId, long? primaryPurposId)
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
