using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SA.Accounting.WPF.HostBuilders;
using SA.Accounting.WPF.Interfaces;
using System.Globalization;
using System.Windows;

namespace SA.Accounting.WPF;

public partial class App : Application
{
    private readonly IHost _host;

    public App()
    {
        _host = CreateHostBuilder().Build();

        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("ar");
    }

    public static IHostBuilder CreateHostBuilder(string[] args = null!)
    {

        return Host.CreateDefaultBuilder(args)
            .AddConfiguration()
                .AddMapster()
                .AddRefit()
                .AddState()
                .AddViewModels()
                .AddFactories()
                .AddServices()
                .AddViews()
                .AddValidators();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        var appNavigationService = _host.Services.GetRequiredService<IAppNavigationService>();
        appNavigationService.Start();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();

        _host.Dispose();

        base.OnExit(e);
    }
}