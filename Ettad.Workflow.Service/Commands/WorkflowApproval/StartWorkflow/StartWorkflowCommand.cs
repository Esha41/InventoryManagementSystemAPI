using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Workflow.Service.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Commands.WorkflowApproval.StartWorkflow
{
    public sealed record StartWorkflowResult(bool Started, long? NotificationWorkflowStepId);

    public class StartWorkflowCommand : IRequest<APIOperationResponse<StartWorkflowResult>>
    {
        public long OrderId { get; }
        public WorkflowType WorkflowType { get; }

        public StartWorkflowCommand(long orderId, WorkflowType workflowType)
        {
            OrderId = orderId;
            WorkflowType = workflowType;
        }
    }

    public class StartWorkflowCommandHandler : IRequestHandler<StartWorkflowCommand, APIOperationResponse<StartWorkflowResult>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITransactionManager _transactionManager;
        private readonly IWorkflowStartNotificationService _workflowStartNotificationService;
        private readonly ILogger<StartWorkflowCommandHandler> _logger;

        public StartWorkflowCommandHandler(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider,
            ITransactionManager transactionManager,
            IWorkflowStartNotificationService workflowStartNotificationService,
            ILogger<StartWorkflowCommandHandler> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
            _transactionManager = transactionManager;
            _workflowStartNotificationService = workflowStartNotificationService;
            _logger = logger;
        }

        public async Task<APIOperationResponse<StartWorkflowResult>> Handle(StartWorkflowCommand request, CancellationToken cancellationToken)
        {
            var ownsTransaction = !_transactionManager.HasActiveTransaction;
            if (ownsTransaction)
                await _transactionManager.BeginAsync(cancellationToken);

            try
            {
                var workflow = await _context.Workflows
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(ws => ws.ParallelRoles)
                    .FirstOrDefaultAsync(w => w.IsActive && !w.IsDeleted && w.WorkflowType == request.WorkflowType, cancellationToken);

                if (workflow != null &&
                    workflow.WorkflowSteps != null &&
                    workflow.WorkflowSteps.Any())
                {
                    var firstWorkflowStep = workflow.WorkflowSteps
                        .OrderBy(ws => ws.StepOrder)
                        .FirstOrDefault();

                    if (firstWorkflowStep != null)
                    {
                        var workflowApprovalStep = new WorkflowApprovalStep
                        {
                            WorkflowStepId = firstWorkflowStep.Id,
                            TargetRequestId = request.OrderId,
                            RequestType = request.WorkflowType,
                            Status = RequestStatus.New,
                            IsCurrent = true,
                            CreationDate = _dateTimeProvider.Now,
                            CreatedBy = _currentUserService.UserId
                        };

                        _context.WorkflowApprovalSteps.Add(workflowApprovalStep);
                        await _context.SaveChangesAsync(cancellationToken);

                        if (ownsTransaction)
                            await _transactionManager.CommitAsync(cancellationToken);

                        long? notificationStepId = null;
                        if (ownsTransaction)
                        {
                            await _workflowStartNotificationService.SendAsync(
                                request.OrderId,
                                firstWorkflowStep.Id,
                                cancellationToken);
                        }
                        else
                            notificationStepId = firstWorkflowStep.Id;

                        return APIOperationResponse<StartWorkflowResult>.Success(
                            new StartWorkflowResult(true, notificationStepId));
                    }
                }

                if (ownsTransaction)
                    await _transactionManager.CommitAsync(cancellationToken);

                return APIOperationResponse<StartWorkflowResult>.Success(new StartWorkflowResult(false, null));
            }
            catch (Exception ex)
            {
                if (ownsTransaction)
                    await _transactionManager.RollbackAsync(cancellationToken);

                _logger.LogError(ex, "Error starting workflow for order. OrderId: {OrderId}, WorkflowType: {WorkflowType}",
                    request.OrderId, request.WorkflowType);

                return APIOperationResponse<StartWorkflowResult>.Fail(ResponseType.InternalServerError,
                    $"An error occurred while starting the approval workflow: {ex.Message}");
            }
        }
    }
}
