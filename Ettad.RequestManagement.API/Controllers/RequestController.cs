using Ettad.CrossCutting.Common.Security;
using Ettad.Data.Enums;
using Ettad.RequestManagement.Service.Common.Dtos;
using Ettad.RequestManagement.Service.Interfaces;
using Ettad.ResponseHandler.Models;
using Ettad.CrossCutting.Comman.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.RequestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RequestController : ApiControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        /// <summary>
        /// Get all requests with optional filtering by status and request type
        /// </summary>
        /// <param name="status">Optional request status filter</param>
        /// <param name="requestType">Optional request type filter</param>
        /// <returns>List of requests</returns>
        [HttpGet]
        [ProducesResponseType(typeof(APIOperationResponse<List<BaseRequestDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.RequestReciever.View", "Permissions.RequestReciever.Page")]
        public async Task<IActionResult> GetAllRequests([FromQuery] RequestStatus? status = null, [FromQuery] RequestType? requestType = null)
        {
            var result = await _requestService.GetAllRequestsAsync(status, requestType);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get requests by department with optional filtering by status and request type
        /// </summary>
        /// <param name="departmentId">Department ID</param>
        /// <param name="status">Optional request status filter</param>
        /// <param name="requestType">Optional request type filter</param>
        /// <returns>List of requests for the specified department</returns>
        [HttpGet("department/{departmentId}")]
        [ProducesResponseType(typeof(APIOperationResponse<List<BaseRequestDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.RequestReciever.View", "Permissions.RequestReciever.Page")]
        public async Task<IActionResult> GetRequestsByDepartment(
            long departmentId,
            [FromQuery] RequestStatus? status = null,
            [FromQuery] RequestType? requestType = null)
        {
            var result = await _requestService.GetRequestsByDepartmentAsync(departmentId, status, requestType);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get requests by requester with optional filtering by status and request type
        /// </summary>
        /// <param name="requesterId">Requester user ID</param>
        /// <param name="status">Optional request status filter</param>
        /// <param name="requestType">Optional request type filter</param>
        /// <returns>List of requests for the specified requester</returns>
        [HttpGet("requester/{requesterId}")]
        [ProducesResponseType(typeof(APIOperationResponse<List<BaseRequestDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [CheckAuthorize("Permissions.RequestReciever.View", "Permissions.RequestReciever.Page")]
        public async Task<IActionResult> GetRequestsByRequester(
            string requesterId,
            [FromQuery] RequestStatus? status = null,
            [FromQuery] RequestType? requestType = null)
        {
            var result = await _requestService.GetRequestsByRequesterAsync(requesterId, status, requestType);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get requests where the current user can take action or has taken action
        /// </summary>
        /// <param name="status">Optional request status filter</param>
        /// <param name="requestType">Optional request type filter</param>
        /// <returns>List of requests available for user action</returns>
        [HttpGet("user-actions")]
        [ProducesResponseType(typeof(APIOperationResponse<List<BaseRequestDto>>), (int)HttpStatusCode.OK)]
        [Authorize]
        public async Task<IActionResult> GetUserActionRequests(
            [FromQuery] RequestStatus? status = null,
            [FromQuery] RequestType? requestType = null)
        {
            var result = await _requestService.GetUserActionRequestsAsync(status, requestType);
            return ProcessResponse(result);
        }

        [HttpPost("Paginated")]
        [ProducesResponseType(typeof(APIOperationResponse<PaginatedList<BaseRequestDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.RequestReciever.View", "Permissions.RequestReciever.Page")]
        public async Task<IActionResult> GetAllPaginated([FromBody] PagedListRequest request)
        {
            var result = await _requestService.GetAllPaginatedAsync(request);
            return ProcessResponse(result);
        }

        [HttpPost("UserActionsPaginated")]
        [ProducesResponseType(typeof(APIOperationResponse<PaginatedList<BaseRequestDto>>), (int)HttpStatusCode.OK)]
        [Authorize]
        public async Task<IActionResult> GetUserActionRequestsPaginated([FromBody] PagedListRequest request)
        {
            var result = await _requestService.GetUserActionRequestsPaginatedAsync(request);
            return ProcessResponse(result);
        }
    }
}
