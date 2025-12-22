using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.Discards.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;

namespace Ettad.RequestManagement.Service.Discards
{
    public interface IDiscardService
    {
        Task<APIOperationResponse<DiscardDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<List<DiscardDto>>> GetAllAsync();
        Task<APIOperationResponse<long>> CreateAsync(CreateDiscardDto inputDto);
        Task<APIOperationResponse<long>> CreateAsync(CreateDiscardDto inputDto, List<IFormFile> files);
        Task<APIOperationResponse<bool>> ChangePriorityAsync(long id, RequestPriority priority);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
    }
}

