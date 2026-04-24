using Mapster;
using Newtonsoft.Json;
using Refit;
using SA.Accounting.Core.Contracts.Common;
using SA.Accounting.Core.Contracts.Company.Responses;
using SA.Accounting.Core.Contracts.Transaction.Requests;
using SA.Accounting.Core.Contracts.TransactionCategory.Responses;
using SA.Accounting.Core.Contracts.TransactionItem.Requests;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using SA.Accounting.WPF.ViewModels.TransactionItem;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public class UpdateTransactionViewModel : ValidatableViewModel<UpdateTransactionViewModel>, IAsyncInitializable<int>
{
    private readonly ITransactionService _transactionService;
    private readonly ITransactionCategoryService _categoryService;
    private readonly ICompanyService _companyService;
    private readonly IDialogService _dialogService;
    private readonly INavigator _navigator;
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;
    private readonly IValidator<UpdateTransactionViewModel> _validator;
    private readonly IValidator<UpdateTransactionItemViewModel> _itemValidator;

    public override ViewType Section => ViewType.Transactions;
    protected override IValidator<UpdateTransactionViewModel> Validator => _validator;

    // ══════ Id ══════
    private int _id;
    public int Id
    {
        get => _id;
        private set { _id = value; OnPropertyChanged(); }
    }

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
    public ObservableCollection<UpdateTransactionItemViewModel> Items { get; } = [];
    public ObservableCollection<TransactionCategoryResponse> Categories { get; } = [];
    public ObservableCollection<CompanyResponse> Companies { get; } = [];

    // ══════ Computed ══════
    public decimal TotalAmount => Items.Sum(i => i.Amount);

    // ══════ Commands ══════
    public ICommand AddItemCommand { get; }
    public ICommand RemoveItemCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public UpdateTransactionViewModel(
        ITransactionService transactionService,
        ITransactionCategoryService categoryService,
        ICompanyService companyService,
        IDialogService dialogService,
        INavigator navigator,
        IViewModelAbstractFactory viewModelAbstractFactory,
        IValidator<UpdateTransactionViewModel> validator,
        IValidator<UpdateTransactionItemViewModel> itemValidator)
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
        RemoveItemCommand = new RelayCommand((item) => RemoveItem((UpdateTransactionItemViewModel)item!));
        SaveCommand = new AsyncRelayCommand(async (_) => await SaveAsync());
        CancelCommand = new AsyncRelayCommand(async (_) => await CancelAsync());
    }

    public async Task InitializeAsync(int transactionId)
    {
        await LoadCategoriesAsync();
        await LoadCompaniesAsync();
        await LoadTransactionAsync(transactionId);
    }

    // ══════ Load ══════
    private async Task LoadTransactionAsync(int transactionId)
    {
        try
        {
            var result = await _transactionService.GetByIdAsync(transactionId);

            Id = result.Id;
            Number = result.Number;
            DateTime = result.DateTime;
            Note = result.Note;

            Items.Clear();
            foreach (var item in result.Items)
            {
                var vm = new UpdateTransactionItemViewModel(_itemValidator)
                {
                    Id = item.Id,
                    Note = item.Note,
                    FileUrl = item.FileUrl,
                    Amount = item.Amount,
                    TransactionCategoryId = item.TransactionCategoryId,
                    CompanyId = item.CompanyId
                };
                vm.PropertyChanged += (_, e) =>
                {
                    if (e.PropertyName == nameof(UpdateTransactionItemViewModel.Amount))
                        OnPropertyChanged(nameof(TotalAmount));
                };
                Items.Add(vm);
            }

            OnPropertyChanged(nameof(TotalAmount));
            //ClearErrors();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ في تحميل البيانات");
        }
    }

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
            var result = (await _companyService.GetAllAsync(new RequestFilters())).Items;
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
        var item = new UpdateTransactionItemViewModel(_itemValidator)
        {
            Id = null // بند جديد
        };
        item.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(UpdateTransactionItemViewModel.Amount))
                OnPropertyChanged(nameof(TotalAmount));
        };
        Items.Add(item);
        OnPropertyChanged(nameof(TotalAmount));
    }

    private void RemoveItem(UpdateTransactionItemViewModel item)
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

            var request = new UpdateTransactionRequest
            {
                Id = Id,
                Number = Number,
                DateTime = DateTime,
                Note = Note,
                Items = Items.Select(i => new UpdateTransactionItemRequest
                {
                    Id = i.Id,
                    Note = i.Note,
                    FileUrl = i.FileUrl,
                    Amount = i.Amount,
                    TransactionCategoryId = i.TransactionCategoryId,
                    CompanyId = i.CompanyId
                }).ToList()
            };

            await _transactionService.UpdateAsync(_id,request);

            await _dialogService.ShowInfoAsync("تم تعديل المعاملة بنجاح ✓", "نجح الحفظ");
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
            "هل تريد إلغاء التعديل؟ سيتم فقدان التغييرات.", "تأكيد الإلغاء"))
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