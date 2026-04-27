using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Commands.Role;
using SA.Accounting.Application.Contracts.Role.Requests;
using SA.Accounting.Application.Queries.Role;
using SA.Accounting.Core.Extensions;

namespace SA.Accounting.API.Controllers.Role;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RolesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        var query = new GetRolesQuery();
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id,CancellationToken cancellationToken = default)
    {
        var query = new GetRoleQuery() with { Id = id};
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    public async Task<IActionResult> Create(RoleRequest request, CancellationToken cancellationToken = default)
    {
        var command = request.Adapt<CreateRoleCommand>();
        var result = await _mediator.Send(command);
        return result.IsSuccess ? CreatedAtAction(nameof(Get),new { id = result.Value.Id},result.Value) : result.ToProblem();
    }

    [HttpPut("{name}")]
    public async Task<IActionResult> Update([FromRoute] string name, RoleRequest request,CancellationToken cancellationToken)
    {
        var command = request.Adapt<UpdateRoleCommand>() with { Name = name };
        var result = await _mediator.Send(command);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{name}")]
    public async Task<IActionResult> Remove([FromRoute] string name, CancellationToken cancellationToken = default)
    {
        var command = new RemoveRoleCommand() with { Name = name };
        var result = await _mediator.Send(command);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
