using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.Interface
{
    public interface IWorkflowApprovalService
    {
        Task<IEnumerable<WorkflowApprovalStepDto>> GetAllAsync();
        Task<WorkflowApprovalStepDto> GetByIdAsync(int id);
        Task<WorkflowApprovalStepDto> CreateAsync(CreateWorkflowApprovalStepDto dto);
        Task<WorkflowApprovalStepDto> UpdateAsync(int id, UpdateWorkflowApprovalStepDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<WorkflowApprovalWithOrderDto>> GetOrdersWithApprovalStepsAsync();
        Task<WorkflowApprovalStepDto> ApproveAsync(ApproveRejectWorkflowApprovalDto dto);
        Task<WorkflowApprovalStepDto> RejectAsync(ApproveRejectWorkflowApprovalDto dto);
        Task<WorkflowApprovalStepDto> ApproveOrReject(ApproveRejectWorkflowApprovalDto dto);
        Task<WorkflowApprovalStep> GetCurrentApprovalStepByRequestIdAsync(int requestId);
        Task<APIOperationResponse<bool>> ProcessActionAsync(ApproveRejectWorkflowApprovalDto model);
        Task<bool> StartWorkflowAsync(long orderId, WorkflowType workflowType);

    }
}
