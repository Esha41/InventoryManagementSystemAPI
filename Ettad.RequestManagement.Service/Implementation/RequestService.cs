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
                .ProjectTo<BaseRequestDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return APIOperationResponse<List<BaseRequestDto>>.Success(requests);
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
                .ProjectTo<BaseRequestDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return APIOperationResponse<List<BaseRequestDto>>.Success(requests);
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
                .ProjectTo<BaseRequestDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return APIOperationResponse<List<BaseRequestDto>>.Success(requests);
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
                .ProjectTo<BaseRequestDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return APIOperationResponse<List<BaseRequestDto>>.Success(requests);
        }

        public async Task<APIOperationResponse<List<BaseRequestDto>>> GetUserActionRequestsAsync(RequestStatus? status = null, RequestType? requestType = null)
        {
            var userId = _currentUserService.UserId;
            var userRoles = _currentUserService.Roles ?? new List<string>();
            var userDepartmentId = _currentUserService.DepartmentId;

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
                    .ProjectTo<BaseRequestDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return APIOperationResponse<List<BaseRequestDto>>.Success(requesterRequests);
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

            // Filter requests where the user can take action or took an action
            // Join with WorkflowApprovalStep and WorkflowStep
            
            var requestIds = await _context.WorkflowApprovalSteps
                .Include(was => was.WorkflowStep)
                .ThenInclude(ws => ws.ApplicationRole)
                .Where(was => 
                    (was.ApproverUserId == userId) || 
                    (was.ApproverUserId == null && userRoles.Contains(was.WorkflowStep.ApplicationRole.Name))
                )
                .Select(was => (long)was.TargetRequestId)
                .Distinct()
                .ToListAsync();

            query = query.Where(r => requestIds.Contains(r.Id));

            var requests = await query
                .Include(r => r.Department)
                .Include(r => r.Requester)
                    .ThenInclude(u => u.Rank)
                .Include(r => r.RequestPurpose)
                .ProjectTo<BaseRequestDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return APIOperationResponse<List<BaseRequestDto>>.Success(requests);
        }
    }
}
