using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Core.Entities.Custodies;

public record CustodyBalance(
decimal Balance,
decimal TotalDeposits,
decimal TotalApprovedExpenses,
decimal TotalReturns,
decimal TotalAdjustmentsIn,
decimal TotalAdjustmentsOut
);
