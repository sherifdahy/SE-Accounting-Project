using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels.Auth;

public class AuthWindowViewModel
{
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;
    private readonly IAppNavigationService _appNavigationService;

    public ICommand UpdateCurrentViewModel { get; }
    public ICommand CloseCommand { get; }
    public INavigator Navigator { get; set; }
    public AuthWindowViewModel(INavigator navigator,IViewModelAbstractFactory viewModelAbstractFactory,IAppNavigationService appNavigationService)
    {
        _viewModelAbstractFactory = viewModelAbstractFactory;
        _appNavigationService = appNavigationService;
        Navigator = navigator;

        UpdateCurrentViewModel = new UpdateCurrentViewModelCommand(navigator, _viewModelAbstractFactory);

        UpdateCurrentViewModel.Execute(ViewType.Login);

        CloseCommand = new RelayCommand((_) => Close());
    }

    private void Close()
    {
        _appNavigationService.CloseApplication();
    }
}
