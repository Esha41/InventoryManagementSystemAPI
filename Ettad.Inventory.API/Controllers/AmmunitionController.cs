using Ettad.CrossCutting.Common.Security;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.Ammunitions;
using Ettad.Inventory.Service.Ammunitions.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AmmunitionController : ApiControllerBase
    {
        private readonly IAmmunitionService _ammunitionService;

        public AmmunitionController(IAmmunitionService ammunitionService)
        {
            _ammunitionService = ammunitionService;
        }


        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Ammunition.View", "Permissions.Ammunition.Page")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _ammunitionService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Ammunition.View", "Permissions.Ammunition.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _ammunitionService.GetAllAsync();
            return ProcessResponse(result);
        }

        [HttpGet("type/{ammunitionType}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Ammunition.View", "Permissions.Ammunition.Page")]
        public async Task<IActionResult> GetByType(AmmunitionType ammunitionType)
        {
            var result = await _ammunitionService.GetByTypeAsync(ammunitionType);
            return ProcessResponse(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Ammunition.Create")]
        public async Task<IActionResult> Create([FromForm] CreateUpdateAmmunitionDto dto, [FromForm] List<IFormFile>? files = null)
        {
            var result = await _ammunitionService.CreateAsync(dto, files);
            return ProcessResponse(result);
        }

        [HttpPost("Import")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Ammunition.Create")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            var result = await _ammunitionService.ImportAsync(file);
            return ProcessResponse(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Ammunition.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] CreateUpdateAmmunitionDto dto)
        {
            var result = await _ammunitionService.UpdateAsync(id, dto);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Ammunition.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _ammunitionService.DeleteAsync(id);
            return ProcessResponse(result);
        }
    }
}
