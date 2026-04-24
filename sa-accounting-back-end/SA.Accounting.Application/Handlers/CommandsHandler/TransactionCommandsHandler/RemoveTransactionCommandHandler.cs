using Microsoft.EntityFrameworkCore;
using SA.Accounting.Application.Commands.Transaction;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Interfaces;

namespace SA.Accounting.Application.Handlers.CommandsHandler.TransactionCommandsHandler;

public class RemoveTransactionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RemoveTransactionCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(RemoveTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.Transactions.FindAsync(x => x.Id == request.Id, [x=>x.Include(i=>i.Items).ThenInclude(e=>e.TransactionCategory)], cancellationToken);

        if (transaction is null)
            return Result.Failure(TransactionErrors.NotFound);

        _unitOfWork.TransactionItems.DeleteRange(transaction.Items);

        _unitOfWork.Transactions.Delete(transaction);

        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
