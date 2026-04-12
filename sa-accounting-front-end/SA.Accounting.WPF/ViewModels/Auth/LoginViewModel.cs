using SA.Accounting.Core.Contracts.Auth.Requests;
using SA.Accounting.WPF.Commands.Auth;
using SA.Accounting.WPF.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;
    private readonly IAuthenticator _authenticator;
    private readonly IAppNavigationService _appNavigationService;
    private string _email;
    private string _password;

    public LoginViewModel(IDialogService dialogService,IAppNavigationService appNavigationService, IAuthenticator authenticator)
    {
        _appNavigationService = appNavigationService;
        _dialogService = dialogService;
        _authenticator = authenticator;
    }
    public string Email { get { return _email; } set { _email = value; } }
    public string Password { get { return _password; } set { _password = value; } }
    public ICommand LoginCommand => new LoginCommand(this,_dialogService,_authenticator,_appNavigationService);
}
