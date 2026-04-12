using SA.Accounting.WPF.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.ViewModels.Auth;

public class AuthWindowViewModel
{
    public INavigator Navigator { get; set; }
    public AuthWindowViewModel(INavigator navigator)
    {
        Navigator = navigator;
        navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Login);
    }
}
