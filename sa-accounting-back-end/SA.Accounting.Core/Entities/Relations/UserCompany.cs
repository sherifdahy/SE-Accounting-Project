using SA.Accounting.Core.Entities.Companies;
using SA.Accounting.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Entities.Relations;

public class UserCompany
{
    public int CompanyId { get; set; }
    public virtual Company Company { get; set; } = default!;
    public int UserId { get; set; }
    public virtual ApplicationUser User { get; set; } = default!;
}
