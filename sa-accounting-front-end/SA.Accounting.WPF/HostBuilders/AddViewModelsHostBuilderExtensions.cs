using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SA.Accounting.WPF.ViewModels;
using SA.Accounting.WPF.ViewModels.Auth;
using SA.Accounting.WPF.ViewModels.Main;

namespace SA.Accounting.WPF.HostBuilders;

public static class AddViewModelsHostBuilderExtensions
{
    public static IHostBuilder AddViewModels(this IHostBuilder host)
    {
        return host.ConfigureServices(services =>
        {
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<AuthWindowViewModel>();

            services.AddSingleton<HomeViewModel>();
            services.AddTransient<HeaderViewModel>();
            services.AddTransient<SidebarViewModel>();

            services.AddTransient<CompaniesViewModel>();
            services.AddTransient<CreateCompanyViewModel>();
            services.AddTransient<UpdateCompanyViewModel>();
            services.AddTransient<DisplayCompanyViewModel>();

            // Platforms
            services.AddSingleton<PlatformsViewModel>();
            services.AddSingleton<CreatePlatformViewModel>();
            services.AddTransient<UpdatePlatformViewModel>();

            // Users
            services.AddTransient<UsersViewModel>();
            services.AddTransient<CreateUserViewModel>();
            services.AddTransient<UpdateUserViewModel>();

            services.AddSingleton<UserBasicInfoViewModel>();
            services.AddSingleton<UserCompaniesViewModel>();

            //Transactions
            services.AddTransient<TransactionsViewModel>();
            services.AddTransient<DisplayTransactionViewModel>();
            services.AddTransient<CreateTransactionViewModel>();
            services.AddTransient<UpdateTransactionViewModel>();


            // Profile
            services.AddTransient<ProfileViewModel>();

            services.AddDistributedMemoryCache();
        });
    }
}
