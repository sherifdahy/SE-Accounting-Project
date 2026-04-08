using SA.Accounting.WPF.Contracts.User.Requests;

namespace SA.Accounting.WPF.Contracts.User.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .NotEqual(x=>x.CurrentPassword);
        }
    }
}
