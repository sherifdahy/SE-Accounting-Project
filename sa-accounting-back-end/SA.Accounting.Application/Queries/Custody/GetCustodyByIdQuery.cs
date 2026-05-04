using SA.Accounting.Application.Contracts.Custodies.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.Custody;

public record GetCustodyByIdQuery(int Id) : IRequest<Result<CustodyDetailsResponse>>;
