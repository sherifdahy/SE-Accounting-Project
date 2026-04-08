using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Selector;

public record UpdateSelectorCommand : IRequest<Result>
{
    public int Id { get; set; }
    public byte ContentType { get; set; }
    public byte Type { get; set; }
    public string Value { get; set; } = string.Empty;
    public int Priority { get; set; }
}
