using SA.Accounting.Application.Contracts.ExpenseCategories.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.Application.Commands.Company;

public record UpdateExpenseCategoryCommand(int Id, ExpenseCategoryRequest Request) : IRequest<Result>;
