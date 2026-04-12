using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Factories.Auth;

public class LoginViewModelFactory : IViewModelFactory<LoginViewModel>
{
    private readonly IDialogService _dialogService;
    private readonly IAppNavigationService _appNavigationService;
    private readonly IAuthenticator _authenticator;

    public LoginViewModelFactory(IDialogService dialogService,IAppNavigationService appNavigationService, IAuthenticator authenticator)
    {
        _dialogService = dialogService;
        _appNavigationService = appNavigationService;
        _authenticator = authenticator;
    }
    public LoginViewModel CreateViewModel()
    {
        return new LoginViewModel(_dialogService,_appNavigationService,_authenticator);
    }
}
