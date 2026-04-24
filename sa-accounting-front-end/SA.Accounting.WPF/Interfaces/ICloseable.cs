using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.Interfaces;

public interface ICloseable
{
    event Action? CloseRequested;
    bool DialogResult { get; }
}
