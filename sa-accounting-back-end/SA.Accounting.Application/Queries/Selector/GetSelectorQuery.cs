using SA.Accounting.Application.Contracts.Selector.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.Selector;

public record GetSelectorQuery : IRequest<Result<SelectorResponse>>
{
    public int Id { get; set; }
}
