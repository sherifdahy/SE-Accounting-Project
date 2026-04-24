using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Interfaces;

public interface IAsyncInitializable<in T>
{
    Task InitializeAsync(T param);
}

public interface IAsyncInitializable
{
    Task InitializeAsync();
}
