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
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using Ettad.EntityFramework.DataBaseContext;

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
        private readonly ApplicationDbContext _context;

        public AmmunitionService(
            ICrossCuttingRepository<Ammunition> ammunitionRepository,
            ICrossCuttingRepository<FileUplodDetails> fileDetailsRepository,
            IMapper mapper,
            IValidator<CreateUpdateAmmunitionDto> validator,
            ICurrentUserService currentUserService,
            ILogger<AmmunitionService> logger,
            IFileUploadService fileUploadService,
            IExcelImportService excelImportService,
            ApplicationDbContext context)
        {
            _ammunitionRepository = ammunitionRepository;
            _fileDetailsRepository = fileDetailsRepository;
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
                ammunition.CreationDate = DateTime.Now;
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
             
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += $" (Inner: {ex.InnerException.Message})";
                }
             
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {errorMessage}");
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
                existingAmmunition.ModificationDate = DateTime.Now;
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
                var importResult = await _excelImportService.ImportFromExcelAsync<AmmunitionImportDto>(file, mappings);

                // Load all lookup data for resolution
                var units = await _context.Units.Where(u => !u.IsDeleted).ToListAsync();
                var caseTypes = await _context.CaseTypes.Where(c => !c.IsDeleted).ToListAsync();
                var propellants = await _context.Propellants.Where(p => !p.IsDeleted).ToListAsync();
                var compatibilities = await _context.Compatibilities.Where(c => !c.IsDeleted).ToListAsync();
                var hazardDivisions = await _context.HazardDivisions.Where(h => !h.IsDeleted).ToListAsync();
                var natureOptions = await _context.NatureOptions.Where(n => !n.IsDeleted).ToListAsync();
                var primaryPurposes = await _context.PrimaryPurposes.Where(p => !p.IsDeleted).ToListAsync();
                var projectileColors = await _context.Colors.Where(p => !p.IsDeleted).ToListAsync();
                var projectileMaterials = await _context.ProjectailMaterials.Where(p => !p.IsDeleted).ToListAsync();
                var classifications = await _context.Classifications.Where(c => !c.IsDeleted).ToListAsync();
                var itemTypes = await _context.ItemTypes.Where(i => !i.IsDeleted).ToListAsync();

                var finalResult = new ImportResult<CreateUpdateAmmunitionDto>
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
                            var dto = new CreateUpdateAmmunitionDto
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
                                
                                // Ammunition specific fields
                                ArmNumber = importDto.ArmNumber,
                                BulletDiameter = importDto.BulletDiameter,
                                IsLinked = importDto.IsLinked,
                                Primer = importDto.Primer,
                                TotalWeight = importDto.TotalWeight
                            };

                            // Resolve Lookups
                            
                            // Bullet Diameter Unit
                            if (!string.IsNullOrWhiteSpace(importDto.BulletDiameterUnit))
                            {
                                var unit = units.FirstOrDefault(u => 
                                    (u.NameEn != null && u.NameEn.Equals(importDto.BulletDiameterUnit, StringComparison.OrdinalIgnoreCase)) || 
                                    (u.NameAr != null && u.NameAr.Equals(importDto.BulletDiameterUnit, StringComparison.OrdinalIgnoreCase)));
                                dto.BulletDiameterUnitId = unit?.Id;
                            }

                            // Case Type
                            if (!string.IsNullOrWhiteSpace(importDto.CaseType))
                            {
                                var item = caseTypes.FirstOrDefault(u => 
                                    (u.NameEn != null && u.NameEn.Equals(importDto.CaseType, StringComparison.OrdinalIgnoreCase)) || 
                                    (u.NameAr != null && u.NameAr.Equals(importDto.CaseType, StringComparison.OrdinalIgnoreCase)));
                                dto.CaseTypeId = item?.Id;
                            }

                            // Propellant
                            if (!string.IsNullOrWhiteSpace(importDto.Propellant))
                            {
                                var item = propellants.FirstOrDefault(u => 
                                    (u.NameEn != null && u.NameEn.Equals(importDto.Propellant, StringComparison.OrdinalIgnoreCase)) || 
                                    (u.NameAr != null && u.NameAr.Equals(importDto.Propellant, StringComparison.OrdinalIgnoreCase)));
                                dto.PropellantId = item?.Id;
                            }

                            // Compatibility
                            if (!string.IsNullOrWhiteSpace(importDto.Compatibility))
                            {
                                var item = compatibilities.FirstOrDefault(u => 
                                    (u.NameEn != null && u.NameEn.Equals(importDto.Compatibility, StringComparison.OrdinalIgnoreCase)) || 
                                    (u.NameAr != null && u.NameAr.Equals(importDto.Compatibility, StringComparison.OrdinalIgnoreCase)));
                                dto.CompatibilityId = item?.Id;
                            }

                            // Hazard Division
                            if (!string.IsNullOrWhiteSpace(importDto.HazardDivision))
                            {
                                var item = hazardDivisions.FirstOrDefault(u => 
                                    (u.NameEn != null && u.NameEn.Equals(importDto.HazardDivision, StringComparison.OrdinalIgnoreCase)) || 
                                    (u.NameAr != null && u.NameAr.Equals(importDto.HazardDivision, StringComparison.OrdinalIgnoreCase)));
                                dto.HazardDivisionId = item?.Id;
                            }

                            // Nature Option
                            if (!string.IsNullOrWhiteSpace(importDto.NatureOption))
                            {
                                var item = natureOptions.FirstOrDefault(u => 
                                    (u.NameEn != null && u.NameEn.Equals(importDto.NatureOption, StringComparison.OrdinalIgnoreCase)) || 
                                    (u.NameAr != null && u.NameAr.Equals(importDto.NatureOption, StringComparison.OrdinalIgnoreCase)));
                                dto.NatureOptionId = item?.Id;
                            }

                            // Primary Purpose
                            if (!string.IsNullOrWhiteSpace(importDto.PrimaryPurpose))
                            {
                                var item = primaryPurposes.FirstOrDefault(u => 
                                    (u.NameEn != null && u.NameEn.Equals(importDto.PrimaryPurpose, StringComparison.OrdinalIgnoreCase)) || 
                                    (u.NameAr != null && u.NameAr.Equals(importDto.PrimaryPurpose, StringComparison.OrdinalIgnoreCase)));
                                dto.PrimaryPurposId = item?.Id;
                            }

                            // Projectile Color
                            if (!string.IsNullOrWhiteSpace(importDto.ProjectileColor))
                            {
                                var item = projectileColors.FirstOrDefault(u => 
                                    (u.NameEn != null && u.NameEn.Equals(importDto.ProjectileColor, StringComparison.OrdinalIgnoreCase)) || 
                                    (u.NameAr != null && u.NameAr.Equals(importDto.ProjectileColor, StringComparison.OrdinalIgnoreCase)));
                                dto.ProjectileColorId = item?.Id;
                            }

                            // Projectile Material
                            if (!string.IsNullOrWhiteSpace(importDto.ProjectileMaterial))
                            {
                                var item = projectileMaterials.FirstOrDefault(u => 
                                    (u.NameEn != null && u.NameEn.Equals(importDto.ProjectileMaterial, StringComparison.OrdinalIgnoreCase)) || 
                                    (u.NameAr != null && u.NameAr.Equals(importDto.ProjectileMaterial, StringComparison.OrdinalIgnoreCase)));
                                dto.ProjectailMaterialId = item?.Id;
                            }

                            // Classification
                            if (!string.IsNullOrWhiteSpace(importDto.Classification))
                            {
                                var item = classifications.FirstOrDefault(u => 
                                    (u.NameEn != null && u.NameEn.Equals(importDto.Classification, StringComparison.OrdinalIgnoreCase)) || 
                                    (u.NameAr != null && u.NameAr.Equals(importDto.Classification, StringComparison.OrdinalIgnoreCase)));
                                dto.ClassificationId = item?.Id;
                            }

                            // Type
                            if (!string.IsNullOrWhiteSpace(importDto.Type))
                            {
                                var item = itemTypes.FirstOrDefault(u => 
                                    (u.NameEn != null && u.NameEn.Equals(importDto.Type, StringComparison.OrdinalIgnoreCase)) || 
                                    (u.NameAr != null && u.NameAr.Equals(importDto.Type, StringComparison.OrdinalIgnoreCase)));
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

                _logger.LogInformation("Ammunition import completed. FileName: {FileName}, SuccessCount: {SuccessCount}, ErrorCount: {ErrorCount}, User: {UserId}", 
                    file?.FileName, finalResult.SuccessCount, finalResult.Errors.Count, _currentUserService.UserId);
                
                return APIOperationResponse<ImportResult<CreateUpdateAmmunitionDto>>.Success(finalResult, "Import processed");
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
                var importResult = await _excelImportService.ImportFromExcelAsync<AmmunitionImportDto>(file, mappings);

                // Load all lookup data for resolution
                var units = await _context.Units.Where(u => !u.IsDeleted).ToListAsync();
                var caseTypes = await _context.CaseTypes.Where(c => !c.IsDeleted).ToListAsync();
                var propellants = await _context.Propellants.Where(p => !p.IsDeleted).ToListAsync();
                var compatibilities = await _context.Compatibilities.Where(c => !c.IsDeleted).ToListAsync();
                var hazardDivisions = await _context.HazardDivisions.Where(h => !h.IsDeleted).ToListAsync();
                var natureOptions = await _context.NatureOptions.Where(n => !n.IsDeleted).ToListAsync();
                var primaryPurposes = await _context.PrimaryPurposes.Where(p => !p.IsDeleted).ToListAsync();
                var projectileColors = await _context.Colors.Where(p => !p.IsDeleted).ToListAsync();
                var projectileMaterials = await _context.ProjectailMaterials.Where(p => !p.IsDeleted).ToListAsync();
                var classifications = await _context.Classifications.Where(c => !c.IsDeleted).ToListAsync();
                var itemTypes = await _context.ItemTypes.Where(i => !i.IsDeleted).ToListAsync();

                var finalResult = new ImportResult<CreateUpdateAmmunitionDto>
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
                        var dto = new CreateUpdateAmmunitionDto
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
                            
                            // Ammunition specific fields
                            ArmNumber = importDto.ArmNumber,
                            BulletDiameter = importDto.BulletDiameter,
                            IsLinked = importDto.IsLinked,
                            Primer = importDto.Primer,
                            TotalWeight = importDto.TotalWeight
                        };

                        // Resolve Lookups
                        // Bullet Diameter Unit
                        if (!string.IsNullOrWhiteSpace(importDto.BulletDiameterUnit))
                        {
                            var unit = units.FirstOrDefault(u => 
                                (u.NameEn != null && u.NameEn.Equals(importDto.BulletDiameterUnit, StringComparison.OrdinalIgnoreCase)) || 
                                (u.NameAr != null && u.NameAr.Equals(importDto.BulletDiameterUnit, StringComparison.OrdinalIgnoreCase)));
                            dto.BulletDiameterUnitId = unit?.Id;
                        }

                        // Case Type
                        if (!string.IsNullOrWhiteSpace(importDto.CaseType))
                        {
                            var item = caseTypes.FirstOrDefault(u => 
                                (u.NameEn != null && u.NameEn.Equals(importDto.CaseType, StringComparison.OrdinalIgnoreCase)) || 
                                (u.NameAr != null && u.NameAr.Equals(importDto.CaseType, StringComparison.OrdinalIgnoreCase)));
                            dto.CaseTypeId = item?.Id;
                        }

                        // Propellant
                        if (!string.IsNullOrWhiteSpace(importDto.Propellant))
                        {
                            var item = propellants.FirstOrDefault(u => 
                                (u.NameEn != null && u.NameEn.Equals(importDto.Propellant, StringComparison.OrdinalIgnoreCase)) || 
                                (u.NameAr != null && u.NameAr.Equals(importDto.Propellant, StringComparison.OrdinalIgnoreCase)));
                            dto.PropellantId = item?.Id;
                        }

                        // Compatibility
                        if (!string.IsNullOrWhiteSpace(importDto.Compatibility))
                        {
                            var item = compatibilities.FirstOrDefault(u => 
                                (u.NameEn != null && u.NameEn.Equals(importDto.Compatibility, StringComparison.OrdinalIgnoreCase)) || 
                                (u.NameAr != null && u.NameAr.Equals(importDto.Compatibility, StringComparison.OrdinalIgnoreCase)));
                            dto.CompatibilityId = item?.Id;
                        }

                        // Hazard Division
                        if (!string.IsNullOrWhiteSpace(importDto.HazardDivision))
                        {
                            var item = hazardDivisions.FirstOrDefault(u => 
                                (u.NameEn != null && u.NameEn.Equals(importDto.HazardDivision, StringComparison.OrdinalIgnoreCase)) || 
                                (u.NameAr != null && u.NameAr.Equals(importDto.HazardDivision, StringComparison.OrdinalIgnoreCase)));
                            dto.HazardDivisionId = item?.Id;
                        }

                        // Nature Option
                        if (!string.IsNullOrWhiteSpace(importDto.NatureOption))
                        {
                            var item = natureOptions.FirstOrDefault(u => 
                                (u.NameEn != null && u.NameEn.Equals(importDto.NatureOption, StringComparison.OrdinalIgnoreCase)) || 
                                (u.NameAr != null && u.NameAr.Equals(importDto.NatureOption, StringComparison.OrdinalIgnoreCase)));
                            dto.NatureOptionId = item?.Id;
                        }

                        // Primary Purpose
                        if (!string.IsNullOrWhiteSpace(importDto.PrimaryPurpose))
                        {
                            var item = primaryPurposes.FirstOrDefault(u => 
                                (u.NameEn != null && u.NameEn.Equals(importDto.PrimaryPurpose, StringComparison.OrdinalIgnoreCase)) || 
                                (u.NameAr != null && u.NameAr.Equals(importDto.PrimaryPurpose, StringComparison.OrdinalIgnoreCase)));
                            dto.PrimaryPurposId = item?.Id;
                        }

                        // Projectile Color
                        if (!string.IsNullOrWhiteSpace(importDto.ProjectileColor))
                        {
                            var item = projectileColors.FirstOrDefault(u => 
                                (u.NameEn != null && u.NameEn.Equals(importDto.ProjectileColor, StringComparison.OrdinalIgnoreCase)) || 
                                (u.NameAr != null && u.NameAr.Equals(importDto.ProjectileColor, StringComparison.OrdinalIgnoreCase)));
                            dto.ProjectileColorId = item?.Id;
                        }

                        // Projectile Material
                        if (!string.IsNullOrWhiteSpace(importDto.ProjectileMaterial))
                        {
                            var item = projectileMaterials.FirstOrDefault(u => 
                                (u.NameEn != null && u.NameEn.Equals(importDto.ProjectileMaterial, StringComparison.OrdinalIgnoreCase)) || 
                                (u.NameAr != null && u.NameAr.Equals(importDto.ProjectileMaterial, StringComparison.OrdinalIgnoreCase)));
                            dto.ProjectailMaterialId = item?.Id;
                        }

                        // Classification
                        if (!string.IsNullOrWhiteSpace(importDto.Classification))
                        {
                            var item = classifications.FirstOrDefault(u => 
                                (u.NameEn != null && u.NameEn.Equals(importDto.Classification, StringComparison.OrdinalIgnoreCase)) || 
                                (u.NameAr != null && u.NameAr.Equals(importDto.Classification, StringComparison.OrdinalIgnoreCase)));
                            dto.ClassificationId = item?.Id;
                        }

                        // Type
                        if (!string.IsNullOrWhiteSpace(importDto.Type))
                        {
                            var item = itemTypes.FirstOrDefault(u => 
                                (u.NameEn != null && u.NameEn.Equals(importDto.Type, StringComparison.OrdinalIgnoreCase)) || 
                                (u.NameAr != null && u.NameAr.Equals(importDto.Type, StringComparison.OrdinalIgnoreCase)));
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
                            var existing = await _ammunitionRepository.FindOneAsync(e => !e.IsDeleted && e.Nsn == nsnKey);
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

                return APIOperationResponse<ImportResult<CreateUpdateAmmunitionDto>>.Success(finalResult, "Preview processed");
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
                // Basic fields (Matching headers with asterisks from template)
                { "Name*", nameof(AmmunitionImportDto.Name) },
                { "Item No*", nameof(AmmunitionImportDto.ItemNo) },
                { "Part No", nameof(AmmunitionImportDto.PartNo) },
                { "Arm Number", nameof(AmmunitionImportDto.ArmNumber) },
                { "NSN", nameof(AmmunitionImportDto.Nsn) },
                { "Price", nameof(AmmunitionImportDto.Price) },
                { "Minimum Quantity", nameof(AmmunitionImportDto.MinimumQuantity) },
                
                // Physical properties
                { "Bullet Diameter", nameof(AmmunitionImportDto.BulletDiameter) },
                { "Bullet Diameter Unit", nameof(AmmunitionImportDto.BulletDiameterUnit) },
                { "Total Weight", nameof(AmmunitionImportDto.TotalWeight) },
                { "Is Linked", nameof(AmmunitionImportDto.IsLinked) },
                { "Primer", nameof(AmmunitionImportDto.Primer) },
                
                // Lookup fields
                { "Case Type", nameof(AmmunitionImportDto.CaseType) },
                { "Propellant", nameof(AmmunitionImportDto.Propellant) },
                { "Compatibility", nameof(AmmunitionImportDto.Compatibility) },
                { "Hazard Division", nameof(AmmunitionImportDto.HazardDivision) },
                { "Nature Option", nameof(AmmunitionImportDto.NatureOption) },
                { "Primary Purpose", nameof(AmmunitionImportDto.PrimaryPurpose) },
                { "Projectile Color", nameof(AmmunitionImportDto.ProjectileColor) },
                { "Projectile Material", nameof(AmmunitionImportDto.ProjectileMaterial) },
                
                // Additional fields
                { "UN Number", nameof(AmmunitionImportDto.UNNumber) },
                { "Distribution", nameof(AmmunitionImportDto.Distribution) },
                { "Reference No", nameof(AmmunitionImportDto.ReferenceNo) },
                { "Classification", nameof(AmmunitionImportDto.Classification) },
                { "Type", nameof(AmmunitionImportDto.Type) },
                { "Notes", nameof(AmmunitionImportDto.Notes) }
            };
        }

        public async Task<APIOperationResponse<byte[]>> GenerateImportTemplateAsync(string language = "en")
        {
            try
            {
                _logger.LogInformation("Generating ammunition import template with all fields and lookup data. Language: {Language}", language);

                // Load all lookup data from database
                var units = await _context.Units
                    .Where(u => !u.IsDeleted)
                    .OrderBy(u => u.NameEn ?? u.NameAr)
                    .ToListAsync();

                var caseTypes = await _context.CaseTypes
                    .Where(c => !c.IsDeleted)
                    .OrderBy(c => c.NameEn ?? c.NameAr)
                    .ToListAsync();

                var propellants = await _context.Propellants
                    .Where(p => !p.IsDeleted)
                    .OrderBy(p => p.NameEn ?? p.NameAr)
                    .ToListAsync();

                var compatibilities = await _context.Compatibilities
                    .Where(c => !c.IsDeleted)
                    .OrderBy(c => c.NameEn ?? c.NameAr)
                    .ToListAsync();

                var hazardDivisions = await _context.HazardDivisions
                    .Where(h => !h.IsDeleted)
                    .OrderBy(h => h.NameEn ?? h.NameAr)
                    .ToListAsync();

                var natureOptions = await _context.NatureOptions
                    .Where(n => !n.IsDeleted)
                    .OrderBy(n => n.NameEn ?? n.NameAr)
                    .ToListAsync();

                var primaryPurposes = await _context.PrimaryPurposes
                    .Where(p => !p.IsDeleted)
                    .OrderBy(p => p.NameEn ?? p.NameAr)
                    .ToListAsync();

                var projectileColors = await _context.Colors
                    .Where(p => !p.IsDeleted)
                    .OrderBy(p => p.NameEn ?? p.NameAr)
                    .ToListAsync();

                var projectileMaterials = await _context.ProjectailMaterials
                    .Where(p => !p.IsDeleted)
                    .OrderBy(p => p.NameEn ?? p.NameAr)
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
                var templateSheet = package.Workbook.Worksheets.Add("Ammunition Import");

                // Headers - Bilingual support (English / Arabic)
                var headers = language == "ar" 
                    ? new[]
                    {
                        "الاسم*", "رقم الصنف*", "رقم الجزء", "رقم ARM", "NSN", "السعر", "الكمية الدنيا",
                        "قطر الرصاصة", "وحدة قطر الرصاصة", "الوزن الكلي", "مرتبط", "الكبسولة",
                        "نوع الغلاف", "المادة الدافعة", "التوافق", "قسم الخطر", "خيار الطبيعة",
                        "الغرض الأساسي", "لون المقذوف", "مادة المقذوف",
                        "رقم الأمم المتحدة", "التوزيع", "الرقم المرجعي", "التصنيف", "النوع", "ملاحظات"
                    }
                    : new[]
                    {
                        "Name*", "Item No*", "Part No", "Arm Number", "NSN", "Price", "Minimum Quantity",
                        "Bullet Diameter", "Bullet Diameter Unit", "Total Weight", "Is Linked", "Primer",
                        "Case Type", "Propellant", "Compatibility", "Hazard Division", "Nature Option",
                        "Primary Purpose", "Projectile Color", "Projectile Material",
                        "UN Number", "Distribution", "Reference No", "Classification", "Type", "Notes"
                    };

                // Add headers with formatting
                for (int col = 1; col <= headers.Length; col++)
                {
                    templateSheet.Cells[1, col].Value = headers[col - 1];
                    templateSheet.Cells[1, col].Style.Font.Bold = true;
                    templateSheet.Cells[1, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    templateSheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    templateSheet.Cells[1, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                // Sample data row
                templateSheet.Cells[2, 1].Value = "6.5×55mm Swedish";
                templateSheet.Cells[2, 2].Value = "AMM-111";
                templateSheet.Cells[2, 3].Value = "P-655-SWE";
                templateSheet.Cells[2, 4].Value = "ARM-111";
                templateSheet.Cells[2, 5].Value = "1305-12-345-6789";
                templateSheet.Cells[2, 6].Value = 4.8;
                templateSheet.Cells[2, 7].Value = 100;
                templateSheet.Cells[2, 8].Value = 6.5;
                templateSheet.Cells[2, 9].Value = units.FirstOrDefault()?.NameEn ?? "";
                templateSheet.Cells[2, 10].Value = 12.5;
                templateSheet.Cells[2, 11].Value = "No";
                templateSheet.Cells[2, 12].Value = "Boxer";

                // Create hidden lookup sheets
                CreateLookupSheet(package, "Units", units.Select(u => u.NameEn ?? u.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                CreateLookupSheet(package, "CaseTypes", caseTypes.Select(c => c.NameEn ?? c.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                CreateLookupSheet(package, "Propellants", propellants.Select(p => p.NameEn ?? p.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                CreateLookupSheet(package, "Compatibilities", compatibilities.Select(c => c.NameEn ?? c.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                CreateLookupSheet(package, "HazardDivisions", hazardDivisions.Select(h => h.NameEn ?? h.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                CreateLookupSheet(package, "NatureOptions", natureOptions.Select(n => n.NameEn ?? n.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                CreateLookupSheet(package, "PrimaryPurposes", primaryPurposes.Select(p => p.NameEn ?? p.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                CreateLookupSheet(package, "ProjectileColors", projectileColors.Select(p => p.NameEn ?? p.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                CreateLookupSheet(package, "ProjectileMaterials", projectileMaterials.Select(p => p.NameEn ?? p.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                CreateLookupSheet(package, "Classifications", classifications.Select(c => c.NameEn ?? c.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                CreateLookupSheet(package, "ItemTypes", itemTypes.Select(i => i.NameEn ?? i.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());

                // Add data validation dropdowns
                AddDataValidation(templateSheet, 9, "Units"); // Bullet Diameter Unit (column 9)
                AddYesNoValidation(templateSheet, 11); // Is Linked (column 11)
                AddDataValidation(templateSheet, 13, "CaseTypes"); // Case Type
                AddDataValidation(templateSheet, 14, "Propellants"); // Propellant
                AddDataValidation(templateSheet, 15, "Compatibilities"); // Compatibility
                AddDataValidation(templateSheet, 16, "HazardDivisions"); // Hazard Division
                AddDataValidation(templateSheet, 17, "NatureOptions"); // Nature Option
                AddDataValidation(templateSheet, 18, "PrimaryPurposes"); // Primary Purpose
                AddDataValidation(templateSheet, 19, "ProjectileColors"); // Projectile Color
                AddDataValidation(templateSheet, 20, "ProjectileMaterials"); // Projectile Material
                AddDataValidation(templateSheet, 24, "Classifications"); // Classification
                AddDataValidation(templateSheet, 25, "ItemTypes"); // Type

                // Set column widths
                for (int col = 1; col <= headers.Length; col++)
                {
                    templateSheet.Column(col).Width = col == 26 ? 30 : 20; // Notes column wider
                }

                // Freeze header row
                templateSheet.View.FreezePanes(2, 1);

                var excelData = package.GetAsByteArray();

                _logger.LogInformation("Ammunition import template generated successfully. FileSize: {FileSize} bytes, Lookup sheets: 11", 
                    excelData.Length);

                return APIOperationResponse<byte[]>.Success(excelData, "Template generated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating ammunition import template");
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
        /// Add Yes/No validation to a column
        /// </summary>
        private void AddYesNoValidation(ExcelWorksheet worksheet, int column)
        {
            var columnLetter = GetColumnLetter(column);
            var validationRange = $"{columnLetter}2:{columnLetter}10000";

            var validation = worksheet.DataValidations.AddListValidation(validationRange);
            validation.Formula.Values.Add("Yes");
            validation.Formula.Values.Add("No");

            validation.ShowErrorMessage = true;
            validation.ErrorTitle = "Invalid Value";
            validation.Error = "Please select 'Yes' or 'No'";
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
