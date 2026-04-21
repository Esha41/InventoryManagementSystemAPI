using System.Net;
using Ettad.CrossCutting.Common.Security;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.Monitoring;
using Ettad.Workflows.Service.Monitoring.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Ettad.Workflows.API.Controllers;

[ApiController]
[Route("api/order-auto-reject/countdown")]
public class OrderAutoRejectCountdownController : ApiControllerBase
{
    private readonly IOrderAutoRejectCountdownService _countdownService;

    public OrderAutoRejectCountdownController(IOrderAutoRejectCountdownService countdownService)
    {
        _countdownService = countdownService;
    }

    [ProducesResponseType(typeof(OrderAutoRejectDashboardSummaryDto), (int)HttpStatusCode.OK)]
    [HttpGet("dashboard-summary")]
    [CheckAuthorize(
        "Permissions.RequestReciever.Page",
        "Permissions.RequestReciever.View",
        "Permissions.OrderAutoReject.View",
        "dashboard_view")]
    public async Task<IActionResult> GetDashboardSummary(CancellationToken cancellationToken)
    {
        return Ok(await _countdownService.GetDashboardSummaryAsync(cancellationToken));
    }

    [ProducesResponseType(typeof(OrderAutoRejectCountdownDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [HttpGet("{requestId:long}")]
    [CheckAuthorize(
        "Permissions.RequestReciever.Page",
        "Permissions.RequestReciever.View",
        "Permissions.OrderAutoReject.View",
        "dashboard_view")]
    public async Task<IActionResult> GetForRequest(long requestId, CancellationToken cancellationToken)
    {
        var (forbidden, dto) = await _countdownService.GetForRequestAsync(requestId, cancellationToken);
        if (forbidden)
            return Forbid();
        return Ok(dto);
    }

    /// <summary>Comma-separated request ids, max 200.</summary>
    [ProducesResponseType(typeof(IReadOnlyList<OrderAutoRejectCountdownDto>), (int)HttpStatusCode.OK)]
    [HttpGet]
    [CheckAuthorize(
        "Permissions.RequestReciever.Page",
        "Permissions.RequestReciever.View",
        "Permissions.OrderAutoReject.View",
        "dashboard_view")]
    public async Task<IActionResult> GetBulk([FromQuery] string? ids, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(ids))
            return Ok(Array.Empty<OrderAutoRejectCountdownDto>());

        var idList = ids
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => long.TryParse(s, out var v) ? v : (long?)null)
            .Where(v => v.HasValue)
            .Select(v => v!.Value)
            .Take(OrderAutoRejectCountdownService.MaxBulkRequestIds)
            .ToList();

        var result = await _countdownService.GetBulkAsync(idList, cancellationToken);
        return Ok(result);
    }
}
