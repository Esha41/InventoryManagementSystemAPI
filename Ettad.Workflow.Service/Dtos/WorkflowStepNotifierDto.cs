namespace Ettad.Workflows.Service.Dtos
{
    /// <summary>
    /// DTO for workflow step notifier
    /// </summary>
    public class WorkflowStepNotifierDto
    {
        public long Id { get; set; }
        public long WorkflowStepId { get; set; }
        public string? UserId { get; set; }
        public string? RoleId { get; set; }
        
        // Navigation properties for display
        public string? UserName { get; set; }
        public string? UserFullNameEn { get; set; }
        public string? UserFullNameAr { get; set; }
        public string? RoleName { get; set; }
        public string? RoleNameAr { get; set; }
    }

    /// <summary>
    /// DTO for updating workflow step notifiers
    /// </summary>
    public class UpdateWorkflowStepNotifiersDto
    {
        public long WorkflowStepId { get; set; }
        public List<string>? UserIds { get; set; }
        public List<string>? RoleIds { get; set; }
    }

    /// <summary>
    /// DTO for creating workflow step notifiers (supports multiple)
    /// </summary>
    public class CreateWorkflowStepNotifierDto
    {
        public long WorkflowStepId { get; set; }
        public List<string>? UserIds { get; set; }
        public List<string>? RoleIds { get; set; }
    }
}

