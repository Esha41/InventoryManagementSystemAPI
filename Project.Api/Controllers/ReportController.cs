using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ettad.EntityFramework.DataBaseContext;
using Microsoft.EntityFrameworkCore;

namespace Ettad.Reporting.Controllers
{
    /// <summary>
    /// Controller for DevExpress Web Report Designer endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Check if user has permission to access the report designer
        /// </summary>
        [HttpGet("can-design")]
        public IActionResult CanDesign()
        {
            var user = User;
            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                return Unauthorized(new { canDesign = false, message = "User is not authenticated" });
            }

            var canDesign = user.HasClaim("Permission", "ReportDesigner.Create") ||
                           user.HasClaim("Permission", "ReportDesigner.Edit") ||
                           user.IsInRole("Administrator");

            return Ok(new { canDesign, message = canDesign ? "User can design reports" : "User cannot design reports" });
        }

        /// <summary>
        /// Get list of available reports for the current user
        /// </summary>
        [HttpGet("reports")]
        public async Task<IActionResult> GetReports()
        {
            var user = User;
            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                return Unauthorized();
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = user.IsInRole("Administrator");

            try
            {
                // Query reports from database
                var query = _context.Reports
                    .Where(r => !r.IsDeleted);

                // If not admin, filter by user's reports or public reports
                if (!isAdmin && !string.IsNullOrEmpty(userId))
                {
                    query = query.Where(r => r.CreatedBy == userId || r.IsPublic);
                }

                var reports = await query
                    .OrderByDescending(r => r.CreatedDate)
                    .Select(r => new
                    {
                        id = r.Id.ToString(),
                        name = r.Name,
                        url = r.Url,
                        status = r.Status,
                        createdDate = r.CreatedDate,
                        modifiedDate = r.ModifiedDate,
                        isPublic = r.IsPublic,
                        description = r.Description
                    })
                    .ToListAsync();

                return Ok(new { reports });
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, new { message = "Error retrieving reports", error = ex.Message });
            }
        }
    }
}
