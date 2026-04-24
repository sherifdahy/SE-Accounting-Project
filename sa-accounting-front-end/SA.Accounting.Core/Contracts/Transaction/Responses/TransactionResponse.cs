using SA.Accounting.Core.Enums;

namespace SA.Accounting.Core.Contracts.Transaction.Responses;

public record TransactionResponse
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } 
    public DateTime DateTime { get; set; }
    public TransactionState CurrentState { get; set; }
    public string Note { get; set; } = string.Empty;
}
