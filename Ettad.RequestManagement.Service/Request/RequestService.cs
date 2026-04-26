using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ettad.Application.Common.Interfaces;
using Ettad.Data.Entities;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.RequestManagement.Service.Common.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Ettad.User.Services.Interfaces;
using System.Linq;
using Ettad.CrossCutting.Comman.Models;
using Ettad.Data.Constants;

namespace Ettad.RequestManagement.Service.Request
{
    public class RequestService : IRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserDelegationService _userDelegationService;
        private readonly IMapper _mapper;

        public RequestService(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            IUserDelegationService userDelegationService,
            IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userDelegationService = userDelegationService;
            _mapper = mapper;
        }

        private IQueryable<BaseRequest> PrepareBaseQuery(IQueryable<BaseRequest> query, FilterData filter)
        {
            query = query
                .Include(r => r.Department)
                .Include(r => r.Requester)
                    .ThenInclude(u => u.Rank)
                .Include(r => r.RequestPurpose)
                .Include(r => r.RequestItems)
                    .ThenInclude(ri => ri.Item);

            if (filter != null)
            {
                // Global Search logic (when Field and Filters are empty)
                if (!string.IsNullOrEmpty(filter.Value) && string.IsNullOrEmpty(filter.Field) && (filter.Filters == null || !filter.Filters.Any()))
                {
                    var searchTerm = filter.Value.ToLower();
                    query = query.Where(r => 
                        (r.RequestNo != null && r.RequestNo.Contains(searchTerm)) || 
                        (r.Requester != null && (r.Requester.UserName.Contains(searchTerm) || r.Requester.FullNameEN.Contains(searchTerm) || r.Requester.FullNameAR.Contains(searchTerm))) || 
                        (r.Department != null && (r.Department.NameAr.Contains(searchTerm) || r.Department.NameEn.Contains(searchTerm))));
                }
            }

            // Default sorting for stable pagination (applied ONLY if no client sort provided)
            // Note: PaginatedList.CreateAsyncForTableBinding will apply the client-side sort via ToFilterView
            if (filter == null || string.IsNullOrEmpty(filter.sortField))
            {
                query = query.OrderByDescending(r => r.CreationDate);
            }

            return query.AsNoTracking();
        }

        public async Task<APIOperationResponse<List<BaseRequestDto>>> GetAllRequestsAsync(RequestStatus? status = null, RequestType? requestType = null)
        {
            var query = _context.BaseRequests.AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(r => r.Status == status.Value);
            }

            if (requestType.HasValue)
            {
                query = query.Where(r => r.RequestType == requestType.Value);
            }

            var requests = await query
                .Include(r => r.Department)
                .Include(r => r.Requester)
                    .ThenInclude(u => u.Rank)
                .Include(r => r.RequestPurpose)
                .Include(r => r.RequestItems)
                    .ThenInclude(ri => ri.Item)
                .ToListAsync();

            var requestDtos = _mapper.Map<List<BaseRequestDto>>(requests);

            return APIOperationResponse<List<BaseRequestDto>>.Success(requestDtos);
        }

        public async Task<APIOperationResponse<List<BaseRequestDto>>> GetRequestsByDepartmentAsync(long departmentId, RequestStatus? status = null, RequestType? requestType = null)
        {
            var query = _context.BaseRequests
                .Where(r => r.DepartmentId == departmentId);

            if (status.HasValue)
            {
                query = query.Where(r => r.Status == status.Value);
            }

            if (requestType.HasValue)
            {
                query = query.Where(r => r.RequestType == requestType.Value);
            }

            var requests = await query
                .Include(r => r.Department)
                .Include(r => r.Requester)
                    .ThenInclude(u => u.Rank)
                .Include(r => r.RequestPurpose)
                .Include(r => r.RequestItems)
                    .ThenInclude(ri => ri.Item)
                .ToListAsync();

            var requestDtos = _mapper.Map<List<BaseRequestDto>>(requests);

            return APIOperationResponse<List<BaseRequestDto>>.Success(requestDtos);
        }

        public async Task<APIOperationResponse<List<BaseRequestDto>>> GetRequestsByRequesterAsync(string requesterId, RequestStatus? status = null, RequestType? requestType = null)
        {
            var query = _context.BaseRequests
                .Where(r => r.RequesterId == requesterId);

            if (status.HasValue)
            {
                query = query.Where(r => r.Status == status.Value);
            }

            if (requestType.HasValue)
            {
                query = query.Where(r => r.RequestType == requestType.Value);
            }

            var requests = await query
                .Include(r => r.Department)
                .Include(r => r.Requester)
                    .ThenInclude(u => u.Rank)
                .Include(r => r.RequestPurpose)
                .Include(r => r.RequestItems)
                    .ThenInclude(ri => ri.Item)
                .ToListAsync();

            var requestDtos = _mapper.Map<List<BaseRequestDto>>(requests);

            return APIOperationResponse<List<BaseRequestDto>>.Success(requestDtos);
        }

        public async Task<APIOperationResponse<List<BaseRequestDto>>> GetRequestsByStatusAndTypeAsync(RequestStatus? status, RequestType? requestType)
        {
            var query = _context.BaseRequests.AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(r => r.Status == status.Value);
            }

            if (requestType.HasValue)
            {
                query = query.Where(r => r.RequestType == requestType.Value);
            }

            var requests = await query
                .Include(r => r.Department)
                .Include(r => r.Requester)
                    .ThenInclude(u => u.Rank)
                .Include(r => r.RequestPurpose)
                .Include(r => r.RequestItems)
                    .ThenInclude(ri => ri.Item)
                .ToListAsync();

            var requestDtos = _mapper.Map<List<BaseRequestDto>>(requests);

            return APIOperationResponse<List<BaseRequestDto>>.Success(requestDtos);
        }

        public async Task<APIOperationResponse<List<BaseRequestDto>>> GetUserActionRequestsAsync(RequestStatus? status = null, RequestType? requestType = null)
        {
            var userId = _currentUserService.UserId;
            var userRoles = _currentUserService.Roles ?? new List<string>();
            var userDepartmentId = _currentUserService.DepartmentId;

            // if current user is super admin return all
            if (_currentUserService.IsSuperAdmin)
                return await GetAllRequestsAsync(status, requestType);

            var query = _context.BaseRequests.AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(r => r.Status == status.Value);
            }

            if (requestType.HasValue)
            {
                query = query.Where(r => r.RequestType == requestType.Value);
            }

            // Check if user has "Order Requester" role - if so, return ONLY their requests
            if (userRoles.Contains(WorkflowRoleNames.OrderRequester))
            {
                query = query.Where(r => r.RequesterId == userId);
                
                var requesterRequests = await query
                    .Include(r => r.Department)
                    .Include(r => r.Requester)
                        .ThenInclude(u => u.Rank)
                    .Include(r => r.RequestPurpose)
                    .Include(r => r.RequestItems)
                        .ThenInclude(ri => ri.Item)
                    .ToListAsync();

                var requestDtos = _mapper.Map<List<BaseRequestDto>>(requesterRequests);

                return APIOperationResponse<List<BaseRequestDto>>.Success(requestDtos);
            }

            // Supply Officer / RequestingEntityCommander: same workflow visibility as other approvers, limited to their department when DepartmentId is set
            var restrictedRoles = new List<string>
            {
                WorkflowRoleNames.SupplyOfficer,
                WorkflowRoleNames.RequestingEntityCommander
            };

            bool isRestrictedRole = userRoles.Any(r => restrictedRoles.Contains(r));

            if (isRestrictedRole && userDepartmentId.HasValue)
            {
                query = query.Where(r => r.DepartmentId == userDepartmentId.Value);
            }

            var activeDelegatorIds = await _userDelegationService.GetActiveDelegatorsForUserAsync(userId, DelegationScope.WorkflowApproval);
            var delegatorRoleNames = new List<string>();
            if (activeDelegatorIds.Any())
            {
                delegatorRoleNames = await _context.Set<IdentityUserRole<string>>()
                    .Where(ur => activeDelegatorIds.Contains(ur.UserId))
                    .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                    .ToListAsync();
            }

            query = query.Where(r => _context.WorkflowApprovalSteps.Any(was =>
                was.TargetRequestId == r.Id && (
                    (was.ApproverUserId == userId || activeDelegatorIds.Contains(was.ApproverUserId)) ||
                    (userRoles.Contains(was.WorkflowStep.ApplicationRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.ApplicationRole.Name)) ||
                    (was.WorkflowStep.HigherApprovalRole != null && (userRoles.Contains(was.WorkflowStep.HigherApprovalRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.HigherApprovalRole.Name))) ||
                    was.WorkflowStep.ParallelRoles.Any(pr => userRoles.Contains(pr.Role.Name) || delegatorRoleNames.Contains(pr.Role.Name))
                )
            ));

            var requests = await query
                .Include(r => r.Department)
                .Include(r => r.Requester)
                    .ThenInclude(u => u.Rank)
                .Include(r => r.RequestPurpose)
                .Include(r => r.RequestItems)
                    .ThenInclude(ri => ri.Item)
                .ToListAsync();

            // Fetch IsMyTurn status only for the requested IDs to optimize database traffic
            var fetchedRequestIds = requests.Select(r => (int)r.Id).ToList();

            var myTurnSet = (await _context.WorkflowApprovalSteps
                .Where(was => was.IsCurrent && fetchedRequestIds.Contains(was.TargetRequestId) && (
                    (was.ApproverUserId == userId || activeDelegatorIds.Contains(was.ApproverUserId)) ||
                    (was.ApproverUserId == null && (
                        (userRoles.Contains(was.WorkflowStep.ApplicationRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.ApplicationRole.Name)) ||
                        (was.WorkflowStep.HigherApprovalRole != null && (userRoles.Contains(was.WorkflowStep.HigherApprovalRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.HigherApprovalRole.Name))) ||
                        was.WorkflowStep.ParallelRoles.Any(pr => userRoles.Contains(pr.Role.Name) || delegatorRoleNames.Contains(pr.Role.Name))
                    ))
                ))
                .Select(was => (long)was.TargetRequestId)
                .Distinct()
                .ToListAsync())
                .ToHashSet();

            var dtos = _mapper.Map<List<BaseRequestDto>>(requests);

            foreach (var dto in dtos)
            {
                dto.IsMyTurn = myTurnSet.Contains(dto.Id);
            }
            return APIOperationResponse<List<BaseRequestDto>>.Success(dtos);
        }

        public async Task<APIOperationResponse<PaginatedList<BaseRequestDto>>> GetAllPaginatedAsync(PagedListRequest request)
        {
            var query = _context.BaseRequests.AsQueryable();
            query = PrepareBaseQuery(query, request.Filter);
            
            var paginatedRequests = await PaginatedList<BaseRequest>.CreateAsyncForTableBinding(query, request);
            var dtos = _mapper.Map<List<BaseRequestDto>>(paginatedRequests.Items);
            
            var result = new PaginatedList<BaseRequestDto>(dtos, paginatedRequests.TotalCount, paginatedRequests.PageIndex, request.PageSize);
            return APIOperationResponse<PaginatedList<BaseRequestDto>>.Success(result);
        }

        public async Task<APIOperationResponse<PaginatedList<BaseRequestDto>>> GetUserActionRequestsPaginatedAsync(PagedListRequest request)
        {
            var userId = _currentUserService.UserId;
            var userRoles = _currentUserService.Roles ?? new List<string>();
            var userDepartmentId = _currentUserService.DepartmentId;

            IQueryable<BaseRequest> query = _context.BaseRequests.AsQueryable();

            // if current user is super admin return all
            if (!_currentUserService.IsSuperAdmin)
            {
                // Check if user has "Order Requester" role - if so, return ONLY their requests
                if (userRoles.Contains(WorkflowRoleNames.OrderRequester))
                {
                    query = query.Where(r => r.RequesterId == userId);
                }
                else
                {
                    // Supply Officer / RequestingEntityCommander: same workflow visibility as other approvers, limited to their department when DepartmentId is set
                    var restrictedRoles = new List<string>
                    {
                        WorkflowRoleNames.SupplyOfficer,
                        WorkflowRoleNames.RequestingEntityCommander
                    };

                    bool isRestrictedRole = userRoles.Any(r => restrictedRoles.Contains(r));

                    if (isRestrictedRole && userDepartmentId.HasValue)
                    {
                        query = query.Where(r => r.DepartmentId == userDepartmentId.Value);
                    }

                    var activeDelegatorIds = await _userDelegationService.GetActiveDelegatorsForUserAsync(userId, DelegationScope.WorkflowApproval);
                    var delegatorRoleNames = new List<string>();
                    if (activeDelegatorIds.Any())
                    {
                        delegatorRoleNames = await _context.Set<IdentityUserRole<string>>()
                            .Where(ur => activeDelegatorIds.Contains(ur.UserId))
                            .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                            .ToListAsync();
                    }

                    bool filterByMyTurn = false;
                    if (request.Filter != null)
                    {
                        var myTurnFilter = request.Filter.Filters?.FirstOrDefault(f => f.Field == "IsMyTurn") ??
                                         (request.Filter.Field == "IsMyTurn" ? request.Filter : null);

                        if (myTurnFilter != null && myTurnFilter.Value?.ToLower() == "true")
                        {
                            filterByMyTurn = true;
                        }

                        if (request.Filter.Filters != null)
                        {
                            request.Filter.Filters = request.Filter.Filters.Where(f => f.Field != "IsMyTurn").ToList();
                        }
                        if (request.Filter.Field == "IsMyTurn")
                        {
                            request.Filter.Field = null;
                            request.Filter.Operator = null;
                            request.Filter.Value = null;
                        }
                    }

                    if (filterByMyTurn)
                    {
                        query = query.Where(r => _context.WorkflowApprovalSteps.Any(was =>
                            was.TargetRequestId == r.Id && was.IsCurrent && (
                                (was.ApproverUserId == userId || activeDelegatorIds.Contains(was.ApproverUserId)) ||
                                (was.ApproverUserId == null && (
                                    (userRoles.Contains(was.WorkflowStep.ApplicationRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.ApplicationRole.Name)) ||
                                    (was.WorkflowStep.HigherApprovalRole != null && (userRoles.Contains(was.WorkflowStep.HigherApprovalRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.HigherApprovalRole.Name))) ||
                                    was.WorkflowStep.ParallelRoles.Any(pr => userRoles.Contains(pr.Role.Name) || delegatorRoleNames.Contains(pr.Role.Name))
                                ))
                            )
                        ));
                    }
                    else
                    {
                        query = query.Where(r => _context.WorkflowApprovalSteps.Any(was =>
                            was.TargetRequestId == r.Id && (
                                (was.ApproverUserId == userId || activeDelegatorIds.Contains(was.ApproverUserId)) ||
                                (userRoles.Contains(was.WorkflowStep.ApplicationRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.ApplicationRole.Name)) ||
                                (was.WorkflowStep.HigherApprovalRole != null && (userRoles.Contains(was.WorkflowStep.HigherApprovalRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.HigherApprovalRole.Name))) ||
                                was.WorkflowStep.ParallelRoles.Any(pr => userRoles.Contains(pr.Role.Name) || delegatorRoleNames.Contains(pr.Role.Name))
                            )
                        ));
                    }
                }
            }

            // Apply base query behavior (Includes, Search, and Sorting)
            query = PrepareBaseQuery(query, request.Filter);

            var paginatedRequests = await PaginatedList<BaseRequest>.CreateAsyncForTableBinding(query, request);
            var dtos = _mapper.Map<List<BaseRequestDto>>(paginatedRequests.Items);

            // Fetch IsMyTurn status
            if (dtos.Any())
            {
                var activeDelegatorIds = await _userDelegationService.GetActiveDelegatorsForUserAsync(userId, DelegationScope.WorkflowApproval);
                var delegatorRoleNames = new List<string>();
                if (activeDelegatorIds.Any())
                {
                    delegatorRoleNames = await _context.Set<IdentityUserRole<string>>()
                        .Where(ur => activeDelegatorIds.Contains(ur.UserId))
                        .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                        .ToListAsync();
                }

                var fetchedRequestIds = dtos.Select(r => (int)r.Id).ToList();
                var myTurnSet = (await _context.WorkflowApprovalSteps
                    .Where(was => was.IsCurrent && fetchedRequestIds.Contains(was.TargetRequestId) && (
                        (was.ApproverUserId == userId || activeDelegatorIds.Contains(was.ApproverUserId)) || 
                        (was.ApproverUserId == null && (
                            (userRoles.Contains(was.WorkflowStep.ApplicationRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.ApplicationRole.Name)) || 
                            (was.WorkflowStep.HigherApprovalRole != null && (userRoles.Contains(was.WorkflowStep.HigherApprovalRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.HigherApprovalRole.Name))) ||
                            was.WorkflowStep.ParallelRoles.Any(pr => userRoles.Contains(pr.Role.Name) || delegatorRoleNames.Contains(pr.Role.Name))
                        ))
                    ))
                    .Select(was => (long)was.TargetRequestId)
                    .Distinct()
                    .ToListAsync())
                    .ToHashSet();

                foreach (var dto in dtos)
                {
                    dto.IsMyTurn = myTurnSet.Contains(dto.Id);
                }
            }

            var result = new PaginatedList<BaseRequestDto>(dtos, paginatedRequests.TotalCount, paginatedRequests.PageIndex, request.PageSize);
            return APIOperationResponse<PaginatedList<BaseRequestDto>>.Success(result);
        }
    }
}
