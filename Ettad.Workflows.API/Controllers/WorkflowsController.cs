using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ettad.Workflows.Service.Command.CreateWorkflow;
using Ettad.Workflows.Service.Command.DeleteWorkflow;
using Ettad.Workflows.Service.Command.UpdateWorkflow;
using Ettad.Workflows.Service.Queries.GetWorkflow;
using Ettad.Workflows.Service.Queries.GetWorkflowById;
using Ettad.CrossCutting.Comman.Models;
using Ettad.ResponseHandler.Models;
using Ettad.Data.Enums;
using Ettad.Application.Common.Interfaces;

namespace Ettad.Workflows.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public WorkflowsController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkflowById(int id)
        {
            var query = new GetWorkflowByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("all")]
        public async Task<IActionResult> GetAllWorkflows([FromBody] PagedListRequest request)
        {
            var query = new GetWorkflowsWithPaginationQuery(request);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("all-list")]
        public async Task<IActionResult> GetAllWorkflowsList()
        {
            var query = new GetWorkflowsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("by-type/{workflowType}")]
        public async Task<IActionResult> GetWorkflowsByType(WorkflowType workflowType)
        {
            var query = new GetWorkflowsByTypeQuery(workflowType);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
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
