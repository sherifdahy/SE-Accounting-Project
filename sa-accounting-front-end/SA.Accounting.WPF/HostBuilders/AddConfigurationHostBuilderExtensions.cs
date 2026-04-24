using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SA.Accounting.WPF.HostBuilders;

public static class AddConfigurationHostBuilderExtensions
{
    public static IHostBuilder AddConfiguration(this IHostBuilder host)
    {
        return host.ConfigureAppConfiguration(config =>
        {
            config.AddJsonFile("appsettings.json");
            config.AddEnvironmentVariables();
        });
    }

     
}
