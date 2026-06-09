using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Ettad.Application.Common.Interfaces;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Inventory.Service.WeaponAccessories.Dtos;
using Ettad.Inventory.Service.WeaponAccessories.Interfaces;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.WeaponAccessories.Services
{
    public class WeaponAccessoryService : IWeaponAccessoryService
    {
        private readonly ICrossCuttingRepository<WeaponAccessory> _weaponAccessoryRepository;
        private readonly ICrossCuttingRepository<Weapon> _weaponRepository;
        private readonly ICrossCuttingRepository<Accessory> _accessoryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateUpdateWeaponAccessoryDto> _createUpdateValidator;
        private readonly IValidator<BulkReplaceWeaponAccessoriesDto> _bulkReplaceValidator;
        private readonly ITransactionManager _transactionManager;

        public WeaponAccessoryService(
            ICrossCuttingRepository<WeaponAccessory> weaponAccessoryRepository,
            ICrossCuttingRepository<Weapon> weaponRepository,
            ICrossCuttingRepository<Accessory> accessoryRepository,
            IMapper mapper,
            IValidator<CreateUpdateWeaponAccessoryDto> createUpdateValidator,
            IValidator<BulkReplaceWeaponAccessoriesDto> bulkReplaceValidator,
            ITransactionManager transactionManager)
        {
            _weaponAccessoryRepository = weaponAccessoryRepository;
            _weaponRepository = weaponRepository;
            _accessoryRepository = accessoryRepository;
            _mapper = mapper;
            _createUpdateValidator = createUpdateValidator;
            _bulkReplaceValidator = bulkReplaceValidator;
            _transactionManager = transactionManager;
        }

        public async Task<APIOperationResponse<List<WeaponAccessoryDto>>> GetByWeaponIdAsync(long weaponId)
        {
            try
            {
                var weaponExists = await _weaponRepository.FindOneAsync(w => w.Id == weaponId && !w.IsDeleted);
                if (weaponExists == null)
                    return APIOperationResponse<List<WeaponAccessoryDto>>.Fail(ResponseType.NotFound, "Weapon not found");

                var links = await _weaponAccessoryRepository.FindAsync(
                    x => x.WeaponId == weaponId,
                    false,
                    nameof(WeaponAccessory.Accessory),
                    nameof(WeaponAccessory.Weapon));

                var dtos = _mapper.Map<List<WeaponAccessoryDto>>(links);
                return APIOperationResponse<List<WeaponAccessoryDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<WeaponAccessoryDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<WeaponAccessoryDto>>> GetByAccessoryIdAsync(long accessoryId)
        {
            try
            {
                var accessoryExists = await _accessoryRepository.FindOneAsync(a => a.Id == accessoryId && !a.IsDeleted);
                if (accessoryExists == null)
                    return APIOperationResponse<List<WeaponAccessoryDto>>.Fail(ResponseType.NotFound, "Accessory not found");

                var links = await _weaponAccessoryRepository.FindAsync(
                    x => x.AccessoryId == accessoryId,
                    false,
                    nameof(WeaponAccessory.Accessory),
                    nameof(WeaponAccessory.Weapon));

                var dtos = _mapper.Map<List<WeaponAccessoryDto>>(links);
                return APIOperationResponse<List<WeaponAccessoryDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<WeaponAccessoryDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<WeaponAccessoryDto>> GetByIdAsync(long weaponId, long accessoryId)
        {
            try
            {
                var link = await _weaponAccessoryRepository.FindOneAsync(
                    x => x.WeaponId == weaponId && x.AccessoryId == accessoryId,
                    false,
                    nameof(WeaponAccessory.Accessory),
                    nameof(WeaponAccessory.Weapon));

                if (link == null)
                    return APIOperationResponse<WeaponAccessoryDto>.Fail(ResponseType.NotFound, "Weapon accessory link not found");

                var dto = _mapper.Map<WeaponAccessoryDto>(link);
                return APIOperationResponse<WeaponAccessoryDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<WeaponAccessoryDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> CreateAsync(CreateUpdateWeaponAccessoryDto dto)
        {
            try
            {
                var validationResult = await _createUpdateValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                var validationError = await ValidateWeaponAndAccessoryAsync(dto.WeaponId, dto.AccessoryId);
                if (validationError != null)
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, validationError);

                var existing = await _weaponAccessoryRepository.FindOneAsync(
                    x => x.WeaponId == dto.WeaponId && x.AccessoryId == dto.AccessoryId);
                if (existing != null)
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "This weapon-accessory link already exists");

                var entity = _mapper.Map<WeaponAccessory>(dto);
                await _weaponAccessoryRepository.AddAsync(entity);

                return APIOperationResponse<bool>.Success(true, "Weapon accessory link created successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(long weaponId, long accessoryId, CreateUpdateWeaponAccessoryDto dto)
        {
            try
            {
                dto.WeaponId = weaponId;
                dto.AccessoryId = accessoryId;

                var validationResult = await _createUpdateValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                var link = await _weaponAccessoryRepository.FindOneAsync(
                    x => x.WeaponId == weaponId && x.AccessoryId == accessoryId);
                if (link == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Weapon accessory link not found");

                link.DefaultQuantity = dto.DefaultQuantity;
                await _weaponAccessoryRepository.UpdateAsync(link);

                return APIOperationResponse<bool>.Success(true, "Weapon accessory link updated successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long weaponId, long accessoryId)
        {
            try
            {
                var link = await _weaponAccessoryRepository.FindOneAsync(
                    x => x.WeaponId == weaponId && x.AccessoryId == accessoryId);
                if (link == null)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Weapon accessory link not found");

                await _weaponAccessoryRepository.DeleteAsync(link);

                return APIOperationResponse<bool>.Success(true, "Weapon accessory link deleted successfully");
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> ReplaceForWeaponAsync(BulkReplaceWeaponAccessoriesDto dto)
        {
            try
            {
                var validationResult = await _bulkReplaceValidator.ValidateAsync(dto);
                if (!validationResult.IsValid)
                {
                    var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, errors);
                }

                var weapon = await _weaponRepository.FindOneAsync(w => w.Id == dto.WeaponId && !w.IsDeleted);
                if (weapon == null || weapon.ItemType != ItemType.Weapon)
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Weapon not found");

                foreach (var item in dto.Accessories)
                {
                    var validationError = await ValidateWeaponAndAccessoryAsync(dto.WeaponId, item.AccessoryId);
                    if (validationError != null)
                        return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, validationError);
                }

                await using var transaction = await _transactionManager.BeginAsync();
                try
                {
                    var existing = await _weaponAccessoryRepository
                        .Find(x => x.WeaponId == dto.WeaponId)
                        .ToListAsync();

                    foreach (var row in existing)
                        await _weaponAccessoryRepository.DeleteAsync(row);

                    if (dto.Accessories.Any())
                    {
                        var newRows = dto.Accessories.Select(a => new WeaponAccessory
                        {
                            WeaponId = dto.WeaponId,
                            AccessoryId = a.AccessoryId,
                            DefaultQuantity = a.DefaultQuantity
                        }).ToList();

                        await _weaponAccessoryRepository.AddRangeAsync(newRows);
                    }

                    await _transactionManager.CommitAsync();
                    return APIOperationResponse<bool>.Success(true, "Weapon accessories replaced successfully");
                }
                catch (Exception)
                {
                    await _transactionManager.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        private async Task<string?> ValidateWeaponAndAccessoryAsync(long weaponId, long accessoryId)
        {
            var weapon = await _weaponRepository.FindOneAsync(w => w.Id == weaponId && !w.IsDeleted);
            if (weapon == null || weapon.ItemType != ItemType.Weapon)
                return "Weapon not found or is not a valid weapon item";

            var accessory = await _accessoryRepository.FindOneAsync(a => a.Id == accessoryId && !a.IsDeleted);
            if (accessory == null || accessory.ItemType != ItemType.Accessory)
                return "Accessory not found or is not a valid accessory item";

            return null;
        }
    }
}
