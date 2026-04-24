using Mapster;
using SA.Accounting.Application.Commands.TransactionCategory;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Interfaces;

namespace SA.Accounting.Application.Handlers.CommandsHandler.TransactionCategoryCommandsHandler;

public class ToggleStatusTransactionCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ToggleStatusTransactionCategoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(ToggleStatusTransactionCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.TransactionCategories.FindAsync(x => x.Id == request.Id, [], cancellationToken); ;

        if (entity is null)
            return Result.Failure(TransactionCategoryErrors.NotFound);

        entity.IsDeleted = !entity.IsDeleted;

        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
