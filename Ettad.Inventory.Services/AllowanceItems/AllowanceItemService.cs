using AutoMapper;
using FluentValidation;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Inventory.Service.AllowanceItems.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;

namespace Ettad.Inventory.Service.AllowanceItems
{
    public class AllowanceItemService : IAllowanceItemService
    {
        private readonly ICrossCuttingRepository<AllowanceItem> _allowanceItemRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateAllowanceItemDto> _validator;
        private readonly ICurrentUserService _currentUserService;

        public AllowanceItemService(
            ICrossCuttingRepository<AllowanceItem> allowanceItemRepository,
            IMapper mapper,
            IValidator<CreateUpdateAllowanceItemDto> validator,
            ICurrentUserService currentUserService)
        {
            _allowanceItemRepository = allowanceItemRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
        }

        public async Task<APIOperationResponse<AllowanceItemDto>> GetByIdAsync(long id)
        {
            try
            {
                var allowanceItem = await _allowanceItemRepository.FindOneAsync(x => x.Id == id);

                if (allowanceItem == null)
                    return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.NotFound, "Allowance item not found");

                var dto = _mapper.Map<AllowanceItemDto>(allowanceItem);
                return APIOperationResponse<AllowanceItemDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<AllowanceItemDto>>> GetAllAsync()
        {
            try
            {
                var allowanceItems = await _allowanceItemRepository.GetAllAsync();

                var dtos = _mapper.Map<List<AllowanceItemDto>>(allowanceItems);
                return APIOperationResponse<List<AllowanceItemDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<AllowanceItemDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<AllowanceItemDto>> CreateAsync(CreateUpdateAllowanceItemDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.BadRequest, errors);
                }

                // Map DTO to entity
                var allowanceItem = _mapper.Map<AllowanceItem>(inputDto);
                allowanceItem.CreationDate = DateTime.UtcNow;
                allowanceItem.CreatedBy = _currentUserService.UserId;

                // Add to repository
                var createdAllowanceItem = await _allowanceItemRepository.AddAsync(allowanceItem);

                // Reload with navigation properties
                var result = await _allowanceItemRepository.FindOneAsync( a => a.Id == createdAllowanceItem.Id);

                var dto = _mapper.Map<AllowanceItemDto>(result);
                return APIOperationResponse<AllowanceItemDto>.Success(dto, "Allowance item created successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<AllowanceItemDto>> UpdateAsync(long id, CreateUpdateAllowanceItemDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.BadRequest, errors);
                }

                // Check if allowance item exists
                var existingAllowanceItem = await _allowanceItemRepository.FindOneAsync(a => a.Id == id);
                if (existingAllowanceItem == null)
                    return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.NotFound, "Allowance item not found");

                // Map updates to entity
                _mapper.Map(inputDto, existingAllowanceItem);
                existingAllowanceItem.ModificationDate = DateTime.UtcNow;
                existingAllowanceItem.ModifiedBy = _currentUserService.UserId;

                // Update in repository
                await _allowanceItemRepository.UpdateAsync(existingAllowanceItem);

                // Reload with navigation properties
                var result = await _allowanceItemRepository.FindOneAsync(a => a.Id == id);

                var dto = _mapper.Map<AllowanceItemDto>(result);
                return APIOperationResponse<AllowanceItemDto>.Success(dto, "Allowance item updated successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<AllowanceItemDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            try
            {
                var allowanceItem = await _allowanceItemRepository.FindOneAsync(a => a.Id == id && !a.IsDeleted);
                if (allowanceItem == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Allowance item not found");

                // Soft delete - interceptor will handle IsDeleted, DeletionDate, and DeletedBy automatically
                await _allowanceItemRepository.DeleteAsync(allowanceItem);

                return APIOperationResponse<bool>.Success(true, "Allowance item deleted successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}

