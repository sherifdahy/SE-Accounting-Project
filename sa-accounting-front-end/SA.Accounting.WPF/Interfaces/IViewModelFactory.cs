using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Interfaces;

public interface IViewModelFactory<T> where T : class 
{
    T CreateViewModel();
}
