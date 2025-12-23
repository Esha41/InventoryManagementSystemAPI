using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ettad.CrossCutting.Data.Repository;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Comman.Enums;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Ammunitions.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Ettad.Inventory.Services.Common;

namespace Ettad.Inventory.Service.Ammunitions
{
    public class AmmunitionService : IAmmunitionService
    {
        private readonly ICrossCuttingRepository<Ammunition> _ammunitionRepository;
        private readonly ICrossCuttingRepository<FileUplodDetails> _fileDetailsRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateAmmunitionDto> _validator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AmmunitionService> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly IExcelImportService _excelImportService;

        public AmmunitionService(
            ICrossCuttingRepository<Ammunition> ammunitionRepository,
            ICrossCuttingRepository<FileUplodDetails> fileDetailsRepository,
            IMapper mapper,
            IValidator<CreateUpdateAmmunitionDto> validator,
            ICurrentUserService currentUserService,
            ILogger<AmmunitionService> logger,
            IFileUploadService fileUploadService,
            IExcelImportService excelImportService)
        {
            _ammunitionRepository = ammunitionRepository;
            _fileDetailsRepository = fileDetailsRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
            _logger = logger;
            _fileUploadService = fileUploadService;
            _excelImportService = excelImportService;
        }

        public async Task<APIOperationResponse<AmmunitionDto>> GetByIdAsync(long id)
        {
            _logger.LogInformation("Getting ammunition by ID. AmmunitionId: {AmmunitionId}, User: {UserId}", 
                id, _currentUserService.UserId);
            
            try
            {
                var ammunition = await _ammunitionRepository.FindOneAsync(
                    a => a.Id == id && !a.IsDeleted,
                    false,
                    nameof(Ammunition.BulletDiameterUnit),
                    nameof(Ammunition.NatureOption),
                    nameof(Ammunition.PrimaryPurpos),
                    nameof(Ammunition.ProjectileColor),
                    nameof(Ammunition.ProjectailMaterial),
                    nameof(Ammunition.CaseType),
                    nameof(Ammunition.Propellant),
                    nameof(Ammunition.Compatibility),
                    nameof(Ammunition.HazardDivision),
                    nameof(Ammunition.Classification),
                    nameof(Ammunition.Type)
                );

                if (ammunition == null)
                    return APIOperationResponse<AmmunitionDto>.Fail(ResponseType.NotFound, "Ammunition not found");

                var dto = _mapper.Map<AmmunitionDto>(ammunition);
                
                // Get images for this ammunition
                var imagesResult = await _fileUploadService.GetByEntityAsync(FileEntityType.Ammunition, ammunition.Id);
                dto.Images = imagesResult.Succeeded && imagesResult.Data != null ? imagesResult.Data : new List<FileUploadDto>();
                
                _logger.LogInformation("Ammunition retrieved successfully. AmmunitionId: {AmmunitionId}, Name: {Name}, User: {UserId}", 
                    id, dto.Name, _currentUserService.UserId);
                
                return APIOperationResponse<AmmunitionDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ammunition by ID. AmmunitionId: {AmmunitionId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<AmmunitionDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<AmmunitionDto>>> GetAllAsync()
        {
            _logger.LogInformation("Getting all ammunitions. User: {UserId}", _currentUserService.UserId);
            
            try
            {
                var ammunitions = await _ammunitionRepository.FindAsync(
                    a => !a.IsDeleted,
                    false,
                    nameof(Ammunition.BulletDiameterUnit),
                    nameof(Ammunition.NatureOption),
                    nameof(Ammunition.PrimaryPurpos),
                    nameof(Ammunition.ProjectileColor),
                    nameof(Ammunition.ProjectailMaterial),
                    nameof(Ammunition.CaseType),
                    nameof(Ammunition.Propellant),
                    nameof(Ammunition.Compatibility),
                    nameof(Ammunition.HazardDivision),
                    nameof(Ammunition.Classification),
                    nameof(Ammunition.Type)
                );

                var dtos = _mapper.Map<List<AmmunitionDto>>(ammunitions);
                
                // Populate images for all ammunitions in a single database query
                var entityIds = dtos.Select(d => d.Id).ToList();
                var imagesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Ammunition, entityIds);
                if (imagesResult.Succeeded && imagesResult.Data != null)
                {
                    foreach (var dto in dtos)
                    {
                        dto.Images = imagesResult.Data.ContainsKey(dto.Id) ? imagesResult.Data[dto.Id] : new List<FileUploadDto>();
                    }
                }
                
                _logger.LogInformation("All ammunitions retrieved successfully. Count: {Count}, User: {UserId}", 
                    dtos.Count, _currentUserService.UserId);
                
                return APIOperationResponse<List<AmmunitionDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all ammunitions. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<AmmunitionDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<AmmunitionDto>>> GetByTypeAsync(AmmunitionType ammunitionType)
        {
            _logger.LogInformation("Getting ammunitions by type. AmmunitionType: {AmmunitionType}, User: {UserId}", ammunitionType, _currentUserService.UserId);

            try
            {
                var ammunitions = await _ammunitionRepository.FindAsync(
                    a => !a.IsDeleted && a.AmmunitionType == ammunitionType,
                    false,
                    nameof(Ammunition.BulletDiameterUnit),
                    nameof(Ammunition.NatureOption),
                    nameof(Ammunition.PrimaryPurpos),
                    nameof(Ammunition.ProjectileColor),
                    nameof(Ammunition.ProjectailMaterial),
                    nameof(Ammunition.CaseType),
                    nameof(Ammunition.Propellant),
                    nameof(Ammunition.Compatibility),
                    nameof(Ammunition.HazardDivision),
                    nameof(Ammunition.Classification),
                    nameof(Ammunition.Type)
                );

                var dtos = _mapper.Map<List<AmmunitionDto>>(ammunitions);
                
                // Populate images for all ammunitions in a single database query
                var entityIds = dtos.Select(d => d.Id).ToList();
                var imagesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Ammunition, entityIds);
                if (imagesResult.Succeeded && imagesResult.Data != null)
                {
                    foreach (var dto in dtos)
                    {
                        dto.Images = imagesResult.Data.ContainsKey(dto.Id) ? imagesResult.Data[dto.Id] : new List<FileUploadDto>();
                    }
                }
                
                _logger.LogInformation("Ammunitions retrieved by type successfully. AmmunitionType: {AmmunitionType}, Count: {Count}, User: {UserId}", 
                    ammunitionType, dtos.Count, _currentUserService.UserId);
                
                return APIOperationResponse<List<AmmunitionDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ammunitions by type. AmmunitionType: {AmmunitionType}, User: {UserId}", ammunitionType, _currentUserService.UserId);
                return APIOperationResponse<List<AmmunitionDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateUpdateAmmunitionDto inputDto, List<IFormFile>? files = null)
        {
            _logger.LogInformation("Creating new ammunition. Name: {Name}, ItemNo: {ItemNo}, User: {UserId}", 
                inputDto?.Name, inputDto?.ItemNo, _currentUserService.UserId);
            
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning("Ammunition validation failed. Errors: {ValidationErrors}, User: {UserId}", 
                        errors, _currentUserService.UserId);
                    
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                if (!string.IsNullOrWhiteSpace(inputDto.Nsn))
                {
                    var existingWithSameNsn = await _ammunitionRepository.FindOneAsync(
                        a => !a.IsDeleted && a.Nsn == inputDto.Nsn.Trim());

                    if (existingWithSameNsn != null)
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "NSN already exists");
                }
                List<long>? savedFileMasterIds = null;
                if (files != null && files.Any())
                {
                    var saveFilesResult = await _fileUploadService.SaveFilesAsync(files, FileEntityType.Ammunition);
                    if (!saveFilesResult.Succeeded)
                    {
                        _logger.LogWarning("File upload failed during ammunition creation. Error: {Error}, User: {UserId}",
                            saveFilesResult.Message, _currentUserService.UserId);
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                            $"File upload failed: {saveFilesResult.Message}");
                    }
                    savedFileMasterIds = saveFilesResult.Data;
                }
                // Map DTO to entity
                var ammunition = _mapper.Map<Ammunition>(inputDto);
                ammunition.AmmunitionType = AmmunitionType.Small;
                ammunition.ItemType = ItemType.Ammunition;
                ammunition.CreationDate = DateTime.UtcNow;
                ammunition.CreatedBy = _currentUserService.UserId;
                ammunition.Nsn = string.IsNullOrWhiteSpace(inputDto.Nsn) ? null : inputDto.Nsn.Trim();

                // Add to repository first to get the ID
                var createdAmmunition = await _ammunitionRepository.AddAsync(ammunition);
                _logger.LogInformation("Ammunition created successfully. AmmunitionId: {AmmunitionId}, Name: {Name}, User: {UserId}",
                                createdAmmunition.Id, createdAmmunition.Name, _currentUserService.UserId);

                // Upload files and link them to the created ammunition using UploadFilesForEntityAsync
                if (files != null && files.Any())
                {
                    var uploadFilesResult = await _fileUploadService.UploadFilesForEntityAsync(
                        files, 
                        FileEntityType.Ammunition, 
                        createdAmmunition.Id);
                    
                    if (!uploadFilesResult.Succeeded)
                    {
                        _logger.LogWarning("File upload failed during ammunition creation. Error: {Error}, User: {UserId}", 
                            uploadFilesResult.Message, _currentUserService.UserId);
                        // Note: Ammunition is already created, but files failed to upload
                        // This is logged but doesn't fail the operation
                    }
                    else
                    {
                        _logger.LogInformation("Files uploaded and linked to ammunition. AmmunitionId: {AmmunitionId}, FileCount: {FileCount}, User: {UserId}",
                            createdAmmunition.Id, files.Count, _currentUserService.UserId);
                    }
                }

                _logger.LogInformation("Ammunition creation completed successfully. AmmunitionId: {AmmunitionId}, Name: {Name}, User: {UserId}", 
                    createdAmmunition.Id, createdAmmunition.Name, _currentUserService.UserId);
                
                return APIOperationResponse<long>.Success(createdAmmunition.Id, "Ammunition created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ammunition. Name: {Name}, ItemNo: {ItemNo}, User: {UserId}", 
                    inputDto?.Name, inputDto?.ItemNo, _currentUserService.UserId);
             
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateAmmunitionDto inputDto)
        {
            _logger.LogInformation("Updating ammunition. AmmunitionId: {AmmunitionId}, Name: {Name}, User: {UserId}", 
                id, inputDto?.Name, _currentUserService.UserId);
            
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                // Check if ammunition exists
                var existingAmmunition = await _ammunitionRepository.FindOneAsync(a => a.Id == id && !a.IsDeleted);
                if (existingAmmunition == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Ammunition not found");

                if (!string.IsNullOrWhiteSpace(inputDto.Nsn))
                {
                    var duplicate = await _ammunitionRepository.FindOneAsync(
                        a => !a.IsDeleted && a.Id != id && a.Nsn == inputDto.Nsn.Trim());

                    if (duplicate != null)
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "NSN already exists");
                }

                // Map updates to entity
                _mapper.Map(inputDto, existingAmmunition);
                existingAmmunition.AmmunitionType = AmmunitionType.Small;
                existingAmmunition.ModificationDate = DateTime.UtcNow;
                existingAmmunition.ModifiedBy = _currentUserService.UserId;
                existingAmmunition.Nsn = string.IsNullOrWhiteSpace(inputDto.Nsn) ? null : inputDto.Nsn.Trim();

                // Update in repository
                await _ammunitionRepository.UpdateAsync(existingAmmunition);
                
                _logger.LogInformation("Ammunition updated successfully. AmmunitionId: {AmmunitionId}, Name: {Name}, User: {UserId}", 
                    id, existingAmmunition.Name, _currentUserService.UserId);
                
                return APIOperationResponse<bool>.Success(true, "Ammunition updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ammunition. AmmunitionId: {AmmunitionId}, Name: {Name}, User: {UserId}", 
                    id, inputDto?.Name, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting ammunition. AmmunitionId: {AmmunitionId}, User: {UserId}", 
                id, _currentUserService.UserId);
            
            try
            {
                var ammunition = await _ammunitionRepository.FindOneAsync(a => a.Id == id && !a.IsDeleted);
                if (ammunition == null)
                {
                    _logger.LogWarning("Ammunition not found for deletion. AmmunitionId: {AmmunitionId}, User: {UserId}", 
                        id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Ammunition not found");
                }

                var name = ammunition.Name;
                
                // Soft delete - interceptor will handle IsDeleted, DeletionDate, and DeletedBy automatically
                await _ammunitionRepository.DeleteAsync(ammunition);

                _logger.LogInformation("Ammunition deleted successfully. AmmunitionId: {AmmunitionId}, Name: {Name}, User: {UserId}", 
                    id, name, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true, "Ammunition deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ammunition. AmmunitionId: {AmmunitionId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
        public async Task<APIOperationResponse<ImportResult<CreateUpdateAmmunitionDto>>> ImportAsync(IFormFile file)
        {
            _logger.LogInformation("Importing ammunitions from file. FileName: {FileName}, User: {UserId}", 
                file?.FileName, _currentUserService.UserId);
            
            try
            {
                var mappings = GetColumnMappings();
                var importResult = await _excelImportService.ImportFromExcelAsync<CreateUpdateAmmunitionDto>(file, mappings);

                if (importResult.SuccessCount > 0)
                {
                    // Process valid records
                    foreach (var dto in importResult.SuccessfulRecords)
                    {
                        // Optional: Check if exists to prevent duplicates if generic service didn't
                        var validationResult = await _validator.ValidateAsync(dto);
                        if (!validationResult.IsValid)
                        {
                            importResult.Errors.Add(new ImportError 
                            { 
                                ErrorMessage = $"Validation failed: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}", 
                                ColumnName = "N/A" 
                            });
                            continue;
                        }

                        // Use CreateAsync logic but simplified to avoid excessive logging/overhead if needed
                        // Or just call CreateAsync directly
                        var createResult = await CreateAsync(dto);
                        if (!createResult.Succeeded)
                        {
                             importResult.Errors.Add(new ImportError 
                            { 
                                ErrorMessage = $"Creation failed: {createResult.Message}", 
                                ColumnName = "N/A" 
                            });
                        }
                    }
                }

                _logger.LogInformation("Ammunition import completed. FileName: {FileName}, SuccessCount: {SuccessCount}, ErrorCount: {ErrorCount}, User: {UserId}", 
                    file?.FileName, importResult.SuccessCount, importResult.Errors?.Count ?? 0, _currentUserService.UserId);
                
                return APIOperationResponse<ImportResult<CreateUpdateAmmunitionDto>>.Success(importResult, "Import processed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing ammunitions from file. FileName: {FileName}, User: {UserId}", 
                    file?.FileName, _currentUserService.UserId);
                return APIOperationResponse<ImportResult<CreateUpdateAmmunitionDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<ImportResult<CreateUpdateAmmunitionDto>>> ImportPreviewAsync(IFormFile file)
        {
            try
            {
                var mappings = GetColumnMappings();
                var importResult = await _excelImportService.ImportFromExcelAsync<CreateUpdateAmmunitionDto>(file, mappings);

                if (importResult.SuccessCount > 0)
                {
                    // Validate records WITHOUT creating them
                    foreach (var dto in importResult.SuccessfulRecords.ToList())
                    {
                        var validationResult = await _validator.ValidateAsync(dto);
                        if (!validationResult.IsValid)
                        {
                            // Move from successful to errors
                            importResult.SuccessfulRecords.Remove(dto);
                            importResult.Errors.Add(new ImportError
                            {
                                ErrorMessage = $"Validation failed: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}",
                                ColumnName = "N/A"
                            });
                        }
                        else
                        {
                            // Check for duplicate NSN
                            if (!string.IsNullOrWhiteSpace(dto.Nsn))
                            {
                                var existingWithSameNsn = await _ammunitionRepository.FindOneAsync(
                                    a => !a.IsDeleted && a.Nsn == dto.Nsn.Trim());

                                if (existingWithSameNsn != null)
                                {
                                    importResult.SuccessfulRecords.Remove(dto);
                                    importResult.Errors.Add(new ImportError
                                    {
                                        ErrorMessage = $"NSN '{dto.Nsn}' already exists",
                                        ColumnName = "NSN"
                                    });
                                }
                            }
                        }
                    }
                }

                return APIOperationResponse<ImportResult<CreateUpdateAmmunitionDto>>.Success(importResult, "Preview processed");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<ImportResult<CreateUpdateAmmunitionDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        private Dictionary<string, string> GetColumnMappings()
        {
            return new Dictionary<string, string>
            {
                { "Name", nameof(CreateUpdateAmmunitionDto.Name) },
                { "Item No", nameof(CreateUpdateAmmunitionDto.ItemNo) },
                { "Part No", nameof(CreateUpdateAmmunitionDto.PartNo) },
                { "Arm Number", nameof(CreateUpdateAmmunitionDto.ArmNumber) },
                { "Price", nameof(CreateUpdateAmmunitionDto.Price) },
                { "Minimum Quantity", nameof(CreateUpdateAmmunitionDto.MinimumQuantity) },
                { "Bullet Diameter", nameof(CreateUpdateAmmunitionDto.BulletDiameter) },
                { "Is Linked", nameof(CreateUpdateAmmunitionDto.IsLinked) },
                { "Primer", nameof(CreateUpdateAmmunitionDto.Primer) },
                { "Total Weight", nameof(CreateUpdateAmmunitionDto.TotalWeight) },
                { "NSN", nameof(CreateUpdateAmmunitionDto.Nsn) }
            };
        }
    }
}
