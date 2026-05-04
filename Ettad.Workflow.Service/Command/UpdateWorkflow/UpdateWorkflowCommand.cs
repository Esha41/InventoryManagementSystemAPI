using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
using Ettad.CrossCutting.Comman.Time;
using Ettad.Workflows.Service.Dtos;

namespace Ettad.Workflows.Service.Command.UpdateWorkflow
{
    public class UpdateWorkflowCommand : IRequest<APIOperationResponse<WorkflowDto>>
    {
        public long Id { get; set; }
        public string WorkflowName { get; set; }
        public WorkflowType WorkflowType { get; set; }      
        public bool IsActive { get; set; } = true;
        public List<WorkflowStepCreateDto> WorkflowSteps { get; set; } = new();
    }

    public class UpdateWorkflowCommandHandler : IRequestHandler<UpdateWorkflowCommand, APIOperationResponse<WorkflowDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UpdateWorkflowCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpdateWorkflowCommandHandler(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            ILogger<UpdateWorkflowCommandHandler> logger,
            IMapper mapper,
            IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _currentUserService = currentUserService;
            _logger = logger;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<APIOperationResponse<WorkflowDto>> Handle(UpdateWorkflowCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating workflow {Id}", request.Id);

            var workflow = await _context.Workflows
                .Include(w => w.WorkflowSteps)
                    .ThenInclude(ws => ws.ParallelRoles)
                        .ThenInclude(pr => pr.Role)
                .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

            if (workflow == null)
                return APIOperationResponse<WorkflowDto>.NotFound($"Workflow {request.Id} not found");

            if (request.WorkflowSteps?.Any() == true && request.WorkflowSteps.GroupBy(x => x.StepOrder).Any(g => g.Count() > 1))
                return APIOperationResponse<WorkflowDto>.BadRequest("Duplicate StepOrder found");

            foreach (var step in request.WorkflowSteps.Where(x => x.Id > 0))
            {
                if (await _context.WorkflowStepApprovalLog.AnyAsync(x => x.WorkflowApprovalStepId == step.Id, cancellationToken))
                    return APIOperationResponse<WorkflowDto>.BadRequest($"Step {step.Id} is in approval history and cannot be modified");
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                // ✅ Deactivate other active workflows of same type
                if (request.IsActive)
                {
                    var activeDupes = await _context.Workflows
                        .Where(w => w.IsActive &&
                                    !w.IsDeleted &&
                                    w.Id != workflow.Id &&
                                    w.WorkflowType == request.WorkflowType)
                        .ToListAsync(cancellationToken);

                    foreach (var wf in activeDupes)
                    {
                        wf.IsActive = false;
                        wf.ModifiedBy = _currentUserService.UserName;
                        wf.ModificationDate = _dateTimeProvider.Now;
                    }
                }

                // ✅ Update workflow info
                workflow.WorkflowName = request.WorkflowName;
                workflow.WorkflowType = request.WorkflowType;
                workflow.IsActive = request.IsActive;
                workflow.ModifiedBy = _currentUserService.UserName;
                workflow.ModificationDate = _dateTimeProvider.Now;

                await UpdateWorkflowSteps(workflow, request.WorkflowSteps, cancellationToken);

                var freshWorkflow = await _context.Workflows
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(ws => ws.ApplicationRole)
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(ws => ws.HigherApprovalRole)
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(ws => ws.ParallelRoles)
                            .ThenInclude(pr => pr.Role)
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(ws => ws.Transitions)
                            .ThenInclude(t => t.TargetWorkflowStep)
                                .ThenInclude(ts => ts.ApplicationRole)
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(ws => ws.Transitions)
                            .ThenInclude(t => t.TargetWorkflowStep)
                                .ThenInclude(ts => ts.HigherApprovalRole)
                    .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

                await transaction.CommitAsync(cancellationToken);
                return APIOperationResponse<WorkflowDto>.Success(_mapper.Map<WorkflowDto>(freshWorkflow));
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }


 

        // Update workflow steps
        private async Task UpdateWorkflowSteps(Ettad.Data.Entities.Workflows.Workflow workflow, List<WorkflowStepCreateDto> incomingSteps, CancellationToken cancellationToken)
        {
            var existingSteps = workflow.WorkflowSteps.ToList();

            // Remove deleted steps
            foreach (var dbStep in existingSteps.Where(db => !incomingSteps.Any(i => i.Id == db.Id)))
            {
                if (!await _context.WorkflowStepApprovalLog.AnyAsync(x => x.WorkflowApprovalStepId == dbStep.Id, cancellationToken))
                    _context.WorkflowSteps.Remove(dbStep);
            }

            // Add/Update
            foreach (var dto in incomingSteps)
            {
                var step = existingSteps.FirstOrDefault(x => x.Id == dto.Id);

                if (step != null)
                {
                    step.StepOrder = dto.StepOrder;
                    step.ApplicationRoleId = dto.ApplicationRoleId;
                    step.ApplicationEntityId = dto.ApplicationEntityId;
                    step.MustApprove = dto.MustApprove;
                    step.RequireHigherApproval = dto.RequireHigherApproval;
                    step.HigherApprovalRoleId = dto.HigherApprovalRoleId;
                    step.ReserveQty = dto.ReserveQty;
                    step.CanReturn = dto.CanReturn;
                    step.ModifiedBy = _currentUserService.UserName;
                    step.ModificationDate = _dateTimeProvider.Now;
                }
                else
                {
                    _context.WorkflowSteps.Add(new WorkflowStep
                    {
                        WorkflowId = workflow.Id,
                        StepOrder = dto.StepOrder,
                        ApplicationRoleId = dto.ApplicationRoleId,
                        ApplicationEntityId = dto.ApplicationEntityId,
                        MustApprove = dto.MustApprove,
                        RequireHigherApproval = dto.RequireHigherApproval,
                        HigherApprovalRoleId = dto.HigherApprovalRoleId,
                        HigherApplicationEntityId=dto.HigherApplicationEntityId,
                        ReserveQty = dto.ReserveQty,
                        CanReturn = dto.CanReturn,
                        CreatedBy = _currentUserService.UserName,
                        CreationDate = _dateTimeProvider.Now
                    });
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            var stepsForParallel = await _context.WorkflowSteps
                .Where(ws => ws.WorkflowId == workflow.Id)
                .Include(ws => ws.ParallelRoles)
                .ToListAsync(cancellationToken);

            foreach (var dto in incomingSteps.OrderBy(x => x.StepOrder))
            {
                var stepEntity = dto.Id > 0
                    ? stepsForParallel.FirstOrDefault(s => s.Id == dto.Id)
                    : stepsForParallel.FirstOrDefault(s => s.StepOrder == dto.StepOrder);
                if (stepEntity == null)
                    continue;

                if (stepEntity.ParallelRoles != null && stepEntity.ParallelRoles.Count > 0)
                    _context.WorkflowStepParallelRoles.RemoveRange(stepEntity.ParallelRoles);

                foreach (var rid in (dto.ParallelRoleIds ?? Enumerable.Empty<string>()).Distinct())
                {
                    if (string.IsNullOrEmpty(rid) || rid == stepEntity.ApplicationRoleId)
                        continue;
                    _context.WorkflowStepParallelRoles.Add(new WorkflowStepParallelRole
                    {
                        WorkflowStepId = stepEntity.Id,
                        RoleId = rid,
                        CreatedBy = _currentUserService.UserName,
                        CreationDate = _dateTimeProvider.Now,
                        ModifiedBy = _currentUserService.UserName,
                        ModificationDate = _dateTimeProvider.Now
                    });
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
