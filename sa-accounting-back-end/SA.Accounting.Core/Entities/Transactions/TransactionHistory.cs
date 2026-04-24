using SA.Accounting.Core.Entities.Base;
using SA.Accounting.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Entities.Transactions;

public class TransactionHistory : AuditableEntity
{
    public int Id { get; set; }
    public TransactionState FromState { get; set; }
    public TransactionState ToState { get; set; }
    public string? Note { get; set; }

    public int TransactionId { get; set; }
    public virtual Transaction Transaction { get; set; } = default!;
}
