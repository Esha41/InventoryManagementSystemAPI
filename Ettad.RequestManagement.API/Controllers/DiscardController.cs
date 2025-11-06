using Ettad.CrossCutting.Common.Security;
using Ettad.RequestManagement.Service.Discards;
using Ettad.RequestManagement.Service.Discards.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.RequestManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiscardController : ApiControllerBase
    {
        private readonly IDiscardService _discardService;

        public DiscardController(IDiscardService discardService)
        {
            _discardService = discardService;
        }

        /// <summary>
        /// Get discard by ID with all details and navigation properties
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Discard.View", "Permissions.Discard.Page")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _discardService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all discards with details and navigation properties
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Discard.View", "Permissions.Discard.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _discardService.GetAllAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Create a new discard with items
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Discard.Create")]
        public async Task<IActionResult> Create([FromBody] CreateDiscardDto dto)
        {
            var result = await _discardService.CreateAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Change the priority of an existing discard
        /// </summary>
        [HttpPatch("{id}/priority")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Discard.Edit")]
        public async Task<IActionResult> ChangePriority(long id, [FromBody] string priority)
        {
            // Validate priority in controller
            if (string.IsNullOrWhiteSpace(priority))
                return BadRequest("Priority is required");

            var result = await _discardService.ChangePriorityAsync(id, priority);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Soft delete a discard
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Discard.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _discardService.DeleteAsync(id);
            return ProcessResponse(result);
        }
    }
}

