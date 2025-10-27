using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Ettad.Workflows.Service.DTO;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Models;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.Queries.GetWorkflow
{
    public class GetWorkflowsWithPaginationQuery : IRequest<APIOperationResponse<PaginatedList<WorkflowDto>>>
    {
        public PagedListRequest PagedListRequest { get; }

        public GetWorkflowsWithPaginationQuery(PagedListRequest pagedListRequest)
        {
            PagedListRequest = pagedListRequest;
        }
    }

    public class GetWorkflowsWithPaginationQueryHandler : IRequestHandler<GetWorkflowsWithPaginationQuery, APIOperationResponse<PaginatedList<WorkflowDto>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetWorkflowsWithPaginationQueryHandler(
            ApplicationDbContext context,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<APIOperationResponse<PaginatedList<WorkflowDto>>> Handle(GetWorkflowsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var workflowsQueryable = _context.Workflows
                .Where(w => !w.IsDeleted)
                .AsQueryable();

            // Filter the query based on the current user's role and OrganizationId
            if (!_currentUserService.IsSuperAdmin)
            {
                workflowsQueryable = workflowsQueryable.Where(w => w.OrganizationId == _currentUserService.OrganizationId);
            }

            var projectedQueryable = workflowsQueryable.ProjectTo<WorkflowDto>(_mapper.ConfigurationProvider);

            var paginatedWorkflows = await PaginatedList<WorkflowDto>.CreateAsyncForTableBinding(
                projectedQueryable,
                request.PagedListRequest
            );

            return APIOperationResponse<PaginatedList<WorkflowDto>>.Success(paginatedWorkflows);
        }
    }
}