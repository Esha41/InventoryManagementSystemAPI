using Ettad.RequestManagement.Service.Requests.Dtos;
using Ettad.ResponseHandler.Models;


namespace Ettad.RequestManagement.Service.Requests
{
    public interface IRequestService
    {
        Task<APIOperationResponse<RequestDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<RequestDto>>> GetAllAsync();
        Task<APIOperationResponse<RequestDto>> CreateAsync(CreateUpdateRequestDto inputDto);
        Task<APIOperationResponse<RequestDto>> UpdateAsync(long id, CreateUpdateRequestDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
    }
}
