using SA.Accounting.WPF.Interfaces;

namespace SA.Accounting.WPF.ViewModels.Main;

public class MainViewModel : ViewModelBase
{
    public INavigator Navigator { get; set; }
    public MainViewModel(INavigator navigator)
    {
        Navigator = navigator;
        Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Home);
    }
}
