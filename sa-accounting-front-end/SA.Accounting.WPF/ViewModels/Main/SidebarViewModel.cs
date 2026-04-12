using SA.Accounting.WPF.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.ViewModels.Main;

public class SidebarViewModel : ViewModelBase
{
    private readonly IAuthenticator _authenticator;
    public SidebarViewModel(IAuthenticator authenticator)
    {
        _authenticator = authenticator;
    }
    public List<string> UserPermissions => _authenticator.CurrentUserResponse!.Permissions;
}
