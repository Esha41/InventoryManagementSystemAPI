using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Entities.Workflows;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Commands.ReplaceWorkflowStepRequesterQtyNotifications
{
    public class ReplaceWorkflowStepRequesterQtyNotificationsCommandHandler
        : IRequestHandler<ReplaceWorkflowStepRequesterQtyNotificationsCommand, APIOperationResponse<bool>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<ReplaceWorkflowStepRequesterQtyNotificationsCommandHandler> _logger;

        public ReplaceWorkflowStepRequesterQtyNotificationsCommandHandler(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider,
            ILogger<ReplaceWorkflowStepRequesterQtyNotificationsCommandHandler> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task<APIOperationResponse<bool>> Handle(
            ReplaceWorkflowStepRequesterQtyNotificationsCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            if (dto == null)
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Body is required");

            if (dto.WorkflowId <= 0)
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "WorkflowId is required");

            if (dto.WorkflowStepIds == null)
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "WorkflowStepIds is required");

            var distinctIds = dto.WorkflowStepIds.Distinct().ToList();

            var workflowOk = await _context.Workflows.AsNoTracking()
                .AnyAsync(w => w.Id == dto.WorkflowId && !w.IsDeleted, cancellationToken);
            if (!workflowOk)
            {
                return APIOperationResponse<bool>.Fail(ResponseType.BadRequest, "Workflow not found");
            }

            if (distinctIds.Count > 0)
            {
                var validStepIds = await _context.WorkflowSteps
                    .AsNoTracking()
                    .Where(s => distinctIds.Contains(s.Id) && s.WorkflowId == dto.WorkflowId)
                    .Select(s => s.Id)
                    .ToListAsync(cancellationToken);

                if (validStepIds.Count != distinctIds.Count)
                {
                    var missing = distinctIds.Except(validStepIds).ToList();
                    _logger.LogWarning(
                        "Replace requester qty notification config: invalid or foreign step ids for workflow {WorkflowId}: {Missing}",
                        dto.WorkflowId,
                        missing);
                    return APIOperationResponse<bool>.Fail(ResponseType.BadRequest,
                        "One or more workflow step IDs are invalid or do not belong to this workflow");
                }
            }

            await using var tx = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var existingForWorkflow = await _context.WorkflowStepRequesterQuantityNotifications
                    .Where(x => x.WorkflowId == dto.WorkflowId)
                    .ToListAsync(cancellationToken);
                _context.WorkflowStepRequesterQuantityNotifications.RemoveRange(existingForWorkflow);

                if (distinctIds.Count > 0)
                {
                    var now = _dateTimeProvider.Now;
                    var userId = _currentUserService.UserId ?? "System";

                    foreach (var stepId in distinctIds.OrderBy(x => x))
                    {
                        _context.WorkflowStepRequesterQuantityNotifications.Add(new WorkflowStepRequesterQuantityNotification
                        {
                            WorkflowId = dto.WorkflowId,
                            WorkflowStepId = stepId,
                            CreationDate = now,
                            CreatedBy = userId
                        });
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
                await tx.CommitAsync(cancellationToken);

                if (distinctIds.Count == 0)
                {
                    _logger.LogInformation(
                        "Cleared requester quantity notification steps for workflow {WorkflowId}",
                        dto.WorkflowId);
                }
                else
                {
                    _logger.LogInformation(
                        "Replaced requester quantity notification steps for workflow {WorkflowId}; count {Count}",
                        dto.WorkflowId,
                        distinctIds.Count);
                }

                return APIOperationResponse<bool>.Success(true, null);
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Failed to replace requester quantity notification configuration");
                return APIOperationResponse<bool>.Fail(ResponseType.InternalServerError, ex.Message);
            }
        }
    }
}
