using Ettad.CrossCutting.Data.Repository;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Entities.Reports;
using Ettad.Reporting.Services.Reports.Dtos;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.EntityFramework.DataBaseContext;

namespace Ettad.Reporting.Services
{
    public class ReportService : IReportService
    {
        private readonly ICrossCuttingRepository<ReportEntity> _reportRepository;
        private readonly ICrossCuttingRepository<ReportStatus> _reportStatusRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ReportService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ReportService(
            ICrossCuttingRepository<ReportEntity> reportRepository,
            ICrossCuttingRepository<ReportStatus> reportStatusRepository,
            ICurrentUserService currentUserService,
            ILogger<ReportService> logger,
            ApplicationDbContext context,
            IDateTimeProvider dateTimeProvider)
        {
            _reportRepository = reportRepository;
            _reportStatusRepository = reportStatusRepository;
            _currentUserService = currentUserService;
            _logger = logger;
            _context = context;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<APIOperationResponse<List<ReportDto>>> GetAllAsync()
        {
            _logger.LogInformation("Getting all reports. User: {UserId}", _currentUserService.UserId);

            try
            {
                var reports = await _context.Reports
                    .Include(r => r.ReportStatus)
                    .Where(r => !r.IsDeleted)
                    .OrderByDescending(r => r.CreationDate)
                    .ToListAsync();

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
                    ReportType = r.ReportType,
                    ReportParameters = r.ReportParameters,
                    IsTemplate = r.IsTemplate,
                    IsPublic = r.IsPublic,
                    CreationDate = r.CreationDate,
                    CreatedBy = r.CreatedBy,
                    ModificationDate = r.ModificationDate,
                    ModifiedBy = r.ModifiedBy,
                    IsDeleted = r.IsDeleted,
                    DeletionDate = r.DeletionDate,
                    DeletedBy = r.DeletedBy
                }).ToList();

                _logger.LogInformation("Retrieved {Count} reports. User: {UserId}", dtos.Count, _currentUserService.UserId);
                return APIOperationResponse<List<ReportDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all reports. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<ReportDto>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<ReportDto>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Getting report by ID. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);

            try
            {
                var report = await _context.Reports
                    .Include(r => r.ReportStatus)
                    .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

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
                    ReportType = report.ReportType,
                    ReportParameters = report.ReportParameters,
                    IsTemplate = report.IsTemplate,
                    IsPublic = report.IsPublic,
                    CreationDate = report.CreationDate,
                    CreatedBy = report.CreatedBy,
                    ModificationDate = report.ModificationDate,
                    ModifiedBy = report.ModifiedBy,
                    IsDeleted = report.IsDeleted,
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
                    return APIOperationResponse<ReportDto>.Fail(ResponseType.BadRequest, "URL is required");

                var report = await _context.Reports
                    .Include(r => r.ReportStatus)
                    .FirstOrDefaultAsync(r => r.Url == url && !r.IsDeleted);

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
                    ReportType = report.ReportType,
                    ReportParameters = report.ReportParameters,
                    IsTemplate = report.IsTemplate,
                    IsPublic = report.IsPublic,
                    CreationDate = report.CreationDate,
                    CreatedBy = report.CreatedBy,
                    ModificationDate = report.ModificationDate,
                    ModifiedBy = report.ModifiedBy,
                    IsDeleted = report.IsDeleted,
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

        public async Task<APIOperationResponse<Guid>> CreateAsync(CreateReportDto dto)
        {
            _logger.LogInformation("Creating report. ReportName: {ReportName}, User: {UserId}", dto.ReportName, _currentUserService.UserId);

            try
            {
                // Validate that ReportStatus exists
                var status = await _reportStatusRepository.FindOneAsync(s => s.Id == dto.ReportStatusId && !s.IsDeleted);
                if (status == null)
                {
                    return APIOperationResponse<Guid>.Fail(ResponseType.BadRequest, "Invalid report status");
                }

                // Check if URL already exists
                var urlExists = await _context.Reports.AnyAsync(r => r.Url == dto.Url && !r.IsDeleted);
                if (urlExists)
                {
                    return APIOperationResponse<Guid>.Fail(ResponseType.BadRequest, "A report with this URL already exists");
                }

                var report = new ReportEntity
                {
                    Id = Guid.NewGuid(),
                    ReportName = dto.ReportName,
                    ReportStatusId = dto.ReportStatusId,
                    Url = dto.Url,
                    Description = dto.Description,
                    LayoutData = dto.LayoutData,
                    ReportType = dto.ReportType,
                    ReportParameters = dto.ReportParameters,
                    IsTemplate = dto.IsTemplate,
                    IsPublic = dto.IsPublic,
                    CreationDate = _dateTimeProvider.Now,
                    CreatedBy = _currentUserService.UserId ?? string.Empty
                };

                await _reportRepository.AddAsync(report);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Report created successfully. ReportId: {ReportId}, ReportName: {ReportName}, User: {UserId}", 
                    report.Id, dto.ReportName, _currentUserService.UserId);

                return APIOperationResponse<Guid>.Success(report.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating report. ReportName: {ReportName}, User: {UserId}", dto.ReportName, _currentUserService.UserId);
                return APIOperationResponse<Guid>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(Guid id, UpdateReportDto dto)
        {
            _logger.LogInformation("Updating report. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);

            try
            {
                var report = await _reportRepository.FindOneAsync(r => r.Id == id && !r.IsDeleted);

                if (report == null)
                {
                    _logger.LogWarning("Report not found for update. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Report not found");
                }

                // Validate that ReportStatus exists
                var status = await _reportStatusRepository.FindOneAsync(s => s.Id == dto.ReportStatusId && !s.IsDeleted);
                if (status == null)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Invalid report status");
                }

                // Check if URL already exists (excluding current report)
                var urlExists = await _context.Reports.AnyAsync(r => r.Url == dto.Url && r.Id != id && !r.IsDeleted);
                if (urlExists)
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "A report with this URL already exists");
                }

                report.ReportName = dto.ReportName;
                report.ReportStatusId = dto.ReportStatusId;
                report.Url = dto.Url;
                report.Description = dto.Description;
                report.LayoutData = dto.LayoutData;
                report.ReportType = dto.ReportType;
                report.ReportParameters = dto.ReportParameters;
                report.IsTemplate = dto.IsTemplate;
                report.IsPublic = dto.IsPublic;
                report.ModificationDate = _dateTimeProvider.Now;
                report.ModifiedBy = _currentUserService.UserId;

                await _reportRepository.UpdateAsync(report);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Report updated successfully. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating report. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting report. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);

            try
            {
                var report = await _reportRepository.FindOneAsync(r => r.Id == id && !r.IsDeleted);

                if (report == null)
                {
                    _logger.LogWarning("Report not found for deletion. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Report not found");
                }

                // Soft delete
                report.IsDeleted = true;
                report.DeletionDate = _dateTimeProvider.Now;
                report.DeletedBy = _currentUserService.UserId;

                await _reportRepository.UpdateAsync(report);
                await _context.SaveChangesAsync();

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
                var statuses = await _reportStatusRepository.FindAsync(s => !s.IsDeleted);

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
    }
}
