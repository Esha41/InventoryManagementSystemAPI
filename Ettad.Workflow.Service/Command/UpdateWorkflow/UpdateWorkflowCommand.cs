using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Workflows.Service.DTO;
using Ettad.Application.Common.Interfaces;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.Command.UpdateWorkflow
{
    public class UpdateWorkflowCommand : IRequest<APIOperationResponse<WorkflowDto>>
    {
        public int Id { get; set; }
        public string WorkflowName { get; set; }
        public WorkflowType WorkflowType { get; set; }
        public RequesterType RequesterType { get; set; }
        public int OrganizationId { get; set; }
        public int? CompanyId { get; set; }
        public int? DepartementId { get; set; }
        public bool IsActive { get; set; } = true;
        public List<WorkflowStepCreateDto> WorkflowSteps { get; set; } = new();
    }

    public class UpdateWorkflowCommandHandler : IRequestHandler<UpdateWorkflowCommand, APIOperationResponse<WorkflowDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UpdateWorkflowCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateWorkflowCommandHandler(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            ILogger<UpdateWorkflowCommandHandler> logger,
            IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<APIOperationResponse<WorkflowDto>> Handle(UpdateWorkflowCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting workflow update for ID {WorkflowId} at {Time}", request.Id, DateTime.UtcNow);

            try
            {
                int orgId = (int)(_currentUserService.IsSuperAdmin ? request.OrganizationId : _currentUserService.OrganizationId);

                // Find existing workflow
                var workflow = await _context.Workflows
                    .Include(w => w.WorkflowSteps)
                    .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

                if (workflow == null)
                {
                    return APIOperationResponse<WorkflowDto>.NotFound($"Workflow with ID {request.Id} not found");
                }

                // Validate step orders are unique
                if (request.WorkflowSteps?.Any() == true && HasDuplicateStepOrder(request.WorkflowSteps))
                {
                    return APIOperationResponse<WorkflowDto>.BadRequest("Duplicate StepOrder values are not allowed in workflow steps.");
                }

                // Check if any workflow step is referenced in approval history
                if (request.WorkflowSteps?.Any() == true)
                {
                    foreach (var step in request.WorkflowSteps)
                    {
                        if (step.Id > 0 && await IsWorkflowStepInApprovalHistory(step.Id, cancellationToken))
                        {
                            return APIOperationResponse<WorkflowDto>.BadRequest($"Workflow step with ID {step.Id} cannot be edited because it is referenced in approval history.");
                        }
                    }
                }

                // Deactivate duplicate active workflows if this one is active
                if (request.IsActive)
                {
                    await CheckAndDeactivateDuplicateWorkflows(request, orgId, workflow.Id, cancellationToken);
                }

                // Update workflow properties
                workflow.WorkflowName = request.WorkflowName;
                workflow.WorkflowType = request.WorkflowType;               
                workflow.IsActive = request.IsActive;
                workflow.ModifiedBy = _currentUserService.UserName;
                workflow.ModificationDate = DateTime.UtcNow;

                // Update workflow steps
                if (request.WorkflowSteps?.Any() == true)
                {
                    await UpdateWorkflowSteps(workflow, request.WorkflowSteps, cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);

                var workflowDto = _mapper.Map<WorkflowDto>(workflow);

                _logger.LogInformation("Workflow updated successfully with ID {WorkflowId} at {Time}", workflow.Id, DateTime.UtcNow);
                return APIOperationResponse<WorkflowDto>.Success(workflowDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating workflow with ID {WorkflowId} at {Time}", request.Id, DateTime.UtcNow);
                return APIOperationResponse<WorkflowDto>.ServerError($"Workflow update failed: {ex.Message}");
            }
        }

        // Check for duplicate active workflows and deactivate them if found
        private async Task CheckAndDeactivateDuplicateWorkflows(UpdateWorkflowCommand request, int organizationId, int currentWorkflowId, CancellationToken cancellationToken)
        {
            var existingActiveWorkflows = await _context.Workflows
                .Where(w => 
                            w.IsActive &&
                            !w.IsDeleted &&
                            w.Id != currentWorkflowId)
                .ToListAsync(cancellationToken);

            if (existingActiveWorkflows.Any())
            {
                _logger.LogInformation("Found {Count} active duplicate workflows for organization {OrganizationId} and department {DepartmentId}. Deactivating them.",
                    existingActiveWorkflows.Count, organizationId, request.DepartementId);

                foreach (var existingWorkflow in existingActiveWorkflows)
                {
                    existingWorkflow.IsActive = false;
                    existingWorkflow.ModificationDate = DateTime.UtcNow;
                    existingWorkflow.ModifiedBy = _currentUserService.UserName;
                }

                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Successfully deactivated {Count} duplicate workflows", existingActiveWorkflows.Count);
            }
        }

        // Update workflow steps
        private async Task UpdateWorkflowSteps(
     Ettad.Data.Entities.Workflows.Workflow workflow,
     List<WorkflowStepCreateDto> incomingSteps,
     CancellationToken cancellationToken)
        {
            var existingSteps = workflow.WorkflowSteps?.ToList() ?? new List<WorkflowStep>();

            // Remove deleted steps
            var stepsToRemove = existingSteps
                .Where(es => !incomingSteps.Any(s => s.Id == es.Id))
                .ToList();

            foreach (var stepToRemove in stepsToRemove)
            {
                if (await IsWorkflowStepInApprovalHistory(stepToRemove.Id, cancellationToken))
                {
                    _logger.LogWarning(
                        "Cannot remove workflow step {StepId} as it is referenced in approval history",
                        stepToRemove.Id);
                    continue;
                }
                _context.WorkflowSteps.Remove(stepToRemove);
            }

            // Update existing steps or add new ones
            foreach (var incomingStep in incomingSteps)
            {
                var existingStep = existingSteps.FirstOrDefault(es => es.Id == incomingStep.Id);

                if (existingStep != null)
                {
                    // Update properties
                    existingStep.StepOrder = incomingStep.StepOrder;
                    existingStep.ApplicationRoleId = incomingStep.ApplicationRoleId;
                    existingStep.ApplicationEntityId = incomingStep.ApplicationEntityId;
                    existingStep.MustApprove = incomingStep.MustApprove;
                    existingStep.RequireHigherApproval = incomingStep.RequireHigherApproval;
                    existingStep.HigherApprovalRoleId = incomingStep.HigherApprovalRoleId;
                    existingStep.ReserveQty = incomingStep.ReserveQty;
                    existingStep.ModifiedBy = _currentUserService.UserName;
                    existingStep.ModificationDate = DateTime.UtcNow;
                }
                else
                {
                    // Add new step
                    var newStep = new WorkflowStep
                    {
                        WorkflowId = workflow.Id,
                        StepOrder = incomingStep.StepOrder,
                        ApplicationRoleId = incomingStep.ApplicationRoleId,
                        ApplicationEntityId = incomingStep.ApplicationEntityId,
                        MustApprove = incomingStep.MustApprove,
                        RequireHigherApproval = incomingStep.RequireHigherApproval,
                        HigherApprovalRoleId = incomingStep.HigherApprovalRoleId,
                        ReserveQty = incomingStep.ReserveQty,
                        CreatedBy = _currentUserService.UserName,
                        CreationDate = DateTime.UtcNow
                    };
                    _context.WorkflowSteps.Add(newStep);
                }
            }
        }


        // Check if workflow step is referenced in approval history
        private async Task<bool> IsWorkflowStepInApprovalHistory(int workflowStepId, CancellationToken cancellationToken)
        {
            return await _context.WorkflowApprovalHistory
                .AnyAsync(ah => ah.WorkflowStepId == workflowStepId, cancellationToken);
        }

        // Ensure unique step orders
        private bool HasDuplicateStepOrder(List<WorkflowStepCreateDto> workflowSteps)
        {
            return workflowSteps
                .GroupBy(s => s.StepOrder)
                .Any(g => g.Count() > 1);
        }
    }
}
