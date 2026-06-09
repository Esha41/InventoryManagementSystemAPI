using Ettad.Inventory.Service.WeaponAccessories.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.WeaponAccessories.Interfaces
{
    public interface IWeaponAccessoryService
    {
        Task<APIOperationResponse<List<WeaponAccessoryDto>>> GetByWeaponIdAsync(long weaponId);
        Task<APIOperationResponse<List<WeaponAccessoryDto>>> GetByAccessoryIdAsync(long accessoryId);
        Task<APIOperationResponse<WeaponAccessoryDto>> GetByIdAsync(long weaponId, long accessoryId);
        Task<APIOperationResponse<bool>> CreateAsync(CreateUpdateWeaponAccessoryDto dto);
        Task<APIOperationResponse<bool>> UpdateAsync(long weaponId, long accessoryId, CreateUpdateWeaponAccessoryDto dto);
        Task<APIOperationResponse<bool>> DeleteAsync(long weaponId, long accessoryId);
        Task<APIOperationResponse<bool>> ReplaceForWeaponAsync(BulkReplaceWeaponAccessoriesDto dto);
    }
}
