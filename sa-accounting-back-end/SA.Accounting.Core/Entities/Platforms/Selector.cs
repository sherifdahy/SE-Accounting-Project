using SA.Accounting.Core.Entities.Base;
using SA.Accounting.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Entities.Platforms;

public class Selector : AuditableEntity
{
    public int Id { get; set; }
    public string Value { get; set; } = string.Empty;
    public SelectorContentType ContentType { get; set; }
    public SelectorType Type { get; set; }
    public int Priority { get; set; }
    public bool IsDeleted { get; set; }
    public int PlatformId { get; set; }
    public virtual Platform Platform { get; set; } = default!;

}
