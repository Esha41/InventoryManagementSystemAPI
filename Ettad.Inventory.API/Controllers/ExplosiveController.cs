using Microsoft.AspNetCore.Http;
using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Explosives;
using Ettad.Inventory.Service.Explosives.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExplosiveController : ApiControllerBase
    {
        private readonly IExplosiveService _explosiveService;

        public ExplosiveController(IExplosiveService explosiveService)
        {
            _explosiveService = explosiveService;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Explosive.View", "Permissions.Explosive.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _explosiveService.GetAllAsync();
            return ProcessResponse(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Explosive.View")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _explosiveService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        [HttpGet("ByType/{type}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Explosive.View")]
        public async Task<IActionResult> GetByType(Ettad.Data.Enums.ExplosiveType type)
        {
            var result = await _explosiveService.GetByTypeAsync(type);
            return ProcessResponse(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Explosive.Create")]
        public async Task<IActionResult> Create([FromForm] CreateUpdateExplosiveDto input, [FromForm] List<IFormFile> files)
        {
            var result = await _explosiveService.CreateAsync(input, files);
            return ProcessResponse(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Explosive.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] CreateUpdateExplosiveDto input)
        {
            var result = await _explosiveService.UpdateAsync(id, input);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Explosive.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _explosiveService.DeleteAsync(id);
            return ProcessResponse(result);
        }
    }
}

