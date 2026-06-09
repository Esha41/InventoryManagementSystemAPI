using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Ammunitions.Dtos;
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
using Ettad.Inventory.Service.Ammunitions.Interfaces;
using Ettad.Inventory.Service.Common.Services;

namespace Ettad.Inventory.Service.Ammunitions.Services
{
    public class AmmunitionService : IAmmunitionService
    {
        private static readonly string[] AmmunitionTemplateSampleIncludes =
        {
            nameof(Ammunition.LookupCaliber),
            nameof(Ammunition.BulletDiameterUnit),
            nameof(Ammunition.CaseType),
            nameof(Ammunition.Propellant),
            nameof(Ammunition.Compatibility),
            nameof(Ammunition.HazardDivision),
            nameof(Ammunition.NatureOption),
            "BaseItemPrimaryPurposes.PrimaryPurpos",
            nameof(Ammunition.ProjectileColor),
            nameof(Ammunition.ProjectailMaterial),
            nameof(Ammunition.Classification),
            nameof(Ammunition.Type),
        };

        private readonly ICrossCuttingRepository<Ammunition> _ammunitionRepository;
        private readonly ICrossCuttingRepository<FileUplodDetails> _fileDetailsRepository;
        private readonly ICrossCuttingRepository<BaseItemPrimaryPurpos> _baseItemPrimaryPurposRepository;
        private readonly ICrossCuttingRepository<ItemDepartmentAssignment> _itemDepartmentAssignmentRepository;
        private readonly ICrossCuttingRepository<InventoryDetail> _inventoryDetailRepository;
        private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;
        private readonly ICrossCuttingRepository<SupplyDetail> _supplyDetailRepository;
        private readonly ICrossCuttingRepository<AllowanceItem> _allowanceItemRepository;
        private readonly ICrossCuttingRepository<AssetSupplyDetail> _assetSupplyDetailRepository;
        private readonly ICrossCuttingRepository<Asset> _assetRepository;
        private readonly ICrossCuttingRepository<Unit> _unitRepository;
        private readonly ICrossCuttingRepository<CaseType> _caseTypeRepository;
        private readonly ICrossCuttingRepository<Propellant> _propellantRepository;
        private readonly ICrossCuttingRepository<Compatibility> _compatibilityRepository;
        private readonly ICrossCuttingRepository<HazardDivision> _hazardDivisionRepository;
        private readonly ICrossCuttingRepository<NatureOption> _natureOptionRepository;
        private readonly ICrossCuttingRepository<PrimaryPurpos> _primaryPurposRepository;
        private readonly ICrossCuttingRepository<Color> _colorRepository;
        private readonly ICrossCuttingRepository<ProjectailMaterial> _projectailMaterialRepository;
        private readonly ICrossCuttingRepository<Classification> _classificationRepository;
        private readonly ICrossCuttingRepository<ItemTypeLookup> _itemTypeLookupRepository;
        private readonly ICrossCuttingRepository<Caliber> _caliberRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateAmmunitionDto> _validator;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<AmmunitionService> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly ITransactionManager _transactionManager;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IInventoryPermanentDeleteExecutor _inventoryPermanentDeleteExecutor;
        private readonly AssetImportManager<CreateUpdateAmmunitionDto, AmmunitionImportDto> _importManager;
        private readonly IItemDepartmentAssignmentService _itemDepartmentAssignmentService;

        // In-memory cache for lookups during import
        private List<Unit> _units;
        private List<CaseType> _caseTypes;
        private List<Propellant> _propellants;
        private List<Compatibility> _compatibilities;
        private List<HazardDivision> _hazardDivisions;
        private List<NatureOption> _natureOptions;
        private List<PrimaryPurpos> _primaryPurposes;
        private List<Color> _projectileColors;
        private List<ProjectailMaterial> _projectileMaterials;
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

        public AmmunitionService(
            ICrossCuttingRepository<Ammunition> ammunitionRepository,
            ICrossCuttingRepository<FileUplodDetails> fileDetailsRepository,
            ICrossCuttingRepository<BaseItemPrimaryPurpos> baseItemPrimaryPurposRepository,
            ICrossCuttingRepository<ItemDepartmentAssignment> itemDepartmentAssignmentRepository,
            ICrossCuttingRepository<InventoryDetail> inventoryDetailRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            ICrossCuttingRepository<SupplyDetail> supplyDetailRepository,
            ICrossCuttingRepository<AllowanceItem> allowanceItemRepository,
            ICrossCuttingRepository<AssetSupplyDetail> assetSupplyDetailRepository,
            ICrossCuttingRepository<Asset> assetRepository,
            ICrossCuttingRepository<Unit> unitRepository,
            ICrossCuttingRepository<CaseType> caseTypeRepository,
            ICrossCuttingRepository<Propellant> propellantRepository,
            ICrossCuttingRepository<Compatibility> compatibilityRepository,
            ICrossCuttingRepository<HazardDivision> hazardDivisionRepository,
            ICrossCuttingRepository<NatureOption> natureOptionRepository,
            ICrossCuttingRepository<PrimaryPurpos> primaryPurposRepository,
            ICrossCuttingRepository<Color> colorRepository,
            ICrossCuttingRepository<ProjectailMaterial> projectailMaterialRepository,
            ICrossCuttingRepository<Classification> classificationRepository,
            ICrossCuttingRepository<ItemTypeLookup> itemTypeLookupRepository,
            ICrossCuttingRepository<Caliber> caliberRepository,
            IMapper mapper,
            IValidator<CreateUpdateAmmunitionDto> validator,
            ICurrentUserService currentUserService,
            ILogger<AmmunitionService> logger,
            IFileUploadService fileUploadService,
            IExcelImportService excelImportService,
            ITransactionManager transactionManager,
            IDateTimeProvider dateTimeProvider,
            IInventoryPermanentDeleteExecutor inventoryPermanentDeleteExecutor,
            IItemDepartmentAssignmentService itemDepartmentAssignmentService)
        {
            _ammunitionRepository = ammunitionRepository;
            _fileDetailsRepository = fileDetailsRepository;
            _baseItemPrimaryPurposRepository = baseItemPrimaryPurposRepository;
            _itemDepartmentAssignmentRepository = itemDepartmentAssignmentRepository;
            _inventoryDetailRepository = inventoryDetailRepository;
            _requestItemRepository = requestItemRepository;
            _supplyDetailRepository = supplyDetailRepository;
            _allowanceItemRepository = allowanceItemRepository;
            _assetSupplyDetailRepository = assetSupplyDetailRepository;
            _assetRepository = assetRepository;
            _unitRepository = unitRepository;
            _caseTypeRepository = caseTypeRepository;
            _propellantRepository = propellantRepository;
            _compatibilityRepository = compatibilityRepository;
            _hazardDivisionRepository = hazardDivisionRepository;
            _natureOptionRepository = natureOptionRepository;
            _primaryPurposRepository = primaryPurposRepository;
            _colorRepository = colorRepository;
            _projectailMaterialRepository = projectailMaterialRepository;
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
            _inventoryPermanentDeleteExecutor = inventoryPermanentDeleteExecutor;
            _itemDepartmentAssignmentService = itemDepartmentAssignmentService;

            _importManager = new AssetImportManager<CreateUpdateAmmunitionDto, AmmunitionImportDto>(excelImportService,
                new LoggerFactory().CreateLogger<AssetImportManager<CreateUpdateAmmunitionDto, AmmunitionImportDto>>());
        }

        public async Task<APIOperationResponse<AmmunitionDto>> GetByIdAsync(long id, bool includeDeleted = false)
        {
            // existing implementation
            _logger.LogInformation("Getting ammunition by ID. AmmunitionId: {AmmunitionId}, User: {UserId}",
               id, _currentUserService.UserId);

            try
            {
                // Check if user has department and assigned items
                var assignedItemIds = await GetAssignedItemIdsAsync();
                if (assignedItemIds != null && !assignedItemIds.Contains(id))
                {
                    return APIOperationResponse<AmmunitionDto>.Fail(ResponseType.NotFound, "Ammunition not found");
                }

                var ammunition = await _ammunitionRepository.FindOneAsync(
                    a => a.Id == id && (includeDeleted || !a.IsDeleted),
                    false,
                    nameof(Ammunition.BulletDiameterUnit),
                    nameof(Ammunition.LookupCaliber),
                    nameof(Ammunition.NatureOption),
                    "BaseItemPrimaryPurposes.PrimaryPurpos",
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

                var imagesResult = await _fileUploadService.GetByEntityAsync(FileEntityType.Ammunition, ammunition.Id);
                dto.Images = imagesResult.Succeeded && imagesResult.Data != null ? imagesResult.Data : new List<FileUploadDto>();

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
            // existing implementation
            _logger.LogInformation("Getting all ammunitions. User: {UserId}", _currentUserService.UserId);

            try
            {
                // Check if user has department and assigned items
                var assignedItemIds = await GetAssignedItemIdsAsync();
                
                var ammunitions = await _ammunitionRepository.FindAsync(
                    a => !a.IsDeleted && (assignedItemIds == null || assignedItemIds.Contains(a.Id)),
                    false,
                    nameof(Ammunition.BulletDiameterUnit),
                    nameof(Ammunition.LookupCaliber),
                    nameof(Ammunition.NatureOption),
                    "BaseItemPrimaryPurposes.PrimaryPurpos",
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

                var entityIds = dtos.Select(d => d.Id).ToList();
                var imagesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Ammunition, entityIds);
                if (imagesResult.Succeeded && imagesResult.Data != null)
                {
                    foreach (var dto in dtos)
                    {
                        dto.Images = imagesResult.Data.ContainsKey(dto.Id) ? imagesResult.Data[dto.Id] : new List<FileUploadDto>();
                    }
                }

                return APIOperationResponse<List<AmmunitionDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all ammunitions. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<AmmunitionDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<PaginatedList<AmmunitionDto>>> GetAllPaginatedAsync(PagedListRequest request)
        {
            // existing implementation
            _logger.LogInformation("Getting ammunitions paginated. Page: {Page}, PageSize: {PageSize}, User: {UserId}",
               request.Page, request.PageSize, _currentUserService.UserId);

            try
            {
                // Check if user has department and assigned items
                var assignedItemIds = await GetAssignedItemIdsAsync();
                var showDeletedOnly = request.DeletedOnly == true;
                
                var query = _ammunitionRepository.Find(
                    a => (showDeletedOnly ? a.IsDeleted : !a.IsDeleted) && (assignedItemIds == null || assignedItemIds.Contains(a.Id)),
                    false,
                    nameof(Ammunition.BulletDiameterUnit),
                    nameof(Ammunition.LookupCaliber),
                    nameof(Ammunition.NatureOption),
                    "BaseItemPrimaryPurposes.PrimaryPurpos",
                    nameof(Ammunition.ProjectileColor),
                    nameof(Ammunition.ProjectailMaterial),
                    nameof(Ammunition.CaseType),
                    nameof(Ammunition.Propellant),
                    nameof(Ammunition.Compatibility),
                    nameof(Ammunition.HazardDivision),
                    nameof(Ammunition.Classification),
                    nameof(Ammunition.Type)
                )
                    .OrderBy(a => a.Id);

                var paginatedEntities = await PaginatedList<Ammunition>.CreateAsyncForTableBinding(query, request);

                var dtos = new List<AmmunitionDto>();
                if (paginatedEntities.Items.Any())
                {
                    dtos = _mapper.Map<List<AmmunitionDto>>(paginatedEntities.Items);

                    var entityIds = dtos.Select(d => d.Id).ToList();
                    var imagesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Ammunition, entityIds);

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

                var result = new PaginatedList<AmmunitionDto>(
                    dtos,
                    paginatedEntities.TotalCount,
                    paginatedEntities.PageIndex,
                    request.PageSize
                );

                return APIOperationResponse<PaginatedList<AmmunitionDto>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting ammunitions paginated. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<PaginatedList<AmmunitionDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<AmmunitionDto>>> GetByTypeAsync(AmmunitionType ammunitionType)
        {
            // existing implementation
            _logger.LogInformation("Getting ammunitions by type. AmmunitionType: {AmmunitionType}, User: {UserId}", ammunitionType, _currentUserService.UserId);

            try
            {
                var ammunitions = await _ammunitionRepository.FindAsync(
                    a => !a.IsDeleted && a.AmmunitionType == ammunitionType,
                    false,
                    nameof(Ammunition.BulletDiameterUnit),
                    nameof(Ammunition.LookupCaliber),
                    nameof(Ammunition.NatureOption),
                    "BaseItemPrimaryPurposes.PrimaryPurpos",
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

                var entityIds = dtos.Select(d => d.Id).ToList();
                var imagesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.Ammunition, entityIds);
                if (imagesResult.Succeeded && imagesResult.Data != null)
                {
                    foreach (var dto in dtos)
                    {
                        dto.Images = imagesResult.Data.ContainsKey(dto.Id) ? imagesResult.Data[dto.Id] : new List<FileUploadDto>();
                    }
                }

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
            // existing implementation
            _logger.LogInformation("Creating new ammunition. Name: {Name}, ItemNo: {ItemNo}, User: {UserId}",
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
                    var existingWithSameNsn = await _ammunitionRepository.FindOneAsync(
                        a => !a.IsDeleted && a.Nsn == inputDto.Nsn.Trim());

                    if (existingWithSameNsn != null)
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "NSN already exists");
                }

                if (files != null && files.Any())
                {
                    var saveFilesResult = await _fileUploadService.SaveFilesAsync(files, FileEntityType.Ammunition);
                    if (!saveFilesResult.Succeeded)
                    {
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                            $"File upload failed: {saveFilesResult.Message}");
                    }
                }

                var ammunition = _mapper.Map<Ammunition>(inputDto);
                ammunition.AmmunitionType = inputDto.AmmunitionType ?? AmmunitionType.Small;
                ammunition.ItemType = ItemType.Ammunition;
                ammunition.CreationDate = _dateTimeProvider.Now;
                ammunition.CreatedBy = _currentUserService.UserId;
                ammunition.ItemNo = inputDto.ItemNo!.Trim();
                ammunition.Nsn = string.IsNullOrWhiteSpace(inputDto.Nsn) ? null : inputDto.Nsn.Trim();

                var createdAmmunition = await _ammunitionRepository.AddAsync(ammunition);

                if (inputDto.PrimaryPurposIds != null && inputDto.PrimaryPurposIds.Any())
                {
                    var purposeRows = inputDto.PrimaryPurposIds
                        .Select(pid => new BaseItemPrimaryPurpos { BaseItemId = createdAmmunition.Id, PrimaryPurposId = pid })
                        .ToList();
                    await _baseItemPrimaryPurposRepository.AddRangeAsync(purposeRows);
                }

                if (files != null && files.Any())
                {
                    await _fileUploadService.UploadFilesForEntityAsync(
                        files,
                        FileEntityType.Ammunition,
                        createdAmmunition.Id);
                }

                return APIOperationResponse<long>.Success(createdAmmunition.Id, "Ammunition created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ammunition");
                var msg = ex.Message + (ex.InnerException != null ? $" (Inner: {ex.InnerException.Message})" : "");
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {msg}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateAmmunitionDto inputDto)
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

                _mapper.Map(inputDto, existingAmmunition);
                existingAmmunition.AmmunitionType = inputDto.AmmunitionType!.Value;
                existingAmmunition.ModificationDate = _dateTimeProvider.Now;
                existingAmmunition.ModifiedBy = _currentUserService.UserId;
                existingAmmunition.ItemNo = inputDto.ItemNo!.Trim();
                existingAmmunition.Nsn = string.IsNullOrWhiteSpace(inputDto.Nsn) ? null : inputDto.Nsn.Trim();

                await _ammunitionRepository.UpdateAsync(existingAmmunition);

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

                return APIOperationResponse<bool>.Success(true, "Ammunition updated successfully");
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
                var ammunition = await _ammunitionRepository.FindOneAsync(a => a.Id == id && !a.IsDeleted);
                if (ammunition == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Ammunition not found");
                }

                var hasRefs = await ItemHasReferencesAsync(id);
                if (hasRefs)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "Cannot delete this ammunition because it is referenced by other records (e.g. inventory, department assignments, supply requests, or allowances). Please remove those references first.");
                }

                await _ammunitionRepository.DeleteAsync(ammunition);

                return APIOperationResponse<bool>.Success(true, "Ammunition deleted successfully");
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
                var ammunition = await _ammunitionRepository.FindOneAsync(a => a.Id == id, includeSoftDeleted: true);
                if (ammunition == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Ammunition not found");
                }

                if (!ammunition.IsDeleted)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Ammunition is not deleted");
                }

                ammunition.IsDeleted = false;
                ammunition.DeletionDate = null;
                ammunition.DeletedBy = null;

                await _ammunitionRepository.UpdateAsync(ammunition);

                return APIOperationResponse<bool>.Success(true, "Ammunition restored successfully");
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
                        "Only a super administrator can permanently delete ammunition.");
                }

                var ammunition = await _ammunitionRepository.FindOneAsync(a => a.Id == id, includeSoftDeleted: true);
                if (ammunition == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Ammunition not found");
                }

                if (!ammunition.IsDeleted)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Only soft-deleted ammunition can be permanently deleted");
                }

                await using var transaction = await _transactionManager.BeginAsync();
                try
                {
                    var (ammoRows, baseRows) = await _inventoryPermanentDeleteExecutor.ExecuteAsync(
                        InventoryPermanentDeleteKind.Ammunition,
                        id);

                    if (ammoRows == 0 || baseRows == 0)
                    {
                        await _transactionManager.RollbackAsync();
                        return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError,
                            "Failed to permanently delete ammunition rows from the database.");
                    }

                    await _transactionManager.CommitAsync();
                    return APIOperationResponse<bool>.Success(true, "Ammunition permanently deleted");
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
                        "Cannot permanently delete this ammunition because it is referenced by other records. Please remove those references first.");
                }
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Checks if the item (BaseItem/Ammunition) is referenced by inventory, department assignments, supply requests, allowances, or asset supplies.
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

        public async Task<APIOperationResponse<ImportResult<CreateUpdateAmmunitionDto>>> ImportAsync(IFormFile file, string language = "en")
        {
            return await _importManager.ImportAsync(
                file,
                language,
                items => LoadLookupsAsync(items),
                MapImportDtoToEntityAsync,
                ValidateDtoAsync,
                async (dto) => await CreateAsync(dto), // Delegate creation to existing CreateAsync
                GetColumnMappings(language));
        }

        public async Task<APIOperationResponse<ImportResult<AmmunitionImportDto>>> ImportPreviewAsync(IFormFile file, string language = "en")
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
                        "الاسم*", "الاسم (بالعربية)", "رقم الصنف*", "Part No", "رقم ARM", "NSN", "السعر", "الكمية الدنيا",
                        "قطر الرصاصة", "وحدة قطر الرصاصة", "الوزن الكلي", "مرتبط", "الكبسولة",
                        "نوع الغلاف", "المادة الدافعة", "التوافق", "قسم الخطر", "خيار الطبيعة",
                        "الغرض الأساسي", "لون المقذوف", "مادة المقذوف", "العيار",
                        "رقم الأمم المتحدة", "التوزيع", "الرقم المرجعي", "التصنيف", "النوع", "ملاحظات"
                   }
                   : new[]
                   {
                        "Name*", "Name (Arabic)", "Item No*", "Part No", "Arm Number", "NSN", "Price", "Minimum Quantity",
                        "Bullet Diameter", "Bullet Diameter Unit", "Total Weight", "Is Linked", "Primer",
                        "Case Type", "Propellant", "Compatibility", "Hazard Division", "Nature Option",
                        "Primary Purpose", "Projectile Color", "Projectile Material", "Caliber",
                        "UN Number", "Distribution", "Reference No", "Classification", "Type", "Notes"
                   };

            var firstAsset = await _ammunitionRepository.FindOneAsync(
                a => !a.IsDeleted,
                false,
                AmmunitionTemplateSampleIncludes);

            return await _importManager.GenerateTemplateAsync(
                language,
                "Ammunition Import",
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
                        sheet.Cells[2, 5].Value = firstAsset.ArmNumber;
                        sheet.Cells[2, 6].Value = firstAsset.Nsn;
                        sheet.Cells[2, 7].Value = firstAsset.Price;
                        sheet.Cells[2, 8].Value = firstAsset.MinimumQuantity;
                        sheet.Cells[2, 9].Value = firstAsset.BulletDiameter;
                        sheet.Cells[2, 10].Value = isAr ? firstAsset.BulletDiameterUnit?.NameAr : firstAsset.BulletDiameterUnit?.NameEn;
                        sheet.Cells[2, 11].Value = firstAsset.TotalWeight;
                        sheet.Cells[2, 12].Value = firstAsset.IsLinked ? "Yes" : "No";
                        sheet.Cells[2, 13].Value = firstAsset.Primer;
                        sheet.Cells[2, 14].Value = isAr ? firstAsset.CaseType?.NameAr : firstAsset.CaseType?.NameEn;
                        sheet.Cells[2, 15].Value = isAr ? firstAsset.Propellant?.NameAr : firstAsset.Propellant?.NameEn;
                        sheet.Cells[2, 16].Value = isAr ? firstAsset.Compatibility?.NameAr : firstAsset.Compatibility?.NameEn;
                        sheet.Cells[2, 17].Value = isAr ? firstAsset.HazardDivision?.NameAr : firstAsset.HazardDivision?.NameEn;
                        sheet.Cells[2, 18].Value = isAr ? firstAsset.NatureOption?.NameAr : firstAsset.NatureOption?.NameEn;
                        sheet.Cells[2, 19].Value = isAr
                            ? firstAsset.BaseItemPrimaryPurposes?.FirstOrDefault()?.PrimaryPurpos?.NameAr
                            : firstAsset.BaseItemPrimaryPurposes?.FirstOrDefault()?.PrimaryPurpos?.NameEn;
                        sheet.Cells[2, 20].Value = isAr ? firstAsset.ProjectileColor?.NameAr : firstAsset.ProjectileColor?.NameEn;
                        sheet.Cells[2, 21].Value = isAr ? firstAsset.ProjectailMaterial?.NameAr : firstAsset.ProjectailMaterial?.NameEn;
                        sheet.Cells[2, 22].Value = isAr ? firstAsset.LookupCaliber?.NameAr : firstAsset.LookupCaliber?.NameEn;
                        sheet.Cells[2, 23].Value = firstAsset.UNNumber;
                        sheet.Cells[2, 24].Value = firstAsset.Distribution;
                        sheet.Cells[2, 25].Value = firstAsset.ReferenceNo;
                        sheet.Cells[2, 26].Value = isAr ? firstAsset.Classification?.NameAr : firstAsset.Classification?.NameEn;
                        sheet.Cells[2, 27].Value = isAr ? firstAsset.Type?.NameAr : firstAsset.Type?.NameEn;
                        sheet.Cells[2, 28].Value = firstAsset.Notes;
                    }
                    else
                    {
                        sheet.Cells[2, 1].Value = "Sample Ammunition";
                        sheet.Cells[2, 2].Value = "AMM-001";
                    }
                },
                (package) =>
                {
                    CreateLookupSheet(package, "Units", _units);
                    CreateLookupSheet(package, "Calibers", _calibers);
                    CreateLookupSheet(package, "CaseTypes", _caseTypes);
                    CreateLookupSheet(package, "Propellants", _propellants);
                    CreateLookupSheet(package, "Compatibilities", _compatibilities);
                    CreateLookupSheet(package, "HazardDivisions", _hazardDivisions);
                    CreateLookupSheet(package, "NatureOptions", _natureOptions);
                    CreateLookupSheet(package, "PrimaryPurposes", _primaryPurposes);
                    CreateLookupSheet(package, "ProjectileColors", _projectileColors);
                    CreateLookupSheet(package, "ProjectileMaterials", _projectileMaterials);
                    CreateLookupSheet(package, "Classifications", _classifications);
                    CreateLookupSheet(package, "ItemTypes", _itemTypes);
                },
                (sheet) =>
                {
                    AddDataValidation(sheet, 10, "Units");
                    AddYesNoValidation(sheet, 12);
                    AddDataValidation(sheet, 14, "CaseTypes");
                    AddDataValidation(sheet, 15, "Propellants");
                    AddDataValidation(sheet, 16, "Compatibilities");
                    AddDataValidation(sheet, 17, "HazardDivisions");
                    AddDataValidation(sheet, 18, "NatureOptions");
                    AddDataValidation(sheet, 19, "PrimaryPurposes");
                    AddDataValidation(sheet, 20, "ProjectileColors");
                    AddDataValidation(sheet, 21, "ProjectileMaterials");
                    AddDataValidation(sheet, 22, "Calibers");
                    AddDataValidation(sheet, 26, "Classifications");
                    AddDataValidation(sheet, 27, "ItemTypes");
                }
            );
        }

        // Helpers
        private async Task LoadLookupsAsync(List<AmmunitionImportDto> importItems = null)
        {
            _units = await _unitRepository.Find(u => !u.IsDeleted && u.ItemType == ItemType.Ammunition).ToListAsync();
            _caseTypes = await _caseTypeRepository.Find(c => !c.IsDeleted).ToListAsync();
            _propellants = await _propellantRepository.Find(p => !p.IsDeleted).ToListAsync();
            _compatibilities = await _compatibilityRepository.Find(c => !c.IsDeleted).ToListAsync();
            _hazardDivisions = await _hazardDivisionRepository.Find(h => !h.IsDeleted).ToListAsync();
            _natureOptions = await _natureOptionRepository.Find(n => !n.IsDeleted).ToListAsync();
            _primaryPurposes = await _primaryPurposRepository.Find(p => !p.IsDeleted).ToListAsync();
            _projectileColors = await _colorRepository.Find(c => !c.IsDeleted).ToListAsync();
            _projectileMaterials = await _projectailMaterialRepository.Find(p => !p.IsDeleted).ToListAsync();
            _classifications = await _classificationRepository.Find(c => !c.IsDeleted).ToListAsync();
            _itemTypes = await _itemTypeLookupRepository.Find(i => !i.IsDeleted && i.ItemType == ItemType.Ammunition).ToListAsync();
            _calibers = await _caliberRepository.Find(c => !c.IsDeleted && c.ItemType == ItemType.Ammunition).ToListAsync();

            // Build cache
            _cachedLookups["Units"] = BuildLookup(_units, x => x.NameEn, x => x.NameAr, x => x.Id);
            _cachedLookups["Calibers"] = BuildLookup(_calibers, x => x.NameEn, x => x.NameAr, x => x.Id);
            _cachedLookups["CaseTypes"] = BuildLookup(_caseTypes, x => x.NameEn, x => x.NameAr, x => x.Id);
            _cachedLookups["Propellants"] = BuildLookup(_propellants, x => x.NameEn, x => x.NameAr, x => x.Id);
            _cachedLookups["Compatibilities"] = BuildLookup(_compatibilities, x => x.NameEn, x => x.NameAr, x => x.Id);
            _cachedLookups["HazardDivisions"] = BuildLookup(_hazardDivisions, x => x.NameEn, x => x.NameAr, x => x.Id);
            _cachedLookups["NatureOptions"] = BuildLookup(_natureOptions, x => x.NameEn, x => x.NameAr, x => x.Id);
            _cachedLookups["PrimaryPurposes"] = BuildLookup(_primaryPurposes, x => x.NameEn, x => x.NameAr, x => x.Id);
            _cachedLookups["ProjectileColors"] = BuildLookup(_projectileColors, x => x.NameEn, x => x.NameAr, x => x.Id);
            _cachedLookups["ProjectileMaterials"] = BuildLookup(_projectileMaterials, x => x.NameEn, x => x.NameAr, x => x.Id);
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

                var existingRecords = await _ammunitionRepository
                    .Find(a => !a.IsDeleted && (itemNos.Contains(a.ItemNo) || a.Nsn != null && nsns.Contains(a.Nsn)))
                    .Select(a => new { a.ItemNo, a.Nsn })
                    .ToListAsync();

                foreach (var rec in existingRecords)
                {
                    if (!string.IsNullOrEmpty(rec.ItemNo)) _existingItemNos.Add(rec.ItemNo);
                    if (!string.IsNullOrEmpty(rec.Nsn)) _existingNsns.Add(rec.Nsn);
                }
            }
        }

        private async Task<CreateUpdateAmmunitionDto> MapImportDtoToEntityAsync(AmmunitionImportDto importDto, string language)
        {
            var dto = new CreateUpdateAmmunitionDto
            {
                Name = importDto.Name,
                NameAr = importDto.NameAr,
                ItemNo = importDto.ItemNo,
                PartNo = importDto.PartNo,
                Price = importDto.Price,
                MinimumQuantity = importDto.MinimumQuantity,
                Nsn = importDto.Nsn,
                Distribution = importDto.Distribution,
                ReferenceNo = importDto.ReferenceNo,
                UNNumber = importDto.UNNumber,
                Notes = importDto.Notes,
                ArmNumber = importDto.ArmNumber,
                BulletDiameter = importDto.BulletDiameter,
                IsLinked = importDto.IsLinked,
                Primer = importDto.Primer,
                TotalWeight = importDto.TotalWeight
            };

            dto.CaliberId = FindLookupIdCached("Calibers", importDto.Caliber);
            dto.BulletDiameterUnitId = FindLookupIdCached("Units", importDto.BulletDiameterUnit);
            dto.CaseTypeId = FindLookupIdCached("CaseTypes", importDto.CaseType);
            dto.PropellantId = FindLookupIdCached("Propellants", importDto.Propellant);
            dto.CompatibilityId = FindLookupIdCached("Compatibilities", importDto.Compatibility);
            dto.HazardDivisionId = FindLookupIdCached("HazardDivisions", importDto.HazardDivision);
            dto.NatureOptionId = FindLookupIdCached("NatureOptions", importDto.NatureOption);
            var primaryPurposId = FindLookupIdCached("PrimaryPurposes", importDto.PrimaryPurpose);
            if (primaryPurposId.HasValue)
                dto.PrimaryPurposIds = new List<long> { primaryPurposId.Value };
            dto.ProjectileColorId = FindLookupIdCached("ProjectileColors", importDto.ProjectileColor);
            dto.ProjectailMaterialId = FindLookupIdCached("ProjectileMaterials", importDto.ProjectileMaterial);
            dto.ClassificationId = FindLookupIdCached("Classifications", importDto.Classification);
            dto.TypeId = FindLookupIdCached("ItemTypes", importDto.Type);
            dto.AmmunitionType = AmmunitionType.Small;

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

        private async Task<List<string>> ValidateDtoAsync(CreateUpdateAmmunitionDto dto)
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
            // Same mapping dictionary as before
            return new Dictionary<string, string>
            {
                { "Name*", nameof(AmmunitionImportDto.Name) },
                { "Name (Arabic)", nameof(AmmunitionImportDto.NameAr) },
                { "Name Arabic", nameof(AmmunitionImportDto.NameAr) },
                { "Item No*", nameof(AmmunitionImportDto.ItemNo) },
                { "Part No", nameof(AmmunitionImportDto.PartNo) },
                { "Arm Number", nameof(AmmunitionImportDto.ArmNumber) },
                { "NSN", nameof(AmmunitionImportDto.Nsn) },
                { "Price", nameof(AmmunitionImportDto.Price) },
                { "Minimum Quantity", nameof(AmmunitionImportDto.MinimumQuantity) },
                { "Bullet Diameter", nameof(AmmunitionImportDto.BulletDiameter) },
                { "Bullet Diameter Unit", nameof(AmmunitionImportDto.BulletDiameterUnit) },
                { "Total Weight", nameof(AmmunitionImportDto.TotalWeight) },
                { "Is Linked", nameof(AmmunitionImportDto.IsLinked) },
                { "Primer", nameof(AmmunitionImportDto.Primer) },
                { "Case Type", nameof(AmmunitionImportDto.CaseType) },
                { "Propellant", nameof(AmmunitionImportDto.Propellant) },
                { "Compatibility", nameof(AmmunitionImportDto.Compatibility) },
                { "Hazard Division", nameof(AmmunitionImportDto.HazardDivision) },
                { "Nature Option", nameof(AmmunitionImportDto.NatureOption) },
                { "Primary Purpose", nameof(AmmunitionImportDto.PrimaryPurpose) },
                { "Projectile Color", nameof(AmmunitionImportDto.ProjectileColor) },
                { "Projectile Material", nameof(AmmunitionImportDto.ProjectileMaterial) },
                { "Caliber", nameof(AmmunitionImportDto.Caliber) },
                { "UN Number", nameof(AmmunitionImportDto.UNNumber) },
                { "Distribution", nameof(AmmunitionImportDto.Distribution) },
                { "Reference No", nameof(AmmunitionImportDto.ReferenceNo) },
                { "Classification", nameof(AmmunitionImportDto.Classification) },
                { "Type", nameof(AmmunitionImportDto.Type) },
                { "Notes", nameof(AmmunitionImportDto.Notes) },
                // Arabic...
                { "الاسم*", nameof(AmmunitionImportDto.Name) },
                { "الاسم (بالعربية)", nameof(AmmunitionImportDto.NameAr) },
                { "رقم الصنف*", nameof(AmmunitionImportDto.ItemNo) },
                { "رقم القطعة", nameof(AmmunitionImportDto.PartNo) },
                { "رقم الجزء", nameof(AmmunitionImportDto.PartNo) }, // backward compatible
                { "رقم ARM", nameof(AmmunitionImportDto.ArmNumber) },
                { "رقم NSN", nameof(AmmunitionImportDto.Nsn) },
                { "السعر", nameof(AmmunitionImportDto.Price) },
                { "الكمية الدنيا", nameof(AmmunitionImportDto.MinimumQuantity) },
                { "قطر الرصاصة", nameof(AmmunitionImportDto.BulletDiameter) },
                { "وحدة قطر الرصاصة", nameof(AmmunitionImportDto.BulletDiameterUnit) },
                { "الوزن الكلي", nameof(AmmunitionImportDto.TotalWeight) },
                { "مرتبط", nameof(AmmunitionImportDto.IsLinked) },
                { "الكبسولة", nameof(AmmunitionImportDto.Primer) },
                { "نوع الغلاف", nameof(AmmunitionImportDto.CaseType) },
                { "المادة الدافعة", nameof(AmmunitionImportDto.Propellant) },
                { "التوافق", nameof(AmmunitionImportDto.Compatibility) },
                { "قسم الخطر", nameof(AmmunitionImportDto.HazardDivision) },
                { "خيار الطبيعة", nameof(AmmunitionImportDto.NatureOption) },
                { "الغرض الأساسي", nameof(AmmunitionImportDto.PrimaryPurpose) },
                { "لون المقذوف", nameof(AmmunitionImportDto.ProjectileColor) },
                { "مادة المقذوف", nameof(AmmunitionImportDto.ProjectileMaterial) },
                { "العيار", nameof(AmmunitionImportDto.Caliber) },
                { "رقم الأمم المتحدة", nameof(AmmunitionImportDto.UNNumber) },
                { "التوزيع", nameof(AmmunitionImportDto.Distribution) },
                { "الرقم المرجعي", nameof(AmmunitionImportDto.ReferenceNo) },
                { "التصنيف", nameof(AmmunitionImportDto.Classification) },
                { "النوع", nameof(AmmunitionImportDto.Type) },
                { "ملاحظات", nameof(AmmunitionImportDto.Notes) }
            };
        }

        private void CreateLookupSheet<T>(ExcelPackage package, string sheetName, List<T> items)
        {
            // Helper for templates
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

        private void AddYesNoValidation(ExcelWorksheet worksheet, int column)
        {
            var columnLetter = GetColumnLetter(column);
            var validationRange = $"{columnLetter}2:{columnLetter}10000";
            var validation = worksheet.DataValidations.AddListValidation(validationRange);
            validation.Formula.Values.Add("Yes");
            validation.Formula.Values.Add("No");
            validation.ShowErrorMessage = true;
            validation.Error = "Please select 'Yes' or 'No'";
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
