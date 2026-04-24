using SA.Accounting.Application.Contracts.Transaction.Responses;
using SA.Accounting.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Transaction;

public record ChangeTransactionStateCommand : IRequest<Result<TransactionDetailResponse>>
{
    public int TransactionId { get; set; }
    public TransactionState NewState { get; set; }
    public string? Note { get; set; }
}
