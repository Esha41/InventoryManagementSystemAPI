using Ettad.Application.Common.Interfaces;
using Ettad.CrossCutting.Comman.Models;
using Ettad.CrossCutting.Common.Security;
using Ettad.Data.Enums;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.Command.CreateWorkflow;
using Ettad.Workflows.Service.Command.DeleteWorkflow;
using Ettad.Workflows.Service.Command.ManageTransitions;
using Ettad.Workflows.Service.Command.UpdateWorkflow;
using Ettad.Workflows.Service.Queries.GetNextSteps;
using Ettad.Workflows.Service.Queries.GetWorkflow;
using Ettad.Workflows.Service.Queries.GetWorkflowById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ettad.Workflows.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WorkflowsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public WorkflowsController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        [HttpGet("step/{stepId}/next-steps")]
        [CheckAuthorize("Permissions.Workflow.View", "Permissions.Workflow.Page")]
        public async Task<IActionResult> GetNextStepsForWorkflowStep(int stepId)
        {
            var query = new GetNextStepsForWorkflowStepQuery(stepId);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("step-transition")]
        [CheckAuthorize("Permissions.Workflow.Edit")]
        public async Task<IActionResult> SetStepTransitions([FromBody] SetWorkflowStepTransitionsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("step-transition")]
        [CheckAuthorize("Permissions.Workflow.Edit")]
        public async Task<IActionResult> RemoveStepTransition([FromBody] RemoveWorkflowStepTransitionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [CheckAuthorize("Permissions.Workflow.View", "Permissions.Workflow.Page")]
        public async Task<IActionResult> GetWorkflowById(int id)
        {
            var query = new GetWorkflowByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("all")]
        [CheckAuthorize("Permissions.Workflow.View", "Permissions.Workflow.Page")]
        public async Task<IActionResult> GetAllWorkflows([FromBody] PagedListRequest request)
        {
            var query = new GetWorkflowsWithPaginationQuery(request);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("all-list")]
        [CheckAuthorize("Permissions.Workflow.View", "Permissions.Workflow.Page")]
        public async Task<IActionResult> GetAllWorkflowsList()
        {
            var query = new GetWorkflowsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("by-type/{workflowType}")]
        [CheckAuthorize("Permissions.Workflow.View", "Permissions.Workflow.Page")]
        public async Task<IActionResult> GetWorkflowsByType(WorkflowType workflowType)
        {
            var query = new GetWorkflowsByTypeQuery(workflowType);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [CheckAuthorize("Permissions.Workflow.Create")]
        public async Task<IActionResult> CreateWorkflow([FromBody] CreateWorkflowCommand command)
        {
            // Restrict workflow creation to SuperAdmin only
            if (!_currentUserService.IsSuperAdmin)
            {
                return Forbid();
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [CheckAuthorize("Permissions.Workflow.Edit")]
        public async Task<IActionResult> UpdateWorkflow([FromBody] UpdateWorkflowCommand command)
        {
            // Restrict workflow updates to SuperAdmin only
            if (!_currentUserService.IsSuperAdmin)
            {
                return Forbid();
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [CheckAuthorize("Permissions.Workflow.Delete")]
        public async Task<IActionResult> DeleteWorkflow(int id)
        {
            // Restrict workflow deletion to SuperAdmin only
            if (!_currentUserService.IsSuperAdmin)
            {
                return Forbid();
            }

            var command = new DeleteWorkflowCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
