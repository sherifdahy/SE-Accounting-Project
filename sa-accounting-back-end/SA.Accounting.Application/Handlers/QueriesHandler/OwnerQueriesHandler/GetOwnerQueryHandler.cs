using Mapster;
using SA.Accounting.Application.Contracts.Owner.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Application.Queries.Owner;
using SA.Accounting.Core.Entities.Interfaces;

namespace SA.Accounting.Application.Handlers.QueriesHandler.OwnerQueriesHandler;

public class GetOwnerQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetOwnerQuary, Result<OwnerResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<OwnerResponse>> Handle(GetOwnerQuary request, CancellationToken cancellationToken)
    {
        var owner = await _unitOfWork.Owners.GetByIdAsync(request.Id,cancellationToken);

        if (owner == null)
            return Result.Failure<OwnerResponse>(OwnerErrors.NotFound);

        return Result.Success<OwnerResponse>(owner.Adapt<OwnerResponse>());
    }
}
