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
    /// Interaction logic for CompaniesView.xaml
    /// </summary>
    public partial class CompaniesView : UserControl
    {
        public CompaniesView()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(DataContext is CompaniesViewModel viewModel)
            {
                await viewModel.InitializeAsync();
            }
        }
    }
}
