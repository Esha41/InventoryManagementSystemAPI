using AutoMapper;
using FluentValidation;
using Ettad.CrossCutting.Data.Repository;
using Entities = Ettad.Data.Entities;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Ettad.RequestManagement.Service.RequestRecivers.Dtos;

namespace Ettad.RequestManagement.Service.RequestRecivers
{
    public class RequestReciverService : IRequestReciverService
    {
        private readonly ICrossCuttingRepository<Entities.RequestReciver> _requestReciverRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateRequestReciverDto> _validator;
        private readonly ICurrentUserService _currentUserService;

        public RequestReciverService(
            ICrossCuttingRepository<Entities.RequestReciver> requestReciverRepository,
            IMapper mapper,
            IValidator<CreateUpdateRequestReciverDto> validator,
            ICurrentUserService currentUserService)
        {
            _requestReciverRepository = requestReciverRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
        }

        public async Task<APIOperationResponse<RequestReciverDto>> GetByIdAsync(long id)
        {
            try
            {
                var requestReciver = await _requestReciverRepository.FindOneAsync(
                    r => r.Id == id,
                    false,
                    nameof(Entities.RequestReciver.Rank)
                );

                if (requestReciver == null)
                    return APIOperationResponse<RequestReciverDto>.Fail(ResponseType.NotFound, "Request receiver not found");

                var dto = _mapper.Map<RequestReciverDto>(requestReciver);
                return APIOperationResponse<RequestReciverDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<RequestReciverDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<RequestReciverDto>>> GetAllAsync()
        {
            try
            {
                var requestRecivers = await _requestReciverRepository.FindAsync(
                    r => true,
                    false,
                    nameof(Entities.RequestReciver.Rank)
                );

                var dtos = _mapper.Map<List<RequestReciverDto>>(requestRecivers);
                return APIOperationResponse<List<RequestReciverDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<RequestReciverDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<RequestReciverDto>> CreateAsync(CreateUpdateRequestReciverDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<RequestReciverDto>.Fail(ResponseType.BadRequest, errors);
                }

                var requestReciver = _mapper.Map<Entities.RequestReciver>(inputDto);
                requestReciver.CreationDate = DateTime.UtcNow;
                requestReciver.CreatedBy = _currentUserService.UserId;

                var createdRequestReciver = await _requestReciverRepository.AddAsync(requestReciver);

                // Reload with navigation properties
                var result = await _requestReciverRepository.FindOneAsync(
                    r => r.Id == createdRequestReciver.Id,
                    false,
                    nameof(Entities.RequestReciver.Rank)
                );

                var dto = _mapper.Map<RequestReciverDto>(result);
                return APIOperationResponse<RequestReciverDto>.Success(dto, "Request receiver created successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<RequestReciverDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<RequestReciverDto>> UpdateAsync(long id, CreateUpdateRequestReciverDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<RequestReciverDto>.Fail(ResponseType.BadRequest, errors);
                }

                // Check if request receiver exists
                var existingRequestReciver = await _requestReciverRepository.FindOneAsync(r => r.Id == id);
                if (existingRequestReciver == null)
                    return APIOperationResponse<RequestReciverDto>.Fail(ResponseType.NotFound, "Request receiver not found");

                // Map updates to entity
                _mapper.Map(inputDto, existingRequestReciver);
                existingRequestReciver.ModificationDate = DateTime.UtcNow;
                existingRequestReciver.ModifiedBy = _currentUserService.UserId;

                await _requestReciverRepository.UpdateAsync(existingRequestReciver);

                // Reload with navigation properties
                var result = await _requestReciverRepository.FindOneAsync(
                    r => r.Id == id,
                    false,
                    nameof(Entities.RequestReciver.Rank)
                );

                var dto = _mapper.Map<RequestReciverDto>(result);
                return APIOperationResponse<RequestReciverDto>.Success(dto, "Request receiver updated successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<RequestReciverDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            try
            {
                var requestReciver = await _requestReciverRepository.FindOneAsync(r => r.Id == id);
                if (requestReciver == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Request receiver not found");

                await _requestReciverRepository.DeleteAsync(requestReciver);

                return APIOperationResponse<bool>.Success(true, "Request receiver deleted successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}

