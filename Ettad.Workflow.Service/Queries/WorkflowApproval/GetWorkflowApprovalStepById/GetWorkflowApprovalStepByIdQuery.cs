using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Queries.WorkflowApproval.GetWorkflowApprovalStepById
{
    public class GetWorkflowApprovalStepByIdQuery : IRequest<APIOperationResponse<WorkflowApprovalStepDto>>
    {
        public long Id { get; }

        public GetWorkflowApprovalStepByIdQuery(long id)
        {
            Id = id;
        }
    }

    public class GetWorkflowApprovalStepByIdQueryHandler : IRequestHandler<GetWorkflowApprovalStepByIdQuery, APIOperationResponse<WorkflowApprovalStepDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GetWorkflowApprovalStepByIdQueryHandler> _logger;

        public GetWorkflowApprovalStepByIdQueryHandler(
            ApplicationDbContext context,
            ILogger<GetWorkflowApprovalStepByIdQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<APIOperationResponse<WorkflowApprovalStepDto>> Handle(GetWorkflowApprovalStepByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.WorkflowApprovalSteps.FindAsync(new object[] { request.Id }, cancellationToken);
                if (entity == null)
                {
                    return APIOperationResponse<WorkflowApprovalStepDto>.NotFound($"Workflow approval step with ID {request.Id} not found.");
                }

                var result = new WorkflowApprovalStepDto
                {
                    Id = entity.Id,
                    WorkflowStepId = entity.WorkflowStepId,
                    TargetRequestId = entity.TargetRequestId,
                    RequestType = entity.RequestType,
                    ApproverUserId = entity.ApproverUserId,
                    ApproverRoleId = entity.ApproverRoleId,
                    IsDelegation = entity.IsDelegation,
                    ApprovedDate = entity.ApprovedDate,
                    Status = entity.Status,
                    Comments = entity.Comments,
                    IsCurrent = entity.IsCurrent,
                    ReturnToStepId = entity.ReturnToStepId
                };

                return APIOperationResponse<WorkflowApprovalStepDto>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching workflow approval step with ID {Id}", request.Id);
                return APIOperationResponse<WorkflowApprovalStepDto>.ServerError($"Processing failed: {ex.Message}");
            }
        }
    }
}
