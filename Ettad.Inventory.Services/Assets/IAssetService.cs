using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.Inventory.Services.Common;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;

namespace Ettad.Inventory.Service.Assets
{
    public interface IAssetService
    {
        Task<APIOperationResponse<List<AssetDto>>> GetAllAsync();
        Task<APIOperationResponse<AssetDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<AssetDto>> GetBySerialNumberAsync(string serialNumber);
        Task<APIOperationResponse<long>> CreateAsync(CreateAssetDto inputDto, List<IFormFile>? files = null);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, UpdateAssetDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        Task<APIOperationResponse<ImportResult<CreateAssetDto>>> ImportAsync(IFormFile file, long depotId);
        Task<APIOperationResponse<ImportResult<CreateAssetDto>>> ImportPreviewAsync(IFormFile file, long depotId);
        Task<APIOperationResponse<byte[]>> GenerateImportTemplateAsync(long depotId, string language = "en");
    }
}

