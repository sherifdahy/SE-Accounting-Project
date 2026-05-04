using Mapster;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Infrastructure.Presistance.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.ExpenseCategoryCommandHandler;

public class UpdateExpenseCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateExpenseCategoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(UpdateExpenseCategoryCommand command,CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.ExpenseCategories
            .FindAsync(x => x.Id == command.Id, [], cancellationToken);

        if (entity is null)
            return Result.Failure(ExpenseCategoryErrors.NotFound);

        var duplicate = _unitOfWork.ExpenseCategories
            .IsExist(x => x.Id != command.Id && x.Name == command.Request.Name);

        if (duplicate)
            return Result.Failure(ExpenseCategoryErrors.DuplicateName);

        command.Request.Adapt(entity);

        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
