using MediatR;
using Ettad.ResponseHandler.Models;
using Ettad.EntityFramework.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using Ettad.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ettad.ResponseHandler.Consts;
using Ettad.CrossCutting.Comman.Time;

namespace Ettad.Workflows.Service.Commands.ManageTransitions
{
    public class RemoveWorkflowStepTransitionCommand : IRequest<APIOperationResponse<bool>>
    {
        public long SourceStepId { get; set; }
        public long TargetStepId { get; set; }
    }

    public class RemoveWorkflowStepTransitionCommandHandler : IRequestHandler<RemoveWorkflowStepTransitionCommand, APIOperationResponse<bool>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RemoveWorkflowStepTransitionCommandHandler(ApplicationDbContext context, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<APIOperationResponse<bool>> Handle(RemoveWorkflowStepTransitionCommand request, CancellationToken cancellationToken)
        {
            var transition = await _context.WorkflowStepTransitions
                .FirstOrDefaultAsync(t => t.SourceWorkflowStepId == request.SourceStepId && t.TargetWorkflowStepId == request.TargetStepId, cancellationToken);

            if (transition == null)
                return APIOperationResponse<bool>.Fail(ResponseType.NotFound, "Transition not found.");

            _context.WorkflowStepTransitions.Remove(transition);
            
            // Check if any transitions remain for this source step
            var remainingTransitions = await _context.WorkflowStepTransitions
                .AnyAsync(t => t.SourceWorkflowStepId == request.SourceStepId && t.TargetWorkflowStepId != request.TargetStepId, cancellationToken);

            if (!remainingTransitions)
            {
                var sourceStep = await _context.WorkflowSteps.FindAsync(request.SourceStepId);
                if (sourceStep != null)
                {
                    sourceStep.CanSkip = false;
                    sourceStep.ModificationDate = _dateTimeProvider.Now;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return APIOperationResponse<bool>.Success(true, "Transition removed successfully.");
        }
    }
}

