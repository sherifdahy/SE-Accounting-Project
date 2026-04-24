using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Interfaces;

public interface IAppNavigationService
{
    void Start();
    void LoginSuccess();
    void Logout();
    void CloseApplication();
}