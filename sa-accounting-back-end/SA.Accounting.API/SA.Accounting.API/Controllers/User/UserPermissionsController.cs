using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Commands.User;
using SA.Accounting.Application.Contracts.PermissionOverride.Requests;
using SA.Accounting.Application.Queries.User;
using SA.Accounting.Core.Extensions;

namespace SA.Accounting.API.Controllers.User;

[Route("api/users")]
[ApiController]
public class UserPermissionsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{userId}/permissions")]
    public async Task<IActionResult> GetDeniedPermissions([FromRoute] int userId, CancellationToken cancellationToken = default)
    {
        var query = new GetUserPermissionOverridesQuery(userId);
        var result = await _mediator.Send(query,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("{userId}/permissions")]
    public async Task<IActionResult> Update([FromRoute] int userId,UpdateUserPermissionOverridesRequest request,CancellationToken cancellationToken = default)
    {
        var command = new UpdatePermissionOverridesCommand(userId, request.DeniedPermissions);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
