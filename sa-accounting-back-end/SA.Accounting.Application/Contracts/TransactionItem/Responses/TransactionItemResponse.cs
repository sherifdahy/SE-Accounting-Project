using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.TransactionItem.Responses;

public record TransactionItemResponse
{
    public int Id { get; set; }
    public string Note { get; set; } = string.Empty;
    public string? FileUrl { get; set; }
    public decimal Amount { get; set; }
    public int TransactionCategoryId { get; set; }
    public string TransactionCategoryName { get; set; } = string.Empty;
    public int CompanyId { get; set; } 
    public string CompanyName { get; set;} = string.Empty;
}
