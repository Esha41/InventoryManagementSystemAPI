using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Queries.WorkflowApproval.GetWorkflowApprovalSteps
{
    public class GetWorkflowApprovalStepsQuery : IRequest<APIOperationResponse<List<WorkflowApprovalStepDto>>>
    {
    }

    public class GetWorkflowApprovalStepsQueryHandler : IRequestHandler<GetWorkflowApprovalStepsQuery, APIOperationResponse<List<WorkflowApprovalStepDto>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GetWorkflowApprovalStepsQueryHandler> _logger;

        public GetWorkflowApprovalStepsQueryHandler(
            ApplicationDbContext context,
            ILogger<GetWorkflowApprovalStepsQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<APIOperationResponse<List<WorkflowApprovalStepDto>>> Handle(GetWorkflowApprovalStepsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.WorkflowApprovalSteps
                    .Select(x => new WorkflowApprovalStepDto
                    {
                        Id = x.Id,
                        WorkflowStepId = x.WorkflowStepId,
                        TargetRequestId = x.TargetRequestId,
                        RequestType = x.RequestType,
                        ApproverUserId = x.ApproverUserId,
                        ApproverRoleId = x.ApproverRoleId,
                        IsDelegation = x.IsDelegation,
                        ApprovedDate = x.ApprovedDate,
                        Status = x.Status,
                        Comments = x.Comments,
                        IsCurrent = x.IsCurrent,
                        ReturnToStepId = x.ReturnToStepId
                    }).ToListAsync(cancellationToken);

                return APIOperationResponse<List<WorkflowApprovalStepDto>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all workflow approval steps");
                return APIOperationResponse<List<WorkflowApprovalStepDto>>.ServerError($"Processing failed: {ex.Message}");
            }
        }
    }
}
