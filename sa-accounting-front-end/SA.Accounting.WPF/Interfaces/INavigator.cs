using SA.Accounting.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SA.Accounting.WPF.Interfaces;

public interface INavigator
{
    ViewModelBase CurrentViewModel { get; set; }

    event Action StateChanged;
}

