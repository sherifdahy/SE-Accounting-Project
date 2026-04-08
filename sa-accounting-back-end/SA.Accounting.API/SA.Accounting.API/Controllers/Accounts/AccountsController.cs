using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Commands.Account;
using SA.Accounting.Application.Contracts.Account.Requests;
using SA.Accounting.Application.Queries.Account;
using SA.Accounting.Core.Extensions;

namespace SA.Accounting.API.Controllers.Accounts;

[Route("api/[controller]")]
[ApiController]
public class AccountsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("/api/companies/{companyId}/[controller]")]
    public async Task<IActionResult> GetAll([FromRoute] int companyId, [FromQuery] bool includeDisabled,  CancellationToken cancellationToken = default)
    {
        var query = new GetAllAccountQuery
        {
            CompanyId = companyId,
            IncludeDisabled = includeDisabled,
        };
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("/api/companies/{companyId}/platforms/{platformId}/[controller]")]
    public async Task<IActionResult> Get([FromRoute] int companyId, [FromRoute] int platformId, CancellationToken cancellationToken = default)
    {
        var query = new GetAccountQuery
        {
            CompanyId = companyId,
            PlatformId = platformId
        };
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("/api/companies/{companyId}/platforms/{platformId}/[controller]")]
    public async Task<IActionResult> Create([FromRoute] int companyId, [FromRoute] int platformId, [FromBody] AccountRequest request, CancellationToken cancellationToken = default)
    {
        var command = request.Adapt<CreateAccountCommand>();
        command.CompanyId = companyId;
        command.PlatformId = platformId;
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(Get), new {  companyId, platformId }, result.Value) : result.ToProblem();
    }

    [HttpPut("/api/companies/{companyId}/platforms/{platformId}/[controller]")]
    public async Task<IActionResult> Update([FromRoute] int companyId, [FromRoute] int platformId, [FromBody] AccountRequest request, CancellationToken cancellationToken = default)
    {
        var command = request.Adapt<UpdateAccountCommand>();
        command.CompanyId = companyId;
        command.PlatfromId = platformId;
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("/api/companies/{companyId}/platforms/{platformId}/[controller]")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int companyId, [FromRoute] int platformId, CancellationToken cancellationToken = default)
    {
        var command = new ToggleStatusAccountCommand
        {
            CompanyId = companyId,
            PlatformId = platformId
        };
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
