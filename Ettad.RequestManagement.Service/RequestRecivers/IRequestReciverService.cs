using Ettad.RequestManagement.Service.RequestRecivers.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.RequestManagement.Service.RequestRecivers
{
    public interface IRequestReciverService
    {
        Task<APIOperationResponse<RequestReciverDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<RequestReciverDto>>> GetAllAsync();
        Task<APIOperationResponse<RequestReciverDto>> CreateAsync(CreateUpdateRequestReciverDto inputDto);
        Task<APIOperationResponse<RequestReciverDto>> UpdateAsync(long id, CreateUpdateRequestReciverDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
    }
}

