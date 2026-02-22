using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Data.Repository;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Microsoft.EntityFrameworkCore;
using Ettad.Modules.ReportManagement.API.Services.Dtos;
using Ettad.Data.Entities;
using Ettad.User.Services.Interfaces;
using Ettad.Modules.ReportManagement.API.Services.Interfaces;

namespace Ettad.Modules.ReportManagement.API.Services.Implementation
{
    public class ScheduledReportService : IScheduledReportService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICrossCuttingRepository<ScheduledReport> _scheduledReportRepository;
        private readonly ICrossCuttingRepository<ReportEntity> _reportRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ScheduledReportService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IScheduledReportExecutionService _executionService;
        private readonly IUserService _userService;

        public ScheduledReportService(
            ICrossCuttingRepository<ScheduledReport> scheduledReportRepository,
            ICrossCuttingRepository<ReportEntity> reportRepository,
            ICurrentUserService currentUserService,
            ApplicationDbContext context,
            ILogger<ScheduledReportService> logger,
            IDateTimeProvider dateTimeProvider,
            IScheduledReportExecutionService executionService,
            IUserService userService)
        {
            _context = context;
            _scheduledReportRepository = scheduledReportRepository;
            _reportRepository = reportRepository;
            _currentUserService = currentUserService;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _executionService = executionService;
            _userService = userService;
        }

        public async Task<APIOperationResponse<List<ScheduledReportDto>>> GetAllAsync()
        {
            _logger.LogInformation("Getting all scheduled reports. User: {UserId}", _currentUserService.UserId);

            try
            {
                var scheduledReports = await _scheduledReportRepository.FindAsync(
                    sr => !sr.IsDeleted,
                    false,
                    nameof(ScheduledReport.Report),
                    nameof(ScheduledReport.Recipients)
                );

                var dtos = new List<ScheduledReportDto>();

                foreach (var sr in scheduledReports)
                {
                    var dto = await MapToDtoAsync(sr);
                    dtos.Add(dto);
                }

                _logger.LogInformation("Retrieved {Count} scheduled reports. User: {UserId}", dtos.Count, _currentUserService.UserId);
                return APIOperationResponse<List<ScheduledReportDto>>.Success(dtos.OrderByDescending(x => x.CreationDate).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting scheduled reports. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<List<ScheduledReportDto>>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred: {ex.Message}"
                );
            }
        }

        public async Task<APIOperationResponse<ScheduledReportDto>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Getting scheduled report {Id}. User: {UserId}", id, _currentUserService.UserId);

            try
            {
                var scheduledReport = await _scheduledReportRepository.FindOneAsync(
                    sr => sr.Id == id && !sr.IsDeleted,
                    false,
                    nameof(ScheduledReport.Report),
                    nameof(ScheduledReport.Recipients)
                );

                if (scheduledReport == null)
                {
                    return APIOperationResponse<ScheduledReportDto>.Fail(
                        ResponseType.NotFound,
                        "Scheduled report not found"
                    );
                }

                var dto = await MapToDtoAsync(scheduledReport);
                return APIOperationResponse<ScheduledReportDto>.Success(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting scheduled report {Id}. User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<ScheduledReportDto>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred: {ex.Message}"
                );
            }
        }

        public async Task<APIOperationResponse<Guid>> CreateAsync(CreateScheduledReportDto dto)
        {
            _logger.LogInformation("Creating scheduled report. User: {UserId}", _currentUserService.UserId);

            try
            {
                // Validate report exists
                var report = await _reportRepository.FindOneAsync(
                    r => r.Id == dto.ReportId && !r.IsDeleted,
                    false
                );
                if (report == null)
                {
                    return APIOperationResponse<Guid>.Fail(
                        ResponseType.NotFound,
                        "Report not found"
                    );
                }

                var scheduledReport = new ScheduledReport
                {
                    Id = Guid.NewGuid(),
                    ScheduleName = dto.ScheduleName,
                    ReportId = dto.ReportId,
                    OutputFormat = dto.OutputFormat,
                    Frequency = dto.Frequency,
                    TimeOfDay = dto.TimeOfDay,
                    DayOfWeek = dto.DayOfWeek,
                    DayOfMonth = dto.DayOfMonth,
                    EmailSubject = dto.EmailSubject,
                    EmailBody = dto.EmailBody,
                    IsActive = true,
                    CreationDate = _dateTimeProvider.Now,
                    CreatedBy = _currentUserService.UserId ?? string.Empty
                };

                // Calculate next run date
                scheduledReport.NextRunDate = CalculateNextRunDate(
                    dto.Frequency,
                    dto.TimeOfDay,
                    dto.DayOfWeek,
                    dto.DayOfMonth
                );

                // Add recipients
                foreach (var recipientDto in dto.Recipients)
                {
                    if (string.IsNullOrWhiteSpace(recipientDto.EmailAddress) && string.IsNullOrWhiteSpace(recipientDto.UserId))
                    {
                        continue; // Skip invalid recipients
                    }

                    var recipient = new ScheduledReportRecipient
                    {
                        Id = Guid.NewGuid(),
                        ScheduledReportId = scheduledReport.Id,
                        UserId = recipientDto.UserId,
                        RecipientType = recipientDto.RecipientType,
                        CreationDate = _dateTimeProvider.Now,
                        CreatedBy = _currentUserService.UserId ?? string.Empty
                    };

                    // If UserId is provided, don't store EmailAddress (we'll fetch it dynamically)
                    // If UserId is null, store EmailAddress for manual email entries
                    if (string.IsNullOrWhiteSpace(recipientDto.UserId))
                    {
                        recipient.EmailAddress = recipientDto.EmailAddress;
                    }
                    // EmailAddress is not stored when UserId is present - it will be fetched from user record

                    scheduledReport.Recipients.Add(recipient);
                }

                await _scheduledReportRepository.AddAsync(scheduledReport);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Created scheduled report {Id}. User: {UserId}", scheduledReport.Id, _currentUserService.UserId);
                return APIOperationResponse<Guid>.Success(scheduledReport.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating scheduled report. User: {UserId}", _currentUserService.UserId);
                return APIOperationResponse<Guid>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred: {ex.Message}"
                );
            }
        }

        public async Task<APIOperationResponse<bool>> UpdateAsync(Guid id, UpdateScheduledReportDto dto)
        {
            _logger.LogInformation("Updating scheduled report {Id}. User: {UserId}", id, _currentUserService.UserId);

            try
            {
                var scheduledReport = await _scheduledReportRepository.FindOneAsync(
                    sr => sr.Id == id && !sr.IsDeleted,
                    false,
                    nameof(ScheduledReport.Recipients)
                );

                if (scheduledReport == null)
                {
                    return APIOperationResponse<bool>.Fail(
                        ResponseType.NotFound,
                        "Scheduled report not found"
                    );
                }

                // Validate report exists
                var report = await _reportRepository.FindOneAsync(
                    r => r.Id == dto.ReportId && !r.IsDeleted,
                    false
                );
                if (report == null)
                {
                    return APIOperationResponse<bool>.Fail(
                        ResponseType.NotFound,
                        "Report not found"
                    );
                }

                scheduledReport.ScheduleName = dto.ScheduleName;
                scheduledReport.ReportId = dto.ReportId;
                scheduledReport.OutputFormat = dto.OutputFormat;
                scheduledReport.Frequency = dto.Frequency;
                scheduledReport.TimeOfDay = dto.TimeOfDay;
                scheduledReport.DayOfWeek = dto.DayOfWeek;
                scheduledReport.DayOfMonth = dto.DayOfMonth;
                scheduledReport.IsActive = dto.IsActive;
                scheduledReport.EmailSubject = dto.EmailSubject;
                scheduledReport.EmailBody = dto.EmailBody;
                scheduledReport.ModificationDate = _dateTimeProvider.Now;
                scheduledReport.ModifiedBy = _currentUserService.UserId;

                // Recalculate next run date if schedule changed
                scheduledReport.NextRunDate = CalculateNextRunDate(
                    dto.Frequency,
                    dto.TimeOfDay,
                    dto.DayOfWeek,
                    dto.DayOfMonth
                );

                // Update recipients - remove existing and add new ones
                var existingRecipients = scheduledReport.Recipients.Where(r => !r.IsDeleted).ToList();
                _logger.LogInformation("Marking {Count} existing recipients as deleted for scheduled report {Id}", existingRecipients.Count, id);
                foreach (var existing in existingRecipients)
                {
                    existing.IsDeleted = true;
                    existing.DeletionDate = _dateTimeProvider.Now;
                    existing.DeletedBy = _currentUserService.UserId;
                    // Explicitly mark as modified to ensure Entity Framework tracks the soft delete
                    _context.Entry(existing).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }

                // Add new recipients - need to explicitly add to DbContext for proper tracking
                _logger.LogInformation("Adding {Count} new recipients for scheduled report {Id}", dto.Recipients.Count, id);
                foreach (var recipientDto in dto.Recipients)
                {
                    if (string.IsNullOrWhiteSpace(recipientDto.EmailAddress) && string.IsNullOrWhiteSpace(recipientDto.UserId))
                    {
                        continue;
                    }

                    var recipient = new ScheduledReportRecipient
                    {
                        Id = Guid.NewGuid(),
                        ScheduledReportId = scheduledReport.Id,
                        UserId = recipientDto.UserId,
                        RecipientType = recipientDto.RecipientType,
                        CreationDate = _dateTimeProvider.Now,
                        CreatedBy = _currentUserService.UserId ?? string.Empty
                    };

                    // If UserId is provided, don't store EmailAddress (we'll fetch it dynamically)
                    // If UserId is null, store EmailAddress for manual email entries
                    if (string.IsNullOrWhiteSpace(recipientDto.UserId))
                    {
                        recipient.EmailAddress = recipientDto.EmailAddress;
                    }
                    // EmailAddress is not stored when UserId is present - it will be fetched from user record

                    // Add to the collection for navigation property
                    scheduledReport.Recipients.Add(recipient);
                    // Explicitly add to DbContext to ensure Entity Framework tracks it as a new entity
                    _context.Set<ScheduledReportRecipient>().Add(recipient);
                }

                // Mark the parent entity as modified (it's already tracked from FindOneAsync)
                // This ensures all changes (including deleted recipients and new recipients) are saved
                _context.Entry(scheduledReport).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Updated scheduled report {Id}. User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating scheduled report {Id}. User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred: {ex.Message}"
                );
            }
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting scheduled report {Id}. User: {UserId}", id, _currentUserService.UserId);

            try
            {
                var scheduledReport = await _scheduledReportRepository.FindOneAsync(
                    sr => sr.Id == id && !sr.IsDeleted,
                    false
                );
                if (scheduledReport == null)
                {
                    return APIOperationResponse<bool>.Fail(
                        ResponseType.NotFound,
                        "Scheduled report not found"
                    );
                }

                await _scheduledReportRepository.DeleteAsync(scheduledReport);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Deleted scheduled report {Id}. User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting scheduled report {Id}. User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred: {ex.Message}"
                );
            }
        }

        public async Task<APIOperationResponse<bool>> ToggleActiveAsync(Guid id, bool isActive)
        {
            _logger.LogInformation("Toggling scheduled report {Id} active status to {IsActive}. User: {UserId}", id, isActive, _currentUserService.UserId);

            try
            {
                var scheduledReport = await _scheduledReportRepository.FindOneAsync(
                    sr => sr.Id == id && !sr.IsDeleted,
                    false
                );
                if (scheduledReport == null)
                {
                    return APIOperationResponse<bool>.Fail(
                        ResponseType.NotFound,
                        "Scheduled report not found"
                    );
                }

                scheduledReport.IsActive = isActive;
                scheduledReport.ModificationDate = _dateTimeProvider.Now;
                scheduledReport.ModifiedBy = _currentUserService.UserId;

                await _scheduledReportRepository.UpdateAsync(scheduledReport);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Toggled scheduled report {Id} active status to {IsActive}. User: {UserId}", id, isActive, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling scheduled report {Id} active status. User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred: {ex.Message}"
                );
            }
        }

        public async Task<APIOperationResponse<List<ScheduledReportExecutionDto>>> GetExecutionHistoryAsync(Guid scheduledReportId)
        {
            _logger.LogInformation("Getting execution history for scheduled report {Id}. User: {UserId}", scheduledReportId, _currentUserService.UserId);

            try
            {
                var executions = await _context.Set<ScheduledReportExecution>()
                    .Where(e => e.ScheduledReportId == scheduledReportId && !e.IsDeleted)
                    .OrderByDescending(e => e.ExecutionDate)
                    .Take(50) // Limit to last 50 executions
                    .ToListAsync();

                var dtos = executions.Select(e => new ScheduledReportExecutionDto
                {
                    Id = e.Id,
                    ScheduledReportId = e.ScheduledReportId,
                    ExecutionDate = e.ExecutionDate,
                    Status = e.Status,
                    ErrorMessage = e.ErrorMessage,
                    RecipientCount = e.RecipientCount,
                    FileSizeBytes = e.FileSizeBytes
                }).ToList();

                return APIOperationResponse<List<ScheduledReportExecutionDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting execution history for scheduled report {Id}. User: {UserId}", scheduledReportId, _currentUserService.UserId);
                return APIOperationResponse<List<ScheduledReportExecutionDto>>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred: {ex.Message}"
                );
            }
        }

        public async Task<APIOperationResponse<bool>> ExecuteNowAsync(Guid id)
        {
            _logger.LogInformation("Execute now requested for scheduled report {Id}. User: {UserId}", id, _currentUserService.UserId);

            try
            {
                // Verify the scheduled report exists and is not deleted
                var scheduledReport = await _scheduledReportRepository.FindOneAsync(
                    sr => sr.Id == id && !sr.IsDeleted,
                    false
                );

                if (scheduledReport == null)
                {
                    _logger.LogWarning("Scheduled report {Id} not found or deleted. User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(
                        ResponseType.NotFound,
                        "Scheduled report not found"
                    );
                }

                // Check if the scheduled report is active
                if (!scheduledReport.IsActive)
                {
                    _logger.LogWarning("Cannot execute scheduled report {Id} because it is disabled. User: {UserId}", id, _currentUserService.UserId);
                    return APIOperationResponse<bool>.Fail(
                        ResponseType.BadRequest,
                        "Cannot execute a disabled scheduled report. Please enable it first."
                    );
                }

                // Execute the report immediately using the execution service
                await _executionService.ExecuteReportAsync(id);

                _logger.LogInformation("Scheduled report {Id} executed successfully. User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing scheduled report {Id}. User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Fail(
                    ResponseType.InternalServerError,
                    $"An error occurred while executing the report: {ex.Message}"
                );
            }
        }

        private async Task<ScheduledReportDto> MapToDtoAsync(ScheduledReport sr)
        {
            var report = sr.Report ?? await _reportRepository.FindOneAsync(
                r => r.Id == sr.ReportId && !r.IsDeleted,
                false
            );

            var recipientDtos = new List<ScheduledReportRecipientDto>();
            foreach (var recipient in sr.Recipients.Where(r => !r.IsDeleted))
            {
                string? userName = null;
                string? emailAddress = null;

                // If UserId is present, get the current email and username from the user
                if (!string.IsNullOrWhiteSpace(recipient.UserId))
                {
                    try
                    {
                        var userResult = await _userService.GetByIdAsync(recipient.UserId);
                        if (userResult.Succeeded && userResult.Data != null)
                        {
                            userName = userResult.Data.UserName;
                            emailAddress = userResult.Data.Email; // Get current email from user record
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error retrieving user {UserId} for scheduled report recipient", recipient.UserId);
                    }
                }
                else
                {
                    // For manual email entries (no UserId), use stored EmailAddress
                    emailAddress = recipient.EmailAddress;
                }

                recipientDtos.Add(new ScheduledReportRecipientDto
                {
                    Id = recipient.Id,
                    ScheduledReportId = recipient.ScheduledReportId,
                    UserId = recipient.UserId,
                    UserName = userName,
                    EmailAddress = emailAddress,
                    RecipientType = recipient.RecipientType
                });
            }

            return new ScheduledReportDto
            {
                Id = sr.Id,
                ScheduleName = sr.ScheduleName,
                ReportId = sr.ReportId,
                ReportName = report?.ReportName ?? string.Empty,
                ReportUrl = report?.Url ?? string.Empty,
                OutputFormat = sr.OutputFormat,
                Frequency = sr.Frequency,
                TimeOfDay = sr.TimeOfDay,
                DayOfWeek = sr.DayOfWeek,
                DayOfMonth = sr.DayOfMonth,
                NextRunDate = sr.NextRunDate,
                LastRunDate = sr.LastRunDate,
                IsActive = sr.IsActive,
                EmailSubject = sr.EmailSubject,
                EmailBody = sr.EmailBody,
                Recipients = recipientDtos,
                CreationDate = sr.CreationDate,
                CreatedBy = sr.CreatedBy
            };
        }

        private DateTime? CalculateNextRunDate(string frequency, string timeOfDay, int? dayOfWeek, int? dayOfMonth)
        {
            if (!TimeSpan.TryParse(timeOfDay, out var time))
            {
                time = new TimeSpan(8, 0, 0); // Default to 8:00 AM
            }

            var now = _dateTimeProvider.Now;
            var today = now.Date;
            var targetTime = today.Add(time);

            // If time has passed today, move to next day
            if (targetTime <= now)
            {
                targetTime = targetTime.AddDays(1);
            }

            switch (frequency.ToLower())
            {
                case "daily":
                    return targetTime;

                case "weekly":
                    if (dayOfWeek.HasValue)
                    {
                        var currentDayOfWeek = (int)targetTime.DayOfWeek;
                        var daysUntilTarget = (dayOfWeek.Value - currentDayOfWeek + 7) % 7;
                        if (daysUntilTarget == 0 && targetTime <= now)
                        {
                            daysUntilTarget = 7; // Move to next week
                        }
                        return targetTime.AddDays(daysUntilTarget);
                    }
                    return targetTime;

                case "monthly":
                    if (dayOfMonth.HasValue)
                    {
                        var day = Math.Min(dayOfMonth.Value, DateTime.DaysInMonth(targetTime.Year, targetTime.Month));
                        var targetDate = new DateTime(targetTime.Year, targetTime.Month, day);
                        if (targetDate < now.Date)
                        {
                            // Move to next month
                            targetDate = targetDate.AddMonths(1);
                            day = Math.Min(dayOfMonth.Value, DateTime.DaysInMonth(targetDate.Year, targetDate.Month));
                            targetDate = new DateTime(targetDate.Year, targetDate.Month, day);
                        }
                        return targetDate.Add(time);
                    }
                    return targetTime;

                default:
                    return targetTime;
            }
        }
    }
}
