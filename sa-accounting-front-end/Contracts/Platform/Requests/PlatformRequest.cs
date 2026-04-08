using SA.Accounting.WPF.Contracts.Selector.Requests;

namespace SA.Accounting.WPF.Contracts.Platform.Requests;

public record PlatformRequest
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public List<SelectorRequest> Selectors { get; set; } = [];
}
