using SA.Accounting.Application.Contracts.ExpenseCategories.Responses;

namespace SA.Accounting.Application.Queries.ExpenseCategory;

public record GetExpenseCategoriesQuery(bool? IncludeDisabled = null) : IRequest<Result<IReadOnlyList<ExpenseCategoryResponse>>>;
