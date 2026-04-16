namespace Ettad.ReportManagement.Service.Dtos
{
    /// <summary>
    /// DTO for report role information
    /// </summary>
    public class ReportRoleDto
    {
        public string RoleId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string? RoleNameEn { get; set; }
        public string? RoleNameAr { get; set; }
    }
}

