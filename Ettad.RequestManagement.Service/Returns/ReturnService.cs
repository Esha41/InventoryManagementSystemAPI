using AutoMapper;
using FluentValidation;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.Returns.Dtos;
using Ettad.Module.lookup.Dtos;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Ettad.Comman.Idenitity;
using Microsoft.Extensions.Logging;
using Ettad.Workflows.Service.Commands.WorkflowApproval.ProcessWorkflowAction;
using Ettad.Workflows.Service.Commands.WorkflowApproval.StartWorkflow;
using Ettad.Workflows.Service.Queries.WorkflowApproval.GetCurrentApprovalStepByRequestId;
using Ettad.Workflows.Service.Dtos;
using Microsoft.AspNetCore.Http;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Notification.Service.Interfaces;
using Ettad.Workflow.Service.Interface;
using MediatR;
using Ettad.RequestManagement.Service.Common.Interfaces;
using Ettad.Inventory.Service.AssetHistory.Dtos;
using Ettad.Inventory.Service.AssetHistory.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ettad.RequestManagement.Service.Returns
{
    public class ReturnService : IReturnService
    {
        private readonly ICrossCuttingRepository<Return> _returnRepository;
        private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;
        private readonly ICrossCuttingRepository<RequestPurpose> _requestPurposeRepository;
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
        private readonly ICrossCuttingRepository<AssetAssignment> _assignmentRepository;
        private readonly IAssetHistoryService _assetHistoryService;
        private readonly ICrossCuttingRepository<Batch> _batchRepository;
        private readonly ICrossCuttingRepository<Depot> _depotRepository;
        private readonly ICrossCuttingRepository<BaseItem> _baseItemRepository;
        private readonly ICrossCuttingRepository<ReturnTrackingLine> _returnTrackingLineRepository;
        private readonly IMediator _mediator;
        private readonly ITransactionManager _transactionManager;
        private readonly IWorkflowStartNotificationService _workflowStartNotificationService;
        private readonly IAttachmentRequirementUploadValidationService _attachmentRequirementUploadValidationService;

        public ReturnService(
            ICrossCuttingRepository<Return> returnRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            ICrossCuttingRepository<RequestPurpose> requestPurposeRepository,
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
            ICrossCuttingRepository<AssetAssignment> assignmentRepository,
            IAssetHistoryService assetHistoryService,
            ICrossCuttingRepository<Batch> batchRepository,
            ICrossCuttingRepository<Depot> depotRepository,
            ICrossCuttingRepository<BaseItem> baseItemRepository,
            ICrossCuttingRepository<ReturnTrackingLine> returnTrackingLineRepository,
            IMediator mediator,
            ITransactionManager transactionManager,
            IWorkflowStartNotificationService workflowStartNotificationService,
            IAttachmentRequirementUploadValidationService attachmentRequirementUploadValidationService)
        {
            _returnRepository = returnRepository;
            _requestItemRepository = requestItemRepository;
            _requestPurposeRepository = requestPurposeRepository;
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
            _assignmentRepository = assignmentRepository;
            _assetHistoryService = assetHistoryService;
            _batchRepository = batchRepository;
            _depotRepository = depotRepository;
            _baseItemRepository = baseItemRepository;
            _returnTrackingLineRepository = returnTrackingLineRepository;
            _mediator = mediator;
            _transactionManager = transactionManager;
            _workflowStartNotificationService = workflowStartNotificationService;
            _attachmentRequirementUploadValidationService = attachmentRequirementUploadValidationService;
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

        public Task<APIOperationResponse<long>> CreateAsync(CreateReturnDto inputDto)
        {
            return CreateAsync(inputDto, new Dictionary<long, IReadOnlyList<IFormFile>>(), null);
        }

        public Task<APIOperationResponse<long>> CreateAsync(CreateReturnDto inputDto, List<IFormFile> files)
        {
            return CreateAsync(inputDto, new Dictionary<long, IReadOnlyList<IFormFile>>(), files);
        }

        public async Task<APIOperationResponse<long>> CreateAsync(
            CreateReturnDto inputDto,
            IReadOnlyDictionary<long, IReadOnlyList<IFormFile>> filesByAttachmentRequirementId,
            List<IFormFile>? otherFiles)
        {
            var currentUserId = _currentUserService.UserId;
            var departmentId = _currentUserService.DepartmentId;

            if (!departmentId.HasValue || departmentId.Value <= 0)
            {
                _logger.LogWarning("Cannot create return. No department assigned to current user. User: {UserId}", currentUserId);
                return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "Department not found for current user.");
            }

            filesByAttachmentRequirementId ??= new Dictionary<long, IReadOnlyList<IFormFile>>();
            var slotFileCount = filesByAttachmentRequirementId
                .Sum(kvp => (kvp.Value ?? Array.Empty<IFormFile>()).Count(f => f != null && f.Length > 0));
            var otherFileCount = (otherFiles ?? new List<IFormFile>())
                .Count(f => f != null && f.Length > 0);

            _logger.LogInformation(
                "Creating new return. DepartmentId: {DepartmentId}, RequestPurposeId: {RequestPurposeId}, User: {UserId}, SlotFileCount: {SlotFileCount}, OtherFileCount: {OtherFileCount}",
                departmentId.Value, inputDto.RequestPurposeId, currentUserId, slotFileCount, otherFileCount);

            try
            {
                var validationResult = await _createValidator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning("Return validation failed. DepartmentId: {DepartmentId}, Errors: {ValidationErrors}, User: {UserId}",
                        departmentId.Value, errors, currentUserId);
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                var requestPurpose = await _requestPurposeRepository.FindOneAsync(
                    rp => rp.Id == inputDto.RequestPurposeId && rp.RequestType == RequestType.Return && !rp.IsDeleted
                );

                if (requestPurpose == null)
                {
                    _logger.LogWarning("Invalid request purpose for return. RequestPurposeId: {RequestPurposeId}, User: {UserId}",
                        inputDto.RequestPurposeId, _currentUserService.UserId);
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "Request purpose must be of type Return");
                }

                var attachmentValidation = await _attachmentRequirementUploadValidationService.ValidateAsync(
                    inputDto.RequestPurposeId,
                    filesByAttachmentRequirementId,
                    otherFiles);

                if (!attachmentValidation.Succeeded)
                {
                    _logger.LogWarning(
                        "Return attachment requirement validation failed. RequestPurposeId: {RequestPurposeId}, Message: {Message}, User: {UserId}",
                        inputDto.RequestPurposeId, attachmentValidation.Message, currentUserId);
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                        attachmentValidation.Message ?? "Attachment validation failed.");
                }

                WorkflowType returnWorkflowType = WorkflowType.Return;
                if (inputDto.ReturnItems != null && inputDto.ReturnItems.Any())
                {
                    var itemIds = inputDto.ReturnItems.Select(ri => ri.ItemId).Distinct().ToList();
                    var items = await _baseItemRepository.FindAsync(
                        item => itemIds.Contains(item.Id) && !item.IsDeleted);

                    if (items.Count() != itemIds.Count)
                    {
                        var foundIds = items.Select(i => i.Id).ToList();
                        var missingIds = itemIds.Except(foundIds).ToList();
                        _logger.LogWarning("Some items not found for return. Missing ItemIds: {MissingIds}, User: {UserId}",
                            string.Join(", ", missingIds), currentUserId);
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                            $"One or more items not found. Item IDs: {string.Join(", ", missingIds)}");
                    }

                    var itemTypes = items.Select(i => i.ItemType).Distinct().ToList();
                    var hasWeapon = itemTypes.Contains(ItemType.Weapon);
                    var hasAmmunition = itemTypes.Contains(ItemType.Ammunition);
                    var hasExplosive = itemTypes.Contains(ItemType.Explosive);
                    var hasOtherTypes = itemTypes.Any(t => t != ItemType.Weapon && t != ItemType.Ammunition && t != ItemType.Explosive);

                    if (hasWeapon && (hasAmmunition || hasExplosive || hasOtherTypes))
                    {
                        _logger.LogWarning("Invalid return: Weapons cannot be returned with other item types. ItemTypes: {ItemTypes}, User: {UserId}",
                            string.Join(", ", itemTypes), currentUserId);
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                            "Weapons cannot be returned with other item types. All items in a weapon return must be weapons.");
                    }

                    var hasAmmoOrExplosive = items.Any(i => i.ItemType == ItemType.Ammunition || i.ItemType == ItemType.Explosive);
                    returnWorkflowType = hasAmmoOrExplosive ? WorkflowType.Return : WorkflowType.Return_Weapon;
                }

                await _transactionManager.BeginAsync();

                Return createdReturn;
                try
                {
                    var flatFiles = new List<IFormFile>();
                    var attachmentRequirementIds = new List<long?>();

                    foreach (var kvp in filesByAttachmentRequirementId.OrderBy(x => x.Key))
                    {
                        foreach (var file in kvp.Value ?? Array.Empty<IFormFile>())
                        {
                            if (file == null || file.Length == 0)
                                continue;

                            flatFiles.Add(file);
                            attachmentRequirementIds.Add(kvp.Key);
                        }
                    }

                    if (otherFiles != null)
                    {
                        foreach (var file in otherFiles)
                        {
                            if (file == null || file.Length == 0)
                                continue;

                            flatFiles.Add(file);
                            attachmentRequirementIds.Add(null);
                        }
                    }

                    List<long>? savedFileMasterIds = null;
                    if (flatFiles.Count > 0)
                    {
                        var saveFilesResult = await _fileUploadService.SaveFilesAsync(flatFiles, FileEntityType.Order);
                        if (!saveFilesResult.Succeeded || saveFilesResult.Data == null)
                        {
                            await _transactionManager.RollbackAsync();
                            _logger.LogWarning("Failed to save files during return creation. Error: {Error}, User: {UserId}",
                                saveFilesResult.Message ?? "Unknown error", currentUserId);
                            return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                                saveFilesResult.Message ?? "File upload failed. Return was not created.");
                        }

                        savedFileMasterIds = saveFilesResult.Data;
                        _logger.LogInformation("Files saved during return transaction. FileCount: {FileCount}, MasterIds: {MasterIds}, User: {UserId}",
                            flatFiles.Count, string.Join(", ", savedFileMasterIds), currentUserId);
                    }

                    var returnEntity = _mapper.Map<Return>(inputDto);
                    returnEntity.DepartmentId = departmentId.Value;

                    returnEntity.RequestNo = await _requestNoGeneratorService.GenerateRequestNoAsync(RequestType.Return, departmentId.Value);

                    _logger.LogInformation("Generated RequestNo: {RequestNo} for return", returnEntity.RequestNo);

                    returnEntity.RequestType = RequestType.Return;
                    returnEntity.Status = RequestStatus.New;
                    returnEntity.CreationDate = _dateTimeProvider.Now;
                    returnEntity.CreatedBy = currentUserId;
                    returnEntity.RequesterId = currentUserId;

                    returnEntity.RequestItems = inputDto.ReturnItems
                        .Select(item =>
                        {
                            var requestItem = _mapper.Map<RequestItem>(item);
                            return requestItem;
                        })
                        .ToList();

                    _logger.LogInformation("Adding {ItemCount} return items to return. RequestNo: {RequestNo}",
                        returnEntity.RequestItems.Count, returnEntity.RequestNo);

                    createdReturn = await _returnRepository.AddAsync(returnEntity);

                    if (savedFileMasterIds != null && savedFileMasterIds.Count > 0)
                    {
                        for (var i = 0; i < savedFileMasterIds.Count; i++)
                        {
                            var detail = new FileUplodDetails
                            {
                                FileUplodMasterId = savedFileMasterIds[i],
                                Entity = FileEntityType.Order,
                                EntityId = createdReturn.Id,
                                AttachmentRequirementId = i < attachmentRequirementIds.Count
                                    ? attachmentRequirementIds[i]
                                    : null
                            };

                            await _fileDetailsRepository.AddAsync(detail);
                        }

                        _logger.LogInformation("Files linked successfully to return. ReturnId: {ReturnId}, FileCount: {FileCount}, User: {UserId}",
                            createdReturn.Id, savedFileMasterIds.Count, currentUserId);
                    }

                    var startResult = await _mediator.Send(
                        new StartWorkflowCommand(createdReturn.Id, returnWorkflowType));
                    var workflowStarted = startResult.Succeeded && startResult.Data?.Started == true;

                    if (!workflowStarted)
                    {
                        await _transactionManager.RollbackAsync();
                        if (!startResult.Succeeded)
                        {
                            _logger.LogWarning("Error starting workflow for return; transaction rolled back. WorkflowType: {WorkflowType}, User: {UserId}, Message: {Message}",
                                returnWorkflowType, currentUserId, startResult.Message);
                            return APIOperationResponse<long>.Fail((ResponseType)startResult.StatusCode,
                                startResult.Message ?? "An error occurred while starting the approval workflow.");
                        }

                        _logger.LogWarning("Failed to start workflow for return; transaction rolled back. WorkflowType: {WorkflowType}, User: {UserId}",
                            returnWorkflowType, currentUserId);
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                            "Return could not be created because the approval workflow could not be started.");
                    }

                    await _transactionManager.CommitAsync();

                    if (startResult.Data!.NotificationWorkflowStepId is long stepId)
                        await _workflowStartNotificationService.SendAsync(createdReturn.Id, stepId);

                    _logger.LogInformation("Workflow started successfully for return. ReturnId: {ReturnId}, WorkflowType: {WorkflowType}, User: {UserId}",
                        createdReturn.Id, returnWorkflowType, currentUserId);

                    await NotifyReturnAsync(
                        "Return Created",
                        $"Return request {createdReturn.RequestNo} has been created.",
                        createdReturn.Id);

                    _logger.LogInformation("Return created successfully. ReturnId: {ReturnId}, RequestNo: {RequestNo}, ItemCount: {ItemCount}, User: {UserId}",
                        createdReturn.Id, createdReturn.RequestNo, createdReturn.RequestItems?.Count ?? 0, currentUserId);

                    return APIOperationResponse<long>.Success(createdReturn.Id, "Return created successfully");
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    await _transactionManager.RollbackAsync();
                    _logger.LogError(ex, "Exception during return creation transaction. User: {UserId}", currentUserId);
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                        $"Return was not created: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                if (_transactionManager.HasActiveTransaction)
                    await _transactionManager.RollbackAsync();

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
                var returnDepot = await _depotRepository.FindOneAsync(d => d.Id == depotId && !d.IsDeleted);
                var depotName = returnDepot?.NameEn ?? returnDepot?.NameAr ?? depotId.ToString();

                foreach (var item in dto.AmmoExplosiveItems ?? Enumerable.Empty<ReturnAmmoExplosiveItemDto>())
                {
                    var badRi = await ValidateRequestItemBelongsToReturnAsync(returnEntity.Id, item.RequestItemId);
                    if (badRi != null)
                    {
                        return badRi;
                    }
                }

                foreach (var item in dto.WeaponItems ?? Enumerable.Empty<ReturnWeaponItemDto>())
                {
                    var badRi = await ValidateRequestItemBelongsToReturnAsync(returnEntity.Id, item.RequestItemId);
                    if (badRi != null)
                    {
                        return badRi;
                    }
                }

                var ammoTrackingItems = new List<ReturnAmmoExplosiveItemDto>();
                var weaponReceipts = new List<(ReturnWeaponItemDto Dto, long AssetId)>();

                // Ammo/Explosive: validate lines only; inventory is not updated (tracking history is written later).
                if (dto.AmmoExplosiveItems != null && dto.AmmoExplosiveItems.Any())
                {
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

                        ammoTrackingItems.Add(item);
                    }
                }

                // Process Weapon items (assets + notes)
                if (dto.WeaponItems != null && dto.WeaponItems.Any())
                {
                    foreach (var item in dto.WeaponItems)
                    {
                        if (!Enum.IsDefined(typeof(AssetStatus), item.Status))
                        {
                            return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                                $"Invalid asset status for item {item.ItemId}, serial '{item.SerialNumber}'.");
                        }

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
                        var assetNotes = string.IsNullOrWhiteSpace(item.Notes) ? null : item.Notes.Trim();

                        var asset = await _assetRepository.FindOneAsync(
                            a => a.SerialNumber == serialNumber && a.ItemId == item.ItemId && !a.IsDeleted
                        );

                        if (asset != null)
                        {
                            await ClearAssetAssignmentOnWeaponReturnAsync(
                                asset,
                                assetNotes,
                                returnEntity.Id,
                                returnEntity.RequestNo,
                                depotName);

                            asset.Status = item.Status;
                            asset.BatchId = batch.Id;
                            asset.DepotId = depotId;
                            asset.Notes = assetNotes;
                            asset.ModificationDate = _dateTimeProvider.Now;
                            asset.ModifiedBy = _currentUserService.UserId;
                            await _assetRepository.UpdateAsync(asset);

                            weaponReceipts.Add((item, asset.Id));

                            _logger.LogInformation("Updated existing asset. AssetId: {AssetId}, SerialNumber: {SerialNumber}, Status: {Status}",
                                asset.Id, serialNumber, item.Status);
                        }
                        else
                        {
                            var newAsset = new Asset
                            {
                                ItemId = item.ItemId,
                                SerialNumber = serialNumber,
                                DepotId = depotId,
                                BatchId = batch.Id,
                                Status = item.Status,
                                IsAssigned = false,
                                Notes = assetNotes,
                                CreationDate = _dateTimeProvider.Now,
                                CreatedBy = _currentUserService.UserId
                            };
                            newAsset = await _assetRepository.AddAsync(newAsset);

                            weaponReceipts.Add((item, newAsset.Id));

                            _logger.LogInformation("Created new asset. SerialNumber: {SerialNumber}, ItemId: {ItemId}, DepotId: {DepotId}, Status: {Status}",
                                serialNumber, item.ItemId, depotId, item.Status);
                        }
                    }
                }

                var now = _dateTimeProvider.Now;
                var userId = _currentUserService.UserId;

                foreach (var ammoDto in ammoTrackingItems)
                {
                    var receiptNotes = string.IsNullOrWhiteSpace(ammoDto.Notes) ? null : ammoDto.Notes.Trim();
                    var line = new ReturnTrackingLine
                    {
                        ReturnId = returnEntity.Id,
                        RequestId = returnEntity.Id,
                        DepotId = depotId,
                        RequestItemId = ammoDto.RequestItemId,
                        ReturnedQuantity = ammoDto.ReturnedQuantity ?? ammoDto.Quantity,
                        ReceivedQuantity = ammoDto.Quantity,
                        Lot = null,
                        BatchNumber = null,
                        SerialNumber = null,
                        Notes = receiptNotes,
                        InventoryDetailId = null,
                        AssetId = null,
                        CreationDate = now,
                        CreatedBy = userId,
                        IsDeleted = false
                    };

                    line = await _returnTrackingLineRepository.AddAsync(line);
                }

                foreach (var (weaponDto, assetId) in weaponReceipts)
                {
                    var batchNumber = (weaponDto.BatchNumber ?? string.Empty).Trim();
                    var serialNumber = (weaponDto.SerialNumber ?? string.Empty).Trim();
                    var receiptNotes = string.IsNullOrWhiteSpace(weaponDto.Notes) ? null : weaponDto.Notes.Trim();

                    var line = new ReturnTrackingLine
                    {
                        ReturnId = returnEntity.Id,
                        RequestId = returnEntity.Id,
                        DepotId = depotId,
                        RequestItemId = weaponDto.RequestItemId,
                        ReturnedQuantity = 1,
                        ReceivedQuantity = 1,
                        Lot = null,
                        BatchNumber = string.IsNullOrEmpty(batchNumber) ? null : batchNumber,
                        SerialNumber = string.IsNullOrEmpty(serialNumber) ? null : serialNumber,
                        Notes = receiptNotes,
                        AssetId = assetId,
                        InventoryDetailId = null,
                        CreationDate = now,
                        CreatedBy = userId,
                        IsDeleted = false
                    };

                    line = await _returnTrackingLineRepository.AddAsync(line);
                }

                var currentStep = await _mediator.Send(new GetCurrentApprovalStepByRequestIdQuery(returnEntity.Id));
                if (currentStep == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "No current workflow step found for this return.");
                }

                if (files != null && files.Count > 0)
                {
                    var stepUpload = await _fileUploadService.UploadFilesForEntityAsync(
                        files,
                        FileEntityType.WorkflowApproval,
                        currentStep.Id);
                    if (!stepUpload.Succeeded)
                    {
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                            stepUpload.Message ?? "Failed to upload workflow step attachments.");
                    }
                }

                var completionComment = string.IsNullOrWhiteSpace(dto.WorkflowStepComments)
                    ? "Return items processed"
                    : dto.WorkflowStepComments.Trim();

                // Approve workflow and complete the return
                try
                {
                    var approveDto = new ApproveRejectWorkflowApprovalDto
                    {
                        BaseRequestID = returnEntity.Id,
                        Action = RequestStatus.Approved,
                        IsApproved = true,
                        Comments = completionComment,
                        SendToHigherApproval = false
                    };

                    var approveResult = await _mediator.Send(new ProcessWorkflowActionCommand(approveDto, null));
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

        public async Task<APIOperationResponse<List<ReturnTrackingLineDto>>> GetReturnTrackingLinesAsync(long returnId)
        {
            _logger.LogInformation("Getting return tracking lines. ReturnId: {ReturnId}, User: {UserId}", returnId, _currentUserService.UserId);

            try
            {
                var returnEntity = await _returnRepository.FindOneAsync(r => r.Id == returnId && !r.IsDeleted);
                if (returnEntity == null)
                {
                    return APIOperationResponse<List<ReturnTrackingLineDto>>.Fail(ResponseType.NotFound, "Return not found");
                }

                var lines = (await _returnTrackingLineRepository.FindAsync(
                    l => l.ReturnId == returnId && !l.IsDeleted,
                    false,
                    nameof(ReturnTrackingLine.Depot),
                    $"{nameof(ReturnTrackingLine.RequestItem)}.{nameof(RequestItem.Item)}")).OrderBy(l => l.Id).ToList();

                var ids = lines.Select(l => l.Id).ToList();
                Dictionary<long, List<FileUploadDto>> filesByLineId = new();
                if (ids.Count > 0)
                {
                    var filesResult = await _fileUploadService.GetByEntitiesAsync(FileEntityType.ReturnTrackingLine, ids);
                    if (filesResult.Succeeded && filesResult.Data != null)
                    {
                        filesByLineId = filesResult.Data;
                    }
                }

                var dtos = lines.Select(l => new ReturnTrackingLineDto
                {
                    Id = l.Id,
                    ReturnId = l.ReturnId,
                    RequestId = l.RequestId,
                    DepotId = l.DepotId,
                    RequestItemId = l.RequestItemId,
                    ItemName = l.RequestItem?.Item?.Name,
                    ItemNo = l.RequestItem?.Item?.ItemNo,
                    ReturnedQuantity = l.ReturnedQuantity,
                    ReceivedQuantity = l.ReceivedQuantity,
                    Lot = l.Lot,
                    BatchNumber = l.BatchNumber,
                    SerialNumber = l.SerialNumber,
                    Notes = l.Notes,
                    AssetId = l.AssetId,
                    InventoryDetailId = l.InventoryDetailId,
                    Depot = l.Depot != null ? _mapper.Map<DepotDto>(l.Depot) : null,
                    Files = filesByLineId.TryGetValue(l.Id, out var f) ? f : new List<FileUploadDto>()
                }).ToList();

                return APIOperationResponse<List<ReturnTrackingLineDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting return tracking lines. ReturnId: {ReturnId}", returnId);
                return APIOperationResponse<List<ReturnTrackingLineDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private async Task ClearAssetAssignmentOnWeaponReturnAsync(
            Asset asset,
            string? returnNotes,
            long returnId,
            string requestNo,
            string depotName)
        {
            if (!asset.IsAssigned && !asset.CurrentAssignmentId.HasValue)
            {
                return;
            }

            AssetAssignment? assignment = null;
            if (asset.CurrentAssignmentId.HasValue)
            {
                assignment = await _assignmentRepository.FindOneAsync(
                    a => a.Id == asset.CurrentAssignmentId.Value
                        && a.AssetId == asset.Id
                        && !a.IsDeleted);
            }

            long? previousDepartmentId = null;
            long? previousCustodianId = null;
            string? previousLocation = null;
            long? assignmentId = null;
            long? orderId = null;

            if (assignment != null && assignment.Status == AssetAssignmentStatus.Active)
            {
                previousDepartmentId = assignment.DepartmentId;
                previousCustodianId = assignment.CustodianId;
                previousLocation = assignment.Location;
                assignmentId = assignment.Id;
                orderId = assignment.OrderId;

                assignment.Status = AssetAssignmentStatus.Returned;
                assignment.ActualReturnDate = _dateTimeProvider.Now;
                if (!string.IsNullOrWhiteSpace(returnNotes))
                {
                    assignment.Notes = string.IsNullOrEmpty(assignment.Notes)
                        ? returnNotes
                        : $"{assignment.Notes}\n{returnNotes}";
                }

                assignment.ModificationDate = _dateTimeProvider.Now;
                assignment.ModifiedBy = _currentUserService.UserId;
                await _assignmentRepository.UpdateAsync(assignment);
            }

            asset.IsAssigned = false;
            asset.CurrentAssignmentId = null;

            await _assetHistoryService.RecordHistoryAsync(asset.Id, AssetHistoryActionType.Returned, new AssetHistoryContext
            {
                Description = $"Weapon returned to depot {depotName} via return request {requestNo}",
                PreviousDepartmentId = previousDepartmentId,
                PreviousCustodianId = previousCustodianId,
                PreviousLocation = previousLocation,
                AssetAssignmentId = assignmentId,
                OrderId = orderId,
                Notes = returnNotes,
                Metadata = JsonSerializer.Serialize(new { ReturnId = returnId, RequestNo = requestNo, DepotName = depotName })
            });

            _logger.LogInformation(
                "Cleared weapon assignment on return. AssetId: {AssetId}, AssignmentId: {AssignmentId}, ReturnId: {ReturnId}, User: {UserId}",
                asset.Id, assignmentId, returnId, _currentUserService.UserId);
        }

        private async Task<APIOperationResponse<bool>?> ValidateRequestItemBelongsToReturnAsync(long returnId, long? requestItemId)
        {
            if (!requestItemId.HasValue)
            {
                return null;
            }

            var ri = await _requestItemRepository.FindOneAsync(
                r => r.Id == requestItemId.Value && r.RequestId == returnId && !r.IsDeleted);

            if (ri == null)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                    $"Request item {requestItemId.Value} is not part of this return.");
            }

            return null;
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
