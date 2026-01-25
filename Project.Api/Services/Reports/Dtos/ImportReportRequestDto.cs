namespace Project.Api.Services.Reports.Dtos
{
    public class ImportReportRequestDto
    {
        public IFormFile File { get; set; } = default!;
        public string? ReportName { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
    }
}
