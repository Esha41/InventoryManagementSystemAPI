using Ettad.Data.Enums;
using Ettad.Inventory.Service.Ammunitions.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;

namespace Ettad.Inventory.Service.Ammunitions
{
    public interface IAmmunitionService
    {
        Task<APIOperationResponse<AmmunitionDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<AmmunitionDto>>> GetAllAsync();
        Task<APIOperationResponse<List<AmmunitionDto>>> GetByTypeAsync(AmmunitionType ammunitionType);
        Task<APIOperationResponse<long>> CreateAsync(CreateUpdateAmmunitionDto inputDto, List<IFormFile>? files = null);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateAmmunitionDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
    }
}
