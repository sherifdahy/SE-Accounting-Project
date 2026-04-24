using Mapster;
using Microsoft.EntityFrameworkCore;
using SA.Accounting.Application.Commands.Transaction;
using SA.Accounting.Application.Contracts.Transaction.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Entities.Transactions;
using SA.Accounting.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.TransactionCommandsHandler;

public class ChangeTransactionStateCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangeTransactionStateCommand, Result<TransactionDetailResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<TransactionDetailResponse>> Handle(ChangeTransactionStateCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.Transactions.FindAsync(
                                                            x => x.Id == request.TransactionId,
                                                            [x => x.Include(t => t.Items)
                                                                   .ThenInclude(i => i.TransactionCategory)
                                                                  .Include(t => t.Items)
                                                                   .ThenInclude(i => i.Company)
                                                                  .Include(t => t.Histories)],
                                                            cancellationToken);

        if (transaction is null)
            return Result.Failure<TransactionDetailResponse>(TransactionErrors.NotFound);

        if (!IsValidTransition(transaction.CurrentState, request.NewState))
            return Result.Failure<TransactionDetailResponse>(TransactionErrors.InvalidStateTransition);

        if (request.NewState == TransactionState.Rejected
            && string.IsNullOrWhiteSpace(request.Note))
            return Result.Failure<TransactionDetailResponse>(TransactionErrors.RejectReasonRequired);

        var oldState = transaction.CurrentState;

        transaction.CurrentState = request.NewState;

        transaction.Histories.Add(new TransactionHistory
        {
            FromState = oldState,
            ToState = request.NewState,
            Note = request.Note
        });

        _unitOfWork.Transactions.Update(transaction);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success(transaction.Adapt<TransactionDetailResponse>());
    }

    private static bool IsValidTransition(TransactionState from, TransactionState to)
    {
        return (from, to) switch
        {
            (TransactionState.Initial, TransactionState.Approved) => true,
            (TransactionState.Initial, TransactionState.Rejected) => true,
            (TransactionState.Approved, TransactionState.Rejected) => true, 
            (TransactionState.Rejected, TransactionState.Initial) => true,
            _ => false
        };
    }
}
