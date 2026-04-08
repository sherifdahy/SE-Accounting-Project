using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Commands.Profile;
using SA.Accounting.Application.Commands.User;
using SA.Accounting.Core.Extensions;

namespace SA.Accounting.API.Controllers;

[Route("me")]
[ApiController]
[Authorize]
public class AccountController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet(GetUserProfileCommand.Route)]
    public async Task<IActionResult> GetUserProfile(CancellationToken cancellationToken = default)
    {
        var command = new GetUserProfileCommand();
        var result = await _mediator.Send(command,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut(UpdateProfileCommand.Route)]
    public async Task<IActionResult> UpdateProfile(UpdateProfileCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut(ChangePasswordCommand.Route)]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


}
