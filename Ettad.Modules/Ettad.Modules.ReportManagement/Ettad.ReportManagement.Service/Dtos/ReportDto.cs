using Ettad.Data.Enums;

namespace Ettad.ReportManagement.Service.Dtos
{
    public class ReportDto
    {
        public Guid Id { get; set; }
        public string ReportName { get; set; } = string.Empty;
        public ReportStatuses ReportStatusId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? Description { get; set; }
        public byte[]? LayoutData { get; set; }
        public string? ReportParameters { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? ModificationDate { get; set; }
        public string? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public string? DeletedBy { get; set; }
        public List<ReportRoleDto>? Roles { get; set; }
    }
}

