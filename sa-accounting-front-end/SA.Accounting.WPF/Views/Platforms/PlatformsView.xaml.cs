using SA.Accounting.WPF.ViewModels;
using System.Windows.Controls;

namespace SA.Accounting.WPF.Views
{
    /// <summary>
    /// Interaction logic for PlatformsView.xaml
    /// </summary>
    public partial class PlatformsView : UserControl
    {
        public PlatformsView()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if(DataContext is PlatformsViewModel vm)
            {
                await vm.InitializeAsync();
            }
        }
    }
}
