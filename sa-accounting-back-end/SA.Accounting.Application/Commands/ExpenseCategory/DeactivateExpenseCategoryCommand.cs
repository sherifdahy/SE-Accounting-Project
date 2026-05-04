using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.ExpenseCategory;

public record DeactivateExpenseCategoryCommand(int Id) : IRequest<Result>;