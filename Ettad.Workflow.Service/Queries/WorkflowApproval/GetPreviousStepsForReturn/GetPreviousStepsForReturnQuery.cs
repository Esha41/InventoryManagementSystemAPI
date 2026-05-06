using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.Workflows.Service.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Queries.WorkflowApproval.GetPreviousStepsForReturn
{
    public class GetPreviousStepsForReturnQuery : IRequest<APIOperationResponse<List<WorkflowStepDto>>>
    {
        public long RequestId { get; }

        public GetPreviousStepsForReturnQuery(long requestId)
        {
            RequestId = requestId;
        }
    }

    public class GetPreviousStepsForReturnQueryHandler : IRequestHandler<GetPreviousStepsForReturnQuery, APIOperationResponse<List<WorkflowStepDto>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GetPreviousStepsForReturnQueryHandler> _logger;

        public GetPreviousStepsForReturnQueryHandler(
            ApplicationDbContext context,
            ILogger<GetPreviousStepsForReturnQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<APIOperationResponse<List<WorkflowStepDto>>> Handle(GetPreviousStepsForReturnQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get the current approval step for this request
                var currentApprovalStep = await _context.WorkflowApprovalSteps
                    .Include(x => x.WorkflowStep)
                    .FirstOrDefaultAsync(x => x.TargetRequestId == request.RequestId && x.IsCurrent, cancellationToken);

                if (currentApprovalStep == null)
                {
                    return APIOperationResponse<List<WorkflowStepDto>>.Success(new List<WorkflowStepDto>());
                }

                // Get all workflow steps in the same workflow
                var allWorkflowSteps = await _context.WorkflowSteps
                    .Include(ws => ws.ApplicationRole)
                    .Include(ws => ws.HigherApprovalRole)
                    .Include(ws => ws.Transitions)
                        .ThenInclude(t => t.TargetWorkflowStep)
                            .ThenInclude(target => target.ApplicationRole)
                    .Include(ws => ws.Transitions)
                        .ThenInclude(t => t.TargetWorkflowStep)
                            .ThenInclude(target => target.HigherApprovalRole)
                    .Where(ws => ws.WorkflowId == currentApprovalStep.WorkflowStep.WorkflowId)
                    .OrderBy(ws => ws.StepOrder)
                    .ToListAsync(cancellationToken);

                // Get only the previous steps (steps with lower StepOrder than current)
                var previousSteps = allWorkflowSteps
                    .Where(ws => ws.StepOrder < currentApprovalStep.WorkflowStep.StepOrder)
                    .Select(ws => new WorkflowStepDto
                    {
                        Id = ws.Id,
                        WorkflowId = ws.WorkflowId,
                        StepOrder = ws.StepOrder,
                        ApplicationRoleId = ws.ApplicationRoleId,
                        ApplicationRoleName = ws.ApplicationRole?.Name,
                        ApplicationRoleNameAr = ws.ApplicationRole?.NameAr,
                        ApplicationEntityId = ws.ApplicationEntityId,
                        MustApprove = ws.MustApprove,
                        RequireHigherApproval = ws.RequireHigherApproval,
                        HigherApprovalRoleId = ws.HigherApprovalRoleId,
                        HigherApplicationEntityId = ws.HigherApplicationEntityId,
                        ReserveQty = ws.ReserveQty,
                        CanSkip = ws.CanSkip,
                        Transitions = ws.Transitions?.Select(t => new WorkflowStepTransitionDto
                        {
                            Id = t.Id,
                            SourceWorkflowStepId = t.SourceWorkflowStepId,
                            TargetWorkflowStepId = t.TargetWorkflowStepId,
                            TargetStep = t.TargetWorkflowStep != null ? new TargetStepDetailsDto
                            {
                                Id = t.TargetWorkflowStep.Id,
                                WorkflowId = t.TargetWorkflowStep.WorkflowId,
                                StepOrder = t.TargetWorkflowStep.StepOrder,
                                ApplicationRole = t.TargetWorkflowStep.ApplicationRole != null ? new RoleDto
                                {
                                    Id = t.TargetWorkflowStep.ApplicationRole.Id,
                                    Name = t.TargetWorkflowStep.ApplicationRole.Name,
                                    NameAr = t.TargetWorkflowStep.ApplicationRole.NameAr,
                                    IsDefaultRole = t.TargetWorkflowStep.ApplicationRole.IsDefaultRole ?? false,
                                    IsSuperAdmin = t.TargetWorkflowStep.ApplicationRole.IsSuperAdmin,
                                    ApplicationEntityIds = new List<long>()
                                } : null,
                                ApplicationEntityId = t.TargetWorkflowStep.ApplicationEntityId,
                                RequireHigherApproval = t.TargetWorkflowStep.RequireHigherApproval,
                                HigherApprovalRole = t.TargetWorkflowStep.HigherApprovalRole != null ? new RoleDto
                                {
                                    Id = t.TargetWorkflowStep.HigherApprovalRole.Id,
                                    Name = t.TargetWorkflowStep.HigherApprovalRole.Name,
                                    NameAr = t.TargetWorkflowStep.HigherApprovalRole.NameAr,
                                    IsDefaultRole = t.TargetWorkflowStep.HigherApprovalRole.IsDefaultRole ?? false,
                                    IsSuperAdmin = t.TargetWorkflowStep.HigherApprovalRole.IsSuperAdmin,
                                    ApplicationEntityIds = new List<long>()
                                } : null,
                                HigherApplicationEntityId = t.TargetWorkflowStep.HigherApplicationEntityId,
                                MustApprove = t.TargetWorkflowStep.MustApprove,
                                ReserveQty = t.TargetWorkflowStep.ReserveQty,
                                CanSkip = t.TargetWorkflowStep.CanSkip
                            } : null
                        }).ToList() ?? new List<WorkflowStepTransitionDto>()
                    })
                    .ToList();

                return APIOperationResponse<List<WorkflowStepDto>>.Success(previousSteps);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching previous workflow steps for return. RequestId: {RequestId}", request.RequestId);
                return APIOperationResponse<List<WorkflowStepDto>>.ServerError($"Processing failed: {ex.Message}");
            }
        }
    }
}
