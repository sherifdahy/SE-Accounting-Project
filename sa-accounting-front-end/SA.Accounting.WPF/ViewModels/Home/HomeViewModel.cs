using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.ViewModels;

public class HomeViewModel : ViewModelBase
{
    public override ViewType Section => ViewType.Home;
}
