using SA.Accounting.Application.Contracts.Selector.Responses;

namespace SA.Accounting.Application.Queries.Selector;

public record GetAllSelectorsQuery : IRequest<Result<List<SelectorResponse>>>
{
    public int PlatformId { get; set; }
    public bool? IncludeDisabled { get; set; }
}