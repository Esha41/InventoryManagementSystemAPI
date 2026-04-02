using System.Collections.Generic;
using AutoMapper;
using FluentValidation;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.Returns.Dtos;
using Ettad.RequestManagement.Service.Common;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Ettad.Notification.Service;
using Microsoft.AspNetCore.Identity;
using Ettad.Comman.Idenitity;
using Microsoft.Extensions.Logging;
using Ettad.Workflows.Service.Interface;
using Ettad.Workflows.Service.DTO;
using Microsoft.AspNetCore.Http;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Comman.Enums;
using Ettad.CrossCutting.Comman.Time;

namespace Ettad.RequestManagement.Service.Returns
{
    public class ReturnService : IReturnService
    {
        private readonly ICrossCuttingRepository<Return> _returnRepository;
        private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;
        private readonly ICrossCuttingRepository<RequestPurpose> _requestPurposeRepository;
        private readonly IWorkflowApprovalService _workflowApprovalService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateReturnDto> _createValidator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRequestNoGeneratorService _requestNoGeneratorService;
        private readonly INotificationHelperService _notificationHelperService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ReturnService> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly ICrossCuttingRepository<FileUplodDetails> _fileDetailsRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ICrossCuttingRepository<Ettad.Data.Entities.Inventory> _inventoryRepository;
        private readonly ICrossCuttingRepository<InventoryDetail> _inventoryDetailRepository;
        private readonly ICrossCuttingRepository<Asset> _assetRepository;
        private readonly ICrossCuttingRepository<Batch> _batchRepository;
        private readonly ICrossCuttingRepository<Depot> _depotRepository;
        private readonly ICrossCuttingRepository<BaseItem> _baseItemRepository;

        public ReturnService(
            ICrossCuttingRepository<Return> returnRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            ICrossCuttingRepository<RequestPurpose> requestPurposeRepository,
            IWorkflowApprovalService workflowApprovalService,
            IMapper mapper,
            IValidator<CreateReturnDto> createValidator,
            ICurrentUserService currentUserService,
            IRequestNoGeneratorService requestNoGeneratorService,
            INotificationHelperService notificationHelperService,
            UserManager<ApplicationUser> userManager,
            ILogger<ReturnService> logger,
            IFileUploadService fileUploadService,
            ICrossCuttingRepository<FileUplodDetails> fileDetailsRepository,
            IDateTimeProvider dateTimeProvider,
            ICrossCuttingRepository<Ettad.Data.Entities.Inventory> inventoryRepository,
            ICrossCuttingRepository<InventoryDetail> inventoryDetailRepository,
            ICrossCuttingRepository<Asset> assetRepository,
            ICrossCuttingRepository<Batch> batchRepository,
            ICrossCuttingRepository<Depot> depotRepository,
            ICrossCuttingRepository<BaseItem> baseItemRepository)
        {
            _returnRepository = returnRepository;
            _requestItemRepository = requestItemRepository;
            _requestPurposeRepository = requestPurposeRepository;
            _workflowApprovalService = workflowApprovalService;
            _mapper = mapper;
            _createValidator = createValidator;
            _currentUserService = currentUserService;
            _requestNoGeneratorService = requestNoGeneratorService;
            _notificationHelperService = notificationHelperService;
            _userManager = userManager;
            _logger = logger;
            _fileUploadService = fileUploadService;
            _fileDetailsRepository = fileDetailsRepository;
            _dateTimeProvider = dateTimeProvider;
            _inventoryRepository = inventoryRepository;
            _inventoryDetailRepository = inventoryDetailRepository;
            _assetRepository = assetRepository;
            _batchRepository = batchRepository;
            _depotRepository = depotRepository;
            _baseItemRepository = baseItemRepository;
        }

        public async Task<APIOperationResponse<ReturnDto>> GetByIdAsync(long id)
        {
            _logger.LogInformation("Getting return by ID: {ReturnId}. User: {UserId}", id, _currentUserService.UserId);
            
            try
            {
                var returnEntity = await _returnRepository.FindOneAsync(
                    r => r.Id == id && !r.IsDeleted,
                    false,
                    nameof(BaseRequest.Department),
                    nameof(BaseRequest.Requester),
                    nameof(BaseRequest.RequestPurpose),
                    $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}",
                    nameof(Return.ReturnToDepot)
                );

                if (returnEntity == null)
                {
                    _logger.LogWarning("Return not found. ReturnId: {ReturnId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<ReturnDto>.Fail(ResponseType.NotFound, "Return not found");
                }

                var dto = _mapper.Map<ReturnDto>(returnEntity);
                
                _logger.LogInformation("Successfully retrieved return. ReturnId: {ReturnId}, RequestNo: {RequestNo}", id, returnEntity.RequestNo);
                return APIOperationResponse<ReturnDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting return by ID. ReturnId: {ReturnId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<ReturnDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<ReturnDto>>> GetAllAsync()
        {
            _logger.LogInformation("Getting all returns. User: {UserId}", _currentUserService.UserId);
            
            try
            {
                var returns = await _returnRepository.FindAsync(
                    r => !r.IsDeleted && r.RequestType == RequestType.Return,
                    false,
                    nameof(BaseRequest.Department),
                    nameof(BaseRequest.Requester),
                    nameof(BaseRequest.RequestPurpose),
                    $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}",
                    nameof(Return.ReturnToDepot)
                );

                var dtos = _mapper.Map<List<ReturnDto>>(returns);
                
                _logger.LogInformation("Successfully retrieved {ReturnCount} returns. User: {UserId}", dtos.Count, _currentUserService.UserId);
                return APIOperationResponse<List<ReturnDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all returns. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<ReturnDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateReturnDto inputDto)
        {
            return await CreateAsync(inputDto, null);
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateReturnDto inputDto, List<IFormFile> files)
        {
            var currentUserId = _currentUserService.UserId;
            var departmentId = _currentUserService.DepartmentId;

            if (!departmentId.HasValue || departmentId.Value <= 0)
            {
                _logger.LogWarning("Cannot create return. No department assigned to current user. User: {UserId}", currentUserId);
                return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "Department not found for current user.");
            }

            _logger.LogInformation("Creating new return. DepartmentId: {DepartmentId}, RequestPurposeId: {RequestPurposeId}, User: {UserId}, HasFiles: {HasFiles}", 
                departmentId.Value, inputDto.RequestPurposeId, currentUserId, files != null && files.Count > 0);
            
            try
            {
                // Validate input
                var validationResult = await _createValidator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning("Return validation failed. DepartmentId: {DepartmentId}, Errors: {ValidationErrors}, User: {UserId}", 
                        departmentId.Value, errors, currentUserId);
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                // Validate that RequestPurposeId belongs to a RequestPurpose with type Return
                var requestPurpose = await _requestPurposeRepository.FindOneAsync(
                    rp => rp.Id == inputDto.RequestPurposeId && rp.RequestType == RequestType.Return && !rp.IsDeleted
                );

                if (requestPurpose == null)
                {
                    _logger.LogWarning("Invalid request purpose for return. RequestPurposeId: {RequestPurposeId}, User: {UserId}", 
                        inputDto.RequestPurposeId, _currentUserService.UserId);
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "Request purpose must be of type Return");
                }

                // Step 1: Save files first (before creating return) to create FileUplodMaster records
                // When files are selected: if file upload fails or throws an exception, do not create the return.
                List<long> savedFileMasterIds = null;
                if (files != null && files.Count > 0)
                {
                    try
                    {
                        var saveFilesResult = await _fileUploadService.SaveFilesAsync(files, FileEntityType.Return);
                        if (!saveFilesResult.Succeeded || saveFilesResult.Data == null)
                        {
                            _logger.LogWarning("Failed to save files before creating return. Error: {Error}, User: {UserId}",
                                saveFilesResult.Message ?? "Unknown error", currentUserId);
                            return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                                saveFilesResult.Message ?? "File upload failed. Return was not created.");
                        }
                        savedFileMasterIds = saveFilesResult.Data;
                        _logger.LogInformation("Files saved successfully before return creation. FileCount: {FileCount}, MasterIds: {MasterIds}, User: {UserId}",
                            files.Count, string.Join(", ", savedFileMasterIds), currentUserId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Exception during file upload before return creation. User: {UserId}", currentUserId);
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                            $"File upload failed: {ex.Message}. Return was not created.");
                    }
                }

                // Step 2: Map DTO to entity
                var returnEntity = _mapper.Map<Return>(inputDto);
                returnEntity.DepartmentId = departmentId.Value;
                
                // Generate RequestNo
                returnEntity.RequestNo = await _requestNoGeneratorService.GenerateRequestNoAsync(RequestType.Return, departmentId.Value);
                
                _logger.LogInformation("Generated RequestNo: {RequestNo} for return", returnEntity.RequestNo);
                
                returnEntity.RequestType = RequestType.Return;
                returnEntity.Status = RequestStatus.New; // Always set to New when creating
                returnEntity.CreationDate = _dateTimeProvider.Now;
                returnEntity.CreatedBy = currentUserId;
                returnEntity.RequesterId = currentUserId;

                // Map return items
                returnEntity.RequestItems = inputDto.ReturnItems
                    .Select(item =>
                    {
                        var requestItem = _mapper.Map<RequestItem>(item);
                        return requestItem;
                    })
                    .ToList();

                _logger.LogInformation("Adding {ItemCount} return items to return. RequestNo: {RequestNo}", 
                    returnEntity.RequestItems.Count, returnEntity.RequestNo);

                // Step 3: Add to repository
                var createdReturn = await _returnRepository.AddAsync(returnEntity);

                // Step 4: Link files to the return (create FileUplodDetails records) if files were saved
                // When files were selected: if linking fails, delete the return and do not succeed.
                if (savedFileMasterIds != null && savedFileMasterIds.Count > 0)
                {
                    try
                    {
                        foreach (var masterId in savedFileMasterIds)
                        {
                            var detail = new FileUplodDetails
                            {
                                FileUplodMasterId = masterId,
                                Entity = FileEntityType.Order,
                                EntityId = createdReturn.Id
                            };

                            await _fileDetailsRepository.AddAsync(detail);
                        }

                        _logger.LogInformation("Files linked successfully to return. ReturnId: {ReturnId}, FileCount: {FileCount}, User: {UserId}",
                            createdReturn.Id, savedFileMasterIds.Count, currentUserId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Exception occurred while linking files to return. ReturnId: {ReturnId}, User: {UserId}. Rolling back return creation.",
                            createdReturn.Id, currentUserId);
                        await _returnRepository.DeleteAsync(createdReturn);
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                            $"File upload failed: {ex.Message}. Return was not created.");
                    }
                }

                // Start workflow for the return
                var workflowStarted = await _workflowApprovalService.StartWorkflowAsync(
                    createdReturn.Id,
                    WorkflowType.Return);

                if (workflowStarted)
                {
                    _logger.LogInformation("Workflow started successfully for return. ReturnId: {ReturnId}, User: {UserId}",
                        createdReturn.Id, currentUserId);
                }
                else
                {
                    _logger.LogWarning("Failed to start workflow for return. ReturnId: {ReturnId}, User: {UserId}",
                        createdReturn.Id, currentUserId);
                }

                await NotifyReturnAsync(
                    "Return Created",
                    $"Return request {createdReturn.RequestNo} has been created.",
                    createdReturn.Id);

                _logger.LogInformation("Return created successfully. ReturnId: {ReturnId}, RequestNo: {RequestNo}, ItemCount: {ItemCount}, User: {UserId}", 
                    createdReturn.Id, createdReturn.RequestNo, createdReturn.RequestItems?.Count ?? 0, currentUserId);
                
                return APIOperationResponse<long>.Success(createdReturn.Id, "Return created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating return. DepartmentId: {DepartmentId}, User: {UserId}", 
                    departmentId.Value, currentUserId);
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> ChangePriorityAsync(long id, RequestPriority priority)
        {
            _logger.LogInformation("Changing return priority. ReturnId: {ReturnId}, NewPriority: {Priority}, User: {UserId}", 
                id, priority, _currentUserService.UserId);
            
            try
            {
                // Check if return exists
                var existingReturn = await _returnRepository.FindOneAsync(
                    r => r.Id == id && !r.IsDeleted
                );

                if (existingReturn == null)
                {
                    _logger.LogWarning("Return not found for priority change. ReturnId: {ReturnId}, User: {UserId}", 
                        id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Return not found");
                }

                var oldPriority = existingReturn.Priority;
                
                // Update priority
                existingReturn.Priority = priority;
                existingReturn.ModificationDate = _dateTimeProvider.Now;
                existingReturn.ModifiedBy = _currentUserService.UserId;

                // Update in repository
                await _returnRepository.UpdateAsync(existingReturn);

                await NotifyReturnAsync(
                    "Return Priority Updated",
                    $"Return request {existingReturn.RequestNo} priority changed to {priority}.",
                    existingReturn.Id);

                _logger.LogInformation("Return priority updated successfully. ReturnId: {ReturnId}, RequestNo: {RequestNo}, OldPriority: {OldPriority}, NewPriority: {NewPriority}, User: {UserId}", 
                    id, existingReturn.RequestNo, oldPriority, priority, _currentUserService.UserId);
                
                return APIOperationResponse<bool>.Success(true, "Return priority updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing return priority. ReturnId: {ReturnId}, Priority: {Priority}, User: {UserId}", 
                    id, priority, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting return. ReturnId: {ReturnId}, User: {UserId}", id, _currentUserService.UserId);
            
            try
            {
                var returnEntity = await _returnRepository.FindOneAsync(r => r.Id == id && !r.IsDeleted);
                if (returnEntity == null)
                {
                    _logger.LogWarning("Return not found for deletion. ReturnId: {ReturnId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Return not found");
                }

                // Soft delete - interceptor will handle IsDeleted, DeletionDate, and DeletedBy automatically
                await _returnRepository.DeleteAsync(returnEntity);

                await NotifyReturnAsync(
                    "Return Deleted",
                    $"Return request {returnEntity.RequestNo} has been deleted.",
                    returnEntity.Id);

                _logger.LogInformation("Return deleted successfully. ReturnId: {ReturnId}, RequestNo: {RequestNo}, User: {UserId}", 
                    id, returnEntity.RequestNo, _currentUserService.UserId);
                
                return APIOperationResponse<bool>.Success(true, "Return deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting return. ReturnId: {ReturnId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> SetDepotAsync(long returnId, SetReturnDepotDto dto)
        {
            _logger.LogInformation("Setting depot for return. ReturnId: {ReturnId}, DepotId: {DepotId}, User: {UserId}",
                returnId, dto.DepotId, _currentUserService.UserId);

            try
            {
                var returnEntity = await _returnRepository.FindOneAsync(r => r.Id == returnId && !r.IsDeleted);
                if (returnEntity == null)
                {
                    _logger.LogWarning("Return not found. ReturnId: {ReturnId}, User: {UserId}", returnId, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Return not found");
                }

                var depot = await _depotRepository.FindOneAsync(d => d.Id == dto.DepotId && !d.IsDeleted);
                if (depot == null)
                {
                    _logger.LogWarning("Depot not found. DepotId: {DepotId}, User: {UserId}", dto.DepotId, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Depot not found");
                }

                returnEntity.ReturnToDepotId = dto.DepotId;
                returnEntity.ModificationDate = _dateTimeProvider.Now;
                returnEntity.ModifiedBy = _currentUserService.UserId;

                await _returnRepository.UpdateAsync(returnEntity);

                _logger.LogInformation("Depot set successfully for return. ReturnId: {ReturnId}, DepotId: {DepotId}, User: {UserId}",
                    returnId, dto.DepotId, _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(true, "Depot set successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting depot for return. ReturnId: {ReturnId}, User: {UserId}", returnId, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> SetDeliveryDateAsync(long returnId, SetReturnDeliveryDateDto dto)
        {
            _logger.LogInformation("Setting delivery date for return. ReturnId: {ReturnId}, DeliveryDate: {DeliveryDate}, User: {UserId}",
                returnId, dto.DeliveryDate, _currentUserService.UserId);

            try
            {
                var returnEntity = await _returnRepository.FindOneAsync(r => r.Id == returnId && !r.IsDeleted);
                if (returnEntity == null)
                {
                    _logger.LogWarning("Return not found. ReturnId: {ReturnId}, User: {UserId}", returnId, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Return not found");
                }

                returnEntity.DeliveryDate = dto.DeliveryDate;
                returnEntity.ModificationDate = _dateTimeProvider.Now;
                returnEntity.ModifiedBy = _currentUserService.UserId;

                await _returnRepository.UpdateAsync(returnEntity);

                _logger.LogInformation("Delivery date set successfully for return. ReturnId: {ReturnId}, DeliveryDate: {DeliveryDate}, User: {UserId}",
                    returnId, dto.DeliveryDate, _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(true, "Delivery date set successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting delivery date for return. ReturnId: {ReturnId}, User: {UserId}", returnId, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> ProcessReturnItemsAsync(long returnId, ProcessReturnItemsDto dto, List<IFormFile>? files = null)
        {
            _logger.LogInformation("Processing return items. ReturnId: {ReturnId}, AmmoExplosiveCount: {AmmoCount}, WeaponCount: {WeaponCount}, FileCount: {FileCount}, User: {UserId}",
                returnId, dto.AmmoExplosiveItems?.Count ?? 0, dto.WeaponItems?.Count ?? 0, files?.Count ?? 0, _currentUserService.UserId);

            try
            {
                var returnEntity = await _returnRepository.FindOneAsync(
                    r => r.Id == returnId && !r.IsDeleted,
                    false,
                    $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}"
                );

                if (returnEntity == null)
                {
                    _logger.LogWarning("Return not found. ReturnId: {ReturnId}, User: {UserId}", returnId, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Return not found");
                }

                if (!returnEntity.ReturnToDepotId.HasValue)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Depot must be set before processing return items");
                }

                var depotId = returnEntity.ReturnToDepotId.Value;

                // Optional attachments: same linkage as return creation (Order entity + request id)
                if (files != null && files.Count > 0)
                {
                    var uploadResult = await _fileUploadService.UploadFilesForEntityAsync(files, FileEntityType.Order, returnId);
                    if (!uploadResult.Succeeded)
                    {
                        _logger.LogWarning("Failed to upload process-return attachments. ReturnId: {ReturnId}, Message: {Message}",
                            returnId, uploadResult.Message);
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                            uploadResult.Message ?? "Failed to upload attachments.");
                    }

                    _logger.LogInformation("Process-return attachments uploaded. ReturnId: {ReturnId}, DetailCount: {Count}",
                        returnId, uploadResult.Data?.Count ?? 0);
                }

                // Process Ammo/Explosive items
                if (dto.AmmoExplosiveItems != null && dto.AmmoExplosiveItems.Any())
                {
                    Ettad.Data.Entities.Inventory returnInventory = null;

                    foreach (var item in dto.AmmoExplosiveItems)
                    {
                        var baseItem = await _baseItemRepository.FindOneAsync(i => i.Id == item.ItemId && !i.IsDeleted);
                        if (baseItem == null)
                        {
                            return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, $"Item with ID {item.ItemId} not found");
                        }

                        if (baseItem.ItemType != ItemType.Ammunition && baseItem.ItemType != ItemType.Explosive)
                        {
                            return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                                $"Item {item.ItemId} is not Ammunition or Explosive type");
                        }

                        var lotKey = (item.Lot ?? string.Empty).Trim();

                        var existingDetail = await _inventoryDetailRepository.FindOneAsync(
                            id => id.ItemId == item.ItemId
                                && id.Lot == lotKey
                                && id.Inventory.DepoId == depotId
                                && !id.Inventory.IsDeleted,
                            false,
                            nameof(InventoryDetail.Inventory)
                        );

                        if (existingDetail != null)
                        {
                            existingDetail.ItemQuantity += item.Quantity;
                            existingDetail.IsReturned = true;
                            existingDetail.ReadyForIssue = false;
                            await _inventoryDetailRepository.UpdateAsync(existingDetail);

                            _logger.LogInformation("Updated existing inventory detail. ItemId: {ItemId}, Lot: {Lot}, NewQuantity: {Quantity}",
                                item.ItemId, lotKey, existingDetail.ItemQuantity);
                        }
                        else
                        {
                            if (returnInventory == null)
                            {
                                returnInventory = new Ettad.Data.Entities.Inventory
                                {
                                    DepoId = depotId,
                                    Notes = $"Return #{returnEntity.RequestNo}",
                                    RecievedDate = _dateTimeProvider.Now,
                                    CreationDate = _dateTimeProvider.Now,
                                    CreatedBy = _currentUserService.UserId,
                                    InventoryDetails = new List<InventoryDetail>()
                                };
                                returnInventory = await _inventoryRepository.AddAsync(returnInventory);
                            }

                            var newDetail = new InventoryDetail
                            {
                                ItemId = item.ItemId,
                                Lot = lotKey,
                                ItemQuantity = item.Quantity,
                                InventoryId = returnInventory.Id,
                                IsReturned = true,
                                ReadyForIssue = false,
                                IsLotEmpty = string.IsNullOrEmpty(lotKey)
                            };
                            await _inventoryDetailRepository.AddAsync(newDetail);

                            _logger.LogInformation("Created new inventory detail. ItemId: {ItemId}, Lot: {Lot}, Quantity: {Quantity}, InventoryId: {InventoryId}",
                                item.ItemId, lotKey, item.Quantity, returnInventory.Id);
                        }
                    }
                }

                // Process Weapon items
                if (dto.WeaponItems != null && dto.WeaponItems.Any())
                {
                    foreach (var item in dto.WeaponItems)
                    {
                        var baseItem = await _baseItemRepository.FindOneAsync(i => i.Id == item.ItemId && !i.IsDeleted);
                        if (baseItem == null)
                        {
                            return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, $"Item with ID {item.ItemId} not found");
                        }

                        if (baseItem.ItemType != ItemType.Weapon)
                        {
                            return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                                $"Item {item.ItemId} is not a Weapon type");
                        }

                        var batchNumber = (item.BatchNumber ?? string.Empty).Trim();

                        var batch = await _batchRepository.FindOneAsync(
                            b => b.BatchNumber == batchNumber && b.DepotId == depotId && !b.IsDeleted
                        );

                        if (batch == null)
                        {
                            batch = new Batch
                            {
                                BatchNumber = batchNumber,
                                DepotId = depotId,
                                CreationDate = _dateTimeProvider.Now,
                                CreatedBy = _currentUserService.UserId
                            };
                            batch = await _batchRepository.AddAsync(batch);

                            _logger.LogInformation("Created new batch. BatchNumber: {BatchNumber}, DepotId: {DepotId}, BatchId: {BatchId}",
                                batchNumber, depotId, batch.Id);
                        }

                        var serialNumber = (item.SerialNumber ?? string.Empty).Trim();

                        var asset = await _assetRepository.FindOneAsync(
                            a => a.SerialNumber == serialNumber && a.ItemId == item.ItemId && !a.IsDeleted
                        );

                        if (asset != null)
                        {
                            asset.Status = AssetStatus.Returned;
                            asset.BatchId = batch.Id;
                            asset.DepotId = depotId;
                            asset.ModificationDate = _dateTimeProvider.Now;
                            asset.ModifiedBy = _currentUserService.UserId;
                            await _assetRepository.UpdateAsync(asset);

                            _logger.LogInformation("Updated existing asset. AssetId: {AssetId}, SerialNumber: {SerialNumber}, Status: Returned",
                                asset.Id, serialNumber);
                        }
                        else
                        {
                            var newAsset = new Asset
                            {
                                ItemId = item.ItemId,
                                SerialNumber = serialNumber,
                                DepotId = depotId,
                                BatchId = batch.Id,
                                Status = AssetStatus.Returned,
                                IsAssigned = false,
                                CreationDate = _dateTimeProvider.Now,
                                CreatedBy = _currentUserService.UserId
                            };
                            await _assetRepository.AddAsync(newAsset);

                            _logger.LogInformation("Created new asset. SerialNumber: {SerialNumber}, ItemId: {ItemId}, DepotId: {DepotId}, Status: Returned",
                                serialNumber, item.ItemId, depotId);
                        }
                    }
                }

                // Approve workflow and complete the return
                try
                {
                    var approveDto = new ApproveRejectWorkflowApprovalDto
                    {
                        BaseRequestID = returnEntity.Id,
                        Action = RequestStatus.Approved,
                        IsApproved = true,
                        Comments = "Return items processed",
                        SendToHigherApproval = false
                    };

                    var approveResult = await _workflowApprovalService.ProcessActionAsync(approveDto);
                    if (!approveResult.Succeeded)
                    {
                        _logger.LogWarning("Failed to approve workflow step for return. ReturnId: {ReturnId}, Error: {Error}",
                            returnId, approveResult.Message);
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                            $"Failed to approve workflow step: {approveResult.Message}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error approving workflow step for return. ReturnId: {ReturnId}", returnId);
                    return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError,
                        $"Failed to approve workflow step: {ex.Message}");
                }

                _logger.LogInformation("Return items processed successfully. ReturnId: {ReturnId}, User: {UserId}",
                    returnId, _currentUserService.UserId);

                return APIOperationResponse<bool>.Success(true, "Return items processed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing return items. ReturnId: {ReturnId}, User: {UserId}", returnId, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private async Task NotifyReturnAsync(string title, string message, long entityId)
        {
            try
            {
                var userId = _currentUserService.UserId;
                var userIds = string.IsNullOrWhiteSpace(userId) ? null : new List<string> { userId };

                await _notificationHelperService.SendNotificationAndEmailAsync(
                    title,
                    message,
                    entityType: nameof(Return),
                    entityId: entityId,
                    userIds: userIds,
                    senderId: userId,
                    includeSuperAdmins: true);
            }
            catch
            {
                // Suppress notification errors to avoid impacting main workflow
            }
        }
    }
}
