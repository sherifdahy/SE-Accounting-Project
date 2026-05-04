using SA.Accounting.Application.Contracts.ExpenseClaims.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Queries.ExpenseClaim;


public record GetExpenseClaimByIdQuery(int Id)
    : IRequest<Result<ExpenseClaimResponse>>;
