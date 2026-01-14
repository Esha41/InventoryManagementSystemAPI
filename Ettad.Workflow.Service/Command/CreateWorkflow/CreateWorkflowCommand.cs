using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Application.Common.Interfaces;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.DTO;
using Ettad.User.Services.DTO;
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
            _logger.LogInformation("Starting workflow creation for {WorkflowName} at {Time}", request.WorkflowName, DateTime.Now);

            try
            {
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
                    CreationDate = DateTime.Now
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
                        .ThenInclude(ws => ws.ApplicationRole)
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(ws => ws.HigherApprovalRole)
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(ws => ws.Transitions)
                            .ThenInclude(t => t.TargetWorkflowStep)
                                .ThenInclude(target => target.ApplicationRole)
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(ws => ws.Transitions)
                            .ThenInclude(t => t.TargetWorkflowStep)
                                .ThenInclude(target => target.HigherApprovalRole)
                    .FirstOrDefaultAsync(w => w.Id == workflow.Id, cancellationToken);

                var workflowDto = MapToWorkflowDto(createdWorkflow);

                _logger.LogInformation("Workflow created successfully with ID {WorkflowId} at {Time}", workflow.Id, DateTime.Now);
                return APIOperationResponse<WorkflowDto>.Success(workflowDto, "Workflow created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating workflow {WorkflowName} at {Time}", request.WorkflowName, DateTime.Now);
                return APIOperationResponse<WorkflowDto>.ServerError($"Workflow creation failed: {ex.Message}");
            }
        }

        // Updated duplicate check without organization/department
        private async Task<bool> CheckAndDeactivateDuplicateWorkflows(CreateWorkflowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Find active workflows of the same type as the incoming request
                var existingActiveWorkflows = await _context.Workflows
                    .Where(w => w.IsActive && !w.IsDeleted && w.WorkflowType == request.WorkflowType)
                    .ToListAsync(cancellationToken);

                if (existingActiveWorkflows.Any())
                {
                    _logger.LogInformation("Found {Count} active workflows of type {Type}. Deactivating them.",
                        existingActiveWorkflows.Count, request.WorkflowType);

                    foreach (var existingWorkflow in existingActiveWorkflows)
                    {
                        existingWorkflow.IsActive = false;
                        existingWorkflow.ModificationDate = DateTime.Now;
                        existingWorkflow.ModifiedBy = _currentUserService.UserName;
                    }

                    await _context.SaveChangesAsync(cancellationToken);
                    _logger.LogInformation("Successfully deactivated {Count} workflows of type {Type}",
                        existingActiveWorkflows.Count, request.WorkflowType);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deactivating duplicate workflows of type {Type}", request.WorkflowType);
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
                    HigherApplicationEntityId = step.HigherApplicationEntityId,
                    ReserveQty = step.ReserveQty,
                    CanReturn = step.CanReturn,
                    CreatedBy = _currentUserService.UserName,
                    CreationDate = DateTime.Now
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
                    ApplicationRoleName = step.ApplicationRole?.Name,
                    ApplicationEntityId = step.ApplicationEntityId,
                    MustApprove = step.MustApprove,
                    RequireHigherApproval = step.RequireHigherApproval,
                    HigherApprovalRoleId = step.HigherApprovalRoleId,
                    HigherApplicationEntityId = step.HigherApplicationEntityId,
                    ReserveQty = step.ReserveQty,
                    CanSkip = step.CanSkip,
                    CanReturn = step.CanReturn,
                    Transitions = step.Transitions?.Select(t => new WorkflowStepTransitionDto
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
                    
                }).ToList()
            };
        }

    }
}