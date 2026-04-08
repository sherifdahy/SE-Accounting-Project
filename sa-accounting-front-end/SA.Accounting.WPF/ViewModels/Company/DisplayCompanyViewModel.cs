using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SA.Accounting.Core.Contracts.Account.Responses;
using SA.Accounting.Core.Contracts.Owner.Responses;
using SA.Accounting.Infrastructure.Handlers;
using SA.Accounting.Core.Interfaces;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SA.Accounting.WPF.ViewModels.Company;

public sealed partial class DisplayCompanyViewModel : ObservableObject
{
    // ─── Services ────────────────────────────────
    private readonly ICompanyService _companyService;

    // ─── Properties ──────────────────────────────
    [ObservableProperty] private int _companyId;
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _taxRegistrationNumber = string.Empty;
    [ObservableProperty] private string _taxFileNumber = string.Empty;
    [ObservableProperty] private string _address = string.Empty;
    [ObservableProperty] private bool _isDeleted;
    [ObservableProperty] private bool _isBusy;

    // ─── Collections ─────────────────────────────
    public ObservableCollection<OwnerResponse> Owners { get; } = [];
    public ObservableCollection<AccountResponse> Accounts { get; } = [];

    // ─── Constructor ─────────────────────────────
    public DisplayCompanyViewModel(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    // ─── Load Company ────────────────────────────
    [RelayCommand]
    private async Task LoadCompanyAsync(int companyId)
    {
        IsBusy = true;
        try
        {
            var company = await _companyService.GetByIdAsync(companyId);
            if (company is null) return;

            CompanyId = company.Id;
            Name = company.Name;
            TaxRegistrationNumber = company.TaxRegistrationNumber;
            TaxFileNumber = company.TaxFileNumber;
            Address = company.Address;
            IsDeleted = company.IsDeleted;

            Owners.Clear();
            foreach (var o in company.Owners ?? [])
                Owners.Add(o);

            Accounts.Clear();
            foreach (var a in company.Accounts ?? [])
                Accounts.Add(a);
        }
        catch (Exception ex) { GlobalExceptionHandler.Handle(ex); }
        finally { IsBusy = false; }
    }

    // ─── Open Platform ───────────────────────────
    [RelayCommand]
    private void OpenPlatform(AccountResponse account)
    {
        if (string.IsNullOrWhiteSpace(account.Platform?.Url)) return;

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = account.Platform.Url,
                UseShellExecute = true
            });
        }
        catch (Exception ex) { GlobalExceptionHandler.Handle(ex); }
    }
}