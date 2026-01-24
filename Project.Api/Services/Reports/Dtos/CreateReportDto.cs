using System.ComponentModel.DataAnnotations;

namespace Ettad.Reporting.Services.Reports.Dtos
{
    public class CreateReportDto
    {
        [Required]
        [MaxLength(200)]
        public string ReportName { get; set; } = string.Empty;

        [Required]
        public long ReportStatusId { get; set; } = 1; // Default to Draft

        [Required]
        [MaxLength(500)]
        public string Url { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public byte[]? LayoutData { get; set; }

        [MaxLength(100)]
        public string? ReportType { get; set; }

        public string? ReportParameters { get; set; }

        public bool IsTemplate { get; set; } = false;

        public bool IsPublic { get; set; } = false;
    }
}
