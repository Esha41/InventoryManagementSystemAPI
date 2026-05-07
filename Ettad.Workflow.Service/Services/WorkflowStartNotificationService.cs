using Ettad.Application.Common.Interfaces;
using Ettad.Data.Constants;
using Ettad.Data.Entities.Workflows;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.Notification.Service.Interfaces;
using Ettad.Workflow.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Services
{
    public class WorkflowStartNotificationService : IWorkflowStartNotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationHelperService _notificationHelperService;
        private readonly IWorkflowStepNotifierService _workflowStepNotifierService;
        private readonly ILogger<WorkflowStartNotificationService> _logger;

        public WorkflowStartNotificationService(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            INotificationHelperService notificationHelperService,
            IWorkflowStepNotifierService workflowStepNotifierService,
            ILogger<WorkflowStartNotificationService> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _notificationHelperService = notificationHelperService;
            _workflowStepNotifierService = workflowStepNotifierService;
            _logger = logger;
        }

        public async Task SendAsync(long requestId, long workflowStepId, CancellationToken cancellationToken = default)
        {
            var step = await _context.WorkflowSteps
                .Include(ws => ws.ParallelRoles)
                .FirstOrDefaultAsync(ws => ws.Id == workflowStepId, cancellationToken);

            if (step == null)
            {
                _logger.LogWarning("Workflow step {WorkflowStepId} not found for start notifications. RequestId: {RequestId}",
                    workflowStepId, requestId);
                return;
            }

            var baseRequest = await _context.BaseRequests
                .FirstOrDefaultAsync(br => br.Id == requestId, cancellationToken);

            var approverRoles = CollectApproverRoleIdsForWorkflowStep(step);

            var (userIds, roleIds) = await FilterNotificationRecipientsByDepartmentAsync(
                approverRoles,
                baseRequest?.DepartmentId,
                cancellationToken);

            await _notificationHelperService.SendNotificationAsync(
                "New Approval Required",
                "A request awaits your approval.",
                "Request",
                requestId,
                userIds,
                roleIds,
                _currentUserService.UserId);

            await SendNotificationsToStepNotifiersOnWorkflowStartAsync(
                workflowStepId,
                requestId,
                cancellationToken);
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
