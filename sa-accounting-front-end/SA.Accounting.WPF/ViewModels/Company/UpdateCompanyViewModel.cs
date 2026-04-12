using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SA.Accounting.Core.Contracts.Company.Requests;
using SA.Accounting.Core.Contracts.Company.Validators;
using SA.Accounting.Core.Contracts.Platform.Responses;
using SA.Accounting.Core;
using SA.Accounting.Infrastructure.Handlers;
using SA.Accounting.Core.Interfaces;
using SA.Accounting.WPF.ViewModels.Account;
using SA.Accounting.WPF.ViewModels.Owner;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace SA.Accounting.WPF.ViewModels.Company;

public sealed partial class UpdateCompanyViewModel
    : ValidatableModel<UpdateCompanyRequest>
{
    // ─── Services ────────────────────────────────
    private readonly ICompanyService _companyService;
    private readonly IPlatformService _platformService;
    private readonly IDialogService _dialogService;

    // ─── Properties ──────────────────────────────
    [ObservableProperty] private int _companyId;
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _taxRegistrationNumber = string.Empty;
    [ObservableProperty] private string _taxFileNumber = string.Empty;
    [ObservableProperty] private string _address = string.Empty;
    [ObservableProperty] private bool _isBusy;

    // ─── Collections ─────────────────────────────
    public ObservableCollection<UpdateOwnerViewModel> Owners { get; } = [];
    public ObservableCollection<UpdateAccountViewModel> Accounts { get; } = [];
    public ObservableCollection<PlatformResponse> Platforms { get; } = [];

    // ─── Constructor ─────────────────────────────
    public UpdateCompanyViewModel(
        ICompanyService companyService,
        IPlatformService platformService,
        IDialogService dialogService)
        : base(new UpdateCompanyRequestValidator())
    {
        _companyService = companyService;
        _platformService = platformService;
        _dialogService = dialogService;
    }

    // ─── Validation hooks ────────────────────────
    partial void OnNameChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(Name));

    partial void OnTaxRegistrationNumberChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(TaxRegistrationNumber));

    partial void OnTaxFileNumberChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(TaxFileNumber));

    partial void OnAddressChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(Address));

    // ─── Load Platforms ──────────────────────────
    [RelayCommand]
    private async Task LoadPlatformsAsync()
    {
        IsBusy = true;
        try
        {
            var result = await _platformService.GetAllAsync(false);
            Platforms.Clear();
            foreach (var p in result ?? [])
                Platforms.Add(p);
        }
        catch (Exception ex) {  }
        finally { IsBusy = false; }
    }

    // ─── Load Company ────────────────────────────
    public async Task LoadCompanyAsync(int companyId)
    {
        IsBusy = true;
        try
        {
            var company = await _companyService.GetByIdAsync(companyId);
            if (company is null) return;

            SuppressValidation = true;

            CompanyId = company.Id;
            Name = company.Name;
            TaxRegistrationNumber = company.TaxRegistrationNumber;
            TaxFileNumber = company.TaxFileNumber;
            Address = company.Address;

            Owners.Clear();
            foreach (var o in company.Owners ?? [])
                Owners.Add(new UpdateOwnerViewModel
                {
                    Id = o.Id,
                    Name = o.Name,
                    Ssn = o.SSN,
                });

            Accounts.Clear();
            foreach (var a in company.Accounts ?? [])
                Accounts.Add(new UpdateAccountViewModel
                {
                    Id = a.Id,
                    Email = a.Email,
                    Password = a.Password,
                    PlatformId = a.Platform.Id,
                });

            SuppressValidation = false;
            ClearAllErrors();
        }
        catch (Exception ex) {  }
        finally { IsBusy = false; }
    }

    // ─── Owners ──────────────────────────────────
    [RelayCommand]
    private void AddOwner() => Owners.Add(new UpdateOwnerViewModel());

    [RelayCommand]
    private void RemoveOwner(UpdateOwnerViewModel owner) => Owners.Remove(owner);

    // ─── Accounts ────────────────────────────────
    [RelayCommand]
    private void AddAccount() => Accounts.Add(new UpdateAccountViewModel());

    [RelayCommand]
    private void RemoveAccount(UpdateAccountViewModel account) => Accounts.Remove(account);

    // ─── Save ────────────────────────────────────
    [RelayCommand]
    private async Task SaveAsync()
    {
        var request = ToRequest();
        ValidateAll(request);
        foreach (var o in Owners) o.ValidateAll(o.ToRequest());
        foreach (var a in Accounts) a.ValidateAll(a.ToRequest());

        bool allValid = IsValid
                     && Owners.All(o => o.IsValid)
                     && Accounts.All(a => a.IsValid);

        if (!allValid)
        {
            await _dialogService.ShowWarningAsync(
                "يرجى تصحيح الأخطاء قبل الحفظ", "تحقق من البيانات");
            return;
        }

        IsBusy = true;
        try
        {
            await _companyService.UpdateAsync(CompanyId, request);
            await _dialogService.ShowInfoAsync(
                "تم تعديل بيانات الشركة بنجاح ✓", "نجح الحفظ");
        }
        finally { IsBusy = false; }
    }

    // ─── Cancel ──────────────────────────────────
    [RelayCommand]
    private async Task CancelAsync()
    {
        var confirmed = await _dialogService.ShowConfirmAsync(
            "هل تريد إلغاء العملية؟ سيتم فقدان جميع التعديلات.",
            "تأكيد الإلغاء");

        if (confirmed)
            await LoadCompanyAsync(CompanyId);
    }

    // ─── Map to Request ──────────────────────────
    private UpdateCompanyRequest ToRequest() => new()
    {
        Name = Name,
        TaxRegistrationNumber = TaxRegistrationNumber,
        TaxFileNumber = TaxFileNumber,
        Address = Address,
        Owners = Owners.Select(o => o.ToRequest()).ToList(),
        Accounts = Accounts.Select(a => a.ToRequest()).ToList(),
    };
}