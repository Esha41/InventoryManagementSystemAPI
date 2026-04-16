using Ettad.ResponseHandler.Models;
using Ettad.ReportManagement.Service.Dtos;

namespace Ettad.ReportManagement.Service.Interfaces
{
    public interface IScheduledReportService
    {
        Task<APIOperationResponse<List<ScheduledReportDto>>> GetAllAsync();
        Task<APIOperationResponse<ScheduledReportDto>> GetByIdAsync(Guid id);
        Task<APIOperationResponse<Guid>> CreateAsync(CreateScheduledReportDto dto);
        Task<APIOperationResponse<bool>> UpdateAsync(Guid id, UpdateScheduledReportDto dto);
        Task<APIOperationResponse<bool>> DeleteAsync(Guid id);
        Task<APIOperationResponse<bool>> ToggleActiveAsync(Guid id, bool isActive);
        Task<APIOperationResponse<List<ScheduledReportExecutionDto>>> GetExecutionHistoryAsync(Guid scheduledReportId);
        Task<APIOperationResponse<bool>> ExecuteNowAsync(Guid id);
    }
}

