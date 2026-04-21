using System.ComponentModel.DataAnnotations;
using Ettad.Data.Enums;

namespace Ettad.ReportManagement.Service.Dtos
{
    public class UpdateReportDto
    {
        [Required]
        [MaxLength(200)]
        public string ReportName { get; set; } = string.Empty;

        [Required]
        public ReportStatuses ReportStatusId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Url { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public byte[]? LayoutData { get; set; }

        public string? ReportParameters { get; set; }
    }
}

