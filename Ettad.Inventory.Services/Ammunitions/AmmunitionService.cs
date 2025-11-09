using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Ammunitions.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;

namespace Ettad.Inventory.Service.Ammunitions
{
    public class AmmunitionService : IAmmunitionService
    {
        private readonly ICrossCuttingRepository<Ammunition> _ammunitionRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateAmmunitionDto> _validator;
        private readonly ICurrentUserService _currentUserService;

        public AmmunitionService(
            ICrossCuttingRepository<Ammunition> ammunitionRepository,
            IMapper mapper,
            IValidator<CreateUpdateAmmunitionDto> validator,
            ICurrentUserService currentUserService)
        {
            _ammunitionRepository = ammunitionRepository;
            _mapper = mapper;
            _validator = validator;
            _currentUserService = currentUserService;
        }

        public async Task<APIOperationResponse<AmmunitionDto>> GetByIdAsync(long id)
        {
            try
            {
                var ammunition = await _ammunitionRepository.FindOneAsync(
                    a => a.Id == id && !a.IsDeleted,
                    false,
                    nameof(Ammunition.Hcc),
                    nameof(Ammunition.BulletDiameterUnit),
                    nameof(Ammunition.CaseLengthUnit),
                    nameof(Ammunition.NatureOption),
                    nameof(Ammunition.PrimaryPurpos),
                    nameof(Ammunition.ProjectileColor),
                    nameof(Ammunition.ProjectailMaterial),
                    nameof(Ammunition.CaseType),
                    nameof(Ammunition.Propellant),
                    nameof(Ammunition.Compatibility),
                    nameof(Ammunition.HazardDivision)
                );

                if (ammunition == null)
                    return APIOperationResponse<AmmunitionDto>.Fail(ResponseType.NotFound, "Ammunition not found");

                var dto = _mapper.Map<AmmunitionDto>(ammunition);
                return APIOperationResponse<AmmunitionDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<AmmunitionDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<AmmunitionDto>>> GetAllAsync()
        {
            try
            {
                var ammunitions = await _ammunitionRepository.FindAsync(
                    a => !a.IsDeleted,
                    false,
                    nameof(Ammunition.Hcc),
                    nameof(Ammunition.BulletDiameterUnit),
                    nameof(Ammunition.CaseLengthUnit),
                    nameof(Ammunition.NatureOption),
                    nameof(Ammunition.PrimaryPurpos),
                    nameof(Ammunition.ProjectileColor),
                    nameof(Ammunition.ProjectailMaterial),
                    nameof(Ammunition.CaseType),
                    nameof(Ammunition.Propellant),
                    nameof(Ammunition.Compatibility),
                    nameof(Ammunition.HazardDivision)
                );

                var dtos = _mapper.Map<List<AmmunitionDto>>(ammunitions);
                return APIOperationResponse<List<AmmunitionDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<AmmunitionDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<AmmunitionDto>> CreateAsync(CreateUpdateAmmunitionDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<AmmunitionDto>.Fail(ResponseType.BadRequest, errors);
                }

                // Map DTO to entity
                var ammunition = _mapper.Map<Ammunition>(inputDto);
                ammunition.ItemType = ItemType.Ammunition;
                ammunition.CreationDate = DateTime.UtcNow;
                ammunition.CreatedBy = _currentUserService.UserId;

                // Add to repository
                var createdAmmunition = await _ammunitionRepository.AddAsync(ammunition);

                // Reload with navigation properties
                var result = await _ammunitionRepository.FindOneAsync(
                    a => a.Id == createdAmmunition.Id,
                    false,
                    nameof(Ammunition.Hcc),
                    nameof(Ammunition.BulletDiameterUnit),
                    nameof(Ammunition.CaseLengthUnit),
                    nameof(Ammunition.NatureOption),
                    nameof(Ammunition.PrimaryPurpos),
                    nameof(Ammunition.ProjectileColor),
                    nameof(Ammunition.ProjectailMaterial),
                    nameof(Ammunition.CaseType),
                    nameof(Ammunition.Propellant),
                    nameof(Ammunition.Compatibility),
                    nameof(Ammunition.HazardDivision)
                );

                var dto = _mapper.Map<AmmunitionDto>(result);
                return APIOperationResponse<AmmunitionDto>.Success(dto, "Ammunition created successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<AmmunitionDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<AmmunitionDto>> UpdateAsync(long id, CreateUpdateAmmunitionDto inputDto)
        {
            try
            {
                // Validate input
                var validationResult = await _validator.ValidateAsync(inputDto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<AmmunitionDto>.Fail(ResponseType.BadRequest, errors);
                }

                // Check if ammunition exists
                var existingAmmunition = await _ammunitionRepository.FindOneAsync(a => a.Id == id && !a.IsDeleted);
                if (existingAmmunition == null)
                    return APIOperationResponse<AmmunitionDto>.Fail(ResponseType.NotFound, "Ammunition not found");

                // Map updates to entity
                _mapper.Map(inputDto, existingAmmunition);
                existingAmmunition.ModificationDate = DateTime.UtcNow;
                existingAmmunition.ModifiedBy = _currentUserService.UserId;

                // Update in repository
                await _ammunitionRepository.UpdateAsync(existingAmmunition);

                // Reload with navigation properties
                var result = await _ammunitionRepository.FindOneAsync(
                    a => a.Id == id,
                    false,
                    nameof(Ammunition.Hcc),
                    nameof(Ammunition.BulletDiameterUnit),
                    nameof(Ammunition.CaseLengthUnit),
                    nameof(Ammunition.NatureOption),
                    nameof(Ammunition.PrimaryPurpos),
                    nameof(Ammunition.ProjectileColor),
                    nameof(Ammunition.ProjectailMaterial),
                    nameof(Ammunition.CaseType),
                    nameof(Ammunition.Propellant),
                    nameof(Ammunition.Compatibility),
                    nameof(Ammunition.HazardDivision)
                );

                var dto = _mapper.Map<AmmunitionDto>(result);
                return APIOperationResponse<AmmunitionDto>.Success(dto, "Ammunition updated successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<AmmunitionDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            try
            {
                var ammunition = await _ammunitionRepository.FindOneAsync(a => a.Id == id && !a.IsDeleted);
                if (ammunition == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Ammunition not found");

                // Soft delete - interceptor will handle IsDeleted, DeletionDate, and DeletedBy automatically
                await _ammunitionRepository.DeleteAsync(ammunition);

                return APIOperationResponse<bool>.Success(true, "Ammunition deleted successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
