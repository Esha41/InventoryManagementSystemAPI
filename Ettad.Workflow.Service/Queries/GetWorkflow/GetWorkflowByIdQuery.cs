using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ettad.Workflows.Service.DTO;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ResponseHandler.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ettad.Workflows.Service.Queries.GetWorkflowById
{
    public class GetWorkflowByIdQuery : IRequest<APIOperationResponse<WorkflowDto>>
    {
        public int Id { get; }

        public GetWorkflowByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetWorkflowByIdQueryHandler : IRequestHandler<GetWorkflowByIdQuery, APIOperationResponse<WorkflowDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetWorkflowByIdQueryHandler> _logger;

        public GetWorkflowByIdQueryHandler(
            ApplicationDbContext context,
            IMapper mapper,
            ILogger<GetWorkflowByIdQueryHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<APIOperationResponse<WorkflowDto>> Handle(GetWorkflowByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var workflow = await _context.Workflows
                    .Include(w => w.WorkflowSteps)
                        .ThenInclude(ws => ws.Transitions)
                    .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

                if (workflow == null)
                {
                    return APIOperationResponse<WorkflowDto>.NotFound($"Workflow with ID {request.Id} not found.");
                }

                var workflowDto = _mapper.Map<WorkflowDto>(workflow);
                return APIOperationResponse<WorkflowDto>.Success(workflowDto);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while fetching workflow by ID {WorkflowId}", request.Id);
                return APIOperationResponse<WorkflowDto>.ServerError($"Processing failed: {e.Message}");
            }
        }
    }
}