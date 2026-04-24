using SA.Accounting.Application.Contracts.Transaction.Responses;
using SA.Accounting.Application.Contracts.TransactionItem.Requests;
using SA.Accounting.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Transaction;

public record CreateTransactionCommand : IRequest<Result<TransactionDetailResponse>>
{
    public int UserId { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public string Note { get; set; } = string.Empty;
    public List<CreateTransactionItemRequest> Items { get; set; } = [];
}
