using SA.Accounting.Core.Entities.Base;
using SA.Accounting.Core.Entities.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Entities.Transactions;

public class TransactionItem : AuditableEntity
{
    public int Id { get; set; }
    public string Note { get; set; } = string.Empty;
    public string? FileUrl { get; set; }
    public decimal Amount { get; set; }
    public int TransactionId { get; set; }
    public virtual Transaction Transaction { get; set; } = default!;
    public int TransactionCategoryId { get; set; }
    public virtual TransactionCategory TransactionCategory { get; set; } = default!;
    public int CompanyId { get; set; }
    public virtual Company Company { get; set; } = default!;
}
