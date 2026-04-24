using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SA.Accounting.WPF.ViewModels.Auth;
using SA.Accounting.WPF.ViewModels.Main;

namespace SA.Accounting.WPF.HostBuilders;

public static class AddViewsHostBuilderExtensions
{
    public static IHostBuilder AddViews(this IHostBuilder host)
    {
        return host.ConfigureServices(services =>
        {
            services.AddTransient(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
            services.AddTransient(s => new AuthWindow(s.GetRequiredService<AuthWindowViewModel>()));
        });
    }
}
