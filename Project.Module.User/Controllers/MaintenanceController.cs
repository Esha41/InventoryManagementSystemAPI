using System.Net;
using System.Threading.Tasks;
using Ettad.CrossCutting.Common.Security;
using Ettad.ResponseHandler.Models;
using Ettad.User.Services.DTO;
using Ettad.User.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ettad.User.API.Controllers;

/// <summary>
/// API for managing application maintenance mode.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MaintenanceController : ApiControllerBase
{
    private readonly IMaintenanceService _maintenanceService;

    public MaintenanceController(IMaintenanceService maintenanceService)
    {
        _maintenanceService = maintenanceService ?? throw new System.ArgumentNullException(nameof(maintenanceService));
    }

    /// <summary>
    /// Gets the current maintenance mode status. Public - no authentication required.
    /// </summary>
    [HttpGet("status")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(APIOperationResponse<MaintenanceStatusDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetStatus()
    {
        var result = await _maintenanceService.GetStatusAsync();
        return ProcessResponse(result);
    }

    /// <summary>
    /// Enables or disables maintenance mode. Requires admin dashboard permission.
    /// </summary>
    [HttpPut("status")]
    [Authorize]
    [CheckAuthorize("Permissions.AdminDashboard.Page", "Permissions.AdminDashboard.View")]
    [ProducesResponseType(typeof(APIOperationResponse<bool>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> SetStatus([FromBody] MaintenanceStatusDto dto)
    {
        var result = await _maintenanceService.SetEnabledAsync(dto.IsEnabled);
        return ProcessResponse(result);
    }
}
