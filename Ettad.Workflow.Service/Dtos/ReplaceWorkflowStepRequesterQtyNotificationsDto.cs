using System.Collections.Generic;

namespace Ettad.Workflows.Service.Dtos
{
    public class ReplaceWorkflowStepRequesterQtyNotificationsDto
    {
        /// <summary>Workflow whose notification configuration is replaced (existing rows for this workflow are deleted first).</summary>
        public long WorkflowId { get; set; }

        /// <summary>Workflow step IDs (must belong to <see cref="WorkflowId"/>) that trigger requester notification when that step is approved.</summary>
        public List<long> WorkflowStepIds { get; set; } = new();
    }
}
