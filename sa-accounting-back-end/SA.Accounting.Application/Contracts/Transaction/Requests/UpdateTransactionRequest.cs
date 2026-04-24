using SA.Accounting.Application.Contracts.TransactionItem.Requests;
using SA.Accounting.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Transaction.Requests;

public record UpdateTransactionRequest
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public string Note { get; set; } = string.Empty;
    public List<UpdateTransactionItemRequest> Items { get; set; } = [];
}
