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
            _logger.LogInformation("Updating workflow {Id}", request.Id);

            var workflow = await _context.Workflows
                .Include(w => w.WorkflowSteps)
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
                    wf.ModificationDate = DateTime.UtcNow;
                }
            }

            // ✅ Update workflow info
            workflow.WorkflowName = request.WorkflowName;
            workflow.WorkflowType = request.WorkflowType;
            workflow.IsActive = request.IsActive;
            workflow.ModifiedBy = _currentUserService.UserName;
            workflow.ModificationDate = DateTime.UtcNow;

            await UpdateWorkflowSteps(workflow, request.WorkflowSteps, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return APIOperationResponse<WorkflowDto>.Success(_mapper.Map<WorkflowDto>(workflow));
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
                    step.ModifiedBy = _currentUserService.UserName;
                    step.ModificationDate = DateTime.UtcNow;
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
                        CreatedBy = _currentUserService.UserName,
                        CreationDate = DateTime.UtcNow
                    });
                }
            }
        }

    }
}
