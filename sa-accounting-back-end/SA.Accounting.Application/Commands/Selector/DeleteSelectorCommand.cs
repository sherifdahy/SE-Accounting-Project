using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Selector;

public record DeleteSelectorCommand : IRequest<Result>
{
    public int Id { get; set; }
}
