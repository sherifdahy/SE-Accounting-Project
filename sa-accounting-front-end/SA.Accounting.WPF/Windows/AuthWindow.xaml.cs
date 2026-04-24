using SA.Accounting.WPF.ViewModels.Auth;
using System.Windows;

namespace SA.Accounting.WPF;
/// <summary>
/// Interaction logic for AuthWindow.xaml
/// </summary>
public partial class AuthWindow : Window
{
    public AuthWindow(object data)
    {
        InitializeComponent();

        DataContext = data;
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (DataContext is AuthWindowViewModel vm)
        {
            vm.CloseCommand.Execute(null);
        }
    }
}
