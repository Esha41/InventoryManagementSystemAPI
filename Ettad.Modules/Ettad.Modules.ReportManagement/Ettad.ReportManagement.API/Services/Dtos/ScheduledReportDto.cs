using System;
using System.Collections.Generic;

namespace Ettad.Modules.ReportManagement.API.Services.Dtos
{
    public class ScheduledReportDto
    {
        public Guid Id { get; set; }
        public string ScheduleName { get; set; } = string.Empty;
        public Guid ReportId { get; set; }
        public string ReportName { get; set; } = string.Empty;
        public string ReportUrl { get; set; } = string.Empty;
        public string OutputFormat { get; set; } = "PDF";
        public string Frequency { get; set; } = "Daily";
        public string TimeOfDay { get; set; } = "08:00";
        public int? DayOfWeek { get; set; }
        public int? DayOfMonth { get; set; }
        public DateTime? NextRunDate { get; set; }
        public DateTime? LastRunDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string? EmailSubject { get; set; }
        public string? EmailBody { get; set; }
        public List<ScheduledReportRecipientDto> Recipients { get; set; } = new List<ScheduledReportRecipientDto>();
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }

    public class ScheduledReportRecipientDto
    {
        public Guid Id { get; set; }
        public Guid ScheduledReportId { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? EmailAddress { get; set; } // Only populated for manual email entries (when UserId is null)
        public string RecipientType { get; set; } = "To"; // To, CC, BCC
    }

    public class CreateScheduledReportDto
    {
        public string ScheduleName { get; set; } = string.Empty;
        public Guid ReportId { get; set; }
        public string OutputFormat { get; set; } = "PDF";
        public string Frequency { get; set; } = "Daily";
        public string TimeOfDay { get; set; } = "08:00";
        public int? DayOfWeek { get; set; }
        public int? DayOfMonth { get; set; }
        public string? EmailSubject { get; set; }
        public string? EmailBody { get; set; }
        public List<CreateScheduledReportRecipientDto> Recipients { get; set; } = new List<CreateScheduledReportRecipientDto>();
    }

    public class CreateScheduledReportRecipientDto
    {
        public string? UserId { get; set; }
        public string? EmailAddress { get; set; }
        public string RecipientType { get; set; } = "To";
    }

    public class UpdateScheduledReportDto
    {
        public string ScheduleName { get; set; } = string.Empty;
        public Guid ReportId { get; set; }
        public string OutputFormat { get; set; } = "PDF";
        public string Frequency { get; set; } = "Daily";
        public string TimeOfDay { get; set; } = "08:00";
        public int? DayOfWeek { get; set; }
        public int? DayOfMonth { get; set; }
        public bool IsActive { get; set; } = true;
        public string? EmailSubject { get; set; }
        public string? EmailBody { get; set; }
        public List<CreateScheduledReportRecipientDto> Recipients { get; set; } = new List<CreateScheduledReportRecipientDto>();
    }

    public class ScheduledReportExecutionDto
    {
        public Guid Id { get; set; }
        public Guid ScheduledReportId { get; set; }
        public DateTime ExecutionDate { get; set; }
        public string Status { get; set; } = "Success";
        public string? ErrorMessage { get; set; }
        public int RecipientCount { get; set; }
        public long? FileSizeBytes { get; set; }
    }

    public class ToggleActiveDto
    {
        public bool IsActive { get; set; }
    }
}
