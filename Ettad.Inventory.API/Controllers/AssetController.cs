using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Assets;
using Ettad.Inventory.Service.Assets.Dtos;
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
    public class AssetController : ApiControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.View", "Permissions.Asset.Page")]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _assetService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.View", "Permissions.Asset.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _assetService.GetAllAsync();
            return ProcessResponse(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Asset.Create")]
        public async Task<IActionResult> Create([FromForm] CreateAssetDto dto, [FromForm] List<IFormFile>? files = null)
        {
            var result = await _assetService.CreateAsync(dto, files);
            return ProcessResponse(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Asset.Edit")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateAssetDto dto)
        {
            var result = await _assetService.UpdateAsync(id, dto);
            return ProcessResponse(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Asset.Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _assetService.DeleteAsync(id);
            return ProcessResponse(result);
        }
    }
}

