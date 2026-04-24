using Mapster;
using SA.Accounting.Application.Commands.TransactionCategory;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.TransactionCategoryCommandsHandler;

public class UpdateTransactionCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateTransactionCategoryCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(UpdateTransactionCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.TransactionCategories.FindAsync(x => x.Id == request.Id, [], cancellationToken); ;

        if (entity is null)
            return Result.Failure(TransactionCategoryErrors.NotFound);

        request.Adapt(entity);

        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
