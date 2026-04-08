using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SA.Accounting.Infrastructure.Handlers;
using SA.Accounting.WPF.Windows;
using SA.Accounting.WPF.Services;
using System.IO;
using System.Windows;

namespace SA.Accounting.WPF;

public partial class App : Application
{
    private readonly IHost _host;

    public static IServiceProvider Services { get; private set; } = null!;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(ConfigureAppConfiguration)
            .ConfigureServices(ConfigureServices)
            .Build();

        Services = _host.Services;
    }

    private void ConfigureAppConfiguration(HostBuilderContext context, IConfigurationBuilder config)
    {
        var env = context.HostingEnvironment;

        config.SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
              .AddEnvironmentVariables();
    }

    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        var configuration = context.Configuration;

        services.AddCustomServices(configuration);
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        await _host.StartAsync();

        ShowLogin();
    }

    public void ShowLogin()
    {
        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        var loginWindow = Services.GetRequiredService<LoginWindow>();
        var result = loginWindow.ShowDialog();

        if (result == true)
            ShowMain();
        else
            Shutdown();
    }

    public void ShowMain()
    {
        ShutdownMode = ShutdownMode.OnMainWindowClose;

        var mainWindow = Services.GetRequiredService<SA.Accounting.WPF.Portals.SystemAdministration.Windows.MainWindow>();
        MainWindow = mainWindow;
        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync(TimeSpan.FromSeconds(5));
        }

        base.OnExit(e);
    }
}