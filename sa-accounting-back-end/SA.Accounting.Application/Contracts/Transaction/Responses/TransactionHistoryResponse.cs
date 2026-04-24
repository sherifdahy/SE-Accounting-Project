using SA.Accounting.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Transaction.Responses;

public record TransactionHistoryResponse
{
    public int Id { get; set; }
    public TransactionState FromState { get; set; }
    public TransactionState ToState { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedByName { get; set; } = string.Empty;
}
