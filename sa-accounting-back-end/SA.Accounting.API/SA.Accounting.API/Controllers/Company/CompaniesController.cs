using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Commands.Company;
using SA.Accounting.Application.Commands.Owner;
using SA.Accounting.Application.Contracts.Common;
using SA.Accounting.Application.Contracts.Company.Requests;
using SA.Accounting.Application.Queries.Company;
using SA.Accounting.Core.Extensions;

namespace SA.Accounting.API.Controllers.Company;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CompaniesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDisabled,[FromQuery] RequestFilters filters,CancellationToken cancellationToken = default)
    {
        var query = new GetAllCompaniesQuery();
        query.IncludeDisabled = includeDisabled;
        query.Filters = filters;
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var query = new GetCompanyQuary();
        query.Id = id;
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CompanyRequest request, CancellationToken cancellationToken = default)
    {
        var command = request.Adapt<CreateCompanyCommand>();
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value) : result.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id,[FromBody] UpdateCompanyRequest request, CancellationToken cancellationToken = default)
    {
        var command = request.Adapt<UpdateCompanyCommand>();
        command.Id = id;
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var command = new ToggleStatusCompanyCommand();
        command.Id = id;
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
