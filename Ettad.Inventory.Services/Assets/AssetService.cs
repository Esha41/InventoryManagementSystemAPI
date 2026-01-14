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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Ettad.EntityFramework.DataBaseContext;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;

namespace Ettad.Inventory.Service.Assets
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
        private readonly ApplicationDbContext _context;

        public AssetService(
            ICrossCuttingRepository<Asset> assetRepository,
            IMapper mapper,
            IValidator<CreateAssetDto> createValidator,
            IValidator<UpdateAssetDto> updateValidator,
            ICurrentUserService currentUserService,
            ILogger<AssetService> logger,
            IFileUploadService fileUploadService,
            IExcelImportService excelImportService,
            ApplicationDbContext context)
        {
            _assetRepository = assetRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _currentUserService = currentUserService;
            _logger = logger;
            _fileUploadService = fileUploadService;
            _excelImportService = excelImportService;
            _context = context;
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
                    $"{nameof(Asset.CurrentAssignment)}.{nameof(AssetAssignment.Custodian)}",
                    $"{nameof(Asset.CurrentAssignment)}.{nameof(AssetAssignment.Department)}"
                );

                if (asset == null)
                    return APIOperationResponse<AssetDto>.Fail(ResponseType.NotFound, "Asset not found");

                var dto = _mapper.Map<AssetDto>(asset);
                
                // Get images for this asset
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
                    $"{nameof(Asset.CurrentAssignment)}.{nameof(AssetAssignment.Custodian)}",
                    $"{nameof(Asset.CurrentAssignment)}.{nameof(AssetAssignment.Department)}"
                );

                if (asset == null)
                {
                    _logger.LogWarning("Asset not found by SerialNumber. SerialNumber: {SerialNumber}, User: {UserId}", 
                        trimmedSerialNumber, _currentUserService.UserId);
                    return APIOperationResponse<AssetDto>.Fail(ResponseType.NotFound, "Asset not found");
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

                // Map DTO to entity
                var asset = _mapper.Map<Asset>(inputDto);
                asset.CreationDate = DateTime.Now;
                asset.CreatedBy = _currentUserService.UserId;
                asset.Status = AssetStatus.Active; // Set default status to Active when creating
                asset.SerialNumber = string.IsNullOrWhiteSpace(inputDto.SerialNumber) ? null : inputDto.SerialNumber.Trim();
                asset.RFID = string.IsNullOrWhiteSpace(inputDto.RFID) ? null : inputDto.RFID.Trim();

                // Add to repository first to get the ID
                var createdAsset = await _assetRepository.AddAsync(asset);
                _logger.LogInformation("Asset created successfully. AssetId: {AssetId}, User: {UserId}",
                                createdAsset.Id, _currentUserService.UserId);

                // Upload files and link them to the created asset
                if (files != null && files.Any())
                {
                    var uploadFilesResult = await _fileUploadService.UploadFilesForEntityAsync(
                        files, 
                        FileEntityType.Asset, 
                        createdAsset.Id);
                    
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

        public async Task<APIOperationResponse<List<long>>> CreateBulkAsync(List<CreateAssetDto> inputDtos)
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

                foreach (var dto in inputDtos)
                {
                    // validation
                    var validationResult = await _createValidator.ValidateAsync(dto);
                    if (!validationResult.IsValid)
                    {
                        var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                        errorMessages.Add($"Item {inputDtos.IndexOf(dto) + 1}: {errors}");
                        continue;
                    }

                    var asset = _mapper.Map<Asset>(dto);
                    asset.CreationDate = DateTime.Now;
                    asset.CreatedBy = _currentUserService.UserId;
                    asset.Status = AssetStatus.Active;
                    asset.SerialNumber = string.IsNullOrWhiteSpace(dto.SerialNumber) ? null : dto.SerialNumber.Trim();
                    asset.RFID = string.IsNullOrWhiteSpace(dto.RFID) ? null : dto.RFID.Trim();

                    await _assetRepository.AddAsync(asset);
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

                return APIOperationResponse<List<long>>.Success(createdIds, "Assets created successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating bulk assets. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<long>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, UpdateAssetDto inputDto)
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

                // Map updates to entity
                _mapper.Map(inputDto, existingAsset);
                existingAsset.ModificationDate = DateTime.Now;
                existingAsset.ModifiedBy = _currentUserService.UserId;
                existingAsset.SerialNumber = string.IsNullOrWhiteSpace(inputDto.SerialNumber) ? null : inputDto.SerialNumber.Trim();
                existingAsset.RFID = string.IsNullOrWhiteSpace(inputDto.RFID) ? null : inputDto.RFID.Trim();

                // Update in repository
                await _assetRepository.UpdateAsync(existingAsset);
                
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

        public async Task<APIOperationResponse<ImportResult<CreateAssetDto>>> ImportAsync(IFormFile file, long depotId)
        {
            _logger.LogInformation("Starting asset import. DepotId: {DepotId}, User: {UserId}", 
                depotId, _currentUserService.UserId);

            try
            {
                // Parse Excel file
                var mappings = GetColumnMappings();
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
                var existingAssetTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var asset in existingAssets)
                {
                    if (!string.IsNullOrWhiteSpace(asset.SerialNumber))
                        existingSerialNumbers.Add(asset.SerialNumber);
                    if (!string.IsNullOrWhiteSpace(asset.RFID))
                        existingRFIDs.Add(asset.RFID);
                    if (!string.IsNullOrWhiteSpace(asset.AssetTag))
                        existingAssetTags.Add(asset.AssetTag);
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

                        // Check for duplicate AssetTag
                        if (!string.IsNullOrWhiteSpace(row.AssetTag))
                        {
                            if (existingAssetTags.Contains(row.AssetTag))
                            {
                                rowErrors.Add($"Asset tag already exists: {row.AssetTag}");
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

                        // Create CreateAssetDto from AssetImportDto
                        var createDto = new CreateAssetDto
                        {
                            ItemId = itemId.Value,
                            DepotId = depotId,
                            SerialNumber = string.IsNullOrWhiteSpace(row.SerialNumber) ? null : row.SerialNumber.Trim(),
                            RFID = string.IsNullOrWhiteSpace(row.RFID) ? null : row.RFID.Trim(),
                            AssetTag = string.IsNullOrWhiteSpace(row.AssetTag) ? null : row.AssetTag.Trim(),
                            PurchaseDate = row.PurchaseDate,
                            WarrantyExpiryDate = row.WarrantyExpiryDate,
                            Condition = string.IsNullOrWhiteSpace(row.Condition) ? null : row.Condition.Trim(),
                            PurchasePrice = row.PurchasePrice,
                            Notes = string.IsNullOrWhiteSpace(row.Notes) ? null : row.Notes.Trim()
                        };

                        // Validate using FluentValidation
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

                        // Create asset entity
                        var asset = _mapper.Map<Asset>(createDto);
                        asset.CreationDate = DateTime.Now;
                        asset.CreatedBy = _currentUserService.UserId;
                        asset.Status = AssetStatus.Active;

                        await _assetRepository.AddAsync(asset);
                        createAssetDtos.Add(createDto);
                        processedCount++;

                        // Add to existing sets to prevent duplicates within import
                        if (!string.IsNullOrWhiteSpace(asset.SerialNumber))
                            existingSerialNumbers.Add(asset.SerialNumber);
                        if (!string.IsNullOrWhiteSpace(asset.RFID))
                            existingRFIDs.Add(asset.RFID);
                        if (!string.IsNullOrWhiteSpace(asset.AssetTag))
                            existingAssetTags.Add(asset.AssetTag);

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

        public async Task<APIOperationResponse<ImportResult<CreateAssetDto>>> ImportPreviewAsync(IFormFile file, long depotId)
        {
            _logger.LogInformation("Starting asset import preview. DepotId: {DepotId}, User: {UserId}", 
                depotId, _currentUserService.UserId);

            try
            {
                // Parse Excel file
                var mappings = GetColumnMappings();
                var importResult = await _excelImportService.ImportFromExcelAsync<AssetImportDto>(file, mappings);

                if (importResult.SuccessCount == 0)
                {
                    _logger.LogWarning("No valid rows found in Excel file for preview. DepotId: {DepotId}, User: {UserId}", 
                        depotId, _currentUserService.UserId);
                    return APIOperationResponse<ImportResult<CreateAssetDto>>.Success(
                        new ImportResult<CreateAssetDto> { Errors = importResult.Errors }, 
                        "Preview processed with no valid records");
                }

                // Load all items for ItemNo/ItemName lookup
                var allItems = await LoadAllItemsAsync();

                // Load existing assets for duplicate checking
                var existingAssets = await _assetRepository.FindAsync(
                    a => a.DepotId == depotId && !a.IsDeleted);

                var existingSerialNumbers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var existingRFIDs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var existingAssetTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var asset in existingAssets)
                {
                    if (!string.IsNullOrWhiteSpace(asset.SerialNumber))
                        existingSerialNumbers.Add(asset.SerialNumber);
                    if (!string.IsNullOrWhiteSpace(asset.RFID))
                        existingRFIDs.Add(asset.RFID);
                    if (!string.IsNullOrWhiteSpace(asset.AssetTag))
                        existingAssetTags.Add(asset.AssetTag);
                }

                var createAssetDtos = new List<CreateAssetDto>();

                // Validate records WITHOUT saving to database
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

                    // Check for duplicate AssetTag
                    if (!string.IsNullOrWhiteSpace(row.AssetTag))
                    {
                        if (existingAssetTags.Contains(row.AssetTag))
                        {
                            rowErrors.Add($"Asset tag already exists: {row.AssetTag}");
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
                        continue;
                    }

                    // Create CreateAssetDto from AssetImportDto
                    var createDto = new CreateAssetDto
                    {
                        ItemId = itemId.Value,
                        DepotId = depotId,
                        SerialNumber = string.IsNullOrWhiteSpace(row.SerialNumber) ? null : row.SerialNumber.Trim(),
                        RFID = string.IsNullOrWhiteSpace(row.RFID) ? null : row.RFID.Trim(),
                        AssetTag = string.IsNullOrWhiteSpace(row.AssetTag) ? null : row.AssetTag.Trim(),
                        PurchaseDate = row.PurchaseDate,
                        WarrantyExpiryDate = row.WarrantyExpiryDate,
                        Condition = string.IsNullOrWhiteSpace(row.Condition) ? null : row.Condition.Trim(),
                        PurchasePrice = row.PurchasePrice,
                        Notes = string.IsNullOrWhiteSpace(row.Notes) ? null : row.Notes.Trim()
                    };

                    // Validate using FluentValidation
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
                        continue;
                    }

                    createAssetDtos.Add(createDto);

                    // Add to existing sets to prevent duplicates within preview
                    if (!string.IsNullOrWhiteSpace(createDto.SerialNumber))
                        existingSerialNumbers.Add(createDto.SerialNumber);
                    if (!string.IsNullOrWhiteSpace(createDto.RFID))
                        existingRFIDs.Add(createDto.RFID);
                    if (!string.IsNullOrWhiteSpace(createDto.AssetTag))
                        existingAssetTags.Add(createDto.AssetTag);
                }

                // Create result with CreateAssetDto
                var result = new ImportResult<CreateAssetDto>
                {
                    SuccessfulRecords = createAssetDtos,
                    Errors = importResult.Errors,
                    TotalProcessed = createAssetDtos.Count + importResult.Errors.Count
                };

                _logger.LogInformation("Asset import preview completed. Valid: {ValidCount}, Errors: {ErrorCount}, DepotId: {DepotId}, User: {UserId}",
                    createAssetDtos.Count, importResult.Errors.Count, depotId, _currentUserService.UserId);

                return APIOperationResponse<ImportResult<CreateAssetDto>>.Success(result, "Preview processed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error previewing asset import. DepotId: {DepotId}, User: {UserId}",
                    depotId, _currentUserService.UserId);
                return APIOperationResponse<ImportResult<CreateAssetDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<byte[]>> GenerateImportTemplateAsync(long depotId, string language = "en")
        {
            try
            {
                _logger.LogInformation("Generating asset import template. DepotId: {DepotId}, Language: {Language}", 
                    depotId, language);

                // Load all items (ammunition, weapons, explosives)
                var ammunitions = await _context.Ammunitions
                    .Where(a => !a.IsDeleted)
                    .ToListAsync();
                
                var weapons = await _context.Weapons
                    .Where(w => !w.IsDeleted)
                    .ToListAsync();
                
                var explosives = await _context.Explosives
                    .Where(e => !e.IsDeleted)
                    .ToListAsync();

                // Combine all items and format as "ItemName (ItemNo)"
                var allItems = new List<BaseItem>();
                allItems.AddRange(ammunitions.Cast<BaseItem>());
                allItems.AddRange(weapons.Cast<BaseItem>());
                allItems.AddRange(explosives.Cast<BaseItem>());

                var itemNames = allItems
                    .Where(i => !string.IsNullOrWhiteSpace(i.Name) && !string.IsNullOrWhiteSpace(i.ItemNo))
                    .Select(i => $"{i.Name} ({i.ItemNo})")
                    .OrderBy(n => n)
                    .ToList();

                // Generate Excel with EPPlus
                using var package = new ExcelPackage();
                
                // Main template sheet
                var templateSheet = package.Workbook.Worksheets.Add("Asset Import");

                // Headers - Bilingual support (English / Arabic)
                var headers = language == "ar"
                    ? new[]
                    {
                        "اسم الصنف", "رقم الصنف", "رقم التسلسل", "RFID", "علامة الأصل",
                        "تاريخ الشراء", "تاريخ انتهاء الضمان", "الحالة", "سعر الشراء", "ملاحظات"
                    }
                    : new[]
                    {
                        "Item Name", "Item No", "Serial Number", "RFID", "Asset Tag",
                        "Purchase Date", "Warranty Expiry Date", "Condition", "Purchase Price", "Notes"
                    };

                // Add headers with formatting
                for (int col = 1; col <= headers.Length; col++)
                {
                    templateSheet.Cells[1, col].Value = headers[col - 1];
                    templateSheet.Cells[1, col].Style.Font.Bold = true;
                    templateSheet.Cells[1, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    templateSheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    templateSheet.Cells[1, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                // Sample data row
                templateSheet.Cells[2, 1].Value = itemNames.FirstOrDefault() ?? "";
                templateSheet.Cells[2, 2].Value = "";
                templateSheet.Cells[2, 3].Value = "";
                templateSheet.Cells[2, 4].Value = "";
                templateSheet.Cells[2, 5].Value = "";
                templateSheet.Cells[2, 6].Value = DateTime.Now.ToString("yyyy-MM-dd");
                templateSheet.Cells[2, 7].Value = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
                templateSheet.Cells[2, 8].Value = "";
                templateSheet.Cells[2, 9].Value = 0;
                templateSheet.Cells[2, 10].Value = "";

                // Create hidden lookup sheet for items
                var lookupSheet = package.Workbook.Worksheets.Add("Items");
                lookupSheet.Hidden = eWorkSheetHidden.Hidden;
                for (int i = 0; i < itemNames.Count; i++)
                {
                    lookupSheet.Cells[i + 1, 1].Value = itemNames[i];
                }

                // Add data validation dropdown for Item Name (column 1)
                var itemNameColumn = GetColumnLetter(1);
                var itemNameValidationRange = $"{itemNameColumn}2:{itemNameColumn}10000";
                var itemNameValidation = templateSheet.DataValidations.AddListValidation(itemNameValidationRange);
                var lastRow = lookupSheet.Dimension?.End.Row ?? 1;
                itemNameValidation.Formula.ExcelFormula = $"'Items'!$A$1:$A${lastRow}";
                itemNameValidation.ShowErrorMessage = true;
                itemNameValidation.ErrorTitle = "Invalid Value";
                itemNameValidation.Error = "Please select an item from the dropdown list";
                itemNameValidation.ShowInputMessage = true;
                itemNameValidation.PromptTitle = "Select Item";
                itemNameValidation.Prompt = "Select an item from the dropdown list";

                // Set column widths
                templateSheet.Column(1).Width = 30; // Item Name
                templateSheet.Column(2).Width = 15; // Item No
                templateSheet.Column(3).Width = 20; // Serial Number
                templateSheet.Column(4).Width = 20; // RFID
                templateSheet.Column(5).Width = 15; // Asset Tag
                templateSheet.Column(6).Width = 15; // Purchase Date
                templateSheet.Column(7).Width = 20; // Warranty Expiry Date
                templateSheet.Column(8).Width = 15; // Condition
                templateSheet.Column(9).Width = 15; // Purchase Price
                templateSheet.Column(10).Width = 30; // Notes

                // Freeze header row
                templateSheet.View.FreezePanes(2, 1);

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

        private Dictionary<string, string> GetColumnMappings()
        {
            return new Dictionary<string, string>
            {
                { "Item Name", nameof(AssetImportDto.ItemName) },
                { "Item No", nameof(AssetImportDto.ItemNo) },
                { "Item ID", nameof(AssetImportDto.ItemId) },
                { "Serial Number", nameof(AssetImportDto.SerialNumber) },
                { "RFID", nameof(AssetImportDto.RFID) },
                { "Asset Tag", nameof(AssetImportDto.AssetTag) },
                { "Purchase Date", nameof(AssetImportDto.PurchaseDate) },
                { "Warranty Expiry Date", nameof(AssetImportDto.WarrantyExpiryDate) },
                { "Condition", nameof(AssetImportDto.Condition) },
                { "Purchase Price", nameof(AssetImportDto.PurchasePrice) },
                { "Notes", nameof(AssetImportDto.Notes) },
                // Arabic column names
                { "اسم الصنف", nameof(AssetImportDto.ItemName) },
                { "رقم الصنف", nameof(AssetImportDto.ItemNo) },
                { "رقم التسلسل", nameof(AssetImportDto.SerialNumber) },
                // Note: "RFID" is the same in both languages, so it's already mapped above
                { "علامة الأصل", nameof(AssetImportDto.AssetTag) },
                { "تاريخ الشراء", nameof(AssetImportDto.PurchaseDate) },
                { "تاريخ انتهاء الضمان", nameof(AssetImportDto.WarrantyExpiryDate) },
                { "الحالة", nameof(AssetImportDto.Condition) },
                { "سعر الشراء", nameof(AssetImportDto.PurchasePrice) },
                { "ملاحظات", nameof(AssetImportDto.Notes) }
            };
        }

        private async Task<List<BaseItem>> LoadAllItemsAsync()
        {
            var items = new List<BaseItem>();
            
            // Load all ammunition, weapons, and explosives
            var ammunitions = await _context.Ammunitions
                .Where(a => !a.IsDeleted)
                .ToListAsync();
            
            var weapons = await _context.Weapons
                .Where(w => !w.IsDeleted)
                .ToListAsync();
            
            var explosives = await _context.Explosives
                .Where(e => !e.IsDeleted)
                .ToListAsync();

            items.AddRange(ammunitions.Cast<BaseItem>());
            items.AddRange(weapons.Cast<BaseItem>());
            items.AddRange(explosives.Cast<BaseItem>());

            return items;
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
