using SA.Accounting.WPF.ViewModels.Auth;
using System.Windows;
using System.Windows.Input;

namespace SA.Accounting.WPF.Windows;

public partial class LoginWindow : Window
{
    private readonly GetTokenViewModel _vm;

    public LoginWindow(GetTokenViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        DataContext = _vm;

        _vm.OnLoginSuccess = () =>
        {
            DialogResult = true;
            Close();
        };
    }

    private void PasswordTxt_PasswordChanged(object sender, RoutedEventArgs e)
        => _vm.Password = PasswordTxt.Password;

    private void Input_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
            _vm.LoginCommand.Execute(null);
    }
}