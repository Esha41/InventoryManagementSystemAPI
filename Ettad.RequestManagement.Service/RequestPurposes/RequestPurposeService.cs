using AutoMapper;
using FluentValidation;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.RequestPurposes.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Time;

namespace Ettad.RequestManagement.Service.RequestPurposes
{
    public class RequestPurposeService : IRequestPurposeService
    {
        private readonly ICrossCuttingRepository<RequestPurpose> _requestPurposeRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateRequestPurposeDto> _validator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RequestPurposeService(
            ICrossCuttingRepository<RequestPurpose> requestPurposeRepository,
            IMapper mapper,
            IValidator<CreateUpdateRequestPurposeDto> validator,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _requestPurposeRepository = requestPurposeRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<APIOperationResponse<long>> CreateForDiscardAsync(CreateUpdateRequestPurposeDto inputDto)
        {
            return await CreateAsync(inputDto, RequestType.Discard);
        }

        public async Task<APIOperationResponse<long>> CreateForReturnAsync(CreateUpdateRequestPurposeDto inputDto)
        {
            return await CreateAsync(inputDto, RequestType.Return);
        }

        public async Task<APIOperationResponse<long>> CreateForOrderAsync(CreateUpdateRequestPurposeDto inputDto)
        {
            return await CreateAsync(inputDto, RequestType.Order);
        }

        private async Task<APIOperationResponse<long>> CreateAsync(CreateUpdateRequestPurposeDto inputDto, RequestType requestType)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, errors);
                }

                var requestPurpose = _mapper.Map<RequestPurpose>(inputDto);
                requestPurpose.RequestType = requestType;
                requestPurpose.CreationDate = _dateTimeProvider.Now;
                requestPurpose.CreatedBy = _currentUserService.UserId;

                var created = await _requestPurposeRepository.AddAsync(requestPurpose);
                return APIOperationResponse<long>.Success(created.Id, "Request purpose created successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<long>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<RequestPurposeDto>> GetByIdForDiscardAsync(long id)
        {
            return await GetByIdAsync(id, RequestType.Discard);
        }

        public async Task<APIOperationResponse<RequestPurposeDto>> GetByIdForReturnAsync(long id)
        {
            return await GetByIdAsync(id, RequestType.Return);
        }

        public async Task<APIOperationResponse<RequestPurposeDto>> GetByIdForOrderAsync(long id)
        {
            return await GetByIdAsync(id, RequestType.Order);
        }

        private async Task<APIOperationResponse<RequestPurposeDto>> GetByIdAsync(long id, RequestType requestType)
        {
            try
            {
                var requestPurpose = await _requestPurposeRepository.FindOneAsync(
                    rp => rp.Id == id && rp.RequestType == requestType && !rp.IsDeleted
                );

                if (requestPurpose == null)
                    return APIOperationResponse<RequestPurposeDto>.Fail(ResponseType.NotFound, "Request purpose not found");

                var dto = _mapper.Map<RequestPurposeDto>(requestPurpose);
                return APIOperationResponse<RequestPurposeDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<RequestPurposeDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<RequestPurposeDto>>> GetAllForDiscardAsync()
        {
            return await GetAllAsync(RequestType.Discard);
        }

        public async Task<APIOperationResponse<List<RequestPurposeDto>>> GetAllForReturnAsync()
        {
            return await GetAllAsync(RequestType.Return);
        }

        public async Task<APIOperationResponse<List<RequestPurposeDto>>> GetAllForOrderAsync()
        {
            return await GetAllAsync(RequestType.Order);
        }

        private async Task<APIOperationResponse<List<RequestPurposeDto>>> GetAllAsync(RequestType requestType)
        {
            try
            {
                var requestPurposes = await _requestPurposeRepository.FindAsync(
                    rp => rp.RequestType == requestType && !rp.IsDeleted
                );

                var dtos = _mapper.Map<List<RequestPurposeDto>>(requestPurposes);
                return APIOperationResponse<List<RequestPurposeDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<RequestPurposeDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateRequestPurposeDto inputDto)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                var existing = await _requestPurposeRepository.FindOneAsync(
                    rp => rp.Id == id && !rp.IsDeleted
                );

                if (existing == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Request purpose not found");

                _mapper.Map(inputDto, existing);
                existing.ModificationDate = _dateTimeProvider.Now;
                existing.ModifiedBy = _currentUserService.UserId;

                await _requestPurposeRepository.UpdateAsync(existing);
                return APIOperationResponse<bool>.Success(true, "Request purpose updated successfully");
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
                var requestPurpose = await _requestPurposeRepository.FindOneAsync(rp => rp.Id == id && !rp.IsDeleted);
                if (requestPurpose == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Request purpose not found");

                await _requestPurposeRepository.DeleteAsync(requestPurpose);
                return APIOperationResponse<bool>.Success(true, "Request purpose deleted successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}

