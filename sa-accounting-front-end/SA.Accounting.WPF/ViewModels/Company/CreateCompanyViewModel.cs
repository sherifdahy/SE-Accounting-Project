using Mapster;
using Newtonsoft.Json;
using Refit;
using SA.Accounting.Core.Contracts.Company.Requests;
using SA.Accounting.Core.Contracts.Platform.Responses;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Account;
using SA.Accounting.WPF.ViewModels.Base;
using SA.Accounting.WPF.ViewModels.Owner;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public class CreateCompanyViewModel : ValidatableViewModel<CreateCompanyViewModel>, IAsyncInitializable
{
    private readonly ICompanyService _companyService;
    private readonly IPlatformService _platformService;
    private readonly IDialogService _dialogService;
    private readonly INavigator _navigator;
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;
    private readonly IValidator<CreateCompanyViewModel> _validator;

    public override ViewType Section => ViewType.Companies;
    protected override IValidator<CreateCompanyViewModel> Validator => _validator;
    private readonly IValidator<CreateOwnerViewModel> _ownerValidator;
    private readonly IValidator<CreateAccountViewModel> _accountValidator;

    // ══════ Properties ══════
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); ValidateProperty(); }
    }

    private string _taxRegistrationNumber = string.Empty;
    public string TaxRegistrationNumber
    {
        get => _taxRegistrationNumber;
        set { _taxRegistrationNumber = value; OnPropertyChanged(); ValidateProperty(); }
    }

    private string _taxFileNumber = string.Empty;
    public string TaxFileNumber
    {
        get => _taxFileNumber;
        set { _taxFileNumber = value; OnPropertyChanged(); ValidateProperty(); }
    }

    private string _address = string.Empty;
    public string Address
    {
        get => _address;
        set { _address = value; OnPropertyChanged(); ValidateProperty(); }
    }

    // ══════ Collections ══════
    public ObservableCollection<CreateOwnerViewModel> Owners { get; } = [];
    public ObservableCollection<CreateAccountViewModel> Accounts { get; } = [];
    public ObservableCollection<PlatformResponse> Platforms { get; } = [];

    // ══════ Commands ══════
    public ICommand AddOwnerCommand { get; }
    public ICommand RemoveOwnerCommand { get; }
    public ICommand AddAccountCommand { get; }
    public ICommand RemoveAccountCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public CreateCompanyViewModel(
        ICompanyService companyService,
        IPlatformService platformService,
        IDialogService dialogService,
        INavigator navigator,
        IViewModelAbstractFactory viewModelAbstractFactory,
        IValidator<CreateCompanyViewModel> validator,
        IValidator<CreateOwnerViewModel> ownerValidator,
        IValidator<CreateAccountViewModel> accountValidator)
    {
        _companyService = companyService;
        _platformService = platformService;
        _dialogService = dialogService;
        _navigator = navigator;
        _viewModelAbstractFactory = viewModelAbstractFactory;
        _validator = validator;
        _ownerValidator = ownerValidator;
        _accountValidator = accountValidator;

        AddOwnerCommand = new RelayCommand((_) => Owners.Add(new CreateOwnerViewModel(_ownerValidator)));
        RemoveOwnerCommand = new RelayCommand((o) => Owners.Remove((CreateOwnerViewModel)o));

        AddAccountCommand = new RelayCommand((_) => Accounts.Add(new CreateAccountViewModel(_accountValidator)));
        RemoveAccountCommand = new RelayCommand((a) => Accounts.Remove((CreateAccountViewModel)a));

        SaveCommand = new AsyncRelayCommand(async (_) => await SaveAsync());
        CancelCommand = new AsyncRelayCommand(async (_) => await CancelAsync());
    }

    public async Task InitializeAsync() => await LoadPlatformsAsync();

    // ══════ Load ══════
    private async Task LoadPlatformsAsync()
    {
        try
        {
            var result = await _platformService.GetAllAsync(false);
            Platforms.Clear();
            foreach (var p in result ?? []) Platforms.Add(p);
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ في تحميل المنصات");
        }
    }

    // ══════ Save ══════
    private async Task SaveAsync()
    {
        try
        {
            ValidateAll();
            if (HasErrors) return;

            var request = this.Adapt<CreateCompanyRequest>();
            await _companyService.CreateAsync(request);

            await _dialogService.ShowInfoAsync("تم إنشاء الشركة بنجاح ✓", "نجح الحفظ");
            NavigateBack();
        }
        catch(ApiException apiEx)
        {
            var errors = JsonConvert.DeserializeObject<ProblemDetails>(apiEx.Content!);
            await _dialogService.ShowErrorAsync(errors!.Errors.First().Value.First()!, "خطأ أثناء الحفظ");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ أثناء الحفظ");
        }
    }

    // ══════ Cancel ══════
    private async Task CancelAsync()
    {
        if (!await _dialogService.ShowConfirmAsync(
            "هل تريد إلغاء العملية؟ سيتم فقدان جميع البيانات المدخلة.", "تأكيد الإلغاء"))
            return;

        NavigateBack();
    }

    private void NavigateBack()
    {
        var vm = _viewModelAbstractFactory.CreateViewModel(ViewType.Companies);
        if (vm is IAsyncInitializable init) _ = init.InitializeAsync();
        _navigator.CurrentViewModel = vm;
    }
}