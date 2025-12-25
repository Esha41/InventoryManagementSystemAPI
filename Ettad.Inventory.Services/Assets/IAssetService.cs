using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;

namespace Ettad.Inventory.Service.Assets
{
    public interface IAssetService
    {
        Task<APIOperationResponse<List<AssetDto>>> GetAllAsync();
        Task<APIOperationResponse<AssetDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<long>> CreateAsync(CreateAssetDto inputDto, List<IFormFile>? files = null);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, UpdateAssetDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
    }
}

