using Ettad.Inventory.Service.Explosives.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;
using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.Explosives
{
    public interface IExplosiveService
    {
        Task<APIOperationResponse<List<ExplosiveDto>>> GetAllAsync();
        Task<APIOperationResponse<ExplosiveDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<ExplosiveDto>>> GetByTypeAsync(ExplosiveType explosiveType);
        Task<APIOperationResponse<long>> CreateAsync(CreateUpdateExplosiveDto inputDto, List<IFormFile>? files = null);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateExplosiveDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
    }
}
