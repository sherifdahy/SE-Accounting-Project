using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Commands.Custody;
using SA.Accounting.Application.Contracts.Custodies.Requests;
using SA.Accounting.Application.Contracts.Custodies.Responses;
using SA.Accounting.Application.Queries.Custody;
using SA.Accounting.Application.Queries.Movement;
using SA.Accounting.Core.Extensions;

namespace SA.Accounting.API.Controllers.Expense;

[ApiController]
[Route("api/custodies")]
public class CustodiesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool? isActive,[FromQuery] int? userId,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCustodiesQuery(isActive, userId), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCustodyById(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCustodyByIdQuery(id), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustodyRequest request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateCustodyCommand(request), cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetCustodyById),new { id = result.Value.Id },result.Value) : result.ToProblem();
    }

    [HttpPatch("{id:int}/close")]
    public async Task<IActionResult> Close(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CloseCustodyCommand(id), cancellationToken);
        return result.IsSuccess? NoContent() : result.ToProblem();
    }

    [HttpGet("{custodyId:int}/movements")]
    public async Task<IActionResult> GetMovements(
        int custodyId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMovementsByCustodyQuery(custodyId), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("{custodyId:int}/movements")]
    public async Task<IActionResult> CreateMovement(int custodyId,[FromBody] CreateMovementRequest request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateMovementCommand(custodyId, request), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}