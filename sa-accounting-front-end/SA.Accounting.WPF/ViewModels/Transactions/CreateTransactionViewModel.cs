using Mapster;
using Newtonsoft.Json;
using Refit;
using SA.Accounting.Core.Contracts.Company.Responses;
using SA.Accounting.Core.Contracts.Transaction.Requests;
using SA.Accounting.Core.Contracts.TransactionCategory.Responses;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public class CreateTransactionViewModel : ValidatableViewModel<CreateTransactionViewModel>, IAsyncInitializable
{
    private readonly ITransactionService _transactionService;
    private readonly ITransactionCategoryService _categoryService;
    private readonly ICompanyService _companyService;
    private readonly IDialogService _dialogService;
    private readonly INavigator _navigator;
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;
    private readonly IAccountStore _accountStore;
    private readonly IValidator<CreateTransactionViewModel> _validator;
    private readonly IValidator<CreateTransactionItemViewModel> _itemValidator;

    public override ViewType Section => ViewType.Transactions;
    protected override IValidator<CreateTransactionViewModel> Validator => _validator;

    // ══════ Properties ══════
    private string _number = string.Empty;
    public string Number
    {
        get => _number;
        set { _number = value; OnPropertyChanged(); ValidateProperty(); }
    }

    private DateTime _dateTime = DateTime.Today;
    public DateTime DateTime
    {
        get => _dateTime;
        set { _dateTime = value; OnPropertyChanged(); ValidateProperty(); }
    }

    private string _note = string.Empty;
    public string Note
    {
        get => _note;
        set { _note = value; OnPropertyChanged(); ValidateProperty(); }
    }

    // ══════ Collections ══════
    public ObservableCollection<CreateTransactionItemViewModel> Items { get; } = [];
    public ObservableCollection<TransactionCategoryResponse> Categories { get; } = [];
    public ObservableCollection<CompanyResponse> Companies { get; } = [];

    // ══════ Computed ══════
    public decimal TotalAmount => Items.Sum(i => i.Amount);

    // ══════ Commands ══════
    public ICommand AddItemCommand { get; }
    public ICommand RemoveItemCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public CreateTransactionViewModel(
        ITransactionService transactionService,
        ITransactionCategoryService categoryService,
        ICompanyService companyService,
        IDialogService dialogService,
        INavigator navigator,
        IViewModelAbstractFactory viewModelAbstractFactory,
        IValidator<CreateTransactionViewModel> validator,
        IValidator<CreateTransactionItemViewModel> itemValidator,
        IAccountStore accountStore)
    {
        _transactionService = transactionService;
        _categoryService = categoryService;
        _companyService = companyService;
        _dialogService = dialogService;
        _navigator = navigator;
        _viewModelAbstractFactory = viewModelAbstractFactory;
        _validator = validator;
        _itemValidator = itemValidator;

        AddItemCommand = new RelayCommand((_) => AddItem());
        RemoveItemCommand = new RelayCommand((item) => RemoveItem((CreateTransactionItemViewModel)item!));
        SaveCommand = new AsyncRelayCommand(async (_) => await SaveAsync());
        CancelCommand = new AsyncRelayCommand(async (_) => await CancelAsync());
        _accountStore = accountStore;
    }

    public async Task InitializeAsync()
    {
        await LoadCategoriesAsync();
        await LoadCompaniesAsync();
    }

    // ══════ Load Lookups ══════
    private async Task LoadCategoriesAsync()
    {
        try
        {
            var result = await _categoryService.GetAllAsync(false);
            Categories.Clear();
            foreach (var c in result ?? []) Categories.Add(c);
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ في تحميل التصنيفات");
        }
    }

    private async Task LoadCompaniesAsync()
    {
        try
        {
            var result = (await _companyService.GetAllAsync(new Core.Contracts.Common.RequestFilters())).Items;
            Companies.Clear();
            foreach (var c in result ?? []) Companies.Add(c);
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ في تحميل الشركات");
        }
    }

    // ══════ Items ══════
    private void AddItem()
    {
        var item = new CreateTransactionItemViewModel(_itemValidator);
        item.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(CreateTransactionItemViewModel.Amount))
                OnPropertyChanged(nameof(TotalAmount));
        };
        Items.Add(item);
        OnPropertyChanged(nameof(TotalAmount));
    }

    private void RemoveItem(CreateTransactionItemViewModel item)
    {
        Items.Remove(item);
        OnPropertyChanged(nameof(TotalAmount));
    }

    // ══════ Save ══════
    private async Task SaveAsync()
    {
        try
        {
            ValidateAll();

            bool itemsHaveErrors = false;
            foreach (var item in Items)
            {
                item.ValidateAll();
                if (item.HasErrors) itemsHaveErrors = true;
            }

            if (HasErrors || itemsHaveErrors) return;

            var request = this.Adapt<CreateTransactionRequest>();
            await _transactionService.CreateAsync(_accountStore.CurrentUserResponse.UserId, request,CancellationToken.None);

            await _dialogService.ShowInfoAsync("تم إنشاء المعاملة بنجاح ✓", "نجح الحفظ");
            NavigateBack();
        }
        catch (ApiException apiEx)
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
        var vm = _viewModelAbstractFactory.CreateViewModel(ViewType.Transactions);
        if (vm is IAsyncInitializable init) _ = init.InitializeAsync();
        _navigator.CurrentViewModel = vm;
    }
}