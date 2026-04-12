using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels;

namespace SA.Accounting.WPF.Factories;

public class ViewModelAbstractFactory : IViewModelAbstractFactory
{
    private readonly IViewModelFactory<HomeViewModel> _homeViewModelFactory;
    private readonly IViewModelFactory<LoginViewModel> _loginViewModelFactory;
    public ViewModelAbstractFactory(
        IViewModelFactory<LoginViewModel> loginViewModelFactory,
        IViewModelFactory<HomeViewModel> homeViewModelFactory)
    {
        _homeViewModelFactory = homeViewModelFactory;
        _loginViewModelFactory = loginViewModelFactory;
    }
    public ViewModelBase CreateViewModel(ViewType viewType)
    {
        switch (viewType)
        {
            case ViewType.Login:
                return _loginViewModelFactory.CreateViewModel();
            case ViewType.Home:
                return _homeViewModelFactory.CreateViewModel();
            default:
                throw new ArgumentException("Invalid view type");
        }
    }
}
