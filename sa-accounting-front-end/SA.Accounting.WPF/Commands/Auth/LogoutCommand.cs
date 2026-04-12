using SA.Accounting.WPF.Interfaces;
using System.Windows.Input;

namespace SA.Accounting.WPF.Commands.Auth;

public class LogoutCommand : ICommand
{
    private readonly IAuthenticator _authenticator;
    private readonly IAppNavigationService _appNavigationService;

    public event EventHandler? CanExecuteChanged;

    public LogoutCommand(IAuthenticator authenticator, IAppNavigationService appNavigationService)
    {
        _authenticator = authenticator;
        _appNavigationService = appNavigationService;
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        _authenticator.Logout();
        _appNavigationService.Logout();
    }
}