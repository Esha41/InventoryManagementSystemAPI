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

namespace Ettad.RequestManagement.Service.Discards
{
    public class DiscardService : IDiscardService
    {
        private readonly ICrossCuttingRepository<Discard> _discardRepository;
        private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;
        private readonly ICrossCuttingRepository<RequestPurpose> _requestPurposeRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDiscardDto> _createValidator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRequestNoGeneratorService _requestNoGeneratorService;
        private readonly INotificationHelperService _notificationHelperService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DiscardService> _logger;

        public DiscardService(
            ICrossCuttingRepository<Discard> discardRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            ICrossCuttingRepository<RequestPurpose> requestPurposeRepository,
            IMapper mapper,
            IValidator<CreateDiscardDto> createValidator,
            ICurrentUserService currentUserService,
            IRequestNoGeneratorService requestNoGeneratorService,
            INotificationHelperService notificationHelperService,
            UserManager<ApplicationUser> userManager,
            ILogger<DiscardService> logger)
        {
            _discardRepository = discardRepository;
            _requestItemRepository = requestItemRepository;
            _requestPurposeRepository = requestPurposeRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _currentUserService = currentUserService;
            _requestNoGeneratorService = requestNoGeneratorService;
            _notificationHelperService = notificationHelperService;
            _userManager = userManager;
            _logger = logger;
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
                    nameof(BaseRequest.Reciever),
                    nameof(BaseRequest.Depot),
                    nameof(BaseRequest.RequestPurpose),
                    $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}",
                    $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}.{nameof(BaseItem.Hcc)}"
                );

                if (discard == null)
                {
                    _logger.LogWarning("Discard not found. DiscardId: {DiscardId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<DiscardDto>.Fail(ResponseType.NotFound, "Discard not found");
                }

                var dto = _mapper.Map<DiscardDto>(discard);
                
                // Fallback: If RequesterName is null but we have a CreatedBy user, use that user's name
                if (string.IsNullOrEmpty(dto.RequesterName) && !string.IsNullOrEmpty(discard.CreatedBy))
                {
                    dto.RequesterName = await GetUserNameByIdAsync(discard.CreatedBy);
                }
                
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
                    nameof(BaseRequest.Reciever),
                    nameof(BaseRequest.Depot),
                    nameof(BaseRequest.RequestPurpose),
                    $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}",
                    $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}.{nameof(BaseItem.Hcc)}"
                );

                var dtos = _mapper.Map<List<DiscardDto>>(discards);
                
                // Fallback: Populate RequesterName from CreatedBy user if not set
                foreach (var dto in dtos)
                {
                    var discard = discards.FirstOrDefault(d => d.Id == dto.Id);
                    if (discard != null && string.IsNullOrEmpty(dto.RequesterName) && !string.IsNullOrEmpty(discard.CreatedBy))
                    {
                        dto.RequesterName = await GetUserNameByIdAsync(discard.CreatedBy);
                    }
                }
                
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
            var currentUserId = _currentUserService.UserId;
            var departmentId = _currentUserService.DepartmentId;

            if (!departmentId.HasValue || departmentId.Value <= 0)
            {
                _logger.LogWarning("Cannot create discard. No department assigned to current user. User: {UserId}", currentUserId);
                return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "Department not found for current user.");
            }

            _logger.LogInformation("Creating new discard. DepartmentId: {DepartmentId}, RequestPurposeId: {RequestPurposeId}, User: {UserId}", 
                departmentId.Value, inputDto.RequestPurposeId, currentUserId);
            
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

                // Map DTO to entity
                var discard = _mapper.Map<Discard>(inputDto);
                discard.DepartmentId = departmentId.Value;
                
                // Generate RequestNo
                discard.RequestNo = await _requestNoGeneratorService.GenerateRequestNoAsync(RequestType.Discard, departmentId.Value);
                
                _logger.LogInformation("Generated RequestNo: {RequestNo} for discard", discard.RequestNo);
                
                discard.RequestType = RequestType.Discard;
                discard.Status = RequestStatus.New; // Always set to New when creating
                discard.CreationDate = DateTime.UtcNow;
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

                // Add to repository
                var createdDiscard = await _discardRepository.AddAsync(discard);

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
                existingDiscard.ModificationDate = DateTime.UtcNow;
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

                await _notificationHelperService.SendNotificationAsync(
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

        /// <summary>
        /// Get user name by user ID from Identity system
        /// </summary>
        private async Task<string> GetUserNameByIdAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    // Try FullNameEN first, then FullNameAR, then UserName
                    if (!string.IsNullOrEmpty(user.FullNameEN))
                        return user.FullNameEN;
                    if (!string.IsNullOrEmpty(user.FullNameAR))
                        return user.FullNameAR;
                    if (!string.IsNullOrEmpty(user.UserName))
                        return user.UserName;
                }
                
                return "System User";
            }
            catch
            {
                return "System User";
            }
        }
    }
}

