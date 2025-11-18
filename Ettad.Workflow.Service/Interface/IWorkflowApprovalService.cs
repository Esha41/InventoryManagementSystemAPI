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
        Task<WorkflowApprovalStepDto> ApproveOrRejectAsync(ApproveRejectWorkflowApprovalDto dto);
    }
}
