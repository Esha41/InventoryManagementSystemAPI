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

        [HttpGet("ByType/{type}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.View")]
        public async Task<IActionResult> GetByType(Ettad.Data.Enums.WeaponType type)
        {
            var result = await _weaponService.GetByTypeAsync(type);
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
    }
}

