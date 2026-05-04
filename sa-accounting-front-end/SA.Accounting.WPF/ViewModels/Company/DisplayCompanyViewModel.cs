using Mapster;
using SA.Accounting.Core.Contracts.Account.Responses;
using SA.Accounting.Core.Contracts.Owner.Responses;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public class DisplayCompanyViewModel : ViewModelBase, IAsyncInitializable<int>
{
    private readonly ICompanyService _companyService;
    private readonly IDialogService _dialogService;
    private readonly INavigator _navigator;
    private readonly IAccountAutomationService _accountAutomationService;
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;
    public override ViewType Section => ViewType.Companies;

    // ══════ Properties ══════
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    private string _taxRegistrationNumber = string.Empty;
    public string TaxRegistrationNumber
    {
        get => _taxRegistrationNumber;
        set { _taxRegistrationNumber = value; OnPropertyChanged(); }
    }

    private string _taxFileNumber = string.Empty;
    public string TaxFileNumber
    {
        get => _taxFileNumber;
        set { _taxFileNumber = value; OnPropertyChanged(); }
    }

    private string _address = string.Empty;
    public string Address
    {
        get => _address;
        set { _address = value; OnPropertyChanged(); }
    }

    private bool _isDeleted;
    public bool IsDeleted
    {
        get => _isDeleted;
        set { _isDeleted = value; OnPropertyChanged(); }
    }

    // ══════ Collections ══════
    public ObservableCollection<OwnerResponse> Owners { get; } = [];
    public ObservableCollection<AccountResponse> Accounts { get; } = [];

    // ══════ Commands ══════
    public ICommand OpenPlatformCommand { get; }
    public ICommand CopyToClipboardCommand { get; }
    public ICommand BackCommand { get; }

    public DisplayCompanyViewModel(
        ICompanyService companyService,
        IDialogService dialogService,
        INavigator navigator,
        IAccountAutomationService accountAutomationService,
        IViewModelAbstractFactory viewModelAbstractFactory)
    {
        _accountAutomationService = accountAutomationService;
        _companyService = companyService;
        _dialogService = dialogService;
        _navigator = navigator;
        _viewModelAbstractFactory = viewModelAbstractFactory;

        OpenPlatformCommand = new RelayCommand(
            execute: async p =>
            {
                if (p is AccountResponse account && !string.IsNullOrWhiteSpace(account.Platform?.Url))
                {
                    try {

                        await _accountAutomationService.OpenAsync(account);
                    }
                    catch(Exception ex) {
                        await _dialogService.ShowErrorAsync(ex.Message,"حدث خطأ");
                    }
                }
            },
            canExecute: p => p is AccountResponse a && !string.IsNullOrWhiteSpace(a.Platform?.Url));

        CopyToClipboardCommand = new RelayCommand(p =>
        {
            if (p is string text && !string.IsNullOrEmpty(text))
                Clipboard.SetText(text);
        });

        BackCommand = new AsyncRelayCommand(async (_) => await NavigateBackAsync());
    }

    // ══════ Initialize ══════
    public async Task InitializeAsync(int companyId)
    {
        try
        {
            var company = await _companyService.GetByIdAsync(companyId);
            company.Adapt(this);

            Owners.Clear();
            foreach (var o in company.Owners ?? []) Owners.Add(o);

            Accounts.Clear();
            foreach (var a in company.Accounts ?? []) Accounts.Add(a);
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ في تحميل بيانات الشركة");
        }
    }

    // ══════ Navigate Back ══════
    private async Task NavigateBackAsync()
    {
        var vm = _viewModelAbstractFactory.CreateViewModel(ViewType.Companies);
        if (vm is IAsyncInitializable init) await init.InitializeAsync();
        _navigator.CurrentViewModel = vm;
    }
}