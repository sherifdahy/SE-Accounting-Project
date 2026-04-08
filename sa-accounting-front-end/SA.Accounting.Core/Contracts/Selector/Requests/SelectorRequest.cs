using SA.Accounting.Core.Enums;

namespace SA.Accounting.Core.Contracts.Selector.Requests;

public class SelectorRequest
{
    public string Value { get; set; } = string.Empty;
    public SelectorContentType ContentType { get; set; }
    public SelectorType Type { get; set; }
    public int Priority { get; set; }
}

