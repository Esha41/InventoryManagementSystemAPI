using Ettad.CrossCutting.Common.Security;
using Ettad.Inventory.Service.Monitoring.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ettad.Inventory.Service.Monitoring.Interfaces;

namespace Ettad.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ItemNotificationController : ApiControllerBase
    {
        private readonly ILowStockMonitorSettingsService _lowStockMonitorSettingsService;
        private readonly ICriticalStockMonitorSettingsService _criticalStockMonitorSettingsService;

        public ItemNotificationController(
            ILowStockMonitorSettingsService lowStockMonitorSettingsService,
            ICriticalStockMonitorSettingsService criticalStockMonitorSettingsService)
        {
            _lowStockMonitorSettingsService = lowStockMonitorSettingsService;
            _criticalStockMonitorSettingsService = criticalStockMonitorSettingsService;
        }

        /// <summary>
        /// Get low stock notification settings (roles and users)
        /// </summary>
        [HttpGet("settings")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("StockNotificationSettingsPage")]
        public async Task<IActionResult> GetSettings()
        {
            var result = await _lowStockMonitorSettingsService.GetSettingsAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update low stock notification settings (roles and users)
        /// </summary>
        [HttpPut("settings")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("StockNotificationSettingsPage")]
        public async Task<IActionResult> UpdateSettings([FromBody] ItemNotificationSettingsDto dto)
        {
            var result = await _lowStockMonitorSettingsService.UpdateSettingsAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get the current schedule (cron expression) for low stock monitoring
        /// </summary>
        [HttpGet("schedule")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("StockNotificationSettingsPage")]
        public async Task<IActionResult> GetSchedule()
        {
            var result = await _lowStockMonitorSettingsService.GetScheduleAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update the schedule (time) for low stock monitoring.
        /// Expects ISO-8601 with offset from the client; cron + Hangfire use server local time.
        /// </summary>
        [HttpPut("schedule")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("StockNotificationSettingsPage")]
        public async Task<IActionResult> UpdateSchedule([FromBody] UpdateScheduleDto dto)
        {
            // Service handles both database update and Hangfire job update
            var result = await _lowStockMonitorSettingsService.UpdateScheduleAsync(dto.ScheduleTime);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get critical stock notification settings (roles and users)
        /// </summary>
        [HttpGet("critical-settings")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("StockNotificationSettingsPage")]
        public async Task<IActionResult> GetCriticalSettings()
        {
            var result = await _criticalStockMonitorSettingsService.GetSettingsAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update critical stock notification settings (roles and users)
        /// </summary>
        [HttpPut("critical-settings")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("StockNotificationSettingsPage")]
        public async Task<IActionResult> UpdateCriticalSettings([FromBody] ItemNotificationSettingsDto dto)
        {
            var result = await _criticalStockMonitorSettingsService.UpdateSettingsAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get the current schedule (cron expression) for critical stock monitoring
        /// </summary>
        [HttpGet("critical-schedule")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("StockNotificationSettingsPage")]
        public async Task<IActionResult> GetCriticalSchedule()
        {
            var result = await _criticalStockMonitorSettingsService.GetScheduleAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update the schedule (time) for critical stock monitoring.
        /// Expects ISO-8601 with offset from the client; cron + Hangfire use server local time.
        /// </summary>
        [HttpPut("critical-schedule")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("StockNotificationSettingsPage")]
        public async Task<IActionResult> UpdateCriticalSchedule([FromBody] UpdateScheduleDto dto)
        {
            var result = await _criticalStockMonitorSettingsService.UpdateScheduleAsync(dto.ScheduleTime);
            return ProcessResponse(result);
        }
    }

    public class UpdateScheduleDto
    {
        public DateTimeOffset ScheduleTime { get; set; }
    }
}
