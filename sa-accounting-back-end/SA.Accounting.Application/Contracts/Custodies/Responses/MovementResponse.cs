using SA.Accounting.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.Custodies.Responses;

public record MovementResponse(
    int Id,
    int CustodyId,
    DateTime DateTime,
    MovementType Type,
    decimal Amount,
    string? Note,
    int? ExpenseClaimId,
    DateTime CreatedAt
);