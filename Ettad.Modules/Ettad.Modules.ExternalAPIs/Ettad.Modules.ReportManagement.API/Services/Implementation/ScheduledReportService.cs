using AutoMapper;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Data.Repository;
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
        private readonly ICrossCuttingRepository<ScheduledReport> _scheduledReportRepository;
        private readonly ICrossCuttingRepository<ReportEntity> _reportRepository;
        private readonly ICrossCuttingRepository<ScheduledReportRecipient> _recipientRepository;
        private readonly ICrossCuttingRepository<ScheduledReportExecution> _executionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ScheduledReportService> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IScheduledReportExecutionService _executionService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ScheduledReportService(
            ICrossCuttingRepository<ScheduledReport> scheduledReportRepository,
            ICrossCuttingRepository<ReportEntity> reportRepository,
            ICrossCuttingRepository<ScheduledReportRecipient> recipientRepository,
            ICrossCuttingRepository<ScheduledReportExecution> executionRepository,
            ICurrentUserService currentUserService,
            ILogger<ScheduledReportService> logger,
            IDateTimeProvider dateTimeProvider,
            IScheduledReportExecutionService executionService,
            IUserService userService,
            IMapper mapper)
        {
            _scheduledReportRepository = scheduledReportRepository;
            _reportRepository = reportRepository;
            _recipientRepository = recipientRepository;
            _executionRepository = executionRepository;
            _currentUserService = currentUserService;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _executionService = executionService;
            _userService = userService;
            _mapper = mapper;
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

                var scheduledReport = _mapper.Map<ScheduledReport>(dto);
                scheduledReport.Id = Guid.NewGuid();
                scheduledReport.IsActive = true;
                scheduledReport.CreationDate = _dateTimeProvider.Now;
                scheduledReport.CreatedBy = _currentUserService.UserId ?? string.Empty;

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

                    var recipient = _mapper.Map<ScheduledReportRecipient>(recipientDto);
                    recipient.Id = Guid.NewGuid();
                    recipient.ScheduledReportId = scheduledReport.Id;
                    recipient.CreationDate = _dateTimeProvider.Now;
                    recipient.CreatedBy = _currentUserService.UserId ?? string.Empty;

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

                _mapper.Map(dto, scheduledReport);
                scheduledReport.ModificationDate = _dateTimeProvider.Now;
                scheduledReport.ModifiedBy = _currentUserService.UserId;

                // Recalculate next run date if schedule changed
                scheduledReport.NextRunDate = CalculateNextRunDate(
                    dto.Frequency,
                    dto.TimeOfDay,
                    dto.DayOfWeek,
                    dto.DayOfMonth
                );

                // Update recipients - hard delete existing and add new ones
                var existingRecipientIds = scheduledReport.Recipients.Where(r => !r.IsDeleted).Select(r => r.Id).ToList();
                if (existingRecipientIds.Any())
                {
                    _logger.LogInformation("Hard deleting {Count} existing recipients for scheduled report {Id}", existingRecipientIds.Count, id);
                    await _recipientRepository
                        .Find(r => existingRecipientIds.Contains(r.Id), includeSoftDeleted: true)
                        .ExecuteDeleteAsync();
                }

                // Add new recipients
                _logger.LogInformation("Adding {Count} new recipients for scheduled report {Id}", dto.Recipients.Count, id);
                foreach (var recipientDto in dto.Recipients)
                {
                    if (string.IsNullOrWhiteSpace(recipientDto.EmailAddress) && string.IsNullOrWhiteSpace(recipientDto.UserId))
                    {
                        continue;
                    }

                    var recipient = _mapper.Map<ScheduledReportRecipient>(recipientDto);
                    recipient.Id = Guid.NewGuid();
                    recipient.ScheduledReportId = scheduledReport.Id;
                    recipient.CreationDate = _dateTimeProvider.Now;
                    recipient.CreatedBy = _currentUserService.UserId ?? string.Empty;

                    // If UserId is provided, don't store EmailAddress (we'll fetch it dynamically)
                    if (string.IsNullOrWhiteSpace(recipientDto.UserId))
                    {
                        recipient.EmailAddress = recipientDto.EmailAddress;
                    }

                    await _recipientRepository.AddAsync(recipient);
                }

                await _scheduledReportRepository.UpdateAsync(scheduledReport);

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
            _logger.LogInformation("Permanently deleting scheduled report {Id}. User: {UserId}", id, _currentUserService.UserId);

            try
            {
                var scheduledReport = await _scheduledReportRepository.FindOneAsync(
                    sr => sr.Id == id, includeSoftDeleted: true);

                if (scheduledReport == null)
                {
                    return APIOperationResponse<bool>.Fail(
                        ResponseType.NotFound,
                        "Scheduled report not found"
                    );
                }

                // Permanently delete execution history
                var deletedExecutions = await _executionRepository
                    .Find(e => e.ScheduledReportId == id, includeSoftDeleted: true)
                    .ExecuteDeleteAsync();
                _logger.LogInformation("Permanently deleted {Count} executions for scheduled report {Id}", deletedExecutions, id);

                // Permanently delete recipients
                var deletedRecipients = await _recipientRepository
                    .Find(r => r.ScheduledReportId == id, includeSoftDeleted: true)
                    .ExecuteDeleteAsync();
                _logger.LogInformation("Permanently deleted {Count} recipients for scheduled report {Id}", deletedRecipients, id);

                // Permanently delete the scheduled report itself
                await _scheduledReportRepository
                    .Find(sr => sr.Id == id, includeSoftDeleted: true)
                    .ExecuteDeleteAsync();

                _logger.LogInformation("Permanently deleted scheduled report {Id}. User: {UserId}", id, _currentUserService.UserId);
                return APIOperationResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error permanently deleting scheduled report {Id}. User: {UserId}", id, _currentUserService.UserId);
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
                var executions = await _executionRepository
                    .Find(e => e.ScheduledReportId == scheduledReportId && !e.IsDeleted)
                    .OrderByDescending(e => e.ExecutionDate)
                    .Take(50) // Limit to last 50 executions
                    .ToListAsync();

                var dtos = _mapper.Map<List<ScheduledReportExecutionDto>>(executions);

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

                var recipientDto = _mapper.Map<ScheduledReportRecipientDto>(recipient);
                recipientDto.UserName = userName;
                recipientDto.EmailAddress = emailAddress; // Override with fetched email if UserId is present
                recipientDtos.Add(recipientDto);
            }

            var dto = _mapper.Map<ScheduledReportDto>(sr);
            dto.Recipients = recipientDtos;
            return dto;
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
