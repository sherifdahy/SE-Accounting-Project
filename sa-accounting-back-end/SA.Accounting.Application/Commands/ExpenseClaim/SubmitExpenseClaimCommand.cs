using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.ExpenseClaim;

public record SubmitExpenseClaimCommand(int Id) : IRequest<Result>;