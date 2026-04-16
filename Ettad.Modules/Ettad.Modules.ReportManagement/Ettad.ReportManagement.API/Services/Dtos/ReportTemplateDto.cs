namespace Ettad.Modules.ReportManagement.API.Services.Dtos
{
    public class ReportTemplateDto
    {
        public string Url { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
