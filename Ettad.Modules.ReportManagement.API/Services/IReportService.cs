using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;
using Ettad.Modules.ReportManagement.API.Services.Dtos;

namespace Ettad.Modules.ReportManagement.API.Services
{
    public interface IReportService
    {
        Task<APIOperationResponse<List<ReportDto>>> GetAllAsync();
        Task<APIOperationResponse<List<ReportDto>>> GetPublicReportsAsync();
        Task<APIOperationResponse<ReportDto>> GetByIdAsync(Guid id);
        Task<APIOperationResponse<ReportDto>> GetByUrlAsync(string url);
        Task<APIOperationResponse<string>> CreateAsync(CreateReportDto dto);
        Task<APIOperationResponse<bool>> UpdateAsync(Guid id, UpdateReportDto dto);
        Task<APIOperationResponse<ReportDto>> SetReportPublicAsync(Guid id, bool isPublic);
        Task<APIOperationResponse<bool>> DeleteAsync(Guid id);
        Task<bool> IsReportExists(string name);
        Task<APIOperationResponse<List<ReportStatusDto>>> GetReportStatusesAsync();
        Task<APIOperationResponse<List<ReportTemplateDto>>> GetTemplatesAsync();
        //Task<APIOperationResponse<Guid>> ImportAsync(IFormFile file, string? reportName = null, string? url = null, string? description = null);
       // Task<IReadOnlyList<TableSchemaInfo>> GetTableNamesAsync(CancellationToken cancellationToken = default);
    }

    public record TableSchemaInfo(string SchemaName, string TableName);
}
