using Ettad.Inventory.Service.Weapons.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;
using Ettad.Inventory.Services.Common;
using Ettad.CrossCutting.Comman.Models;

namespace Ettad.Inventory.Service.Weapons
{
    public interface IWeaponService
    {
        Task<APIOperationResponse<List<WeaponDto>>> GetAllAsync();
        Task<APIOperationResponse<PaginatedList<WeaponDto>>> GetAllPaginatedAsync(PagedListRequest request);
        Task<APIOperationResponse<WeaponDto>> GetByIdAsync(long id, bool includeDeleted = false);

        Task<APIOperationResponse<long>> CreateAsync(CreateUpdateWeaponDto inputDto, List<IFormFile>? files = null);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateWeaponDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        Task<APIOperationResponse<bool>> RestoreAsync(long id);
        Task<APIOperationResponse<bool>> PermanentDeleteAsync(long id);
        Task<APIOperationResponse<ImportResult<CreateUpdateWeaponDto>>> ImportAsync(IFormFile file, string language = "en");
        Task<APIOperationResponse<ImportResult<WeaponImportDto>>> ImportPreviewAsync(IFormFile file, string language = "en");
        Task<APIOperationResponse<byte[]>> GenerateImportTemplateAsync(string language = "en");
    }
}
