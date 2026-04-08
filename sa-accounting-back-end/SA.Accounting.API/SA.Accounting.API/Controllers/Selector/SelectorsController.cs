using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Commands.Selector;
using SA.Accounting.Application.Contracts.Selector.Requests;
using SA.Accounting.Application.Queries.Selector;
using SA.Accounting.Core.Extensions;

namespace SA.Accounting.API.Controllers.Selector;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SelectorsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("/api/platforms/{platformId}/[controller]")]
    public async Task<IActionResult> GetAll([FromRoute] int platformId, [FromQuery] bool includeDisabled, CancellationToken cancellationToken = default)
    {
        var query = new GetAllSelectorsQuery
        {
            PlatformId = platformId,
            IncludeDisabled = includeDisabled,
        };
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var query = new GetSelectorQuery
        {
            Id = id,
        };
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("/api/platforms/{platformId}/[controller]")]
    public async Task<IActionResult> Create([FromRoute] int platformId, [FromBody] SelectorRequest request, CancellationToken cancellationToken = default)
    {
        var command = request.Adapt<CreateSelectorCommand>();
        command.PlatformId = platformId;
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = result.Value.Id}, result.Value) : result.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] SelectorRequest request, CancellationToken cancellationToken = default)
    {
        var command = request.Adapt<UpdateSelectorCommand>();
        command.Id = id;
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var command = new DeleteSelectorCommand
        {
            Id = id
        };
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
