using Microsoft.EntityFrameworkCore;
using SA.Accounting.Application.Commands.Transaction;
using SA.Accounting.Application.Contracts.TransactionItem.Requests;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Entities.Transactions;
using SA.Accounting.Core.Enums;

namespace SA.Accounting.Application.Handlers.CommandsHandler.TransactionCommandsHandler;

public class UpdateTransactionCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateTransactionCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        // 1. Find transaction
        var transaction = await _unitOfWork.Transactions
            .FindAsync(
                x => x.Id == request.Id,
                [x => x.Include(d => d.Items)],
                cancellationToken);

        if (transaction is null)
            return Result.Failure(TransactionErrors.NotFound);

        // 2. Validate state
        if (transaction.CurrentState == TransactionState.Approved)
            return Result.Failure(TransactionErrors.CannotEditApprovedTransaction);

        // 3. Validate references
        var validationResult = await ValidateReferences(request, cancellationToken);
        if (validationResult is not null)
            return validationResult;

        // 4. Handle rejected → re-submit
        HandleRejectedState(transaction);

        // 5. Update transaction fields
        UpdateTransactionFields(transaction, request);

        // 6. Update items (Add / Update / Delete)
        UpdateTransactionItems(transaction, request.Items);

        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }

    private async Task<Result?> ValidateReferences(
        UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var inputCompaniesIds = request.Items.Select(x => x.CompanyId).Distinct();
        var inputCategoriesIds = request.Items.Select(x => x.TransactionCategoryId).Distinct();

        // Validate companies
        var existingCompaniesIds = (await _unitOfWork.Companies
            .FindAllAsync(x => inputCompaniesIds.Contains(x.Id), [], cancellationToken))
            .Select(x => x.Id);

        if (inputCompaniesIds.Except(existingCompaniesIds).Any())
            return Result.Failure(CompanyErrors.NotFound);

        // Validate categories
        var existingCategoriesIds = (await _unitOfWork.TransactionCategories
            .FindAllAsync(x => inputCategoriesIds.Contains(x.Id), [], cancellationToken))
            .Select(x => x.Id);

        if (inputCategoriesIds.Except(existingCategoriesIds).Any())
            return Result.Failure(TransactionCategoryErrors.NotFound);

        return null;
    }

    private static void HandleRejectedState(Transaction transaction)
    {
        if (transaction.CurrentState != TransactionState.Rejected)
            return;

        transaction.CurrentState = TransactionState.Initial;
        transaction.Histories.Add(new TransactionHistory
        {
            FromState = TransactionState.Rejected,
            ToState = TransactionState.Initial,
            Note = "تم التعديل وإعادة التقديم"
        });
    }

    private static void UpdateTransactionFields(Transaction transaction, UpdateTransactionCommand request)
    {
        transaction.Number = request.Number;
        transaction.DateTime = request.DateTime;
        transaction.Note = request.Note;
    }

    private static void UpdateTransactionItems(
        Transaction transaction, List<UpdateTransactionItemRequest> requestItems)
    {
        var requestItemIds = requestItems
            .Where(i => i.Id.HasValue)
            .Select(i => i.Id!.Value)
            .ToHashSet();

        // Delete removed items
        var itemsToDelete = transaction.Items
            .Where(i => !requestItemIds.Contains(i.Id))
            .ToList();

        foreach (var item in itemsToDelete)
            transaction.Items.Remove(item);

        // Add or update items
        foreach (var requestItem in requestItems)
        {
            if (requestItem.Id.HasValue)
            {
                var existingItem = transaction.Items
                    .FirstOrDefault(i => i.Id == requestItem.Id.Value);

                if (existingItem is not null)
                    MapToExistingItem(existingItem, requestItem);
            }
            else
            {
                transaction.Items.Add(MapToNewItem(requestItem));
            }
        }
    }

    private static void MapToExistingItem(TransactionItem item, UpdateTransactionItemRequest request)
    {
        item.Note = request.Note;
        item.FileUrl = request.FileUrl;
        item.Amount = request.Amount;
        item.TransactionCategoryId = request.TransactionCategoryId;
        item.CompanyId = request.CompanyId;
    }

    private static TransactionItem MapToNewItem(UpdateTransactionItemRequest request)
    {
        return new TransactionItem
        {
            Note = request.Note,
            FileUrl = request.FileUrl,
            Amount = request.Amount,
            TransactionCategoryId = request.TransactionCategoryId,
            CompanyId = request.CompanyId
        };
    }
}