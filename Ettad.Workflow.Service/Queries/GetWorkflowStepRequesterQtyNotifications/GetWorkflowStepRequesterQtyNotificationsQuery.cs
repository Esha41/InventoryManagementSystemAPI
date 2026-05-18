using System.Collections.Generic;
using MediatR;

namespace Ettad.Workflows.Service.Queries.GetWorkflowStepRequesterQtyNotifications
{
    public class GetWorkflowStepRequesterQtyNotificationsQuery : IRequest<List<long>>
    {
        public long WorkflowId { get; }

        public GetWorkflowStepRequesterQtyNotificationsQuery(long workflowId)
        {
            WorkflowId = workflowId;
        }
    }
}
