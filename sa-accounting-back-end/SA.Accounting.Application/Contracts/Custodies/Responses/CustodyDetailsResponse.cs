using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Custodies.Responses;

public record CustodyDetailsResponse(
    int Id,
    string Number,
    int UserId,
    string UserFullName,
    bool IsActive,
    decimal Balance,
    decimal TotalDeposits,
    decimal TotalApprovedExpenses,
    decimal TotalReturns,
    decimal TotalAdjustmentsIn,
    decimal TotalAdjustmentsOut,
    string? Note,
    DateTime CreatedAt
);