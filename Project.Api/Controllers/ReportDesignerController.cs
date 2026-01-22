using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ettad.Reporting.Controllers
{
    /// <summary>
    /// Controller for DevExpress Web Report Designer endpoints
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportDesignerController : ControllerBase
    {
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
        public IActionResult GetReports()
        {
            var user = User;
            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                return Unauthorized();
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = user.IsInRole("Administrator");

            // This will be implemented with actual database query
            // For now, return empty list
            return Ok(new { reports = new List<object>() });
        }
    }
}
