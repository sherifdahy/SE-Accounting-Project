using Mapster;
using SA.Accounting.Core.Contracts.Account.Requests;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.ViewModels.Base;

namespace SA.Accounting.WPF.ViewModels.Account;

public class CreateAccountViewModel : ValidatableViewModel<CreateAccountViewModel>
{
    private readonly IValidator<CreateAccountViewModel> _validator;
    protected override IValidator<CreateAccountViewModel> Validator => _validator;

    public CreateAccountViewModel(IValidator<CreateAccountViewModel> validator)
    {
        _validator = validator;
    }

    // ══════ Properties ══════
    private string _email = string.Empty;
    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(); ValidateProperty(); }
    }

    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); ValidateProperty(); }
    }

    private int _platformId;
    public int PlatformId
    {
        get => _platformId;
        set { _platformId = value; OnPropertyChanged(); ValidateProperty(); }
    }
}