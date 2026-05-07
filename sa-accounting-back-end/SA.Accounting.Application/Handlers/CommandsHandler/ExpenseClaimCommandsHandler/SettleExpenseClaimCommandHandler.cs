using Microsoft.EntityFrameworkCore;
using SA.Accounting.Application.Commands.ExpenseClaim;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Custodies;
using SA.Accounting.Core.Entities.ExpenseClaims;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Enums;
using SA.Accounting.Infrastructure.Presistance.Data;
using SA.Accounting.Services.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.ExpenseClaimCommandsHandler;

public class SettleExpenseClaimHandler(IUnitOfWork unitOfWork,ICustodyBalanceCalculator custodyBalanceCalculator) : IRequestHandler<SettleExpenseClaimCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICustodyBalanceCalculator _custodyBalanceCalculator = custodyBalanceCalculator;

    public async Task<Result> Handle(SettleExpenseClaimCommand command,CancellationToken cancellationToken)
    {
        // 1. Load claim with items
        var claim = await _unitOfWork.ExpenseClaims
            .FindAsync(x => x.Id == command.Id, [d=>d.Include(i=>i.Items)], cancellationToken);

        if (claim is null)
            return Result.Failure(ExpenseClaimErrors.NotFound);

        // 2. State check
        if (claim.CurrentState != ExpenseClaimState.Approved &&
            claim.CurrentState != ExpenseClaimState.PartiallyApproved)
        {
            return Result.Failure(ExpenseClaimErrors.CannotSettle);
        }

        // 3. Idempotency check (defense in depth - DB has unique index too)
        var alreadySettled = _unitOfWork.CustodyMovements.IsExist(x => x.ExpenseClaimId == claim.Id && x.Type == MovementType.ApprovedExpense);

        if (alreadySettled)
            return Result.Failure(ExpenseClaimErrors.AlreadySettled);

        // 4. Calculate approved amount
        var approvedItems = claim.Items
            .Where(i => i.State == ExpenseClaimItemState.Approved)
            .ToList();

        if (approvedItems.Count == 0)
            return Result.Failure(ExpenseClaimErrors.NoApprovedItems);

        var approvedAmount = approvedItems.Sum(i => i.Amount);

        // 5. Get user's active custody
        var custody = await _unitOfWork.Custodies.FindAsync(x => x.UserId == claim.UserId && !x.IsDisabled,[],cancellationToken);

        if (custody is null)
            return Result.Failure(ExpenseClaimErrors.NoActiveCustody);

        // 6. Check balance
        var currentBalance = await _custodyBalanceCalculator.GetBalanceAsync(custody.Id, cancellationToken);

        if (currentBalance < approvedAmount)
            return Result.Failure(ExpenseClaimErrors.InsufficientCustodyBalance);

        // 7. Create the Movement (ApprovedExpense)
        var movement = new CustodyMovement
        {
            CustodyId = custody.Id,
            ExpenseClaimId = claim.Id,
            Type = MovementType.ApprovedExpense,
            Amount = approvedAmount,
            Note = $"Settlement for claim {claim.Number}"
        };

        await _unitOfWork.CustodyMovements.AddAsync(movement, cancellationToken);

        // 8. Transition state
        var fromState = claim.CurrentState;
        claim.CurrentState = ExpenseClaimState.Settled;

        // 9. Add history
        claim.Histories.Add(new ExpenseClaimHistory
        {
            ExpenseClaimId = claim.Id,
            FromState = fromState,
            ToState = ExpenseClaimState.Settled,
            Note = $"Settled {approvedAmount:F2} from custody {custody.Number}."
        });

        // 10. Save (single transaction - all or nothing)
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
