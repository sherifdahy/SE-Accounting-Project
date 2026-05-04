using SA.Accounting.Application.Commands.ExpenseCategory;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Interfaces;

namespace SA.Accounting.Application.Handlers.CommandsHandler.ExpenseCategoryCommandHandler;

public class ActivateExpenseCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ActivateExpenseCategoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(ActivateExpenseCategoryCommand command,CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.ExpenseCategories
            .FindAsync(x => x.Id == command.Id,[],cancellationToken);

        if (entity is null)
            return Result.Failure(ExpenseCategoryErrors.NotFound);

        if (!entity.IsDisabled)
            return Result.Success(); // idempotent

        entity.IsDisabled = false;
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
