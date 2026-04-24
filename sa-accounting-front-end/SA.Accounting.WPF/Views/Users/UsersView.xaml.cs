using SA.Accounting.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace SA.Accounting.WPF.Views
{
    public partial class UsersView : UserControl
    {
        public UsersView()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is UsersViewModel viewModel)
            {
                await viewModel.InitializeAsync();
            }
        }
    }
}