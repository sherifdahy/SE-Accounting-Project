using SA.Accounting.Core.Entities.Interfaces;
using SA.Accounting.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Services.Services;

public class ExpenseClaimNumberGenerator(IUnitOfWork unitOfWork) : IExpenseClaimNumberGenerator
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<string> GenerateAsync(CancellationToken cancellationToken = default)
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"CUS-{year}-";

        var lastRecord = _unitOfWork.ExpenseClaims
            .Last(x => x.Number.StartsWith(prefix), x => x.Number);

        var nextSequence = 1;
        if (lastRecord is not null && !string.IsNullOrEmpty(lastRecord.Number))
        {
            var sequencePart = lastRecord.Number.Substring(prefix.Length);
            if (int.TryParse(sequencePart, out var lastSequence))
                nextSequence = lastSequence + 1;
        }

        return $"{prefix}{nextSequence:D4}";
    }
}
