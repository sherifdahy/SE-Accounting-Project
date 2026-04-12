using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels;

namespace SA.Accounting.WPF.Factories.Home;

public class HomeViewModelFactory : IViewModelFactory<HomeViewModel>
{
    public HomeViewModel CreateViewModel()
    {
        return new HomeViewModel();
    }
}
