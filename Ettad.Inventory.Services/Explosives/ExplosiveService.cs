using AutoMapper;
using FluentValidation;
using Ettad.CrossCutting.Data.Repository;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Comman.Enums;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Explosives.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Ettad.Inventory.Services.Common;

namespace Ettad.Inventory.Service.Explosives
{
    public class ExplosiveService : IExplosiveService
    {
        private readonly ICrossCuttingRepository<Explosive> _explosiveRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateExplosiveDto> _validator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ExplosiveService> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly IExcelImportService _excelImportService;

        public ExplosiveService(
            ICrossCuttingRepository<Explosive> explosiveRepository,
            IMapper mapper,
            IValidator<CreateUpdateExplosiveDto> validator,
            ICurrentUserService currentUserService,
            ILogger<ExplosiveService> logger,
            IFileUploadService fileUploadService,
            IExcelImportService excelImportService)
        {
            _explosiveRepository = explosiveRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
            _logger = logger;
            _fileUploadService = fileUploadService;
            _excelImportService = excelImportService;
        }

        public async Task<APIOperationResponse<ExplosiveDto>> GetByIdAsync(long id)
        {
            try
            {
                var explosive = await _explosiveRepository.FindOneAsync(
                    e => e.Id == id && !e.IsDeleted,
                    false,
                    nameof(Explosive.NetExplosiveQuantityUnit),
                    nameof(Explosive.TotalWeightUnit),
                    nameof(Explosive.HazardDivision),
                    nameof(Explosive.Compatibility)
                );

                if (explosive == null)
                    return APIOperationResponse<ExplosiveDto>.Fail(ResponseType.NotFound, "Explosive not found");

                var dto = _mapper.Map<ExplosiveDto>(explosive);
                return APIOperationResponse<ExplosiveDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<ExplosiveDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<ExplosiveDto>>> GetAllAsync()
        {
            try
            {
                var explosives = await _explosiveRepository.FindAsync(
                    e => !e.IsDeleted,
                    false,
                    nameof(Explosive.NetExplosiveQuantityUnit),
                    nameof(Explosive.TotalWeightUnit),
                    nameof(Explosive.HazardDivision),
                    nameof(Explosive.Compatibility)
                );

                var dtos = _mapper.Map<List<ExplosiveDto>>(explosives);
                return APIOperationResponse<List<ExplosiveDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<ExplosiveDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<ExplosiveDto>>> GetByTypeAsync(ExplosiveType explosiveType)
        {
            _logger.LogInformation("Getting explosives by type. ExplosiveType: {ExplosiveType}, User: {UserId}", explosiveType, _currentUserService.UserId);

            try
            {
                var explosives = await _explosiveRepository.FindAsync(
                    e => !e.IsDeleted && e.ExplosiveType == explosiveType,
                    false,
                    nameof(Explosive.NetExplosiveQuantityUnit),
                    nameof(Explosive.TotalWeightUnit),
                    nameof(Explosive.HazardDivision),
                    nameof(Explosive.Compatibility)
                );

                var dtos = _mapper.Map<List<ExplosiveDto>>(explosives);
                return APIOperationResponse<List<ExplosiveDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving explosives by type. ExplosiveType: {ExplosiveType}, User: {UserId}", explosiveType, _currentUserService.UserId);
                return APIOperationResponse<List<ExplosiveDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateUpdateExplosiveDto inputDto, List<IFormFile>? files = null)
        {
            _logger.LogInformation("Creating new explosive. Name: {Name}, ItemNo: {ItemNo}, User: {UserId}", 
                inputDto?.Name, inputDto?.ItemNo, _currentUserService.UserId);
            
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning("Explosive validation failed. Errors: {ValidationErrors}, User: {UserId}", 
                        errors, _currentUserService.UserId);
                    
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                if (!string.IsNullOrWhiteSpace(inputDto.Nsn))
                {
                    var existingWithSameNsn = await _explosiveRepository.FindOneAsync(
                        e => !e.IsDeleted && e.Nsn == inputDto.Nsn.Trim());

                    if (existingWithSameNsn != null)
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "NSN already exists");
                }

                if (files != null && files.Any())
                {
                    var saveFilesResult = await _fileUploadService.SaveFilesAsync(files, FileEntityType.Explosive);
                
                    if (!saveFilesResult.Succeeded)
                    {
                        _logger.LogWarning("File upload failed during explosive creation. Error: {Error}, User: {UserId}",
                            saveFilesResult.Message, _currentUserService.UserId);
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                            $"File upload failed: {saveFilesResult.Message}");
                    }
                }

                // Map DTO to entity
                var explosive = _mapper.Map<Explosive>(inputDto);
                explosive.ItemType = ItemType.Explosive;
                explosive.CreationDate = DateTime.UtcNow;
                explosive.CreatedBy = _currentUserService.UserId;
                explosive.Nsn = string.IsNullOrWhiteSpace(inputDto.Nsn) ? null : inputDto.Nsn.Trim();

                // Add to repository first to get the ID
                var createdExplosive = await _explosiveRepository.AddAsync(explosive);
                _logger.LogInformation("Explosive created successfully. ExplosiveId: {ExplosiveId}, Name: {Name}, User: {UserId}",
                                createdExplosive.Id, createdExplosive.Name, _currentUserService.UserId);

                // Upload files and link them to the created explosive
                if (files != null && files.Any())
                {
                    var uploadFilesResult = await _fileUploadService.UploadFilesForEntityAsync(
                        files, 
                        FileEntityType.Explosive, 
                        createdExplosive.Id);
                    
                    if (!uploadFilesResult.Succeeded)
                    {
                        _logger.LogWarning("File upload failed during explosive creation. Error: {Error}, User: {UserId}", 
                            uploadFilesResult.Message, _currentUserService.UserId);
                    }
                    else
                    {
                        _logger.LogInformation("Files uploaded and linked to explosive. ExplosiveId: {ExplosiveId}, FileCount: {FileCount}, User: {UserId}",
                            createdExplosive.Id, files.Count, _currentUserService.UserId);
                    }
                }

                return APIOperationResponse<long>.Success(createdExplosive.Id, "Explosive created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating explosive. Name: {Name}, User: {UserId}", 
                    inputDto?.Name, _currentUserService.UserId);
             
                var msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += $" (Inner: {ex.InnerException.Message})";
                }
             
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {msg}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateExplosiveDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                // Check if explosive exists
                var existingExplosive = await _explosiveRepository.FindOneAsync(e => e.Id == id && !e.IsDeleted);
                if (existingExplosive == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Explosive not found");

                if (!string.IsNullOrWhiteSpace(inputDto.Nsn))
                {
                    var duplicate = await _explosiveRepository.FindOneAsync(
                        e => !e.IsDeleted && e.Id != id && e.Nsn == inputDto.Nsn.Trim());

                    if (duplicate != null)
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "NSN already exists");
                }

                // Map updates to entity
                _mapper.Map(inputDto, existingExplosive);
                existingExplosive.ModificationDate = DateTime.UtcNow;
                existingExplosive.ModifiedBy = _currentUserService.UserId;
                existingExplosive.Nsn = string.IsNullOrWhiteSpace(inputDto.Nsn) ? null : inputDto.Nsn.Trim();

                // Update in repository
                await _explosiveRepository.UpdateAsync(existingExplosive);
                return APIOperationResponse<bool>.Success(true, "Explosive updated successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting explosive. ExplosiveId: {ExplosiveId}, User: {UserId}", 
                id, _currentUserService.UserId);
            
            try
            {
                var explosive = await _explosiveRepository.FindOneAsync(e => e.Id == id && !e.IsDeleted);
                if (explosive == null)
                {
                    _logger.LogWarning("Explosive not found for deletion. ExplosiveId: {ExplosiveId}, User: {UserId}", 
                        id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Explosive not found");
                }

                var name = explosive.Name;
                
                await _explosiveRepository.DeleteAsync(explosive);

                _logger.LogInformation("Explosive deleted successfully. ExplosiveId: {ExplosiveId}, Name: {Name}, User: {UserId}", 
                    id, name, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true, "Explosive deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting explosive. ExplosiveId: {ExplosiveId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<ImportResult<CreateUpdateExplosiveDto>>> ImportAsync(IFormFile file)
        {
            try
            {
                var mappings = GetColumnMappings();
                var importResult = await _excelImportService.ImportFromExcelAsync<CreateUpdateExplosiveDto>(file, mappings);

                if (importResult.SuccessCount > 0)
                {
                    foreach (var dto in importResult.SuccessfulRecords)
                    {
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

                return APIOperationResponse<ImportResult<CreateUpdateExplosiveDto>>.Success(importResult, "Import processed");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<ImportResult<CreateUpdateExplosiveDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<ImportResult<CreateUpdateExplosiveDto>>> ImportPreviewAsync(IFormFile file)
        {
            try
            {
                var mappings = GetColumnMappings();
                var importResult = await _excelImportService.ImportFromExcelAsync<CreateUpdateExplosiveDto>(file, mappings);

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
                                var existingWithSameNsn = await _explosiveRepository.FindOneAsync(
                                    e => !e.IsDeleted && e.Nsn == dto.Nsn.Trim());

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

                return APIOperationResponse<ImportResult<CreateUpdateExplosiveDto>>.Success(importResult, "Preview processed");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<ImportResult<CreateUpdateExplosiveDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        private Dictionary<string, string> GetColumnMappings()
        {
            return new Dictionary<string, string>
            {
                { "Name", nameof(CreateUpdateExplosiveDto.Name) },
                { "Item No", nameof(CreateUpdateExplosiveDto.ItemNo) },
                { "Part No", nameof(CreateUpdateExplosiveDto.PartNo) },
                { "Price", nameof(CreateUpdateExplosiveDto.Price) },
                { "Minimum Quantity", nameof(CreateUpdateExplosiveDto.MinimumQuantity) },
                { "NSN", nameof(CreateUpdateExplosiveDto.Nsn) },
                { "Explosive Type", nameof(CreateUpdateExplosiveDto.ExplosiveType) },
                { "UN Number", nameof(CreateUpdateExplosiveDto.UNNumber) },
                { "Net Explosive Quantity", nameof(CreateUpdateExplosiveDto.NetExplosiveQuantity) },
                { "Total Weight", nameof(CreateUpdateExplosiveDto.TotalWeight) }
            };
        }
    }
}
