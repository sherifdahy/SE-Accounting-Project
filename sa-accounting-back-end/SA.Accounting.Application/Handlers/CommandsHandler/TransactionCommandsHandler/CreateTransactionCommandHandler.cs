using Mapster;
using Microsoft.AspNetCore.Identity;
using SA.Accounting.Application.Commands.Transaction;
using SA.Accounting.Application.Contracts.Transaction.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Identity;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Entities.Transactions;
using SA.Accounting.Core.Enums;

namespace SA.Accounting.Application.Handlers.CommandsHandler.TransactionCommandsHandler;

public class CreateTransactionCommandHandler(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager) : IRequestHandler<CreateTransactionCommand, Result<TransactionDetailResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Result<TransactionDetailResponse>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByIdAsync(request.UserId.ToString()) is not ApplicationUser)
            return Result.Failure<TransactionDetailResponse>(UserErrors.NotFound);

        var inputCompaniesIds = request.Items.Select(x => x.CompanyId).Distinct();
        var inputTransactionCategoriesIds = request.Items.Select(x => x.TransactionCategoryId).Distinct();

        var existingCompaniesIds = (await _unitOfWork.Companies
        .FindAllAsync(x => inputCompaniesIds.Contains(x.Id), [], cancellationToken))
        .Select(x => x.Id);

        var missingCompaniesIds = inputCompaniesIds.Except(existingCompaniesIds);

        if (missingCompaniesIds.Any())
            return Result.Failure<TransactionDetailResponse>(CompanyErrors.NotFound);


        var existingCategoriesIds = (await _unitOfWork.TransactionCategories
                .FindAllAsync(x => inputTransactionCategoriesIds.Contains(x.Id), [], cancellationToken))
                .Select(x => x.Id);

        var missingCategoriesIds = inputTransactionCategoriesIds.Except(existingCategoriesIds);

        if (missingCategoriesIds.Any())
            return Result.Failure<TransactionDetailResponse>(TransactionCategoryErrors.NotFound);

        var entity = request.Adapt<Transaction>();

        entity.CurrentState = TransactionState.Initial;

        entity.Histories.Add(new TransactionHistory
        {
            FromState = TransactionState.Initial,
            ToState = TransactionState.Initial,
            Note = "تم إنشاء المعاملة"
        });

        await _unitOfWork.Transactions.AddAsync(entity,cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success(entity.Adapt<TransactionDetailResponse>());
    }
}
