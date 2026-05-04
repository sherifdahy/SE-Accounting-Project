using SA.Accounting.Application.Contracts.ExpenseClaims.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.ExpenseClaim;

public record ReviewExpenseClaimCommand(int Id, ReviewExpenseClaimRequest Request)
    : IRequest<Result>;
