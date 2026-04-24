using SA.Accounting.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace SA.Accounting.WPF.Views
{
    /// <summary>
    /// Interaction logic for CreateUserView.xaml
    /// </summary>
    public partial class CreateUserView : UserControl
    {
        public CreateUserView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is CreateUserViewModel vm
                && vm.LoadRolesCommand.CanExecute(null))
            {
                vm.LoadRolesCommand.Execute(null);
            }
        }
    }
}
