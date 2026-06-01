using AutoMapper;
using Ettad.Application.Common.Interfaces;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.Data.Entities;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.Data.Interfaces.Repositories;
using Ettad.RequestManagement.Service.Common.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ettad.User.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Ettad.CrossCutting.Comman.Models;
using Ettad.Data.Constants;

namespace Ettad.RequestManagement.Service.Request
{
    public class RequestService : IRequestService
    {
        private static readonly string[] BaseRequestListIncludes =
        [
            nameof(BaseRequest.Department),
            nameof(BaseRequest.Requester),
            $"{nameof(BaseRequest.Requester)}.{nameof(ApplicationUser.Rank)}",
            nameof(BaseRequest.RequestPurpose),
            nameof(BaseRequest.RequestItems),
            $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}",
        ];

        private static readonly string[] WorkflowApprovalStepVisibilityIncludes =
        [
            nameof(WorkflowApprovalStep.WorkflowStep),
            $"{nameof(WorkflowApprovalStep.WorkflowStep)}.{nameof(WorkflowStep.ApplicationRole)}",
            $"{nameof(WorkflowApprovalStep.WorkflowStep)}.{nameof(WorkflowStep.HigherApprovalRole)}",
            $"{nameof(WorkflowApprovalStep.WorkflowStep)}.{nameof(WorkflowStep.ParallelRoles)}",
            $"{nameof(WorkflowApprovalStep.WorkflowStep)}.{nameof(WorkflowStep.ParallelRoles)}.{nameof(WorkflowStepParallelRole.Role)}",
        ];

        private readonly ICrossCuttingRepository<BaseRequest> _baseRequestRepository;
        private readonly ICrossCuttingRepository<WorkflowApprovalStep> _workflowApprovalStepRepository;
        private readonly ICrossCuttingRepository<ApplicationRole> _roleRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserDelegationService _userDelegationService;
        private readonly IMapper _mapper;

        public RequestService(
            ICrossCuttingRepository<BaseRequest> baseRequestRepository,
            ICrossCuttingRepository<WorkflowApprovalStep> workflowApprovalStepRepository,
            ICrossCuttingRepository<ApplicationRole> roleRepository,
            ICurrentUserService currentUserService,
            IUserDelegationService userDelegationService,
            IMapper mapper)
        {
            _baseRequestRepository = baseRequestRepository;
            _workflowApprovalStepRepository = workflowApprovalStepRepository;
            _roleRepository = roleRepository;
            _currentUserService = currentUserService;
            _userDelegationService = userDelegationService;
            _mapper = mapper;
        }

        private IQueryable<WorkflowApprovalStep> WorkflowStepsWithNavigations() =>
            _workflowApprovalStepRepository.Find(_ => true, includeSoftDeleted: false, WorkflowApprovalStepVisibilityIncludes);

        private async Task<List<string>> GetDelegatorRoleNamesAsync(IReadOnlyCollection<string> delegatorRoleIds)
        {
            if (delegatorRoleIds == null || delegatorRoleIds.Count == 0)
                return new List<string>();

            // Map the captured delegated role ids (the role each delegator was logged in with at
            // delegation creation) to their role names, rather than expanding all delegator roles.
            return await _roleRepository
                .Find(r => delegatorRoleIds.Contains(r.Id))
                .Where(r => r.Name != null)
                .Select(r => r.Name!)
                .Distinct()
                .ToListAsync();
        }

        private IQueryable<BaseRequest> PrepareBaseQuery(IQueryable<BaseRequest> query, FilterData filter, bool applyDefaultSort = true)
        {
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
            if (applyDefaultSort && (filter == null || string.IsNullOrEmpty(filter.sortField)))
            {
                query = query
                    .OrderByDescending(r => r.Priority)
                    .ThenByDescending(r => r.CreationDate);
            }

            return query;
        }

        private static IQueryable<BaseRequest> OrderByActionRequiredFirst(
            IQueryable<BaseRequest> query,
            IQueryable<WorkflowApprovalStep> stepsQuery,
            string userId,
            IReadOnlyList<string> activeDelegatorIds,
            IReadOnlyList<string> userRoles,
            IReadOnlyList<string> delegatorRoleNames)
        {
            return query
                .OrderByDescending(r => stepsQuery.Any(was =>
                    was.TargetRequestId == r.Id && was.IsCurrent && (
                        (was.ApproverUserId == userId || activeDelegatorIds.Contains(was.ApproverUserId)) ||
                        (was.ApproverUserId == null && (
                            (userRoles.Contains(was.WorkflowStep.ApplicationRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.ApplicationRole.Name)) ||
                            (was.WorkflowStep.HigherApprovalRole != null && (userRoles.Contains(was.WorkflowStep.HigherApprovalRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.HigherApprovalRole.Name))) ||
                            was.WorkflowStep.ParallelRoles.Any(pr => userRoles.Contains(pr.Role.Name) || delegatorRoleNames.Contains(pr.Role.Name))
                        ))
                    )
                ))
                .ThenByDescending(r => r.Priority)
                .ThenByDescending(r => r.CreationDate);
        }

        private static IQueryable<BaseRequest> ApplyUserActionsCompositeSort(
            IQueryable<BaseRequest> query,
            FilterData filter)
        {
            if (string.IsNullOrEmpty(filter.sortField))
            {
                return query;
            }

            // Match FilterProvider: sortDirection == 1 → ascending
            bool descending = filter.sortDirection != 1;
            var field = filter.sortField;

            if (string.Equals(field, "Priority", StringComparison.OrdinalIgnoreCase))
            {
                filter.sortField = null;
                filter.sortDirection = 0;
                return descending
                    ? query.OrderByDescending(r => r.Priority).ThenByDescending(r => r.CreationDate)
                    : query.OrderBy(r => r.Priority).ThenByDescending(r => r.CreationDate);
            }

            if (string.Equals(field, "CreationDate", StringComparison.OrdinalIgnoreCase))
            {
                filter.sortField = null;
                filter.sortDirection = 0;
                return descending
                    ? query.OrderByDescending(r => r.CreationDate).ThenByDescending(r => r.Priority)
                    : query.OrderBy(r => r.CreationDate).ThenByDescending(r => r.Priority);
            }

            return query;
        }

        public async Task<APIOperationResponse<List<BaseRequestDto>>> GetAllRequestsAsync(RequestStatus? status = null, RequestType? requestType = null)
        {
            var requests = await _baseRequestRepository
                .Find(
                    r =>
                        (!status.HasValue || r.Status == status.Value) &&
                        (!requestType.HasValue || r.RequestType == requestType.Value),
                    false,
                    BaseRequestListIncludes)
                .ToListAsync();

            var requestDtos = _mapper.Map<List<BaseRequestDto>>(requests);

            return APIOperationResponse<List<BaseRequestDto>>.Success(requestDtos);
        }

        public async Task<APIOperationResponse<List<BaseRequestDto>>> GetRequestsByDepartmentAsync(long departmentId, RequestStatus? status = null, RequestType? requestType = null)
        {
            var requests = await _baseRequestRepository
                .Find(
                    r =>
                        r.DepartmentId == departmentId &&
                        (!status.HasValue || r.Status == status.Value) &&
                        (!requestType.HasValue || r.RequestType == requestType.Value),
                    false,
                    BaseRequestListIncludes)
                .ToListAsync();

            var requestDtos = _mapper.Map<List<BaseRequestDto>>(requests);

            return APIOperationResponse<List<BaseRequestDto>>.Success(requestDtos);
        }

        public async Task<APIOperationResponse<List<BaseRequestDto>>> GetRequestsByRequesterAsync(string requesterId, RequestStatus? status = null, RequestType? requestType = null)
        {
            var requests = await _baseRequestRepository
                .Find(
                    r =>
                        r.RequesterId == requesterId &&
                        (!status.HasValue || r.Status == status.Value) &&
                        (!requestType.HasValue || r.RequestType == requestType.Value),
                    false,
                    BaseRequestListIncludes)
                .ToListAsync();

            var requestDtos = _mapper.Map<List<BaseRequestDto>>(requests);

            return APIOperationResponse<List<BaseRequestDto>>.Success(requestDtos);
        }

        public async Task<APIOperationResponse<List<BaseRequestDto>>> GetRequestsByStatusAndTypeAsync(RequestStatus? status, RequestType? requestType)
        {
            var requests = await _baseRequestRepository
                .Find(
                    r =>
                        (!status.HasValue || r.Status == status.Value) &&
                        (!requestType.HasValue || r.RequestType == requestType.Value),
                    false,
                    BaseRequestListIncludes)
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

            var query = _baseRequestRepository.Find(
                r =>
                    (!status.HasValue || r.Status == status.Value) &&
                    (!requestType.HasValue || r.RequestType == requestType.Value),
                false,
                BaseRequestListIncludes);

            // Check if user has "Order Requester" role - if so, return ONLY their requests
            if (userRoles.Contains(WorkflowRoleNames.OrderRequester))
            {
                var requesterRequests = await query
                    .Where(r => r.RequesterId == userId)
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

            var activeDelegations = await _userDelegationService.GetActiveDelegationsForUserAsync(userId);
            var activeDelegatorIds = activeDelegations.Select(d => d.DelegatorUserId).Distinct().ToList();
            var delegatorRoleIds = activeDelegations.Select(d => d.DelegatorRoleId).Where(id => !string.IsNullOrEmpty(id)).Distinct().ToList();
            var delegatorRoleNames = await GetDelegatorRoleNamesAsync(delegatorRoleIds);

            var stepsQuery = WorkflowStepsWithNavigations();
            query = query.Where(r => stepsQuery.Any(was =>
                was.TargetRequestId == r.Id && (
                    (was.ApproverUserId == userId || activeDelegatorIds.Contains(was.ApproverUserId)) ||
                    (userRoles.Contains(was.WorkflowStep.ApplicationRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.ApplicationRole.Name)) ||
                    (was.WorkflowStep.HigherApprovalRole != null && (userRoles.Contains(was.WorkflowStep.HigherApprovalRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.HigherApprovalRole.Name))) ||
                    was.WorkflowStep.ParallelRoles.Any(pr => userRoles.Contains(pr.Role.Name) || delegatorRoleNames.Contains(pr.Role.Name))
                )
            ));

            var requests = await query.ToListAsync();

            // Fetch IsMyTurn status only for the requested IDs to optimize database traffic
            var fetchedRequestIds = requests.Select(r => r.Id).ToList();

            var myTurnSet = (await WorkflowStepsWithNavigations()
                .Where(was => was.IsCurrent && fetchedRequestIds.Contains(was.TargetRequestId) && (
                    (was.ApproverUserId == userId || activeDelegatorIds.Contains(was.ApproverUserId)) ||
                    (was.ApproverUserId == null && (
                        (userRoles.Contains(was.WorkflowStep.ApplicationRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.ApplicationRole.Name)) ||
                        (was.WorkflowStep.HigherApprovalRole != null && (userRoles.Contains(was.WorkflowStep.HigherApprovalRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.HigherApprovalRole.Name))) ||
                        was.WorkflowStep.ParallelRoles.Any(pr => userRoles.Contains(pr.Role.Name) || delegatorRoleNames.Contains(pr.Role.Name))
                    ))
                ))
                .Select(was => was.TargetRequestId)
                .Distinct()
                .ToListAsync())
                .ToHashSet();

            var dtos = _mapper.Map<List<BaseRequestDto>>(requests);

            foreach (var dto in dtos)
            {
                dto.IsMyTurn = myTurnSet.Contains(dto.Id);
            }

            dtos = dtos
                .OrderByDescending(d => d.IsMyTurn)
                .ThenByDescending(d => d.Priority)
                .ThenByDescending(d => d.CreationDate)
                .ToList();

            return APIOperationResponse<List<BaseRequestDto>>.Success(dtos);
        }

        public async Task<APIOperationResponse<PaginatedList<BaseRequestDto>>> GetAllPaginatedAsync(PagedListRequest request)
        {
            var query = _baseRequestRepository.Find(_ => true, false, BaseRequestListIncludes);
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

            var activeDelegations = await _userDelegationService.GetActiveDelegationsForUserAsync(userId);
            var activeDelegatorIds = activeDelegations.Select(d => d.DelegatorUserId).Distinct().ToList();
            var delegatorRoleIds = activeDelegations.Select(d => d.DelegatorRoleId).Where(id => !string.IsNullOrEmpty(id)).Distinct().ToList();
            var delegatorRoleNames = await GetDelegatorRoleNamesAsync(delegatorRoleIds);
            var stepsQuery = WorkflowStepsWithNavigations();

            IQueryable<BaseRequest> query = _baseRequestRepository.Find(_ => true, false, BaseRequestListIncludes);

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
                        query = query.Where(r => stepsQuery.Any(was =>
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
                        query = query.Where(r => stepsQuery.Any(was =>
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

            // Apply search only; default user-actions sort is action-required first
            query = PrepareBaseQuery(query, request.Filter, applyDefaultSort: false);

            var hasClientSort = request.Filter != null && !string.IsNullOrEmpty(request.Filter.sortField);
            if (!hasClientSort)
            {
                query = OrderByActionRequiredFirst(query, stepsQuery, userId, activeDelegatorIds, userRoles, delegatorRoleNames);
            }
            else
            {
                query = ApplyUserActionsCompositeSort(query, request.Filter);
            }

            var paginatedRequests = await PaginatedList<BaseRequest>.CreateAsyncForTableBinding(query, request);
            var dtos = _mapper.Map<List<BaseRequestDto>>(paginatedRequests.Items);

            // Fetch IsMyTurn status
            if (dtos.Any())
            {
                var fetchedRequestIds = dtos.Select(r => r.Id).ToList();
                var myTurnSet = (await WorkflowStepsWithNavigations()
                    .Where(was => was.IsCurrent && fetchedRequestIds.Contains(was.TargetRequestId) && (
                        (was.ApproverUserId == userId || activeDelegatorIds.Contains(was.ApproverUserId)) ||
                        (was.ApproverUserId == null && (
                            (userRoles.Contains(was.WorkflowStep.ApplicationRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.ApplicationRole.Name)) ||
                            (was.WorkflowStep.HigherApprovalRole != null && (userRoles.Contains(was.WorkflowStep.HigherApprovalRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.HigherApprovalRole.Name))) ||
                            was.WorkflowStep.ParallelRoles.Any(pr => userRoles.Contains(pr.Role.Name) || delegatorRoleNames.Contains(pr.Role.Name))
                        ))
                    ))
                    .Select(was => was.TargetRequestId)
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
