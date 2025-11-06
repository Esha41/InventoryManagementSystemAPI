using Ettad.CrossCutting.Common.Security;
using Ettad.RequestManagement.Service.Returns;
using Ettad.RequestManagement.Service.Returns.Dtos;
using Ettad.Data.Enums;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.RequestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReturnController : ApiControllerBase
    {
        private readonly IReturnService _returnService;

        public ReturnController(IReturnService returnService)
        {
            _returnService = returnService;
        }

        /// <summary>
        /// Get return by ID with all details and navigation properties
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Return.View", "Permissions.Return.Page")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _returnService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all returns with details and navigation properties
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Return.View", "Permissions.Return.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _returnService.GetAllAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Create a new return with items
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Return.Create")]
        public async Task<IActionResult> Create([FromBody] CreateReturnDto dto)
        {
            var result = await _returnService.CreateAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Change the priority of an existing return
        /// </summary>
        [HttpPatch("{id}/priority")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Return.Edit")]
        public async Task<IActionResult> ChangePriority(long id, [FromBody] RequestPriority priority)
        {
            var result = await _returnService.ChangePriorityAsync(id, priority);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Soft delete a return
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Return.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _returnService.DeleteAsync(id);
            return ProcessResponse(result);
        }
    }
}

