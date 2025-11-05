using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Application.Common.Interfaces;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ettad.Data.Entities.Workflows;

namespace Ettad.Workflows.Service.Command.CreateWorkflow
{
    public class CreateWorkflowCommand : IRequest<APIOperationResponse<WorkflowDto>>
    {
        public string WorkflowName { get; set; }
        public WorkflowType WorkflowType { get; set; }
        public RequesterType RequesterType { get; set; }       
        public bool IsActive { get; set; } = true;
        public List<WorkflowStepCreateDto> WorkflowSteps { get; set; } = new();
    }

    public class CreateWorkflowCommandHandler : IRequestHandler<CreateWorkflowCommand, APIOperationResponse<WorkflowDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<CreateWorkflowCommandHandler> _logger;

        public CreateWorkflowCommandHandler(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            ILogger<CreateWorkflowCommandHandler> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<APIOperationResponse<WorkflowDto>> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting workflow creation for {WorkflowName} at {Time}", request.WorkflowName, DateTime.UtcNow);

            try
            {
                // Check for duplicate active workflow
                var deactivationResult = await CheckAndDeactivateDuplicateWorkflows(request, cancellationToken);
                if (!deactivationResult)
                {
                    return APIOperationResponse<WorkflowDto>.ServerError("Failed to deactivate duplicate workflows.");
                }

                var workflow = new Ettad.Data.Entities.Workflows.Workflow
                {
                    WorkflowName = request.WorkflowName,
                    WorkflowType = request.WorkflowType,
                    IsActive = request.IsActive,
                    CreatedBy = _currentUserService.UserName,
                    CreationDate = DateTime.UtcNow
                };

                _context.Workflows.Add(workflow);
                await _context.SaveChangesAsync(cancellationToken);

                if (request.WorkflowSteps?.Any() == true)
                {
                    var stepsResult = await AddWorkflowSteps(request.WorkflowSteps, workflow.Id, cancellationToken);
                    if (!stepsResult)
                    {
                        return APIOperationResponse<WorkflowDto>.ServerError("Workflow created but failed to add workflow steps.");
                    }
                }

                // Reload the workflow with steps to return complete data
                var createdWorkflow = await _context.Workflows
                    .Include(w => w.WorkflowSteps)
                    .FirstOrDefaultAsync(w => w.Id == workflow.Id, cancellationToken);

                var workflowDto = MapToWorkflowDto(createdWorkflow);

                _logger.LogInformation("Workflow created successfully with ID {WorkflowId} at {Time}", workflow.Id, DateTime.UtcNow);
                return APIOperationResponse<WorkflowDto>.Success(workflowDto, "Workflow created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating workflow {WorkflowName} at {Time}", request.WorkflowName, DateTime.UtcNow);
                return APIOperationResponse<WorkflowDto>.ServerError($"Workflow creation failed: {ex.Message}");
            }
        }

        // Updated duplicate check without organization/department
        private async Task<bool> CheckAndDeactivateDuplicateWorkflows(CreateWorkflowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingActiveWorkflows = await _context.Workflows
                    .Where(w => w.IsActive && !w.IsDeleted)
                    .ToListAsync(cancellationToken);

                if (existingActiveWorkflows.Any())
                {
                    _logger.LogInformation("Found {Count} active workflows. Deactivating them.", existingActiveWorkflows.Count);

                    foreach (var existingWorkflow in existingActiveWorkflows)
                    {
                        existingWorkflow.IsActive = false;
                        existingWorkflow.ModificationDate = DateTime.UtcNow;
                        existingWorkflow.ModifiedBy = _currentUserService.UserName;
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                    _logger.LogInformation("Successfully deactivated {Count} existing workflows", existingActiveWorkflows.Count);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deactivating duplicate workflows");
                return false;
            }
        }


        // Adds workflow steps to the created workflow
        private async Task<bool> AddWorkflowSteps(List<WorkflowStepCreateDto> workflowSteps, int workflowId, CancellationToken cancellationToken)
        {
            try
            {
                var stepsToAdd = workflowSteps.Select(step => new WorkflowStep
                {
                    WorkflowId = workflowId,
                    StepOrder = step.StepOrder,
                    ApplicationRoleId = step.ApplicationRoleId,
                    ApplicationEntityId = step.ApplicationEntityId,
                    MustApprove = step.MustApprove,
                    RequireHigherApproval = step.RequireHigherApproval,
                    HigherApprovalRoleId = step.HigherApprovalRoleId,
                    ReserveQty = step.ReserveQty,
                    CreatedBy = _currentUserService.UserName,
                    CreationDate = DateTime.UtcNow
                }).ToList();

                _context.WorkflowSteps.AddRange(stepsToAdd);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Added {StepCount} workflow steps for workflow ID {WorkflowId}", stepsToAdd.Count, workflowId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding workflow steps for workflow ID {WorkflowId}", workflowId);
                return false;
            }
        }


        // Manual mapping from Workflow entity to WorkflowDto
        private WorkflowDto MapToWorkflowDto(Ettad.Data.Entities.Workflows.Workflow workflow)
        {
            if (workflow == null) return null;

            return new WorkflowDto
            {
                Id = workflow.Id,
                WorkflowName = workflow.WorkflowName,
                WorkflowType = workflow.WorkflowType,
                IsActive = workflow.IsActive,
                IsDeleted = workflow.IsDeleted,

                WorkflowSteps = workflow.WorkflowSteps?.Select(step => new WorkflowStepDto
                {
                    Id = step.Id,
                    WorkflowId = step.WorkflowId,
                    StepOrder = step.StepOrder,
                    ApplicationRoleId = step.ApplicationRoleId,
                    ApplicationEntityId = step.ApplicationEntityId,
                    MustApprove = step.MustApprove,
                    RequireHigherApproval = step.RequireHigherApproval,
                    HigherApprovalRoleId = step.HigherApprovalRoleId,
                    ReserveQty = step.ReserveQty
                    
                }).ToList()
            };
        }

    }
}