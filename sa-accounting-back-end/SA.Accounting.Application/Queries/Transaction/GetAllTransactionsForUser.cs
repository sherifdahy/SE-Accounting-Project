using SA.Accounting.Application.Contracts.Common;
using SA.Accounting.Application.Contracts.Transaction.Responses;
using SA.Accounting.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.Transaction;

public record GetAllTransactionsForUserQuery : IRequest<Result<PaginatedList<TransactionResponse>>>
{
    public int UserId { get; set; }
    public RequestFilters Filters { get; set; } = default!;
}
