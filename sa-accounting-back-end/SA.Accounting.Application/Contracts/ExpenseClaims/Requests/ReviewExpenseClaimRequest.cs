using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.ExpenseClaims.Requests;


public class ReviewExpenseClaimRequest
{
    public string? Note { get; set; }
    public List<ReviewExpenseClaimItemRequest> Items { get; set; } = new();
}
