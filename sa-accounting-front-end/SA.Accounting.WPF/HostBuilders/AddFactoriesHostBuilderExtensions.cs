using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SA.Accounting.WPF.Factories;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels;
using SA.Accounting.WPF.ViewModels.Base;
using SA.Accounting.WPF.ViewModels.Company;

namespace SA.Accounting.WPF.HostBuilders;

public static class AddFactoriesHostBuilderExtensions
{
    public static IHostBuilder AddFactories(this IHostBuilder host)
    {
        return host.ConfigureServices(services =>
        {
            services.AddSingleton<IViewModelAbstractFactory, ViewModelAbstractFactory>();

            services.AddSingleton<CreateViewModel<HomeViewModel>>(s => () => s.GetRequiredService<HomeViewModel>());
            services.AddSingleton<CreateViewModel<LoginViewModel>>(s => () => s.GetRequiredService<LoginViewModel>());
            services.AddSingleton<CreateViewModel<PlatformsViewModel>>(s => () => s.GetRequiredService<PlatformsViewModel>());
            services.AddSingleton<CreateViewModel<CreatePlatformViewModel>>(s => () => s.GetRequiredService<CreatePlatformViewModel>());
            services.AddSingleton<CreateViewModel<UpdatePlatformViewModel>>(s => () => s.GetRequiredService<UpdatePlatformViewModel>());
            services.AddSingleton<CreateViewModel<UsersViewModel>>(s => () => s.GetRequiredService<UsersViewModel>());
            services.AddSingleton<CreateViewModel<CreateUserViewModel>>(s => () => s.GetRequiredService<CreateUserViewModel>());
            services.AddSingleton<CreateViewModel<UpdateUserViewModel>>(s => () => s.GetRequiredService<UpdateUserViewModel>());
            services.AddSingleton<CreateViewModel<UserBasicInfoViewModel>>(s => () => s.GetRequiredService<UserBasicInfoViewModel>());
            services.AddSingleton<CreateViewModel<UserCompaniesViewModel>>(s => () => s.GetRequiredService<UserCompaniesViewModel>());
            services.AddSingleton<CreateViewModel<CompaniesViewModel>>(s => () => s.GetRequiredService<CompaniesViewModel>());
            services.AddSingleton<CreateViewModel<DisplayCompanyViewModel>>(s => () => s.GetRequiredService<DisplayCompanyViewModel>());
            services.AddSingleton<CreateViewModel<CreateCompanyViewModel>>(s => () => s.GetRequiredService<CreateCompanyViewModel>());
            services.AddSingleton<CreateViewModel<UpdateCompanyViewModel>>(s => () => s.GetRequiredService<UpdateCompanyViewModel>());
            services.AddSingleton<CreateViewModel<UpdateCompanyViewModel>>(s => () => s.GetRequiredService<UpdateCompanyViewModel>());
            services.AddSingleton<CreateViewModel<TransactionsViewModel>>(s => () => s.GetRequiredService<TransactionsViewModel>());
            services.AddSingleton<CreateViewModel<DisplayTransactionViewModel>>(s => () => s.GetRequiredService<DisplayTransactionViewModel>());
            services.AddSingleton<CreateViewModel<CreateTransactionViewModel>>(s => () => s.GetRequiredService<CreateTransactionViewModel>());
            services.AddSingleton<CreateViewModel<UpdateTransactionViewModel>>(s => () => s.GetRequiredService<UpdateTransactionViewModel>());
            services.AddSingleton<CreateViewModel<ProfileViewModel>>(s => () => s.GetRequiredService<ProfileViewModel>());
            services.AddSingleton<CreateViewModel<UserPermissionsViewModel>>(s => () => s.GetRequiredService<UserPermissionsViewModel>());


        });
    }
}
