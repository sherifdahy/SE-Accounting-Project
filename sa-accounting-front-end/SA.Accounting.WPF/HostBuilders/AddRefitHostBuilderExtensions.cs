using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
using SA.Accounting.Infrastructure.Clients.Account;
using SA.Accounting.Infrastructure.Clients.Auth;
using SA.Accounting.Infrastructure.Clients.Company;
using SA.Accounting.Infrastructure.Clients.Platform;
using SA.Accounting.Infrastructure.Clients.Role;
using SA.Accounting.Infrastructure.Clients.Transaction;
using SA.Accounting.Infrastructure.Clients.TransactionCategory;
using SA.Accounting.Infrastructure.Clients.User;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SA.Accounting.WPF.HostBuilders;

public static class AddRefitHostBuilderExtensions
{
    public static IHostBuilder AddRefit(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices((context,services) =>
        {
            services.AddTransient<AuthHeaderHandler>();

            var apiSettings = context.Configuration.GetSection(nameof(ApiSettings)).Get<ApiSettings>();

            services.AddRefitClient<IAuthClient>()
                .ConfigureHttpClient(options => options.BaseAddress = new Uri(apiSettings!.BaseUrl))
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                });

            services.AddRefitClient<IUserClient>()
                .ConfigureHttpClient(options => options.BaseAddress = new Uri(apiSettings!.BaseUrl))
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                })
                .AddHttpMessageHandler<AuthHeaderHandler>();

            services.AddRefitClient<ICompanyClient>()
                .ConfigureHttpClient(options => options.BaseAddress = new Uri(apiSettings!.BaseUrl))
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                })
                .AddHttpMessageHandler<AuthHeaderHandler>();

            services.AddRefitClient<IPlatformClient>()
                .ConfigureHttpClient(options => options.BaseAddress = new Uri(apiSettings!.BaseUrl))
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                })
                .AddHttpMessageHandler<AuthHeaderHandler>();

            services.AddRefitClient<IRoleClient>()
                .ConfigureHttpClient(options => options.BaseAddress = new Uri(apiSettings!.BaseUrl))
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                })
                .AddHttpMessageHandler<AuthHeaderHandler>();

            services.AddRefitClient<ITransactionClient>()
                .ConfigureHttpClient(options => options.BaseAddress = new Uri(apiSettings!.BaseUrl))
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                })
                .AddHttpMessageHandler<AuthHeaderHandler>();

            services.AddRefitClient<ITransactionCategoryClient>()
                .ConfigureHttpClient(options => options.BaseAddress = new Uri(apiSettings!.BaseUrl))
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                })
                .AddHttpMessageHandler<AuthHeaderHandler>();


            services.AddRefitClient<IAccountClient>()
                .ConfigureHttpClient(options => options.BaseAddress = new Uri(apiSettings!.BaseUrl))
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                })
                .AddHttpMessageHandler<AuthHeaderHandler>();
        });

        return hostBuilder;
    }
}
