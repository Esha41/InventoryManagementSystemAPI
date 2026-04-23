using Ettad.Inventory.Service.ItemDepartmentAssignments.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.ItemDepartmentAssignments.Interfaces
{
    public interface IItemDepartmentAssignmentService
    {
        Task<APIOperationResponse<ItemDepartmentAssignmentDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<ItemDepartmentAssignmentDto>>> GetAllAsync();
        Task<APIOperationResponse<List<ItemDepartmentAssignmentDto>>> GetByDepartmentIdAsync(long departmentId);
        Task<APIOperationResponse<List<ItemDepartmentAssignmentDto>>> GetByItemIdAsync(long itemId);
        Task<APIOperationResponse<long>> CreateAsync(CreateUpdateItemDepartmentAssignmentDto inputDto);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateItemDepartmentAssignmentDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        Task<APIOperationResponse<bool>> BulkAssignAsync(List<CreateUpdateItemDepartmentAssignmentDto> assignments);
        Task<APIOperationResponse<List<DepartmentAssignmentSummaryDto>>> GetDepartmentSummariesAsync();
    }
}
