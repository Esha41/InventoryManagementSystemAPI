
namespace Ettad.ReportManagement.Service.Dtos
{
    /// <summary>
    /// DTO for setting a report as public with role restrictions
    /// </summary>
    public class SetReportPublicDto
    {
        /// <summary>
        /// Whether the report should be public (Published) or private (Draft)
        /// </summary>
        public bool IsPublic { get; set; }

        /// <summary>
        /// List of role IDs that can access this report when it's public.
        /// If empty and IsPublic is true, the report will be accessible to all authenticated users.
        /// </summary>
        public List<string> RoleIds { get; set; } = new List<string>();
    }
}

