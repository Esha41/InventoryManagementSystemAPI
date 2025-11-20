using Ettad.CrossCutting.Common.Security;
using Ettad.Data.Enums;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.DTO;
using Ettad.Workflows.Service.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ettad.Workflows.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowApprovalController : ApiControllerBase
    {
        private readonly IWorkflowApprovalService _service;

        public WorkflowApprovalController(IWorkflowApprovalService service)
        {
            _service = service;
        }

        [HttpGet]
        [CheckAuthorize("Permissions.RequestReciver.Page", "Permissions.RequestReciver.View")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        [CheckAuthorize("Permissions.RequestReciver.Page", "Permissions.RequestReciver.View")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [CheckAuthorize("Permissions.RequestReciver.Create")]
        public async Task<IActionResult> Create([FromBody] CreateWorkflowApprovalStepDto dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }

        [HttpPut("{id}")]
        [CheckAuthorize("Permissions.RequestReciver.Edit")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateWorkflowApprovalStepDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [CheckAuthorize("Permissions.RequestReciver.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return Ok();
        }

        [HttpGet("AllOrders")]
     //   [CheckAuthorize("Permissions.RequestReciver.Page", "Permissions.RequestReciver.View")]
        public async Task<IActionResult> GetOrdersWithApprovalSteps()
        {
            var result = await _service.GetOrdersWithApprovalStepsAsync();
            return Ok(result);
        }

        [HttpPost("approve-reject")]
        [CheckAuthorize("Permissions.RequestReciver.Create", "Permissions.RequestReciver.Edit")]
        public async Task<IActionResult> ApproveOrReject([FromBody] ApproveRejectWorkflowApprovalDto dto)
        {
            try
            {
                var result = await _service.ApproveOrReject(dto);

                // Determine success message based on action
                string successMessage = dto.Action == RequestStatus.Approved
                    ? "Request approved successfully."
                    : dto.Action == RequestStatus.Rejected
                    ? "Request rejected successfully."
                    : "Workflow step processed successfully.";

                // Wrap into API Response
                var response = new APIOperationResponse<WorkflowApprovalStepDto>
                {
                    StatusCode = (int)ResponseType.Success,
                    Data = result,
                    Message = successMessage
                };

                return ProcessResponse(response);
            }
            catch (KeyNotFoundException ex)
            {
                return ProcessResponse(new APIOperationResponse<string>
                {
                    StatusCode = (int)ResponseType.NotFound,
                    Message = ex.Message
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return ProcessResponse(new APIOperationResponse<string>
                {
                    StatusCode = (int)ResponseType.Unauthorized,
                    Message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return ProcessResponse(new APIOperationResponse<string>
                {
                    StatusCode = (int)ResponseType.BadRequest,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return ProcessResponse(new APIOperationResponse<string>
                {
                    StatusCode = (int)ResponseType.InternalServerError,
                    Message = $"An unexpected error occurred: {ex.Message}"
                });
            }
        }

    }
}

