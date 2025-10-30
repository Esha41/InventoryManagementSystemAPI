using Ettad.Inventory.Services.Ammunitions.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Services.Ammunitions
{
    public interface IAmmunitionService
    {
        Task<APIOperationResponse<AmmunitionDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<AmmunitionDto>>> GetAllAsync();
        Task<APIOperationResponse<AmmunitionDto>> CreateAsync(CreateUpdateAmmunitionDto inputDto);
        Task<APIOperationResponse<AmmunitionDto>> UpdateAsync(long id, CreateUpdateAmmunitionDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
    }
}
