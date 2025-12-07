using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Monitoring;
using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemNotificationController : ApiControllerBase
    {
        private readonly ILowStockMonitorService _monitorService;

        public ItemNotificationController(ILowStockMonitorService monitorService)
        {
            _monitorService = monitorService;
        }

        /// <summary>
        /// Get low stock notification settings (roles and users)
        /// </summary>
        [HttpGet("settings")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View")]
        public async Task<IActionResult> GetSettings()
        {
            var result = await _monitorService.GetSettingsAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update low stock notification settings (roles and users)
        /// </summary>
        [HttpPut("settings")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.Edit")]
        public async Task<IActionResult> UpdateSettings([FromBody] LowStockNotificationSettingsDto dto)
        {
            var result = await _monitorService.UpdateSettingsAsync(dto);
            return ProcessResponse(result);
        }
    }
}
