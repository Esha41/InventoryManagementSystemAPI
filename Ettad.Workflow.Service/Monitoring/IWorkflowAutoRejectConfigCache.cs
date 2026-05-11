using Ettad.Workflows.Service.Dtos;

namespace Ettad.Workflows.Service.Monitoring;

public interface IWorkflowAutoRejectConfigCache
{
    Task<WorkflowAutoRejectTriggerConfig> GetConfigAsync(long workflowId, CancellationToken cancellationToken = default);

    void InvalidateConfig(long workflowId);
}
