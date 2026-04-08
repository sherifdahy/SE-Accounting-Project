using SA.Accounting.Core.Entities.Base;
using SA.Accounting.Core.Entities.Relations;
using SA.Accounting.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Entities.Transactions;

public class Transaction : AuditableEntity
{
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public TransactionState State { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<CompanyUserTransaction> CompanyUserTransaction { get; set; } = new HashSet<CompanyUserTransaction>();
    public virtual ICollection<TransactionItem> TransactionItems { get; set; } = new HashSet<TransactionItem>();
}
