using MediatR;
using Microsoft.EntityFrameworkCore;
using Ettad.Workflows.Service.DTO;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.Queries.GetNextSteps
{
    public class GetNextStepsForWorkflowStepQuery : IRequest<APIOperationResponse<List<WorkflowStepDto>>>
    {
        public int WorkflowStepId { get; }

        public GetNextStepsForWorkflowStepQuery(int workflowStepId)
        {
            WorkflowStepId = workflowStepId;
        }
    }

    public class GetNextStepsForWorkflowStepQueryHandler : IRequestHandler<GetNextStepsForWorkflowStepQuery, APIOperationResponse<List<WorkflowStepDto>>>
    {
        private readonly ApplicationDbContext _context;

        public GetNextStepsForWorkflowStepQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<APIOperationResponse<List<WorkflowStepDto>>> Handle(GetNextStepsForWorkflowStepQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get the current workflow step
                var currentStep = await _context.WorkflowSteps
                    .Include(ws => ws.ApplicationRole)
                    .Include(ws => ws.Transitions)
                    .FirstOrDefaultAsync(ws => ws.Id == request.WorkflowStepId, cancellationToken);

                if (currentStep == null)
                {
                    return APIOperationResponse<List<WorkflowStepDto>>.NotFound($"Workflow step with ID {request.WorkflowStepId} not found.");
                }

                // Get all workflow steps in the same workflow
                var allWorkflowSteps = await _context.WorkflowSteps
                    .Include(ws => ws.ApplicationRole)
                    .Include(ws => ws.Transitions)
                    .Where(ws => ws.WorkflowId == currentStep.WorkflowId)
                    .OrderBy(ws => ws.StepOrder)
                    .ToListAsync(cancellationToken);

                var allowedNextSteps = new List<WorkflowStepDto>();

                // 1. Sequential Next Step
                var nextSequentialStep = allWorkflowSteps
                    .FirstOrDefault(ws => ws.StepOrder > currentStep.StepOrder);

                if (nextSequentialStep != null)
                {
                    allowedNextSteps.Add(MapToDto(nextSequentialStep));
                }

                // 2. Allowed Skip Targets (if CanSkip is true)
                if (currentStep.CanSkip && currentStep.Transitions != null && currentStep.Transitions.Any())
                {
                    var skipTargetIds = currentStep.Transitions.Select(t => t.TargetWorkflowStepId).ToList();
                    var skipTargets = allWorkflowSteps
                        .Where(ws => skipTargetIds.Contains(ws.Id))
                        .Select(MapToDto)
                        .ToList();

                    // Add skip targets if not already in the list (avoid duplicates if skip target is same as sequential)
                    foreach (var target in skipTargets)
                    {
                        if (!allowedNextSteps.Any(s => s.Id == target.Id))
                        {
                            allowedNextSteps.Add(target);
                        }
                    }
                }

                return APIOperationResponse<List<WorkflowStepDto>>.Success(allowedNextSteps);
            }
            catch (Exception ex)
            {
                return APIOperationResponse<List<WorkflowStepDto>>.ServerError($"An error occurred while fetching next steps: {ex.Message}");
            }
        }

        private WorkflowStepDto MapToDto(Ettad.Data.Entities.Workflows.WorkflowStep ws)
        {
            return new WorkflowStepDto
            {
                Id = ws.Id,
                WorkflowId = ws.WorkflowId,
                StepOrder = ws.StepOrder,
                ApplicationRoleId = ws.ApplicationRoleId,
                ApplicationRoleName = ws.ApplicationRole?.Name,
                ApplicationEntityId = ws.ApplicationEntityId,
                MustApprove = ws.MustApprove,
                RequireHigherApproval = ws.RequireHigherApproval,
                HigherApprovalRoleId = ws.HigherApprovalRoleId,
                HigherApplicationEntityId = ws.HigherApplicationEntityId,
                ReserveQty = ws.ReserveQty,
                CanSkip = ws.CanSkip,
                AllowedSkipTargetIds = ws.Transitions?.Select(t => t.TargetWorkflowStepId).ToList() ?? new List<int>()
            };
        }
    }
}

