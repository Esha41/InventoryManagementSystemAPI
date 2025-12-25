using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Inventory.Service.Assets
{
    public interface IEmployeeService
    {
        Task<APIOperationResponse<List<EmployeeDto>>> GetAllAsync();
        Task<APIOperationResponse<EmployeeDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<long>> CreateAsync(CreateUpdateEmployeeDto inputDto);
        Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateEmployeeDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
    }
}

