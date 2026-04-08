using SA.Accounting.Application.Contracts.Platform.Responses;
using SA.Accounting.Application.Contracts.Selector.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Platform;

public record CreatePlatformCommand : IRequest<Result<PlatformDetailResponse>>
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public List<SelectorRequest> Selectors { get; set; } = [];
}
