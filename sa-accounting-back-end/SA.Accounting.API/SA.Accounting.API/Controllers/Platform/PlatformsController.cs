using Asp.Versioning;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Commands.Platform;
using SA.Accounting.Application.Contracts.Common;
using SA.Accounting.Application.Contracts.Platform.Requests;
using SA.Accounting.Application.Queries.Platform;
using SA.Accounting.Core.Extensions;

namespace SA.Accounting.API.Controllers.Platform;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class PlatformsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDisabled, CancellationToken cancellationToken = default)
    {
        var query = new GetAllPlatformsQuery();
        query.IncludeDisabled = includeDisabled;
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var query = new GetPlatformQuery();
        query.Id = id;
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlatformRequest request, CancellationToken cancellationToken = default)
    {
        var command = request.Adapt<CreatePlatformCommand>();
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value) : result.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePlatformRequest request, CancellationToken cancellationToken = default)
    {
        var command = request.Adapt<UpdatePlatformCommand>();
        command.Id = id;
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var command = new ToggleStatusPlatformCommand();
        command.Id = id;
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
