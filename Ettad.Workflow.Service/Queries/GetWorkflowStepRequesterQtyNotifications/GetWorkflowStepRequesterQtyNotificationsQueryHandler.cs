using Ettad.Data.Entities.Workflows;
using Ettad.EntityFramework.DataBaseContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ettad.Workflows.Service.Queries.GetWorkflowStepRequesterQtyNotifications
{
    public class GetWorkflowStepRequesterQtyNotificationsQueryHandler
        : IRequestHandler<GetWorkflowStepRequesterQtyNotificationsQuery, List<long>>
    {
        private readonly ApplicationDbContext _context;

        public GetWorkflowStepRequesterQtyNotificationsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<long>> Handle(
            GetWorkflowStepRequesterQtyNotificationsQuery request,
            CancellationToken cancellationToken)
        {
            if (request.WorkflowId <= 0)
                return new List<long>();

            return await _context.WorkflowStepRequesterQuantityNotifications
                .AsNoTracking()
                .Where(x => x.WorkflowId == request.WorkflowId)
                .OrderBy(x => x.WorkflowStepId)
                .Select(x => x.WorkflowStepId)
                .ToListAsync(cancellationToken);
        }
    }
}
