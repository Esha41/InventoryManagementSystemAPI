using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ettad.CrossCutting.Data.Repository;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Comman.Enums;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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

        public AssetService(
            ICrossCuttingRepository<Asset> assetRepository,
            IMapper mapper,
            IValidator<CreateAssetDto> createValidator,
            IValidator<UpdateAssetDto> updateValidator,
            ICurrentUserService currentUserService,
            ILogger<AssetService> logger,
            IFileUploadService fileUploadService)
        {
            _assetRepository = assetRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _currentUserService = currentUserService;
            _logger = logger;
            _fileUploadService = fileUploadService;
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
                    nameof(Asset.Department),
                    nameof(Asset.Custodian)
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

        public async Task<APIOperationResponse<List<AssetDto>>> GetAllAsync()
        {
            _logger.LogInformation("Getting all assets. User: {UserId}", _currentUserService.UserId);
            
            try
            {
                var assets = await _assetRepository.FindAsync(
                    a => !a.IsDeleted,
                    false,
                    nameof(Asset.Item),
                    nameof(Asset.Depot),
                    nameof(Asset.Department),
                    nameof(Asset.Custodian)
                );

                var dtos = _mapper.Map<List<AssetDto>>(assets);
                
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
                asset.CreationDate = DateTime.UtcNow;
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
                existingAsset.ModificationDate = DateTime.UtcNow;
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
    }
}
