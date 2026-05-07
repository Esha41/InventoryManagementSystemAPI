using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Base;
using Ettad.CrossCutting.Comman.Idenitity;

namespace Ettad.Data.Entities.Workflows
{
    /// <summary>
    /// Represents a user or role that should be notified when an action is taken on a workflow step
    /// </summary>
    public class WorkflowStepNotifier : AuditEntity<long>
    {
        public long WorkflowStepId { get; set; }
        public string? UserId { get; set; }
        public string? RoleId { get; set; }

        #region Navigation Properties
        public virtual WorkflowStep WorkflowStep { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
        #endregion
    }
}
