using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.State.Accounts;
using SA.Accounting.WPF.State.Authenticators;
using SA.Accounting.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.HostBuilders;

public static class AddStateHostBuilderExtensions
{
    public static IHostBuilder AddState(this IHostBuilder host)
    {
        return host.ConfigureServices(services =>
        {
            services.AddSingleton<IAccountStore, AccountStore>();
            services.AddSingleton<INavigator, Navigator>();
            services.AddSingleton<IAuthenticator, Authenticator>();
        });
    }
}
