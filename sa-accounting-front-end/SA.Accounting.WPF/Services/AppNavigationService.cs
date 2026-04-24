using Microsoft.Extensions.DependencyInjection;
using SA.Accounting.WPF.Interfaces;
using System.Drawing.Printing;
using System.Windows;

namespace SA.Accounting.WPF.Services;

public class AppNavigationService : IAppNavigationService
{
    private readonly IServiceProvider _serviceProvider;
    private AuthWindow? _authWindow;
    private MainWindow? _mainWindow;

    public AppNavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Start()
    {
        ShowAuthWindow();
    }
    public void LoginSuccess()
    {
        HideAuthWindow();
        ShowMainWindow();
    }

    public void Logout()
    {
        HideMainWindow();
        ShowAuthWindow();
    }
    public void CloseApplication()
    {
        Application.Current.Shutdown();
    }

    private void HideAuthWindow()
    {
        _authWindow!.Hide();
    }

    private void HideMainWindow()
    {
        _mainWindow!.Hide();
    }

    private void ShowAuthWindow()
    {
        _authWindow = _serviceProvider.GetRequiredService<AuthWindow>();
        _authWindow.Show();
    }

    private void ShowMainWindow()
    {
        _mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        _mainWindow.Show();
    }
}