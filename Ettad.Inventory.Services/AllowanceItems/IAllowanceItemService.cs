using Ettad.Inventory.Service.AllowanceItems.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.AllowanceItems
{
    public interface IAllowanceItemService
    {
        Task<APIOperationResponse<AllowanceItemDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<AllowanceItemDto>>> GetAllAsync();
        Task<APIOperationResponse<AllowanceItemDto>> CreateAsync(CreateUpdateAllowanceItemDto inputDto);
        Task<APIOperationResponse<AllowanceItemDto>> UpdateAsync(long id, CreateUpdateAllowanceItemDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
    }
}
