using SA.Accounting.WPF.ViewModels.User;
using System.Windows.Controls;

namespace SA.Accounting.WPF.Views.UserControls.Users;

public partial class UpdateUserControl : UserControl
{
    public UpdateUserControl(UpdateUserViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}