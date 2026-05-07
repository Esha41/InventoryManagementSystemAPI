using Ettad.Application.Common.Interfaces;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Data.Interfaces.Services;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.User.Services.Interfaces;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Queries.WorkflowApproval.GetOrdersWithApprovalSteps
{
    public class GetOrdersWithApprovalStepsQuery : IRequest<APIOperationResponse<List<WorkflowApprovalWithOrderDto>>>
    {
    }

    public class GetOrdersWithApprovalStepsQueryHandler : IRequestHandler<GetOrdersWithApprovalStepsQuery, APIOperationResponse<List<WorkflowApprovalWithOrderDto>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserDelegationService _userDelegationService;
        private readonly IEffectiveRoleRepository _effectiveRoleService;
        private readonly ILogger<GetOrdersWithApprovalStepsQueryHandler> _logger;

        public GetOrdersWithApprovalStepsQueryHandler(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            IUserDelegationService userDelegationService,
            IEffectiveRoleRepository effectiveRoleService,
            ILogger<GetOrdersWithApprovalStepsQueryHandler> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userDelegationService = userDelegationService;
            _effectiveRoleService = effectiveRoleService;
            _logger = logger;
        }

        public async Task<APIOperationResponse<List<WorkflowApprovalWithOrderDto>>> Handle(GetOrdersWithApprovalStepsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _currentUserService.UserId;

                // 1. Effective role ID(s) for current user (active session)
                var userRoleIds = string.IsNullOrEmpty(currentUserId)
                    ? new List<string>()
                    : (await _effectiveRoleService.GetEffectiveRoleIdsAsync(currentUserId)).ToList();

                // 1.5 Get active delegations (users who delegated to current user for workflow approval)
                var activeDelegatorIds = await _userDelegationService.GetActiveDelegatorsForUserAsync(currentUserId, DelegationScope.WorkflowApproval);
                var delegatorRoleIds = new List<string>();
                
                if (activeDelegatorIds != null && activeDelegatorIds.Any())
                {
                    foreach (var delegatorId in activeDelegatorIds)
                        delegatorRoleIds.AddRange(await _effectiveRoleService.GetEffectiveRoleIdsAsync(delegatorId));
                }

                // Ensure lists are not null for Contains queries (though EF handles this, it's safer)
                if (userRoleIds == null) userRoleIds = new List<string>();
                if (delegatorRoleIds == null) delegatorRoleIds = new List<string>();

                // 2. Query workflow approval steps joined with workflow steps and base requests
                var query = from ws in _context.WorkflowApprovalSteps
                            join br in _context.BaseRequests
                                on ws.TargetRequestId equals br.Id
                            join wfs in _context.WorkflowSteps
                                on ws.WorkflowStepId equals wfs.Id
                            where
                                ws.IsCurrent // Only fetch current active steps
                                && (
                                    // Step assigned directly to this user
                                    ws.ApproverUserId == currentUserId
                                    // OR user role matches main approver
                                    || userRoleIds.Contains(wfs.ApplicationRoleId)
                                    // OR user role matches higher approval
                                    || (!string.IsNullOrEmpty(wfs.HigherApprovalRoleId) && userRoleIds.Contains(wfs.HigherApprovalRoleId))
                                    // OR user role matches a parallel approver role on this step
                                    || _context.WorkflowStepParallelRoles.Any(pr => pr.WorkflowStepId == wfs.Id && userRoleIds.Contains(pr.RoleId))
                                    
                                    // --- Delegation Logic ---
                                    // OR step assigned to a delegator
                                    || activeDelegatorIds.Contains(ws.ApproverUserId)
                                    // OR delegator matches main approver role
                                    || delegatorRoleIds.Contains(wfs.ApplicationRoleId)
                                    // OR delegator matches higher approval role
                                    || (!string.IsNullOrEmpty(wfs.HigherApprovalRoleId) && delegatorRoleIds.Contains(wfs.HigherApprovalRoleId))
                                    // OR delegator matches a parallel approver role
                                    || _context.WorkflowStepParallelRoles.Any(pr => pr.WorkflowStepId == wfs.Id && delegatorRoleIds.Contains(pr.RoleId))
                                )

                            select new WorkflowApprovalWithOrderDto
                            {
                                // WorkflowStep properties
                                WorkflowId = wfs.WorkflowId,
                                StepOrder = wfs.StepOrder,
                                ApplicationRoleId = wfs.ApplicationRoleId,
                                ApplicationEntityId = wfs.ApplicationEntityId,
                                RequireHigherApproval = wfs.RequireHigherApproval,
                                HigherApprovalRoleId = wfs.HigherApprovalRoleId,
                                HigherApplicationEntityId = wfs.HigherApplicationEntityId,
                                ReserveQty = wfs.ReserveQty,

                                // WorkflowApprovalStep properties
                                WorkflowStepId = ws.WorkflowStepId,
                                TargetRequestId = ws.TargetRequestId,
                                RequestType = ws.RequestType,
                                ApprovedDate = ws.ApprovedDate,
                                Status = ws.Status,
                                Comments = ws.Comments,
                                IsCurrent = ws.IsCurrent,
                                ApproverUserId = ws.ApproverUserId,

                                // BaseRequest properties
                                BaseRequestId = br.Id,
                                RequestNo = br.RequestNo,
                                BaseRequestType = br.RequestType,
                                Reason = br.Reason,
                                Priority = br.Priority,
                                BaseRequestStatus = br.Status,
                                Notes = br.Notes,
                                DepartmentId = br.DepartmentId,
                                RequesterId = br.RequesterId,
                                RequestPurposeId = br.RequestPurposeId,
                                RequestDate = br.CreationDate,
                                IsDelegation = ws.IsDelegation,

                                WorkflowApprovalStepId = ws.Id
                            };

                var result = await query.ToListAsync(cancellationToken);
                return APIOperationResponse<List<WorkflowApprovalWithOrderDto>>.Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching orders with approval steps");
                return APIOperationResponse<List<WorkflowApprovalWithOrderDto>>.ServerError($"Processing failed: {ex.Message}");
            }
        }
    }
}
