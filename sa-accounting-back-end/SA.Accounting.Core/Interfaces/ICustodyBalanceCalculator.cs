using SA.Accounting.Core.Entities.Custodies;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Services.Services;

public interface ICustodyBalanceCalculator
{
    Task<CustodyBalance> CalculateAsync(int custodyId, CancellationToken cancellationToken = default);
    Task<decimal> GetBalanceAsync(int custodyId, CancellationToken cancellationToken = default);
}
