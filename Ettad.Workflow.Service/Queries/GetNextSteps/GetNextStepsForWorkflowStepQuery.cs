using MediatR;
using Microsoft.EntityFrameworkCore;
using Ettad.Workflows.Service.DTO;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
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
                    .Include(ws => ws.HigherApprovalRole)
                    .Include(ws => ws.Transitions)
                        .ThenInclude(t => t.TargetWorkflowStep)
                            .ThenInclude(target => target.ApplicationRole)
                    .Include(ws => ws.Transitions)
                        .ThenInclude(t => t.TargetWorkflowStep)
                            .ThenInclude(target => target.HigherApprovalRole)
                    .FirstOrDefaultAsync(ws => ws.Id == request.WorkflowStepId, cancellationToken);

                if (currentStep == null)
                {
                    return APIOperationResponse<List<WorkflowStepDto>>.NotFound($"Workflow step with ID {request.WorkflowStepId} not found.");
                }

                // Get ALL workflow steps that come after the current step (StepOrder > currentStep.StepOrder)
                var allNextStepsEntities = await _context.WorkflowSteps
                    .Include(ws => ws.ApplicationRole)
                    .Include(ws => ws.HigherApprovalRole)
                    .Include(ws => ws.Transitions)
                        .ThenInclude(t => t.TargetWorkflowStep)
                            .ThenInclude(target => target.ApplicationRole)
                    .Include(ws => ws.Transitions)
                        .ThenInclude(t => t.TargetWorkflowStep)
                            .ThenInclude(target => target.HigherApprovalRole)
                    .Where(ws => ws.WorkflowId == currentStep.WorkflowId && ws.StepOrder > currentStep.StepOrder)
                    .OrderBy(ws => ws.StepOrder)
                    .ToListAsync(cancellationToken);

                var allNextSteps = allNextStepsEntities.Select(ws => MapToDto(ws)).ToList();

                return APIOperationResponse<List<WorkflowStepDto>>.Success(allNextSteps);
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
                CanReturn = ws.CanReturn,
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
                        CanSkip = t.TargetWorkflowStep.CanSkip,
                        CanReturn = t.TargetWorkflowStep.CanReturn
                    } : null
                }).ToList() ?? new List<WorkflowStepTransitionDto>()
            };
        }
    }
}

