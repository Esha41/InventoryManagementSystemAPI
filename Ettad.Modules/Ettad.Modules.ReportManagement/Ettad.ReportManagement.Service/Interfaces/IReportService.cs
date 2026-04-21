using Ettad.ResponseHandler.Models;
using Ettad.ReportManagement.Service.Dtos;

namespace Ettad.ReportManagement.Service.Interfaces
{
    public interface IReportService
    {
        Task<APIOperationResponse<List<ReportDto>>> GetAllAsync();
        Task<APIOperationResponse<List<ReportDto>>> GetPublicReportsAsync();
        Task<APIOperationResponse<ReportDto>> GetByIdAsync(Guid id);
        Task<APIOperationResponse<ReportDto>> GetByUrlAsync(string url);
        Task<APIOperationResponse<string>> CreateAsync(CreateReportDto dto);
        Task<APIOperationResponse<bool>> UpdateAsync(Guid id, UpdateReportDto dto);
        Task<APIOperationResponse<ReportDto>> SetReportPublicAsync(Guid id, SetReportPublicDto dto);
        Task<APIOperationResponse<bool>> DeleteAsync(Guid id);
        Task<bool> IsReportExists(string name);
        Task<APIOperationResponse<List<ReportTemplateDto>>> GetTemplatesAsync();
        Task<APIOperationResponse<List<string>>> GetReportRoleIdsAsync(Guid reportId);

        //Task<APIOperationResponse<Guid>> ImportAsync(IFormFile file, string? reportName = null, string? url = null, string? description = null);
     }
}

