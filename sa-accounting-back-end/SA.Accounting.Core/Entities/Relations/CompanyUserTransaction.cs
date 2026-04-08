using SA.Accounting.Core.Entities.Companies;
using SA.Accounting.Core.Entities.Identity;
using SA.Accounting.Core.Entities.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Entities.Relations;

public class CompanyUserTransaction
{
    public int CompanyId { get; set; }
    public virtual Company Company { get; set; } = default!;
    public int UserId { get; set; }
    public virtual ApplicationUser User { get; set; } = default!;
    public int TransactionId { get; set; }
    public virtual Transaction Transaction { get; set; } = default!;
}
