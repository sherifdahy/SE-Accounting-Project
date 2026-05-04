using SA.Accounting.Application.Commands.ExpenseCategory;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Infrastructure.Presistance.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.ExpenseCategoryCommandHandler;

public class DeactivateExpenseCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeactivateExpenseCategoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeactivateExpenseCategoryCommand command,CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.ExpenseCategories
            .FindAsync(x => x.Id == command.Id, [], cancellationToken);

        if (entity is null)
            return Result.Failure(ExpenseCategoryErrors.NotFound);

        if (entity.IsDisabled)
            return Result.Failure(ExpenseCategoryErrors.AlreadyInactive);

        entity.IsDisabled = true;
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
