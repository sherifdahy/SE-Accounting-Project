using Microsoft.AspNetCore.Mvc.RazorPages;
using SA.Accounting.Application.Contracts.Selector.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Selector;

public record CreateSelectorCommand : IRequest<Result<SelectorResponse>>
{
    public int PlatformId { get; set; }
    public byte ContentType { get; set; }
    public byte Type { get; set; }
    public string Value { get; set; } = string.Empty;
    public int Priority { get; set; }
}
