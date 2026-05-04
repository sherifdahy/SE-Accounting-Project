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

public class ReviewExpenseClaimHandler(IUnitOfWork unitOfWork) : IRequestHandler<ReviewExpenseClaimCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result> Handle(ReviewExpenseClaimCommand command,CancellationToken cancellationToken)
    {
        var request = command.Request;

        var claim = await _unitOfWork.ExpenseClaims
            .FindAsync(x => x.Id == command.Id, [d=>d.Include(i=>i.Items)],cancellationToken);

        if (claim is null)
            return Result.Failure(ExpenseClaimErrors.NotFound);

        // 1. State check
        if (claim.CurrentState != ExpenseClaimState.Submitted)
            return Result.Failure(ExpenseClaimErrors.CannotReview);

        // 2. No duplicate items in request
        var requestItemIds = request.Items.Select(i => i.ExpenseClaimItemId).ToList();
        if (requestItemIds.Count != requestItemIds.Distinct().Count())
            return Result.Failure(ExpenseClaimErrors.DuplicateItemReview);

        // 3. All claim items must be reviewed (no missing)
        var claimItemIds = claim.Items.Select(i => i.Id).ToHashSet();
        var requestItemIdSet = requestItemIds.ToHashSet();

        if (!claimItemIds.SetEquals(requestItemIdSet))
            return Result.Failure(ExpenseClaimErrors.ReviewMustCoverAllItems);

        // 4. Validate each item state
        foreach (var itemReq in request.Items)
        {
            if (itemReq.State != ExpenseClaimItemState.Approved &&
                itemReq.State != ExpenseClaimItemState.Rejected)
            {
                return Result.Failure(ExpenseClaimErrors.InvalidItemState);
            }

            if (itemReq.State == ExpenseClaimItemState.Rejected
                && string.IsNullOrWhiteSpace(itemReq.RejectionReason))
            {
                return Result.Failure(ExpenseClaimErrors.RejectionReasonRequired);
            }
        }

        // 5. Apply review to each item
        var reviewDict = request.Items.ToDictionary(i => i.ExpenseClaimItemId);
        foreach (var item in claim.Items)
        {
            var review = reviewDict[item.Id];
            item.State = review.State;
            item.RejectionReason = review.State == ExpenseClaimItemState.Rejected
                ? review.RejectionReason
                : null;
        }

        // 6. Calculate final claim state
        var approvedCount = claim.Items.Count(i => i.State == ExpenseClaimItemState.Approved);
        var rejectedCount = claim.Items.Count(i => i.State == ExpenseClaimItemState.Rejected);
        var totalCount = claim.Items.Count;

        var finalState = (approvedCount, rejectedCount) switch
        {
            var (a, _) when a == totalCount => ExpenseClaimState.Approved,
            var (_, r) when r == totalCount => ExpenseClaimState.Rejected,
            _ => ExpenseClaimState.PartiallyApproved
        };

        var fromState = claim.CurrentState;
        claim.CurrentState = finalState;

        // 7. Add history
        claim.Histories.Add(new ExpenseClaimHistory
        {
            ExpenseClaimId = claim.Id,
            FromState = fromState,
            ToState = finalState,
            Note = string.IsNullOrWhiteSpace(request.Note)
                ? $"Reviewed: {approvedCount} approved, {rejectedCount} rejected."
                : request.Note
        });

        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
