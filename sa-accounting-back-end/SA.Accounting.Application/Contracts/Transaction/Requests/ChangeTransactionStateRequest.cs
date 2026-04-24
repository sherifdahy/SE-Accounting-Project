using SA.Accounting.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Transaction.Requests;

public class ChangeTransactionStateRequest
{
    public TransactionState NewState { get; set; }
    public string? Note { get; set; }
}
