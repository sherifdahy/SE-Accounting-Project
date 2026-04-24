using SA.Accounting.WPF.Commands.Auth;
using SA.Accounting.WPF.Interfaces;
using System.Windows.Input;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.ViewModels.Base;

namespace SA.Accounting.WPF.ViewModels;

public class LoginViewModel : ValidatableViewModel<LoginViewModel>
{
    private readonly IDialogService _dialogService;
    private readonly IAuthenticator _authenticator;
    private readonly IAppNavigationService _appNavigationService;
    private readonly IValidator<LoginViewModel> _validator;

    public LoginViewModel(IDialogService dialogService,IAppNavigationService appNavigationService,IAuthenticator authenticator,IValidator<LoginViewModel> validator)
    {
        _dialogService = dialogService;
        _appNavigationService = appNavigationService;
        _authenticator = authenticator;
        _validator = validator;

        LoginCommand = new LoginCommand(this, _dialogService, _authenticator, _appNavigationService);

        ErrorsChanged += (_, __) =>
        {
            OnPropertyChanged(nameof(CanExecute));
        };
    }

    protected override IValidator<LoginViewModel> Validator => _validator;

    private string _email = string.Empty;
    private string _password = string.Empty;

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
            ValidateProperty(nameof(Email));
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
            ValidateProperty(nameof(Password));
        }
    }

    public bool CanExecute => !HasErrors;
    public ICommand LoginCommand { get; }
    public MessageViewModel ErrorMessage { get; } = new();
}