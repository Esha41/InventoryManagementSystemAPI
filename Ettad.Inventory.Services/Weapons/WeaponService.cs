using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Drawing;
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
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using Ettad.EntityFramework.DataBaseContext;

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
        private readonly ApplicationDbContext _context;

        public WeaponService(
            ICrossCuttingRepository<Weapon> weaponRepository,
            IMapper mapper,
            IValidator<CreateUpdateWeaponDto> validator,
            ICurrentUserService currentUserService,
            ILogger<WeaponService> logger,
            IFileUploadService fileUploadService,
            IExcelImportService excelImportService,
            ApplicationDbContext context)
        {
            _weaponRepository = weaponRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
            _logger = logger;
            _fileUploadService = fileUploadService;
            _excelImportService = excelImportService;
            _context = context;
            
            // Set EPPlus license context
            ExcelPackage.License.SetNonCommercialPersonal("Ettad");
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
                weapon.CreationDate = DateTime.Now;
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
                existingWeapon.ModificationDate = DateTime.Now;
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
                var importResult = await _excelImportService.ImportFromExcelAsync<WeaponImportDto>(file, mappings);

                // Load all lookup data for resolution
                var units = await _context.Units.Where(u => !u.IsDeleted).ToListAsync();
                var countries = await _context.Countries.Where(c => !c.IsDeleted).ToListAsync();
                var classifications = await _context.Classifications.Where(c => !c.IsDeleted).ToListAsync();
                var itemTypes = await _context.ItemTypes.Where(i => !i.IsDeleted).ToListAsync();

                var finalResult = new ImportResult<CreateUpdateWeaponDto>
                {
                    TotalProcessed = importResult.TotalProcessed
                };
                
                // Copy any initial parsing errors
                finalResult.Errors.AddRange(importResult.Errors);

                if (importResult.SuccessCount > 0)
                {
                    // Track NSNs and ItemNos seen in this import file to detect duplicates within the file
                    var seenNsns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    var seenItemNos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    
                    int rowNumber = 2; // Start from row 2 (row 1 is headers)
                    foreach (var importDto in importResult.SuccessfulRecords)
                    {
                        try
                        {
                            var dto = new CreateUpdateWeaponDto
                            {
                                // Basic fields
                                Name = importDto.Name,
                                ItemNo = importDto.ItemNo,
                                PartNo = importDto.PartNo,
                                Price = importDto.Price,
                                MinimumQuantity = importDto.MinimumQuantity,
                                Nsn = importDto.Nsn,
                                Distribution = importDto.Distribution,
                                ReferenceNo = importDto.ReferenceNo,
                                UNNumber = importDto.UNNumber,
                                Notes = importDto.Notes,
                                
                                // Weapon specific fields
                                Caliber = importDto.Caliber,
                                YearOfManufacture = importDto.YearOfManufacture,
                                Model = importDto.Model
                            };

                            // Resolve Lookups
                            
                            // Caliber Unit
                            if (!string.IsNullOrWhiteSpace(importDto.CaliberUnit))
                            {
                                var unit = units.FirstOrDefault(u => 
                                    (u.NameEn != null && u.NameEn.Equals(importDto.CaliberUnit, StringComparison.OrdinalIgnoreCase)) || 
                                    (u.NameAr != null && u.NameAr.Equals(importDto.CaliberUnit, StringComparison.OrdinalIgnoreCase)));
                                dto.CaliberUnitId = unit?.Id;
                            }

                            // Country of Manufacture
                            if (!string.IsNullOrWhiteSpace(importDto.CountryOfManufacture))
                            {
                                var country = countries.FirstOrDefault(c => 
                                    (c.NameEn != null && c.NameEn.Equals(importDto.CountryOfManufacture, StringComparison.OrdinalIgnoreCase)) || 
                                    (c.NameAr != null && c.NameAr.Equals(importDto.CountryOfManufacture, StringComparison.OrdinalIgnoreCase)));
                                dto.CountryOfManufactureId = country?.Id;
                            }

                            // Classification
                            if (!string.IsNullOrWhiteSpace(importDto.Classification))
                            {
                                var item = classifications.FirstOrDefault(c => 
                                    (c.NameEn != null && c.NameEn.Equals(importDto.Classification, StringComparison.OrdinalIgnoreCase)) || 
                                    (c.NameAr != null && c.NameAr.Equals(importDto.Classification, StringComparison.OrdinalIgnoreCase)));
                                dto.ClassificationId = item?.Id;
                            }

                            // Type
                            if (!string.IsNullOrWhiteSpace(importDto.Type))
                            {
                                var item = itemTypes.FirstOrDefault(i => 
                                    (i.NameEn != null && i.NameEn.Equals(importDto.Type, StringComparison.OrdinalIgnoreCase)) || 
                                    (i.NameAr != null && i.NameAr.Equals(importDto.Type, StringComparison.OrdinalIgnoreCase)));
                                dto.TypeId = item?.Id;
                            }

                            // Validate
                            var validationResult = await _validator.ValidateAsync(dto);
                            if (!validationResult.IsValid)
                            {
                                finalResult.Errors.Add(new ImportError 
                                { 
                                    RowNumber = rowNumber,
                                    ErrorMessage = $"Row {rowNumber}: Validation failed: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}", 
                                    ColumnName = "N/A",
                                    RowData = dto
                                });
                                rowNumber++;
                                continue;
                            }

                            // Check for duplicate ItemNo within the import file
                            if (!string.IsNullOrWhiteSpace(dto.ItemNo))
                            {
                                var itemNoKey = dto.ItemNo.Trim();
                                if (seenItemNos.Contains(itemNoKey))
                                {
                                    finalResult.Errors.Add(new ImportError 
                                    { 
                                        RowNumber = rowNumber,
                                        ErrorMessage = $"Row {rowNumber}: Item No '{dto.ItemNo}' appears multiple times in the import file", 
                                        ColumnName = "Item No",
                                        RowData = dto
                                    });
                                    rowNumber++;
                                    continue;
                                }
                                seenItemNos.Add(itemNoKey);
                            }
                            
                            // Check for duplicate NSN within the import file
                            if (!string.IsNullOrWhiteSpace(dto.Nsn))
                            {
                                var nsnKey = dto.Nsn.Trim();
                                if (seenNsns.Contains(nsnKey))
                                {
                                    finalResult.Errors.Add(new ImportError 
                                    { 
                                        RowNumber = rowNumber,
                                        ErrorMessage = $"Row {rowNumber}: NSN '{dto.Nsn}' appears multiple times in the import file", 
                                        ColumnName = "NSN",
                                        RowData = dto
                                    });
                                    rowNumber++;
                                    continue;
                                }
                                seenNsns.Add(nsnKey);
                            }

                            // Create using existing CreateAsync (which also checks database duplicates)
                            var createResult = await CreateAsync(dto);
                            if (!createResult.Succeeded)
                            {
                                // Extract inner exception details for better error messages
                                var errorMessage = createResult.Message;
                                _logger.LogError("Row {RowNumber} creation failed: {ErrorMessage}", rowNumber, errorMessage);
                                
                                finalResult.Errors.Add(new ImportError 
                                { 
                                    RowNumber = rowNumber,
                                    ErrorMessage = $"Row {rowNumber}: {errorMessage}", 
                                    ColumnName = "N/A",
                                    RowData = dto
                                });
                            }
                            else
                            {
                                finalResult.SuccessfulRecords.Add(dto);
                            }
                            
                            rowNumber++;
                        }
                        catch (Exception ex)
                        {
                            var errorMsg = ex.Message;
                            if (ex.InnerException != null)
                            {
                                errorMsg += $" (Inner: {ex.InnerException.Message})";
                            }
                            
                            _logger.LogError(ex, "Row {RowNumber} processing error: {ErrorMessage}", rowNumber, errorMsg);
                            
                            finalResult.Errors.Add(new ImportError
                            {
                                RowNumber = rowNumber,
                                ErrorMessage = $"Row {rowNumber}: {errorMsg}",
                                ColumnName = "N/A"
                            });
                            rowNumber++;
                        }
                    }
                }

                _logger.LogInformation("Weapon import completed. FileName: {FileName}, SuccessCount: {SuccessCount}, ErrorCount: {ErrorCount}, User: {UserId}", 
                    file?.FileName, finalResult.SuccessCount, finalResult.Errors.Count, _currentUserService.UserId);
                
                return APIOperationResponse<ImportResult<CreateUpdateWeaponDto>>.Success(finalResult, "Import processed");
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
                var importResult = await _excelImportService.ImportFromExcelAsync<WeaponImportDto>(file, mappings);

                // Load lookup data for resolution
                var units = await _context.Units.Where(u => !u.IsDeleted).ToListAsync();
                var countries = await _context.Countries.Where(c => !c.IsDeleted).ToListAsync();
                var classifications = await _context.Classifications.Where(c => !c.IsDeleted).ToListAsync();
                var itemTypes = await _context.ItemTypes.Where(i => !i.IsDeleted).ToListAsync();

                var finalResult = new ImportResult<CreateUpdateWeaponDto>
                {
                    TotalProcessed = importResult.TotalProcessed
                };
                
                finalResult.Errors.AddRange(importResult.Errors);

                if (importResult.SuccessCount > 0)
                {
                    // Track NSNs and ItemNos seen in this import file to detect duplicates within the file
                    var seenNsns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    var seenItemNos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    
                    int rowNumber = 2; // Start from row 2 (row 1 is headers)
                    foreach (var importDto in importResult.SuccessfulRecords)
                    {
                        var dto = new CreateUpdateWeaponDto
                        {
                            // Basic fields
                            Name = importDto.Name,
                            ItemNo = importDto.ItemNo,
                            PartNo = importDto.PartNo,
                            Price = importDto.Price,
                            MinimumQuantity = importDto.MinimumQuantity,
                            Nsn = importDto.Nsn,
                            Distribution = importDto.Distribution,
                            ReferenceNo = importDto.ReferenceNo,
                            UNNumber = importDto.UNNumber,
                            Notes = importDto.Notes,
                            
                            // Weapon specific fields
                            Caliber = importDto.Caliber,
                            YearOfManufacture = importDto.YearOfManufacture,
                            Model = importDto.Model
                        };

                        // Resolve Lookups
                        if (!string.IsNullOrWhiteSpace(importDto.CaliberUnit))
                        {
                            var unit = units.FirstOrDefault(u => 
                                (u.NameEn != null && u.NameEn.Equals(importDto.CaliberUnit, StringComparison.OrdinalIgnoreCase)) || 
                                (u.NameAr != null && u.NameAr.Equals(importDto.CaliberUnit, StringComparison.OrdinalIgnoreCase)));
                            dto.CaliberUnitId = unit?.Id;
                        }
                        if (!string.IsNullOrWhiteSpace(importDto.CountryOfManufacture))
                        {
                            var country = countries.FirstOrDefault(c => 
                                (c.NameEn != null && c.NameEn.Equals(importDto.CountryOfManufacture, StringComparison.OrdinalIgnoreCase)) || 
                                (c.NameAr != null && c.NameAr.Equals(importDto.CountryOfManufacture, StringComparison.OrdinalIgnoreCase)));
                            dto.CountryOfManufactureId = country?.Id;
                        }
                        if (!string.IsNullOrWhiteSpace(importDto.Classification))
                        {
                            var item = classifications.FirstOrDefault(c => 
                                (c.NameEn != null && c.NameEn.Equals(importDto.Classification, StringComparison.OrdinalIgnoreCase)) || 
                                (c.NameAr != null && c.NameAr.Equals(importDto.Classification, StringComparison.OrdinalIgnoreCase)));
                            dto.ClassificationId = item?.Id;
                        }
                        if (!string.IsNullOrWhiteSpace(importDto.Type))
                        {
                            var item = itemTypes.FirstOrDefault(i => 
                                (i.NameEn != null && i.NameEn.Equals(importDto.Type, StringComparison.OrdinalIgnoreCase)) || 
                                (i.NameAr != null && i.NameAr.Equals(importDto.Type, StringComparison.OrdinalIgnoreCase)));
                            dto.TypeId = item?.Id;
                        }

                        // Validate
                        var validationResult = await _validator.ValidateAsync(dto);
                        if (!validationResult.IsValid)
                        {
                            finalResult.Errors.Add(new ImportError 
                            { 
                                RowNumber = rowNumber,
                                ErrorMessage = $"Row {rowNumber}: Validation failed: {string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))}", 
                                ColumnName = "N/A",
                                RowData = dto // Include the row data for preview
                            });
                            rowNumber++;
                            continue;
                        }
                        
                        // Check for duplicate ItemNo within the import file
                        if (!string.IsNullOrWhiteSpace(dto.ItemNo))
                        {
                            var itemNoKey = dto.ItemNo.Trim();
                            if (seenItemNos.Contains(itemNoKey))
                            {
                                finalResult.Errors.Add(new ImportError 
                                { 
                                    RowNumber = rowNumber,
                                    ErrorMessage = $"Row {rowNumber}: Item No '{dto.ItemNo}' appears multiple times in the import file", 
                                    ColumnName = "Item No",
                                    RowData = dto
                                });
                                rowNumber++;
                                continue;
                            }
                            seenItemNos.Add(itemNoKey);
                        }
                        
                        // Check for duplicate NSN within the import file
                        if (!string.IsNullOrWhiteSpace(dto.Nsn))
                        {
                            var nsnKey = dto.Nsn.Trim();
                            if (seenNsns.Contains(nsnKey))
                            {
                                finalResult.Errors.Add(new ImportError 
                                { 
                                    RowNumber = rowNumber,
                                    ErrorMessage = $"Row {rowNumber}: NSN '{dto.Nsn}' appears multiple times in the import file", 
                                    ColumnName = "NSN",
                                    RowData = dto // Include the row data for preview
                                });
                                rowNumber++;
                                continue;
                            }
                            seenNsns.Add(nsnKey);
                            
                            // Also check if NSN exists in database
                            var existing = await _weaponRepository.FindOneAsync(w => !w.IsDeleted && w.Nsn == nsnKey);
                            if (existing != null)
                            {
                                finalResult.Errors.Add(new ImportError 
                                { 
                                    RowNumber = rowNumber,
                                    ErrorMessage = $"Row {rowNumber}: NSN '{dto.Nsn}' already exists in the database", 
                                    ColumnName = "NSN",
                                    RowData = dto // Include the row data for preview
                                });
                                rowNumber++;
                                continue;
                            }
                        }

                        finalResult.SuccessfulRecords.Add(dto);
                        rowNumber++;
                    }
                }

                return APIOperationResponse<ImportResult<CreateUpdateWeaponDto>>.Success(finalResult, "Preview processed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in weapon import preview");
                return APIOperationResponse<ImportResult<CreateUpdateWeaponDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        private Dictionary<string, string> GetColumnMappings()
        {
            return new Dictionary<string, string>
            {
                // English headers
                { "Name*", nameof(WeaponImportDto.Name) },
                { "Item No*", nameof(WeaponImportDto.ItemNo) },
                { "Part No", nameof(WeaponImportDto.PartNo) },
                { "NSN", nameof(WeaponImportDto.Nsn) },
                { "Price", nameof(WeaponImportDto.Price) },
                { "Minimum Quantity", nameof(WeaponImportDto.MinimumQuantity) },
                { "Caliber", nameof(WeaponImportDto.Caliber) },
                { "Caliber Unit", nameof(WeaponImportDto.CaliberUnit) },
                { "Year Of Manufacture", nameof(WeaponImportDto.YearOfManufacture) },
                { "Country Of Manufacture", nameof(WeaponImportDto.CountryOfManufacture) },
                { "Model", nameof(WeaponImportDto.Model) },
                { "UN Number", nameof(WeaponImportDto.UNNumber) },
                { "Distribution", nameof(WeaponImportDto.Distribution) },
                { "Reference No", nameof(WeaponImportDto.ReferenceNo) },
                { "Classification", nameof(WeaponImportDto.Classification) },
                { "Type", nameof(WeaponImportDto.Type) },
                { "Notes", nameof(WeaponImportDto.Notes) },
                
                // Arabic headers (same mappings)
                { "الاسم*", nameof(WeaponImportDto.Name) },
                { "رقم الصنف*", nameof(WeaponImportDto.ItemNo) },
                { "رقم الجزء", nameof(WeaponImportDto.PartNo) },
                // Note: "NSN" is the same in both languages, so it's already mapped above
                { "السعر", nameof(WeaponImportDto.Price) },
                { "الكمية الدنيا", nameof(WeaponImportDto.MinimumQuantity) },
                { "العيار", nameof(WeaponImportDto.Caliber) },
                { "وحدة العيار", nameof(WeaponImportDto.CaliberUnit) },
                { "سنة التصنيع", nameof(WeaponImportDto.YearOfManufacture) },
                { "بلد التصنيع", nameof(WeaponImportDto.CountryOfManufacture) },
                { "النموذج", nameof(WeaponImportDto.Model) },
                { "رقم الأمم المتحدة", nameof(WeaponImportDto.UNNumber) },
                { "التوزيع", nameof(WeaponImportDto.Distribution) },
                { "الرقم المرجعي", nameof(WeaponImportDto.ReferenceNo) },
                { "التصنيف", nameof(WeaponImportDto.Classification) },
                { "النوع", nameof(WeaponImportDto.Type) },
                { "ملاحظات", nameof(WeaponImportDto.Notes) }
            };
        }

        public async Task<APIOperationResponse<byte[]>> GenerateImportTemplateAsync(string language = "en")
        {
            try
            {
                _logger.LogInformation("Generating weapon import template with all fields and lookup data. Language: {Language}", language);

                // Load lookup data from database
                var units = await _context.Units
                    .Where(u => !u.IsDeleted)
                    .OrderBy(u => u.NameEn ?? u.NameAr)
                    .ToListAsync();

                var countries = await _context.Countries
                    .Where(c => !c.IsDeleted)
                    .OrderBy(c => c.NameEn ?? c.NameAr)
                    .ToListAsync();

                var classifications = await _context.Classifications
                    .Where(c => !c.IsDeleted)
                    .OrderBy(c => c.NameEn ?? c.NameAr)
                    .ToListAsync();

                var itemTypes = await _context.ItemTypes
                    .Where(i => !i.IsDeleted)
                    .OrderBy(i => i.NameEn ?? i.NameAr)
                    .ToListAsync();

                // Generate Excel with EPPlus
                using var package = new ExcelPackage();

                // Main template sheet
                var templateSheet = package.Workbook.Worksheets.Add("Weapon Import");

                // Headers - Bilingual support (English / Arabic)
                var headers = language == "ar"
                    ? new[]
                    {
                        "الاسم*", "رقم الصنف*", "رقم الجزء", "NSN", "السعر", "الكمية الدنيا",
                        "العيار", "وحدة العيار", "سنة التصنيع", "بلد التصنيع", "النموذج",
                        "رقم الأمم المتحدة", "التوزيع", "الرقم المرجعي", "التصنيف", "النوع", "ملاحظات"
                    }
                    : new[]
                    {
                        "Name*", "Item No*", "Part No", "NSN", "Price", "Minimum Quantity",
                        "Caliber", "Caliber Unit", "Year Of Manufacture", "Country Of Manufacture", "Model",
                        "UN Number", "Distribution", "Reference No", "Classification", "Type", "Notes"
                    };

                // Add headers with formatting
                for (int col = 1; col <= headers.Length; col++)
                {
                    templateSheet.Cells[1, col].Value = headers[col - 1];
                    templateSheet.Cells[1, col].Style.Font.Bold = true;
                    templateSheet.Cells[1, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    templateSheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                    templateSheet.Cells[1, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                // Sample data row
                templateSheet.Cells[2, 1].Value = "AK-47";
                templateSheet.Cells[2, 2].Value = "WPN-001";
                templateSheet.Cells[2, 3].Value = "P-AK47";
                templateSheet.Cells[2, 4].Value = "1005-12-345-6789";
                templateSheet.Cells[2, 5].Value = 500;
                templateSheet.Cells[2, 6].Value = 10;
                templateSheet.Cells[2, 7].Value = "7.62";
                templateSheet.Cells[2, 8].Value = units.FirstOrDefault()?.NameEn ?? "";
                templateSheet.Cells[2, 9].Value = 1950;
                templateSheet.Cells[2, 10].Value = countries.FirstOrDefault()?.NameEn ?? "";

                // Create hidden lookup sheets
                CreateLookupSheet(package, "Units", units.Select(u => u.NameEn ?? u.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                CreateLookupSheet(package, "Countries", countries.Select(c => c.NameEn ?? c.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                CreateLookupSheet(package, "Classifications", classifications.Select(c => c.NameEn ?? c.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                CreateLookupSheet(package, "ItemTypes", itemTypes.Select(i => i.NameEn ?? i.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());

                // Add data validation dropdowns
                AddDataValidation(templateSheet, 8, "Units"); // Caliber Unit (column 8)
                AddDataValidation(templateSheet, 10, "Countries"); // Country Of Manufacture
                AddDataValidation(templateSheet, 15, "Classifications"); // Classification
                AddDataValidation(templateSheet, 16, "ItemTypes"); // Type

                // Set column widths
                for (int col = 1; col <= headers.Length; col++)
                {
                    templateSheet.Column(col).Width = col == 17 ? 30 : 20; // Notes column wider
                }

                // Freeze header row
                templateSheet.View.FreezePanes(2, 1);

                var excelData = package.GetAsByteArray();

                _logger.LogInformation("Weapon import template generated successfully. FileSize: {FileSize} bytes, Lookup sheets: 4", 
                    excelData.Length);

                return APIOperationResponse<byte[]>.Success(excelData, "Template generated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating weapon import template");
                return APIOperationResponse<byte[]>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Create a hidden sheet with lookup values
        /// </summary>
        private ExcelWorksheet CreateLookupSheet(ExcelPackage package, string sheetName, List<string> values)
        {
            var lookupSheet = package.Workbook.Worksheets.Add(sheetName);
            lookupSheet.Hidden = eWorkSheetHidden.Hidden;

            for (int i = 0; i < values.Count; i++)
            {
                lookupSheet.Cells[i + 1, 1].Value = values[i];
            }

            return lookupSheet;
        }

        /// <summary>
        /// Add data validation dropdown to a column using worksheet reference
        /// </summary>
        private void AddDataValidation(ExcelWorksheet worksheet, int column, string lookupSheetName)
        {
            var columnLetter = GetColumnLetter(column);
            var validationRange = $"{columnLetter}2:{columnLetter}10000";

            var validation = worksheet.DataValidations.AddListValidation(validationRange);

            var lookupSheet = worksheet.Workbook.Worksheets[lookupSheetName];
            var lastRow = lookupSheet.Dimension?.End.Row ?? 1;
            validation.Formula.ExcelFormula = $"'{lookupSheetName}'!$A$1:$A${lastRow}";

            validation.ShowErrorMessage = true;
            validation.ErrorTitle = "Invalid Value";
            validation.Error = $"Please select a value from the {lookupSheetName} list";
            validation.ShowInputMessage = true;
            validation.PromptTitle = "Select Value";
            validation.Prompt = $"Select a value from the dropdown list";
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
