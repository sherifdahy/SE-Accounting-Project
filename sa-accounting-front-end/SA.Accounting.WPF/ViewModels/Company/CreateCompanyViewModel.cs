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

public sealed partial class CreateCompanyViewModel
    : ValidatableModel<CreateCompanyRequest>
{
    // ─── Services ────────────────────────────────
    private readonly ICompanyService _companyService;
    private readonly IPlatformService _platformService;
    private readonly IDialogService _dialogService;

    // ─── Properties ──────────────────────────────
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _taxRegistrationNumber = string.Empty;
    [ObservableProperty] private string _taxFileNumber = string.Empty;
    [ObservableProperty] private string _address = string.Empty;
    [ObservableProperty] private bool _isBusy;

    // ─── Collections ─────────────────────────────
    public ObservableCollection<CreateOwnerViewModel> Owners { get; } = [];
    public ObservableCollection<CreateAccountViewModel> Accounts { get; } = [];
    public ObservableCollection<PlatformResponse> Platforms { get; } = [];

    // ─── Constructor ─────────────────────────────
    public CreateCompanyViewModel(
        ICompanyService companyService,
        IPlatformService platformService,
        IDialogService dialogService)
        : base(new CreateCompanyRequestValidator())
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
        catch (Exception ex) { GlobalExceptionHandler.Handle(ex); }
        finally { IsBusy = false; }
    }

    // ─── Owners ──────────────────────────────────
    [RelayCommand]
    private void AddOwner() => Owners.Add(new CreateOwnerViewModel());

    [RelayCommand]
    private void RemoveOwner(CreateOwnerViewModel owner) => Owners.Remove(owner);

    // ─── Accounts ────────────────────────────────
    [RelayCommand]
    private void AddAccount() => Accounts.Add(new CreateAccountViewModel());

    [RelayCommand]
    private void RemoveAccount(CreateAccountViewModel account) => Accounts.Remove(account);

    // ─── Save ────────────────────────────────────
    [RelayCommand]
    private async Task SaveAsync()
    {
        // Validate الكل
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
            var result = await _companyService.CreateAsync(request);

            if (result is not null)
            {
                await _dialogService.ShowInfoAsync("تم إنشاء الشركة بنجاح ✓", "نجح الحفظ");
                ResetForm();
            }
            else
            {
                await _dialogService.ShowErrorAsync(
                    "حدث خطأ أثناء الحفظ، يرجى المحاولة مرة أخرى", "خطأ");
            }
        }
        catch (Exception ex) { GlobalExceptionHandler.Handle(ex); }
        finally { IsBusy = false; }
    }

    // ─── Cancel ──────────────────────────────────
    [RelayCommand]
    private async Task CancelAsync()
    {
        var confirmed = await _dialogService.ShowConfirmAsync(
            "هل تريد إلغاء العملية؟ سيتم فقدان جميع البيانات المدخلة.",
            "تأكيد الإلغاء");

        if (confirmed) ResetForm();
    }

    // ─── Reset ───────────────────────────────────
    private void ResetForm()
    {
        SuppressValidation = true;
        Name = string.Empty;
        TaxRegistrationNumber = string.Empty;
        TaxFileNumber = string.Empty;
        Address = string.Empty;
        Owners.Clear();
        Accounts.Clear();
        SuppressValidation = false;
        ClearAllErrors();
    }

    // ─── Map to Request ──────────────────────────
    private CreateCompanyRequest ToRequest() => new()
    {
        Name = Name,
        TaxRegistrationNumber = TaxRegistrationNumber,
        TaxFileNumber = TaxFileNumber,
        Address = Address,
        Owners = Owners.Select(o => o.ToRequest()).ToList(),
        Accounts = Accounts.Select(a => a.ToRequest()).ToList(),
    };
}