using SA.Accounting.WPF.ViewModels.User;
using System.Windows.Controls;

namespace SA.Accounting.WPF.Views.UserControls.Users;

public partial class CreateUserControl : UserControl
{
    public CreateUserControl(CreateUserViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}