using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ettad.Workflows.Service.DTO;
using Ettad.Application.Common.Interfaces;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.Queries.GetWorkflow
{
    public class GetWorkflowsQuery : IRequest<APIOperationResponse<List<WorkflowDto>>>
    {
    }

    public class GetWorkflowsQueryHandler : IRequestHandler<GetWorkflowsQuery, APIOperationResponse<List<WorkflowDto>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetWorkflowsQueryHandler(
            ApplicationDbContext context,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<APIOperationResponse<List<WorkflowDto>>> Handle(GetWorkflowsQuery request, CancellationToken cancellationToken)
        {
            var workflowsQueryable = _context.Workflows
                .Where(w => !w.IsDeleted)
                .AsQueryable();

            // Filter the query based on the current user's role and OrganizationId
            if (!_currentUserService.IsSuperAdmin)
            {
                workflowsQueryable = workflowsQueryable.Where(w => w.OrganizationId == _currentUserService.OrganizationId);
            }

            var workflows = await workflowsQueryable
                .ProjectTo<WorkflowDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return APIOperationResponse<List<WorkflowDto>>.Success(workflows);
        }
    }
}