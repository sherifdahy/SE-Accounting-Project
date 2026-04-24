using Mapster;
using Newtonsoft.Json;
using Refit;
using SA.Accounting.Core.Contracts.Platform.Requests;
using SA.Accounting.Core.Contracts.Selector.Requests;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using SA.Accounting.WPF.ViewModels.Selector;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public sealed class CreatePlatformViewModel : ValidatableViewModel<CreatePlatformViewModel>
{
    // ══════ Services ══════
    private readonly IPlatformService _platformService;
    private readonly IDialogService _dialogService;
    private readonly INavigator _navigator;
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;
    private readonly IValidator<CreatePlatformViewModel> _validator;

    public override ViewType Section => ViewType.Platforms;
    protected override IValidator<CreatePlatformViewModel> Validator => _validator;

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

    // ══════ Collections ══════
    public ObservableCollection<CreateSelectorViewModel> Selectors { get; } = [];

    public SelectorContentType[] ContentTypes { get; } =
        Enum.GetValues<SelectorContentType>();

    public SelectorType[] SelectorTypes { get; } =
        Enum.GetValues<SelectorType>();

    // ══════ Commands ══════
    public ICommand AddSelectorCommand { get; }
    public ICommand RemoveSelectorCommand { get; }
    public ICommand MoveSelectorUpCommand { get; }
    public ICommand MoveSelectorDownCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    // ══════ Constructor ══════
    public CreatePlatformViewModel(
        IPlatformService platformService,
        IDialogService dialogService,
        INavigator navigator,
        IViewModelAbstractFactory viewModelAbstractFactory,
        IValidator<CreatePlatformViewModel> validator)
    {
        _platformService = platformService;
        _dialogService = dialogService;
        _navigator = navigator;
        _viewModelAbstractFactory = viewModelAbstractFactory;
        _validator = validator;

        AddSelectorCommand = new RelayCommand(_ => AddSelector());

        RemoveSelectorCommand = new RelayCommand(
            execute: p => RemoveSelector((CreateSelectorViewModel)p!),
            canExecute: p => p is CreateSelectorViewModel);

        MoveSelectorUpCommand = new RelayCommand(
            execute: p => MoveSelectorUp((CreateSelectorViewModel)p!),
            canExecute: p => p is CreateSelectorViewModel s && Selectors.IndexOf(s) > 0);

        MoveSelectorDownCommand = new RelayCommand(
            execute: p => MoveSelectorDown((CreateSelectorViewModel)p!),
            canExecute: p => p is CreateSelectorViewModel s
                             && Selectors.IndexOf(s) >= 0
                             && Selectors.IndexOf(s) < Selectors.Count - 1);

        SaveCommand = new AsyncRelayCommand(async _ => await SaveAsync());
        CancelCommand = new AsyncRelayCommand(async _ => await CancelAsync());
    }

    // ══════ Selector Operations ══════
    private void AddSelector()
    {
        Selectors.Add(new CreateSelectorViewModel());
        RecalculatePriorities();
    }

    private void RemoveSelector(CreateSelectorViewModel selector)
    {
        Selectors.Remove(selector);
        RecalculatePriorities();
    }

    private void MoveSelectorUp(CreateSelectorViewModel selector)
    {
        var index = Selectors.IndexOf(selector);
        if (index <= 0) return;

        Selectors.Move(index, index - 1);
        RecalculatePriorities();
    }

    private void MoveSelectorDown(CreateSelectorViewModel selector)
    {
        var index = Selectors.IndexOf(selector);
        if (index < 0 || index >= Selectors.Count - 1) return;

        Selectors.Move(index, index + 1);
        RecalculatePriorities();
    }

    private void RecalculatePriorities()
    {
        for (int i = 0; i < Selectors.Count; i++)
        {
            Selectors[i].Priority = i + 1;
            Selectors[i].DisplayOrder = i + 1;
        }
    }

    // ══════ Save ══════
    private async Task SaveAsync()
    {
        try
        {
            ValidateAll();
            if (HasErrors) return;

            RecalculatePriorities();

            var request = new PlatformRequest
            {
                Name = Name,
                Url = Url,
                ImageUrl = ImageUrl,
                Selectors = Selectors.Adapt<List<SelectorRequest>>()
            };

            await _platformService.CreateAsync(request);
            await _dialogService.ShowInfoAsync("تم إنشاء المنصة بنجاح ✓", "نجح الحفظ");
            NavigateBack();
        }
        catch (ApiException apiEx)
        {
            var errors = JsonConvert.DeserializeObject<ProblemDetails>(apiEx.Content!);
            await _dialogService.ShowErrorAsync(
                errors!.Errors.First().Value.First()!, "خطأ أثناء الحفظ");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ أثناء الحفظ");
        }
    }

    // ══════ Cancel ══════
    private async Task CancelAsync()
    {
        bool hasData = !string.IsNullOrWhiteSpace(Name)
                    || !string.IsNullOrWhiteSpace(Url)
                    || !string.IsNullOrWhiteSpace(ImageUrl)
                    || Selectors.Count > 0;

        if (hasData)
        {
            if (!await _dialogService.ShowConfirmAsync(
                "هل تريد إلغاء العملية؟ سيتم فقدان جميع البيانات المدخلة.",
                "تأكيد الإلغاء"))
                return;
        }

        NavigateBack();
    }

    // ══════ Navigation ══════
    private void NavigateBack()
    {
        var vm = _viewModelAbstractFactory.CreateViewModel(ViewType.Platforms);
        if (vm is IAsyncInitializable init) _ = init.InitializeAsync();
        _navigator.CurrentViewModel = vm;
    }
}