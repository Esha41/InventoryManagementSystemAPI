using System.Net;
using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.WeaponAccessories.Dtos;
using Ettad.Inventory.Service.WeaponAccessories.Interfaces;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WeaponAccessoryController : ApiControllerBase
    {
        private readonly IWeaponAccessoryService _weaponAccessoryService;

        public WeaponAccessoryController(IWeaponAccessoryService weaponAccessoryService)
        {
            _weaponAccessoryService = weaponAccessoryService;
        }

        [HttpGet("weapon/{weaponId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.View")]
        public async Task<IActionResult> GetByWeaponId(long weaponId)
        {
            var result = await _weaponAccessoryService.GetByWeaponIdAsync(weaponId);
            return ProcessResponse(result);
        }

        [HttpGet("accessory/{accessoryId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Accessory.View")]
        public async Task<IActionResult> GetByAccessoryId(long accessoryId)
        {
            var result = await _weaponAccessoryService.GetByAccessoryIdAsync(accessoryId);
            return ProcessResponse(result);
        }

        [HttpGet("{weaponId}/{accessoryId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.View")]
        public async Task<IActionResult> GetById(long weaponId, long accessoryId)
        {
            var result = await _weaponAccessoryService.GetByIdAsync(weaponId, accessoryId);
            return ProcessResponse(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Weapon.Edit")]
        public async Task<IActionResult> Create([FromBody] CreateUpdateWeaponAccessoryDto input)
        {
            var result = await _weaponAccessoryService.CreateAsync(input);
            return ProcessResponse(result);
        }

        [HttpPut("{weaponId}/{accessoryId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.Edit")]
        public async Task<IActionResult> Update(long weaponId, long accessoryId, [FromBody] CreateUpdateWeaponAccessoryDto input)
        {
            var result = await _weaponAccessoryService.UpdateAsync(weaponId, accessoryId, input);
            return ProcessResponse(result);
        }

        [HttpDelete("{weaponId}/{accessoryId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.Edit")]
        public async Task<IActionResult> Delete(long weaponId, long accessoryId)
        {
            var result = await _weaponAccessoryService.DeleteAsync(weaponId, accessoryId);
            return ProcessResponse(result);
        }

        [HttpPut("weapon/{weaponId}/replace")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Weapon.Edit")]
        public async Task<IActionResult> ReplaceForWeapon(long weaponId, [FromBody] BulkReplaceWeaponAccessoriesDto input)
        {
            input.WeaponId = weaponId;
            var result = await _weaponAccessoryService.ReplaceForWeaponAsync(input);
            return ProcessResponse(result);
        }
    }
}
