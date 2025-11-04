using Ettad.CrossCutting.Common.Security;
using Ettad.RequestManagement.Service.RequestRecivers;
using Ettad.RequestManagement.Service.RequestRecivers.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.RequestManagement.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [CheckAuthorize(
        "Permissions.RequestReciver.Page",
        "Permissions.RequestReciver.View",
        "Permissions.RequestReciver.Create",
        "Permissions.RequestReciver.Edit",
        "Permissions.RequestReciver.Delete"
    )]
    public class RequestReciverController : ApiControllerBase
    {
        private readonly IRequestReciverService _requestReciverService;

        public RequestReciverController(IRequestReciverService requestReciverService)
        {
            _requestReciverService = requestReciverService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _requestReciverService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _requestReciverService.GetAllAsync();
            return ProcessResponse(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> Create([FromBody] CreateUpdateRequestReciverDto dto)
        {
            var result = await _requestReciverService.CreateAsync(dto);
            return ProcessResponse(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(long id, [FromBody] CreateUpdateRequestReciverDto dto)
        {
            var result = await _requestReciverService.UpdateAsync(id, dto);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _requestReciverService.DeleteAsync(id);
            return ProcessResponse(result);
        }
    }
}

