using System.Net;
using Ettad.CrossCutting.Common.Security;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.Monitoring;
using Ettad.Workflows.Service.Monitoring.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Ettad.Workflows.API.Controllers;

[ApiController]
[Route("api/order-auto-reject/countdown")]
public class RequestAutoRejectCountdownController : ApiControllerBase
{
    private static readonly HashSet<string> ValidBulkRequestTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "order",
        "return",
        "discard"
    };

    private readonly IRequestAutoRejectCountdownService _countdownService;

    public RequestAutoRejectCountdownController(IRequestAutoRejectCountdownService countdownService)
    {
        _countdownService = countdownService;
    }

    [ProducesResponseType(typeof(RequestAutoRejectDashboardSummaryDto), (int)HttpStatusCode.OK)]
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

    [ProducesResponseType(typeof(RequestAutoRejectCountdownDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    [HttpGet("{requestId:long}")]
    [CheckAuthorize(
        "Permissions.RequestReciever.Page",
        "Permissions.RequestReciever.View",
        "Permissions.OrderAutoReject.View",
        "dashboard_view")]
    public async Task<IActionResult> GetForRequest(long requestId, CancellationToken cancellationToken)
    {
        var (forbidden, dto) = await _countdownService.GetCountdownAsync(requestId, "order", cancellationToken);
        if (forbidden)
            return Forbid();
        return Ok(dto);
    }

    /// <summary>Comma-separated request ids, max 200. Optional <paramref name="type"/>: order (default), return, discard.</summary>
    [ProducesResponseType(typeof(IReadOnlyList<RequestAutoRejectCountdownDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [HttpGet]
    [CheckAuthorize(
        "Permissions.RequestReciever.Page",
        "Permissions.RequestReciever.View",
        "Permissions.OrderAutoReject.View",
        "dashboard_view")]
    public async Task<IActionResult> GetBulk([FromQuery] string? ids, [FromQuery] string? type = "order", CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(ids))
            return Ok(Array.Empty<RequestAutoRejectCountdownDto>());

        var normalizedType = string.IsNullOrWhiteSpace(type) ? "order" : type.Trim();
        if (!ValidBulkRequestTypes.Contains(normalizedType))
            return BadRequest($"Invalid type. Allowed values: order, return, discard.");

        var idList = ids
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => long.TryParse(s, out var v) ? v : (long?)null)
            .Where(v => v.HasValue)
            .Select(v => v!.Value)
            .Take(RequestAutoRejectCountdownService.MaxBulkRequestIds)
            .ToList();

        var result = await _countdownService.GetBulkAsync(idList, normalizedType, cancellationToken);
        return Ok(result);
    }
}
