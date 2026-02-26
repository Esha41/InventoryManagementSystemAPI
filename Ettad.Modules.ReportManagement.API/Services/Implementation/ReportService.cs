using AutoMapper;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Enums;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ettad.Modules.ReportManagement.API.Services.Dtos;
using Ettad.Data.Entities;
using Ettad.Modules.ReportManagement.API.Services.Interfaces;

namespace Ettad.Modules.ReportManagement.API.Services.Implementation
{
    public class ReportService : IReportService
    {
        private readonly ICrossCuttingRepository<ReportEntity> _reportRepository;
        private readonly ICrossCuttingRepository<ReportStatus> _reportStatusRepository;
        private readonly ICrossCuttingRepository<ReportRole> _reportRoleRepository;
        private readonly ICrossCuttingRepository<ScheduledReport> _scheduledReportRepository;
        private readonly ICrossCuttingRepository<ScheduledReportRecipient> _scheduledReportRecipientRepository;
        private readonly ICrossCuttingRepository<ScheduledReportExecution> _scheduledReportExecutionRepository;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ReportService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IMapper _mapper;

        public ReportService(
            ICrossCuttingRepository<ReportEntity> reportRepository,
            ICrossCuttingRepository<ReportStatus> reportStatusRepository,
            ICrossCuttingRepository<ReportRole> reportRoleRepository,
            ICrossCuttingRepository<ScheduledReport> scheduledReportRepository,
            ICrossCuttingRepository<ScheduledReportRecipient> scheduledReportRecipientRepository,
            ICrossCuttingRepository<ScheduledReportExecution> scheduledReportExecutionRepository,
            RoleManager<ApplicationRole> roleManager,
            ICurrentUserService currentUserService,
            ILogger<ReportService> logger,
            IDateTimeProvider dateTimeProvider,
            IMapper mapper)
        {
            _reportRepository = reportRepository;
            _reportStatusRepository = reportStatusRepository;
            _reportRoleRepository = reportRoleRepository;
            _scheduledReportRepository = scheduledReportRepository;
            _scheduledReportRecipientRepository = scheduledReportRecipientRepository;
            _scheduledReportExecutionRepository = scheduledReportExecutionRepository;
            _roleManager = roleManager;
            _currentUserService = currentUserService;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _mapper = mapper;
        }

        public async Task<APIOperationResponse<List<ReportDto>>> GetAllAsync()
        {
            _logger.LogInformation("Getting all reports. User: {UserId}", _currentUserService.UserId);

            try
            {
                System.Linq.Expressions.Expression<Func<ReportEntity, bool>> filter;

                filter = r => r.ReportStatusId != (int)ReportStatuses.Inactive;

                var reports = await _reportRepository.FindAsync(
                                r => r.ReportStatusId != (int)ReportStatuses.Inactive,
                                false,
                                nameof(ReportEntity.ReportStatus)
                            );

                // Get all report IDs
                var reportIds = reports.Select(r => r.Id).ToList();

                // Get all role associations for these reports
                var reportRoles = (await _reportRoleRepository.FindAsync(
                    rr => reportIds.Contains(rr.ReportId) && !rr.IsDeleted,
                    false,
                    nameof(ReportRole.Role)
                )).ToList();

                // Group roles by report ID
                var rolesByReportId = reportRoles
                    .GroupBy(rr => rr.ReportId)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(rr => new ReportRoleDto
                        {
                            RoleId = rr.RoleId,
                            RoleName = rr.Role?.Name ?? string.Empty,
                            RoleNameEn = rr.Role?.Name,
                            RoleNameAr = rr.Role?.NameAr
                        }).ToList()
                    );

                var dtos = reports.Select(r =>
                {
                    var dto = _mapper.Map<ReportDto>(r);
                    dto.Roles = rolesByReportId.ContainsKey(r.Id) ? rolesByReportId[r.Id] : null;
                    return dto;
                }).OrderBy(x => x.CreationDate).ToList();

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
                var isSuperAdmin = _currentUserService.IsSuperAdmin;
                var userRoleNames = _currentUserService.Roles ?? new List<string>();

                // Convert role names to role IDs
                var userRoleIds = new List<string>();
                if (userRoleNames.Any())
                {
                    userRoleIds = await _roleManager.Roles
                        .Where(r => userRoleNames.Contains(r.Name) || userRoleNames.Contains(r.NameAr))
                        .Select(r => r.Id)
                        .ToListAsync();

                    _logger.LogInformation("User has {RoleCount} roles. Role names: {RoleNames}, Role IDs: {RoleIds}",
                        userRoleIds.Count,
                        string.Join(", ", userRoleNames),
                        string.Join(", ", userRoleIds));
                }

                // Get all published reports
                var reportsList = (await _reportRepository.FindAsync(
                    r => r.ReportStatusId == (int)ReportStatuses.Published && !r.IsDeleted,
                    false,
                    nameof(ReportEntity.ReportStatus)
                )).ToList();

                if (!isSuperAdmin)
                {
                    // Get role associations for these reports
                    var reportIds = reportsList.Select(r => r.Id).ToList();
                    var allReportRoles = (await _reportRoleRepository.FindAsync(
                        rr => reportIds.Contains(rr.ReportId) && !rr.IsDeleted,
                        false
                    )).ToList();

                    var rolesByReportId = allReportRoles
                        .GroupBy(rr => rr.ReportId)
                        .ToDictionary(g => g.Key, g => g.Select(rr => rr.RoleId).ToList());

                    reportsList = reportsList.Where(r =>
                        // No role restrictions (report has no roles assigned)
                        !rolesByReportId.ContainsKey(r.Id)
                        // OR user has a matching role
                        || rolesByReportId[r.Id].Any(roleId => userRoleIds.Contains(roleId))
                    ).ToList();
                }

                var dtos = _mapper.Map<List<ReportDto>>(reportsList)
                    .OrderBy(x => x.CreationDate)
                    .ToList();

                _logger.LogInformation("Retrieved {Count} public reports. User: {UserId}", dtos.Count, _currentUserService.UserId);

                return APIOperationResponse<List<ReportDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving public reports. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<ReportDto>>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred: {ex.Message}");
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

                var dto = _mapper.Map<ReportDto>(report);

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

                var dto = _mapper.Map<ReportDto>(report);

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
                var reportId = Guid.NewGuid();
                var report = _mapper.Map<ReportEntity>(dto);
                report.Id = reportId;
                report.Url = reportId + "/" + dto.Url;
                report.CreationDate = _dateTimeProvider.Now;
                report.CreatedBy = _currentUserService.UserId ?? string.Empty;

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

                _mapper.Map(dto, report);
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

        public async Task<APIOperationResponse<ReportDto>> SetReportPublicAsync(Guid id, SetReportPublicDto dto)
        {
            _logger.LogInformation("Setting report public/private. ReportId: {ReportId}, IsPublic: {IsPublic}, RoleIds: {RoleIds}, User: {UserId}",
                id, dto.IsPublic, string.Join(",", dto.RoleIds ?? new List<string>()), _currentUserService.UserId);

            try
            {
                var report = await _reportRepository.FindOneAsync(
                    r => r.Id == id && r.ReportStatusId != (int)ReportStatuses.Inactive,
                    false);

                if (report == null)
                {
                    _logger.LogWarning("Report not found. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<ReportDto>.Fail(ResponseType.NotFound, "Report not found");
                }

                // Update report status
                report.ReportStatusId = dto.IsPublic ? (int)ReportStatuses.Published : (int)ReportStatuses.Draft;
                report.ModificationDate = _dateTimeProvider.Now;
                report.ModifiedBy = _currentUserService.UserId;

                // Save report update first before handling roles
                await _reportRepository.UpdateAsync(report);

                // Handle role associations
                if (dto.IsPublic)
                {
                    // Get existing role associations for this report
                    var existingReportRoles = (await _reportRoleRepository.FindAsync(
                        rr => rr.ReportId == id && !rr.IsDeleted,
                        true
                    )).ToList();

                    // Get the set of role IDs that should remain (from the DTO)
                    var targetRoleIds = dto.RoleIds != null ? dto.RoleIds.Distinct().ToHashSet() : new HashSet<string>();

                    // Verify that all provided role IDs exist in the database
                    if (targetRoleIds.Any())
                    {
                        var validRoleIds = await _roleManager.Roles
                            .Where(r => targetRoleIds.Contains(r.Id))
                            .Select(r => r.Id)
                            .ToListAsync();

                        var invalidRoleIds = targetRoleIds.Except(validRoleIds).ToList();
                        if (invalidRoleIds.Any())
                        {
                            _logger.LogWarning("Invalid role IDs provided: {InvalidRoleIds}", string.Join(",", invalidRoleIds));
                            return APIOperationResponse<ReportDto>.BadRequest($"Invalid role IDs: {string.Join(", ", invalidRoleIds)}");
                        }

                        // Update targetRoleIds to only include valid ones
                        targetRoleIds = validRoleIds.ToHashSet();
                    }

                    // Hard delete roles that are no longer selected (exist in DB but not in target list)
                    var rolesToDeleteIds = existingReportRoles
                        .Where(rr => !targetRoleIds.Contains(rr.RoleId))
                        .Select(rr => rr.RoleId)
                        .ToList();

                    if (rolesToDeleteIds.Any())
                    {
                        _logger.LogInformation("Hard deleting {Count} role associations for report {ReportId}", rolesToDeleteIds.Count, id);

                        var deletedCount = await _reportRoleRepository
                            .Find(rr => rr.ReportId == id && rolesToDeleteIds.Contains(rr.RoleId) && !rr.IsDeleted, includeSoftDeleted: true)
                            .ExecuteDeleteAsync();

                        _logger.LogInformation("Successfully hard deleted {DeletedCount} role associations for report {ReportId}", deletedCount, id);
                    }

                    // Add new role associations that don't already exist
                    var existingRoleIds = existingReportRoles
                        .Where(rr => targetRoleIds.Contains(rr.RoleId))
                        .Select(rr => rr.RoleId)
                        .ToHashSet();

                    foreach (var roleId in targetRoleIds)
                    {
                        if (!existingRoleIds.Contains(roleId))
                        {
                            var reportRole = new ReportRole
                            {
                                Id = Guid.NewGuid(),
                                ReportId = id,
                                RoleId = roleId,
                                CreationDate = _dateTimeProvider.Now,
                                CreatedBy = _currentUserService.UserId ?? string.Empty
                            };
                            await _reportRoleRepository.AddAsync(reportRole);
                        }
                    }
                }
                // When making private, preserve role associations (don't delete them)
                // This allows roles to be preserved when toggling between public and private

                var updated = await _reportRepository.FindOneAsync(
                    r => r.Id == id,
                    false,
                    nameof(ReportEntity.ReportStatus));

                // Get role associations for this report (preserve them even when making private)
                var reportRoles = (await _reportRoleRepository.FindAsync(
                    rr => rr.ReportId == id && !rr.IsDeleted,
                    false,
                    nameof(ReportRole.Role)
                )).ToList();

                var roles = reportRoles.Select(rr => new ReportRoleDto
                {
                    RoleId = rr.RoleId,
                    RoleName = rr.Role?.Name ?? string.Empty,
                    RoleNameEn = rr.Role?.Name,
                    RoleNameAr = rr.Role?.NameAr
                }).ToList();

                // If no roles, set to null (means accessible to all users)
                if (!roles.Any())
                {
                    roles = null;
                }

                var resultDto = _mapper.Map<ReportDto>(updated);
                resultDto.Roles = roles;

                _logger.LogInformation("Report set to {Status}. ReportId: {ReportId}, User: {UserId}", dto.IsPublic ? "Published" : "Draft", id, _currentUserService.UserId);
                return APIOperationResponse<ReportDto>.Success(resultDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting report public/private. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<ReportDto>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Permanently deleting report. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);

            try
            {
                var report = await _reportRepository.FindOneAsync(x => x.Id == id, includeSoftDeleted: true);

                if (report == null)
                {
                    _logger.LogWarning("Report not found for deletion. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Report not found");
                }

                // Get all scheduled report IDs for this report
                var scheduledReportIds = _scheduledReportRepository
                    .Find(sr => sr.ReportId == id, includeSoftDeleted: true)
                    .Select(sr => sr.Id)
                    .ToList();

                if (scheduledReportIds.Any())
                {
                    // Permanently delete scheduled report execution history
                    var deletedExecutions = await _scheduledReportExecutionRepository
                        .Find(e => scheduledReportIds.Contains(e.ScheduledReportId), includeSoftDeleted: true)
                        .ExecuteDeleteAsync();
                    _logger.LogInformation("Permanently deleted {Count} scheduled report executions for report {ReportId}", deletedExecutions, id);

                    // Permanently delete scheduled report recipients
                    var deletedRecipients = await _scheduledReportRecipientRepository
                        .Find(r => scheduledReportIds.Contains(r.ScheduledReportId), includeSoftDeleted: true)
                        .ExecuteDeleteAsync();
                    _logger.LogInformation("Permanently deleted {Count} scheduled report recipients for report {ReportId}", deletedRecipients, id);

                    // Permanently delete scheduled reports
                    var deletedSchedules = await _scheduledReportRepository
                        .Find(sr => sr.ReportId == id, includeSoftDeleted: true)
                        .ExecuteDeleteAsync();
                    _logger.LogInformation("Permanently deleted {Count} scheduled reports for report {ReportId}", deletedSchedules, id);
                }

                // Permanently delete report role associations
                var deletedRoles = await _reportRoleRepository
                    .Find(rr => rr.ReportId == id, includeSoftDeleted: true)
                    .ExecuteDeleteAsync();
                _logger.LogInformation("Permanently deleted {Count} report role associations for report {ReportId}", deletedRoles, id);

                // Permanently delete the report itself
                await _reportRepository
                    .Find(r => r.Id == id, includeSoftDeleted: true)
                    .ExecuteDeleteAsync();

                _logger.LogInformation("Report permanently deleted successfully. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error permanently deleting report. ReportId: {ReportId}, User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<APIOperationResponse<List<ReportStatusDto>>> GetReportStatusesAsync()
        {
            _logger.LogInformation("Getting all report statuses. User: {UserId}", _currentUserService.UserId);

            try
            {
                var statuses = await _reportStatusRepository.GetAllAsync();

                var dtos = _mapper.Map<List<ReportStatusDto>>(statuses);

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

                // add filtering logic here based on user permissions/department if needed
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

        public async Task<APIOperationResponse<List<string>>> GetReportRoleIdsAsync(Guid reportId)
        {
            _logger.LogInformation("Getting role IDs for report. ReportId: {ReportId}, User: {UserId}", reportId, _currentUserService.UserId);

            try
            {
                var roleIds = _reportRoleRepository
                    .Find(rr => rr.ReportId == reportId && !rr.IsDeleted)
                    .Select(rr => rr.RoleId)
                    .ToList();

                _logger.LogInformation("Retrieved {Count} role IDs for report. ReportId: {ReportId}, User: {UserId}", roleIds.Count, reportId, _currentUserService.UserId);
                return APIOperationResponse<List<string>>.Success(roleIds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving role IDs for report. ReportId: {ReportId}, User: {UserId}", reportId, _currentUserService.UserId);
                return APIOperationResponse<List<string>>.Fail(ResponseType.InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

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

        //private string GenerateUrlFromName(string name)
        //{
        //    // Convert report name to URL-friendly format
        //    var url = name.Trim();
        //    // Replace spaces and special characters with underscores
        //    url = System.Text.RegularExpressions.Regex.Replace(url, @"[^a-zA-Z0-9_-]", "_");
        //    // Remove multiple consecutive underscores
        //    url = System.Text.RegularExpressions.Regex.Replace(url, @"_+", "_");
        //    // Remove leading/trailing underscores
        //    url = url.Trim('_');
        //    // Ensure it's not empty
        //    if (string.IsNullOrEmpty(url))
        //    {
        //        url = $"report_{DateTime.Now:yyyyMMddHHmmss}";
        //    }
        //    return url;
        //}

        //private bool IsValidUrl(string url)
        //{
        //    if (string.IsNullOrWhiteSpace(url))
        //    {
        //        return false;
        //    }

        //    // Check for invalid characters (only allow alphanumeric, underscore, hyphen)
        //    if (System.Text.RegularExpressions.Regex.IsMatch(url, @"[^a-zA-Z0-9_-]"))
        //    {
        //        return false;
        //    }

        //    // Check for path traversal attempts
        //    if (url.Contains("..") || url.Contains("/") || url.Contains("\\"))
        //    {
        //        return false;
        //    }

        //    // Check length
        //    if (url.Length > 500)
        //    {
        //        return false;
        //    }

        //    return true;
        //}
    }
}
