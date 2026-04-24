using Mapster;
using SA.Accounting.Core.Contracts.Platform.Requests;
using SA.Accounting.Core.Contracts.Selector.Requests;
using SA.Accounting.Core.Enums;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using SA.Accounting.WPF.ViewModels.Selector;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public class UpdatePlatformViewModel : ValidatableViewModel<UpdatePlatformViewModel>, IAsyncInitializable<int>
{
    private readonly IPlatformService _platformService;
    private readonly IDialogService _dialogService;
    private readonly INavigator _navigator;
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;
    private readonly IValidator<UpdatePlatformViewModel> _validator;

    public override ViewType Section => ViewType.Platforms;
    protected override IValidator<UpdatePlatformViewModel> Validator => _validator;

    private int _platformId;

    // ══════ Properties ══════
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); ValidateProperty(); }
    }

    private string _url = string.Empty;
    public string Url
    {
        get => _url;
        set { _url = value; OnPropertyChanged(); ValidateProperty(); }
    }

    private string _imageUrl = string.Empty;
    public string ImageUrl
    {
        get => _imageUrl;
        set { _imageUrl = value; OnPropertyChanged(); ValidateProperty(); }
    }

    // ══════ Change Tracking ══════
    private string _originalName = string.Empty;
    private string _originalUrl = string.Empty;
    private string _originalImageUrl = string.Empty;
    private int _originalSelectorsHash;

    public bool HasChanges =>
        Name != _originalName
        || Url != _originalUrl
        || ImageUrl != _originalImageUrl
        || GetSelectorsHash() != _originalSelectorsHash;

    // ══════ Collections ══════
    public ObservableCollection<CreateSelectorViewModel> Selectors { get; } = [];

    public SelectorContentType[] ContentTypes { get; } = Enum.GetValues<SelectorContentType>();
    public SelectorType[] SelectorTypes { get; } = Enum.GetValues<SelectorType>();

    // ══════ Commands ══════
    public ICommand AddSelectorCommand { get; }
    public ICommand RemoveSelectorCommand { get; }
    public ICommand MoveSelectorUpCommand { get; }
    public ICommand MoveSelectorDownCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public UpdatePlatformViewModel(
        IPlatformService platformService,
        IDialogService dialogService,
        INavigator navigator,
        IViewModelAbstractFactory viewModelAbstractFactory,
        IValidator<UpdatePlatformViewModel> validator)
    {
        _platformService = platformService;
        _dialogService = dialogService;
        _navigator = navigator;
        _viewModelAbstractFactory = viewModelAbstractFactory;
        _validator = validator;

        AddSelectorCommand = new RelayCommand((_) =>
        {
            Selectors.Add(new CreateSelectorViewModel());
            RecalculatePriorities();
        });

        RemoveSelectorCommand = new RelayCommand((s) =>
        {
            Selectors.Remove((CreateSelectorViewModel)s);
            RecalculatePriorities();
        });

        MoveSelectorUpCommand = new RelayCommand((s) =>
        {
            var index = Selectors.IndexOf((CreateSelectorViewModel)s);
            if (index > 0) { Selectors.Move(index, index - 1); RecalculatePriorities(); }
        });

        MoveSelectorDownCommand = new RelayCommand((s) =>
        {
            var index = Selectors.IndexOf((CreateSelectorViewModel)s);
            if (index >= 0 && index < Selectors.Count - 1) { Selectors.Move(index, index + 1); RecalculatePriorities(); }
        });

        SaveCommand = new AsyncRelayCommand(async (_) => await SaveAsync());
        CancelCommand = new AsyncRelayCommand(async (_) => await CancelAsync());
    }

    // ══════ Initialize ══════
    public async Task InitializeAsync(int platformId)
    {
        _platformId = platformId;
        await LoadPlatformAsync();
    }

    // ══════ Load ══════
    private async Task LoadPlatformAsync()
    {
        try
        {
            var platform = await _platformService.GetByIdAsync(_platformId);
            if (platform is null)
            {
                await _dialogService.ShowErrorAsync("لم يتم العثور على المنصة", "خطأ");
                NavigateBack();
                return;
            }

            Name = platform.Name;
            Url = platform.Url;
            ImageUrl = platform.ImageUrl;

            Selectors.Clear();
            foreach (var s in platform.Selectors?.Where(s => !s.IsDeleted).OrderBy(s => s.Priority))
            {
                Selectors.Add(new CreateSelectorViewModel
                {
                    Value = s.Value,
                    ContentType = s.ContentType,
                    Type = s.Type,
                    Priority = s.Priority,
                    DisplayOrder = s.Priority
                });
            }

            RecalculatePriorities();

            // Store originals for change tracking
            _originalName = Name;
            _originalUrl = Url;
            _originalImageUrl = ImageUrl;
            _originalSelectorsHash = GetSelectorsHash();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ في تحميل بيانات المنصة");
        }
    }

    // ══════ Helpers ══════
    private void RecalculatePriorities()
    {
        for (int i = 0; i < Selectors.Count; i++)
        {
            Selectors[i].Priority = i + 1;
            Selectors[i].DisplayOrder = i + 1;
        }
    }

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

    // ══════ Save ══════
    private async Task SaveAsync()
    {
        try
        {
            RecalculatePriorities();
            ValidateAll();

            if (HasErrors) return;

            if (!HasChanges)
            {
                await _dialogService.ShowInfoAsync("لم يتم إجراء أي تغييرات", "تنبيه");
                return;
            }

            var request = new PlatformRequest
            {
                Name = Name,
                Url = Url,
                ImageUrl = ImageUrl,
                Selectors = Selectors.Adapt<List<SelectorRequest>>()
            };

            await _platformService.UpdateAsync(_platformId, request);
            await _dialogService.ShowInfoAsync("تم تحديث المنصة بنجاح ✓", "نجح التحديث");
            NavigateBack();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ أثناء الحفظ");
        }
    }

    // ══════ Cancel ══════
    private async Task CancelAsync()
    {
        if (HasChanges)
        {
            if (!await _dialogService.ShowConfirmAsync(
                "هل تريد إلغاء التعديل؟ سيتم فقدان جميع التغييرات.", "تأكيد الإلغاء"))
                return;
        }

        NavigateBack();
    }

    private void NavigateBack()
    {
        var vm = _viewModelAbstractFactory.CreateViewModel(ViewType.Platforms);
        if (vm is IAsyncInitializable init) _ = init.InitializeAsync();
        _navigator.CurrentViewModel = vm;
    }
}