using Ettad.Inventory.Service.Explosives.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;
using Ettad.Data.Enums;
using Ettad.CrossCutting.Comman.Models;
using Ettad.Inventory.Service.Common.Interfaces;

namespace Ettad.Inventory.Service.Explosives.Interfaces
{
    public interface IExplosiveService
    {
        Task<APIOperationResponse<List<ExplosiveDto>>> GetAllAsync();
        Task<APIOperationResponse<PaginatedList<ExplosiveDto>>> GetAllPaginatedAsync(PagedListRequest request);
        Task<APIOperationResponse<ExplosiveDto>> GetByIdAsync(long id, bool includeDeleted = false);
        Task<APIOperationResponse<long>> CreateAsync(CreateUpdateExplosiveDto inputDto, List<IFormFile>? files = null);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateExplosiveDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        Task<APIOperationResponse<bool>> RestoreAsync(long id);
        Task<APIOperationResponse<bool>> PermanentDeleteAsync(long id);
        Task<APIOperationResponse<ImportResult<CreateUpdateExplosiveDto>>> ImportAsync(IFormFile file, string language = "en");
        Task<APIOperationResponse<ImportResult<ExplosiveImportDto>>> ImportPreviewAsync(IFormFile file, string language = "en");
        Task<APIOperationResponse<byte[]>> GenerateImportTemplateAsync(string language = "en");
    }
}
