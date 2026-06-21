using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;

namespace Ettad.User.Services.Interfaces
{
    public interface IAdminAnalyticsService
    {
        Task<APIOperationResponse<SystemHealthMetricsDto>> GetSystemHealthMetricsAsync();
        Task<APIOperationResponse<PerformanceMetricsDto>> GetPerformanceMetricsAsync();
        Task<APIOperationResponse<UserActivityMetricsDto>> GetUserActivityMetricsAsync();
        Task<APIOperationResponse<RequestMetricsDto>> GetRequestMetricsAsync();
        Task<APIOperationResponse<RequestTrendsDto>> GetRequestTrendsAsync(string period);
        Task<APIOperationResponse<TopRequestedItemsDto>> GetTopRequestedItemsAsync(int limit);
        Task<APIOperationResponse<WorkflowPerformanceDto>> GetWorkflowPerformanceAsync(int days);
    }
}
