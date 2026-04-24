using Mapster;
using SA.Accounting.Core.Contracts.Account.Requests;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.ViewModels.Base;

namespace SA.Accounting.WPF.ViewModels.Account;

public class UpdateAccountViewModel : ValidatableViewModel<UpdateAccountViewModel>
{
    private readonly IValidator<UpdateAccountViewModel> _validator;
    protected override IValidator<UpdateAccountViewModel> Validator => _validator;

    public UpdateAccountViewModel(IValidator<UpdateAccountViewModel> validator)
    {
        _validator = validator;
    }

    private int _id;
    public int Id
    {
        get => _id;
        set { _id = value; OnPropertyChanged(); }
    }

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