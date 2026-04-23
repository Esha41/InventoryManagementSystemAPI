using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ettad.Workflow.Service.Interface
{
    /// <summary>
    /// Service interface for managing workflow step notifiers
    /// </summary>
    public interface IWorkflowStepNotifierService
    {
        /// <summary>
        /// Get all notifiers for a specific workflow step
        /// </summary>
        Task<APIOperationResponse<List<WorkflowStepNotifierDto>>> GetNotifiersByStepIdAsync(int workflowStepId);

        /// <summary>
        /// Update notifiers for a workflow step (replaces existing notifiers)
        /// </summary>
        Task<APIOperationResponse<bool>> UpdateStepNotifiersAsync(UpdateWorkflowStepNotifiersDto dto);

        /// <summary>
        /// Add notifiers to a workflow step (supports multiple users and/or roles)
        /// </summary>
        Task<bool> AddNotifierAsync(CreateWorkflowStepNotifierDto dto);

        /// <summary>
        /// Remove a notifier from a workflow step
        /// </summary>
        Task<APIOperationResponse<bool>> RemoveNotifierAsync(int notifierId);

        /// <summary>
        /// Get all user IDs and role IDs that should be notified for a workflow step
        /// </summary>
        Task<APIOperationResponse<(List<string> UserIds, List<string> RoleIds)>> GetNotifierIdsByStepIdAsync(int workflowStepId);
    }
}

