using Ettad.CrossCutting.Common.Security;
using Ettad.RequestManagement.Service.Requests;
using Ettad.RequestManagement.Service.Requests.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.RequestManagement.API.Controller
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

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Request.View", "Permissions.Request.Page")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _requestService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Request.View", "Permissions.Request.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _requestService.GetAllAsync();
            return ProcessResponse(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Request.Create")]
        public async Task<IActionResult> Create([FromBody] CreateUpdateRequestDto dto)
        {
            var result = await _requestService.CreateAsync(dto);
            return ProcessResponse(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Request.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] CreateUpdateRequestDto dto)
        {
            var result = await _requestService.UpdateAsync(id, dto);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Request.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _requestService.DeleteAsync(id);
            return ProcessResponse(result);
        }
    }
}
