using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.Returns.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.RequestManagement.Service.Returns
{
    public interface IReturnService
    {
        Task<APIOperationResponse<ReturnDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<ReturnDto>>> GetAllAsync();
        Task<APIOperationResponse<long>> CreateAsync(CreateReturnDto inputDto);
        Task<APIOperationResponse<bool>> ChangePriorityAsync(long id, RequestPriority priority);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
    }
}

