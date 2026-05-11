using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.Workflows.Service.Dtos;
using Ettad.Workflows.Service.Mapper;

namespace Ettad.Workflows.Service.Queries.GetWorkflowById
{
    public class GetWorkflowByIdQuery : IRequest<APIOperationResponse<WorkflowDto>>
    {
        public long Id { get; }

        public GetWorkflowByIdQuery(long id)
        {
            Id = id;
        }
    }

    public class GetWorkflowByIdQueryHandler : IRequestHandler<GetWorkflowByIdQuery, APIOperationResponse<WorkflowDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetWorkflowByIdQueryHandler> _logger;

        public GetWorkflowByIdQueryHandler(
            ApplicationDbContext context,
            IMapper mapper,
            ILogger<GetWorkflowByIdQueryHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<APIOperationResponse<WorkflowDto>> Handle(GetWorkflowByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var workflow = await _context.Workflows
                    .Include(w => w.AutoRejectTrigger)
                        .ThenInclude(t => t!.TriggerRoles)
                    .Include(w => w.AutoRejectTrigger)
                        .ThenInclude(t => t!.TriggerSteps)
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(step => step.Transitions)
                            .ThenInclude(t => t.TargetWorkflowStep)
                                .ThenInclude(target => target.ApplicationRole)
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(step => step.Transitions)
                            .ThenInclude(t => t.TargetWorkflowStep)
                                .ThenInclude(target => target.HigherApprovalRole)
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(step => step.ApplicationRole)
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(step => step.ParallelRoles)
                            .ThenInclude(pr => pr.Role)
                    .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

                if (workflow == null)
                {
                    return APIOperationResponse<WorkflowDto>.NotFound($"Workflow with ID {request.Id} not found.");
                }

                var workflowDto = new WorkflowDto
                {
                    Id = workflow.Id,
                    WorkflowName = workflow.WorkflowName,
                    WorkflowType = workflow.WorkflowType,
                    IsActive = workflow.IsActive,
                    IsDeleted = workflow.IsDeleted,
                    WorkflowSteps = workflow.WorkflowSteps.Select(step => new WorkflowStepDto
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
                        ParallelRoles = step.ParallelRoles?.Select(pr => new WorkflowStepParallelRoleDto
                        {
                            Id = pr.Id,
                            WorkflowStepId = pr.WorkflowStepId,
                            RoleId = pr.RoleId,
                            RoleName = pr.Role?.Name,
                            RoleNameAr = pr.Role?.NameAr
                        }).ToList() ?? new List<WorkflowStepParallelRoleDto>(),
                        Transitions = step.Transitions.Select(t => new WorkflowStepTransitionDto
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
                        }).ToList()
                    }).ToList()
                };

                WorkflowAutoRejectTriggerMapper.MapToWorkflowDto(workflowDto, workflow.AutoRejectTrigger);

                return APIOperationResponse<WorkflowDto>.Success(workflowDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching workflow by ID {WorkflowId}", request.Id);
                return APIOperationResponse<WorkflowDto>.ServerError($"Processing failed: {e.Message}");
            }
        }
    }
}