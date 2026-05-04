using Mapster;
using SA.Accounting.Application.Contracts.Custodies.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Application.Queries.Movement;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Infrastructure.Presistance.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.QueriesHandler.MovementQueriesHandler;

public class GetMovementsByCustodyHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetMovementsByCustodyQuery, Result<IReadOnlyList<MovementResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<IReadOnlyList<MovementResponse>>> Handle(GetMovementsByCustodyQuery query,CancellationToken cancellationToken)
    {
        var exists = _unitOfWork.Custodies
            .IsExist(x => x.Id == query.CustodyId);

        if (!exists)
            return Result.Failure<IReadOnlyList<MovementResponse>>(CustodyErrors.NotFound);

        var movements = await _unitOfWork.Movements
            .FindAllAsync(x => x.CustodyId == query.CustodyId, [], cancellationToken);

        return Result.Success<IReadOnlyList<MovementResponse>>(movements.Adapt<List<MovementResponse>>());
    }
}
