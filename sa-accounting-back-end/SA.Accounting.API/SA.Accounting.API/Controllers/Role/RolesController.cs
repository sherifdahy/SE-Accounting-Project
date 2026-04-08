using MediatR;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Queries.Role;
using SA.Accounting.Core.Extensions;

namespace SA.Accounting.API.Controllers.Role;

[Route("api/[controller]")]
[ApiController]
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


}
