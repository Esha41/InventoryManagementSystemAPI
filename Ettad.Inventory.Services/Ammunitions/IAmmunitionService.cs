using Ettad.Data.Enums;
using Ettad.Inventory.Service.Ammunitions.Dtos;
using Ettad.Inventory.Services.Common;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;
using Ettad.CrossCutting.Comman.Models;

namespace Ettad.Inventory.Service.Ammunitions
{
    public interface IAmmunitionService
    {
        Task<APIOperationResponse<AmmunitionDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<AmmunitionDto>>> GetAllAsync();
        Task<APIOperationResponse<PaginatedList<AmmunitionDto>>> GetAllPaginatedAsync(PagedListRequest request);
        Task<APIOperationResponse<List<AmmunitionDto>>> GetByTypeAsync(AmmunitionType ammunitionType);
        Task<APIOperationResponse<long>> CreateAsync(CreateUpdateAmmunitionDto inputDto, List<IFormFile>? files = null);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateAmmunitionDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        Task<APIOperationResponse<ImportResult<CreateUpdateAmmunitionDto>>> ImportAsync(IFormFile file, string language = "en");
        Task<APIOperationResponse<ImportResult<AmmunitionImportDto>>> ImportPreviewAsync(IFormFile file, string language = "en");
        Task<APIOperationResponse<byte[]>> GenerateImportTemplateAsync(string language = "en");
    }
}
