using Ettad.Inventory.Service.Ammunitions.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.Ammunitions
{
    public interface IAmmunitionService
    {
        Task<APIOperationResponse<AmmunitionDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<AmmunitionDto>>> GetAllAsync();
        Task<APIOperationResponse<long>> CreateAsync(CreateUpdateAmmunitionDto inputDto);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateAmmunitionDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
    }
}
