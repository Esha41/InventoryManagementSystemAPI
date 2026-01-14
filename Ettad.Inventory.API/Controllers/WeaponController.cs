using Microsoft.AspNetCore.Http;
using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Weapons;
using Ettad.Inventory.Service.Weapons.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WeaponController : ApiControllerBase
    {
        private readonly IWeaponService _weaponService;

        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.View", "Permissions.Weapon.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _weaponService.GetAllAsync();
            return ProcessResponse(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.View")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _weaponService.GetByIdAsync(id);
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
        public async Task<IActionResult> Import(IFormFile file)
        {
            var result = await _weaponService.ImportAsync(file);
            return ProcessResponse(result);
        }

        [HttpPost("ImportPreview")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.Create")]
        public async Task<IActionResult> ImportPreview(IFormFile file)
        {
            var result = await _weaponService.ImportPreviewAsync(file);
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

                var fileName = $"Weapon_Import_Template_{language}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(templateResult.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"Error generating template: {ex.Message}");
            }
        }
    }
}

