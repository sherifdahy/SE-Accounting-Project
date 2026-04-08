using SA.Accounting.WPF.Contracts.Selector.Responses;

namespace SA.Accounting.WPF.Contracts.Platform.Responses;

public record PlatformDetailResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public List<SelectorResponse> Selectors { get; set; } = [];
}
