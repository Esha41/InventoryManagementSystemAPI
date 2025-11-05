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
        private readonly ICrossCuttingRepository<Entities.RequestDetail> _requestDetailRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateRequestDto> _validator;
        private readonly ICurrentUserService _currentUserService;

        public RequestServices(
            ICrossCuttingRepository<Entities.Request> requestRepository,
            ICrossCuttingRepository<Entities.RequestDetail> requestDetailRepository,
            IMapper mapper,
            IValidator<CreateUpdateRequestDto> validator,
            ICurrentUserService currentUserService)
        {
            _requestRepository = requestRepository;
            _requestDetailRepository = requestDetailRepository;
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
                    nameof(Entities.Request.RequesterRank),
                    $"{nameof(Entities.Request.ResquestDetails)}.{nameof(Entities.RequestDetail.Item)}"
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
                    nameof(Entities.Request.RequesterRank),
                    $"{nameof(Entities.Request.ResquestDetails)}.{nameof(Entities.RequestDetail.Item)}"
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

                // Create RequestDetails if provided
                if (inputDto.RequestDetails != null && inputDto.RequestDetails.Any())
                {
                    var requestDetails = inputDto.RequestDetails.Select(rd => new Entities.RequestDetail
                    {
                        RequestId = createdRequest.Id,
                        ItemId = rd.ItemId,
                        ItemQuantity = rd.ItemQuantity
                    }).ToList();

                    foreach (var detail in requestDetails)
                    {
                        await _requestDetailRepository.AddAsync(detail);
                    }
                }

                // Reload with navigation properties
                var result = await _requestRepository.FindOneAsync(
                    r => r.Id == createdRequest.Id,
                    false,
                    nameof(Entities.Request.Depo),
                    nameof(Entities.Request.Department),
                    nameof(Entities.Request.RequestReciver),
                    nameof(Entities.Request.RequesterRank),
                    $"{nameof(Entities.Request.ResquestDetails)}.{nameof(Entities.RequestDetail.Item)}"
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

                // Handle RequestDetails update
                if (inputDto.RequestDetails != null)
                {
                    // Get existing request details
                    var existingDetails = await _requestDetailRepository.FindAsync(rd => rd.RequestId == id);
                    
                    // Delete existing details
                    foreach (var detail in existingDetails)
                    {
                        await _requestDetailRepository.DeleteAsync(detail);
                    }

                    // Create new details
                    if (inputDto.RequestDetails.Any())
                    {
                        var newRequestDetails = inputDto.RequestDetails.Select(rd => new Entities.RequestDetail
                        {
                            RequestId = id,
                            ItemId = rd.ItemId,
                            ItemQuantity = rd.ItemQuantity
                        }).ToList();

                        foreach (var detail in newRequestDetails)
                        {
                            await _requestDetailRepository.AddAsync(detail);
                        }
                    }
                }

                // Reload with navigation properties
                var result = await _requestRepository.FindOneAsync(
                    r => r.Id == id,
                    false,
                    nameof(Entities.Request.Depo),
                    nameof(Entities.Request.Department),
                    nameof(Entities.Request.RequestReciver),
                    nameof(Entities.Request.RequesterRank),
                    $"{nameof(Entities.Request.ResquestDetails)}.{nameof(Entities.RequestDetail.Item)}"
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

        public async Task<APIOperationResponse<bool>> SoftDeleteRequestDetailAsync(long requestId, long requestDetailId)
        {
            try
            {
                // Check if request exists
                var request = await _requestRepository.FindOneAsync(r => r.Id == requestId);
                if (request == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Request not found");

                // Find the specific request detail
                var requestDetail = await _requestDetailRepository.FindOneAsync(rd => rd.Id == requestDetailId && rd.RequestId == requestId);
                if (requestDetail == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Request detail not found");

                // Soft delete the request detail
                requestDetail.IsDeleted = true;
                requestDetail.DeletedDate = DateTime.UtcNow;
                requestDetail.DeletedBy = _currentUserService.UserId;
                await _requestDetailRepository.UpdateAsync(requestDetail);

                // Check if all request details are soft deleted
                var allDetails = await _requestDetailRepository.FindAsync(rd => rd.RequestId == requestId);
                
                // If no active details remain (all are soft deleted due to query filter), delete the request
                if (!allDetails.Any())
                {
                    await _requestRepository.DeleteAsync(request);
                    return APIOperationResponse<bool>.Success(true, "Request detail deleted and request removed because all details are deleted");
                }

                return APIOperationResponse<bool>.Success(true, "Request detail deleted successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
