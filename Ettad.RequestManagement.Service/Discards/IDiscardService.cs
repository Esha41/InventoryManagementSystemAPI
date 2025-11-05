using Ettad.RequestManagement.Service.Discards.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.RequestManagement.Service.Discards
{
    public interface IDiscardService
    {
        Task<APIOperationResponse<DiscardDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<DiscardDto>>> GetAllAsync();
        Task<APIOperationResponse<DiscardDto>> CreateAsync(CreateDiscardDto inputDto);
        Task<APIOperationResponse<DiscardDto>> UpdateAsync(long id, UpdateDiscardDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
    }
}

