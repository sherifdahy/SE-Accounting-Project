using SA.Accounting.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SA.Accounting.WPF.Interfaces;

public interface INavigator
{
    ViewModelBase CurrentViewModel { get; set; }
    ICommand UpdateCurrentViewModelCommand { get; }
}

