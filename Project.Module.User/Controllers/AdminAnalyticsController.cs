using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.Interfaces;
using Ettad.CrossCutting.Common.Security;
using System.Threading.Tasks;

namespace Ettad.User.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/admin/analytics")]
    public class AdminAnalyticsController : ApiControllerBase
    {
        private readonly IAdminAnalyticsService _analyticsService;

        public AdminAnalyticsController(IAdminAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("system-health")]
        [CheckAuthorize("Permissions.Analytics.View", "Permissions.Analytics.Page")]
        public async Task<IActionResult> GetSystemHealth()
        {
            var response = await _analyticsService.GetSystemHealthMetricsAsync();
            return ProcessResponse(response);
        }

        [HttpGet("performance")]
        [CheckAuthorize("Permissions.Analytics.View", "Permissions.Analytics.Page")]
        public async Task<IActionResult> GetPerformance()
        {
            var response = await _analyticsService.GetPerformanceMetricsAsync();
            return ProcessResponse(response);
        }

        [HttpGet("user-activity")]
        [CheckAuthorize("Permissions.AdminDashboard.View")]
        public async Task<IActionResult> GetUserActivity()
        {
            var response = await _analyticsService.GetUserActivityMetricsAsync();
            return ProcessResponse(response);
        }

        [HttpGet("request-metrics")]
        [CheckAuthorize("Permissions.Analytics.View", "Permissions.Analytics.Page")]
        public async Task<IActionResult> GetRequestMetrics()
        {
            var response = await _analyticsService.GetRequestMetricsAsync();
            return ProcessResponse(response);
        }

        [HttpGet("request-trends")]
        [CheckAuthorize("Permissions.Analytics.View", "Permissions.Analytics.Page")]
        public async Task<IActionResult> GetRequestTrends([FromQuery] string period = "daily")
        {
            var response = await _analyticsService.GetRequestTrendsAsync(period);
            return ProcessResponse(response);
        }

        [HttpGet("top-requested-items")]
        [CheckAuthorize("Permissions.Analytics.View", "Permissions.Analytics.Page")]
        public async Task<IActionResult> GetTopRequestedItems([FromQuery] int limit = 10)
        {
            var response = await _analyticsService.GetTopRequestedItemsAsync(limit);
            return ProcessResponse(response);
        }
    }
}
