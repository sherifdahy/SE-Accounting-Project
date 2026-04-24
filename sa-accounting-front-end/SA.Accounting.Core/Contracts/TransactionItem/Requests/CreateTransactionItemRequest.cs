namespace SA.Accounting.Core.Contracts.TransactionItem.Requests;

public record CreateTransactionItemRequest
{
    public string Note { get; set; } = string.Empty;
    public string? FileUrl { get; set; }
    public decimal Amount { get; set; }
    public int TransactionCategoryId { get; set; }
    public int CompanyId { get; set; }
}
