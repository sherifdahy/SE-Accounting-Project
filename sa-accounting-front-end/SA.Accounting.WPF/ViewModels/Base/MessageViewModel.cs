using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.ViewModels.Base;

public class MessageViewModel : ViewModelBase
{
    private string _message = string.Empty;
    public string Message { get {return _message; } set { _message = value;OnPropertyChanged();OnPropertyChanged(nameof(HasMessage)); } }
    public bool HasMessage => !string.IsNullOrEmpty(Message);

}
