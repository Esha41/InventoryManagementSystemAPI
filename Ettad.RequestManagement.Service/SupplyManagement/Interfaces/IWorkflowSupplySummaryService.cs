using Ettad.RequestManagement.Service.SupplyManagement.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.RequestManagement.Service.SupplyManagement.Interfaces
{
    public interface IWorkflowSupplySummaryService
    {
        Task<APIOperationResponse<WorkflowSupplySummaryDto>> GetSummaryForOrderAsync(long orderId);
    }
}
