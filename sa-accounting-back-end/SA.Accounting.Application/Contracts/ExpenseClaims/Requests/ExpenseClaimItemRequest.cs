using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.ExpenseClaims.Requests
{
    public class ExpenseClaimItemRequest
    {
        public int? Id { get; set; }
        public int CompanyId { get; set; }
        public int ExpenseCategoryId { get; set; }
        public string Note { get; set; } = string.Empty;
        public string? FileUrl { get; set; }
        public decimal Amount { get; set; }
    }
}
