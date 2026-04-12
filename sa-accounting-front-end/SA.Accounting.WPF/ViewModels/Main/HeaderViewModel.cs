using SA.Accounting.WPF.Commands.Auth;
using SA.Accounting.WPF.Commands.Main;
using SA.Accounting.WPF.Interfaces;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels.Main;

public class HeaderViewModel : ViewModelBase
{
    private bool _isUserMenuOpen;
    public IAuthenticator Authenticator { get; }
    public HeaderViewModel(IAuthenticator authenticator, IAppNavigationService appNavigationService)
    {
        Authenticator = authenticator;
        ToggleUserMenuCommand = new ToggleUserMenuCommand(this);
        LogoutCommand = new LogoutCommand(authenticator, appNavigationService);
    }

    public bool IsUserMenuOpen
    {
        get => _isUserMenuOpen;
        set
        {
            _isUserMenuOpen = value;
            OnPropertyChanged(nameof(IsUserMenuOpen));
        }
    }

    public ICommand ToggleUserMenuCommand { get; }
    public ICommand LogoutCommand { get; }
}