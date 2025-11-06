using Ettad.RequestManagement.Service.RequestPurposes.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.RequestManagement.Service.RequestPurposes
{
    public interface IRequestPurposeService
    {
        Task<APIOperationResponse<long>> CreateForDiscardAsync(CreateUpdateRequestPurposeDto inputDto);
        Task<APIOperationResponse<long>> CreateForReturnAsync(CreateUpdateRequestPurposeDto inputDto);
        Task<APIOperationResponse<long>> CreateForOrderAsync(CreateUpdateRequestPurposeDto inputDto);
        
        Task<APIOperationResponse<RequestPurposeDto>> GetByIdForDiscardAsync(long id);
        Task<APIOperationResponse<RequestPurposeDto>> GetByIdForReturnAsync(long id);
        Task<APIOperationResponse<RequestPurposeDto>> GetByIdForOrderAsync(long id);
        
        Task<APIOperationResponse<List<RequestPurposeDto>>> GetAllForDiscardAsync();
        Task<APIOperationResponse<List<RequestPurposeDto>>> GetAllForReturnAsync();
        Task<APIOperationResponse<List<RequestPurposeDto>>> GetAllForOrderAsync();
        
        Task<APIOperationResponse<bool>> UpdateAsync(long id, CreateUpdateRequestPurposeDto inputDto);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
    }
}

