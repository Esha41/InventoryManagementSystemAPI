using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ettad.Application.Common.Interfaces;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Models;
using Ettad.Data.Enums;
using Ettad.User.Services.DTO;
using Ettad.Workflows.Service.Dtos;
using Ettad.Workflows.Service.Mapper;

namespace Ettad.Workflows.Service.Queries.GetWorkflow
{
    public class GetWorkflowsByTypeQuery : IRequest<APIOperationResponse<List<WorkflowDto>>>
    {
        public WorkflowType WorkflowType { get; }

        public GetWorkflowsByTypeQuery(WorkflowType workflowType)
        {
            WorkflowType = workflowType;
        }
    }

    public class GetWorkflowsByTypeQueryHandler : IRequestHandler<GetWorkflowsByTypeQuery, APIOperationResponse<List<WorkflowDto>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetWorkflowsByTypeQueryHandler(
            ApplicationDbContext context,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<APIOperationResponse<List<WorkflowDto>>> Handle(GetWorkflowsByTypeQuery request, CancellationToken cancellationToken)
        {
            var workflowsQueryable = _context.Workflows
                .Where(w => !w.IsDeleted && w.WorkflowType == request.WorkflowType)
                .AsQueryable();

            // Filter based on user if needed
            if (!_currentUserService.IsSuperAdmin)
            {
                workflowsQueryable = workflowsQueryable; // Add filter if required
            }

            var workflows = await workflowsQueryable
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
                .ToListAsync(cancellationToken);

            var workflowDtos = workflows.Select(w => new WorkflowDto
            {
                Id = w.Id,
                WorkflowName = w.WorkflowName,
                WorkflowType = w.WorkflowType,
                IsActive = w.IsActive,
                IsDeleted = w.IsDeleted,
                WorkflowSteps = w.WorkflowSteps.Select(step => new WorkflowStepDto
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
            }).ToList();

            var workflowDict = workflows.ToDictionary(w => w.Id);
            foreach (var dto in workflowDtos)
            {
                if (workflowDict.TryGetValue(dto.Id, out var wf))
                    WorkflowAutoRejectTriggerMapper.MapToWorkflowDto(dto, wf.AutoRejectTrigger);
            }

            return APIOperationResponse<List<WorkflowDto>>.Success(workflowDtos);
        }
    }
}

