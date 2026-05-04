using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Commands.Company;
using SA.Accounting.Application.Commands.ExpenseCategory;
using SA.Accounting.Application.Contracts.ExpenseCategories.Requests;
using SA.Accounting.Application.Contracts.ExpenseCategories.Responses;
using SA.Accounting.Application.Queries.ExpenseCategory;
using SA.Accounting.Core.Extensions;

namespace SA.Accounting.API.Controllers.Expense;

[ApiController]
[Route("api/expense-categories")]
public class ExpenseCategoriesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ExpenseCategoryResponse>>> GetAll(bool includeDisabled = false,CancellationToken cancellationToken=default)
    {
        var query = new GetExpenseCategoriesQuery(includeDisabled);
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExpenseCategoryResponse>> GetById(int id,CancellationToken cancellationToken)
    {
        var query = new GetExpenseCategoryByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost]
    public async Task<ActionResult<ExpenseCategoryResponse>> Create([FromBody] ExpenseCategoryRequest request,CancellationToken cancellationToken = default)
    {
        var command = new CreateExpenseCategoryCommand(request);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(GetById),new {id = result.Value.Id}, result.Value) : result.ToProblem();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ExpenseCategoryResponse>> Update(int id,[FromBody] ExpenseCategoryRequest request,CancellationToken cancellationToken = default)
    {
        var command = new UpdateExpenseCategoryCommand(id,request);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken = default)
    {
        var command = new DeleteExpenseCategoryCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPatch("{id:int}/deactivate")]
    public async Task<IActionResult> Deactivate(int id, CancellationToken cancellationToken = default)
    {
        var command = new DeactivateExpenseCategoryCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPatch("{id:int}/activate")]
    public async Task<IActionResult> Activate(int id, CancellationToken cancellationToken = default)
    {
        var command = new ActivateExpenseCategoryCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
