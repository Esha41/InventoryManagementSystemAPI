using Ettad.CrossCutting.Common.Security;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.Commands.ReplaceWorkflowStepRequesterQtyNotifications;
using Ettad.Workflows.Service.Dtos;
using Ettad.Workflows.Service.Queries.GetWorkflowStepRequesterQtyNotifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Workflows.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WorkflowStepRequesterQtyNotificationsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public WorkflowStepRequesterQtyNotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Configured workflow step IDs for the given workflow (requester notification when that step is approved).</summary>
        [HttpGet("workflow/{workflowId:long}")]
        [ProducesResponseType(typeof(APIOperationResponse<List<long>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Workflow.View", "Permissions.Workflow.Page")]
        public async Task<IActionResult> GetConfiguredStepIdsForWorkflow(long workflowId)
        {
            var ids = await _mediator.Send(new GetWorkflowStepRequesterQtyNotificationsQuery(workflowId));
            var wrapped = APIOperationResponse<List<long>>.Success(ids, null);
            return ProcessResponse(wrapped);
        }

        /// <summary>Replace configured workflow step IDs for one workflow (deletes existing rows for that workflow, then inserts the new set).</summary>
        [HttpPut]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Workflow.Edit")]
        public async Task<IActionResult> Replace([FromBody] ReplaceWorkflowStepRequesterQtyNotificationsDto dto)
        {
            var result = await _mediator.Send(new ReplaceWorkflowStepRequesterQtyNotificationsCommand(dto));
            return ProcessResponse(result);
        }
    }
}
