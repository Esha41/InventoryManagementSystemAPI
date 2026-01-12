using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ettad.Application.Common.Interfaces;
using Ettad.Data.Entities;
using Ettad.Data.Entities.Workflows;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.CrossCutting.Comman.Constants;
using Ettad.RequestManagement.Service.Common.Dtos;
using Ettad.RequestManagement.Service.Interfaces;
using Ettad.ResponseHandler.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Ettad.RequestManagement.Service.Implementation
{
    public class RequestService : IRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public RequestService(
            ApplicationDbContext context,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
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

            // Roles that are restricted to their own department
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

            // --- DELEGATION LOGIC START ---
            // Use Qatar Time (UTC+3) for business logic validations as the user operates in this timezone
            var qatarNow = DateTime.UtcNow.AddHours(3);
            
            var activeDelegatorIds = await _context.UserDelegations
                .Where(d => d.DelegateeUserId == userId &&
                            d.IsActive &&
                            d.StartDate <= qatarNow &&
                            d.EndDate >= qatarNow)
                .Select(d => d.DelegatorUserId)
                .ToListAsync();

            var delegatorRoleNames = new List<string>();
            if (activeDelegatorIds.Any())
            {
                delegatorRoleNames = await _context.Set<IdentityUserRole<string>>()
                    .Where(ur => activeDelegatorIds.Contains(ur.UserId))
                    // Manual Join to get Role Names
                    .Join(_context.Roles, 
                          ur => ur.RoleId, 
                          r => r.Id, 
                          (ur, r) => r.Name)
                    .ToListAsync();
            }
            // --- DELEGATION LOGIC END ---

            // Filter requests where the user can take action or took an action
            // Optimization: Use Any() to filter directly in SQL instead of fetching all IDs first
            query = query.Where(r => _context.WorkflowApprovalSteps.Any(was => 
                was.TargetRequestId == r.Id && (
                    // 1. Direct Assignment (User OR Delegators)
                    (was.ApproverUserId == userId || activeDelegatorIds.Contains(was.ApproverUserId)) || 
                    
                    // 2. Role Assignment (User Role OR Delegator Role)
                    (was.ApproverUserId == null && (
                        (userRoles.Contains(was.WorkflowStep.ApplicationRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.ApplicationRole.Name)) || 
                        (was.WorkflowStep.HigherApprovalRole != null && (userRoles.Contains(was.WorkflowStep.HigherApprovalRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.HigherApprovalRole.Name)))
                    ))
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
                    // 1. Direct Assignment (User OR Delegators)
                    (was.ApproverUserId == userId || activeDelegatorIds.Contains(was.ApproverUserId)) || 
                    
                    // 2. Role Assignment (User Role OR Delegator Role)
                    (was.ApproverUserId == null && (
                        (userRoles.Contains(was.WorkflowStep.ApplicationRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.ApplicationRole.Name)) || 
                        (was.WorkflowStep.HigherApprovalRole != null && (userRoles.Contains(was.WorkflowStep.HigherApprovalRole.Name) || delegatorRoleNames.Contains(was.WorkflowStep.HigherApprovalRole.Name)))
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
    }
}
