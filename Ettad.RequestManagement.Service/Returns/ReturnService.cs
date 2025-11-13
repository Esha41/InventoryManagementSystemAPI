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
            ILogger<ReturnService> logger)
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
                    nameof(BaseRequest.Reciever),
                    nameof(BaseRequest.Depot),
                    nameof(BaseRequest.RequestPurpose),
                    $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}",
                    $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}.{nameof(BaseItem.Hcc)}"
                );

                if (returnEntity == null)
                {
                    _logger.LogWarning("Return not found. ReturnId: {ReturnId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<ReturnDto>.Fail(ResponseType.NotFound, "Return not found");
                }

                var dto = _mapper.Map<ReturnDto>(returnEntity);
                
                // Fallback: If RequesterName is null but we have a CreatedBy user, use that user's name
                if (string.IsNullOrEmpty(dto.RequesterName) && !string.IsNullOrEmpty(returnEntity.CreatedBy))
                {
                    dto.RequesterName = await GetUserNameByIdAsync(returnEntity.CreatedBy);
                }
                
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
                    nameof(BaseRequest.Reciever),
                    nameof(BaseRequest.Depot),
                    nameof(BaseRequest.RequestPurpose),
                    $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}",
                    $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}.{nameof(BaseItem.Hcc)}"
                );

                var dtos = _mapper.Map<List<ReturnDto>>(returns);
                
                // Fallback: Populate RequesterName from CreatedBy user if not set
                foreach (var dto in dtos)
                {
                    var returnEntity = returns.FirstOrDefault(r => r.Id == dto.Id);
                    if (returnEntity != null && string.IsNullOrEmpty(dto.RequesterName) && !string.IsNullOrEmpty(returnEntity.CreatedBy))
                    {
                        dto.RequesterName = await GetUserNameByIdAsync(returnEntity.CreatedBy);
                    }
                }
                
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
            var currentUserId = _currentUserService.UserId;
            var departmentId = _currentUserService.DepartmentId;

            if (!departmentId.HasValue || departmentId.Value <= 0)
            {
                _logger.LogWarning("Cannot create return. No department assigned to current user. User: {UserId}", currentUserId);
                return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "Department not found for current user.");
            }

            _logger.LogInformation("Creating new return. DepartmentId: {DepartmentId}, RequestPurposeId: {RequestPurposeId}, User: {UserId}", 
                departmentId.Value, inputDto.RequestPurposeId, currentUserId);
            
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

                // Map DTO to entity
                var returnEntity = _mapper.Map<Return>(inputDto);
                returnEntity.DepartmentId = departmentId.Value;
                
                // Generate RequestNo
                returnEntity.RequestNo = await _requestNoGeneratorService.GenerateRequestNoAsync(RequestType.Return, departmentId.Value);
                
                _logger.LogInformation("Generated RequestNo: {RequestNo} for return", returnEntity.RequestNo);
                
                returnEntity.RequestType = RequestType.Return;
                returnEntity.Status = RequestStatus.New; // Always set to New when creating
                returnEntity.CreationDate = DateTime.UtcNow;
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

                // Add to repository
                var createdReturn = await _returnRepository.AddAsync(returnEntity);

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
                existingReturn.ModificationDate = DateTime.UtcNow;
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

        private async Task NotifyReturnAsync(string title, string message, long entityId)
        {
            try
            {
                var userId = _currentUserService.UserId;
                var userIds = string.IsNullOrWhiteSpace(userId) ? null : new List<string> { userId };

                await _notificationHelperService.SendNotificationAsync(
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



