using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Commands.Profile;
using SA.Accounting.Application.Commands.User;
using SA.Accounting.Application.Contracts.Profile.Requests;
using SA.Accounting.Application.Queries.Profile;
using SA.Accounting.Core.Extensions;

namespace SA.Accounting.API.Controllers.Account;

[Route("me")]
[ApiController]
[Authorize]
public class AccountController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet()]
    public async Task<IActionResult> GetUserProfile(CancellationToken cancellationToken = default)
    {
        var query = new GetProfileQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("info")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request,CancellationToken cancellationToken = default)
    {
        var command = request.Adapt<UpdateProfileCommand>();
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request,CancellationToken cancellationToken = default)
    {
        var command = request.Adapt<ChangePasswordCommand>();
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }


}
