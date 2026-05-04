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
        private readonly ICrossCuttingRepository<IdentityUserRole<string>> _userRoleRepository;
        private readonly ICrossCuttingRepository<ApplicationRole> _roleRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserDelegationService _userDelegationService;
        private readonly IMapper _mapper;

        public RequestService(
            ICrossCuttingRepository<BaseRequest> baseRequestRepository,
            ICrossCuttingRepository<WorkflowApprovalStep> workflowApprovalStepRepository,
            ICrossCuttingRepository<IdentityUserRole<string>> userRoleRepository,
            ICrossCuttingRepository<ApplicationRole> roleRepository,
            ICurrentUserService currentUserService,
            IUserDelegationService userDelegationService,
            IMapper mapper)
        {
            _baseRequestRepository = baseRequestRepository;
            _workflowApprovalStepRepository = workflowApprovalStepRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _currentUserService = currentUserService;
            _userDelegationService = userDelegationService;
            _mapper = mapper;
        }

        private IQueryable<WorkflowApprovalStep> WorkflowStepsWithNavigations() =>
            _workflowApprovalStepRepository.Find(_ => true, includeSoftDeleted: false, WorkflowApprovalStepVisibilityIncludes);

        private async Task<List<string>> GetDelegatorRoleNamesAsync(IReadOnlyCollection<string> activeDelegatorIds)
        {
            if (activeDelegatorIds == null || activeDelegatorIds.Count == 0)
                return new List<string>();

            return await _userRoleRepository
                .Find(ur => activeDelegatorIds.Contains(ur.UserId))
                .Join(_roleRepository.Find(_ => true), ur => ur.RoleId, r => r.Id, (ur, r) => r.Name)
                .Where(n => n != null)
                .Select(n => n!)
                .Distinct()
                .ToListAsync();
        }

        private IQueryable<BaseRequest> PrepareBaseQuery(IQueryable<BaseRequest> query, FilterData filter)
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
            if (filter == null || string.IsNullOrEmpty(filter.sortField))
            {
                query = query.OrderByDescending(r => r.CreationDate);
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

            var activeDelegatorIds = await _userDelegationService.GetActiveDelegatorsForUserAsync(userId, DelegationScope.WorkflowApproval);
            var delegatorRoleNames = await GetDelegatorRoleNamesAsync(activeDelegatorIds);

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

                    var activeDelegatorIds = await _userDelegationService.GetActiveDelegatorsForUserAsync(userId, DelegationScope.WorkflowApproval);
                    var delegatorRoleNames = await GetDelegatorRoleNamesAsync(activeDelegatorIds);

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

                    var stepsQuery = WorkflowStepsWithNavigations();
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

            // Apply base query behavior (search and sorting)
            query = PrepareBaseQuery(query, request.Filter);

            var paginatedRequests = await PaginatedList<BaseRequest>.CreateAsyncForTableBinding(query, request);
            var dtos = _mapper.Map<List<BaseRequestDto>>(paginatedRequests.Items);

            // Fetch IsMyTurn status
            if (dtos.Any())
            {
                var activeDelegatorIds = await _userDelegationService.GetActiveDelegatorsForUserAsync(userId, DelegationScope.WorkflowApproval);
                var delegatorRoleNames = await GetDelegatorRoleNamesAsync(activeDelegatorIds);

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
