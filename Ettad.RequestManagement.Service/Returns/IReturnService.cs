using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.Returns.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;

namespace Ettad.RequestManagement.Service.Returns
{
    public interface IReturnService
    {
        Task<APIOperationResponse<ReturnDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<ReturnDto>>> GetAllAsync();
        Task<APIOperationResponse<long>> CreateAsync(CreateReturnDto inputDto);
        Task<APIOperationResponse<long>> CreateAsync(CreateReturnDto inputDto, List<IFormFile> files);
        Task<APIOperationResponse<bool>> ChangePriorityAsync(long id, RequestPriority priority);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        Task<APIOperationResponse<bool>> SetDepotAsync(long returnId, SetReturnDepotDto dto);
        Task<APIOperationResponse<bool>> SetDeliveryDateAsync(long returnId, SetReturnDeliveryDateDto dto);
        Task<APIOperationResponse<bool>> ProcessReturnItemsAsync(long returnId, ProcessReturnItemsDto dto, List<IFormFile>? files = null);

        Task<APIOperationResponse<List<ReturnTrackingLineDto>>> GetReturnTrackingLinesAsync(long returnId);
    }
}

