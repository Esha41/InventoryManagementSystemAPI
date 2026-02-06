using Ettad.CrossCutting.Common.Security;
using Ettad.Reporting.Services;
using Ettad.Reporting.Services.Reports.Dtos;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ettad.Reporting.Controllers
{
    /// <summary>
    /// Controller for managing reports and DevExpress Web Report Designer endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportController : ApiControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Get all reports
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Report.View", "Permissions.Report.Page")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _reportService.GetAllAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all public reports (Published status) - accessible to all authenticated users
        /// </summary>
        [HttpGet("public")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPublicReports()
        {
            var result = await _reportService.GetPublicReportsAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get report by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Report.View", "Permissions.Report.Page")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _reportService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get report by URL (for DevExpress ReportStorage)
        /// </summary>
        [HttpGet("url/{url}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Report.View", "Permissions.Report.Page")]
        public async Task<IActionResult> GetByUrl(string url)
        {
            var result = await _reportService.GetByUrlAsync(url);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Create a new report
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [CheckAuthorize("Permissions.Report.Create")]
        public async Task<IActionResult> Create([FromBody] CreateReportDto dto)
        {
            var result = await _reportService.CreateAsync(dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Update an existing report
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Report.Edit")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReportDto dto)
        {
            var result = await _reportService.UpdateAsync(id, dto);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Set report public (Published) or private (Draft). Body: { "isPublic": true|false }.
        /// </summary>
        [HttpPatch("{id}/public")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Report.Edit")]
        public async Task<IActionResult> SetReportPublic(Guid id, bool isPublic)
        {
            var result = await _reportService.SetReportPublicAsync(id, isPublic);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Delete a report (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [CheckAuthorize("Permissions.Report.Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _reportService.DeleteAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all report statuses
        /// </summary>
        [HttpGet("statuses")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Report.View", "Permissions.Report.Page")]
        public async Task<IActionResult> GetReportStatuses()
        {
            var result = await _reportService.GetReportStatusesAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Get all available report templates
        /// </summary>
        [HttpGet("templates")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [CheckAuthorize("Permissions.Report.View", "Permissions.Report.Create", "Permissions.Report.Page")]
        public async Task<IActionResult> GetTemplates()
        {
            var result = await _reportService.GetTemplatesAsync();
            return ProcessResponse(result);
        }

        /// <summary>
        /// Import a report from a file (.repx or .xml)
        /// </summary>
        //[HttpPost("import")]
        //[Consumes("multipart/form-data")]
        //[ProducesResponseType((int)HttpStatusCode.Created)]
        //[CheckAuthorize("Permissions.Report.Create")]
        //public async Task<IActionResult> Import([FromForm] ImportReportRequestDto request)
        //{
        //    var result = await _reportService.ImportAsync(
        //        request.File,
        //        request.ReportName,
        //        request.Url,
        //        request.Description);

        //    return ProcessResponse(result);
        //}
    }
}
