using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Interfaces;

public interface IAppNavigationService
{
    void Start();
    void LoginSucceeded();
    void Logout();
}