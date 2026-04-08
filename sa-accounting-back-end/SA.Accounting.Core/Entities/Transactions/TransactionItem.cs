using SA.Accounting.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Entities.Transactions;

public class TransactionItem : AuditableEntity
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public string Note { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public bool IsDeleted { get; set; }
    public int TransactionId { get; set; }
    public virtual Transaction Transaction { get; set; } = default!;
    public int TransactionCategoryId { get; set; }
    public virtual TransactionCategory TransactionCategory { get; set; } = default!;
}
