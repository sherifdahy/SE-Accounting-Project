using SA.Accounting.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Contracts.ExpenseClaims.Responses;

public record ExpenseClaimItemResponse(
    int Id,
    int CompanyId,
    string CompanyName,
    int ExpenseCategoryId,
    string ExpenseCategoryName,
    string Note,
    string? FileUrl,
    decimal Amount,
    ExpenseClaimItemState State,
    string? RejectionReason
);
