using SA.Accounting.Application.Contracts.Custodies.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.Movement;

public record GetMovementsByCustodyQuery(int CustodyId)
    : IRequest<Result<IReadOnlyList<MovementResponse>>>;
