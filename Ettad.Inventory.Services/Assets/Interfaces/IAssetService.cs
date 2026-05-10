using Ettad.Data.Enums;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Comman.Models;
using Microsoft.AspNetCore.Http;
using Ettad.Inventory.Service.Common.Interfaces;
using System.Collections.Generic;

namespace Ettad.Inventory.Service.Assets.Interfaces
{
    public interface IAssetService
    {
        Task<APIOperationResponse<List<AssetDto>>> GetAllAsync(long? depotId = null, List<long>? depotIds = null);
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
        /// <summary>
        /// Paginated distinct catalog items that have assets; each row is item id, identity fields, and total asset count.
        /// </summary>
        Task<APIOperationResponse<PaginatedList<AssetItemCatalogSummaryDto>>> GetAssetCatalogItemSummariesPagedAsync(
            PagedListRequest request,
            long? depotId = null,
            List<long>? depotIds = null,
            ItemType? itemType = null);
        Task<APIOperationResponse<List<AssetDto>>> GetAssetsByItemIdAsync(long itemId, long? depotId = null);
        /// <summary>
        /// Same data as <see cref="GetAssetsByItemIdAsync"/> but paged (Kendo-style <see cref="PagedListRequest"/> body).
        /// </summary>
        Task<APIOperationResponse<PaginatedList<AssetDto>>> GetAssetsByItemIdPagedAsync(long itemId, PagedListRequest request, long? depotId = null);
    }
}

