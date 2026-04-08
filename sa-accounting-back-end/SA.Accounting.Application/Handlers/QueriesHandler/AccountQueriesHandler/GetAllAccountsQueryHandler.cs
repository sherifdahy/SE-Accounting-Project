using Mapster;
using Microsoft.EntityFrameworkCore;
using SA.Accounting.Application.Contracts.Account.Responses;
using SA.Accounting.Application.Contracts.Platform.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Application.Queries.Account;
using SA.Accounting.Core.Entities.Companies;
using SA.Accounting.Core.Entities.Interfaces;
using System.Linq.Expressions;

namespace SA.Accounting.Application.Handlers.QueriesHandler.AccountQueriesHandler;

public class GetAllAccountsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllAccountQuery, Result<List<AccountResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<List<AccountResponse>>> Handle(GetAllAccountQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Company, bool>> query = x => (x.Id == request.CompanyId) ;

        var company = await _unitOfWork.Companies.FindAsync(query, [x=>x.Include(s=>s.Accounts.Where(x=> (request.IncludeDisabled == true || x.IsDeleted == false))).ThenInclude(s=>s.Platform)],cancellationToken);

        if (company == null)
            return Result.Failure<List<AccountResponse>>(CompanyErrors.NotFound);

        var response = company.Accounts.Adapt<List<AccountResponse>>();

        return Result.Success(response);
    }
}
