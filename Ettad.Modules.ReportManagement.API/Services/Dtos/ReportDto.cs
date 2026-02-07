namespace Ettad.Modules.ReportManagement.API.Services.Dtos
{
    public class ReportDto
    {
        public Guid Id { get; set; }
        public string ReportName { get; set; } = string.Empty;
        public long ReportStatusId { get; set; }
        public string ReportStatusNameEn { get; set; } = string.Empty;
        public string ReportStatusNameAr { get; set; } = string.Empty;
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
    }
}
