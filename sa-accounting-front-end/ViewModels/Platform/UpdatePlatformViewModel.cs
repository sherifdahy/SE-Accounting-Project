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
using System.Security.Policy;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SA.Accounting.WPF.ViewModels.Platform;

public sealed partial class UpdatePlatformViewModel
    : ValidatableModel<PlatformRequest>
{
    private readonly IPlatformService _platformService;
    private readonly IDialogService _dialogService;

    private readonly int _platformId;

    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _url = string.Empty;
    [ObservableProperty] private string _imageUrl = string.Empty;
    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private bool _isLoaded;

    private string _originalName = string.Empty;
    private string _originalUrl = string.Empty;
    private string _originalImageUrl = string.Empty;
    private int _originalSelectorsHash;

    public event Action? OnSaved;
    public event Action? OnCancelled;

    public ObservableCollection<CreateSelectorViewModel> Selectors { get; } = [];

    public SelectorContentType[] ContentTypes { get; } =
        Enum.GetValues<SelectorContentType>();

    public SelectorType[] SelectorTypes { get; } =
        Enum.GetValues<SelectorType>();

    public UpdatePlatformViewModel(
        int platformId,
        IPlatformService platformService,
        IDialogService dialogService)
        : base(new PlatformRequestValidator())
    {
        _platformId = platformId;
        _platformService = platformService;
        _dialogService = dialogService;
    }

    partial void OnNameChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(Name));

    partial void OnUrlChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(Url));

    partial void OnImageUrlChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(ImageUrl));

    [RelayCommand]
    public async Task LoadAsync()
    {
        IsBusy = true;
        try
        {
            var platform = await _platformService.GetByIdAsync(_platformId);
            if (platform is null)
            {
                await _dialogService.ShowErrorAsync(
                    "لم يتم العثور على المنصة", "خطأ");
                OnCancelled?.Invoke();
                return;
            }

            SuppressValidation = true;

            Name = platform.Name;
            Url = platform.Url;
            ImageUrl = platform.ImageUrl;

            Selectors.Clear();
            var orderedSelectors = platform.Selectors
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.Priority);

            foreach (var selector in orderedSelectors)
            {
                var vm = new CreateSelectorViewModel
                {
                    Value = selector.Value,
                    ContentType = selector.ContentType,
                    Type = selector.Type,
                    Priority = selector.Priority,
                    DisplayOrder = selector.Priority
                };
                Selectors.Add(vm);
            }

            RecalculatePriorities();

            SuppressValidation = false;
            ClearAllErrors();

            _originalName = Name;
            _originalUrl = Url;
            _originalImageUrl = ImageUrl;
            _originalSelectorsHash = GetSelectorsHash();

            IsLoaded = true;
        }
        catch (Exception ex)
        {
            GlobalExceptionHandler.Handle(ex);
            await _dialogService.ShowErrorAsync(
                "حدث خطأ أثناء تحميل بيانات المنصة", "خطأ");
        }
        finally { IsBusy = false; }
    }

    private void RecalculatePriorities()
    {
        for (int i = 0; i < Selectors.Count; i++)
        {
            Selectors[i].Priority = i + 1;
            Selectors[i].DisplayOrder = i + 1;
        }
    }

    public bool HasChanges =>
        Name != _originalName
        || Url != _originalUrl
        || ImageUrl != _originalImageUrl
        || GetSelectorsHash() != _originalSelectorsHash;

    private int GetSelectorsHash()
    {
        if (Selectors.Count == 0) return 0;

        var hash = new HashCode();
        foreach (var s in Selectors)
        {
            hash.Add(s.Value);
            hash.Add(s.ContentType);
            hash.Add(s.Type);
            hash.Add(s.Priority);
        }
        return hash.ToHashCode();
    }

    [RelayCommand]
    private void AddSelector()
    {
        Selectors.Add(new CreateSelectorViewModel());
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
        if (index <= 0) return;

        Selectors.Move(index, index - 1);
        RecalculatePriorities();
    }

    [RelayCommand]
    private void MoveSelectorDown(CreateSelectorViewModel selector)
    {
        var index = Selectors.IndexOf(selector);
        if (index < 0 || index >= Selectors.Count - 1) return;

        Selectors.Move(index, index + 1);
        RecalculatePriorities();
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
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

        if (!HasChanges)
        {
            await _dialogService.ShowInfoAsync(
                "لم يتم إجراء أي تغييرات", "تنبيه");
            return;
        }

        IsBusy = true;
        try
        {
            await _platformService.UpdateAsync(_platformId, request);

            await _dialogService.ShowInfoAsync(
                "تم تحديث المنصة بنجاح ✓", "نجح التحديث");
            OnSaved?.Invoke();
        }
        catch (Exception ex) { GlobalExceptionHandler.Handle(ex); }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        if (HasChanges)
        {
            var confirmed = await _dialogService.ShowConfirmAsync(
                "هل تريد إلغاء التعديل؟ سيتم فقدان جميع التغييرات.",
                "تأكيد الإلغاء");

            if (!confirmed) return;
        }

        OnCancelled?.Invoke();
    }

    private PlatformRequest ToRequest() => new()
    {
        Name = Name,
        Url = Url,
        ImageUrl = ImageUrl,
        Selectors = Selectors.Select(s => s.ToRequest()).ToList()
    };
}