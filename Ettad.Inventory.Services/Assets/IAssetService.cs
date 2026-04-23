using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Comman.Models;
using Microsoft.AspNetCore.Http;
using Ettad.Inventory.Service.Common.Interfaces;

namespace Ettad.Inventory.Service.Assets
{
    public interface IAssetService
    {
        Task<APIOperationResponse<List<AssetDto>>> GetAllAsync(long? depotId = null);
        Task<APIOperationResponse<AssetDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<AssetDto>> GetBySerialNumberAsync(string serialNumber);
        Task<APIOperationResponse<long>> CreateAsync(CreateAssetDto inputDto, List<IFormFile>? files = null);
        Task<APIOperationResponse<List<long>>> CreateBulkAsync(string? dtosJson, List<IFormFile>? files = null);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, UpdateAssetDto inputDto, List<IFormFile>? files = null);
        Task<APIOperationResponse<BulkCreateFromTemplateResultDto>> CreateBulkFromTemplateAsync(CreateBulkAssetsFromTemplateDto dto);
        Task<APIOperationResponse<bool>> UpdateSerialNumberAsync(long assetId, string? serialNumber);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        Task<APIOperationResponse<ImportResult<CreateAssetDto>>> ImportAsync(IFormFile file, long depotId, string language = "en");
        Task<APIOperationResponse<ImportResult<AssetImportDto>>> ImportPreviewAsync(IFormFile file, long depotId, string language = "en");
        Task<APIOperationResponse<byte[]>> GenerateImportTemplateAsync(long depotId, string language = "en");
        Task<APIOperationResponse<PaginatedList<AssetDto>>> GetAssetsPaginatedAsync(long? depotId, PagedListRequest request);
        Task<APIOperationResponse<List<AssetDto>>> GetAssetsByItemIdAsync(long itemId, long? depotId = null);
    }
}

