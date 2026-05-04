using SA.Accounting.Application.Contracts.Custodies.Responses;
using SA.Accounting.Application.Queries.Custody;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Enums;

namespace SA.Accounting.Application.Handlers.QueriesHandler.CustodyQueriesHandler;

public class GetCustodiesHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCustodiesQuery, Result<IReadOnlyList<CustodyResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<CustodyResponse>>> Handle(GetCustodiesQuery query,CancellationToken cancellationToken)
    {
        //var q = _unitOfWork.Custodies.GetAllAsync(cancellationToken);

        //if (query.IsActive.HasValue)
        //    q = q.Where(x => x.IsActive == query.IsActive.Value);

        //if (query.UserId.HasValue)
        //    q = q.Where(x => x.UserId == query.UserId.Value);

        //var list = await q
        //    .OrderByDescending(x => x.CreatedAt)
        //    .Select(x => new CustodyResponse(
        //        x.Id,
        //        x.Number,
        //        x.UserId,
        //        (x.User.FirstName + " " + x.User.LastName).Trim(),
        //        x.IsActive,
        //        x.Movements
        //            .Where(m => m.Type == MovementType.Deposit
        //                     || m.Type == MovementType.AdjustmentIn)
        //            .Sum(m => (decimal?)m.Amount) ?? 0m
        //        -
        //        x.Movements
        //            .Where(m => m.Type == MovementType.ApprovedExpense
        //                     || m.Type == MovementType.Return
        //                     || m.Type == MovementType.AdjustmentOut)
        //            .Sum(m => (decimal?)m.Amount) ?? 0m,
        //        x.CreatedAt))
        //    .ToListAsync(cancellationToken);

        return Result.Success<IReadOnlyList<CustodyResponse>>([]);
    }
}
