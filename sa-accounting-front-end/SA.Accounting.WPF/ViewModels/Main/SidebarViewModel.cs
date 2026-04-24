using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Factories;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.State.Navigators;
using SA.Accounting.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels.Main;

public class SidebarViewModel : ViewModelBase
{
    private readonly IAuthenticator _authenticator;
    private readonly IViewModelAbstractFactory _ViewModelAbstractFactory;
    private readonly INavigator _navigator;
    public ViewModelBase CurrentViewModel => _navigator.CurrentViewModel;
    public ICommand UpdateCurrentViewModel { get; }
    public List<string> UserPermissions { get; }
    public SidebarViewModel(IAuthenticator authenticator,INavigator navigator,IViewModelAbstractFactory viewModelAbstractFactory)
    {
        _authenticator = authenticator;
        _navigator = navigator;
        _ViewModelAbstractFactory = viewModelAbstractFactory;

        UserPermissions = _authenticator.CurrentUserResponse!.Permissions;

        UpdateCurrentViewModel = new UpdateCurrentViewModelCommand(navigator, _ViewModelAbstractFactory);
        UpdateCurrentViewModel.Execute(ViewType.Home);

        _navigator.StateChanged += Navigator_Chaned;
    }

    private void Navigator_Chaned()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}
