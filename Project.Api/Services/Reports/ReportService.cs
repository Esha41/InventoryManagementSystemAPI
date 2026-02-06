using DevExpress.CodeParser;
using DevExpress.DocumentServices.ServiceModel.DataContracts;
using DevExpress.Office.Utils;
using DevExpress.XtraRichEdit.Import.Html;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Entities.Reports;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Reporting.Services.Reports.Dtos;
using Project.Api.Services.Reports.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Microsoft.EntityFrameworkCore;

namespace Ettad.Reporting.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICrossCuttingRepository<ReportEntity> _reportRepository;
        private readonly ICrossCuttingRepository<ReportStatus> _reportStatusRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ReportService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        //private static readonly HashSet<string> ExcludedTables = new(StringComparer.OrdinalIgnoreCase)
        // {
        //     "__EFMigrationsHistory",
        //     "sysdiagrams"
        // };

        public ReportService(
            ICrossCuttingRepository<ReportEntity> reportRepository,
            ICrossCuttingRepository<ReportStatus> reportStatusRepository,
            ICurrentUserService currentUserService, ApplicationDbContext context,
            ILogger<ReportService> logger,
            IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _reportRepository = reportRepository;
            _reportStatusRepository = reportStatusRepository;
            _currentUserService = currentUserService;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<APIOperationResponse<List<ReportDto>>> GetAllAsync()
        {
            _logger.LogInformation("Getting all reports. User: {UserId}", _currentUserService.UserId);

            try
            {
                System.Linq.Expressions.Expression<System.Func<ReportEntity, bool>> filter;

                filter = r => r.ReportStatusId != (int)ReportStatuses.Inactive;

                var reports = await _reportRepository.FindAsync(
                                r => r.ReportStatusId != (int)ReportStatuses.Inactive,
                                false,
                                nameof(ReportEntity.ReportStatus)
                            );

                var dtos = reports.Select(r => new ReportDto
                {
                    Id = r.Id,
                    ReportName = r.ReportName,
                    ReportStatusId = r.ReportStatusId,
                    ReportStatusNameEn = r.ReportStatus?.NameEn ?? string.Empty,
                    ReportStatusNameAr = r.ReportStatus?.NameAr ?? string.Empty,
                    Url = r.Url,
                    Description = r.Description,
                    LayoutData = r.LayoutData,
                    ReportParameters = r.ReportParameters,
                    CreationDate = r.CreationDate,
                    CreatedBy = r.CreatedBy,
                    ModificationDate = r.ModificationDate,
                    ModifiedBy = r.ModifiedBy,
                    DeletionDate = r.DeletionDate,
                    DeletedBy = r.DeletedBy
                }).OrderBy(x=>x.CreationDate).ToList();

                _logger.LogInformation("Retrieved {Count} reports. User: {UserId}", dtos.Count, _currentUserService.UserId);
                return APIOperationResponse<List<ReportDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all reports. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<ReportDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<ReportDto>>> GetPublicReportsAsync()
        {
            _logger.LogInformation("Getting all public reports. User: {UserId}", _currentUserService.UserId);

            try
            {
                var reports = await _reportRepository.FindAsync(
                                r => r.ReportStatusId == (int)ReportStatuses.Published,
                                false,
                                nameof(ReportEntity.ReportStatus)
                            );

                var dtos = reports.Select(r => new ReportDto
                {
                    Id = r.Id,
                    ReportName = r.ReportName,
                    ReportStatusId = r.ReportStatusId,
                    ReportStatusNameEn = r.ReportStatus?.NameEn ?? string.Empty,
                    ReportStatusNameAr = r.ReportStatus?.NameAr ?? string.Empty,
                    Url = r.Url,
                    Description = r.Description,
                    LayoutData = r.LayoutData,
                    ReportParameters = r.ReportParameters,
                    CreationDate = r.CreationDate,
                    CreatedBy = r.CreatedBy,
                    ModificationDate = r.ModificationDate,
                    ModifiedBy = r.ModifiedBy,
                    DeletionDate = r.DeletionDate,
                    DeletedBy = r.DeletedBy
                }).OrderBy(x => x.CreationDate).ToList();

                _logger.LogInformation("Retrieved {Count} public reports. User: {UserId}", dtos.Count, _currentUserService.UserId);
                return APIOperationResponse<List<ReportDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving public reports. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<ReportDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<ReportDto>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Getting report by ID. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);

            try
            {
                var report = await _reportRepository.FindOneAsync(
                  x => x.Id == id && x.ReportStatusId != (int)ReportStatuses.Inactive,
                  false,
                  nameof(ReportEntity.ReportStatus));

                if (report == null)
                {
                    _logger.LogWarning("Report not found. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<ReportDto>.Fail(ResponseType.NotFound, "Report not found");
                }

                var dto = new ReportDto
                {
                    Id = report.Id,
                    ReportName = report.ReportName,
                    ReportStatusId = report.ReportStatusId,
                    ReportStatusNameEn = report.ReportStatus?.NameEn ?? string.Empty,
                    ReportStatusNameAr = report.ReportStatus?.NameAr ?? string.Empty,
                    Url = report.Url,
                    Description = report.Description,
                    LayoutData = report.LayoutData,
                    ReportParameters = report.ReportParameters,
                    CreationDate = report.CreationDate,
                    CreatedBy = report.CreatedBy,
                    ModificationDate = report.ModificationDate,
                    ModifiedBy = report.ModifiedBy,
                    DeletionDate = report.DeletionDate,
                    DeletedBy = report.DeletedBy
                };

                _logger.LogInformation("Report retrieved successfully. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<ReportDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving report by ID. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<ReportDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<ReportDto>> GetByUrlAsync(string url)
        {
            _logger.LogInformation("Getting report by URL. Url: {Url}, User: {UserId}", url, _currentUserService.UserId);

            try
            {
                if (string.IsNullOrWhiteSpace(url))
                    return APIOperationResponse<ReportDto>.BadRequest("URL is required");
                var report = await _reportRepository.FindOneAsync(
                                      x => x.Url == url && x.ReportStatusId != (int)ReportStatuses.Inactive,
                                      false,
                                      nameof(ReportEntity.ReportStatus));

                if (report == null)
                {
                    _logger.LogWarning("Report not found by URL. Url: {Url}, User: {UserId}", url, _currentUserService.UserId);
                    return APIOperationResponse<ReportDto>.Fail(ResponseType.NotFound, "Report not found");
                }

                var dto = new ReportDto
                {
                    Id = report.Id,
                    ReportName = report.ReportName,
                    ReportStatusId = report.ReportStatusId,
                    ReportStatusNameEn = report.ReportStatus?.NameEn ?? string.Empty,
                    ReportStatusNameAr = report.ReportStatus?.NameAr ?? string.Empty,
                    Url = report.Url,
                    Description = report.Description,
                    LayoutData = report.LayoutData,
                    ReportParameters = report.ReportParameters,
                    CreationDate = report.CreationDate,
                    CreatedBy = report.CreatedBy,
                    ModificationDate = report.ModificationDate,
                    ModifiedBy = report.ModifiedBy,
                    DeletionDate = report.DeletionDate,
                    DeletedBy = report.DeletedBy
                };

                _logger.LogInformation("Report retrieved successfully by URL. ReportId: {ReportId}, Url: {Url}, User: {UserId}", 
                    report.Id, url, _currentUserService.UserId);
                return APIOperationResponse<ReportDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving report by URL. Url: {Url}, User: {UserId}", url, _currentUserService.UserId);
                return APIOperationResponse<ReportDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<string>> CreateAsync(CreateReportDto dto)
        {
            _logger.LogInformation("Creating report. ReportName: {ReportName}, User: {UserId}", dto.ReportName, _currentUserService.UserId);

            try
            {
                // Check if report name already exists

                //var reportEntity = await _reportRepository.FindOneAsync(
                //                      x => x.ReportName == dto.ReportName && x.ReportStatusId != (int)ReportStatuses.Inactive,
                //                      false,
                //                      nameof(ReportEntity.ReportStatus));
                //if (reportEntity!=null)
                //{
                //    return APIOperationResponse<Guid>.BadRequest("Report with this name already exists, give another.");
                //}

                var reportId = Guid.NewGuid();
                var report = new ReportEntity
                {
                    Id = reportId,
                    ReportName = dto.ReportName,
                    ReportStatusId = dto.ReportStatusId,
                    Url = reportId + "/" + dto.Url,
                    Description = dto.Description,
                    LayoutData = dto.LayoutData,
                    ReportParameters = dto.ReportParameters,
                    CreationDate = _dateTimeProvider.Now,
                    CreatedBy = _currentUserService.UserId ?? string.Empty
                };

                await _reportRepository.AddAsync(report);

                _logger.LogInformation("Report created successfully. ReportId: {ReportId}, ReportName: {ReportName}, User: {UserId}", 
                    report.Id, dto.ReportName, _currentUserService.UserId);

                return APIOperationResponse<string>.Success(report.Url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating report. ReportName: {ReportName}, User: {UserId}", dto.ReportName, _currentUserService.UserId);
                return APIOperationResponse<string>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(Guid id, UpdateReportDto dto)
        {
            _logger.LogInformation("Updating report. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);

            try
            {
                var report = await _reportRepository.FindOneAsync(
                  x => x.Id == id && x.ReportStatusId != (int)ReportStatuses.Inactive,
                  false,
                  nameof(ReportEntity.ReportStatus));

                if (report == null)
                {
                    _logger.LogWarning("Report not found for update. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Report not found");
                }

                // Check if report name already exists (excluding current report)
                if (report.ReportName != dto.ReportName)
                {
                    var reportEntity = await _reportRepository.FindOneAsync(r => r.ReportName == dto.ReportName && r.ReportStatusId != (int)ReportStatuses.Inactive);
                    if (reportEntity != null)
                    {
                        return APIOperationResponse<bool>.BadRequest("Report with this name already exists, give another.");
                    }
                }

                report.ReportName = dto.ReportName;
                report.ReportStatusId = dto.ReportStatusId;
                report.Url = dto.Url;
                report.Description = dto.Description;
                report.LayoutData = dto.LayoutData;
                report.ReportParameters = dto.ReportParameters;
                report.ModificationDate = _dateTimeProvider.Now;
                report.ModifiedBy = _currentUserService.UserId;

                await _reportRepository.UpdateAsync(report);

                _logger.LogInformation("Report updated successfully. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating report. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<ReportDto>> SetReportPublicAsync(Guid id, bool isPublic)
        {
            _logger.LogInformation("Setting report public/private. ReportId: {ReportId}, IsPublic: {IsPublic}, User: {UserId}", id, isPublic, _currentUserService.UserId);

            try
            {
                var report = await _context.Reports
                    .Include(r => r.ReportStatus)
                    .FirstOrDefaultAsync(r => r.Id == id && r.ReportStatusId != (int)ReportStatuses.Inactive);

                if (report == null)
                {
                    _logger.LogWarning("Report not found. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<ReportDto>.Fail(ResponseType.NotFound, "Report not found");
                }

                report.ReportStatusId = isPublic ? (int)ReportStatuses.Published : (int)ReportStatuses.Draft;
                report.ModificationDate = _dateTimeProvider.Now;
                report.ModifiedBy = _currentUserService.UserId;

                await _reportRepository.UpdateAsync(report);
                await _context.SaveChangesAsync();

                var updated = await _context.Reports
                    .Include(r => r.ReportStatus)
                    .FirstAsync(r => r.Id == id);

                var dto = new ReportDto
                {
                    Id = updated.Id,
                    ReportName = updated.ReportName,
                    ReportStatusId = updated.ReportStatusId,
                    ReportStatusNameEn = updated.ReportStatus?.NameEn ?? string.Empty,
                    ReportStatusNameAr = updated.ReportStatus?.NameAr ?? string.Empty,
                    Url = updated.Url,
                    Description = updated.Description,
                    LayoutData = updated.LayoutData,
                    ReportParameters = updated.ReportParameters,
                    CreationDate = updated.CreationDate,
                    CreatedBy = updated.CreatedBy,
                    ModificationDate = updated.ModificationDate,
                    ModifiedBy = updated.ModifiedBy,
                    IsDeleted = updated.IsDeleted,
                    DeletionDate = updated.DeletionDate,
                    DeletedBy = updated.DeletedBy
                };

                _logger.LogInformation("Report set to {Status}. ReportId: {ReportId}, User: {UserId}", isPublic ? "Published" : "Draft", id, _currentUserService.UserId);
                return APIOperationResponse<ReportDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting report public/private. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<ReportDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting report. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);

            try
            {
                var report = await _reportRepository.FindOneAsync(
                                  x => x.Id == id && x.ReportStatusId != (int)ReportStatuses.Inactive,
                                  false,
                                  nameof(ReportEntity.ReportStatus));
              
                if (report == null)
                {
                    _logger.LogWarning("Report not found for deletion. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Report not found");
                }

                // Soft delete via status
                report.ReportStatusId = (int)ReportStatuses.Inactive;
                report.IsDeleted = true;
                report.DeletionDate = _dateTimeProvider.Now;
                report.DeletedBy = _currentUserService.UserId;

                await _reportRepository.UpdateAsync(report);

                _logger.LogInformation("Report deleted successfully. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting report. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<ReportStatusDto>>> GetReportStatusesAsync()
        {
            _logger.LogInformation("Getting all report statuses. User: {UserId}", _currentUserService.UserId);

            try
            {
                var statuses = await _reportStatusRepository.GetAllAsync();

                var dtos = statuses.Select(s => new ReportStatusDto
                {
                    Id = s.Id,
                    NameEn = s.NameEn,
                    NameAr = s.NameAr
                }).ToList();

                _logger.LogInformation("Retrieved {Count} report statuses. User: {UserId}", dtos.Count, _currentUserService.UserId);
                return APIOperationResponse<List<ReportStatusDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving report statuses. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<ReportStatusDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<ReportTemplateDto>>> GetTemplatesAsync()
        {
            _logger.LogInformation("Getting all report templates. User: {UserId}", _currentUserService.UserId);

            try
            {
                // Get user department and permissions if needed for filtering
                var userDepartmentId = _currentUserService.DepartmentId;
                var canViewAll = _currentUserService.IsSuperAdmin;

                var templates = new List<ReportTemplateDto>
                {
                    new ReportTemplateDto
                    {
                        Url = "BaseReportTemplate",
                        Name = "Base Report Template",
                        Description = "A basic report template with standard sections"
                    },
                    new ReportTemplateDto
                    {
                        Url = "AllowanceItemsReportTemplate",
                        Name = "Allowance Items Report",
                        Description = "Template for allowance items reporting"
                    }
                };

                // You can add filtering logic here based on user permissions/department if needed
                // For example:
                // if (!canViewAll && userDepartmentId.HasValue)
                // {
                //     // Filter templates based on department
                // }

                _logger.LogInformation("Retrieved {Count} report templates. User: {UserId}", templates.Count, _currentUserService.UserId);
                return APIOperationResponse<List<ReportTemplateDto>>.Success(templates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving report templates. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<ReportTemplateDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        //public async Task<IReadOnlyList<TableSchemaInfo>> GetTableNamesAsync(CancellationToken cancellationToken = default)
        //{
        //    const string sql = @"
        //                        SELECT 
        //                            t.TABLE_SCHEMA AS SchemaName, 
        //                            t.TABLE_NAME AS TableName 
        //                        FROM INFORMATION_SCHEMA.TABLES t
        //                        WHERE t.TABLE_TYPE = 'BASE TABLE' 
        //                          AND t.TABLE_CATALOG = DB_NAME()
        //                          AND t.TABLE_NAME NOT IN ('__EFMigrationsHistory', 'sysdiagrams')
        //                        ORDER BY t.TABLE_SCHEMA, t.TABLE_NAME";

        //    var rows = await _context.Database
        //        .SqlQueryRaw<TableSchemaInfoDto>(sql)
        //        .ToListAsync(cancellationToken);

        //    return rows
        //        .Select(r => new TableSchemaInfo(r.SchemaName ?? "dbo", r.TableName ?? ""))
        //        .Where(t => !string.IsNullOrEmpty(t.TableName) && !ExcludedTables.Contains(t.TableName))
        //        .ToList();
        //}

        //public async Task<APIOperationResponse<Guid>> ImportAsync(IFormFile file, string? reportName = null, string? url = null, string? description = null)
        //{
        //    _logger.LogInformation("Importing report from file. FileName: {FileName}, User: {UserId}", file?.FileName, _currentUserService.UserId);

        //    try
        //    {
        //        if (file == null || file.Length == 0)
        //        {
        //            return APIOperationResponse<Guid>.BadRequest("No file provided");
        //        }

        //        // Validate file extension (.repx or .xml)
        //        var allowedExtensions = new[] { ".repx", ".xml" };
        //        var fileExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
        //        if (string.IsNullOrEmpty(fileExtension) || !allowedExtensions.Contains(fileExtension))
        //        {
        //            return APIOperationResponse<Guid>.BadRequest($"Invalid file type. Only {string.Join(", ", allowedExtensions)} files are allowed.");
        //        }

        //        // Validate file size (max 10MB)
        //        const long maxFileSize = 10 * 1024 * 1024; // 10MB
        //        if (file.Length > maxFileSize)
        //        {
        //            return APIOperationResponse<Guid>.BadRequest($"File size exceeds maximum allowed size of {maxFileSize / (1024 * 1024)}MB");
        //        }

        //        // Read file content
        //        byte[] layoutData;
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            await file.CopyToAsync(memoryStream);
        //            layoutData = memoryStream.ToArray();
        //        }

        //        // Validate XML format
        //        try
        //        {
        //            var xmlContent = Encoding.UTF8.GetString(layoutData);
        //            XDocument.Parse(xmlContent); // Validate XML structure
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogWarning(ex, "Invalid XML format in imported file. FileName: {FileName}", file.FileName);
        //            return APIOperationResponse<Guid>.BadRequest("Invalid file format. The file must be a valid DevExpress report XML file.");
        //        }

        //        // Extract report name from filename if not provided
        //        var finalReportName = reportName ?? Path.GetFileNameWithoutExtension(file.FileName);
        //        if (string.IsNullOrWhiteSpace(finalReportName))
        //        {
        //            finalReportName = $"Imported Report {DateTime.Now:yyyyMMddHHmmss}";
        //        }

        //        // Check if report name already exists
        //        var reportEntity = await _reportRepository.FindOneAsync(r => r.ReportName == finalReportName && r.ReportStatusId != (int)ReportStatuses.Inactive);
        //        if (reportEntity != null)
        //        {
        //            return APIOperationResponse<Guid>.BadRequest("Report with this name already exists, give another.");
        //        }

        //        // Generate unique URL from report name if not provided
        //        var baseUrl = !string.IsNullOrWhiteSpace(url) 
        //            ? url.Trim() 
        //            : GenerateUrlFromName(finalReportName);
                
        //        // Validate URL format
        //        if (!IsValidUrl(baseUrl))
        //        {
        //            return APIOperationResponse<Guid>.BadRequest("Invalid URL format. URL can only contain letters, numbers, underscores, and hyphens.");
        //        }
                
        //        var finalUrl = baseUrl;
        //        var counter = 1;

        //        while (await _context.Reports.AnyAsync(r => r.Url == finalUrl && r.ReportStatusId != (int)ReportStatuses.Inactive))
        //        {
        //            finalUrl = $"{baseUrl}_{counter}";
        //            counter++;
        //        }

        //        // Validate that ReportStatus exists (default to Draft = 1)
        //        var status = await _reportStatusRepository.FindOneAsync(s => s.Id == 1);
        //        if (status == null)
        //        {
        //            return APIOperationResponse<Guid>.BadRequest("Default report status not found");
        //        }

        //        // Create report entity
        //        var report = new ReportEntity
        //        {
        //            Id = Guid.NewGuid(),
        //            ReportName = finalReportName,
        //            ReportStatusId = 1, // Default to Draft
        //            Url = finalUrl,
        //            Description = description,
        //            LayoutData = layoutData,
        //            CreationDate = _dateTimeProvider.Now,
        //            CreatedBy = _currentUserService.UserId ?? string.Empty
        //        };

        //        await _reportRepository.AddAsync(report);

        //        _logger.LogInformation("Report imported successfully. ReportId: {ReportId}, ReportName: {ReportName}, User: {UserId}", 
        //            report.Id, finalReportName, _currentUserService.UserId);

        //        return APIOperationResponse<Guid>.Success(report.Id);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error importing report. FileName: {FileName}, User: {UserId}", file?.FileName, _currentUserService.UserId);
        //        return APIOperationResponse<Guid>.Fail(ResponseType.InternalServerError, $"An error occurred while importing the report: {ex.Message}");
        //    }
        //}

        private string GenerateUrlFromName(string name)
        {
            // Convert report name to URL-friendly format
            var url = name.Trim();
            // Replace spaces and special characters with underscores
            url = System.Text.RegularExpressions.Regex.Replace(url, @"[^a-zA-Z0-9_-]", "_");
            // Remove multiple consecutive underscores
            url = System.Text.RegularExpressions.Regex.Replace(url, @"_+", "_");
            // Remove leading/trailing underscores
            url = url.Trim('_');
            // Ensure it's not empty
            if (string.IsNullOrEmpty(url))
            {
                url = $"report_{DateTime.Now:yyyyMMddHHmmss}";
            }
            return url;
        }

        private bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            // Check for invalid characters (only allow alphanumeric, underscore, hyphen)
            if (System.Text.RegularExpressions.Regex.IsMatch(url, @"[^a-zA-Z0-9_-]"))
            {
                return false;
            }

            // Check for path traversal attempts
            if (url.Contains("..") || url.Contains("/") || url.Contains("\\"))
            {
                return false;
            }

            // Check length
            if (url.Length > 500)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> IsReportExists(string name)
        {
            var reportExist = false;

            var report = await _reportRepository.FindOneAsync(
                                  x => x.ReportName == name && x.ReportStatusId != (int)ReportStatuses.Inactive,
                                  false,
                                  nameof(ReportEntity.ReportStatus));

            if (report == null)
                return reportExist;
            reportExist = true;

            return reportExist;

        }
    }
}
