using Mapster;
using Newtonsoft.Json;
using Refit;
using SA.Accounting.Core.Contracts.Account.Requests;
using SA.Accounting.Core.Contracts.Company.Requests;
using SA.Accounting.Core.Contracts.Owner.Requests;
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

public class UpdateCompanyViewModel : ValidatableViewModel<UpdateCompanyViewModel>, IAsyncInitializable<int>
{
    private readonly ICompanyService _companyService;
    private readonly IPlatformService _platformService;
    private readonly IDialogService _dialogService;
    private readonly INavigator _navigator;
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;
    private readonly IValidator<UpdateCompanyViewModel> _validator;
    private readonly IValidator<UpdateOwnerViewModel> _ownerValidator;
    private readonly IValidator<UpdateAccountViewModel> _accountValidator;

    public override ViewType Section => ViewType.Companies;
    protected override IValidator<UpdateCompanyViewModel> Validator => _validator;

    // ══════ Properties ══════
    private int _companyId;

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
    public ObservableCollection<UpdateOwnerViewModel> Owners { get; } = [];
    public ObservableCollection<UpdateAccountViewModel> Accounts { get; } = [];
    public ObservableCollection<PlatformResponse> Platforms { get; } = [];

    // ══════ Commands ══════
    public ICommand AddOwnerCommand { get; }
    public ICommand RemoveOwnerCommand { get; }
    public ICommand AddAccountCommand { get; }
    public ICommand RemoveAccountCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public UpdateCompanyViewModel(
        ICompanyService companyService,
        IPlatformService platformService,
        IDialogService dialogService,
        INavigator navigator,
        IViewModelAbstractFactory viewModelAbstractFactory,
        IValidator<UpdateCompanyViewModel> validator,
        IValidator<UpdateOwnerViewModel> ownerValidator,
        IValidator<UpdateAccountViewModel> accountValidator)
    {
        _companyService = companyService;
        _platformService = platformService;
        _dialogService = dialogService;
        _navigator = navigator;
        _viewModelAbstractFactory = viewModelAbstractFactory;
        _validator = validator;
        _ownerValidator = ownerValidator;
        _accountValidator = accountValidator;

        AddOwnerCommand = new RelayCommand((_) => Owners.Add(new UpdateOwnerViewModel(_ownerValidator)));
        RemoveOwnerCommand = new RelayCommand((o) => Owners.Remove((UpdateOwnerViewModel)o));

        AddAccountCommand = new RelayCommand((_) => Accounts.Add(new UpdateAccountViewModel(_accountValidator)));
        RemoveAccountCommand = new RelayCommand((a) => Accounts.Remove((UpdateAccountViewModel)a));

        SaveCommand = new AsyncRelayCommand(async (_) => await SaveAsync());
        CancelCommand = new AsyncRelayCommand(async (_) => await CancelAsync());
    }

    // ══════ Initialize ══════
    public async Task InitializeAsync(int companyId)
    {
        _companyId = companyId;
        await LoadPlatformsAsync();
        await LoadCompanyAsync();
    }

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

    private async Task LoadCompanyAsync()
    {
        try
        {
            var company = await _companyService.GetByIdAsync(_companyId);
            if (company is null) return;

            Name = company.Name;
            TaxRegistrationNumber = company.TaxRegistrationNumber;
            TaxFileNumber = company.TaxFileNumber;
            Address = company.Address;

            Owners.Clear();
            foreach (var o in company.Owners ?? [])
            {
                var vm = new UpdateOwnerViewModel(_ownerValidator)
                {
                    Id = o.Id,
                    Name = o.Name,
                    SSN = o.SSN
                };
                Owners.Add(vm);
            }

            Accounts.Clear();
            foreach (var a in company.Accounts ?? [])
            {
                var vm = new UpdateAccountViewModel(_accountValidator)
                {
                    Id = a.Id,
                    Email = a.Email,
                    Password = a.Password,
                    PlatformId = a.Platform.Id
                };
                Accounts.Add(vm);
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ في تحميل بيانات الشركة");
        }
    }

    // ══════ Save ══════
    private async Task SaveAsync()
    {
        try
        {
            // Validate all
            ValidateAll();
            foreach (var o in Owners) o.ValidateAll();
            foreach (var a in Accounts) a.ValidateAll();

            bool allValid = !HasErrors
                         && Owners.All(o => !o.HasErrors)
                         && Accounts.All(a => !a.HasErrors);

            if (!allValid)
            {
                await _dialogService.ShowWarningAsync("يرجى تصحيح الأخطاء قبل الحفظ", "تحقق من البيانات");
                return;
            }

            var request = new UpdateCompanyRequest
            {
                Name = Name,
                TaxRegistrationNumber = TaxRegistrationNumber,
                TaxFileNumber = TaxFileNumber,
                Address = Address,
                Owners = Owners.Select(o => o.Adapt<UpdateOwnerRequest>()).ToList(),
                Accounts = Accounts.Select(a => a.Adapt<UpdateAccountRequest>()).ToList()
            };

            await _companyService.UpdateAsync(_companyId, request);
            await _dialogService.ShowInfoAsync("تم تعديل بيانات الشركة بنجاح ✓", "نجح الحفظ");
            NavigateBack();
        }
        catch(ApiException apiEx)
        {
            var errors = JsonConvert.DeserializeObject<ProblemDetails>(apiEx.Content);
            await _dialogService.ShowErrorAsync(errors.Errors.First().Value.First(),"حدث خطأ اثناء الحفظ");
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
            "هل تريد إلغاء العملية؟ سيتم فقدان جميع التعديلات.", "تأكيد الإلغاء"))
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