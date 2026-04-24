using SA.Accounting.Core.Contracts.TransactionItem.Requests;
using SA.Accounting.Core.Enums;

namespace SA.Accounting.Core.Contracts.Transaction.Requests;

public record CreateTransactionRequest
{
    public string Number { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public string Note { get; set; } = string.Empty;
    public List<CreateTransactionItemRequest> Items { get; set; } = [];
}
