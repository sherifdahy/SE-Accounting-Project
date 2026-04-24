using SA.Accounting.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SA.Accounting.WPF.Views
{
    /// <summary>
    /// Interaction logic for UserBasicInfoView.xaml
    /// </summary>
    public partial class UserBasicInfoView : UserControl
    {
        public UserBasicInfoView()
        {
            InitializeComponent();
        }


        private void RadPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is UserBasicInfoViewModel viewModel)
            {
                viewModel.Password = PasswordBox.Password;
            }
        }
    }
}
