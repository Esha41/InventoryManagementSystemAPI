using Microsoft.AspNetCore.Http;
using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Weapons.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Comman.Models;
using Ettad.Inventory.Service.Weapons.Interfaces;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WeaponController : ApiControllerBase
    {
        private readonly IWeaponService _weaponService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public WeaponController(IWeaponService weaponService, IDateTimeProvider dateTimeProvider)
        {
            _weaponService = weaponService;
            _dateTimeProvider = dateTimeProvider;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.View", "Permissions.Weapon.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _weaponService.GetAllAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Weapons compatible with one or more ammunition caliber ids (issue request association step).
        /// </summary>
        [HttpGet("for-ammunition-association")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.View", "Permissions.Weapon.Page")]
        public async Task<IActionResult> GetForAmmunitionAssociation([FromQuery] List<long> ammunitionCaliberIds)
        {
            var result = await _weaponService.GetForAmmunitionAssociationAsync(ammunitionCaliberIds ?? new List<long>());
            return ProcessResponse(result);
        }

        [HttpPost("Paginated")]
        [ProducesResponseType(typeof(APIOperationResponse<PaginatedList<WeaponDto>>), (int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.View", "Permissions.Weapon.Page")]
        public async Task<IActionResult> GetAllPaginated([FromBody] PagedListRequest request)
        {
            var result = await _weaponService.GetAllPaginatedAsync(request);
            return ProcessResponse(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.View")]
        public async Task<IActionResult> GetById(long id, [FromQuery] bool includeDeleted = false)
        {
            var result = await _weaponService.GetByIdAsync(id, includeDeleted);
            return ProcessResponse(result);
        }

        [HttpPost("{id}/restore")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.Edit")]
        public async Task<IActionResult> Restore(long id)
        {
            var result = await _weaponService.RestoreAsync(id);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}/permanent")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Weapon.Delete")]
        public async Task<IActionResult> PermanentDelete(long id)
        {
            var result = await _weaponService.PermanentDeleteAsync(id);
            return ProcessResponse(result);
        }


        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Weapon.Create")]
        public async Task<IActionResult> Create([FromForm] CreateUpdateWeaponDto input, [FromForm] List<IFormFile> files)
        {
            var result = await _weaponService.CreateAsync(input, files);
            return ProcessResponse(result);
        }

        [HttpPost("Import")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.Create")]
        public async Task<IActionResult> Import(IFormFile file, [FromQuery] string language = "en")
        {
            var result = await _weaponService.ImportAsync(file, language);
            return ProcessResponse(result);
        }

        [HttpPost("ImportPreview")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.Create")]
        public async Task<IActionResult> ImportPreview(IFormFile file, [FromQuery] string language = "en")
        {
            var result = await _weaponService.ImportPreviewAsync(file, language);
            return ProcessResponse(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] CreateUpdateWeaponDto input)
        {
            var result = await _weaponService.UpdateAsync(id, input);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _weaponService.DeleteAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Generate weapon import template with Excel data validation (dropdowns for lookups)
        /// </summary>
        /// <param name="language">Language for template headers (en/ar), defaults to 'en'</param>
        /// <returns>Excel file with data validation dropdowns and all fields from web form</returns>
        [HttpGet("template")]
        [ProducesResponseType(typeof(FileContentResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [CheckAuthorize("Permissions.Weapon.Create")]
        public async Task<IActionResult> GenerateImportTemplate([FromQuery] string language = "en")
        {
            try
            {
                var templateResult = await _weaponService.GenerateImportTemplateAsync(language);
                
                if (!templateResult.Succeeded || templateResult.Data == null)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, templateResult.Message);
                }

                var fileName = $"Weapon_Import_Template_{language}_{_dateTimeProvider.Now:yyyyMMddHHmmss}.xlsx";
                return File(templateResult.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Error generating template: {ex.Message}");
            }
        }
    }
}

