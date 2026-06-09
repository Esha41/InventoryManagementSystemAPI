using Ettad.CrossCutting.Comman.Models;
using Ettad.Inventory.Service.Accessories.Dtos;
using Ettad.Inventory.Service.Common.Services;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;

namespace Ettad.Inventory.Service.Accessories.Interfaces
{
    public interface IAccessoryService
    {
        Task<APIOperationResponse<List<AccessoryDto>>> GetAllAsync();
        Task<APIOperationResponse<PaginatedList<AccessoryDto>>> GetAllPaginatedAsync(PagedListRequest request);
        Task<APIOperationResponse<AccessoryDto>> GetByIdAsync(long id, bool includeDeleted = false);
        Task<APIOperationResponse<long>> CreateAsync(CreateUpdateAccessoryDto inputDto, List<IFormFile>? files = null);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateAccessoryDto inputDto, List<IFormFile>? files = null, bool removeImage = false);
        Task<APIOperationResponse<AccessoryImageFileDto>> GetMainImageAsync(long id);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        Task<APIOperationResponse<bool>> RestoreAsync(long id);
        Task<APIOperationResponse<bool>> PermanentDeleteAsync(long id);
        Task<APIOperationResponse<ImportResult<CreateUpdateAccessoryDto>>> ImportAsync(IFormFile file, string language = "en");
        Task<APIOperationResponse<ImportResult<AccessoryImportDto>>> ImportPreviewAsync(IFormFile file, string language = "en");
        Task<APIOperationResponse<byte[]>> GenerateImportTemplateAsync(string language = "en");
    }
}
