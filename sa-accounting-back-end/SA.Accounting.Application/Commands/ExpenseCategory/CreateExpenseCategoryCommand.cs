using SA.Accounting.Application.Contracts.ExpenseCategories.Requests;
using SA.Accounting.Application.Contracts.ExpenseCategories.Responses;

namespace SA.Accounting.Application.Commands.ExpenseCategory;
public record CreateExpenseCategoryCommand(ExpenseCategoryRequest Request) : IRequest<Result<ExpenseCategoryResponse>>;
