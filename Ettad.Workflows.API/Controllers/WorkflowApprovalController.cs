using Ettad.CrossCutting.Common.Security;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.Commands.WorkflowApproval.ProcessWorkflowAction;
using Ettad.Workflows.Service.Dtos;
using Ettad.Workflows.Service.Queries.WorkflowApproval.GetAllBaseRequests;
using Ettad.Workflows.Service.Queries.WorkflowApproval.GetBaseRequestById;
using Ettad.Workflows.Service.Queries.WorkflowApproval.GetOrdersWithApprovalSteps;
using Ettad.Workflows.Service.Queries.WorkflowApproval.GetPreviousStepsForReturn;
using Ettad.Workflows.Service.Queries.WorkflowApproval.GetWorkflowApprovalStepById;
using Ettad.Workflows.Service.Queries.WorkflowApproval.GetWorkflowApprovalSteps;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ettad.Workflows.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowApprovalController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public WorkflowApprovalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [CheckAuthorize("Permissions.RequestReciever.Page", "Permissions.RequestReciever.View", "CanCancelRequest")]
        public async Task<IActionResult> GetAll()
            => ProcessResponse(await _mediator.Send(new GetWorkflowApprovalStepsQuery()));

        [HttpGet("{id}")]
        [CheckAuthorize("Permissions.RequestReciever.Page", "Permissions.RequestReciever.View", "CanCancelRequest")]
        public async Task<IActionResult> Get(long id)
            => ProcessResponse(await _mediator.Send(new GetWorkflowApprovalStepByIdQuery(id)));

        //[HttpGet("AllOrders")]
        //[CheckAuthorize("Permissions.RequestReciever.Page", "Permissions.RequestReciever.View")]
        //public async Task<IActionResult> GetOrdersWithApprovalSteps()
        //    => ProcessResponse(await _mediator.Send(new GetOrdersWithApprovalStepsQuery()));

        [HttpGet("AllBaseRequests")]
        [CheckAuthorize("Permissions.RequestReciever.Page", "Permissions.RequestReciever.View", "CanCancelRequest")]
        public async Task<IActionResult> GetAllBaseRequests()
        {
            var result = await _mediator.Send(new GetAllBaseRequestsQuery());
            return ProcessResponse(result);
        }

        [HttpGet("BaseRequest/{requestId}")]
        [CheckAuthorize("Permissions.RequestReciever.Page", "Permissions.RequestReciever.View", "CanCancelRequest")]
        public async Task<IActionResult> GetBaseRequestById(long requestId)
        {
            var result = await _mediator.Send(new GetBaseRequestByIdQuery(requestId));
            return ProcessResponse(result);
        }

        [HttpPost("process-action")]
        [Consumes("multipart/form-data", "application/json")]
        [CheckAuthorize("Permissions.RequestReciever.Create", "Permissions.RequestReciever.Edit", "CanCancelRequest")]
        public async Task<IActionResult> ProcessAction(
            [FromForm] ApproveRejectWorkflowApprovalDto dto,
            [FromForm] List<IFormFile>? files = null)
            => ProcessResponse(await _mediator.Send(new ProcessWorkflowActionCommand(dto, files)));

        [HttpGet("previous-steps/{requestId}")]
        [CheckAuthorize("Permissions.RequestReciever.Page", "Permissions.RequestReciever.View", "CanCancelRequest")]
        public async Task<IActionResult> GetPreviousWorkflowStepsForReturn(long requestId)
            => ProcessResponse(await _mediator.Send(new GetPreviousStepsForReturnQuery(requestId)));

    }
}
