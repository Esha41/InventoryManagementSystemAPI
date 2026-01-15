using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
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
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.CrossCutting.Comman.Time;

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
        private readonly ApplicationDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ExplosiveService(
            ICrossCuttingRepository<Explosive> explosiveRepository,
            IMapper mapper,
            IValidator<CreateUpdateExplosiveDto> validator,
            ICurrentUserService currentUserService,
            ILogger<ExplosiveService> logger,
            IFileUploadService fileUploadService,
            IExcelImportService excelImportService,
            ApplicationDbContext context,
            IDateTimeProvider dateTimeProvider)
        {
            _explosiveRepository = explosiveRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
            _logger = logger;
            _fileUploadService = fileUploadService;
            _excelImportService = excelImportService;
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            
            // Set EPPlus license context
            ExcelPackage.License.SetNonCommercialPersonal("Ettad");
        }

        public async Task<APIOperationResponse<ExplosiveDto>> GetByIdAsync(long id)
        {
            try
            {
                var explosive = await _explosiveRepository.FindOneAsync(
                    e => e.Id == id && !e.IsDeleted,
                    false,
                    nameof(Explosive.HazardDivision),
                    nameof(Explosive.Classification),
                    nameof(Explosive.Type)
                );

                if (explosive == null)
                    return APIOperationResponse<ExplosiveDto>.Fail(ResponseType.NotFound, "Explosive not found");

                var dto = _mapper.Map<ExplosiveDto>(explosive);
                
                // Get images for this explosive
                var imagesResult = await _fileUploadService.GetByEntityAsync(FileEntityType.Explosive, explosive.Id);
                dto.Images = imagesResult.Succeeded && imagesResult.Data != null ? imagesResult.Data : new List<FileUploadDto>();
                
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
                    nameof(Explosive.HazardDivision),
                    nameof(Explosive.Classification),
                    nameof(Explosive.Type)
                );

                var dtos = _mapper.Map<List<ExplosiveDto>>(explosives);
                
                // Populate images for all explosives in a single database query
                var entityIds = dtos.Select(d => d.Id).ToList();
                var imagesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Explosive, entityIds);
                if (imagesResult.Succeeded && imagesResult.Data != null)
                {
                    foreach (var dto in dtos)
                    {
                        dto.Images = imagesResult.Data.ContainsKey(dto.Id) ? imagesResult.Data[dto.Id] : new List<FileUploadDto>();
                    }
                }
                
                return APIOperationResponse<List<ExplosiveDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
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
                explosive.CreationDate = _dateTimeProvider.Now;
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
                existingExplosive.ModificationDate = _dateTimeProvider.Now;
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



        public async Task<APIOperationResponse<ImportResult<CreateUpdateExplosiveDto>>> ImportAsync(IFormFile file, string language = "en")
        {
            _logger.LogInformation("Importing explosives from file. FileName: {FileName}, Language: {Language}, User: {UserId}", 
                file?.FileName, language, _currentUserService.UserId);
            
            try
            {
                var mappings = GetColumnMappings(language);
                var importResult = await _excelImportService.ImportFromExcelAsync<ExplosiveImportDto>(file, mappings);

                // Load lookup data for resolution
                var hazardDivisions = await _context.HazardDivisions.Where(h => !h.IsDeleted).ToListAsync();
                var classifications = await _context.Classifications.Where(c => !c.IsDeleted).ToListAsync();
                var itemTypes = await _context.ItemTypes.Where(i => !i.IsDeleted).ToListAsync();
                
                // Note: ExplosiveUnit is an enum, we'll parse it manually

                var finalResult = new ImportResult<CreateUpdateExplosiveDto>
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
                            var dto = new CreateUpdateExplosiveDto
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
                                Notes = importDto.Notes
                            };

                            // Resolve Lookups
                            
                            // Hazard Division
                            if (!string.IsNullOrWhiteSpace(importDto.HazardDivision))
                            {
                                var item = hazardDivisions.FirstOrDefault(h => 
                                    (h.NameEn != null && h.NameEn.Equals(importDto.HazardDivision, StringComparison.OrdinalIgnoreCase)) || 
                                    (h.NameAr != null && h.NameAr.Equals(importDto.HazardDivision, StringComparison.OrdinalIgnoreCase)));
                                dto.HazardDivisionId = item?.Id;
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

                            // NEQ Unit (Enum)
                            if (!string.IsNullOrWhiteSpace(importDto.NEQUnit))
                            {
                                if (Enum.TryParse<ExplosiveUnit>(importDto.NEQUnit, true, out var unitValue))
                                {
                                    dto.Unit = unitValue;
                                }
                                // If parsing fails, it defaults to first enum value or we could add error
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

                _logger.LogInformation("Explosive import completed. FileName: {FileName}, SuccessCount: {SuccessCount}, ErrorCount: {ErrorCount}, User: {UserId}", 
                    file?.FileName, finalResult.SuccessCount, finalResult.Errors.Count, _currentUserService.UserId);
                
                return APIOperationResponse<ImportResult<CreateUpdateExplosiveDto>>.Success(finalResult, "Import processed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing explosives from file. FileName: {FileName}, User: {UserId}", 
                    file?.FileName, _currentUserService.UserId);
                return APIOperationResponse<ImportResult<CreateUpdateExplosiveDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }
        
        public async Task<APIOperationResponse<ImportResult<CreateUpdateExplosiveDto>>> ImportPreviewAsync(IFormFile file, string language = "en")
        {
            _logger.LogInformation("Previewing explosive import. FileName: {FileName}, Language: {Language}, User: {UserId}", 
                file?.FileName, language, _currentUserService.UserId);
                
           try
            {
                var mappings = GetColumnMappings(language);
                var importResult = await _excelImportService.ImportFromExcelAsync<ExplosiveImportDto>(file, mappings);

                 // Load lookup data for resolution
                var hazardDivisions = await _context.HazardDivisions.Where(h => !h.IsDeleted).ToListAsync();
                var classifications = await _context.Classifications.Where(c => !c.IsDeleted).ToListAsync();
                var itemTypes = await _context.ItemTypes.Where(i => !i.IsDeleted).ToListAsync();

                var finalResult = new ImportResult<CreateUpdateExplosiveDto>
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
                        var rowErrors = new List<string>();
                        var dto = new CreateUpdateExplosiveDto
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
                            Notes = importDto.Notes
                        };
 
                        // Resolve Lookups
                        if (!string.IsNullOrWhiteSpace(importDto.HazardDivision))
                        {
                            var item = hazardDivisions.FirstOrDefault(h => 
                                (h.NameEn != null && h.NameEn.Equals(importDto.HazardDivision, StringComparison.OrdinalIgnoreCase)) || 
                                (h.NameAr != null && h.NameAr.Equals(importDto.HazardDivision, StringComparison.OrdinalIgnoreCase)));
                            dto.HazardDivisionId = item?.Id;
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
                        if (!string.IsNullOrWhiteSpace(importDto.NEQUnit))
                        {
                            if (Enum.TryParse<ExplosiveUnit>(importDto.NEQUnit, true, out var unitValue))
                                dto.Unit = unitValue;
                        }
 
                        // Validate using FluentValidation
                        var validationResult = await _validator.ValidateAsync(dto);
                        if (!validationResult.IsValid)
                        {
                            rowErrors.AddRange(validationResult.Errors.Select(e => e.ErrorMessage));
                        }
                        
                        // Check for duplicate ItemNo within the import file
                        if (!string.IsNullOrWhiteSpace(dto.ItemNo))
                        {
                            var itemNoKey = dto.ItemNo.Trim();
                            if (seenItemNos.Contains(itemNoKey))
                            {
                                rowErrors.Add($"Item No '{dto.ItemNo}' appears multiple times in the import file");
                            }
                            else
                            {
                                seenItemNos.Add(itemNoKey);
                                
                                // Check if ItemNo exists in database
                                var existing = await _explosiveRepository.FindOneAsync(e => !e.IsDeleted && e.ItemNo == itemNoKey);
                                if (existing != null)
                                {
                                    rowErrors.Add($"Item No '{dto.ItemNo}' already exists in the database");
                                }
                            }
                        }
                        
                        // Check for duplicate NSN within the import file
                        if (!string.IsNullOrWhiteSpace(dto.Nsn))
                        {
                            var nsnKey = dto.Nsn.Trim();
                            if (seenNsns.Contains(nsnKey))
                            {
                                rowErrors.Add($"NSN '{dto.Nsn}' appears multiple times in the import file");
                            }
                            else
                            {
                                seenNsns.Add(nsnKey);
                                
                                // Also check if NSN exists in database
                                var existing = await _explosiveRepository.FindOneAsync(e => !e.IsDeleted && e.Nsn == nsnKey);
                                if (existing != null)
                                {
                                    rowErrors.Add($"NSN '{dto.Nsn}' already exists in the database");
                                }
                            }
                        }
                        
                        if (rowErrors.Any())
                        {
                            finalResult.Errors.Add(new ImportError 
                            { 
                                RowNumber = importDto.RowNumber,
                                ErrorMessage = $"Row {importDto.RowNumber}: {string.Join("; ", rowErrors)}", 
                                ColumnName = "N/A",
                                RowData = dto
                            });
                        }
                        else
                        {
                            finalResult.SuccessfulRecords.Add(dto);
                        }
                    }
                }
                
                return APIOperationResponse<ImportResult<CreateUpdateExplosiveDto>>.Success(finalResult, "Preview processed");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<ImportResult<CreateUpdateExplosiveDto>>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }

        private Dictionary<string, string> GetColumnMappings(string language = "en")
        {
            var mappings = new Dictionary<string, string>
            {
                // English headers
                { "Name*", nameof(ExplosiveImportDto.Name) },
                { "Item No*", nameof(ExplosiveImportDto.ItemNo) },
                { "Part No", nameof(ExplosiveImportDto.PartNo) },
                { "Price", nameof(ExplosiveImportDto.Price) },
                { "Minimum Quantity", nameof(ExplosiveImportDto.MinimumQuantity) },
                { "NSN", nameof(ExplosiveImportDto.Nsn) },
                { "UN Number", nameof(ExplosiveImportDto.UNNumber) },
                { "NEQ Unit", nameof(ExplosiveImportDto.NEQUnit) },
                { "Distribution", nameof(ExplosiveImportDto.Distribution) },
                { "Reference No", nameof(ExplosiveImportDto.ReferenceNo) },
                { "Hazard Division", nameof(ExplosiveImportDto.HazardDivision) },
                { "Classification", nameof(ExplosiveImportDto.Classification) },
                { "Type", nameof(ExplosiveImportDto.Type) },
                { "Notes", nameof(ExplosiveImportDto.Notes) },
 
                // Arabic headers
                { "الاسم*", nameof(ExplosiveImportDto.Name) },
                { "رقم الصنف*", nameof(ExplosiveImportDto.ItemNo) },
                { "رقم الجزء", nameof(ExplosiveImportDto.PartNo) },
                { "رقم NSN", nameof(ExplosiveImportDto.Nsn) },
                // "NSN" is already in English block
                { "السعر", nameof(ExplosiveImportDto.Price) },
                { "الكمية الدنيا", nameof(ExplosiveImportDto.MinimumQuantity) },
                { "رقم الأمم المتحدة", nameof(ExplosiveImportDto.UNNumber) },
                { "وحدة NEQ", nameof(ExplosiveImportDto.NEQUnit) },
                { "التوزيع", nameof(ExplosiveImportDto.Distribution) },
                { "الرقم المرجعي", nameof(ExplosiveImportDto.ReferenceNo) },
                { "قسم الخطر", nameof(ExplosiveImportDto.HazardDivision) },
                { "التصنيف", nameof(ExplosiveImportDto.Classification) },
                { "النوع", nameof(ExplosiveImportDto.Type) },
                { "ملاحظات", nameof(ExplosiveImportDto.Notes) }
            };
 
            return mappings;
        }

        public async Task<APIOperationResponse<byte[]>> GenerateImportTemplateAsync(string language = "en")
        {
            try
            {
                _logger.LogInformation("Generating explosive import template with all fields and lookup data. Language: {Language}", language);

                // Load lookup data from database
                var units = await _context.Units
                    .Where(u => !u.IsDeleted)
                    .OrderBy(u => u.NameEn ?? u.NameAr)
                    .ToListAsync();

                var hazardDivisions = await _context.HazardDivisions
                    .Where(h => !h.IsDeleted)
                    .OrderBy(h => h.NameEn ?? h.NameAr)
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
                var templateSheet = package.Workbook.Worksheets.Add("Explosive Import");

                // Headers - Bilingual support (English / Arabic)
                var headers = language == "ar"
                    ? new[]
                    {
                        "الاسم*", "رقم الصنف*", "رقم الجزء", "NSN", "السعر", "الكمية الدنيا",
                        "نوع المتفجرات", "رقم الأمم المتحدة", "كمية المتفجرات الصافية", "وحدة NEQ",
                        "التوزيع", "الرقم المرجعي", "قسم الخطر", "التصنيف", "النوع", "ملاحظات"
                    }
                    : new[]
                    {
                        "Name*", "Item No*", "Part No", "NSN", "Price", "Minimum Quantity",
                        "Explosive Type", "UN Number", "Net Explosive Quantity", "NEQ Unit",
                        "Distribution", "Reference No", "Hazard Division", "Classification", "Type", "Notes"
                    };

                // Add headers with formatting
                for (int col = 1; col <= headers.Length; col++)
                {
                    templateSheet.Cells[1, col].Value = headers[col - 1];
                    templateSheet.Cells[1, col].Style.Font.Bold = true;
                    templateSheet.Cells[1, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    templateSheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                    templateSheet.Cells[1, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                // Sample data row
                templateSheet.Cells[2, 1].Value = "TNT";
                templateSheet.Cells[2, 2].Value = "EXP-001";
                templateSheet.Cells[2, 3].Value = "P-TNT";
                templateSheet.Cells[2, 4].Value = "1375-12-345-6789";
                templateSheet.Cells[2, 5].Value = 50;
                templateSheet.Cells[2, 6].Value = 5;
                templateSheet.Cells[2, 7].Value = "High Explosive";
                templateSheet.Cells[2, 8].Value = "UN0209";
                templateSheet.Cells[2, 9].Value = 10;
                templateSheet.Cells[2, 10].Value = units.FirstOrDefault()?.NameEn ?? "";

                // Create hidden lookup sheets
                CreateLookupSheet(package, "Units", units.Select(u => u.NameEn ?? u.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                CreateLookupSheet(package, "HazardDivisions", hazardDivisions.Select(h => h.NameEn ?? h.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                CreateLookupSheet(package, "Classifications", classifications.Select(c => c.NameEn ?? c.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());
                CreateLookupSheet(package, "ItemTypes", itemTypes.Select(i => i.NameEn ?? i.NameAr ?? "").Where(n => !string.IsNullOrWhiteSpace(n)).ToList());

                // Add data validation dropdowns
                AddDataValidation(templateSheet, 10, "Units"); // NEQ Unit (column 10)
                AddDataValidation(templateSheet, 13, "HazardDivisions"); // Hazard Division
                AddDataValidation(templateSheet, 14, "Classifications"); // Classification
                AddDataValidation(templateSheet, 15, "ItemTypes"); // Type

                // Set column widths
                for (int col = 1; col <= headers.Length; col++)
                {
                    templateSheet.Column(col).Width = col == 16 ? 30 : 20; // Notes column wider
                }

                // Freeze header row
                templateSheet.View.FreezePanes(2, 1);

                var excelData = package.GetAsByteArray();

                _logger.LogInformation("Explosive import template generated successfully. FileSize: {FileSize} bytes, Lookup sheets: 4", 
                    excelData.Length);

                return APIOperationResponse<byte[]>.Success(excelData, "Template generated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating explosive import template");
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
