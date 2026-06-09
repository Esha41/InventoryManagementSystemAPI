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

        private HashSet<string> _existingItemNos = new(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _newlyAddedItemNos = new(StringComparer.OrdinalIgnoreCase);

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

        private static readonly string[] AccessoryImportPreviewHeaders = { "name", "nameAr", "itemNo" };

        public async Task<APIOperationResponse<ImportResult<CreateUpdateAccessoryDto>>> ImportAsync(IFormFile file, string language = "en")
        {
            var result = await _importManager.ImportAsync(
                file,
                language,
                items => LoadLookupsAsync(items),
                MapImportDtoToEntityAsync,
                ValidateDtoAsync,
                async (dto) => await CreateAsync(dto),
                GetColumnMappings(language));

            if (result.Succeeded && result.Data != null)
            {
                result.Data.ImportHeaders = AccessoryImportPreviewHeaders.ToList();
            }

            return result;
        }

        public async Task<APIOperationResponse<ImportResult<AccessoryImportDto>>> ImportPreviewAsync(IFormFile file, string language = "en")
        {
            var result = await _importManager.ImportPreviewAsync(
               file,
               language,
               items => LoadLookupsAsync(items),
               MapImportDtoToEntityAsync,
               ValidateDtoAsync,
               GetColumnMappings(language));

            if (result.Succeeded && result.Data != null)
            {
                result.Data.ImportHeaders = AccessoryImportPreviewHeaders.ToList();
            }

            return result;
        }

        public async Task<APIOperationResponse<byte[]>> GenerateImportTemplateAsync(string language = "en")
        {
            await LoadLookupsAsync();

            var headers = language == "ar"
                ? new[] { "الاسم (بالإنجليزية)*", "الاسم (بالعربية)", "رقم المادة" }
                : new[] { "Name (English)*", "Name (Arabic)", "Item No" };

            var firstAsset = await _accessoryRepository.FindOneAsync(a => !a.IsDeleted);

            return await _importManager.GenerateTemplateAsync(
                language,
                "Accessory Import",
                headers,
                (sheet) =>
                {
                    if (firstAsset != null)
                    {
                        sheet.Cells[2, 1].Value = firstAsset.Name;
                        sheet.Cells[2, 2].Value = firstAsset.NameAr;
                        sheet.Cells[2, 3].Value = firstAsset.ItemNo;
                    }
                    else
                    {
                        sheet.Cells[2, 1].Value = "Rifle Scope";
                        sheet.Cells[2, 2].Value = "منظار بندقية";
                        sheet.Cells[2, 3].Value = "ACC-001";
                    }
                },
                null,
                null
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
            _existingItemNos.Clear();
            _newlyAddedItemNos.Clear();

            if (importItems != null && importItems.Any())
            {
                var itemNos = importItems.Select(x => x.ItemNo).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();

                var existingRecords = await _accessoryRepository
                    .Find(a => !a.IsDeleted && itemNos.Contains(a.ItemNo))
                    .Select(a => a.ItemNo)
                    .ToListAsync();

                foreach (var itemNo in existingRecords)
                {
                    if (!string.IsNullOrEmpty(itemNo)) _existingItemNos.Add(itemNo);
                }
            }
        }

        private Task<CreateUpdateAccessoryDto> MapImportDtoToEntityAsync(AccessoryImportDto importDto, string language)
        {
            return Task.FromResult(new CreateUpdateAccessoryDto
            {
                Name = importDto.Name,
                NameAr = importDto.NameAr,
                ItemNo = importDto.ItemNo
            });
        }

        private async Task<List<string>> ValidateDtoAsync(CreateUpdateAccessoryDto dto)
        {
            var errors = new List<string>();
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                errors.AddRange(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            if (!string.IsNullOrWhiteSpace(dto.ItemNo))
            {
                if (_existingItemNos.Contains(dto.ItemNo))
                    errors.Add($"Item No '{dto.ItemNo}' already exists in the database");
                else if (_newlyAddedItemNos.Contains(dto.ItemNo))
                    errors.Add($"Item No '{dto.ItemNo}' is duplicated in the current file");
                else
                    _newlyAddedItemNos.Add(dto.ItemNo);
            }

            return errors;
        }

        private Dictionary<string, string> GetColumnMappings(string language)
        {
            return new Dictionary<string, string>
            {
                { "Name (English)*", nameof(AccessoryImportDto.Name) },
                { "Name (English)", nameof(AccessoryImportDto.Name) },
                { "Name*", nameof(AccessoryImportDto.Name) },
                { "Name", nameof(AccessoryImportDto.Name) },
                { "Name (Arabic)", nameof(AccessoryImportDto.NameAr) },
                { "Name Arabic", nameof(AccessoryImportDto.NameAr) },
                { "Item No*", nameof(AccessoryImportDto.ItemNo) },
                { "Item No", nameof(AccessoryImportDto.ItemNo) },
                { "الاسم (بالإنجليزية)*", nameof(AccessoryImportDto.Name) },
                { "الاسم (بالإنجليزية)", nameof(AccessoryImportDto.Name) },
                { "الاسم*", nameof(AccessoryImportDto.Name) },
                { "الاسم (بالعربية)", nameof(AccessoryImportDto.NameAr) },
                { "رقم المادة*", nameof(AccessoryImportDto.ItemNo) },
                { "رقم المادة", nameof(AccessoryImportDto.ItemNo) },
                { "رقم الصنف*", nameof(AccessoryImportDto.ItemNo) },
                { "رقم الصنف", nameof(AccessoryImportDto.ItemNo) }
            };
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
