using SA.Accounting.Application.Contracts.Transaction.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.Transaction;

public record GetTransactionQuery : IRequest<Result<TransactionDetailResponse>>
{
    public int Id { get; set; }
}
