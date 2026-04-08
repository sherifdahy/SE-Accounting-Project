using Mapster;
using SA.Accounting.Application.Contracts.Selector.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Application.Queries.Selector;
using SA.Accounting.Core.Entities.Interfaces;

namespace SA.Accounting.Application.Handlers.QueriesHandler.SelectorQueriesHandler;

public class GetSelectorQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetSelectorQuery, Result<SelectorResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<SelectorResponse>> Handle(GetSelectorQuery request, CancellationToken cancellationToken)
    {
        var selector = await _unitOfWork.Selectors.GetByIdAsync(request.Id,cancellationToken);

        if (selector == null)
            return Result.Failure<SelectorResponse>(SelectorErrors.NotFound);

        return Result.Success(selector.Adapt<SelectorResponse>());
    }
}
