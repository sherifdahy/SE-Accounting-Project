using Microsoft.EntityFrameworkCore.Metadata;
using SA.Accounting.WPF.Commands;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.Models;
using SA.Accounting.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SA.Accounting.WPF.State.Navigators;

public class Navigator : ObservableObject , INavigator
{
    private ViewModelBase? _currentViewModel;
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;

    public ViewModelBase CurrentViewModel 
    { 
        get { return _currentViewModel!; }
        set 
        { 
            _currentViewModel = value;
            OnPropertyChanged(nameof(CurrentViewModel));
        } 
    }

    public Navigator(IViewModelAbstractFactory viewModelAbstractFactory)
    {
        _viewModelAbstractFactory = viewModelAbstractFactory;
    }
    public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this,_viewModelAbstractFactory);
}
