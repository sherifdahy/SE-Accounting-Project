using Microsoft.EntityFrameworkCore.Metadata;
using SA.Accounting.WPF.Commands;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.Models;
using SA.Accounting.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SA.Accounting.WPF.State.Navigators;

public class Navigator : INavigator
{
    private ViewModelBase? _currentViewModel;
    public ViewModelBase CurrentViewModel
    {
        get { return _currentViewModel!; }
        set
        {
            _currentViewModel = value;
            StateChanged?.Invoke();
        }
    }

    public event Action StateChanged;
}
