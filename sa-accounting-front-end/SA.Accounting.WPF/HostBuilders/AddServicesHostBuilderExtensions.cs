using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SA.Accounting.WPF.HostBuilders;

public static class AddServicesHostBuilderExtensions
{
    public static IHostBuilder AddServices(this IHostBuilder host)
    {
        return host.ConfigureServices((context, services) =>
        {
            services.AddSingleton<IAppNavigationService, AppNavigationService>();

            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IJwtTokenService, JwtTokenService>();
            services.AddSingleton<IPermissionService, PermissionService>();

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<IPlatformService, PlatformService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDialogService, DialogService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<ITransactionCategoryService, TransactionCategoryService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddScoped<IAccountAutomationService, PlaywrightAccountAutomationService>();
        });
    }
}
