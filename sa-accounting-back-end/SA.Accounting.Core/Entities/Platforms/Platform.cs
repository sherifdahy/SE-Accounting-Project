using SA.Accounting.Core.Entities.Base;
using SA.Accounting.Core.Entities.Companies;
using SA.Accounting.Core.Entities.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Entities.Platforms;

public class Platform : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public virtual ICollection<Account> Accounts { get; set; } = new HashSet<Account>();
    public virtual ICollection<Selector> Selectors { get; set; } = new HashSet<Selector>();
}
