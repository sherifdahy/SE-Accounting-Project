using SA.Accounting.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.ExpenseClaims.Requests;

public class ReviewExpenseClaimItemRequest
{
    public int ExpenseClaimItemId { get; set; }
    public ExpenseClaimItemState State { get; set; }
    public string? RejectionReason { get; set; }
}
