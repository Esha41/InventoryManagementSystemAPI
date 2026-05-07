
using System.Threading;
using System.Threading.Tasks;

namespace Ettad.Workflow.Service.Interface
{
    /// <summary>
    /// Sends notifications when a workflow is started (approvers and step notifiers).
    /// Call after the transaction that created the workflow approval step has committed.
    /// </summary>
    public interface IWorkflowStartNotificationService
    {
        /// <param name="requestId">Target request (e.g. order/discard) id.</param>
        /// <param name="workflowStepId">Workflow definition step id (not WorkflowApprovalStep id).</param>
        Task SendAsync(long requestId, long workflowStepId, CancellationToken cancellationToken = default);
    }
}
