using SA.Accounting.Application.Contracts.Custodies.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.Custody;

public record GetCustodiesQuery(bool? IsActive = null, int? UserId = null)
    : IRequest<Result<IReadOnlyList<CustodyResponse>>>;
