using SA.Accounting.Application.Contracts.Custodies.Requests;
using SA.Accounting.Application.Contracts.Custodies.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Custody;

public record CreateMovementCommand(int CustodyId, CreateMovementRequest Request)
    : IRequest<Result<MovementResponse>>;
