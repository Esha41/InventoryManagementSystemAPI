using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Monitoring;
using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Hangfire;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemNotificationController : ApiControllerBase
    {
        private readonly ILowStockMonitorSettingsService _settingsService;
        private readonly IRecurringJobManager _recurringJobManager;

        public ItemNotificationController(
            ILowStockMonitorSettingsService settingsService,
            IRecurringJobManager recurringJobManager)
        {
            _settingsService = settingsService;
            _recurringJobManager = recurringJobManager;
        }

        /// <summary>
        /// Get low stock notification settings (roles and users)
        /// </summary>
        [HttpGet("settings")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View")]
        public async Task<IActionResult> GetSettings()
        {
            var result = await _settingsService.GetSettingsAsync();
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
            var result = await _settingsService.UpdateSettingsAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get the current schedule (cron expression) for low stock monitoring
        /// </summary>
        [HttpGet("schedule")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.View")]
        public async Task<IActionResult> GetSchedule()
        {
            var result = await _settingsService.GetScheduleAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update the schedule (time) for low stock monitoring
        /// Accepts a DateTime and converts it to cron expression in the backend
        /// Updates both the database settings and the Hangfire recurring job immediately
        /// </summary>
        [HttpPut("schedule")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Inventory.Edit")]
        public async Task<IActionResult> UpdateSchedule([FromBody] UpdateScheduleDto dto)
        {
            // Service handles both database update and Hangfire job update
            var result = await _settingsService.UpdateScheduleAsync(dto.ScheduleTime);
            return ProcessResponse(result);
        }
    }

    public class UpdateScheduleDto
    {
        public DateTime ScheduleTime { get; set; }
    }
}
