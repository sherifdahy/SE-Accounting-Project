using SA.Accounting.Core.Entities.Base;
using SA.Accounting.Core.Entities.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Entities.Companies;

public class Owner : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public int CompanyId { get; set; }
    public virtual Company Company { get; set; } = default!;
}
