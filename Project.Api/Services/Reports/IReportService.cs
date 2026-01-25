using Ettad.Reporting.Services.Reports.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;

namespace Ettad.Reporting.Services
{
    public interface IReportService
    {
        Task<APIOperationResponse<List<ReportDto>>> GetAllAsync();
        Task<APIOperationResponse<ReportDto>> GetByIdAsync(Guid id);
        Task<APIOperationResponse<ReportDto>> GetByUrlAsync(string url);
        Task<APIOperationResponse<Guid>> CreateAsync(CreateReportDto dto);
        Task<APIOperationResponse<bool>> UpdateAsync(Guid id, UpdateReportDto dto);
        Task<APIOperationResponse<ReportDto>> SetReportPublicAsync(Guid id, bool isPublic);
        Task<APIOperationResponse<bool>> DeleteAsync(Guid id);
        Task<APIOperationResponse<List<ReportStatusDto>>> GetReportStatusesAsync();
        Task<APIOperationResponse<Guid>> ImportAsync(IFormFile file, string? reportName = null, string? url = null, string? description = null);

        /// <summary>
        /// Returns all user tables (schema + name) for the configured database.
        /// Excludes system tables (e.g. __EFMigrationsHistory, sysdiagrams).
        /// </summary>
        Task<IReadOnlyList<TableSchemaInfo>> GetTableNamesAsync(CancellationToken cancellationToken = default);
    }

    public record TableSchemaInfo(string SchemaName, string TableName);
}
