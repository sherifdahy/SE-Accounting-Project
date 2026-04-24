using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SA.Accounting.Core.Contracts.Company.Validators;
using SA.Accounting.WPF.Validators;
using SA.Accounting.WPF.Validators.Accounts;
using SA.Accounting.WPF.Validators.Auth;
using SA.Accounting.WPF.Validators.Companies;
using SA.Accounting.WPF.Validators.Owners;
using SA.Accounting.WPF.Validators.Platforms;
using SA.Accounting.WPF.Validators.TransactionItems;
using SA.Accounting.WPF.Validators.Transactions;
using SA.Accounting.WPF.ViewModels;
using SA.Accounting.WPF.ViewModels.Account;
using SA.Accounting.WPF.ViewModels.Company;
using SA.Accounting.WPF.ViewModels.Owner;
using SA.Accounting.WPF.ViewModels.TransactionItem;

namespace SA.Accounting.WPF.HostBuilders;

public static class AddValidatorsHostBuilder
{
    public static IHostBuilder AddValidators(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureServices(services =>
        {
            services.AddTransient<IValidator<LoginViewModel>, LoginViewModelValidator>();
            services.AddTransient<IValidator<CreateCompanyViewModel>, CreateCompanyViewModelValidator>();
            services.AddTransient<IValidator<CreateOwnerViewModel>, CreateOwnerViewModelValidator>();
            services.AddTransient<IValidator<CreateAccountViewModel>, CreateAccountViewModelValidator>();
            services.AddTransient<IValidator<UpdatePlatformViewModel>, UpdatePlatformViewModelValidator>();
            services.AddTransient<IValidator<UpdateCompanyViewModel>, UpdateCompanyViewModelValidator>();
            services.AddTransient<IValidator<UpdateOwnerViewModel>, UpdateOwnerViewModelValidator>();
            services.AddTransient<IValidator<UpdateAccountViewModel>, UpdateAccountViewModelValidator>();
            services.AddTransient<IValidator<CreatePlatformViewModel>, CreatePlatformViewModelValidator>();
            services.AddTransient<IValidator<CreateTransactionViewModel>, CreateTransactionViewModelValidator>();
            services.AddTransient<IValidator<CreateTransactionItemViewModel>, CreateTransactionItemValidator>();
            services.AddTransient<IValidator<UpdateTransactionViewModel>, UpdateTransactionViewModelValidator>();
            services.AddTransient<IValidator<UpdateTransactionItemViewModel>, UpdateTransactionItemValidator>();
        });
    }
}
