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

        public DiscardService(
            ICrossCuttingRepository<Discard> discardRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            ICrossCuttingRepository<RequestPurpose> requestPurposeRepository,
            IMapper mapper,
            IValidator<CreateDiscardDto> createValidator,
            ICurrentUserService currentUserService,
            IRequestNoGeneratorService requestNoGeneratorService,
            INotificationHelperService notificationHelperService)
        {
            _discardRepository = discardRepository;
            _requestItemRepository = requestItemRepository;
            _requestPurposeRepository = requestPurposeRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _currentUserService = currentUserService;
            _requestNoGeneratorService = requestNoGeneratorService;
            _notificationHelperService = notificationHelperService;
        }

        public async Task<APIOperationResponse<DiscardDto>> GetByIdAsync(long id)
        {
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
                    return APIOperationResponse<DiscardDto>.Fail(ResponseType.NotFound, "Discard not found");

                var dto = _mapper.Map<DiscardDto>(discard);
                return APIOperationResponse<DiscardDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<DiscardDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<DiscardDto>>> GetAllAsync()
        {
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
                return APIOperationResponse<List<DiscardDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<DiscardDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateDiscardDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _createValidator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                // Validate that RequestPurposeId belongs to a RequestPurpose with type Discard
                var requestPurpose = await _requestPurposeRepository.FindOneAsync(
                    rp => rp.Id == inputDto.RequestPurposeId && rp.RequestType == RequestType.Discard && !rp.IsDeleted
                );

                if (requestPurpose == null)
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "Request purpose must be of type Discard");

                // Map DTO to entity
                var discard = _mapper.Map<Discard>(inputDto);
                
                // Generate RequestNo
                discard.RequestNo = await _requestNoGeneratorService.GenerateRequestNoAsync(RequestType.Discard, inputDto.DepartmentId);
                
                discard.RequestType = RequestType.Discard;
                discard.Status = RequestStatus.New; // Always set to New when creating
                discard.CreationDate = DateTime.UtcNow;
                discard.CreatedBy = _currentUserService.UserId;

                // Map discard items
                discard.RequestItems = inputDto.DiscardItems
                    .Select(item =>
                    {
                        var requestItem = _mapper.Map<RequestItem>(item);
                        return requestItem;
                    })
                    .ToList();

                // Add to repository
                var createdDiscard = await _discardRepository.AddAsync(discard);

                await NotifyDiscardAsync(
                    "Discard Created",
                    $"Discard request {createdDiscard.RequestNo} has been created.",
                    createdDiscard.Id);

                return APIOperationResponse<long>.Success(createdDiscard.Id, "Discard created successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> ChangePriorityAsync(long id, RequestPriority priority)
        {
            try
            {
                // Check if discard exists
                var existingDiscard = await _discardRepository.FindOneAsync(
                    d => d.Id == id && !d.IsDeleted
                );

                if (existingDiscard == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Discard not found");

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

                return APIOperationResponse<bool>.Success(true, "Discard priority updated successfully");
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
                var discard = await _discardRepository.FindOneAsync(d => d.Id == id && !d.IsDeleted);
                if (discard == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Discard not found");

                // Soft delete - interceptor will handle IsDeleted, DeletionDate, and DeletedBy automatically
                await _discardRepository.DeleteAsync(discard);

                await NotifyDiscardAsync(
                    "Discard Deleted",
                    $"Discard request {discard.RequestNo} has been deleted.",
                    discard.Id);

                return APIOperationResponse<bool>.Success(true, "Discard deleted successfully");
            }
            catch (Exception ex)
            {
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
    }
}

