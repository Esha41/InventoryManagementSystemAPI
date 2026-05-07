using MediatR;
using Ettad.ResponseHandler.Models;
using Ettad.Data.Entities.Workflows;
using Ettad.EntityFramework.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using Ettad.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ettad.ResponseHandler.Consts;
using Ettad.CrossCutting.Comman.Time;

namespace Ettad.Workflows.Service.Commands.ManageTransitions
{
    public class SetWorkflowStepTransitionsCommand : IRequest<APIOperationResponse<bool>>
    {
        public long SourceStepId { get; set; }
        public List<long> TargetStepIds { get; set; } = new List<long>();
    }

    public class SetWorkflowStepTransitionsCommandHandler : IRequestHandler<SetWorkflowStepTransitionsCommand, APIOperationResponse<bool>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public SetWorkflowStepTransitionsCommandHandler(ApplicationDbContext context, ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<APIOperationResponse<bool>> Handle(SetWorkflowStepTransitionsCommand request, CancellationToken cancellationToken)
        {
            // Allow empty list to remove all transitions
            if (request.TargetStepIds == null)
            {
                request.TargetStepIds = new List<long>();
            }

            // Get source step
            var sourceStep = await _context.WorkflowSteps.FindAsync(new object[] { request.SourceStepId }, cancellationToken);
            if (sourceStep == null)
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Source step not found.");

            // Validate target steps only if list is not empty
            if (request.TargetStepIds.Any())
            {
                // Get all target steps
                var targetSteps = await _context.WorkflowSteps
                    .Where(ws => request.TargetStepIds.Contains(ws.Id))
                    .ToListAsync(cancellationToken);

                if (targetSteps.Count != request.TargetStepIds.Count)
                {
                    var foundIds = targetSteps.Select(ts => ts.Id).ToList();
                    var missingIds = request.TargetStepIds.Except(foundIds).ToList();
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, 
                        $"One or more target steps not found. Missing IDs: {string.Join(", ", missingIds)}");
                }

                // Validate all target steps belong to same workflow
                if (targetSteps.Any(ts => ts.WorkflowId != sourceStep.WorkflowId))
                {
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "All steps must belong to the same workflow.");
                }

                // Validate all target steps are after source step
                if (targetSteps.Any(ts => ts.StepOrder <= sourceStep.StepOrder))
                {
                    var invalidSteps = targetSteps.Where(ts => ts.StepOrder <= sourceStep.StepOrder).Select(ts => ts.Id).ToList();
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, 
                        $"Target steps must be after source step. Invalid step IDs: {string.Join(", ", invalidSteps)}");
                }
            }

            // Get existing transitions for this source step
            var existingTransitions = await _context.WorkflowStepTransitions
                .Where(t => t.SourceWorkflowStepId == request.SourceStepId)
                .ToListAsync(cancellationToken);

            var existingTargetIds = existingTransitions.Select(t => t.TargetWorkflowStepId).ToList();
            var newTargetIds = request.TargetStepIds.Except(existingTargetIds).ToList();
            var removedTargetIds = existingTargetIds.Except(request.TargetStepIds).ToList();

            // Remove transitions that are no longer in the list
            if (removedTargetIds.Any())
            {
                var transitionsToRemove = existingTransitions
                    .Where(t => removedTargetIds.Contains(t.TargetWorkflowStepId))
                    .ToList();
                _context.WorkflowStepTransitions.RemoveRange(transitionsToRemove);
            }

            // Add new transitions
            if (newTargetIds.Any())
            {
                var newTransitions = newTargetIds.Select(targetId => new WorkflowStepTransition
                {
                    SourceWorkflowStepId = request.SourceStepId,
                    TargetWorkflowStepId = targetId,
                    CreatedBy = _currentUserService.UserName ?? "System",
                    CreationDate = _dateTimeProvider.Now
                }).ToList();

                _context.WorkflowStepTransitions.AddRange(newTransitions);
            }

            // Enable CanSkip if we have any transitions
            var finalTransitionCount = existingTransitions.Count - removedTargetIds.Count + newTargetIds.Count;
            if (finalTransitionCount > 0 && !sourceStep.CanSkip)
            {
                sourceStep.CanSkip = true;
                sourceStep.ModificationDate = _dateTimeProvider.Now;
                sourceStep.ModifiedBy = _currentUserService.UserName ?? "System";
            }
            else if (finalTransitionCount == 0 && sourceStep.CanSkip)
            {
                // Disable CanSkip if no transitions remain
                sourceStep.CanSkip = false;
                sourceStep.ModificationDate = _dateTimeProvider.Now;
                sourceStep.ModifiedBy = _currentUserService.UserName ?? "System";
            }

            await _context.SaveChangesAsync(cancellationToken);

            var addedCount = newTargetIds.Count;
            var removedCount = removedTargetIds.Count;
            var message = $"Transition{(addedCount + removedCount > 1 ? "s" : "")} processed successfully. " +
                         $"Added: {addedCount}, Removed: {removedCount}, Total transitions: {finalTransitionCount}";

            return APIOperationResponse<bool>.Success(true, message);
        }
    }
}

