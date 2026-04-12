using SA.Accounting.WPF.ViewModels;

namespace SA.Accounting.WPF.Interfaces;


public enum ViewType
{
    Login,
    Home,
}
public interface IViewModelAbstractFactory
{
    ViewModelBase CreateViewModel(ViewType viewType);
}
