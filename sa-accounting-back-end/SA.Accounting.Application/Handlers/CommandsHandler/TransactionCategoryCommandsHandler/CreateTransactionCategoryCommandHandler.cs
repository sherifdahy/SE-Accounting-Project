using Mapster;
using SA.Accounting.Application.Commands.TransactionCategory;
using SA.Accounting.Application.Contracts.TransactionCategory.Responses;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Entities.Transactions;

namespace SA.Accounting.Application.Handlers.CommandsHandler.TransactionCategoryCommandsHandler;

public class CreateTransactionCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateTransactionCategoryCommand, Result<TransactionCategoryResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<TransactionCategoryResponse>> Handle(CreateTransactionCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Adapt<TransactionCategory>();

        await _unitOfWork.TransactionCategories.AddAsync(entity);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success(entity.Adapt<TransactionCategoryResponse>());
    }
}
