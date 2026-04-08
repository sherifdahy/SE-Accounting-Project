using SA.Accounting.WPF.Enums;

namespace SA.Accounting.WPF.Contracts.Selector.Responses;

public record SelectorResponse
{
    public int Id { get; set; }
    public string Value { get; set; } = string.Empty;
    public SelectorContentType ContentType { get; set; }
    public SelectorType Type { get; set; }
    public int Priority { get; set; }
    public bool IsDeleted { get; set; }
}
