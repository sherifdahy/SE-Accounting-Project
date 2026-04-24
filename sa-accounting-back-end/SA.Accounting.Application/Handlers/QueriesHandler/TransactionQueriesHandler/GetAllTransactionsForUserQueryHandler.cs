using Mapster;
using Microsoft.AspNetCore.Identity;
using SA.Accounting.Application.Contracts.Transaction.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Application.Queries.Transaction;
using SA.Accounting.Core.Abstractions;
using SA.Accounting.Core.Entities.Identity;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Entities.Transactions;
using System.Linq.Expressions;

namespace SA.Accounting.Application.Handlers.QueriesHandler.TransactionQueriesHandler;

public class GetAllTransactionsForUserQueryHandler(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager) : IRequestHandler<GetAllTransactionsForUserQuery, Result<PaginatedList<TransactionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result<PaginatedList<TransactionResponse>>> Handle(GetAllTransactionsForUserQuery request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByIdAsync(request.UserId.ToString()) is not ApplicationUser)
            return Result.Failure<PaginatedList<TransactionResponse>>(UserErrors.NotFound);

        Expression<Func<Transaction, bool>> query = 
            x=>x.UserId == request.UserId && (string.IsNullOrEmpty(request.Filters.SearchValue) || x.Number.Contains(request.Filters.SearchValue));

        var count = await _unitOfWork.Transactions.CountAsync(query);

        var transactions = await _unitOfWork.Transactions.FindAllAsync(query, request.Filters.PageSize * (request.Filters.PageNumber - 1), request.Filters.PageSize, request.Filters.SortColumn, request.Filters.SortDirection, cancellationToken);

        return Result.Success(PaginatedList<TransactionResponse>.Create(transactions.Adapt<List<TransactionResponse>>(), count, request.Filters.PageNumber, request.Filters.PageSize));
    }
}
