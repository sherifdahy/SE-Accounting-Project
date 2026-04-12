using Microsoft.Extensions.DependencyInjection;
using SA.Accounting.WPF.Interfaces;
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
        ShowLoginWindow();
    }

    public void LoginSucceeded()
    {
        CloseAuthWindow();
        ShowMainWindow();
    }

    public void Logout()
    {
        CloseMainWindow();
        ShowLoginWindow();
    }

    private void ShowLoginWindow()
    {
        Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

        _authWindow = _serviceProvider.GetRequiredService<AuthWindow>();
        _authWindow.Show();
    }

    private void ShowMainWindow()
    {
        Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

        _mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        Application.Current.MainWindow = _mainWindow;
        _mainWindow.Show();
    }

    private void CloseAuthWindow()
    {
        if (_authWindow != null)
        {
            _authWindow.Close();
            _authWindow = null;
        }
    }

    private void CloseMainWindow()
    {
        if (_mainWindow != null)
        {
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            _mainWindow.Close();
            _mainWindow = null;
        }
    }
}