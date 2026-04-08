using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Commands.User;
using SA.Accounting.Application.Contracts.Common;
using SA.Accounting.Application.Contracts.User.Requests;
using SA.Accounting.Application.Queries.User;
using SA.Accounting.Core.Extensions;

namespace SA.Accounting.API.Controllers.User;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]RequestFilters filters,[FromQuery]bool includeDisabled = false)
    {
        var query = new GetAllUsersQuery() with { IncludeDisabled = includeDisabled , Filters = filters};
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute]int id)
    {
        var query = new GetUserQuery() with { UserId = id };
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody]CreateUserRequest request)
    {
        var command = request.Adapt<CreateUserCommand>();
        var result = await _mediator.Send(command);
        return result.IsSuccess ? CreatedAtAction(nameof(Get), new { result.Value.Id },null) : result.ToProblem();
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> Update([FromRoute] int userId, [FromBody] UpdateUserRequest request)
    {
        var command = request.Adapt<UpdateUserCommand>() with { UserId = userId };
        var result = await _mediator.Send(command);  
        return result.IsSuccess ? NoContent() : result.ToProblem(); 
    }

    [HttpPatch("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int id)
    {
        var command = new ToggleStatusUserCommand() with { UserId = id };
        var result = await _mediator.Send(command);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
