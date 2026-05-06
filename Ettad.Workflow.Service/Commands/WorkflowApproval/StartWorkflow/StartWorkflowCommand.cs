using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Constants;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Notification.Service.Interfaces;
using Ettad.ResponseHandler.Models;
using Ettad.Workflow.Service.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Commands.WorkflowApproval.StartWorkflow
{
    public class StartWorkflowCommand : IRequest<APIOperationResponse<bool>>
    {
        public long OrderId { get; }
        public WorkflowType WorkflowType { get; }

        public StartWorkflowCommand(long orderId, WorkflowType workflowType)
        {
            OrderId = orderId;
            WorkflowType = workflowType;
        }
    }

    public class StartWorkflowCommandHandler : IRequestHandler<StartWorkflowCommand, APIOperationResponse<bool>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationHelperService _notificationHelperService;
        private readonly IWorkflowStepNotifierService _workflowStepNotifierService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITransactionManager _transactionManager;
        private readonly ILogger<StartWorkflowCommandHandler> _logger;

        public StartWorkflowCommandHandler(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            INotificationHelperService notificationHelperService,
            IWorkflowStepNotifierService workflowStepNotifierService,
            IDateTimeProvider dateTimeProvider,
            ITransactionManager transactionManager,
            ILogger<StartWorkflowCommandHandler> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _notificationHelperService = notificationHelperService;
            _workflowStepNotifierService = workflowStepNotifierService;
            _dateTimeProvider = dateTimeProvider;
            _transactionManager = transactionManager;
            _logger = logger;
        }

        public async Task<APIOperationResponse<bool>> Handle(StartWorkflowCommand request, CancellationToken cancellationToken)
        {
            await using var transaction = await _transactionManager.BeginAsync(cancellationToken);

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
                        await _transactionManager.CommitAsync(cancellationToken);

                        var baseRequest = await _context.BaseRequests
                            .FirstOrDefaultAsync(br => br.Id == request.OrderId, cancellationToken);

                        var approverRoles = CollectApproverRoleIdsForWorkflowStep(firstWorkflowStep);

                        var (userIds, roleIds) = await FilterNotificationRecipientsByDepartmentAsync(
                            approverRoles,
                            baseRequest?.DepartmentId,
                            cancellationToken);

                        await _notificationHelperService.SendNotificationAsync(
                            "New Approval Required",
                            "A request awaits your approval.",
                            "Request",
                            request.OrderId,
                            userIds,
                            roleIds,
                            _currentUserService.UserId);

                        await SendNotificationsToStepNotifiersOnWorkflowStartAsync(
                            firstWorkflowStep.Id,
                            request.OrderId,
                            cancellationToken);

                        return APIOperationResponse<bool>.Success(true);
                    }
                }

                await _transactionManager.CommitAsync(cancellationToken);
                return APIOperationResponse<bool>.Success(false);
            }
            catch (Exception ex)
            {
                await _transactionManager.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error starting workflow for order. OrderId: {OrderId}, WorkflowType: {WorkflowType}",
                    request.OrderId, request.WorkflowType);
                return APIOperationResponse<bool>.Success(false);
            }
        }

        private static List<string> CollectApproverRoleIdsForWorkflowStep(WorkflowStep wfs)
        {
            var list = new List<string>();
            if (!string.IsNullOrEmpty(wfs.ApplicationRoleId))
                list.Add(wfs.ApplicationRoleId);
            if (!string.IsNullOrEmpty(wfs.HigherApprovalRoleId))
                list.Add(wfs.HigherApprovalRoleId);
            if (wfs.ParallelRoles != null)
            {
                foreach (var pr in wfs.ParallelRoles)
                {
                    if (!string.IsNullOrEmpty(pr.RoleId))
                        list.Add(pr.RoleId);
                }
            }
            return list.Distinct().ToList();
        }

        private async Task<(List<string>? userIds, List<string>? roleIds)> FilterNotificationRecipientsByDepartmentAsync(
            List<string> roleIds,
            long? departmentId,
            CancellationToken cancellationToken)
        {
            if (roleIds == null || !roleIds.Any())
                return (null, null);

            var restrictedRoleNames = new List<string>
            {
                WorkflowRoleNames.SupplyOfficer,
                WorkflowRoleNames.RequestingEntityCommander
            };

            var roles = await _context.Roles
                .Where(r => roleIds.Contains(r.Id))
                .ToListAsync(cancellationToken);

            var restrictedRoleIds = new List<string>();
            var nonRestrictedRoleIds = new List<string>();
            var userIds = new List<string>();

            foreach (var role in roles)
            {
                if (restrictedRoleNames.Contains(role.Name))
                    restrictedRoleIds.Add(role.Id);
                else
                    nonRestrictedRoleIds.Add(role.Id);
            }

            if (restrictedRoleIds.Any() && departmentId.HasValue)
            {
                var usersInRestrictedRoles = await (from userRole in _context.Set<IdentityUserRole<string>>()
                                                    join user in _context.Users on userRole.UserId equals user.Id
                                                    where restrictedRoleIds.Contains(userRole.RoleId)
                                                          && user.DepartmentId == departmentId.Value
                                                          && !user.IsDeleted
                                                    select user.Id)
                                                    .Distinct()
                                                    .ToListAsync(cancellationToken);

                userIds.AddRange(usersInRestrictedRoles);
            }

            if (userIds.Any())
                return (userIds, nonRestrictedRoleIds.Any() ? nonRestrictedRoleIds : null);

            return (null, roleIds);
        }

        private async Task SendNotificationsToStepNotifiersOnWorkflowStartAsync(
            long workflowStepId,
            long requestId,
            CancellationToken cancellationToken)
        {
            try
            {
                var notifiersResult = await _workflowStepNotifierService.GetNotifierIdsByStepIdAsync(workflowStepId);

                if (!notifiersResult.Succeeded ||
                    notifiersResult.Data.UserIds.Count == 0 && notifiersResult.Data.RoleIds.Count == 0)
                    return;

                var (userIds, roleIds) = notifiersResult.Data;

                var baseRequest = await _context.BaseRequests
                    .FirstOrDefaultAsync(x => x.Id == requestId, cancellationToken);

                if (baseRequest == null)
                    return;

                await _notificationHelperService.SendNotificationAsync(
                    title: "Request Management",
                    message: $"Request #{baseRequest.RequestNo} has reached a workflow step that requires your attention.",
                    entityType: "Request",
                    entityId: baseRequest.Id,
                    userIds: userIds.Any() ? userIds : null,
                    roleIds: roleIds.Any() ? roleIds : null,
                    senderId: _currentUserService.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error sending start notifications to step notifiers for workflow step {WorkflowStepId}, RequestId: {RequestId}",
                    workflowStepId,
                    requestId);
            }
        }
    }
}
