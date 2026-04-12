using SA.Accounting.WPF.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SA.Accounting.WPF.Commands.Base;

public class UpdateCurrentViewModelCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    private readonly INavigator _navigator;
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;
    public UpdateCurrentViewModelCommand(INavigator navigator,IViewModelAbstractFactory viewModelAbstractFactory)
    {
        _navigator = navigator;
        _viewModelAbstractFactory = viewModelAbstractFactory;
    }
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        if(parameter is ViewType)
        {
            var viewType = (ViewType)parameter;
            _navigator.CurrentViewModel = _viewModelAbstractFactory.CreateViewModel(viewType);
        }
    }
}
