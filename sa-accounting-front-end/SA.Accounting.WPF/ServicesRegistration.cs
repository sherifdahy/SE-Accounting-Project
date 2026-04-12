using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using SA.Accounting.Core.Interfaces;
using SA.Accounting.Infrastructure.Clients.Auth;
using SA.Accounting.Infrastructure.Clients.Company;
using SA.Accounting.Infrastructure.Clients.Platform;
using SA.Accounting.Infrastructure.Clients.User;
using SA.Accounting.WPF.Factories;
using SA.Accounting.WPF.Factories.Auth;
using SA.Accounting.WPF.Factories.Home;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.Portals.SystemAdministration.UserControls.Companies;
using SA.Accounting.WPF.Portals.SystemAdministration.UserControls.Platforms;
using SA.Accounting.WPF.Services;
using SA.Accounting.WPF.State.Authenticators;
using SA.Accounting.WPF.State.Navigators;
using SA.Accounting.WPF.ViewModels;
using SA.Accounting.WPF.ViewModels.Auth;
using SA.Accounting.WPF.ViewModels.Company;
using SA.Accounting.WPF.ViewModels.Main;
using SA.Accounting.WPF.ViewModels.Platform;
using SA.Accounting.WPF.ViewModels.User;
using SA.Accounting.WPF.Views.UserControls.Users;
using SA.Accounting.WPF.Windows;
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


        services.AddSingleton<INavigator, Navigator>();
        services.AddSingleton<IAuthenticator, Authenticator>();
        services.AddSingleton<IAppNavigationService, AppNavigationService>();

        services.AddSingleton<IViewModelAbstractFactory, ViewModelAbstractFactory>();
        services.AddSingleton<IViewModelFactory<HomeViewModel>, HomeViewModelFactory>();
        services.AddSingleton<IViewModelFactory<LoginViewModel>, LoginViewModelFactory>();

        services.AddTransient<SidebarViewModel>();
        services.AddTransient<HeaderViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<AuthWindowViewModel>();
        services.AddTransient(s=> new MainWindow(s.GetRequiredService<MainViewModel>(),s.GetRequiredService<HeaderViewModel>(),s.GetRequiredService<SidebarViewModel>()));
        services.AddTransient(s => new AuthWindow(s.GetRequiredService<AuthWindowViewModel>()));

        return services;
    }

    public static IServiceCollection AddWindowsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<LoginWindow>();
        services.AddTransient<MainWindow>();

        return services;
    }

    public static IServiceCollection AddViewModelsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<CreateCompanyViewModel>();
        services.AddTransient<UpdateCompanyViewModel>();
        services.AddTransient<DisplayCompanyViewModel>();
        services.AddTransient<GetTokenViewModel>();
        services.AddTransient<PlatformsViewModel>();
        services.AddTransient<CreatePlatformViewModel>();
        services.AddTransient<UpdatePlatformViewModel>();
        services.AddTransient<UsersViewModel>();
        services.AddTransient<CreateUserViewModel>();
        services.AddTransient<UpdateUserViewModel>();

        return services;
    }

    public static IServiceCollection AddUserControlsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<CompaniesControl>();
        services.AddTransient<UsersControl>();
        services.AddTransient<CreateUserControl>();
        services.AddTransient<UpdateUserControl>();
        services.AddTransient<CreateCompanyControl>();
        services.AddTransient<UpdateCompanyControl>();
        services.AddTransient<DisplayCompanyControl>();
        services.AddTransient<PlatformsControl>();
        services.AddTransient<CreatePlatformControl>();
        services.AddTransient<UpdatePlatformControl>();

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
        services.AddTransient<IUserService, UserService>();

        services.AddDistributedMemoryCache();

        return services;
    }
}
