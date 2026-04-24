using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.Models;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SA.Accounting.WPF.ViewModels.Base;

public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : ViewModelBase;
public abstract class ViewModelBase : INotifyPropertyChanged
{
    public virtual ViewType Section => ViewType.None;

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
