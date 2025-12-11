using Ettad.CrossCutting.Common.Security;
using Ettad.ResponseHandler.Consts;
using Ettad.ResponseHandler.Models;
using Ettad.Workflow.Service.DTO;
using Ettad.Workflow.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace Ettad.Workflows.API.Controllers
{
    /// <summary>
    /// Controller for managing workflow approval step notifiers
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WorkflowStepNotifiersController : ApiControllerBase
    {
        private readonly IWorkflowStepNotifierService _notifierService;

        public WorkflowStepNotifiersController(IWorkflowStepNotifierService notifierService)
        {
            _notifierService = notifierService;
        }

        /// <summary>
        /// Get all notifiers for a specific workflow step
        /// </summary>
        /// <param name="stepId">Workflow step ID</param>
        /// <returns>List of notifiers with user and role details</returns>
        [HttpGet("step/{stepId}")]
        [ProducesResponseType(typeof(APIOperationResponse<List<WorkflowStepNotifierDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.WorkFlowType.View", "Permissions.WorkFlowType.Page")]
        public async Task<IActionResult> GetNotifiersByStepId(int stepId)
        {
            var result = await _notifierService.GetNotifiersByStepIdAsync(stepId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all user IDs and role IDs that should be notified for a workflow step
        /// </summary>
        /// <param name="stepId">Workflow step ID</param>
        /// <returns>Object containing lists of user IDs and role IDs</returns>
        [HttpGet("step/{stepId}/ids")]
        [ProducesResponseType(typeof(APIOperationResponse<(List<string> UserIds, List<string> RoleIds)>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.WorkFlowType.View", "Permissions.WorkFlowType.Page")]
        public async Task<IActionResult> GetNotifierIdsByStepId(int stepId)
        {
            var result = await _notifierService.GetNotifierIdsByStepIdAsync(stepId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update notifiers for a workflow step (replaces all existing notifiers)
        /// </summary>
        /// <param name="stepId">Workflow step ID</param>
        /// <param name="dto">DTO containing user IDs and role IDs to notify</param>
        /// <returns>Success status</returns>
        [HttpPut("step/{stepId}")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.WorkFlowType.Edit")]
        public async Task<IActionResult> UpdateStepNotifiers(int stepId, [FromBody] UpdateWorkflowStepNotifiersDto dto)
        {
            // Ensure the stepId in the DTO matches the route parameter
            dto.WorkflowStepId = stepId;
            
            var result = await _notifierService.UpdateStepNotifiersAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Add notifiers to a workflow step (supports multiple users and/or roles)
        /// </summary>
        /// <param name="dto">DTO containing workflow step ID, user IDs, and role IDs</param>
        /// <returns>Success status</returns>
        [HttpPost]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.WorkFlowType.Edit")]
        public async Task<IActionResult> AddNotifiers([FromBody] CreateWorkflowStepNotifierDto dto)
        {
            // Validate DTO
            if (dto == null)
            {
                var nullResponse = new APIOperationResponse<bool>
                {
                    StatusCode = (int)ResponseType.BadRequest,
                    Data = false,
                    Message = "Request body cannot be null"
                };
                return ProcessResponse(nullResponse);
            }

            // Validate that at least one list is provided
            if ((dto.UserIds == null || !dto.UserIds.Any()) &&
                (dto.RoleIds == null || !dto.RoleIds.Any()))
            {
                var emptyResponse = new APIOperationResponse<bool>
                {
                    StatusCode = (int)ResponseType.BadRequest,
                    Data = false,
                    Message = "At least one user ID or role ID must be provided"
                };
                return ProcessResponse(emptyResponse);
            }

            var success = await _notifierService.AddNotifierAsync(dto);
            
            if (success)
            {
                var response = new APIOperationResponse<bool>
                {
                    StatusCode = (int)ResponseType.Success,
                    Data = true,
                    Message = "Notifiers added successfully"
                };
                return ProcessResponse(response);
            }
            else
            {
                var response = new APIOperationResponse<bool>
                {
                    StatusCode = (int)ResponseType.BadRequest,
                    Data = false,
                    Message = "Failed to add notifiers. Please verify that the workflow step exists and all user/role IDs are valid."
                };
                return ProcessResponse(response);
            }
        }

        /// <summary>
        /// Remove a notifier from a workflow step
        /// </summary>
        /// <param name="id">Notifier ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [CheckAuthorize("Permissions.WorkFlowType.Edit")]
        public async Task<IActionResult> RemoveNotifier(int id)
        {
            var result = await _notifierService.RemoveNotifierAsync(id);
            return ProcessResponse(result);
        }
    }
}

