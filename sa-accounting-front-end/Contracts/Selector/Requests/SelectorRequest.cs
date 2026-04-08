using SA.Accounting.WPF.Enums;

namespace SA.Accounting.WPF.Contracts.Selector.Requests;

public class SelectorRequest
{
    public string Value { get; set; } = string.Empty;
    public SelectorContentType ContentType { get; set; }
    public SelectorType Type { get; set; }
    public int Priority { get; set; }
}
