using Ettad.Reporting.Services.Reports.Dtos;
using Ettad.ResponseHandler.Models;

namespace Ettad.Reporting.Services
{
    public interface IReportService
    {
        Task<APIOperationResponse<List<ReportDto>>> GetAllAsync();
        Task<APIOperationResponse<ReportDto>> GetByIdAsync(Guid id);
        Task<APIOperationResponse<ReportDto>> GetByUrlAsync(string url);
        Task<APIOperationResponse<Guid>> CreateAsync(CreateReportDto dto);
        Task<APIOperationResponse<bool>> UpdateAsync(Guid id, UpdateReportDto dto);
        Task<APIOperationResponse<bool>> DeleteAsync(Guid id);
        Task<APIOperationResponse<List<ReportStatusDto>>> GetReportStatusesAsync();
    }
}
