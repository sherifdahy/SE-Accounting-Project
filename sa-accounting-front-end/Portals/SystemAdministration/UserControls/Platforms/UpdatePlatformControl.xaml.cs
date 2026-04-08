using SA.Accounting.WPF.ViewModels.Platform;
using System.Windows;
using System.Windows.Controls;

namespace SA.Accounting.WPF.Portals.SystemAdministration.UserControls.Platforms;

public partial class UpdatePlatformControl : UserControl
{
    private readonly UpdatePlatformViewModel _viewModel;

    public UpdatePlatformControl(UpdatePlatformViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = viewModel;
        InitializeComponent();
    }

    private async void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        await _viewModel.LoadAsync();
    }
}