using AutoMapper;
using FluentValidation;
using Ettad.CrossCutting.Data.Repository;
using Entities = Ettad.Data.Entities;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Ettad.RequestManagement.Service.Requests.Dtos;

namespace Ettad.RequestManagement.Service.Requests
{
    public class RequestServices :IRequestService
    {
        private readonly ICrossCuttingRepository<Entities.Request> _requestRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateRequestDto> _validator;
        private readonly ICurrentUserService _currentUserService;

        public RequestServices(
            ICrossCuttingRepository<Entities.Request> requestRepository,
            IMapper mapper,
            IValidator<CreateUpdateRequestDto> validator,
            ICurrentUserService currentUserService)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
        }

        public async Task<APIOperationResponse<RequestDto>> GetByIdAsync(long id)
        {
            try
            {
                var request = await _requestRepository.FindOneAsync(
                    r => r.Id == id,
                    false,
                    nameof(Entities.Request.Depo),
                    nameof(Entities.Request.Department),
                    nameof(Entities.Request.RequestReciver),
                    nameof(Entities.Request.ResquestDetails)
                );

                if (request == null)
                    return APIOperationResponse<RequestDto>.Fail(ResponseType.NotFound, "Request not found");

                var dto = _mapper.Map<RequestDto>(request);
                return APIOperationResponse<RequestDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<RequestDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<RequestDto>>> GetAllAsync()
        {
            try
            {
                var requests = await _requestRepository.FindAsync(
                    r => true,
                    false,
                    nameof(Entities.Request.Depo),
                    nameof(Entities.Request.Department),
                    nameof(Entities.Request.RequestReciver),
                    nameof(Entities.Request.ResquestDetails)
                );

                var dtos = _mapper.Map<List<RequestDto>>(requests);
                return APIOperationResponse<List<RequestDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<RequestDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<RequestDto>> CreateAsync(CreateUpdateRequestDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<RequestDto>.Fail(ResponseType.BadRequest, errors);
                }

                // Map DTO to entity
                var request = _mapper.Map<Entities.Request>(inputDto);
                request.CreationDate = DateTime.UtcNow;
                request.CreatedBy = _currentUserService.UserId;

                // Add to repository
                var createdRequest = await _requestRepository.AddAsync(request);

                // Reload with navigation properties
                var result = await _requestRepository.FindOneAsync(
                    r => r.Id == createdRequest.Id,
                    false,
                    nameof(Entities.Request.Depo),
                    nameof(Entities.Request.Department),
                    nameof(Entities.Request.RequestReciver),
                    nameof(Entities.Request.ResquestDetails)
                );

                var dto = _mapper.Map<RequestDto>(result);
                return APIOperationResponse<RequestDto>.Success(dto, "Request created successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<RequestDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<RequestDto>> UpdateAsync(long id, CreateUpdateRequestDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<RequestDto>.Fail(ResponseType.BadRequest, errors);
                }

                // Check if request exists
                var existingRequest = await _requestRepository.FindOneAsync(r => r.Id == id);
                if (existingRequest == null)
                    return APIOperationResponse<RequestDto>.Fail(ResponseType.NotFound, "Request not found");

                // Map updates to entity
                _mapper.Map(inputDto, existingRequest);
                existingRequest.ModificationDate = DateTime.UtcNow;
                existingRequest.ModifiedBy = _currentUserService.UserId;

                // Update in repository
                await _requestRepository.UpdateAsync(existingRequest);

                // Reload with navigation properties
                var result = await _requestRepository.FindOneAsync(
                    r => r.Id == id,
                    false,
                    nameof(Entities.Request.Depo),
                    nameof(Entities.Request.Department),
                    nameof(Entities.Request.RequestReciver),
                    nameof(Entities.Request.ResquestDetails)
                );

                var dto = _mapper.Map<RequestDto>(result);
                return APIOperationResponse<RequestDto>.Success(dto, "Request updated successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<RequestDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            try
            {
                var request = await _requestRepository.FindOneAsync(r => r.Id == id);
                if (request == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Request not found");

                // Hard delete - Request entity doesn't have IsDeleted property
                await _requestRepository.DeleteAsync(request);

                return APIOperationResponse<bool>.Success(true, "Request deleted successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
