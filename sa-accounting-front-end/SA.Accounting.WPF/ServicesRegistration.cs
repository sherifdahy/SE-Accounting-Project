using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using SA.Accounting.Core.Interfaces;
using SA.Accounting.Infrastructure.Clients.Auth;
using SA.Accounting.Infrastructure.Clients.Company;
using SA.Accounting.Infrastructure.Clients.Platform;
using SA.Accounting.Infrastructure.Handlers;
using SA.Accounting.Infrastructure.OptionClasses;
using SA.Accounting.Services;
using SA.Accounting.WPF.Services;
using System.Net.Http;

namespace SA.Accounting.WPF;

public static class ServicesRegistration
{
    public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddWindowsConfig(services, configuration);
        AddViewModelsConfig(services, configuration);
        AddUserControlsConfig(services, configuration);
        AddRefitConfig(services, configuration);
        AddServicesConfig(services, configuration);

        return services;
    }

    public static IServiceCollection AddWindowsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<Windows.LoginWindow>();
        services.AddTransient<Portals.SystemAdministration.Windows.MainWindow>();

        return services;
    }

    public static IServiceCollection AddViewModelsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ViewModels.Company.CreateCompanyViewModel>();
        services.AddTransient<ViewModels.Company.UpdateCompanyViewModel>();
        services.AddTransient<ViewModels.Company.DisplayCompanyViewModel>();
        services.AddTransient<ViewModels.Auth.GetTokenViewModel>();
        services.AddTransient<ViewModels.Platform.PlatformsViewModel>();
        services.AddTransient<ViewModels.Platform.CreatePlatformViewModel>();
        services.AddTransient<ViewModels.Platform.UpdatePlatformViewModel>();

        return services;
    }

    public static IServiceCollection AddUserControlsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<Portals.SystemAdministration.Dialogs.Companies.CompanyFormDialog>();
        services.AddTransient<Portals.SystemAdministration.UserControls.Companies.CompaniesControl>();
        services.AddTransient<Portals.SystemAdministration.UserControls.Employees.EmployeesControl>();
        services.AddTransient<Portals.SystemAdministration.UserControls.Companies.CreateCompanyControl>();
        services.AddTransient<Portals.SystemAdministration.UserControls.Companies.UpdateCompanyControl>();
        services.AddTransient<Portals.SystemAdministration.UserControls.Companies.DisplayCompanyControl>();
        services.AddTransient<Portals.SystemAdministration.UserControls.Platforms.PlatformsControl>();
        services.AddTransient<Portals.SystemAdministration.UserControls.Platforms.CreatePlatformControl>();
        services.AddTransient<Portals.SystemAdministration.UserControls.Platforms.UpdatePlatformControl>();

        return services;
    }

    public static IServiceCollection AddRefitConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<AuthHeaderHandler>();

        var apiSettings = configuration.GetSection(nameof(ApiSettings)).Get<ApiSettings>();

        services.AddRefitClient<IAuthClient>()
            .ConfigureHttpClient(options => options.BaseAddress = new Uri(apiSettings!.BaseUrl))
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            });

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

        return services;
    }

    public static IServiceCollection AddServicesConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICacheService, CacheService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IPermissionService, PermissionService>();
        services.AddSingleton<IDialogService, DialogService>();

        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<ICompanyService, CompanyService>();
        services.AddTransient<IPlatformService, PlatformService>();

        services.AddDistributedMemoryCache();

        return services;
    }
}
