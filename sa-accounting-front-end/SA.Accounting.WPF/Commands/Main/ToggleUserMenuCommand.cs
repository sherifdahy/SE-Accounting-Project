using SA.Accounting.WPF.ViewModels.Main;
using System.Windows.Input;

namespace SA.Accounting.WPF.Commands.Main;

public class ToggleUserMenuCommand : ICommand
{
    private readonly HeaderViewModel _headerViewModel;

    public ToggleUserMenuCommand(HeaderViewModel headerViewModel)
    {
        _headerViewModel = headerViewModel;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        _headerViewModel.IsUserMenuOpen = !_headerViewModel.IsUserMenuOpen;
    }
}