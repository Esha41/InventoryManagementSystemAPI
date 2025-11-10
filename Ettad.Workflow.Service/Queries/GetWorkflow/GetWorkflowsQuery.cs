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

            // Filter based on user if needed
            if (!_currentUserService.IsSuperAdmin)
            {
                workflowsQueryable = workflowsQueryable; // Add filter if required
            }

            var workflows = await workflowsQueryable
                .Select(
                   w => new WorkflowDto
                    {
                        Id = w.Id,
                        WorkflowName = w.WorkflowName,
                        WorkflowType = w.WorkflowType,
                       // WorkflowTypeName = t.NameEn, // or NameAr if you prefer
                        IsActive = w.IsActive,
                        IsDeleted = w.IsDeleted,
                        WorkflowSteps = w.WorkflowSteps.Select(step => new WorkflowStepDto
                        {
                            Id = step.Id,
                            WorkflowId = step.WorkflowId,
                            StepOrder = step.StepOrder,
                            ApplicationRoleId = step.ApplicationRoleId,
                            ApplicationEntityId = step.ApplicationEntityId,
                            MustApprove = step.MustApprove,
                            RequireHigherApproval = step.RequireHigherApproval,
                            HigherApprovalRoleId = step.HigherApprovalRoleId,
                            HigherApplicationEntityId= step.HigherApplicationEntityId,
                            ReserveQty = step.ReserveQty
                        }).ToList()
                    }
                )
                .ToListAsync(cancellationToken);

            return APIOperationResponse<List<WorkflowDto>>.Success(workflows);
        }

    }
}