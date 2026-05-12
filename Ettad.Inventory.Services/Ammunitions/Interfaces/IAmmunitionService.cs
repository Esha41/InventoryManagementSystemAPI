using Ettad.Data.Enums;
using Ettad.Inventory.Service.Ammunitions.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;
using Ettad.CrossCutting.Comman.Models;
using Ettad.Inventory.Service.Common.Services;

namespace Ettad.Inventory.Service.Ammunitions.Interfaces
{
    public interface IAmmunitionService
    {
        Task<APIOperationResponse<AmmunitionDto>> GetByIdAsync(long id, bool includeDeleted = false);
        Task<APIOperationResponse<List<AmmunitionDto>>> GetAllAsync();
        Task<APIOperationResponse<PaginatedList<AmmunitionDto>>> GetAllPaginatedAsync(PagedListRequest request);
        Task<APIOperationResponse<List<AmmunitionDto>>> GetByTypeAsync(AmmunitionType ammunitionType);
        Task<APIOperationResponse<long>> CreateAsync(CreateUpdateAmmunitionDto inputDto, List<IFormFile>? files = null);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateAmmunitionDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        Task<APIOperationResponse<bool>> RestoreAsync(long id);
        Task<APIOperationResponse<bool>> PermanentDeleteAsync(long id);
        Task<APIOperationResponse<ImportResult<CreateUpdateAmmunitionDto>>> ImportAsync(IFormFile file, string language = "en");
        Task<APIOperationResponse<ImportResult<AmmunitionImportDto>>> ImportPreviewAsync(IFormFile file, string language = "en");
        Task<APIOperationResponse<byte[]>> GenerateImportTemplateAsync(string language = "en");
    }
}
