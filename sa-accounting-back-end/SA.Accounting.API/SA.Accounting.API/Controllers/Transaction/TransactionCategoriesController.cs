using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Commands.TransactionCategory;
using SA.Accounting.Application.Contracts.TransactionCategory.Requests;
using SA.Accounting.Application.Queries.TransactionCategory;
using SA.Accounting.Core.Extensions;

namespace SA.Accounting.API.Controllers.Transaction;

[Route("api/[controller]")]
[ApiController]
public class TransactionCategoriesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool? includeDisabled, CancellationToken cancellationToken = default)
    {
        var query = new GetAllTransactionCategoriesQuery() with { IncludeDisabled = includeDisabled};
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id,CancellationToken cancellationToken = default)
    {
        var query = new GetTransactionCategoryQuery() with {Id = id};
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TransactionCategoryRequest request,CancellationToken cancellationToken = default)
    {
        var command = request.Adapt<CreateTransactionCategoryCommand>();
        var result = await _mediator.Send(command);
        return result.IsSuccess ? CreatedAtAction(nameof(Get), new { result.Value.Id } ,result.Value ) : result.ToProblem();
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute]int id,TransactionCategoryRequest request,CancellationToken cancellationToken = default)
    {
        var command = request.Adapt<UpdateTransactionCategoryCommand>() with { Id = id };
        var result = await _mediator.Send(command);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpDelete("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus(int id, CancellationToken cancellationToken = default)
    {
        var command = new ToggleStatusTransactionCategoryCommand() with { Id = id };
        var result = await _mediator.Send(command);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
