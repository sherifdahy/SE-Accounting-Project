using SA.Accounting.Core.Entities.Base;
using SA.Accounting.Core.Entities.Companies;
using SA.Accounting.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Entities.ExpenseClaims;

public class ExpenseClaimItem : AuditableEntity
{
    public int Id { get; set; }
    public string Note { get; set; } = string.Empty;
    public string? FileUrl { get; set; }
    public decimal Amount { get; set; }
    public ExpenseClaimItemState State { get; set; } = ExpenseClaimItemState.Pending;
    public string? RejectionReason { get; set; }
    public int ExpenseClaimId { get; set; }
    public virtual ExpenseClaim ExpenseClaim { get; set; } = default!;
    public int ExpenseCategoryId { get; set; }
    public virtual ExpenseCategory ExpenseCategory { get; set; } = default!;
    public int CompanyId { get; set; }
    public virtual Company Company { get; set; } = default!;
}