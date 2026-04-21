using System.Linq;
using System.Net;
using Ettad.CrossCutting.Common.Security;
using Ettad.ResponseHandler.Models;
using Ettad.Workflows.Service.Settings;
using Ettad.Workflows.Service.Settings.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Ettad.Workflows.API.Controllers;

[ApiController]
[Route("api/order-auto-reject-settings")]
public class OrderAutoRejectSettingsController : ApiControllerBase
{
    private readonly IOrderAutoRejectSettingsService _service;

    public OrderAutoRejectSettingsController(IOrderAutoRejectSettingsService service)
    {
        _service = service;
    }

    [ProducesResponseType(typeof(OrderAutoRejectSettingsDto), (int)HttpStatusCode.OK)]
    [HttpGet]
    [CheckAuthorize("Permissions.OrderAutoRejectSettings.Page")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        return Ok(await _service.GetAsync(cancellationToken));
    }

    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [HttpPut]
    [CheckAuthorize("Permissions.OrderAutoRejectSettings.Edit")]
    public async Task<IActionResult> Put([FromBody] UpdateOrderAutoRejectSettingsDto dto, CancellationToken cancellationToken)
    {
        try
        {
            await _service.UpdateAsync(dto, cancellationToken);
            return Ok();
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
        }
    }
}
