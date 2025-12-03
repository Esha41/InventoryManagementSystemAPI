using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.Comman.Enums;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Project.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileUploadController : ApiControllerBase
    {
        private readonly IFileUploadService _fileUploadService;

        public FileUploadController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        /// <summary>
        /// Gets all files for a given entity type and primary id.
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByEntity([FromQuery] FileEntityType entityId, [FromQuery] long primaryId)
        {
            var result = await _fileUploadService.GetByEntityAsync(entityId, primaryId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Gets a single file record by its master id.
        /// </summary>
        [HttpGet("{id:long}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _fileUploadService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Deletes a file (master and its details).
        /// </summary>
        [HttpDelete("{id:long}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _fileUploadService.DeleteAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Marks this file as the main file for its entity.
        /// </summary>
        [HttpPut("{id:long}/set-main")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SetMain(long id)
        {
            var result = await _fileUploadService.SetMainAsync(id);
            return ProcessResponse(result);
        }
    }
}


