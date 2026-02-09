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
using Ettad.CrossCutting.Comman.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Ettad.Inventory.Service.ItemDepartmentAssignments;

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
        private readonly ApplicationDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly AssetImportManager<CreateUpdateExplosiveDto, ExplosiveImportDto> _importManager;
        private readonly IItemDepartmentAssignmentService _itemDepartmentAssignmentService;

        // In-memory lookups
        private List<HazardDivision> _hazardDivisions;
        private List<Classification> _classifications;
        private List<ItemTypeLookup> _itemTypes;
        private List<Unit> _units;

        // Optimization: Dictionary for fast O(1) lookups during import
        private Dictionary<string, Dictionary<string, long>> _cachedLookups = new Dictionary<string, Dictionary<string, long>>();

        // Cache for existing records to prevent N+1 queries during validation
        private HashSet<string> _existingItemNos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _existingNsns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _newlyAddedItemNos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _newlyAddedNsns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public ExplosiveService(
            ICrossCuttingRepository<Explosive> explosiveRepository,
            IMapper mapper,
            IValidator<CreateUpdateExplosiveDto> validator,
            ICurrentUserService currentUserService,
            ILogger<ExplosiveService> logger,
            IFileUploadService fileUploadService,
            IExcelImportService excelImportService,
            ApplicationDbContext context,
            IDateTimeProvider dateTimeProvider,
            IItemDepartmentAssignmentService itemDepartmentAssignmentService)
        {
            _explosiveRepository = explosiveRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
            _logger = logger;
            _fileUploadService = fileUploadService;
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _itemDepartmentAssignmentService = itemDepartmentAssignmentService;

            _importManager = new AssetImportManager<CreateUpdateExplosiveDto, ExplosiveImportDto>(excelImportService,
               new LoggerFactory().CreateLogger<AssetImportManager<CreateUpdateExplosiveDto, ExplosiveImportDto>>());
        }

        public async Task<APIOperationResponse<ExplosiveDto>> GetByIdAsync(long id)
        {
            // existing implementation
            try
            {
                // Check if user has department and assigned items
                var assignedItemIds = await GetAssignedItemIdsAsync();
                if (assignedItemIds != null && !assignedItemIds.Contains(id))
                {
                    return APIOperationResponse<ExplosiveDto>.Fail(ResponseType.NotFound, "Explosive not found");
                }

                var explosive = await _explosiveRepository.FindOneAsync(
                    e => e.Id == id && !e.IsDeleted,
                    false,
                    nameof(Explosive.HazardDivision),
                    nameof(Explosive.Classification),
                    nameof(Explosive.Type),
                    nameof(Explosive.Unit)
                );

                if (explosive == null)
                    return APIOperationResponse<ExplosiveDto>.Fail(ResponseType.NotFound, "Explosive not found");

                var dto = _mapper.Map<ExplosiveDto>(explosive);

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
            // existing implementation
            try
            {
                // Check if user has department and assigned items
                var assignedItemIds = await GetAssignedItemIdsAsync();
                
                var explosives = await _explosiveRepository.FindAsync(
                    e => !e.IsDeleted && (assignedItemIds == null || assignedItemIds.Contains(e.Id)),
                    false,
                    nameof(Explosive.HazardDivision),
                    nameof(Explosive.Classification),
                    nameof(Explosive.Type),
                    nameof(Explosive.Unit)
                );

                var dtos = _mapper.Map<List<ExplosiveDto>>(explosives);

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

        public async Task<APIOperationResponse<PaginatedList<ExplosiveDto>>> GetAllPaginatedAsync(PagedListRequest request)
        {
            // existing implementation
            try
            {
                // Check if user has department and assigned items
                var assignedItemIds = await GetAssignedItemIdsAsync();
                
                var query = _explosiveRepository.Find(
                    e => !e.IsDeleted && (assignedItemIds == null || assignedItemIds.Contains(e.Id)),
                    false,
                    nameof(Explosive.HazardDivision),
                    nameof(Explosive.Classification),
                    nameof(Explosive.Type),
                    nameof(Explosive.Unit)
                );

                var paginatedEntities = await PaginatedList<Explosive>.CreateAsyncForTableBinding(query, request);

                var dtos = new List<ExplosiveDto>();
                if (paginatedEntities.Items.Any())
                {
                    dtos = _mapper.Map<List<ExplosiveDto>>(paginatedEntities.Items);

                    var entityIds = dtos.Select(d => d.Id).ToList();
                    var imagesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Explosive, entityIds);

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

                var result = new PaginatedList<ExplosiveDto>(
                    dtos,
                    paginatedEntities.TotalCount,
                    paginatedEntities.PageIndex,
                    request.PageSize
                );

                return APIOperationResponse<PaginatedList<ExplosiveDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<PaginatedList<ExplosiveDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateUpdateExplosiveDto inputDto, List<IFormFile>? files = null)
        {
            // existing implementation
            try
            {
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
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
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                            $"File upload failed: {saveFilesResult.Message}");
                    }
                }

                var explosive = _mapper.Map<Explosive>(inputDto);
                explosive.ItemType = ItemType.Explosive;
                explosive.CreationDate = _dateTimeProvider.Now;
                explosive.CreatedBy = _currentUserService.UserId;
                explosive.Nsn = string.IsNullOrWhiteSpace(inputDto.Nsn) ? null : inputDto.Nsn.Trim();

                var createdExplosive = await _explosiveRepository.AddAsync(explosive);

                if (files != null && files.Any())
                {
                    await _fileUploadService.UploadFilesForEntityAsync(
                        files,
                        FileEntityType.Explosive,
                        createdExplosive.Id);
                }

                return APIOperationResponse<long>.Success(createdExplosive.Id, "Explosive created successfully");
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null) msg += $" (Inner: {ex.InnerException.Message})";
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {msg}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateExplosiveDto inputDto)
        {
            // existing implementation
            try
            {
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

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

                _mapper.Map(inputDto, existingExplosive);
                existingExplosive.ModificationDate = _dateTimeProvider.Now;
                existingExplosive.ModifiedBy = _currentUserService.UserId;
                existingExplosive.Nsn = string.IsNullOrWhiteSpace(inputDto.Nsn) ? null : inputDto.Nsn.Trim();

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
            // existing implementation
            try
            {
                var explosive = await _explosiveRepository.FindOneAsync(e => e.Id == id && !e.IsDeleted);
                if (explosive == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Explosive not found");

                await _explosiveRepository.DeleteAsync(explosive);

                return APIOperationResponse<bool>.Success(true, "Explosive deleted successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        // Import Implementation
        public async Task<APIOperationResponse<ImportResult<CreateUpdateExplosiveDto>>> ImportAsync(IFormFile file, string language = "en")
        {
            return await _importManager.ImportAsync(
                file,
                language,
                items => LoadLookupsAsync(items),
                MapImportDtoToEntityAsync,
                ValidateDtoAsync,
                async (dto) => await CreateAsync(dto),
                GetColumnMappings(language));
        }

        public async Task<APIOperationResponse<ImportResult<ExplosiveImportDto>>> ImportPreviewAsync(IFormFile file, string language = "en")
        {
            return await _importManager.ImportPreviewAsync(
               file,
               language,
               items => LoadLookupsAsync(items),
               MapImportDtoToEntityAsync,
               ValidateDtoAsync,
               GetColumnMappings(language));
        }

        public async Task<APIOperationResponse<byte[]>> GenerateImportTemplateAsync(string language = "en")
        {
            await LoadLookupsAsync();

            var headers = language == "ar"
                   ? new[]
                   {
                        "الاسم*", "رقم الصنف*", "رقم الجزء", "NSN", "السعر", "الكمية الدنيا",
                        "رقم الأمم المتحدة", "وحدة",
                        "التوزيع", "الرقم المرجعي", "قسم الخطر", "التصنيف", "النوع", "ملاحظات"
                   }
                   : new[]
                   {
                        "Name*", "Item No*", "Part No", "NSN", "Price", "Minimum Quantity",
                        "UN Number", "Unit",
                        "Distribution", "Reference No", "Hazard Division", "Classification", "Type", "Notes"
                   };

            var firstAsset = await _context.Explosives
                .Include(e => e.HazardDivision)
                .Include(e => e.Classification)
                .Include(e => e.Type)
                .Include(e => e.Unit)
                .FirstOrDefaultAsync(e => !e.IsDeleted);

            return await _importManager.GenerateTemplateAsync(
                language,
                "Explosive Import",
                headers,
                (sheet) =>
                {
                    if (firstAsset != null)
                    {
                        var isAr = language == "ar";
                        sheet.Cells[2, 1].Value = firstAsset.Name;
                        sheet.Cells[2, 2].Value = firstAsset.ItemNo;
                        sheet.Cells[2, 3].Value = firstAsset.PartNo;
                        sheet.Cells[2, 4].Value = firstAsset.Nsn;
                        sheet.Cells[2, 5].Value = firstAsset.Price;
                        sheet.Cells[2, 6].Value = firstAsset.MinimumQuantity;
                        sheet.Cells[2, 7].Value = firstAsset.UNNumber;
                        sheet.Cells[2, 8].Value = isAr ? firstAsset.Unit?.NameAr : firstAsset.Unit?.NameEn; // Unit
                        sheet.Cells[2, 9].Value = firstAsset.Distribution;
                        sheet.Cells[2, 10].Value = firstAsset.ReferenceNo;
                        sheet.Cells[2, 11].Value = isAr ? firstAsset.HazardDivision?.NameAr : firstAsset.HazardDivision?.NameEn;
                        sheet.Cells[2, 12].Value = isAr ? firstAsset.Classification?.NameAr : firstAsset.Classification?.NameEn;
                        sheet.Cells[2, 13].Value = isAr ? firstAsset.Type?.NameAr : firstAsset.Type?.NameEn;
                        sheet.Cells[2, 14].Value = firstAsset.Notes;
                    }
                    else
                    {
                        sheet.Cells[2, 1].Value = "Sample Explosive";
                        sheet.Cells[2, 2].Value = "EXP-001";
                    }
                },
                (package) =>
                {
                    CreateLookupSheet(package, "HazardDivisions", _hazardDivisions);
                    CreateLookupSheet(package, "Classifications", _classifications);
                    CreateLookupSheet(package, "ItemTypes", _itemTypes);
                    CreateLookupSheet(package, "Units", _units);
                },
                (sheet) =>
                {
                    AddDataValidation(sheet, 8, "Units"); // Unit dropdown
                    AddDataValidation(sheet, 11, "HazardDivisions");
                    AddDataValidation(sheet, 12, "Classifications");
                    AddDataValidation(sheet, 13, "ItemTypes");
                }
            );
        }

        // Helpers


        private async Task LoadLookupsAsync(List<ExplosiveImportDto> importItems = null)
        {
            _hazardDivisions = await _context.HazardDivisions.Where(h => !h.IsDeleted).ToListAsync();
            _classifications = await _context.Classifications.Where(c => !c.IsDeleted).ToListAsync();
            _itemTypes = await _context.ItemTypes.Where(i => !i.IsDeleted && i.ItemType == ItemType.Explosive).ToListAsync();
            _units = await _context.Units.Where(u => !u.IsDeleted && u.ItemType == ItemType.Explosive).ToListAsync();

            // Build cache
            _cachedLookups["HazardDivisions"] = BuildLookup(_hazardDivisions, x => x.NameEn, x => x.NameAr, x => x.Id);
            _cachedLookups["Classifications"] = BuildLookup(_classifications, x => x.NameEn, x => x.NameAr, x => x.Id);
            _cachedLookups["ItemTypes"] = BuildLookup(_itemTypes, x => x.NameEn, x => x.NameAr, x => x.Id);
            _cachedLookups["Units"] = BuildLookup(_units, x => x.NameEn, x => x.NameAr, x => x.Id);

            // Optimization: Bulk fetch ItemNo and NSN duplicates
            _existingItemNos.Clear();
            _existingNsns.Clear();
            _newlyAddedItemNos.Clear();
            _newlyAddedNsns.Clear();

            if (importItems != null && importItems.Any())
            {
                var itemNos = importItems.Select(x => x.ItemNo).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();
                var nsns = importItems.Select(x => x.Nsn).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();

                var existingRecords = await _context.Explosives
                    .Where(e => !e.IsDeleted && (itemNos.Contains(e.ItemNo) || (e.Nsn != null && nsns.Contains(e.Nsn))))
                    .Select(e => new { e.ItemNo, e.Nsn })
                    .ToListAsync();

                foreach (var rec in existingRecords)
                {
                    if (!string.IsNullOrEmpty(rec.ItemNo)) _existingItemNos.Add(rec.ItemNo);
                    if (!string.IsNullOrEmpty(rec.Nsn)) _existingNsns.Add(rec.Nsn);
                }
            }
        }

        private async Task<CreateUpdateExplosiveDto> MapImportDtoToEntityAsync(ExplosiveImportDto importDto, string language)
        {
            var dto = new CreateUpdateExplosiveDto
            {
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

            dto.HazardDivisionId = FindLookupIdCached("HazardDivisions", importDto.HazardDivision);
            dto.ClassificationId = FindLookupIdCached("Classifications", importDto.Classification);
            dto.TypeId = FindLookupIdCached("ItemTypes", importDto.Type);
            dto.UnitId = FindLookupIdCached("Units", importDto.NEQUnit);

            return dto;
        }

        private Dictionary<string, long> BuildLookup<T>(IEnumerable<T> items, Func<T, string> getNameEn, Func<T, string> getNameAr, Func<T, long> getId)
        {
            var dict = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
            if (items == null) return dict;

            foreach (var item in items)
            {
                var en = getNameEn(item);
                if (!string.IsNullOrWhiteSpace(en) && !dict.ContainsKey(en)) dict[en] = getId(item);

                var ar = getNameAr(item);
                if (!string.IsNullOrWhiteSpace(ar) && !dict.ContainsKey(ar)) dict[ar] = getId(item);
            }
            return dict;
        }

        private long? FindLookupIdCached(string key, string name)
        {
            if (string.IsNullOrWhiteSpace(name) || !_cachedLookups.ContainsKey(key)) return null;
            if (_cachedLookups[key].TryGetValue(name, out var id)) return id;
            return null;
        }

        private async Task<List<string>> ValidateDtoAsync(CreateUpdateExplosiveDto dto)
        {
            var errors = new List<string>();
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                errors.AddRange(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            // Check duplicates using optimized HashSets
            if (!string.IsNullOrWhiteSpace(dto.ItemNo))
            {
                if (_existingItemNos.Contains(dto.ItemNo))
                    errors.Add($"Item No '{dto.ItemNo}' already exists in the database");
                else if (_newlyAddedItemNos.Contains(dto.ItemNo))
                    errors.Add($"Item No '{dto.ItemNo}' is duplicated in the current file");
                else
                    _newlyAddedItemNos.Add(dto.ItemNo);
            }

            if (!string.IsNullOrWhiteSpace(dto.Nsn))
            {
                if (_existingNsns.Contains(dto.Nsn))
                    errors.Add($"NSN '{dto.Nsn}' already exists in the database");
                else if (_newlyAddedNsns.Contains(dto.Nsn))
                    errors.Add($"NSN '{dto.Nsn}' is duplicated in the current file");
                else
                    _newlyAddedNsns.Add(dto.Nsn);
            }

            return errors;
        }

        private Dictionary<string, string> GetColumnMappings(string language)
        {
            return new Dictionary<string, string>
            {
                { "Name*", nameof(ExplosiveImportDto.Name) },
                { "Item No*", nameof(ExplosiveImportDto.ItemNo) },
                { "Part No", nameof(ExplosiveImportDto.PartNo) },
                { "Price", nameof(ExplosiveImportDto.Price) },
                { "Minimum Quantity", nameof(ExplosiveImportDto.MinimumQuantity) },
                { "NSN", nameof(ExplosiveImportDto.Nsn) },
                { "UN Number", nameof(ExplosiveImportDto.UNNumber) },
                { "Unit", nameof(ExplosiveImportDto.NEQUnit) },
                { "Distribution", nameof(ExplosiveImportDto.Distribution) },
                { "Reference No", nameof(ExplosiveImportDto.ReferenceNo) },
                { "Hazard Division", nameof(ExplosiveImportDto.HazardDivision) },
                { "Classification", nameof(ExplosiveImportDto.Classification) },
                { "Type", nameof(ExplosiveImportDto.Type) },
                { "Notes", nameof(ExplosiveImportDto.Notes) },
                // Arabic
                { "الاسم*", nameof(ExplosiveImportDto.Name) },
                { "رقم الصنف*", nameof(ExplosiveImportDto.ItemNo) },
                { "رقم الجزء", nameof(ExplosiveImportDto.PartNo) },
                { "رقم NSN", nameof(ExplosiveImportDto.Nsn) },
                { "السعر", nameof(ExplosiveImportDto.Price) },
                { "الكمية الدنيا", nameof(ExplosiveImportDto.MinimumQuantity) },
                { "رقم الأمم المتحدة", nameof(ExplosiveImportDto.UNNumber) },
                { "وحدة", nameof(ExplosiveImportDto.NEQUnit) },
                { "التوزيع", nameof(ExplosiveImportDto.Distribution) },
                { "الرقم المرجعي", nameof(ExplosiveImportDto.ReferenceNo) },
                { "قسم الخطر", nameof(ExplosiveImportDto.HazardDivision) },
                { "التصنيف", nameof(ExplosiveImportDto.Classification) },
                { "النوع", nameof(ExplosiveImportDto.Type) },
                { "ملاحظات", nameof(ExplosiveImportDto.Notes) }
            };
        }

        private void CreateLookupSheet<T>(ExcelPackage package, string sheetName, List<T> items)
        {
            var names = items.Select(x => {
                var nameEn = (string)x.GetType().GetProperty("NameEn")?.GetValue(x);
                var nameAr = (string)x.GetType().GetProperty("NameAr")?.GetValue(x);
                return nameEn ?? nameAr ?? "";
            }).Where(x => !string.IsNullOrEmpty(x)).ToList();

            var lookupSheet = package.Workbook.Worksheets.Add(sheetName);
            lookupSheet.Hidden = eWorkSheetHidden.Hidden;

            for (int i = 0; i < names.Count; i++)
            {
                lookupSheet.Cells[i + 1, 1].Value = names[i];
            }
        }

        private void CreateStringListLookupSheet(ExcelPackage package, string sheetName, List<string> values)
        {
            var lookupSheet = package.Workbook.Worksheets.Add(sheetName);
            lookupSheet.Hidden = eWorkSheetHidden.Hidden;

            for (int i = 0; i < values.Count; i++)
            {
                lookupSheet.Cells[i + 1, 1].Value = values[i];
            }
        }

        private void AddDataValidation(ExcelWorksheet worksheet, int column, string lookupSheetName)
        {
            var columnLetter = GetColumnLetter(column);
            var validationRange = $"{columnLetter}2:{columnLetter}10000";
            var validation = worksheet.DataValidations.AddListValidation(validationRange);
            var lookupSheet = worksheet.Workbook.Worksheets[lookupSheetName];
            var lastRow = lookupSheet.Dimension?.End.Row ?? 1;
            validation.Formula.ExcelFormula = $"'{lookupSheetName}'!$A$1:$A${lastRow}";
            validation.ShowErrorMessage = true;
            validation.Error = $"Please select a value from the {lookupSheetName} list";
        }

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

        /// <summary>
        /// Gets the list of assigned item IDs for the current user's department.
        /// Returns null if user has no department or no assigned items (meaning no filtering should be applied).
        /// Returns a HashSet with assigned item IDs if user has a department and has assigned items (meaning filter to only those items).
        /// </summary>
        private async Task<HashSet<long>?> GetAssignedItemIdsAsync()
        {
            // If user has no department, return null to indicate no filtering
            if (!_currentUserService.DepartmentId.HasValue)
            {
                return null;
            }

            // Get assigned items for the user's department
            var assignmentsResult = await _itemDepartmentAssignmentService.GetByDepartmentIdAsync(_currentUserService.DepartmentId.Value);
            
            // If no assignments found or error occurred, return null (no filtering - keep code as is)
            if (!assignmentsResult.Succeeded || assignmentsResult.Data == null || !assignmentsResult.Data.Any())
            {
                return null;
            }

            // Return the set of assigned item IDs (filter to only these items)
            return new HashSet<long>(assignmentsResult.Data.Select(a => a.ItemId));
        }
    }
}
