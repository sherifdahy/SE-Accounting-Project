using Microsoft.EntityFrameworkCore;
using SA.Accounting.Application.Commands.ExpenseClaim;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.ExpenseClaims;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Enums;
using SA.Accounting.Infrastructure.Presistance.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.ExpenseClaimCommandsHandler;

public class SubmitExpenseClaimHandler(IUnitOfWork unitOfWork) : IRequestHandler<SubmitExpenseClaimCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(SubmitExpenseClaimCommand command,CancellationToken cancellationToken)
    {
        var claim = await _unitOfWork.ExpenseClaims
            .FindAsync(x => x.Id == command.Id, [x=>x.Include(d=>d.Items)],cancellationToken);

        if (claim is null)
            return Result.Failure(ExpenseClaimErrors.NotFound);

        // 1. State check
        if (claim.CurrentState != ExpenseClaimState.Draft &&
            claim.CurrentState != ExpenseClaimState.ReturnedForEdit)
        {
            return Result.Failure(ExpenseClaimErrors.CannotSubmit);
        }

        // 2. Must have items
        if (claim.Items.Count == 0)
            return Result.Failure(ExpenseClaimErrors.EmptyItems);

        var fromState = claim.CurrentState;

        // 3. Reset items to Pending (في حالة الـ ReturnedForEdit)
        foreach (var item in claim.Items)
        {
            item.State = ExpenseClaimItemState.Pending;
            item.RejectionReason = null;
        }

        // 4. Transition state
        claim.CurrentState = ExpenseClaimState.Submitted;

        // 5. Add history
        claim.Histories.Add(new ExpenseClaimHistory
        {
            ExpenseClaimId = claim.Id,
            FromState = fromState,
            ToState = ExpenseClaimState.Submitted,
            Note = "Claim submitted for review."
        });

        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
