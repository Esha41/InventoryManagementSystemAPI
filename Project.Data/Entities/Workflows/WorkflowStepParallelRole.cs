using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Base;
using Ettad.CrossCutting.Comman.Idenitity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ettad.Data.Entities.Workflows
{
    /// <summary>
    /// Additional roles that may approve the same workflow step in parallel (any-of) with <see cref="WorkflowStep.ApplicationRoleId"/>.
    /// </summary>
    public class WorkflowStepParallelRole : AuditEntity<long>
    {
        public long WorkflowStepId { get; set; }
        public string RoleId { get; set; } = null!;

        public virtual WorkflowStep WorkflowStep { get; set; } = null!;
        public virtual ApplicationRole Role { get; set; } = null!;
    }
}
