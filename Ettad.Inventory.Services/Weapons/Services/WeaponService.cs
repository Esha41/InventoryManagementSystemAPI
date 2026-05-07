using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Weapons.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Comman.Models;
using System;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Inventory.Service.Common.Interfaces;
using Ettad.Inventory.Service.ItemDepartmentAssignments.Interfaces;
using Ettad.Inventory.Service.Weapons.Interfaces;

namespace Ettad.Inventory.Service.Weapons.Services
{
    public class WeaponService : IWeaponService
    {
        private static readonly string[] WeaponTemplateSampleIncludes =
        {
            nameof(Weapon.LookupCaliber),
            nameof(Weapon.CaliberUnit),
            nameof(Weapon.CountryOfManufacture),
            nameof(Weapon.Classification),
            nameof(Weapon.Type),
            "BaseItemPrimaryPurposes.PrimaryPurpos"
        };

        private readonly ICrossCuttingRepository<Weapon> _weaponRepository;
        private readonly ICrossCuttingRepository<BaseItemPrimaryPurpos> _baseItemPrimaryPurposRepository;
        private readonly ICrossCuttingRepository<ItemDepartmentAssignment> _itemDepartmentAssignmentRepository;
        private readonly ICrossCuttingRepository<InventoryDetail> _inventoryDetailRepository;
        private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;
        private readonly ICrossCuttingRepository<SupplyDetail> _supplyDetailRepository;
        private readonly ICrossCuttingRepository<AllowanceItem> _allowanceItemRepository;
        private readonly ICrossCuttingRepository<AssetSupplyDetail> _assetSupplyDetailRepository;
        private readonly ICrossCuttingRepository<Asset> _assetRepository;
        private readonly ICrossCuttingRepository<BaseItem> _baseItemRepository;
        private readonly ICrossCuttingRepository<Unit> _unitRepository;
        private readonly ICrossCuttingRepository<Country> _countryRepository;
        private readonly ICrossCuttingRepository<Classification> _classificationRepository;
        private readonly ICrossCuttingRepository<ItemTypeLookup> _itemTypeLookupRepository;
        private readonly ICrossCuttingRepository<Caliber> _caliberRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateWeaponDto> _validator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<WeaponService> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly ITransactionManager _transactionManager;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly AssetImportManager<CreateUpdateWeaponDto, WeaponImportDto> _importManager;
        private readonly IItemDepartmentAssignmentService _itemDepartmentAssignmentService;

        // In-memory lookups
        private List<Unit> _units;
        private List<Country> _countries;
        private List<Classification> _classifications;
        private List<ItemTypeLookup> _itemTypes;
        private List<Caliber> _calibers;

        // Optimization: Dictionary for fast O(1) lookups during import
        private Dictionary<string, Dictionary<string, long>> _cachedLookups = new Dictionary<string, Dictionary<string, long>>();

        // Cache for existing records to prevent N+1 queries during validation
        private HashSet<string> _existingItemNos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _existingNsns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _newlyAddedItemNos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _newlyAddedNsns = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public WeaponService(
            ICrossCuttingRepository<Weapon> weaponRepository,
            ICrossCuttingRepository<BaseItemPrimaryPurpos> baseItemPrimaryPurposRepository,
            ICrossCuttingRepository<ItemDepartmentAssignment> itemDepartmentAssignmentRepository,
            ICrossCuttingRepository<InventoryDetail> inventoryDetailRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            ICrossCuttingRepository<SupplyDetail> supplyDetailRepository,
            ICrossCuttingRepository<AllowanceItem> allowanceItemRepository,
            ICrossCuttingRepository<AssetSupplyDetail> assetSupplyDetailRepository,
            ICrossCuttingRepository<Asset> assetRepository,
            ICrossCuttingRepository<BaseItem> baseItemRepository,
            ICrossCuttingRepository<Unit> unitRepository,
            ICrossCuttingRepository<Country> countryRepository,
            ICrossCuttingRepository<Classification> classificationRepository,
            ICrossCuttingRepository<ItemTypeLookup> itemTypeLookupRepository,
            ICrossCuttingRepository<Caliber> caliberRepository,
            IMapper mapper,
            IValidator<CreateUpdateWeaponDto> validator,
            ICurrentUserService currentUserService,
            ILogger<WeaponService> logger,
            IFileUploadService fileUploadService,
            IExcelImportService excelImportService,
            ITransactionManager transactionManager,
            IDateTimeProvider dateTimeProvider,
            IItemDepartmentAssignmentService itemDepartmentAssignmentService)
        {
            _weaponRepository = weaponRepository;
            _baseItemPrimaryPurposRepository = baseItemPrimaryPurposRepository;
            _itemDepartmentAssignmentRepository = itemDepartmentAssignmentRepository;
            _inventoryDetailRepository = inventoryDetailRepository;
            _requestItemRepository = requestItemRepository;
            _supplyDetailRepository = supplyDetailRepository;
            _allowanceItemRepository = allowanceItemRepository;
            _assetSupplyDetailRepository = assetSupplyDetailRepository;
            _assetRepository = assetRepository;
            _baseItemRepository = baseItemRepository;
            _unitRepository = unitRepository;
            _countryRepository = countryRepository;
            _classificationRepository = classificationRepository;
            _itemTypeLookupRepository = itemTypeLookupRepository;
            _caliberRepository = caliberRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
            _logger = logger;
            _fileUploadService = fileUploadService;
            _transactionManager = transactionManager;
            _dateTimeProvider = dateTimeProvider;
            _itemDepartmentAssignmentService = itemDepartmentAssignmentService;

            _importManager = new AssetImportManager<CreateUpdateWeaponDto, WeaponImportDto>(excelImportService,
               new LoggerFactory().CreateLogger<AssetImportManager<CreateUpdateWeaponDto, WeaponImportDto>>());
        }

        public async Task<APIOperationResponse<WeaponDto>> GetByIdAsync(long id, bool includeDeleted = false)
        {
            _logger.LogInformation("Getting weapon by ID. WeaponId: {WeaponId}, User: {UserId}",
                 id, _currentUserService.UserId);

            try
            {
                // Check if user has department and assigned items
                var assignedItemIds = await GetAssignedItemIdsAsync();
                if (assignedItemIds != null && !assignedItemIds.Contains(id))
                {
                    return APIOperationResponse<WeaponDto>.Fail(ResponseType.NotFound, "Weapon not found");
                }

                var weapon = await _weaponRepository.FindOneAsync(
                    w => w.Id == id && (includeDeleted || !w.IsDeleted),
                    includeDeleted,
                    nameof(Weapon.CaliberUnit),
                    nameof(Weapon.LookupCaliber),
                    nameof(Weapon.CountryOfManufacture),
                    nameof(Weapon.Classification),
                    nameof(Weapon.Type),
                    "BaseItemPrimaryPurposes.PrimaryPurpos"
                );

                if (weapon == null)
                {
                    return APIOperationResponse<WeaponDto>.Fail(ResponseType.NotFound, "Weapon not found");
                }

                var dto = _mapper.Map<WeaponDto>(weapon);

                var imagesResult = await _fileUploadService.GetByEntityAsync(FileEntityType.Weapon, weapon.Id);
                dto.Images = imagesResult.Succeeded && imagesResult.Data != null ? imagesResult.Data : new List<FileUploadDto>();

                return APIOperationResponse<WeaponDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving weapon by ID. WeaponId: {WeaponId}, User: {UserId}",
                    id, _currentUserService.UserId);
                return APIOperationResponse<WeaponDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<PaginatedList<WeaponDto>>> GetAllPaginatedAsync(PagedListRequest request)
        {
            _logger.LogInformation("Getting weapons paginated. Page: {Page}, PageSize: {PageSize}, User: {UserId}",
                request.Page, request.PageSize, _currentUserService.UserId);

            try
            {
                // Check if user has department and assigned items
                var assignedItemIds = await GetAssignedItemIdsAsync();
                var showDeletedOnly = request.DeletedOnly == true;
                
                var query = _weaponRepository.Find(
                    w => (showDeletedOnly ? w.IsDeleted : !w.IsDeleted) && (assignedItemIds == null || assignedItemIds.Contains(w.Id)),
                    showDeletedOnly,
                    nameof(Weapon.CaliberUnit),
                    nameof(Weapon.LookupCaliber),
                    nameof(Weapon.CountryOfManufacture),
                    nameof(Weapon.Classification),
                    nameof(Weapon.Type),
                    "BaseItemPrimaryPurposes.PrimaryPurpos"
                );

                var paginatedEntities = await PaginatedList<Weapon>.CreateAsyncForTableBinding(query, request);

                var dtos = new List<WeaponDto>();
                if (paginatedEntities.Items.Any())
                {
                    dtos = _mapper.Map<List<WeaponDto>>(paginatedEntities.Items);

                    var entityIds = dtos.Select(d => d.Id).ToList();
                    var imagesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Weapon, entityIds);

                    if (imagesResult.Succeeded && imagesResult.Data != null)
                    {
                        foreach (var dto in dtos)
                        {
                            if (imagesResult.Data.ContainsKey(dto.Id)) dto.Images = imagesResult.Data[dto.Id];
                        }
                    }
                }

                var result = new PaginatedList<WeaponDto>(
                    dtos,
                    paginatedEntities.TotalCount,
                    paginatedEntities.PageIndex,
                    request.PageSize
                );

                return APIOperationResponse<PaginatedList<WeaponDto>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting weapons paginated. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<PaginatedList<WeaponDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<WeaponDto>>> GetAllAsync()
        {
            _logger.LogInformation("Getting all weapons. User: {UserId}", _currentUserService.UserId);

            try
            {
                // Check if user has department and assigned items
                var assignedItemIds = await GetAssignedItemIdsAsync();
                
                var weapons = await _weaponRepository.FindAsync(
                    w => !w.IsDeleted && (assignedItemIds == null || assignedItemIds.Contains(w.Id)),
                    false,
                    nameof(Weapon.CaliberUnit),
                    nameof(Weapon.LookupCaliber),
                    nameof(Weapon.CountryOfManufacture),
                    nameof(Weapon.Classification),
                    nameof(Weapon.Type),
                    "BaseItemPrimaryPurposes.PrimaryPurpos"
                );

                var dtos = _mapper.Map<List<WeaponDto>>(weapons);

                var entityIds = dtos.Select(d => d.Id).ToList();
                var imagesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Weapon, entityIds);
                if (imagesResult.Succeeded && imagesResult.Data != null)
                {
                    foreach (var dto in dtos)
                    {
                        dto.Images = imagesResult.Data.ContainsKey(dto.Id) ? imagesResult.Data[dto.Id] : new List<FileUploadDto>();
                    }
                }

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
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
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
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                            $"File upload failed: {saveFilesResult.Message}");
                    }
                }

                var weapon = _mapper.Map<Weapon>(inputDto);
                weapon.CaliberCategory = inputDto.CaliberCategory ?? WeaponCaliberCategory.Small;
                weapon.ItemType = ItemType.Weapon;
                weapon.CreationDate = _dateTimeProvider.Now;
                weapon.CreatedBy = _currentUserService.UserId;
                weapon.Nsn = string.IsNullOrWhiteSpace(inputDto.Nsn) ? null : inputDto.Nsn.Trim();

                var createdWeapon = await _weaponRepository.AddAsync(weapon);

                if (inputDto.PrimaryPurposIds != null && inputDto.PrimaryPurposIds.Any())
                {
                    var purposeRows = inputDto.PrimaryPurposIds
                        .Select(pid => new BaseItemPrimaryPurpos { BaseItemId = createdWeapon.Id, PrimaryPurposId = pid })
                        .ToList();
                    await _baseItemPrimaryPurposRepository.AddRangeAsync(purposeRows);
                }

                if (files != null && files.Any())
                {
                    await _fileUploadService.UploadFilesForEntityAsync(
                       files,
                       FileEntityType.Weapon,
                       createdWeapon.Id);
                }

                return APIOperationResponse<long>.Success(createdWeapon.Id, "Weapon created successfully");
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null) msg += $" (Inner: {ex.InnerException.Message})";
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {msg}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateWeaponDto inputDto)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                var existingWeapon = await _weaponRepository.FindOneAsync(w => w.Id == id && !w.IsDeleted);
                if (existingWeapon == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Weapon not found");

                if (!string.IsNullOrWhiteSpace(inputDto.Nsn))
                {
                    var duplicate = await _weaponRepository.FindOneAsync(
                        w => !w.IsDeleted && w.Id != id && w.Nsn == inputDto.Nsn.Trim());

                    if (duplicate != null)
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "NSN already exists");
                }

                _mapper.Map(inputDto, existingWeapon);
                existingWeapon.CaliberCategory = inputDto.CaliberCategory!.Value;
                existingWeapon.ModificationDate = _dateTimeProvider.Now;
                existingWeapon.ModifiedBy = _currentUserService.UserId;
                existingWeapon.Nsn = string.IsNullOrWhiteSpace(inputDto.Nsn) ? null : inputDto.Nsn.Trim();

                await _weaponRepository.UpdateAsync(existingWeapon);

                var existingPurposes = (await _baseItemPrimaryPurposRepository.FindAsync(x => x.BaseItemId == id)).ToList();
                foreach (var p in existingPurposes)
                    await _baseItemPrimaryPurposRepository.DeleteAsync(p);

                if (inputDto.PrimaryPurposIds != null && inputDto.PrimaryPurposIds.Any())
                {
                    var newPurposes = inputDto.PrimaryPurposIds
                        .Select(pid => new BaseItemPrimaryPurpos { BaseItemId = id, PrimaryPurposId = pid })
                        .ToList();
                    await _baseItemPrimaryPurposRepository.AddRangeAsync(newPurposes);
                }

                return APIOperationResponse<bool>.Success(true, "Weapon updated successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            try
            {
                var weapon = await _weaponRepository.FindOneAsync(w => w.Id == id && !w.IsDeleted);
                if (weapon == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Weapon not found");

                var hasRefs = await ItemHasReferencesAsync(id);
                if (hasRefs)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Cannot delete this weapon because it is referenced by other records (e.g. inventory, department assignments, supply requests, or allowances). Please remove those references first.");
                }

                await _weaponRepository.DeleteAsync(weapon);

                return APIOperationResponse<bool>.Success(true, "Weapon deleted successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> RestoreAsync(long id)
        {
            try
            {
                var weapon = await _weaponRepository.FindOneAsync(w => w.Id == id, includeSoftDeleted: true);
                if (weapon == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Weapon not found");
                }

                if (!weapon.IsDeleted)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Weapon is not deleted");
                }

                weapon.IsDeleted = false;
                weapon.DeletionDate = null;
                weapon.DeletedBy = null;

                await _weaponRepository.UpdateAsync(weapon);

                return APIOperationResponse<bool>.Success(true, "Weapon restored successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restoring weapon. WeaponId: {WeaponId}", id);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> PermanentDeleteAsync(long id)
        {
            try
            {
                var weapon = await _weaponRepository.FindOneAsync(w => w.Id == id, includeSoftDeleted: true);
                if (weapon == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Weapon not found");
                }

                if (!weapon.IsDeleted)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Only soft-deleted weapons can be permanently deleted");
                }

                await using var transaction = await _transactionManager.BeginAsync();
                try
                {
                    var purposes = (await _baseItemPrimaryPurposRepository.FindAsync(x => x.BaseItemId == id)).ToList();
                    foreach (var p in purposes)
                        await _baseItemPrimaryPurposRepository.DeleteAsync(p);

                    await _weaponRepository.DeleteAsync(weapon);

                    var baseItem = await _baseItemRepository.GetByIdAsync(id);
                    if (baseItem == null)
                    {
                        await _transactionManager.RollbackAsync();
                        return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, "Failed to remove base item record");
                    }

                    await _baseItemRepository.DeleteAsync(baseItem);

                    await _transactionManager.CommitAsync();
                    return APIOperationResponse<bool>.Success(true, "Weapon permanently deleted");
                }
                catch (Exception ex)
                {
                    await _transactionManager.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                if (IsForeignKeyViolation(ex))
                {
                    _logger.LogWarning(ex, "Foreign key constraint prevented permanent delete of weapon. WeaponId: {WeaponId}", id);
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Cannot permanently delete this weapon because it is referenced by other records. Please remove those references first.");
                }
                _logger.LogError(ex, "Error permanently deleting weapon. WeaponId: {WeaponId}", id);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if the item (BaseItem/Weapon) is referenced by inventory, department assignments, supply requests, allowances, or asset supplies.
        /// </summary>
        private async Task<bool> ItemHasReferencesAsync(long itemId)
        {
            var hasDeptAssignment = await _itemDepartmentAssignmentRepository
                .Find(x => x.ItemId == itemId)
                .AnyAsync();
            if (hasDeptAssignment) return true;

            var hasInventory = await _inventoryDetailRepository
                .Find(x => x.ItemId == itemId)
                .AnyAsync();
            if (hasInventory) return true;

            var hasRequestItem = await _requestItemRepository
                .Find(x => x.ItemId == itemId && !x.IsDeleted)
                .AnyAsync();
            if (hasRequestItem) return true;

            var hasSupplyDetail = await _supplyDetailRepository
                .Find(x => x.ItemId == itemId && !x.IsDeleted)
                .AnyAsync();
            if (hasSupplyDetail) return true;

            var hasAllowance = await _allowanceItemRepository
                .Find(x => x.ItemId == itemId && !x.IsDeleted)
                .AnyAsync();
            if (hasAllowance) return true;

            var hasAssetSupply = await _assetSupplyDetailRepository
                .Find(x => x.ItemId == itemId && !x.IsDeleted)
                .AnyAsync();
            if (hasAssetSupply) return true;

            var hasAsset = await _assetRepository
                .Find(x => x.ItemId == itemId && !x.IsDeleted)
                .AnyAsync();
            if (hasAsset) return true;

            return false;
        }

        private static bool IsForeignKeyViolation(Exception ex)
        {
            while (ex != null)
            {
                var msg = ex.Message ?? string.Empty;
                if (msg.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) ||
                    msg.Contains("REFERENCE constraint", StringComparison.OrdinalIgnoreCase) ||
                    msg.Contains("referenced by", StringComparison.OrdinalIgnoreCase))
                    return true;
                ex = ex.InnerException;
            }
            return false;
        }

        // Import Implementation
        public async Task<APIOperationResponse<ImportResult<CreateUpdateWeaponDto>>> ImportAsync(IFormFile file, string language = "en")
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

        public async Task<APIOperationResponse<ImportResult<WeaponImportDto>>> ImportPreviewAsync(IFormFile file, string language = "en")
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
                        "الاسم*", "رقم الصنف*", "Part No", "رقم NSN", "السعر", "الكمية الدنيا",
                        "العيار", "وحدة العيار", "سنة الصنع", "بلد الصنع", "النموذج",
                        "رقم الأمم المتحدة", "التوزيع", "الرقم المرجعي", "التصنيف", "النوع", "ملاحظات"
                   }
                   : new[]
                   {
                        "Name*", "Item No*", "Part No", "NSN", "Price", "Minimum Quantity",
                        "Caliber", "Caliber Unit", "Year Of Manufacture", "Country Of Manufacture", "Model",
                        "UN Number", "Distribution", "Reference No", "Classification", "Type", "Notes"
                   };

            var firstAsset = await _weaponRepository.FindOneAsync(
                w => !w.IsDeleted,
                false,
                WeaponTemplateSampleIncludes);

            return await _importManager.GenerateTemplateAsync(
                language,
                "Weapon Import",
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
                        sheet.Cells[2, 7].Value = isAr ? firstAsset.LookupCaliber?.NameAr : firstAsset.LookupCaliber?.NameEn;
                        sheet.Cells[2, 8].Value = isAr ? firstAsset.CaliberUnit?.NameAr : firstAsset.CaliberUnit?.NameEn;
                        sheet.Cells[2, 9].Value = firstAsset.YearOfManufacture;
                        sheet.Cells[2, 10].Value = isAr ? firstAsset.CountryOfManufacture?.NameAr : firstAsset.CountryOfManufacture?.NameEn;
                        sheet.Cells[2, 11].Value = firstAsset.Model;
                        sheet.Cells[2, 12].Value = firstAsset.UNNumber;
                        sheet.Cells[2, 13].Value = firstAsset.Distribution;
                        sheet.Cells[2, 14].Value = firstAsset.ReferenceNo;
                        sheet.Cells[2, 15].Value = isAr ? firstAsset.Classification?.NameAr : firstAsset.Classification?.NameEn;
                        sheet.Cells[2, 16].Value = isAr ? firstAsset.Type?.NameAr : firstAsset.Type?.NameEn;
                        sheet.Cells[2, 17].Value = firstAsset.Notes;
                    }
                    else
                    {
                        sheet.Cells[2, 1].Value = "Sample Weapon";
                        sheet.Cells[2, 2].Value = "WPN-001";
                    }
                },
                (package) =>
                {
                    CreateLookupSheet(package, "Units", _units);
                    CreateLookupSheet(package, "Calibers", _calibers);
                    CreateLookupSheet(package, "Countries", _countries);
                    CreateLookupSheet(package, "Classifications", _classifications);
                    CreateLookupSheet(package, "ItemTypes", _itemTypes);
                },
                (sheet) =>
                {
                    AddDataValidation(sheet, 7, "Calibers");
                    AddDataValidation(sheet, 8, "Units");
                    AddDataValidation(sheet, 10, "Countries");
                    AddDataValidation(sheet, 15, "Classifications");
                    AddDataValidation(sheet, 16, "ItemTypes");
                }
            );
        }

        // Helpers
        private async Task LoadLookupsAsync(List<WeaponImportDto> importItems = null)
        {
            _units = await _unitRepository.Find(u => !u.IsDeleted && u.ItemType == ItemType.Weapon).ToListAsync();
            _countries = await _countryRepository.Find(c => !c.IsDeleted).ToListAsync();
            _classifications = await _classificationRepository.Find(c => !c.IsDeleted).ToListAsync();
            _itemTypes = await _itemTypeLookupRepository.Find(i => !i.IsDeleted && i.ItemType == ItemType.Weapon).ToListAsync();
            _calibers = await _caliberRepository.Find(c => !c.IsDeleted && c.ItemType == ItemType.Weapon).ToListAsync();

            // Build cache
            _cachedLookups["Units"] = BuildLookup(_units, x => x.NameEn, x => x.NameAr, x => x.Id);
            _cachedLookups["Calibers"] = BuildLookup(_calibers, x => x.NameEn, x => x.NameAr, x => x.Id);
            _cachedLookups["Countries"] = BuildLookup(_countries, x => x.NameEn, x => x.NameAr, x => x.Id);
            _cachedLookups["Classifications"] = BuildLookup(_classifications, x => x.NameEn, x => x.NameAr, x => x.Id);
            _cachedLookups["ItemTypes"] = BuildLookup(_itemTypes, x => x.NameEn, x => x.NameAr, x => x.Id);

            // Optimization: Bulk fetch ItemNo and NSN duplicates
            _existingItemNos.Clear();
            _existingNsns.Clear();
            _newlyAddedItemNos.Clear();
            _newlyAddedNsns.Clear();

            if (importItems != null && importItems.Any())
            {
                var itemNos = importItems.Select(x => x.ItemNo).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();
                var nsns = importItems.Select(x => x.Nsn).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();

                var existingRecords = await _weaponRepository
                    .Find(w => !w.IsDeleted && (itemNos.Contains(w.ItemNo) || w.Nsn != null && nsns.Contains(w.Nsn)))
                    .Select(w => new { w.ItemNo, w.Nsn })
                    .ToListAsync();

                foreach (var rec in existingRecords)
                {
                    if (!string.IsNullOrEmpty(rec.ItemNo)) _existingItemNos.Add(rec.ItemNo);
                    if (!string.IsNullOrEmpty(rec.Nsn)) _existingNsns.Add(rec.Nsn);
                }
            }
        }

        private async Task<CreateUpdateWeaponDto> MapImportDtoToEntityAsync(WeaponImportDto importDto, string language)
        {
            var dto = new CreateUpdateWeaponDto
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
                Notes = importDto.Notes,
                YearOfManufacture = importDto.YearOfManufacture,
                Model = importDto.Model
            };

            dto.CaliberId = FindLookupIdCached("Calibers", importDto.Caliber);
            dto.CaliberUnitId = FindLookupIdCached("Units", importDto.CaliberUnit);
            dto.ClassificationId = FindLookupIdCached("Classifications", importDto.Classification);
            dto.TypeId = FindLookupIdCached("ItemTypes", importDto.Type);
            dto.CaliberCategory = WeaponCaliberCategory.Small;

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

        private async Task<List<string>> ValidateDtoAsync(CreateUpdateWeaponDto dto)
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
                // Arabic
                 { "الاسم*", nameof(WeaponImportDto.Name) },
                { "رقم الصنف*", nameof(WeaponImportDto.ItemNo) },
                { "رقم القطعة", nameof(WeaponImportDto.PartNo) },
                { "رقم الجزء", nameof(WeaponImportDto.PartNo) }, // backward compatible
                { "رقم NSN", nameof(WeaponImportDto.Nsn) },
                { "السعر", nameof(WeaponImportDto.Price) },
                { "الكمية الدنيا", nameof(WeaponImportDto.MinimumQuantity) },
                { "العيار", nameof(WeaponImportDto.Caliber) },
                { "وحدة العيار", nameof(WeaponImportDto.CaliberUnit) },
                { "سنة الصنع", nameof(WeaponImportDto.YearOfManufacture) },
                { "سنة التصنيع", nameof(WeaponImportDto.YearOfManufacture) }, // backward compatible
                { "بلد الصنع", nameof(WeaponImportDto.CountryOfManufacture) },
                { "بلد التصنيع", nameof(WeaponImportDto.CountryOfManufacture) }, // backward compatible
                { "بلد المنشأ", nameof(WeaponImportDto.CountryOfManufacture) }, // backward compatible
                { "النموذج", nameof(WeaponImportDto.Model) },
                { "رقم الأمم المتحدة", nameof(WeaponImportDto.UNNumber) },
                { "التوزيع", nameof(WeaponImportDto.Distribution) },
                { "الرقم المرجعي", nameof(WeaponImportDto.ReferenceNo) },
                { "التصنيف", nameof(WeaponImportDto.Classification) },
                { "النوع", nameof(WeaponImportDto.Type) },
                { "ملاحظات", nameof(WeaponImportDto.Notes) }
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
