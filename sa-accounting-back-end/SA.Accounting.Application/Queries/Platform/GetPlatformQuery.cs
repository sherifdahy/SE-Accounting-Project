using SA.Accounting.Application.Contracts.Platform.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.Platform;

public record GetPlatformQuery : IRequest<Result<PlatformDetailResponse>>
{
    public int Id { get; set; }
}
