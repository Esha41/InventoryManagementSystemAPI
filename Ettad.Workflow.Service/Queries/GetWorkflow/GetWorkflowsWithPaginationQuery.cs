using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Models;
using Ettad.Data.Entities.Workflows;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ettad.Workflows.Service.Dtos;
using WorkflowEntity = global::Ettad.Data.Entities.Workflows.Workflow;

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
                .AsNoTracking()
                .Where(w => !w.IsDeleted)
                .Include(w => w.AutoRejectTrigger)
                    .ThenInclude(t => t!.TriggerRoles)
                .Include(w => w.AutoRejectTrigger)
                    .ThenInclude(t => t!.TriggerSteps)
                .Include(w => w.WorkflowSteps)
                    .ThenInclude(step => step.Transitions)
                        .ThenInclude(t => t.TargetWorkflowStep)
                            .ThenInclude(target => target.ApplicationRole)
                .Include(w => w.WorkflowSteps)
                    .ThenInclude(step => step.Transitions)
                        .ThenInclude(t => t.TargetWorkflowStep)
                            .ThenInclude(target => target.HigherApprovalRole)
                .Include(w => w.WorkflowSteps)
                    .ThenInclude(step => step.ApplicationRole)
                .Include(w => w.WorkflowSteps)
                    .ThenInclude(step => step.ParallelRoles)
                        .ThenInclude(pr => pr.Role)
                .AsQueryable();

            if (!_currentUserService.IsSuperAdmin)
            {
                workflowsQueryable = workflowsQueryable;
            }

            PagedListRequestNormalizer.Normalize(request.PagedListRequest);

            var paginatedEntities = await PaginatedList<WorkflowEntity>.CreateAsyncForTableBinding(
                workflowsQueryable,
                request.PagedListRequest);

            var dtoItems = paginatedEntities.Items.Select(w =>
            {
                var dto = _mapper.Map<WorkflowDto>(w);
                return dto;
            }).ToList();

            var paginatedWorkflows = new PaginatedList<WorkflowDto>(
                dtoItems,
                paginatedEntities.TotalCount,
                paginatedEntities.PageIndex,
                request.PagedListRequest.PageSize);

            return APIOperationResponse<PaginatedList<WorkflowDto>>.Success(paginatedWorkflows);
        }
    }
}
