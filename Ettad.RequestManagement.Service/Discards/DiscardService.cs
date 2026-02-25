using System.Collections.Generic;
using AutoMapper;
using FluentValidation;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.Discards.Dtos;
using Ettad.RequestManagement.Service.Common;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Ettad.Notification.Service;
using Microsoft.AspNetCore.Identity;
using Ettad.Comman.Idenitity;
using Microsoft.Extensions.Logging;
using Ettad.Workflows.Service.Interface;
using Microsoft.AspNetCore.Http;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Comman.Enums;
using Ettad.CrossCutting.Comman.Time;

namespace Ettad.RequestManagement.Service.Discards
{
    public class DiscardService : IDiscardService
    {
        private readonly ICrossCuttingRepository<Discard> _discardRepository;
        private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;
        private readonly ICrossCuttingRepository<RequestPurpose> _requestPurposeRepository;
        private readonly IWorkflowApprovalService _workflowApprovalService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDiscardDto> _createValidator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRequestNoGeneratorService _requestNoGeneratorService;
        private readonly INotificationHelperService _notificationHelperService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DiscardService> _logger;
        private readonly IFileUploadService _fileUploadService;
        private readonly ICrossCuttingRepository<FileUplodDetails> _fileDetailsRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DiscardService(
            ICrossCuttingRepository<Discard> discardRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            ICrossCuttingRepository<RequestPurpose> requestPurposeRepository,
            IWorkflowApprovalService workflowApprovalService,
            IMapper mapper,
            IValidator<CreateDiscardDto> createValidator,
            ICurrentUserService currentUserService,
            IRequestNoGeneratorService requestNoGeneratorService,
            INotificationHelperService notificationHelperService,
            UserManager<ApplicationUser> userManager,
            ILogger<DiscardService> logger,
            IFileUploadService fileUploadService,
            ICrossCuttingRepository<FileUplodDetails> fileDetailsRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _discardRepository = discardRepository;
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
        }

        public async Task<APIOperationResponse<DiscardDto>> GetByIdAsync(long id)
        {
            _logger.LogInformation("Getting discard by ID: {DiscardId}. User: {UserId}", id, _currentUserService.UserId);
            
            try
            {
                var discard = await _discardRepository.FindOneAsync(
                    d => d.Id == id && !d.IsDeleted,
                    false,
                    nameof(BaseRequest.Department),
                    nameof(BaseRequest.Requester),
                    nameof(BaseRequest.RequestPurpose),
                    $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}"
                );

                if (discard == null)
                {
                    _logger.LogWarning("Discard not found. DiscardId: {DiscardId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<DiscardDto>.Fail(ResponseType.NotFound, "Discard not found");
                }

                var dto = _mapper.Map<DiscardDto>(discard);
                
                
                _logger.LogInformation("Successfully retrieved discard. DiscardId: {DiscardId}, RequestNo: {RequestNo}", id, discard.RequestNo);
                return APIOperationResponse<DiscardDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting discard by ID. DiscardId: {DiscardId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<DiscardDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<DiscardDto>>> GetAllAsync()
        {
            _logger.LogInformation("Getting all discards. User: {UserId}", _currentUserService.UserId);
            
            try
            {
                var discards = await _discardRepository.FindAsync(
                    d => !d.IsDeleted && d.RequestType == RequestType.Discard,
                    false,
                    nameof(BaseRequest.Department),
                    nameof(BaseRequest.Requester),
                    nameof(BaseRequest.RequestPurpose),
                    $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}"
                );

                var dtos = _mapper.Map<List<DiscardDto>>(discards);
                
                _logger.LogInformation("Successfully retrieved {DiscardCount} discards. User: {UserId}", dtos.Count, _currentUserService.UserId);
                return APIOperationResponse<List<DiscardDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all discards. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<DiscardDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateDiscardDto inputDto)
        {
            return await CreateAsync(inputDto, null);
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateDiscardDto inputDto, List<IFormFile> files)
        {
            var currentUserId = _currentUserService.UserId;
            var departmentId = _currentUserService.DepartmentId;

            if (!departmentId.HasValue || departmentId.Value <= 0)
            {
                _logger.LogWarning("Cannot create discard. No department assigned to current user. User: {UserId}", currentUserId);
                return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "Department not found for current user.");
            }

            _logger.LogInformation("Creating new discard. DepartmentId: {DepartmentId}, RequestPurposeId: {RequestPurposeId}, User: {UserId}, HasFiles: {HasFiles}", 
                departmentId.Value, inputDto.RequestPurposeId, currentUserId, files != null && files.Count > 0);
            
            try
            {
                // Validate input
                var validationResult = await _createValidator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    _logger.LogWarning("Discard validation failed. DepartmentId: {DepartmentId}, Errors: {ValidationErrors}, User: {UserId}", 
                        departmentId.Value, errors, currentUserId);
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                // Validate that RequestPurposeId belongs to a RequestPurpose with type Discard
                var requestPurpose = await _requestPurposeRepository.FindOneAsync(
                    rp => rp.Id == inputDto.RequestPurposeId && rp.RequestType == RequestType.Discard && !rp.IsDeleted
                );

                if (requestPurpose == null)
                {
                    _logger.LogWarning("Invalid request purpose for discard. RequestPurposeId: {RequestPurposeId}, User: {UserId}", 
                        inputDto.RequestPurposeId, _currentUserService.UserId);
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "Request purpose must be of type Discard");
                }

                // Step 1: Save files first (before creating discard) to create FileUplodMaster records
                // When files are selected: if file upload fails or throws an exception, do not create the discard.
                List<long> savedFileMasterIds = null;
                if (files != null && files.Count > 0)
                {
                    try
                    {
                        // Use FileEntityType.Order for discard files (no Discard FileEntityType exists)
                        var saveFilesResult = await _fileUploadService.SaveFilesAsync(files, FileEntityType.Order);
                        if (!saveFilesResult.Succeeded || saveFilesResult.Data == null)
                        {
                            _logger.LogWarning("Failed to save files before creating discard. Error: {Error}, User: {UserId}",
                                saveFilesResult.Message ?? "Unknown error", currentUserId);
                            return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                                saveFilesResult.Message ?? "File upload failed. Discard was not created.");
                        }
                        savedFileMasterIds = saveFilesResult.Data;
                        _logger.LogInformation("Files saved successfully before discard creation. FileCount: {FileCount}, MasterIds: {MasterIds}, User: {UserId}",
                            files.Count, string.Join(", ", savedFileMasterIds), currentUserId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Exception during file upload before discard creation. User: {UserId}", currentUserId);
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                            $"File upload failed: {ex.Message}. Discard was not created.");
                    }
                }

                // Step 2: Map DTO to entity
                var discard = _mapper.Map<Discard>(inputDto);
                discard.DepartmentId = departmentId.Value;
                
                // Generate RequestNo
                discard.RequestNo = await _requestNoGeneratorService.GenerateRequestNoAsync(RequestType.Discard, departmentId.Value);
                
                _logger.LogInformation("Generated RequestNo: {RequestNo} for discard", discard.RequestNo);
                
                discard.RequestType = RequestType.Discard;
                discard.Status = RequestStatus.New; // Always set to New when creating
                discard.CreationDate = _dateTimeProvider.Now;
                discard.CreatedBy = currentUserId;
                discard.RequesterId = currentUserId;

                // Map discard items
                discard.RequestItems = inputDto.DiscardItems
                    .Select(item =>
                    {
                        var requestItem = _mapper.Map<RequestItem>(item);
                        return requestItem;
                    })
                    .ToList();

                _logger.LogInformation("Adding {ItemCount} discard items to discard. RequestNo: {RequestNo}", 
                    discard.RequestItems.Count, discard.RequestNo);

                // Step 3: Add to repository
                var createdDiscard = await _discardRepository.AddAsync(discard);

                // Step 4: Link files to the discard (create FileUplodDetails records) if files were saved
                // When files were selected: if linking fails, delete the discard and do not succeed.
                if (savedFileMasterIds != null && savedFileMasterIds.Count > 0)
                {
                    try
                    {
                        foreach (var masterId in savedFileMasterIds)
                        {
                            var detail = new FileUplodDetails
                            {
                                FileUplodMasterId = masterId,
                                Entity = FileEntityType.Order, // Use Order for discard files
                                EntityId = createdDiscard.Id
                            };

                            await _fileDetailsRepository.AddAsync(detail);
                        }

                        _logger.LogInformation("Files linked successfully to discard. DiscardId: {DiscardId}, FileCount: {FileCount}, User: {UserId}",
                            createdDiscard.Id, savedFileMasterIds.Count, currentUserId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Exception occurred while linking files to discard. DiscardId: {DiscardId}, User: {UserId}. Rolling back discard creation.",
                            createdDiscard.Id, currentUserId);
                        await _discardRepository.DeleteAsync(createdDiscard);
                        return APIOperationResponse<long>.Fail(ResponseType.BadRequest,
                            $"File upload failed: {ex.Message}. Discard was not created.");
                    }
                }

                // Start workflow for the discard
                var workflowStarted = await _workflowApprovalService.StartWorkflowAsync(
                    createdDiscard.Id,
                    WorkflowType.Discard);

                if (workflowStarted)
                {
                    _logger.LogInformation("Workflow started successfully for discard. DiscardId: {DiscardId}, User: {UserId}",
                        createdDiscard.Id, currentUserId);
                }
                else
                {
                    _logger.LogWarning("Failed to start workflow for discard. DiscardId: {DiscardId}, User: {UserId}",
                        createdDiscard.Id, currentUserId);
                }

                await NotifyDiscardAsync(
                    "Discard Created",
                    $"Discard request {createdDiscard.RequestNo} has been created.",
                    createdDiscard.Id);

                _logger.LogInformation("Discard created successfully. DiscardId: {DiscardId}, RequestNo: {RequestNo}, ItemCount: {ItemCount}, User: {UserId}", 
                    createdDiscard.Id, createdDiscard.RequestNo, createdDiscard.RequestItems?.Count ?? 0, currentUserId);
                
                return APIOperationResponse<long>.Success(createdDiscard.Id, "Discard created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating discard. DepartmentId: {DepartmentId}, User: {UserId}", 
                    departmentId.Value, currentUserId);
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> ChangePriorityAsync(long id, RequestPriority priority)
        {
            _logger.LogInformation("Changing discard priority. DiscardId: {DiscardId}, NewPriority: {Priority}, User: {UserId}", 
                id, priority, _currentUserService.UserId);
            
            try
            {
                // Check if discard exists
                var existingDiscard = await _discardRepository.FindOneAsync(
                    d => d.Id == id && !d.IsDeleted
                );

                if (existingDiscard == null)
                {
                    _logger.LogWarning("Discard not found for priority change. DiscardId: {DiscardId}, User: {UserId}", 
                        id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Discard not found");
                }

                var oldPriority = existingDiscard.Priority;
                
                // Update priority
                existingDiscard.Priority = priority;
                existingDiscard.ModificationDate = _dateTimeProvider.Now;
                existingDiscard.ModifiedBy = _currentUserService.UserId;

                // Update in repository
                await _discardRepository.UpdateAsync(existingDiscard);

                await NotifyDiscardAsync(
                    "Discard Priority Updated",
                    $"Discard request {existingDiscard.RequestNo} priority changed to {priority}.",
                    existingDiscard.Id);

                _logger.LogInformation("Discard priority updated successfully. DiscardId: {DiscardId}, RequestNo: {RequestNo}, OldPriority: {OldPriority}, NewPriority: {NewPriority}, User: {UserId}", 
                    id, existingDiscard.RequestNo, oldPriority, priority, _currentUserService.UserId);
                
                return APIOperationResponse<bool>.Success(true, "Discard priority updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing discard priority. DiscardId: {DiscardId}, Priority: {Priority}, User: {UserId}", 
                    id, priority, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            _logger.LogInformation("Deleting discard. DiscardId: {DiscardId}, User: {UserId}", id, _currentUserService.UserId);
            
            try
            {
                var discard = await _discardRepository.FindOneAsync(d => d.Id == id && !d.IsDeleted);
                if (discard == null)
                {
                    _logger.LogWarning("Discard not found for deletion. DiscardId: {DiscardId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Discard not found");
                }

                // Soft delete - interceptor will handle IsDeleted, DeletionDate, and DeletedBy automatically
                await _discardRepository.DeleteAsync(discard);

                await NotifyDiscardAsync(
                    "Discard Deleted",
                    $"Discard request {discard.RequestNo} has been deleted.",
                    discard.Id);

                _logger.LogInformation("Discard deleted successfully. DiscardId: {DiscardId}, RequestNo: {RequestNo}, User: {UserId}", 
                    id, discard.RequestNo, _currentUserService.UserId);
                
                return APIOperationResponse<bool>.Success(true, "Discard deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting discard. DiscardId: {DiscardId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private async Task NotifyDiscardAsync(string title, string message, long entityId)
        {
            try
            {
                var userId = _currentUserService.UserId;
                var userIds = string.IsNullOrWhiteSpace(userId) ? null : new List<string> { userId };

                await _notificationHelperService.SendNotificationAndEmailAsync(
                    title,
                    message,
                    entityType: nameof(Discard),
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
