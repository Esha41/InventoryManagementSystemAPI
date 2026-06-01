using Ettad.Application.Common.Interfaces;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.User.Services.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ettad.Workflows.Service.Queries.WorkflowApproval.GetCurrentApprovalStepByRequestId
{
    /// <summary>
    /// Resolves the current workflow approval step for a request for the calling user (roles, delegation, super-admin).
    /// </summary>
    public class GetCurrentApprovalStepByRequestIdQuery : IRequest<WorkflowApprovalStep?>
    {
        public long RequestId { get; }

        public GetCurrentApprovalStepByRequestIdQuery(long requestId)
        {
            RequestId = requestId;
        }
    }

    public class GetCurrentApprovalStepByRequestIdQueryHandler
        : IRequestHandler<GetCurrentApprovalStepByRequestIdQuery, WorkflowApprovalStep?>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserDelegationService _userDelegationService;
        private readonly IEffectiveRoleRepository _effectiveRoleService;

        public GetCurrentApprovalStepByRequestIdQueryHandler(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            IUserDelegationService userDelegationService,
            IEffectiveRoleRepository effectiveRoleService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userDelegationService = userDelegationService;
            _effectiveRoleService = effectiveRoleService;
        }

        public async Task<WorkflowApprovalStep?> Handle(
            GetCurrentApprovalStepByRequestIdQuery request,
            CancellationToken cancellationToken)
        {
            var steps = await _context.WorkflowApprovalSteps
                .Include(x => x.WorkflowStep)
                    .ThenInclude(ws => ws.ApplicationRole)
                .Include(x => x.WorkflowStep)
                    .ThenInclude(ws => ws.ParallelRoles)
                .Include(x => x.WorkflowStep)
                    .ThenInclude(ws => ws.Transitions)
                .Where(x => x.TargetRequestId == request.RequestId && x.IsCurrent)
                .OrderBy(x => x.Id)
                .ToListAsync(cancellationToken);

            if (!steps.Any())
                return null;

            var currentUserId = _currentUserService.UserId;

            if (_currentUserService.IsSuperAdmin)
            {
                var s0 = steps.First();
                s0.IsDelegation = false;
                return s0;
            }

            foreach (var step in steps)
            {
                step.IsDelegation = false;
                if (await TryAuthorizeWorkflowApprovalStepAsync(step, currentUserId))
                    return step;
            }

            throw new UnauthorizedAccessException("User cannot approve/reject this step");
        }

        private async Task<bool> TryAuthorizeWorkflowApprovalStepAsync(WorkflowApprovalStep step, string currentUserId)
        {
            var userRoleIds = string.IsNullOrEmpty(currentUserId)
                ? new List<string>()
                : (await _effectiveRoleService.GetEffectiveRoleIdsAsync(currentUserId)).ToList();

            var workflowStep = step.WorkflowStep;
            if (workflowStep == null)
                return false;

            var allowedRoles = CollectApproverRoleIdsForWorkflowStep(workflowStep)
                .Where(id => !string.IsNullOrEmpty(id))
                .Distinct()
                .ToList();

            if ((step.ApproverUserId == currentUserId) || userRoleIds.Any(r => allowedRoles.Contains(r)))
            {
                step.IsDelegation = false;
                return true;
            }

            var activeDelegations = await _userDelegationService.GetActiveDelegationsForUserAsync(currentUserId);
            if (activeDelegations == null || !activeDelegations.Any())
                return false;

            if (activeDelegations.Any(d => d.DelegatorUserId == step.ApproverUserId))
            {
                step.IsDelegation = true;
                return true;
            }

            // Authorize via the captured delegated role (the role the delegator was logged in
            // with when creating the delegation), not the delegator's current effective role.
            var delegatedRoleIds = activeDelegations
                .Select(d => d.DelegatorRoleId)
                .Where(id => !string.IsNullOrEmpty(id))
                .Distinct()
                .ToList();

            if (delegatedRoleIds.Any(r => allowedRoles.Contains(r)))
            {
                step.IsDelegation = true;
                return true;
            }

            return false;
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
    }
}
