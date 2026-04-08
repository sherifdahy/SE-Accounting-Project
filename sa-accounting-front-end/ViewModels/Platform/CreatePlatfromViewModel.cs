using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SA.Accounting.WPF.Contracts.Platform.Requests;
using SA.Accounting.WPF.Contracts.Platform.Validators;
using SA.Accounting.WPF.Core;
using SA.Accounting.WPF.Enums;
using SA.Accounting.WPF.Handlers;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Selector;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SA.Accounting.WPF.ViewModels.Platform;

public sealed partial class CreatePlatformViewModel
    : ValidatableModel<PlatformRequest>
{
    // ─── Services ────────────────────────────────
    private readonly IPlatformService _platformService;
    private readonly IDialogService _dialogService;

    // ─── Properties ──────────────────────────────
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _url = string.Empty;
    [ObservableProperty] private string _imageUrl = string.Empty;
    [ObservableProperty] private bool _isBusy;

    // ─── Events ──────────────────────────────────
    public event Action? OnSaved;
    public event Action? OnCancelled;

    // ─── Collections ─────────────────────────────
    public ObservableCollection<CreateSelectorViewModel> Selectors { get; } = [];

    public SelectorContentType[] ContentTypes { get; } =
        Enum.GetValues<SelectorContentType>();

    public SelectorType[] SelectorTypes { get; } =
        Enum.GetValues<SelectorType>();

    // ─── Constructor ─────────────────────────────
    public CreatePlatformViewModel(
        IPlatformService platformService,
        IDialogService dialogService)
        : base(new PlatformRequestValidator())
    {
        _platformService = platformService;
        _dialogService = dialogService;
    }

    // ─── Validation Hooks ────────────────────────
    partial void OnNameChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(Name));

    partial void OnUrlChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(Url));

    partial void OnImageUrlChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(ImageUrl));

    // ─── Recalculate Priorities ──────────────────
    /// <summary>
    /// يعيد حساب Priority و DisplayOrder لكل selector
    /// بناءً على ترتيبه الفعلي في القائمة
    /// </summary>
    private void RecalculatePriorities()
    {
        for (int i = 0; i < Selectors.Count; i++)
        {
            Selectors[i].Priority = i + 1;
            Selectors[i].DisplayOrder = i + 1;
        }
    }

    // ─── Commands ────────────────────────────────
    [RelayCommand]
    private void AddSelector()
    {
        var selector = new CreateSelectorViewModel();
        Selectors.Add(selector);
        RecalculatePriorities();
    }

    [RelayCommand]
    private void RemoveSelector(CreateSelectorViewModel selector)
    {
        Selectors.Remove(selector);
        RecalculatePriorities();
    }

    [RelayCommand]
    private void MoveSelectorUp(CreateSelectorViewModel selector)
    {
        var index = Selectors.IndexOf(selector);
        if (index <= 0) return; // أول عنصر أو مش موجود

        Selectors.Move(index, index - 1);
        RecalculatePriorities();
    }

    [RelayCommand]
    private void MoveSelectorDown(CreateSelectorViewModel selector)
    {
        var index = Selectors.IndexOf(selector);
        if (index < 0 || index >= Selectors.Count - 1) return; // آخر عنصر أو مش موجود

        Selectors.Move(index, index + 1);
        RecalculatePriorities();
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        // تأكد إن ال priorities محدثة قبل الحفظ
        RecalculatePriorities();

        var request = ToRequest();
        ValidateAll(request);
        foreach (var s in Selectors) s.ValidateAll(s.ToRequest());

        bool allValid = IsValid && Selectors.All(s => s.IsValid);

        if (!allValid)
        {
            await _dialogService.ShowWarningAsync(
                "يرجى تصحيح الأخطاء قبل الحفظ", "تحقق من البيانات");
            return;
        }

        IsBusy = true;
        try
        {
            var result = await _platformService.CreateAsync(request);

            if (result is not null)
            {
                await _dialogService.ShowInfoAsync(
                    "تم إنشاء المنصة بنجاح ✓", "نجح الحفظ");
                ResetForm();
                OnSaved?.Invoke();
            }
            else
            {
                await _dialogService.ShowErrorAsync(
                    "حدث خطأ أثناء الحفظ", "خطأ");
            }
        }
        catch (Exception ex) { GlobalExceptionHandler.Handle(ex); }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        bool hasData = !string.IsNullOrWhiteSpace(Name)
                    || !string.IsNullOrWhiteSpace(Url)
                    || !string.IsNullOrWhiteSpace(ImageUrl)
                    || Selectors.Count > 0;

        if (hasData)
        {
            var confirmed = await _dialogService.ShowConfirmAsync(
                "هل تريد إلغاء العملية؟ سيتم فقدان جميع البيانات المدخلة.",
                "تأكيد الإلغاء");

            if (!confirmed) return;
        }

        ResetForm();
        OnCancelled?.Invoke();
    }

    // ─── Reset ───────────────────────────────────
    private void ResetForm()
    {
        SuppressValidation = true;
        Name = string.Empty;
        Url = string.Empty;
        ImageUrl = string.Empty;
        Selectors.Clear();
        SuppressValidation = false;
        ClearAllErrors();
    }

    // ─── Map ─────────────────────────────────────
    private PlatformRequest ToRequest() => new()
    {
        Name = Name,
        Url = Url,
        ImageUrl = ImageUrl,
        Selectors = Selectors.Select(s => s.ToRequest()).ToList()
    };
}