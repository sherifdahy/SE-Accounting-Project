using MediatR;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Commands.ExpenseClaim;
using SA.Accounting.Application.Contracts.ExpenseClaims.Requests;
using SA.Accounting.Application.Contracts.ExpenseClaims.Responses;
using SA.Accounting.Application.Queries.ExpenseClaim;
using SA.Accounting.Core.Extensions;

namespace SA.Accounting.API.Controllers.Expense;

[ApiController]
[Route("api/expense-claims")]
public class ExpenseClaimsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ExpenseClaimResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetExpenseClaimsQuery(), cancellationToken);
        return result.IsSuccess? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExpenseClaimResponse>> GetById(int id,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetExpenseClaimByIdQuery(id), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("{userId}")]
    public async Task<ActionResult<ExpenseClaimResponse>> Create(int userId,[FromBody] ExpenseClaimRequest request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateExpenseClaimCommand(userId, request), cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetById),new { id = result.Value.Id },result.Value): result.ToProblem();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ExpenseClaimResponse>> Update(int id,[FromBody] ExpenseClaimRequest request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateExpenseClaimCommand(id, request), cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPost("{id:int}/submit")]
    public async Task<IActionResult> Submit(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SubmitExpenseClaimCommand(id), cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPost("{id:int}/review")]
    public async Task<IActionResult> Review(int id,[FromBody] ReviewExpenseClaimRequest request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ReviewExpenseClaimCommand(id, request), cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPost("{id:int}/return-for-edit")]
    public async Task<IActionResult> ReturnForEdit(
        int id,
        [FromBody] ReturnExpenseClaimForEditRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ReturnExpenseClaimForEditCommand(id, request), cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CancelExpenseClaimCommand(id), cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPost("{id:int}/settle")]
    public async Task<IActionResult> Settle(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new SettleExpenseClaimCommand(id), cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}