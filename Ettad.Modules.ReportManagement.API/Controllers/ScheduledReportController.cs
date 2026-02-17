using Ettad.CrossCutting.Common.Security;
using Ettad.Modules.ReportManagement.API.Services.Dtos;
using Ettad.Modules.ReportManagement.API.Services.Interfaces;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Modules.ReportManagement.API.Controllers
{
    /// <summary>
    /// Controller for managing scheduled reports
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ScheduledReportController : ApiControllerBase
    {
        private readonly IScheduledReportService _scheduledReportService;

        public ScheduledReportController(IScheduledReportService scheduledReportService)
        {
            _scheduledReportService = scheduledReportService;
        }

        /// <summary>
        /// Get all scheduled reports
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("ReportDesigner")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _scheduledReportService.GetAllAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get scheduled report by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("ReportDesigner")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _scheduledReportService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Create a new scheduled report
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("ReportDesigner")]
        public async Task<IActionResult> Create([FromBody] CreateScheduledReportDto dto)
        {
            var result = await _scheduledReportService.CreateAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update an existing scheduled report
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("ReportDesigner")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateScheduledReportDto dto)
        {
            var result = await _scheduledReportService.UpdateAsync(id, dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Delete a scheduled report
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("ReportDesigner")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _scheduledReportService.DeleteAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Toggle active status of a scheduled report
        /// </summary>
        [HttpPatch("{id}/toggle-active")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("ReportDesigner")]
        public async Task<IActionResult> ToggleActive(Guid id, [FromBody] ToggleActiveDto dto)
        {
            var result = await _scheduledReportService.ToggleActiveAsync(id, dto.IsActive);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get execution history for a scheduled report
        /// </summary>
        [HttpGet("{id}/executions")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("ReportDesigner")]
        public async Task<IActionResult> GetExecutionHistory(Guid id)
        {
            var result = await _scheduledReportService.GetExecutionHistoryAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Execute a scheduled report immediately
        /// </summary>
        [HttpPost("{id}/execute-now")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("ReportDesigner")]
        public async Task<IActionResult> ExecuteNow(Guid id)
        {
            var result = await _scheduledReportService.ExecuteNowAsync(id);
            return ProcessResponse(result);
        }
    }
}
