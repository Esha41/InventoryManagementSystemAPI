using AutoMapper;
using FluentValidation;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.Discards.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;

namespace Ettad.RequestManagement.Service.Discards
{
    public class DiscardService : IDiscardService
    {
        private readonly ICrossCuttingRepository<Discard> _discardRepository;
        private readonly ICrossCuttingRepository<RequestItem> _requestItemRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDiscardDto> _createValidator;
        private readonly IValidator<UpdateDiscardDto> _updateValidator;
        private readonly ICurrentUserService _currentUserService;

        public DiscardService(
            ICrossCuttingRepository<Discard> discardRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            IMapper mapper,
            IValidator<CreateDiscardDto> createValidator,
            IValidator<UpdateDiscardDto> updateValidator,
            ICurrentUserService currentUserService)
        {
            _discardRepository = discardRepository;
            _requestItemRepository = requestItemRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _currentUserService = currentUserService;
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

        public async Task<APIOperationResponse<DiscardDto>> CreateAsync(CreateDiscardDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _createValidator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<DiscardDto>.Fail(ResponseType.BadRequest, errors);
                }

                // Map DTO to entity
                var discard = _mapper.Map<Discard>(inputDto);
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

                // Reload with navigation properties
                var result = await _discardRepository.FindOneAsync(
                    d => d.Id == createdDiscard.Id,
                    false,
                    nameof(BaseRequest.Department),
                    nameof(BaseRequest.Requester),
                    nameof(BaseRequest.Reciever),
                    nameof(BaseRequest.Depot),
                    nameof(BaseRequest.RequestPurpose),
                    $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}",
                    $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}.{nameof(BaseItem.Hcc)}"
                );

                var dto = _mapper.Map<DiscardDto>(result);
                return APIOperationResponse<DiscardDto>.Success(dto, "Discard created successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<DiscardDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<DiscardDto>> UpdateAsync(long id, UpdateDiscardDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _updateValidator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<DiscardDto>.Fail(ResponseType.BadRequest, errors);
                }

                // Check if discard exists
                var existingDiscard = await _discardRepository.FindOneAsync(
                    d => d.Id == id && !d.IsDeleted,
                    false,
                    nameof(BaseRequest.RequestItems)
                );

                if (existingDiscard == null)
                    return APIOperationResponse<DiscardDto>.Fail(ResponseType.NotFound, "Discard not found");

                // Map updates to entity (excluding RequestItems)
                _mapper.Map(inputDto, existingDiscard);
                existingDiscard.ModificationDate = DateTime.UtcNow;
                existingDiscard.ModifiedBy = _currentUserService.UserId;

                // Handle discard items updates
                // Remove old items that are not in the update DTO
                var existingItemIds = inputDto.DiscardItems
                    .Where(item => item.Id.HasValue)
                    .Select(item => item.Id.Value)
                    .ToList();

                var itemsToRemove = existingDiscard.RequestItems
                    .Where(item => !existingItemIds.Contains(item.Id))
                    .ToList();

                foreach (var item in itemsToRemove)
                {
                    await _requestItemRepository.DeleteAsync(item);
                }

                // Update existing items and add new ones
                foreach (var itemDto in inputDto.DiscardItems)
                {
                    if (itemDto.Id.HasValue)
                    {
                        // Update existing item
                        var existingItem = existingDiscard.RequestItems.FirstOrDefault(i => i.Id == itemDto.Id.Value);
                        if (existingItem != null)
                        {
                            _mapper.Map(itemDto, existingItem);
                        }
                    }
                    else
                    {
                        // Add new item
                        var newItem = _mapper.Map<RequestItem>(itemDto);
                        newItem.RequestId = id;
                        existingDiscard.RequestItems.Add(newItem);
                    }
                }

                // Update in repository
                await _discardRepository.UpdateAsync(existingDiscard);

                // Reload with navigation properties
                var result = await _discardRepository.FindOneAsync(
                    d => d.Id == id,
                    false,
                    nameof(BaseRequest.Department),
                    nameof(BaseRequest.Requester),
                    nameof(BaseRequest.Reciever),
                    nameof(BaseRequest.Depot),
                    nameof(BaseRequest.RequestPurpose),
                    $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}",
                    $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}.{nameof(BaseItem.Hcc)}"
                );

                var dto = _mapper.Map<DiscardDto>(result);
                return APIOperationResponse<DiscardDto>.Success(dto, "Discard updated successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<DiscardDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
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

                return APIOperationResponse<bool>.Success(true, "Discard deleted successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}

