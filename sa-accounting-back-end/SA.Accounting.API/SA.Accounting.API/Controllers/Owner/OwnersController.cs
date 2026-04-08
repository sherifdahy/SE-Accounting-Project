using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Commands.Owner;
using SA.Accounting.Application.Contracts.Common;
using SA.Accounting.Application.Contracts.Owner.Requests;
using SA.Accounting.Application.Queries.Owner;
using SA.Accounting.Core.Extensions;

namespace SA.Accounting.API.Controllers.Owner;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OwnersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("/api/companies/{companyId}/owners")]
    public async Task<IActionResult> GetAll([FromRoute] int companyId, [FromQuery] bool includeDisabled, [FromQuery] RequestFilters filters, CancellationToken cancellationToken = default)
    {
        var query = new GetAllOwnersQuery();
        query.CompanyId = companyId;
        query.IncludeDisabled = includeDisabled;
        query.Filters = filters;
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var query = new GetOwnerQuary();
        query.Id = id;
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("/api/companies/{companyId}/[controller]")]
    public async Task<IActionResult> Create([FromRoute] int companyId, [FromBody] OwnerRequest request, CancellationToken cancellationToken = default)
    {
        var command = request.Adapt<CreateOwnerCommand>();
        command.CompanyId = companyId;
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value) : result.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] OwnerRequest request, CancellationToken cancellationToken = default)
    {
        var command = request.Adapt<UpdateOwnerCommand>();
        command.Id = id;
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var command = new ToggleStatusOwnerCommand();
        command.Id = id;
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
