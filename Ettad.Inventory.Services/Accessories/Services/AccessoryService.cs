using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Accessories.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Comman.Models;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Inventory.Service.ItemDepartmentAssignments.Interfaces;
using Ettad.Inventory.Service.Accessories.Interfaces;
using Ettad.Inventory.Service.Common.Services;

namespace Ettad.Inventory.Service.Accessories.Services
{
    public class AccessoryService : IAccessoryService
    {
        private static readonly string[] AccessoryTemplateSampleIncludes =
        {
            nameof(Accessory.Classification),
            nameof(Accessory.Type),
            $"{nameof(Accessory.BaseItemPrimaryPurposes)}.{nameof(BaseItemPrimaryPurpos.PrimaryPurpos)}"
        };

        private readonly ICrossCuttingRepository<Accessory> _accessoryRepository;
        private readonly ICrossCuttingRepository<BaseItemPrimaryPurpos> _baseItemPrimaryPurposRepository;
        private readonly ICrossCuttingRepository<ItemDepartmentAssignment> _itemDepartmentAssignmentRepository;
        private readonly ICrossCuttingRepository<InventoryDetail> _inventoryDetailRepository;
        private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;
        private readonly ICrossCuttingRepository<SupplyDetail> _supplyDetailRepository;
        private readonly ICrossCuttingRepository<AllowanceItem> _allowanceItemRepository;
        private readonly ICrossCuttingRepository<AssetSupplyDetail> _assetSupplyDetailRepository;
        private readonly ICrossCuttingRepository<Asset> _assetRepository;
        private readonly ICrossCuttingRepository<WeaponAccessory> _weaponAccessoryRepository;
        private readonly ICrossCuttingRepository<Classification> _classificationRepository;
        private readonly ICrossCuttingRepository<ItemTypeLookup> _itemTypeLookupRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateAccessoryDto> _validator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AccessoryService> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly ITransactionManager _transactionManager;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IInventoryPermanentDeleteExecutor _inventoryPermanentDeleteExecutor;
        private readonly AssetImportManager<CreateUpdateAccessoryDto, AccessoryImportDto> _importManager;
        private readonly IItemDepartmentAssignmentService _itemDepartmentAssignmentService;

        private List<Classification> _classifications;
        private List<ItemTypeLookup> _itemTypes;
        private Dictionary<string, Dictionary<string, long>> _cachedLookups = new();
        private HashSet<string> _existingItemNos = new(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _existingNsns = new(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _newlyAddedItemNos = new(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _newlyAddedNsns = new(StringComparer.OrdinalIgnoreCase);

        public AccessoryService(
            ICrossCuttingRepository<Accessory> accessoryRepository,
            ICrossCuttingRepository<BaseItemPrimaryPurpos> baseItemPrimaryPurposRepository,
            ICrossCuttingRepository<ItemDepartmentAssignment> itemDepartmentAssignmentRepository,
            ICrossCuttingRepository<InventoryDetail> inventoryDetailRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            ICrossCuttingRepository<SupplyDetail> supplyDetailRepository,
            ICrossCuttingRepository<AllowanceItem> allowanceItemRepository,
            ICrossCuttingRepository<AssetSupplyDetail> assetSupplyDetailRepository,
            ICrossCuttingRepository<Asset> assetRepository,
            ICrossCuttingRepository<WeaponAccessory> weaponAccessoryRepository,
            ICrossCuttingRepository<Classification> classificationRepository,
            ICrossCuttingRepository<ItemTypeLookup> itemTypeLookupRepository,
            IMapper mapper,
            IValidator<CreateUpdateAccessoryDto> validator,
            ICurrentUserService currentUserService,
            ILogger<AccessoryService> logger,
            IFileUploadService fileUploadService,
            IExcelImportService excelImportService,
            ITransactionManager transactionManager,
            IDateTimeProvider dateTimeProvider,
            IInventoryPermanentDeleteExecutor inventoryPermanentDeleteExecutor,
            IItemDepartmentAssignmentService itemDepartmentAssignmentService)
        {
            _accessoryRepository = accessoryRepository;
            _baseItemPrimaryPurposRepository = baseItemPrimaryPurposRepository;
            _itemDepartmentAssignmentRepository = itemDepartmentAssignmentRepository;
            _inventoryDetailRepository = inventoryDetailRepository;
            _requestItemRepository = requestItemRepository;
            _supplyDetailRepository = supplyDetailRepository;
            _allowanceItemRepository = allowanceItemRepository;
            _assetSupplyDetailRepository = assetSupplyDetailRepository;
            _assetRepository = assetRepository;
            _weaponAccessoryRepository = weaponAccessoryRepository;
            _classificationRepository = classificationRepository;
            _itemTypeLookupRepository = itemTypeLookupRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
            _logger = logger;
            _fileUploadService = fileUploadService;
            _transactionManager = transactionManager;
            _dateTimeProvider = dateTimeProvider;
            _inventoryPermanentDeleteExecutor = inventoryPermanentDeleteExecutor;
            _itemDepartmentAssignmentService = itemDepartmentAssignmentService;

            _importManager = new AssetImportManager<CreateUpdateAccessoryDto, AccessoryImportDto>(excelImportService,
               new LoggerFactory().CreateLogger<AssetImportManager<CreateUpdateAccessoryDto, AccessoryImportDto>>());
        }

        public async Task<APIOperationResponse<AccessoryDto>> GetByIdAsync(long id, bool includeDeleted = false)
        {
            try
            {
                var assignedItemIds = await GetAssignedItemIdsAsync();
                if (assignedItemIds != null && !assignedItemIds.Contains(id))
                {
                    return APIOperationResponse<AccessoryDto>.Fail(ResponseType.NotFound, "Accessory not found");
                }

                var accessory = await _accessoryRepository.FindOneAsync(
                    a => a.Id == id && (includeDeleted || !a.IsDeleted),
                    includeDeleted,
                    nameof(Accessory.Classification),
                    nameof(Accessory.Type),
                    $"{nameof(Accessory.BaseItemPrimaryPurposes)}.{nameof(BaseItemPrimaryPurpos.PrimaryPurpos)}"
                );

                if (accessory == null)
                    return APIOperationResponse<AccessoryDto>.Fail(ResponseType.NotFound, "Accessory not found");

                var dto = _mapper.Map<AccessoryDto>(accessory);

                var imagesResult = await _fileUploadService.GetByEntityAsync(FileEntityType.Accessory, accessory.Id);
                dto.Images = imagesResult.Succeeded && imagesResult.Data != null ? imagesResult.Data : new List<FileUploadDto>();

                return APIOperationResponse<AccessoryDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<AccessoryDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<AccessoryDto>>> GetAllAsync()
        {
            try
            {
                var assignedItemIds = await GetAssignedItemIdsAsync();

                var accessories = await _accessoryRepository.FindAsync(
                    a => !a.IsDeleted && (assignedItemIds == null || assignedItemIds.Contains(a.Id)),
                    false,
                    nameof(Accessory.Classification),
                    nameof(Accessory.Type),
                    $"{nameof(Accessory.BaseItemPrimaryPurposes)}.{nameof(BaseItemPrimaryPurpos.PrimaryPurpos)}"
                );

                var dtos = _mapper.Map<List<AccessoryDto>>(accessories);

                var entityIds = dtos.Select(d => d.Id).ToList();
                var imagesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Accessory, entityIds);
                if (imagesResult.Succeeded && imagesResult.Data != null)
                {
                    foreach (var dto in dtos)
                    {
                        dto.Images = imagesResult.Data.ContainsKey(dto.Id) ? imagesResult.Data[dto.Id] : new List<FileUploadDto>();
                    }
                }

                return APIOperationResponse<List<AccessoryDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<AccessoryDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<PaginatedList<AccessoryDto>>> GetAllPaginatedAsync(PagedListRequest request)
        {
            try
            {
                var assignedItemIds = await GetAssignedItemIdsAsync();
                var showDeletedOnly = request.DeletedOnly == true;

                var query = _accessoryRepository.Find(
                    a => (showDeletedOnly ? a.IsDeleted : !a.IsDeleted) && (assignedItemIds == null || assignedItemIds.Contains(a.Id)),
                    showDeletedOnly,
                    nameof(Accessory.Classification),
                    nameof(Accessory.Type),
                    $"{nameof(Accessory.BaseItemPrimaryPurposes)}.{nameof(BaseItemPrimaryPurpos.PrimaryPurpos)}"
                )
                    .OrderBy(a => a.Id);

                var paginatedEntities = await PaginatedList<Accessory>.CreateAsyncForTableBinding(query, request);

                var dtos = new List<AccessoryDto>();
                if (paginatedEntities.Items.Any())
                {
                    dtos = _mapper.Map<List<AccessoryDto>>(paginatedEntities.Items);

                    var entityIds = dtos.Select(d => d.Id).ToList();
                    var imagesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Accessory, entityIds);

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

                var result = new PaginatedList<AccessoryDto>(
                    dtos,
                    paginatedEntities.TotalCount,
                    paginatedEntities.PageIndex,
                    request.PageSize
                );

                return APIOperationResponse<PaginatedList<AccessoryDto>>.Success(result);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<PaginatedList<AccessoryDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateUpdateAccessoryDto inputDto, List<IFormFile>? files = null)
        {
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
                    var existingWithSameNsn = await _accessoryRepository.FindOneAsync(
                        a => !a.IsDeleted && a.Nsn == inputDto.Nsn.Trim());

                    if (existingWithSameNsn != null)
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "NSN already exists");
                }

                if (files != null && files.Any())
                {
                    var saveFilesResult = await _fileUploadService.SaveFilesAsync(files, FileEntityType.Accessory);

                    if (!saveFilesResult.Succeeded)
                    {
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                            $"File upload failed: {saveFilesResult.Message}");
                    }
                }

                var accessory = _mapper.Map<Accessory>(inputDto);
                accessory.ItemType = ItemType.Accessory;
                accessory.CreationDate = _dateTimeProvider.Now;
                accessory.CreatedBy = _currentUserService.UserId;
                accessory.ItemNo = string.IsNullOrWhiteSpace(inputDto.ItemNo) ? null : inputDto.ItemNo.Trim();
                accessory.Nsn = string.IsNullOrWhiteSpace(inputDto.Nsn) ? null : inputDto.Nsn.Trim();

                var createdAccessory = await _accessoryRepository.AddAsync(accessory);

                if (inputDto.PrimaryPurposIds != null && inputDto.PrimaryPurposIds.Any())
                {
                    var purposeRows = inputDto.PrimaryPurposIds
                        .Select(pid => new BaseItemPrimaryPurpos { BaseItemId = createdAccessory.Id, PrimaryPurposId = pid })
                        .ToList();
                    await _baseItemPrimaryPurposRepository.AddRangeAsync(purposeRows);
                }

                if (files != null && files.Any())
                {
                    await _fileUploadService.UploadFilesForEntityAsync(
                        files,
                        FileEntityType.Accessory,
                        createdAccessory.Id);
                }

                return APIOperationResponse<long>.Success(createdAccessory.Id, "Accessory created successfully");
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null) msg += $" (Inner: {ex.InnerException.Message})";
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {msg}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateAccessoryDto inputDto)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                var existingAccessory = await _accessoryRepository.FindOneAsync(a => a.Id == id && !a.IsDeleted);
                if (existingAccessory == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Accessory not found");

                if (!string.IsNullOrWhiteSpace(inputDto.Nsn))
                {
                    var duplicate = await _accessoryRepository.FindOneAsync(
                        a => !a.IsDeleted && a.Id != id && a.Nsn == inputDto.Nsn.Trim());

                    if (duplicate != null)
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "NSN already exists");
                }

                _mapper.Map(inputDto, existingAccessory);
                existingAccessory.ModificationDate = _dateTimeProvider.Now;
                existingAccessory.ModifiedBy = _currentUserService.UserId;
                existingAccessory.ItemNo = string.IsNullOrWhiteSpace(inputDto.ItemNo) ? null : inputDto.ItemNo.Trim();
                existingAccessory.Nsn = string.IsNullOrWhiteSpace(inputDto.Nsn) ? null : inputDto.Nsn.Trim();

                await _accessoryRepository.UpdateAsync(existingAccessory);

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

                return APIOperationResponse<bool>.Success(true, "Accessory updated successfully");
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
                var accessory = await _accessoryRepository.FindOneAsync(a => a.Id == id && !a.IsDeleted);
                if (accessory == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Accessory not found");

                var hasRefs = await ItemHasReferencesAsync(id);
                if (hasRefs)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Cannot delete this accessory because it is referenced by other records (e.g. inventory, department assignments, supply requests, allowances, or weapon links). Please remove those references first.");
                }

                await _accessoryRepository.DeleteAsync(accessory);

                return APIOperationResponse<bool>.Success(true, "Accessory deleted successfully");
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
                var accessory = await _accessoryRepository.FindOneAsync(a => a.Id == id, includeSoftDeleted: true);
                if (accessory == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Accessory not found");
                }

                if (!accessory.IsDeleted)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Accessory is not deleted");
                }

                accessory.IsDeleted = false;
                accessory.DeletionDate = null;
                accessory.DeletedBy = null;

                await _accessoryRepository.UpdateAsync(accessory);

                return APIOperationResponse<bool>.Success(true, "Accessory restored successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> PermanentDeleteAsync(long id)
        {
            try
            {
                if (!_currentUserService.IsSuperAdmin)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.Forbidden,
                        "Only a super administrator can permanently delete accessories.");
                }

                var accessory = await _accessoryRepository.FindOneAsync(a => a.Id == id, includeSoftDeleted: true);
                if (accessory == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Accessory not found");
                }

                if (!accessory.IsDeleted)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Only soft-deleted accessories can be permanently deleted");
                }

                var hasWeaponLinks = await _weaponAccessoryRepository
                    .Find(x => x.AccessoryId == id)
                    .AnyAsync();
                if (hasWeaponLinks)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Cannot permanently delete this accessory because it is linked to one or more weapons. Remove those links first.");
                }

                await using var transaction = await _transactionManager.BeginAsync();
                try
                {
                    var (accessoryRows, baseRows) = await _inventoryPermanentDeleteExecutor.ExecuteAsync(
                        InventoryPermanentDeleteKind.Accessory,
                        id);

                    if (accessoryRows == 0 || baseRows == 0)
                    {
                        await _transactionManager.RollbackAsync();
                        return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError,
                            "Failed to permanently delete accessory rows from the database.");
                    }

                    await _transactionManager.CommitAsync();
                    return APIOperationResponse<bool>.Success(true, "Accessory permanently deleted");
                }
                catch (Exception)
                {
                    await _transactionManager.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                if (IsForeignKeyViolation(ex))
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Cannot permanently delete this accessory because it is referenced by other records. Please remove those references first.");
                }
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<ImportResult<CreateUpdateAccessoryDto>>> ImportAsync(IFormFile file, string language = "en")
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

        public async Task<APIOperationResponse<ImportResult<AccessoryImportDto>>> ImportPreviewAsync(IFormFile file, string language = "en")
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
                    "الاسم*", "الاسم (بالعربية)", "رقم الصنف", "رقم القطعة", "NSN", "السعر", "الكمية الدنيا",
                    "رقم الأمم المتحدة", "التوزيع", "الرقم المرجعي", "التصنيف", "النوع", "ملاحظات"
                }
                : new[]
                {
                    "Name*", "Name (Arabic)", "Item No", "Part No", "NSN", "Price", "Minimum Quantity",
                    "UN Number", "Distribution", "Reference No", "Classification", "Type", "Notes"
                };

            var firstAsset = await _accessoryRepository.FindOneAsync(
                a => !a.IsDeleted,
                false,
                AccessoryTemplateSampleIncludes);

            return await _importManager.GenerateTemplateAsync(
                language,
                "Accessory Import",
                headers,
                (sheet) =>
                {
                    if (firstAsset != null)
                    {
                        var isAr = language == "ar";
                        sheet.Cells[2, 1].Value = firstAsset.Name;
                        sheet.Cells[2, 2].Value = firstAsset.NameAr;
                        sheet.Cells[2, 3].Value = firstAsset.ItemNo;
                        sheet.Cells[2, 4].Value = firstAsset.PartNo;
                        sheet.Cells[2, 5].Value = firstAsset.Nsn;
                        sheet.Cells[2, 6].Value = firstAsset.Price;
                        sheet.Cells[2, 7].Value = firstAsset.MinimumQuantity;
                        sheet.Cells[2, 8].Value = firstAsset.UNNumber;
                        sheet.Cells[2, 9].Value = firstAsset.Distribution;
                        sheet.Cells[2, 10].Value = firstAsset.ReferenceNo;
                        sheet.Cells[2, 11].Value = isAr ? firstAsset.Classification?.NameAr : firstAsset.Classification?.NameEn;
                        sheet.Cells[2, 12].Value = isAr ? firstAsset.Type?.NameAr : firstAsset.Type?.NameEn;
                        sheet.Cells[2, 13].Value = firstAsset.Notes;
                    }
                    else
                    {
                        sheet.Cells[2, 1].Value = "Sample Accessory";
                        sheet.Cells[2, 2].Value = "ملحق تجريبي";
                        sheet.Cells[2, 3].Value = "ACC-001";
                    }
                },
                (package) =>
                {
                    CreateLookupSheet(package, "Classifications", _classifications);
                    CreateLookupSheet(package, "ItemTypes", _itemTypes);
                },
                (sheet) =>
                {
                    AddDataValidation(sheet, 11, "Classifications");
                    AddDataValidation(sheet, 12, "ItemTypes");
                }
            );
        }

        private async Task<bool> ItemHasReferencesAsync(long itemId)
        {
            var hasWeaponLink = await _weaponAccessoryRepository
                .Find(x => x.AccessoryId == itemId)
                .AnyAsync();
            if (hasWeaponLink) return true;

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

        private async Task LoadLookupsAsync(List<AccessoryImportDto> importItems = null)
        {
            _classifications = await _classificationRepository.Find(c => !c.IsDeleted).ToListAsync();
            _itemTypes = await _itemTypeLookupRepository.Find(i => !i.IsDeleted && i.ItemType == ItemType.Accessory).ToListAsync();

            _cachedLookups["Classifications"] = BuildLookup(_classifications, x => x.NameEn, x => x.NameAr, x => x.Id);
            _cachedLookups["ItemTypes"] = BuildLookup(_itemTypes, x => x.NameEn, x => x.NameAr, x => x.Id);

            _existingItemNos.Clear();
            _existingNsns.Clear();
            _newlyAddedItemNos.Clear();
            _newlyAddedNsns.Clear();

            if (importItems != null && importItems.Any())
            {
                var itemNos = importItems.Select(x => x.ItemNo).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();
                var nsns = importItems.Select(x => x.Nsn).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();

                var existingRecords = await _accessoryRepository
                    .Find(a => !a.IsDeleted && (itemNos.Contains(a.ItemNo) || (a.Nsn != null && nsns.Contains(a.Nsn))))
                    .Select(a => new { a.ItemNo, a.Nsn })
                    .ToListAsync();

                foreach (var rec in existingRecords)
                {
                    if (!string.IsNullOrEmpty(rec.ItemNo)) _existingItemNos.Add(rec.ItemNo);
                    if (!string.IsNullOrEmpty(rec.Nsn)) _existingNsns.Add(rec.Nsn);
                }
            }
        }

        private Task<CreateUpdateAccessoryDto> MapImportDtoToEntityAsync(AccessoryImportDto importDto, string language)
        {
            var dto = new CreateUpdateAccessoryDto
            {
                Name = importDto.Name,
                NameAr = importDto.NameAr,
                ItemNo = string.IsNullOrWhiteSpace(importDto.ItemNo) ? null : importDto.ItemNo.Trim(),
                PartNo = importDto.PartNo,
                Price = importDto.Price,
                MinimumQuantity = importDto.MinimumQuantity,
                Nsn = importDto.Nsn,
                Distribution = importDto.Distribution,
                ReferenceNo = importDto.ReferenceNo,
                UNNumber = importDto.UNNumber,
                Notes = importDto.Notes
            };

            dto.ClassificationId = FindLookupIdCached("Classifications", importDto.Classification);
            dto.TypeId = FindLookupIdCached("ItemTypes", importDto.Type);

            return Task.FromResult(dto);
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

        private async Task<List<string>> ValidateDtoAsync(CreateUpdateAccessoryDto dto)
        {
            var errors = new List<string>();
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                errors.AddRange(validationResult.Errors.Select(e => e.ErrorMessage));
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
                { "Name*", nameof(AccessoryImportDto.Name) },
                { "Name (Arabic)", nameof(AccessoryImportDto.NameAr) },
                { "Name Arabic", nameof(AccessoryImportDto.NameAr) },
                { "Item No*", nameof(AccessoryImportDto.ItemNo) },
                { "Part No", nameof(AccessoryImportDto.PartNo) },
                { "NSN", nameof(AccessoryImportDto.Nsn) },
                { "Price", nameof(AccessoryImportDto.Price) },
                { "Minimum Quantity", nameof(AccessoryImportDto.MinimumQuantity) },
                { "UN Number", nameof(AccessoryImportDto.UNNumber) },
                { "Distribution", nameof(AccessoryImportDto.Distribution) },
                { "Reference No", nameof(AccessoryImportDto.ReferenceNo) },
                { "Classification", nameof(AccessoryImportDto.Classification) },
                { "Type", nameof(AccessoryImportDto.Type) },
                { "Notes", nameof(AccessoryImportDto.Notes) },
                { "الاسم*", nameof(AccessoryImportDto.Name) },
                { "الاسم (بالعربية)", nameof(AccessoryImportDto.NameAr) },
                { "رقم الصنف*", nameof(AccessoryImportDto.ItemNo) },
                { "رقم القطعة", nameof(AccessoryImportDto.PartNo) },
                { "رقم NSN", nameof(AccessoryImportDto.Nsn) },
                { "السعر", nameof(AccessoryImportDto.Price) },
                { "الكمية الدنيا", nameof(AccessoryImportDto.MinimumQuantity) },
                { "رقم الأمم المتحدة", nameof(AccessoryImportDto.UNNumber) },
                { "التوزيع", nameof(AccessoryImportDto.Distribution) },
                { "الرقم المرجعي", nameof(AccessoryImportDto.ReferenceNo) },
                { "التصنيف", nameof(AccessoryImportDto.Classification) },
                { "النوع", nameof(AccessoryImportDto.Type) },
                { "ملاحظات", nameof(AccessoryImportDto.Notes) }
            };
        }

        private void CreateLookupSheet<T>(ExcelPackage package, string sheetName, List<T> items)
        {
            var names = items.Select(x =>
            {
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

        private async Task<HashSet<long>?> GetAssignedItemIdsAsync()
        {
            if (!_currentUserService.DepartmentId.HasValue)
            {
                return null;
            }

            var assignmentsResult = await _itemDepartmentAssignmentService.GetByDepartmentIdAsync(_currentUserService.DepartmentId.Value);

            if (!assignmentsResult.Succeeded || assignmentsResult.Data == null || !assignmentsResult.Data.Any())
            {
                return null;
            }

            return new HashSet<long>(assignmentsResult.Data.Select(a => a.ItemId));
        }
    }
}
