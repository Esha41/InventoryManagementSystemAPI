using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Data.Constants;
using Ettad.Data.Entities;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.Data.Interfaces.Services;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Ettad.Workflows.Service.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.Workflows.Service.Queries.WorkflowApproval.GetAllBaseRequests
{
    public class GetAllBaseRequestsQuery : IRequest<APIOperationResponse<List<BaseRequestDto>>>
    {
    }

    public class GetAllBaseRequestsQueryHandler : IRequestHandler<GetAllBaseRequestsQuery, APIOperationResponse<List<BaseRequestDto>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserDelegationService _userDelegationService;
        private readonly IEffectiveRoleRepository _effectiveRoleService;
        private readonly IFileUploadService _fileUploadService;
        private readonly ILogger<GetAllBaseRequestsQueryHandler> _logger;

        public GetAllBaseRequestsQueryHandler(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            IUserDelegationService userDelegationService,
            IEffectiveRoleRepository effectiveRoleService,
            IFileUploadService fileUploadService,
            ILogger<GetAllBaseRequestsQueryHandler> logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userDelegationService = userDelegationService;
            _effectiveRoleService = effectiveRoleService;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        public async Task<APIOperationResponse<List<BaseRequestDto>>> Handle(GetAllBaseRequestsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _currentUserService.UserId;
                var userDepartmentId = _currentUserService.DepartmentId;

                // --- DELEGATION & ROLE PRE-FETCHING START ---
                var userRoleIds = string.IsNullOrEmpty(currentUserId)
                    ? new List<string>()
                    : (await _effectiveRoleService.GetEffectiveRoleIdsAsync(currentUserId)).ToList();

                var activeDelegations = await _userDelegationService.GetActiveDelegationsForUserAsync(currentUserId);
                var activeDelegatorIds = activeDelegations.Select(d => d.DelegatorUserId).Distinct().ToList();

                // Captured delegated roles: the role each delegator was logged in with when the
                // delegation was created. The delegatee acts as exactly that role.
                var delegatorRoleIds = activeDelegations
                    .Select(d => d.DelegatorRoleId)
                    .Where(id => !string.IsNullOrEmpty(id))
                    .Distinct()
                    .ToList();

                var allRelevantRoleIds = userRoleIds.Concat(delegatorRoleIds).Distinct().ToList();
                var allRelevantRoleNames = allRelevantRoleIds.Count == 0
                    ? new List<string>()
                    : await _context.Roles
                        .Where(r => allRelevantRoleIds.Contains(r.Id))
                        .Select(r => r.Name!)
                        .ToListAsync(cancellationToken);
                // --- DELEGATION & ROLE PRE-FETCHING END ---

                List<long> allowedRequestIds;

                // Roles that are restricted to their own department
                var restrictedRoles = new List<string>
                {
                    WorkflowRoleNames.SupplyOfficer,
                    WorkflowRoleNames.RequestingEntityCommander
                };

                var userRoleNames = userRoleIds.Count == 0
                    ? new List<string>()
                    : await _context.Roles
                        .Where(r => userRoleIds.Contains(r.Id))
                        .Select(r => r.Name!)
                        .ToListAsync(cancellationToken);

                var shouldFilterByDepartment = userDepartmentId.HasValue && 
                    userRoleNames.Any(roleName => restrictedRoles.Contains(roleName));

                // If superadmin, get all request IDs (still filter by department if user has restricted role)
                if (_currentUserService.IsSuperAdmin)
                {
                    var query = _context.BaseRequests.Where(br => !br.IsDeleted);
                    
                    // Filter by department only if user has a restricted role
                    if (shouldFilterByDepartment)
                    {
                        query = query.Where(br => br.DepartmentId == userDepartmentId.Value);
                    }
                    
                    allowedRequestIds = await query.Select(br => br.Id).ToListAsync(cancellationToken);
                }
                else
                {
                    // Get request IDs that the user has permission to approve (including delegation)
                    var workflowRequestIds = new List<long>();
                    
                    if (userRoleIds.Any() || activeDelegatorIds.Any())
                    {
                        var workflowQuery = from ws in _context.WorkflowApprovalSteps
                                           join br in _context.BaseRequests
                                               on ws.TargetRequestId equals br.Id
                                           join wfs in _context.WorkflowSteps
                                               on ws.WorkflowStepId equals wfs.Id
                                           where
                                               !br.IsDeleted &&
                                               (
                                                   // Direct Assignment (User OR Delegator)
                                                   (ws.ApproverUserId == currentUserId || activeDelegatorIds.Contains(ws.ApproverUserId)) ||
                                                   
                                                   // Role Assignment (User Role OR Delegator Role) + parallel approver roles
                                                   (
                                                       (userRoleIds.Contains(wfs.ApplicationRoleId) || delegatorRoleIds.Contains(wfs.ApplicationRoleId)) ||
                                                       (!string.IsNullOrEmpty(wfs.HigherApprovalRoleId) && (userRoleIds.Contains(wfs.HigherApprovalRoleId) || delegatorRoleIds.Contains(wfs.HigherApprovalRoleId))) ||
                                                       _context.WorkflowStepParallelRoles.Any(pr => pr.WorkflowStepId == wfs.Id && (userRoleIds.Contains(pr.RoleId) || delegatorRoleIds.Contains(pr.RoleId)))
                                                   )
                                               )
                                           select br;
                        
                        // Filter by department only if user has a restricted role
                        if (shouldFilterByDepartment)
                        {
                            workflowQuery = workflowQuery.Where(br => br.DepartmentId == userDepartmentId.Value);
                        }
                        
                        workflowRequestIds = await workflowQuery
                            .Select(br => br.Id)
                            .Distinct()
                            .ToListAsync(cancellationToken);
                    }

                    // Get request IDs where the user is the requester
                    var requesterQuery = _context.BaseRequests.Where(br => !br.IsDeleted && br.RequesterId == currentUserId);
                    
                    // Filter by department only if user has a restricted role
                    if (shouldFilterByDepartment)
                    {
                        requesterQuery = requesterQuery.Where(br => br.DepartmentId == userDepartmentId.Value);
                    }
                    
                    var requesterRequestIds = await requesterQuery
                        .Select(br => br.Id)
                        .ToListAsync(cancellationToken);

                    // Combine both lists and remove duplicates
                    allowedRequestIds = workflowRequestIds
                        .Union(requesterRequestIds)
                        .Distinct()
                        .ToList();
                }

                if (!allowedRequestIds.Any())
                    return APIOperationResponse<List<BaseRequestDto>>.Success(new List<BaseRequestDto>());

                // Get BaseRequests that the user has permission to approve
                var baseRequests = await _context.BaseRequests
                    .Include(br => br.Requester)
                    .Include(br => br.Department)
                    .Include(br => br.RequestPurpose)
                    .Where(br => !br.IsDeleted && allowedRequestIds.Contains(br.Id))
                    .Select(br => new BaseRequestDto
                    {
                        Id = br.Id,
                        RequestNo = br.RequestNo,
                        RequestType = br.RequestType,
                        Reason = br.Reason,
                        Priority = br.Priority,
                        Status = br.Status,
                        RequestDate = br.CreationDate,
                        Notes = br.Notes,
                        DepartmentId = br.DepartmentId,
                        RequesterId = br.RequesterId,
                        RequestPurposeId = br.RequestPurposeId,
                        DepartmentName = br.Department != null ? br.Department.NameEn : null,
                        DepartmentNameAr = br.Department != null ? br.Department.NameAr : null,
                        DepartmentNameEn = br.Department != null ? br.Department.NameEn : null,
                        RequesterName = br.Requester != null ? (br.Requester.FullNameEN ?? br.Requester.FullNameAR ?? br.Requester.UserName) : null,
                        RequesterNameEn = br.Requester != null ? br.Requester.FullNameEN : null,
                        RequesterNameAr = br.Requester != null ? br.Requester.FullNameAR : null,
                        RequesterUserName = br.Requester != null ? br.Requester.UserName : null,
                        RequestPurposeName = br.RequestPurpose != null ? br.RequestPurpose.NameEn : null,
                        RequestPurposeNameAr = br.RequestPurpose != null ? br.RequestPurpose.NameAr : null,
                        RequestPurposeNameEn = br.RequestPurpose != null ? br.RequestPurpose.NameEn : null
                    })
                    .ToListAsync(cancellationToken);

                await EnrichOrderSupplyDatesAsync(baseRequests, cancellationToken);

                // Get all approval history for these requests
                var approvalHistoryData = await (from log in _context.WorkflowStepApprovalLog
                                                join was in _context.WorkflowApprovalSteps
                                                    on log.WorkflowApprovalStepId equals was.Id
                                                join wfs in _context.WorkflowSteps
                                                    on log.WorkflowStepId equals wfs.Id into wfsJoin
                                                from wfs in wfsJoin.DefaultIfEmpty()
                                                join role in _context.Roles
                                                    on wfs.ApplicationRoleId equals role.Id into roleJoin
                                                from role in roleJoin.DefaultIfEmpty()
                                                join actedRole in _context.Roles
                                                    on log.ChangedByRoleId equals actedRole.Id into actedRoleJoin
                                                from actedRole in actedRoleJoin.DefaultIfEmpty()
                                                from user in _context.Users.Where(u => u.Id == log.ChangedBy || u.UserName == log.ChangedBy).DefaultIfEmpty()
                                                where allowedRequestIds.Contains(was.TargetRequestId)
                                                select new
                                                {
                                                    RequestId = was.TargetRequestId,
                                                    History = new ApprovalHistoryDto
                                                    {
                                                        Id = log.Id,
                                                        WorkflowApprovalStepId = log.WorkflowApprovalStepId,
                                                        WorkflowStepId = log.WorkflowStepId,
                                                        OldRequestStatus = log.OldRequestStatus,
                                                        NewRequestStatus = log.NewRequestStatus,
                                                        Comments = log.Comments,
                                                        ChangedBy = log.ChangedBy,
                                                        ApproverNameEn = user != null ? user.FullNameEN : null,
                                                        ApproverNameAr = user != null ? user.FullNameAR : null,
                                                        ChangedAt = log.ChangedAt,
                                                        StepOrder = wfs != null ? wfs.StepOrder : null,
                                                        ApplicationRoleId = wfs != null ? wfs.ApplicationRoleId : null,
                                                        ApplicationRoleName = role != null ? role.Name : null,
                                                        ApplicationRoleNameAr = role != null ? role.NameAr : null,
                                                        RequireHigherApproval = wfs != null ? wfs.RequireHigherApproval : false,
                                                        HigherApprovalRoleId = wfs != null ? wfs.HigherApprovalRoleId : null,
                                                        CanReturn = wfs != null ? wfs.CanReturn : false,
                                                        IsDelegation = was.IsDelegation,
                                                        ChangedByRoleId = log.ChangedByRoleId,
                                                        ChangedByRoleName = actedRole != null ? actedRole.Name : null,
                                                        ChangedByRoleNameAr = actedRole != null ? actedRole.NameAr : null,
                                                        Files = new List<FileUploadDto>()
                                                    }
                                                })
                                                .OrderBy(h => h.History.ChangedAt)
                                                .ToListAsync(cancellationToken);


                // Group approval history by request ID
                var historyByRequestId = approvalHistoryData
                    .GroupBy(h => h.RequestId)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.History).ToList());

                // Get all active workflows with their steps, grouped by WorkflowType
                var workflowsByType = await _context.Workflows
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(ws => ws.ApplicationRole)
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(ws => ws.HigherApprovalRole)
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(ws => ws.ParallelRoles)
                            .ThenInclude(pr => pr.Role)
                    .Where(w => w.IsActive && !w.IsDeleted)
                    .GroupBy(w => w.WorkflowType)
                    .ToDictionaryAsync(g => g.Key, g => g.FirstOrDefault(), cancellationToken);

                // Get pending workflow approval steps for these requests
                var pendingStepsData = await (from was in _context.WorkflowApprovalSteps
                                             join wfs in _context.WorkflowSteps
                                                 .Include(ws => ws.ApplicationRole)
                                                 .Include(ws => ws.HigherApprovalRole)
                                                 on was.WorkflowStepId equals wfs.Id
                                             where allowedRequestIds.Contains(was.TargetRequestId) &&
                                                   was.IsCurrent &&
                                                   (was.Status == RequestStatus.New || was.Status == RequestStatus.UnderProcess)
                                             select new
                                             {
                                                 RequestId = was.TargetRequestId,
                                                 was.WorkflowStepId,
                                                 WorkflowApprovalStepId = was.Id,
                                                 was.Status,
                                                 was.Comments,
                                                 was.CreationDate,
                                                 wfs.StepOrder,
                                                 was.ApproverUserId,
                                                 wfs.ApplicationRoleId,
                                                 ApplicationRoleName = wfs.ApplicationRole != null ? wfs.ApplicationRole.Name : null,
                                                 ApplicationRoleNameAr = wfs.ApplicationRole != null ? wfs.ApplicationRole.NameAr : null,
                                                 wfs.RequireHigherApproval,
                                                 wfs.HigherApprovalRoleId,
                                                 HigherApprovalRoleName = wfs.HigherApprovalRole != null ? wfs.HigherApprovalRole.Name : null,
                                                 HigherApprovalRoleNameAr = wfs.HigherApprovalRole != null ? wfs.HigherApprovalRole.NameAr : null,
                                                 wfs.CanReturn
                                             })
                                             .ToListAsync(cancellationToken);

                // Group pending steps by request ID
                var pendingStepsByRequestId = pendingStepsData
                    .GroupBy(p => p.RequestId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                var pendingParallelWorkflowStepIds = pendingStepsData.Select(p => p.WorkflowStepId).Distinct().ToList();
                var pendingParallelRolesByWfStep = await BuildParallelRolesByStepIdsAsync(pendingParallelWorkflowStepIds, cancellationToken);
                var pendingParallelRoleIdsByWfStep = await BuildParallelRoleIdsByStepIdsAsync(pendingParallelWorkflowStepIds, cancellationToken);

                // Get ALL workflow approval steps for these requests (not just pending or logged ones)
                var allWorkflowApprovalSteps = await _context.WorkflowApprovalSteps
                    .Where(was => allowedRequestIds.Contains(was.TargetRequestId))
                    .Select(was => new { was.Id, was.TargetRequestId })
                    .ToListAsync(cancellationToken);

                // Collect all workflow approval step IDs (from all steps, history, and pending steps)
                var allApprovalStepIds = new HashSet<long>();
                
                foreach (var step in allWorkflowApprovalSteps)
                {
                    if (step.Id > 0)
                    {
                        allApprovalStepIds.Add(step.Id);
                    }
                }
                
                foreach (var historyItem in approvalHistoryData)
                {
                    if (historyItem.History.WorkflowApprovalStepId > 0)
                    {
                        allApprovalStepIds.Add(historyItem.History.WorkflowApprovalStepId);
                    }
                }
                
                foreach (var pendingStep in pendingStepsData)
                {
                    if (pendingStep.WorkflowApprovalStepId > 0)
                    {
                        allApprovalStepIds.Add(pendingStep.WorkflowApprovalStepId);
                    }
                }

                // Get files for all approval steps
                var filesByStepId = new Dictionary<long, List<FileUploadDto>>();
                foreach (var stepId in allApprovalStepIds)
                {
                    var filesResult = await _fileUploadService.GetByEntityAsync(FileEntityType.WorkflowApproval, stepId);
                    if (filesResult.Succeeded && filesResult.Data != null)
                    {
                        filesByStepId[stepId] = filesResult.Data;
                    }
                    else
                    {
                        filesByStepId[stepId] = new List<FileUploadDto>();
                    }
                }

                // Get files for orders, returns, and discards
                var requestFilesByRequestId = new Dictionary<long, List<FileUploadDto>>();
                var requestIds = baseRequests
                    .Where(r => r.RequestType == RequestType.Order || r.RequestType == RequestType.Return || r.RequestType == RequestType.Discard)
                    .Select(r => new { r.Id, r.RequestType })
                    .ToList();

                foreach (var requestInfo in requestIds)
                {
                    try
                    {
                        var filesResult = await _fileUploadService.GetByEntityAsync(FileEntityType.Order, requestInfo.Id);
                        if (filesResult.Succeeded && filesResult.Data != null)
                        {
                            requestFilesByRequestId[requestInfo.Id] = filesResult.Data;
                        }
                        else
                        {
                            requestFilesByRequestId[requestInfo.Id] = new List<FileUploadDto>();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error getting files for request. RequestId: {RequestId}, RequestType: {RequestType}", 
                            requestInfo.Id, requestInfo.RequestType);
                        requestFilesByRequestId[requestInfo.Id] = new List<FileUploadDto>();
                    }
                }

                // Assign approval history and merge with all workflow steps for each request
                foreach (var baseRequest in baseRequests)
                {
                    var combinedHistory = new List<ApprovalHistoryDto>();

                    // Assign files to order, return, and discard requests
                    if ((baseRequest.RequestType == RequestType.Order || 
                         baseRequest.RequestType == RequestType.Return || 
                         baseRequest.RequestType == RequestType.Discard) &&
                        requestFilesByRequestId.TryGetValue(baseRequest.Id, out var requestFiles))
                    {
                        baseRequest.Files = requestFiles;
                    }
                    else
                    {
                        baseRequest.Files = new List<FileUploadDto>();
                    }

                    // Get completed approval history and assign files to each step
                    if (historyByRequestId.TryGetValue(baseRequest.Id, out var history))
                    {
                        foreach (var historyItem in history)
                        {
                            // Assign files to this approval step
                            if (historyItem.WorkflowApprovalStepId > 0 && filesByStepId.TryGetValue(historyItem.WorkflowApprovalStepId, out var stepFiles))
                            {
                                historyItem.Files = stepFiles;
                            }
                            else
                            {
                                historyItem.Files = new List<FileUploadDto>();
                            }
                            combinedHistory.Add(historyItem);
                        }
                    }

                    // Do not synthesize pending/future steps when the workflow has ended
                    bool skipPendingAndFutureSteps =
                        baseRequest.Status == RequestStatus.Rejected ||
                        baseRequest.Status == RequestStatus.Cancelled ||
                        baseRequest.Status == RequestStatus.Approved ||
                        baseRequest.Status == RequestStatus.AutoRejected;

                    if (!skipPendingAndFutureSteps && combinedHistory.Any(h =>
                        h.NewRequestStatus == RequestStatus.Rejected ||
                        h.NewRequestStatus == RequestStatus.Cancelled))
                    {
                        skipPendingAndFutureSteps = true;
                    }

                    if (!skipPendingAndFutureSteps)
                    {
                        // Get the workflow for this request type
                        var workflowType = (WorkflowType)baseRequest.RequestType;
                        if (workflowsByType.TryGetValue(workflowType, out var workflow) && workflow != null)
                        {
                            // Get all workflow steps
                            var allWorkflowSteps = workflow.WorkflowSteps.OrderBy(ws => ws.StepOrder).ToList();

                            // Create a set of workflow step IDs that have been completed or are pending
                            var completedOrPendingStepIds = new HashSet<long>();
                            foreach (var h in combinedHistory)
                            {
                                if (h.WorkflowStepId.HasValue)
                                    completedOrPendingStepIds.Add(h.WorkflowStepId.Value);
                            }

                            // Get pending steps for this request and add them
                            if (pendingStepsByRequestId.TryGetValue(baseRequest.Id, out var requestPendingSteps))
                            {
                                // Only add the next pending step (the one with the lowest step order that hasn't been completed)
                                var nextPendingStep = requestPendingSteps
                                    .OrderBy(p => p.StepOrder)
                                    .FirstOrDefault();
                                
                                if (nextPendingStep != null)
                                {
                                    completedOrPendingStepIds.Add(nextPendingStep.WorkflowStepId);
                                    
                                    // Check if this is a higher approval step
                                    bool isHigherApprovalStep = nextPendingStep.RequireHigherApproval &&
                                        combinedHistory.Any(h => 
                                            h.WorkflowStepId == nextPendingStep.WorkflowStepId && 
                                            h.NewRequestStatus == RequestStatus.Approved);
                                    
                                    // Use HigherApprovalRoleName if this is a higher approval step, otherwise use ApplicationRoleName
                                    string roleNameToUse = isHigherApprovalStep && !string.IsNullOrEmpty(nextPendingStep.HigherApprovalRoleName)
                                        ? nextPendingStep.HigherApprovalRoleName
                                        : nextPendingStep.ApplicationRoleName;
                                    
                                    string roleNameArToUse = isHigherApprovalStep && !string.IsNullOrEmpty(nextPendingStep.HigherApprovalRoleNameAr)
                                        ? nextPendingStep.HigherApprovalRoleNameAr
                                        : nextPendingStep.ApplicationRoleNameAr;
                                    
                                    // Calculate IsCurrentUserApprover (including delegation)
                                    bool isCurrentUserApprover = false;

                                    if (_currentUserService.IsSuperAdmin)
                                    {
                                        isCurrentUserApprover = true;
                                    }
                                    else if (!string.IsNullOrEmpty(nextPendingStep.ApproverUserId))
                                    {
                                        // If assigned to specific user, check direct OR delegator
                                        isCurrentUserApprover = nextPendingStep.ApproverUserId == currentUserId || 
                                                               activeDelegatorIds.Contains(nextPendingStep.ApproverUserId);
                                    }
                                    else
                                    {
                                        // Check role requirements (User Roles OR Delegator Roles)
                                        string requiredRoleId = isHigherApprovalStep ? nextPendingStep.HigherApprovalRoleId : nextPendingStep.ApplicationRoleId;
                                        string requiredRoleName = roleNameToUse;

                                        // Check Role ID match
                                        if (!string.IsNullOrEmpty(requiredRoleId))
                                        {
                                            if (userRoleIds.Contains(requiredRoleId) || delegatorRoleIds.Contains(requiredRoleId))
                                            {
                                                isCurrentUserApprover = true;
                                            }
                                        }
                                        
                                        // Check Role Name match (if ID didn't match)
                                        if (!isCurrentUserApprover && !string.IsNullOrEmpty(requiredRoleName))
                                        {
                                             var normalizedRequired = requiredRoleName.ToLower().Replace(" ", "").Replace(".", "").Replace("_", "").Replace("-", "").Replace("(", "").Replace(")", "");
                                             
                                             if (allRelevantRoleNames.Any(r => !string.IsNullOrEmpty(r) && r.ToLower().Replace(" ", "").Replace(".", "").Replace("_", "").Replace("-", "").Replace("(", "").Replace(")", "") == normalizedRequired))
                                             {
                                                 isCurrentUserApprover = true;
                                             }
                                        }

                                        if (!isCurrentUserApprover && !isHigherApprovalStep &&
                                            pendingParallelRoleIdsByWfStep.TryGetValue(nextPendingStep.WorkflowStepId, out var parallelRoleIdsForApprover) &&
                                            parallelRoleIdsForApprover.Any(pid =>
                                                userRoleIds.Contains(pid) || delegatorRoleIds.Contains(pid)))
                                        {
                                            isCurrentUserApprover = true;
                                        }
                                    }

                                    pendingParallelRolesByWfStep.TryGetValue(nextPendingStep.WorkflowStepId, out var parallelRolesForPending);
                                    parallelRolesForPending ??= new List<WorkflowStepParallelRoleDto>();

                                    var pendingStep = new ApprovalHistoryDto
                                    {
                                        Id = nextPendingStep.WorkflowApprovalStepId,
                                        WorkflowApprovalStepId = nextPendingStep.WorkflowApprovalStepId,
                                        WorkflowStepId = nextPendingStep.WorkflowStepId,
                                        OldRequestStatus = nextPendingStep.Status,
                                        NewRequestStatus = nextPendingStep.Status,
                                        Comments = nextPendingStep.Comments,
                                        ChangedBy = null,
                                        ChangedAt = nextPendingStep.CreationDate,
                                        StepOrder = nextPendingStep.StepOrder,
                                        ApplicationRoleId = isHigherApprovalStep ? nextPendingStep.HigherApprovalRoleId : nextPendingStep.ApplicationRoleId,
                                        ApplicationRoleName = roleNameToUse,
                                        ApplicationRoleNameAr = roleNameArToUse,
                                        RequireHigherApproval = nextPendingStep.RequireHigherApproval,
                                        HigherApprovalRoleId = nextPendingStep.HigherApprovalRoleId,
                                        CanReturn = nextPendingStep.CanReturn,
                                        IsPending = true,
                                        IsCurrentUserApprover = isCurrentUserApprover,
                                        EligibleParallelRoles = parallelRolesForPending,
                                        EligibleParallelRoleNamesEn = string.Join(" | ", parallelRolesForPending.Select(x => x.RoleName).Where(x => !string.IsNullOrEmpty(x)).Distinct()),
                                        EligibleParallelRoleNamesAr = string.Join(" | ", parallelRolesForPending.Select(x => x.RoleNameAr).Where(x => !string.IsNullOrEmpty(x)).Distinct())
                                    };
                                    
                                    // Assign files to pending step if any
                                    if (nextPendingStep.WorkflowApprovalStepId > 0 && filesByStepId.TryGetValue(nextPendingStep.WorkflowApprovalStepId, out var pendingStepFiles))
                                    {
                                        pendingStep.Files = pendingStepFiles;
                                    }
                                    else
                                    {
                                        pendingStep.Files = new List<FileUploadDto>();
                                    }
                                    
                                    combinedHistory.Add(pendingStep);
                                }
                            }
                            else
                            {
                                // If no pending steps exist, find the next workflow step that should be started
                                var nextWorkflowStep = allWorkflowSteps
                                    .Where(ws => !completedOrPendingStepIds.Contains(ws.Id))
                                    .OrderBy(ws => ws.StepOrder)
                                    .FirstOrDefault();
                                
                                if (nextWorkflowStep != null)
                                {
                                    var futureStep = new ApprovalHistoryDto
                                    {
                                        Id = 0,
                                        WorkflowApprovalStepId = 0,
                                        WorkflowStepId = nextWorkflowStep.Id,
                                        OldRequestStatus = RequestStatus.New,
                                        NewRequestStatus = RequestStatus.New,
                                        Comments = null,
                                        ChangedBy = null,
                                        ChangedAt = DateTime.MinValue,
                                        StepOrder = nextWorkflowStep.StepOrder,
                                        ApplicationRoleId = nextWorkflowStep.ApplicationRoleId,
                                        ApplicationRoleName = nextWorkflowStep.ApplicationRole != null ? nextWorkflowStep.ApplicationRole.Name : null,
                                        ApplicationRoleNameAr = nextWorkflowStep.ApplicationRole != null ? nextWorkflowStep.ApplicationRole.NameAr : null,
                                        RequireHigherApproval = nextWorkflowStep.RequireHigherApproval,
                                        HigherApprovalRoleId = nextWorkflowStep.HigherApprovalRoleId,
                                        CanReturn = nextWorkflowStep.CanReturn,
                                        IsPending = true,
                                        EligibleParallelRoles = nextWorkflowStep.ParallelRoles != null
                                            ? nextWorkflowStep.ParallelRoles.Select(pr => new WorkflowStepParallelRoleDto
                                            {
                                                Id = pr.Id,
                                                WorkflowStepId = pr.WorkflowStepId,
                                                RoleId = pr.RoleId,
                                                RoleName = pr.Role?.Name ?? pr.RoleId,
                                                RoleNameAr = pr.Role?.NameAr ?? pr.Role?.Name ?? pr.RoleId
                                            }).ToList()
                                            : new List<WorkflowStepParallelRoleDto>(),
                                        EligibleParallelRoleNamesEn = nextWorkflowStep.ParallelRoles != null && nextWorkflowStep.ParallelRoles.Count > 0
                                            ? string.Join(" | ", nextWorkflowStep.ParallelRoles.Select(pr => pr.Role?.Name ?? pr.RoleId).Where(x => !string.IsNullOrEmpty(x)).Distinct())
                                            : null,
                                        EligibleParallelRoleNamesAr = nextWorkflowStep.ParallelRoles != null && nextWorkflowStep.ParallelRoles.Count > 0
                                            ? string.Join(" | ", nextWorkflowStep.ParallelRoles.Select(pr => pr.Role?.NameAr ?? pr.Role?.Name ?? pr.RoleId).Where(x => !string.IsNullOrEmpty(x)).Distinct())
                                            : null,
                                        Files = new List<FileUploadDto>()
                                    };
                                    combinedHistory.Add(futureStep);
                                }
                            }
                        }
                    }

                    // Sort: completed steps first (by step order and date), then pending steps last
                    baseRequest.ApprovalHistory = combinedHistory
                        .OrderBy(h => h.IsPending ? 1 : 0)
                        .ThenBy(h => h.StepOrder ?? int.MaxValue)
                        .ThenBy(h => h.ChangedAt == DateTime.MinValue ? DateTime.MaxValue : h.ChangedAt)                    
                        .ToList();
                }

                return APIOperationResponse<List<BaseRequestDto>>.Success(baseRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all base requests");
                return APIOperationResponse<List<BaseRequestDto>>.ServerError($"Processing failed: {ex.Message}");
            }
        }

        private async Task EnrichOrderSupplyDatesAsync(List<BaseRequestDto> baseRequests, CancellationToken cancellationToken)
        {
            var orderIds = baseRequests.Where(r => r.RequestType == RequestType.Order).Select(r => r.Id).ToList();
            if (orderIds.Count == 0)
                return;

            var map = await _context.Set<Order>().AsNoTracking()
                .Where(o => orderIds.Contains(o.Id))
                .Select(o => new { o.Id, o.SupplyDate })
                .ToDictionaryAsync(x => x.Id, x => x.SupplyDate, cancellationToken);

            foreach (var br in baseRequests)
            {
                if (map.TryGetValue(br.Id, out var sd))
                    br.SupplyDate = sd;
            }
        }

        private async Task<Dictionary<long, List<WorkflowStepParallelRoleDto>>> BuildParallelRolesByStepIdsAsync(List<long> stepIds, CancellationToken cancellationToken)
        {
            if (stepIds == null || stepIds.Count == 0)
                return new Dictionary<long, List<WorkflowStepParallelRoleDto>>();
            var distinctIds = stepIds.Distinct().ToList();
            var rows = await _context.WorkflowStepParallelRoles
                .Where(pr => distinctIds.Contains(pr.WorkflowStepId))
                .Join(_context.Roles, pr => pr.RoleId, r => r.Id, (pr, r) => new WorkflowStepParallelRoleDto
                {
                    Id = pr.Id,
                    WorkflowStepId = pr.WorkflowStepId,
                    RoleId = pr.RoleId,
                    RoleName = r.Name ?? r.Id,
                    RoleNameAr = r.NameAr ?? r.Name
                })
                .ToListAsync(cancellationToken);
            return rows
                .GroupBy(x => x.WorkflowStepId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        private async Task<Dictionary<long, List<string>>> BuildParallelRoleIdsByStepIdsAsync(List<long> stepIds, CancellationToken cancellationToken)
        {
            if (stepIds == null || stepIds.Count == 0)
                return new Dictionary<long, List<string>>();
            var distinctIds = stepIds.Distinct().ToList();
            return await _context.WorkflowStepParallelRoles
                .Where(pr => distinctIds.Contains(pr.WorkflowStepId))
                .GroupBy(pr => pr.WorkflowStepId)
                .ToDictionaryAsync(g => g.Key, g => g.Select(x => x.RoleId).ToList(), cancellationToken);
        }
    }
}
