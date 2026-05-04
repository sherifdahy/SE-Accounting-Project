using SA.Accounting.Core.Entities.Base;
using SA.Accounting.Core.Entities.ExpenseClaims;
using SA.Accounting.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Entities.Custodies;

public class Movement : AuditableEntity
{
    public int Id { get; set; }
    public int CustodyId { get; set; }
    public virtual Custody Custody { get; set; } = default!;
    public DateTime DateTime { get; set; }
    public MovementType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
    public int? ExpenseClaimId { get; set; }
    public virtual ExpenseClaim? ExpenseClaim { get; set; }
}
