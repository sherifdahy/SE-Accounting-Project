using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.ExpenseClaims.Requests;

public class ExpenseClaimRequest
{
    public DateTime ClaimDate { get; set; }
    public string? Note { get; set; }
    public List<ExpenseClaimItemRequest> Items { get; set; } = new();
}
