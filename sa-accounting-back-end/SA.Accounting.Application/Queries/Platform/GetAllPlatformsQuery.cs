using SA.Accounting.Application.Contracts.Common;
using SA.Accounting.Application.Contracts.Platform.Responses;
using SA.Accounting.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.Platform;

public record GetAllPlatformsQuery : IRequest<Result<List<PlatformResponse>>>
{
    public bool? IncludeDisabled { get; set; }
}
