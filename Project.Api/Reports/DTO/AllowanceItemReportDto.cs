namespace Project.Api.Reports.DTO
{
    public class AllowanceItemReportDto
    {
        public string? ItemName { get; set; }
        public long DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int Year { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal ReservedQuantity { get; set; }
        public decimal UsedQuantity { get; set; }
        public decimal RemainingQuantity { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
