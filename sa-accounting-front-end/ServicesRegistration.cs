using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using SA.Accounting.WPF.Clients.Auth;
using SA.Accounting.WPF.Clients.Company;
using SA.Accounting.WPF.Clients.Platform;
using SA.Accounting.WPF.Handlers;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.OptionClasses;
using SA.Accounting.WPF.Portals.SystemAdministration.Dialogs.Companies;
using SA.Accounting.WPF.Portals.SystemAdministration.UserControls.Companies;
using SA.Accounting.WPF.Portals.SystemAdministration.UserControls.Employees;
using SA.Accounting.WPF.Portals.SystemAdministration.UserControls.Platforms;
using SA.Accounting.WPF.Services;
using SA.Accounting.WPF.ViewModels.Auth;
using SA.Accounting.WPF.ViewModels.Company;
using SA.Accounting.WPF.ViewModels.Platform;
using SA.Accounting.WPF.Windows;
using System.Net.Http;
using Telerik.Windows.Controls.Scheduling;

namespace SA.Accounting.WPF;

public static class ServicesRegistration
{
    public static IServiceCollection AddWindowsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<LoginWindow>();
        services.AddTransient<Portals.SystemAdministration.Windows.MainWindow>();

        return services;
    }

    public static IServiceCollection AddDialogConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<CompanyFormDialog>();

        return services;
    }

    public static IServiceCollection AddViewModelsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<CreateCompanyViewModel>();
        services.AddTransient<UpdateCompanyViewModel>();
        services.AddTransient<DisplayCompanyViewModel>();
        services.AddTransient<GetTokenViewModel>();
        services.AddTransient<PlatformsViewModel>();

        return services;
    }

    public static IServiceCollection AddUserControlsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<CompaniesControl>();
        services.AddTransient<EmployeesControl>();
        services.AddTransient<CreateCompanyControl>();
        services.AddTransient<UpdateCompanyControl>();
        services.AddTransient<DisplayCompanyControl>();
        services.AddTransient<PlatformsControl>();
        services.AddTransient<CreatePlatformControl>();
        services.AddTransient<UpdatePlatformControl>();

        return services;
    }

    public static IServiceCollection AddRefitConfig(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddTransient<AuthHeaderHandler>(); 

        var apiSettings = configuration.GetSection(nameof(ApiSettings)).Get<ApiSettings>();

        services.AddRefitClient<ICompanyClient>().ConfigureHttpClient(options =>
        {
            options.BaseAddress = new Uri(apiSettings!.BaseUrl);
        }).ConfigurePrimaryHttpMessageHandler(()=> new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        }).AddHttpMessageHandler<AuthHeaderHandler>();

        services.AddRefitClient<IAuthClient>().ConfigureHttpClient(options =>
        {
            options.BaseAddress = new Uri(apiSettings!.BaseUrl);
        }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        });

        services.AddRefitClient<IPlatformClient>().ConfigureHttpClient(options =>
        {
            options.BaseAddress = new Uri(apiSettings!.BaseUrl);
        }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        }).AddHttpMessageHandler<AuthHeaderHandler>();


        return services; 
    }

    public static IServiceCollection AddServicesConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICacheService, CacheService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IPermissionService, PermissionService>();

        services.AddTransient<IAuthService, AuthService>();

        services.AddTransient<ICompanyService, CompanyService>();
        services.AddTransient<IPlatformService, PlatfromService>();
        services.AddSingleton<IDialogService, DialogService>();

        services.AddDistributedMemoryCache();

        return services;
    }

}
