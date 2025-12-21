using Ettad.Inventory.Service.Weapons.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;
using Ettad.Data.Enums;
using Ettad.Inventory.Services.Common;

namespace Ettad.Inventory.Service.Weapons
{
    public interface IWeaponService
    {
        Task<APIOperationResponse<List<WeaponDto>>> GetAllAsync();
        Task<APIOperationResponse<WeaponDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<WeaponDto>>> GetByTypeAsync(WeaponType weaponType);
        Task<APIOperationResponse<long>> CreateAsync(CreateUpdateWeaponDto inputDto, List<IFormFile>? files = null);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateWeaponDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        Task<APIOperationResponse<ImportResult<CreateUpdateWeaponDto>>> ImportAsync(IFormFile file);
    }
}
