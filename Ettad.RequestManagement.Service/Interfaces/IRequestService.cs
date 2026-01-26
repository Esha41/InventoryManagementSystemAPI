using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.Common.Dtos;
using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Comman.Models;

namespace Ettad.RequestManagement.Service.Interfaces
{
    public interface IRequestService
    {
        Task<APIOperationResponse<List<BaseRequestDto>>> GetAllRequestsAsync(RequestStatus? status = null, RequestType? requestType = null);
        Task<APIOperationResponse<List<BaseRequestDto>>> GetRequestsByDepartmentAsync(long departmentId, RequestStatus? status = null, RequestType? requestType = null);
        Task<APIOperationResponse<List<BaseRequestDto>>> GetRequestsByRequesterAsync(string requesterId, RequestStatus? status = null, RequestType? requestType = null);
        Task<APIOperationResponse<List<BaseRequestDto>>> GetRequestsByStatusAndTypeAsync(RequestStatus? status, RequestType? requestType);
        Task<APIOperationResponse<List<BaseRequestDto>>> GetUserActionRequestsAsync(RequestStatus? status = null, RequestType? requestType = null);
        Task<APIOperationResponse<PaginatedList<BaseRequestDto>>> GetAllPaginatedAsync(PagedListRequest request);
        Task<APIOperationResponse<PaginatedList<BaseRequestDto>>> GetUserActionRequestsPaginatedAsync(PagedListRequest request);
    }
}
