using Mapster;
using Microsoft.EntityFrameworkCore;
using SA.Accounting.Application.Contracts.Transaction.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Application.Queries.Transaction;
using SA.Accounting.Core.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.QueriesHandler.TransactionQueriesHandler;

public class GetTransactionQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetTransactionQuery, Result<TransactionDetailResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<TransactionDetailResponse>> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.Transactions.FindAsync(x => x.Id == request.Id, [x => x.Include(i => i.Items).ThenInclude(e => e.TransactionCategory), x => x.Include(i => i.Histories).ThenInclude(d=>d.CreatedBy), x => x.Include(i => i.Items).ThenInclude(e => e.Company)], cancellationToken);

        if (transaction is null)
            return Result.Failure<TransactionDetailResponse>(TransactionErrors.NotFound);

        var response = transaction.Adapt<TransactionDetailResponse>();

        return Result.Success(response);
    }
}
