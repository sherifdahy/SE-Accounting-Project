using SA.Accounting.Application.Contracts.TransactionItem.Requests;

namespace SA.Accounting.Application.Contracts.TransactionItem.Validators;

public class UpdateTransactionItemRequestValidator : AbstractValidator<UpdateTransactionItemRequest>
{
    public UpdateTransactionItemRequestValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.TransactionCategoryId)
            .GreaterThan(0);

        RuleFor(x => x.CompanyId)
            .GreaterThan(0);
    }
}
