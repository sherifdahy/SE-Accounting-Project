using SA.Accounting.Application.Commands.Custody;
using SA.Accounting.Application.Contracts.Custodies.Responses;
using SA.Accounting.Application.Errors;
using SA.Accounting.Core.Entities.Custodies;
using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Enums;
using SA.Accounting.Infrastructure.Presistance.Data;
using SA.Accounting.Services.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Handlers.CommandsHandler.MovementCommandsHandler;
public class CreateMovementHandler(IUnitOfWork unitOfWork,ICustodyBalanceCalculator balanceCalculator) : IRequestHandler<CreateMovementCommand, Result<MovementResponse>>
{
    private readonly ICustodyBalanceCalculator _balanceCalculator = balanceCalculator;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<MovementResponse>> Handle(CreateMovementCommand command,CancellationToken cancellationToken)
    {
        var request = command.Request;

        // 1. الـ ApprovedExpense ممنوع manually
        if (request.Type == MovementType.ApprovedExpense)
            return Result.Failure<MovementResponse>(CustodyErrors.InvalidMovementType);

        // 2. الـ Custody موجودة و Active
        var custody = await _unitOfWork.Custodies
            .FindAsync(x => x.Id == command.CustodyId, [], cancellationToken);

        if (custody is null)
            return Result.Failure<MovementResponse>(CustodyErrors.NotFound);

        if (!custody.IsActive)
            return Result.Failure<MovementResponse>(CustodyErrors.NotActive);

        // 3. لو outflow, نتأكد إن الرصيد كافي
        if (IsOutflow(request.Type))
        {
            var currentBalance = await _balanceCalculator.GetBalanceAsync(
                custody.Id, cancellationToken);

            if (currentBalance < request.Amount)
                return Result.Failure<MovementResponse>(CustodyErrors.InsufficientBalance);
        }

        // 4. إنشاء الـ Movement
        var movement = new Movement
        {
            CustodyId = custody.Id,
            DateTime = DateTime.UtcNow,
            Type = request.Type,
            Amount = request.Amount,
            Note = request.Note,
            ExpenseClaimId = null
        };

        await _unitOfWork.Movements.AddAsync(movement, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        var response = new MovementResponse(
            movement.Id,
            movement.CustodyId,
            movement.DateTime,
            movement.Type,
            movement.Amount,
            movement.Note,
            movement.ExpenseClaimId,
            movement.CreatedAt);

        return Result.Success(response);
    }

    private static bool IsOutflow(MovementType type) =>
        type is MovementType.Return or MovementType.AdjustmentOut;
}
