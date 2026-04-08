using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Interfaces;

public interface IPermissionService
{
    Task<bool> HasPermissionAsync(string permission);
    Task<string> GetUserEmailAsync();
}