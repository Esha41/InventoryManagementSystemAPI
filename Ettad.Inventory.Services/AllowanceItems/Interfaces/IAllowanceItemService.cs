using Ettad.Inventory.Service.AllowanceItems.Dtos;
using Ettad.ResponseHandler.Models;
using Ettad.Data.Enums;

namespace Ettad.Inventory.Service.AllowanceItems.Interfaces
{
    public interface IAllowanceItemService
    {
        Task<APIOperationResponse<AllowanceItemDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<AllowanceItemDto>>> GetAllAsync();
        Task<APIOperationResponse<long>> CreateAsync(CreateUpdateAllowanceItemDto inputDto);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateAllowanceItemDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        Task<APIOperationResponse<AllowanceItemByDepartmentDto>> GetByDepartmentAndYearAsync(long departmentId, int year);
        Task<APIOperationResponse<List<AllowanceItemByDepartmentDto>>> GetByDepartmentAsync(long departmentId);
        Task<APIOperationResponse<bool>> BulkCreateAsync(BulkCreateAllowanceItemDto inputDto);
        Task<APIOperationResponse<AllowanceReserveDetailsDto>> GetReserveDetailsAsync(long departmentId, int year);
        Task<APIOperationResponse<AllowanceReserveDetailsByItemDto>> GetReserveDetailsByItemAsync(long departmentId, int year);
    }
}
