using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SA.Accounting.Application.Commands.User;
using SA.Accounting.Application.Contracts.Common;
using SA.Accounting.Application.Contracts.User.Requests;
using SA.Accounting.Application.Queries.User;
using SA.Accounting.Core.Extensions;

namespace SA.Accounting.API.Controllers.User;

[Route("api/Users")]
[ApiController]
[Authorize]
public class UserCompaniesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{userId}/companies")]
    public async Task<IActionResult> GetCompanies([FromRoute] int userId,[FromQuery] RequestFilters filters)
    {
        var query = new GetAllUserCompaniesQuery() with { UserId = userId, Filters = filters };
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpPost("{userId}/companies")]
    public async Task<IActionResult> AssignCompanies([FromRoute] int userId,[FromBody] AssignUserCompaniesRequest request)
    {
        var command = new AssignUserCompaniesCommand() with
        {
            UserId = userId,
            CompanyIds = request.CompanyIds
        };
        var result = await _mediator.Send(command);
        return result.IsSuccess ? CreatedAtAction(nameof(GetCompanies), new { userId },null) : result.ToProblem();
    }

    [HttpDelete("{userId}/companies/{companyId}")]
    public async Task<IActionResult> RemoveCompany([FromRoute] int userId,[FromRoute] int companyId)
    {
        var command = new RemoveUserCompanyCommand() with
        {
            UserId = userId,
            CompanyId = companyId
        };
        var result = await _mediator.Send(command);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}