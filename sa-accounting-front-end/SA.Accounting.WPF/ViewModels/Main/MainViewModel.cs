using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Commands.Main;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels.Main;

public class MainViewModel : ViewModelBase
{
    private readonly INavigator _navigator;
    private readonly IAppNavigationService _appNavigationService;

    public SidebarViewModel SidebarViewModel { get; set; }
    public HeaderViewModel HeaderViewModel { get; set; }
    public ViewModelBase CurrentViewModel => _navigator.CurrentViewModel;
    public ICommand CloseCommand { get; }
    public MainViewModel(INavigator navigator,SidebarViewModel sidebarViewModel,HeaderViewModel headerViewModel,IAppNavigationService appNavigationService)
    {
        SidebarViewModel = sidebarViewModel;
        HeaderViewModel = headerViewModel;
        _appNavigationService = appNavigationService;
        _navigator = navigator;

        _navigator.StateChanged += Navigator_StateChanged;

        CloseCommand = new RelayCommand((_) => Close());
    }

    private void Navigator_StateChanged()
    {
        OnPropertyChanged(nameof(CurrentViewModel));
    }

    private void Close()
    {
        _appNavigationService.CloseApplication();
    }
}
