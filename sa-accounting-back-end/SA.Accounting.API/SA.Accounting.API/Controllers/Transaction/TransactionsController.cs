using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Commands.Transaction;
using SA.Accounting.Application.Contracts.Common;
using SA.Accounting.Application.Contracts.Transaction.Requests;
using SA.Accounting.Application.Queries.Transaction;
using SA.Accounting.Core.Extensions;

namespace SA.Accounting.API.Controllers.Transaction;

[Route("api/[controller]")]
[ApiController]
public class TransactionsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet()]
    public async Task<IActionResult> GetAll([FromQuery]RequestFilters filters,CancellationToken cancellationToken = default)
    {
        var query = new GetAllTransactionsQuery() with { Filters = filters };
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("users/{userId}")]
    public async Task<IActionResult> GetAll([FromRoute]int userId,[FromQuery] RequestFilters filters, CancellationToken cancellationToken = default)
    {
        var query = new GetAllTransactionsForUserQuery() with { UserId = userId ,Filters = filters };
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var query = new GetTransactionQuery() with { Id = id };
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("users/{userId}")]
    public async Task<IActionResult> Create([FromRoute]int userId, CreateTransactionRequest request , CancellationToken cancellationToken = default)
    {
        var query = request.Adapt<CreateTransactionCommand>() with { UserId = userId };
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(Get),new { id = userId },result.Value) : result.ToProblem();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, UpdateTransactionRequest request, CancellationToken cancellationToken = default)
    {
        var query = request.Adapt<UpdateTransactionCommand>() with { Id = id };
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var query = new RemoveTransactionCommand() with { Id = id };
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> ChangeTransactionState([FromRoute] int id,ChangeTransactionStateRequest request, CancellationToken cancellationToken = default)
    {
        var command = request.Adapt<ChangeTransactionStateCommand>() with { TransactionId = id };
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
