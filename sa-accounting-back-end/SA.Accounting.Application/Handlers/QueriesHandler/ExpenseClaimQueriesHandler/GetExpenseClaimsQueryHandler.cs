using Mapster;
using SA.Accounting.Application.Contracts.ExpenseClaims.Responses;
using SA.Accounting.Application.Queries.ExpenseClaim;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Infrastructure.Presistance.Data;

namespace SA.Accounting.Application.Handlers.QueriesHandler.ExpenseClaimQueriesHandler;

public class GetExpenseClaimsHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetExpenseClaimsQuery, Result<IReadOnlyList<ExpenseClaimListItemResponse>>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<IReadOnlyList<ExpenseClaimListItemResponse>>> Handle(GetExpenseClaimsQuery query,CancellationToken cancellationToken)
    {
        var list = (await _unitOfWork.ExpenseClaims
            .GetAllAsync(cancellationToken))
            .Select(x => new ExpenseClaimListItemResponse(
                x.Id,
                x.Number,
                x.ClaimDate,
                x.CurrentState,
                x.UserId,
                x.User.Name,
                x.Items.Sum(i => (decimal?)i.Amount) ?? 0m,
                x.Items.Count,
                x.CreatedAt));

        return Result.Success<IReadOnlyList<ExpenseClaimListItemResponse>>(list.Adapt<IReadOnlyList<ExpenseClaimListItemResponse>>());
    }
}
