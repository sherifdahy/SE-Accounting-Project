using SA.Accounting.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Selector.Requests;

public class SelectorRequest
{
    public string Value { get; set; } = string.Empty;
    public int Priority { get; set; }
    public SelectorContentType ContentType { get; set; }
    public SelectorType Type { get; set; }
}
