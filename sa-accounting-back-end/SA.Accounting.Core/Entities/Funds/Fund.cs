using SA.Accounting.Core.Entities.Base;
using SA.Accounting.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Entities.Funds;

public class Fund : AuditableEntity
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime DateTime { get; set; }
    public string? Note { get; set; }

    public int UserId { get; set; }
    public virtual ApplicationUser User { get; set; } = default!;
}
