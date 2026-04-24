using SA.Accounting.WPF.Commands.Auth;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Commands.Main;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels.Main;

public class HeaderViewModel : ViewModelBase
{
    private bool _isUserMenuOpen;
    private readonly INavigator _navigator;
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;

    public IAuthenticator Authenticator { get; }
    public HeaderViewModel(IAuthenticator authenticator,INavigator navigator, IAppNavigationService appNavigationService,IViewModelAbstractFactory viewModelAbstractFactory)
    {
        Authenticator = authenticator;
        _navigator = navigator;
        _viewModelAbstractFactory = viewModelAbstractFactory;
        ToggleUserMenuCommand = new ToggleUserMenuCommand(this);
        LogoutCommand = new LogoutCommand(authenticator, appNavigationService);
        NavigateToProfileCommand = new AsyncRelayCommand(_ => NavigateTo(ViewType.Profile));
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
    public ICommand NavigateToProfileCommand { get; }
    public ICommand LogoutCommand { get; }

    private async Task NavigateTo(ViewType viewType)
    {
        var vm = _viewModelAbstractFactory.CreateViewModel(viewType);

        if (vm is IAsyncInitializable initializable)
            await initializable.InitializeAsync();

        _navigator.CurrentViewModel = vm;
    }

}