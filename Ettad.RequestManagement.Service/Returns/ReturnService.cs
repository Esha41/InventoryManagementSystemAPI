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
using Microsoft.AspNetCore.Identity;
using Ettad.Comman.Idenitity;

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
        private readonly UserManager<ApplicationUser> _userManager;

        public ReturnService(
            ICrossCuttingRepository<Return> returnRepository,
            ICrossCuttingRepository<RequestItem> requestItemRepository,
            ICrossCuttingRepository<RequestPurpose> requestPurposeRepository,
            IMapper mapper,
            IValidator<CreateReturnDto> createValidator,
            ICurrentUserService currentUserService,
            IRequestNoGeneratorService requestNoGeneratorService,
            UserManager<ApplicationUser> userManager)
        {
            _returnRepository = returnRepository;
            _requestItemRepository = requestItemRepository;
            _requestPurposeRepository = requestPurposeRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _currentUserService = currentUserService;
            _requestNoGeneratorService = requestNoGeneratorService;
            _userManager = userManager;
        }

        public async Task<APIOperationResponse<ReturnDto>> GetByIdAsync(long id)
        {
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
                    return APIOperationResponse<ReturnDto>.Fail(ResponseType.NotFound, "Return not found");

                var dto = _mapper.Map<ReturnDto>(returnEntity);
                
                // Fallback: If RequesterName is null but we have a CreatedBy user, use that user's name
                if (string.IsNullOrEmpty(dto.RequesterName) && !string.IsNullOrEmpty(returnEntity.CreatedBy))
                {
                    dto.RequesterName = await GetUserNameByIdAsync(returnEntity.CreatedBy);
                }
                
                return APIOperationResponse<ReturnDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<ReturnDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<ReturnDto>>> GetAllAsync()
        {
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
                
                return APIOperationResponse<List<ReturnDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<ReturnDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<long>> CreateAsync(CreateReturnDto inputDto)
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

                // Validate that RequestPurposeId belongs to a RequestPurpose with type Return
                var requestPurpose = await _requestPurposeRepository.FindOneAsync(
                    rp => rp.Id == inputDto.RequestPurposeId && rp.RequestType == RequestType.Return && !rp.IsDeleted
                );

                if (requestPurpose == null)
                    return APIOperationResponse<long>.Fail(ResponseType.BadRequest, "Request purpose must be of type Return");

                // Map DTO to entity
                var returnEntity = _mapper.Map<Return>(inputDto);
                
                // Generate RequestNo
                returnEntity.RequestNo = await _requestNoGeneratorService.GenerateRequestNoAsync(RequestType.Return, inputDto.DepartmentId);
                
                returnEntity.RequestType = RequestType.Return;
                returnEntity.Status = RequestStatus.New; // Always set to New when creating
                returnEntity.CreationDate = DateTime.UtcNow;
                returnEntity.CreatedBy = _currentUserService.UserId;

                // Map return items
                returnEntity.RequestItems = inputDto.ReturnItems
                    .Select(item =>
                    {
                        var requestItem = _mapper.Map<RequestItem>(item);
                        return requestItem;
                    })
                    .ToList();

                // Add to repository
                var createdReturn = await _returnRepository.AddAsync(returnEntity);

                return APIOperationResponse<long>.Success(createdReturn.Id, "Return created successfully");
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
                // Check if return exists
                var existingReturn = await _returnRepository.FindOneAsync(
                    r => r.Id == id && !r.IsDeleted
                );

                if (existingReturn == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Return not found");

                // Update priority
                existingReturn.Priority = priority;
                existingReturn.ModificationDate = DateTime.UtcNow;
                existingReturn.ModifiedBy = _currentUserService.UserId;

                // Update in repository
                await _returnRepository.UpdateAsync(existingReturn);

                return APIOperationResponse<bool>.Success(true, "Return priority updated successfully");
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
                var returnEntity = await _returnRepository.FindOneAsync(r => r.Id == id && !r.IsDeleted);
                if (returnEntity == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Return not found");

                // Soft delete - interceptor will handle IsDeleted, DeletionDate, and DeletedBy automatically
                await _returnRepository.DeleteAsync(returnEntity);

                return APIOperationResponse<bool>.Success(true, "Return deleted successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
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



