using AutoMapper;
using FluentValidation;
using Ettad.CrossCutting.Data.Repository;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Comman.Enums;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Weapons.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Ettad.Inventory.Services.Common;

namespace Ettad.Inventory.Service.Weapons
{
    public class WeaponService : IWeaponService
    {
        private readonly ICrossCuttingRepository<Weapon> _weaponRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateWeaponDto> _validator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<WeaponService> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly IExcelImportService _excelImportService;

        public WeaponService(
            ICrossCuttingRepository<Weapon> weaponRepository,
            IMapper mapper,
            IValidator<CreateUpdateWeaponDto> validator,
            ICurrentUserService currentUserService,
            ILogger<WeaponService> logger,
            IFileUploadService fileUploadService,
            IExcelImportService excelImportService)
        {
            _weaponRepository = weaponRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
            _logger = logger;
            _fileUploadService = fileUploadService;
            _excelImportService = excelImportService;
        }

        public async Task<APIOperationResponse<WeaponDto>> GetByIdAsync(long id)
        {
            _logger.LogInformation("Getting weapon by ID. WeaponId: {WeaponId}, User: {UserId}", 
                id, _currentUserService.UserId);
            
            try
            {
                var weapon = await _weaponRepository.FindOneAsync(
                    w => w.Id == id && !w.IsDeleted,
                    false,
                    nameof(Weapon.CaliberUnit),
                    nameof(Weapon.CountryOfManufacture),
                    nameof(Weapon.Classification),
                    nameof(Weapon.Type)
                );

                if (weapon == null)
                {
                    _logger.LogWarning("Weapon not found. WeaponId: {WeaponId}, User: {UserId}", 
                        id, _currentUserService.UserId);
                    return APIOperationResponse<WeaponDto>.Fail(ResponseType.NotFound, "Weapon not found");
                }

                var dto = _mapper.Map<WeaponDto>(weapon);
                
                // Get images for this weapon
                var imagesResult = await _fileUploadService.GetByEntityAsync(FileEntityType.Weapon, weapon.Id);
                dto.Images = imagesResult.Succeeded && imagesResult.Data != null ? imagesResult.Data : new List<FileUploadDto>();
                
                _logger.LogInformation("Weapon retrieved successfully. WeaponId: {WeaponId}, Name: {Name}, User: {UserId}", 
                    id, dto.Name, _currentUserService.UserId);
                
                return APIOperationResponse<WeaponDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving weapon by ID. WeaponId: {WeaponId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<WeaponDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<WeaponDto>>> GetAllAsync()
        {
            _logger.LogInformation("Getting all weapons. User: {UserId}", _currentUserService.UserId);
            
            try
            {
                var weapons = await _weaponRepository.FindAsync(
                    w => !w.IsDeleted,
                    false,
                    nameof(Weapon.CaliberUnit),
                    nameof(Weapon.CountryOfManufacture),
                    nameof(Weapon.Classification),
                    nameof(Weapon.Type)
                );

                var dtos = _mapper.Map<List<WeaponDto>>(weapons);
                
                // Populate images for all weapons in a single database query
                var entityIds = dtos.Select(d => d.Id).ToList();
                var imagesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Weapon, entityIds);
                if (imagesResult.Succeeded && imagesResult.Data != null)
                {
                    foreach (var dto in dtos)
                    {
                        dto.Images = imagesResult.Data.ContainsKey(dto.Id) ? imagesResult.Data[dto.Id] : new List<FileUploadDto>();
                    }
                }
                
                _logger.LogInformation("All weapons retrieved successfully. Count: {Count}, User: {UserId}", 
                    dtos.Count, _currentUserService.UserId);
                
                return APIOperationResponse<List<WeaponDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all weapons. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<WeaponDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }



        public async Task<APIOperationResponse<long>> CreateAsync(CreateUpdateWeaponDto inputDto, List<IFormFile>? files = null)
        {
            _logger.LogInformation("Creating new weapon. Name: {Name}, ItemNo: {ItemNo}, User: {UserId}", 
                inputDto?.Name, inputDto?.ItemNo, _currentUserService.UserId);
            
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning("Weapon validation failed. Errors: {ValidationErrors}, User: {UserId}", 
                        errors, _currentUserService.UserId);
                    
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                if (!string.IsNullOrWhiteSpace(inputDto.Nsn))
                {
                    var existingWithSameNsn = await _weaponRepository.FindOneAsync(
                        w => !w.IsDeleted && w.Nsn == inputDto.Nsn.Trim());

                    if (existingWithSameNsn != null)
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "NSN already exists");
                }

                if (files != null && files.Any())
                {
                    var saveFilesResult = await _fileUploadService.SaveFilesAsync(files, FileEntityType.Weapon);
                
                    if (!saveFilesResult.Succeeded)
                    {
                        _logger.LogWarning("File upload failed during weapon creation. Error: {Error}, User: {UserId}",
                            saveFilesResult.Message, _currentUserService.UserId);
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                            $"File upload failed: {saveFilesResult.Message}");
                    }
                }

                // Map DTO to entity
                var weapon = _mapper.Map<Weapon>(inputDto);
                weapon.ItemType = ItemType.Weapon;
                weapon.CreationDate = DateTime.UtcNow;
                weapon.CreatedBy = _currentUserService.UserId;
                weapon.Nsn = string.IsNullOrWhiteSpace(inputDto.Nsn) ? null : inputDto.Nsn.Trim();

                // Add to repository first to get the ID
                var createdWeapon = await _weaponRepository.AddAsync(weapon);
                _logger.LogInformation("Weapon created successfully. WeaponId: {WeaponId}, Name: {Name}, User: {UserId}",
                                createdWeapon.Id, createdWeapon.Name, _currentUserService.UserId);

                // Upload files and link them to the created weapon
                if (files != null && files.Any())
                {
                    var uploadFilesResult = await _fileUploadService.UploadFilesForEntityAsync(
                        files, 
                        FileEntityType.Weapon, 
                        createdWeapon.Id);
                    
                    if (!uploadFilesResult.Succeeded)
                    {
                        _logger.LogWarning("File upload failed during weapon creation. Error: {Error}, User: {UserId}", 
                            uploadFilesResult.Message, _currentUserService.UserId);
                    }
                    else
                    {
                        _logger.LogInformation("Files uploaded and linked to weapon. WeaponId: {WeaponId}, FileCount: {FileCount}, User: {UserId}",
                            createdWeapon.Id, files.Count, _currentUserService.UserId);
                    }
                }

                return APIOperationResponse<long>.Success(createdWeapon.Id, "Weapon created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating weapon. Name: {Name}, User: {UserId}", 
                    inputDto?.Name, _currentUserService.UserId);
             
                var msg = ex.Message;
                if (ex.InnerException != null)
                {
                    msg += $" (Inner: {ex.InnerException.Message})";
                }
             
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {msg}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateWeaponDto inputDto)
        {
            _logger.LogInformation("Updating weapon. WeaponId: {WeaponId}, Name: {Name}, User: {UserId}", 
                id, inputDto?.Name, _currentUserService.UserId);
            
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning("Weapon validation failed. Errors: {ValidationErrors}, WeaponId: {WeaponId}, User: {UserId}", 
                        errors, id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                // Check if weapon exists
                var existingWeapon = await _weaponRepository.FindOneAsync(w => w.Id == id && !w.IsDeleted);
                if (existingWeapon == null)
                {
                    _logger.LogWarning("Weapon not found for update. WeaponId: {WeaponId}, User: {UserId}", 
                        id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Weapon not found");
                }

                if (!string.IsNullOrWhiteSpace(inputDto.Nsn))
                {
                    var duplicate = await _weaponRepository.FindOneAsync(
                        w => !w.IsDeleted && w.Id != id && w.Nsn == inputDto.Nsn.Trim());

                    if (duplicate != null)
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "NSN already exists");
                }

                // Map updates to entity
                _mapper.Map(inputDto, existingWeapon);
                existingWeapon.ModificationDate = DateTime.UtcNow;
                existingWeapon.ModifiedBy = _currentUserService.UserId;
                existingWeapon.Nsn = string.IsNullOrWhiteSpace(inputDto.Nsn) ? null : inputDto.Nsn.Trim();

                // Update in repository
                await _weaponRepository.UpdateAsync(existingWeapon);
                
                _logger.LogInformation("Weapon updated successfully. WeaponId: {WeaponId}, Name: {Name}, User: {UserId}", 
                    id, existingWeapon.Name, _currentUserService.UserId);
                
                return APIOperationResponse<bool>.Success(true, "Weapon updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating weapon. WeaponId: {WeaponId}, Name: {Name}, User: {UserId}", 
                    id, inputDto?.Name, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting weapon. WeaponId: {WeaponId}, User: {UserId}", 
                id, _currentUserService.UserId);
            
            try
            {
                var weapon = await _weaponRepository.FindOneAsync(w => w.Id == id && !w.IsDeleted);
                if (weapon == null)
                {
                    _logger.LogWarning("Weapon not found for deletion. WeaponId: {WeaponId}, User: {UserId}", 
                        id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Weapon not found");
                }

                var name = weapon.Name;
                
                await _weaponRepository.DeleteAsync(weapon);

                _logger.LogInformation("Weapon deleted successfully. WeaponId: {WeaponId}, Name: {Name}, User: {UserId}", 
                    id, name, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true, "Weapon deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting weapon. WeaponId: {WeaponId}, User: {UserId}", 
                    id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<ImportResult<CreateUpdateWeaponDto>>> ImportAsync(IFormFile file)
        {
            _logger.LogInformation("Importing weapons from file. FileName: {FileName}, User: {UserId}", 
                file?.FileName, _currentUserService.UserId);
            
            try
            {
                var mappings = GetColumnMappings();
                var importResult = await _excelImportService.ImportFromExcelAsync<CreateUpdateWeaponDto>(file, mappings);

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

                _logger.LogInformation("Weapon import completed. FileName: {FileName}, SuccessCount: {SuccessCount}, ErrorCount: {ErrorCount}, User: {UserId}", 
                    file?.FileName, importResult.SuccessCount, importResult.Errors?.Count ?? 0, _currentUserService.UserId);
                
                return APIOperationResponse<ImportResult<CreateUpdateWeaponDto>>.Success(importResult, "Import processed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing weapons from file. FileName: {FileName}, User: {UserId}", 
                    file?.FileName, _currentUserService.UserId);
                return APIOperationResponse<ImportResult<CreateUpdateWeaponDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        public async Task<APIOperationResponse<ImportResult<CreateUpdateWeaponDto>>> ImportPreviewAsync(IFormFile file)
        {
            try
            {
                var mappings = GetColumnMappings();
                var importResult = await _excelImportService.ImportFromExcelAsync<CreateUpdateWeaponDto>(file, mappings);

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
                                var existingWithSameNsn = await _weaponRepository.FindOneAsync(
                                    w => !w.IsDeleted && w.Nsn == dto.Nsn.Trim());

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

                return APIOperationResponse<ImportResult<CreateUpdateWeaponDto>>.Success(importResult, "Preview processed");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<ImportResult<CreateUpdateWeaponDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        private Dictionary<string, string> GetColumnMappings()
        {
            return new Dictionary<string, string>
            {
                { "Name", nameof(CreateUpdateWeaponDto.Name) },
                { "Item No", nameof(CreateUpdateWeaponDto.ItemNo) },
                { "Part No", nameof(CreateUpdateWeaponDto.PartNo) },
                { "Price", nameof(CreateUpdateWeaponDto.Price) },
                { "Minimum Quantity", nameof(CreateUpdateWeaponDto.MinimumQuantity) },
                { "NSN", nameof(CreateUpdateWeaponDto.Nsn) },
                { "Caliber", nameof(CreateUpdateWeaponDto.Caliber) },
                { "Year Of Manufacture", nameof(CreateUpdateWeaponDto.YearOfManufacture) },
                { "Model", nameof(CreateUpdateWeaponDto.Model) }
            };
        }
    }
}
