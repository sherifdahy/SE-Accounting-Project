using Mapster;
using SA.Accounting.Application.Contracts.Transaction.Responses;
using SA.Accounting.Application.Queries.Transaction;
using SA.Accounting.Application.Queries.TransactionCategory;
using SA.Accounting.Core.Abstractions;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Entities.Transactions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SA.Accounting.Application.Handlers.QueriesHandler.TransactionQueriesHandler;

public class GetAllTransactionsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllTransactionsQuery, Result<PaginatedList<TransactionResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<PaginatedList<TransactionResponse>>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Transaction, bool>> query =
            x=> (string.IsNullOrEmpty(request.Filters.SearchValue) || x.Number.Contains(request.Filters.SearchValue));

        var count = await _unitOfWork.Transactions.CountAsync(query);

        var transactions = await _unitOfWork.Transactions.FindAllAsync(query, request.Filters.PageSize * (request.Filters.PageNumber - 1), request.Filters.PageSize, request.Filters.SortColumn, request.Filters.SortDirection, cancellationToken);

        return Result.Success(PaginatedList<TransactionResponse>.Create(transactions.Adapt<List<TransactionResponse>>(), count, request.Filters.PageNumber, request.Filters.PageSize));
    }
}
