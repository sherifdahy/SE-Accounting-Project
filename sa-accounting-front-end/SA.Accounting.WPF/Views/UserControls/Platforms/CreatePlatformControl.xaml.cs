using SA.Accounting.WPF.ViewModels.Platform;
using System.Windows.Controls;

namespace SA.Accounting.WPF.Portals.SystemAdministration.UserControls.Platforms;

public partial class CreatePlatformControl : UserControl
{
    public CreatePlatformControl(CreatePlatformViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}