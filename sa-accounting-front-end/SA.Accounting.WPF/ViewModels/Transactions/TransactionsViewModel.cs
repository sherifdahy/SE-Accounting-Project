using SA.Accounting.Core.Contracts.Common;
using SA.Accounting.Core.Contracts.Transaction.Responses;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public class TransactionsViewModel : ViewModelBase, IAsyncInitializable
{
    private readonly ITransactionService _transactionService;
    private readonly INavigator _navigator;
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;
    private readonly IDialogService _dialogService;

    public override ViewType Section => ViewType.Transactions;

    // ══════ Data ══════
    public ObservableCollection<TransactionResponse> Transactions { get; } = [];

    // ══════ Search ══════
    private CancellationTokenSource? _searchCts;
    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            _ = DebounceSearchAsync();
        }
    }

    // ══════ Pagination ══════
    private int _currentPage = 1;
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(PaginationText));
        }
    }

    private int _totalPages;
    public int TotalPages
    {
        get => _totalPages;
        private set
        {
            _totalPages = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(PaginationText));
        }
    }

    private int _totalCount;
    public int TotalCount
    {
        get => _totalCount;
        private set { _totalCount = value; OnPropertyChanged(); }
    }

    public string PaginationText => TotalPages > 0
        ? $"صفحة {CurrentPage} من {TotalPages} — إجمالي {TotalCount}"
        : string.Empty;

    public bool HasNoTransactions => Transactions.Count == 0;

    // ══════ Commands ══════
    public ICommand AddTransactionCommand { get; }
    public ICommand DisplayTransactionCommand { get; }
    public ICommand UpdateTransactionCommand { get; }
    public ICommand DeleteTransactionCommand { get; }
    public ICommand NextPageCommand { get; }
    public ICommand PreviousPageCommand { get; }

    public TransactionsViewModel(
        ITransactionService transactionService,
        INavigator navigator,
        IViewModelAbstractFactory viewModelAbstractFactory,
        IDialogService dialogService)
    {
        _transactionService = transactionService;
        _navigator = navigator;
        _viewModelAbstractFactory = viewModelAbstractFactory;
        _dialogService = dialogService;

        AddTransactionCommand = new AsyncRelayCommand(async (_) => await NavigateToAddTransactionAsync());
        DisplayTransactionCommand = new AsyncRelayCommand(async (t) => await NavigateToDisplayTransactionAsync((TransactionResponse)t!));
        UpdateTransactionCommand = new AsyncRelayCommand(async (t) => await NavigateToUpdateTransactionAsync((TransactionResponse)t!));
        DeleteTransactionCommand = new AsyncRelayCommand(async (t) => await DeleteTransactionAsync((TransactionResponse)t!));

        NextPageCommand = new AsyncRelayCommand(async (_) =>
        {
            if (CurrentPage < TotalPages) { CurrentPage++; await LoadAsync(); }
        });

        PreviousPageCommand = new AsyncRelayCommand(async (_) =>
        {
            if (CurrentPage > 1) { CurrentPage--; await LoadAsync(); }
        });
    }

    public async Task InitializeAsync() => await LoadAsync();

    // ══════ Load ══════
    private async Task LoadAsync()
    {
        try
        {
            var result = await _transactionService.GetAllAsync(
                new RequestFilters
                {
                    PageNumber = CurrentPage,
                    PageSize = 10,
                    SortColumn = nameof(TransactionResponse.Id),
                    SortDirection = "DESC",
                    SearchValue = string.IsNullOrWhiteSpace(SearchText) ? null : SearchText.Trim()
                },
                CancellationToken.None);

            Transactions.Clear();
            foreach (var t in result.Items) Transactions.Add(t);

            TotalCount = result.TotalCount;
            TotalPages = result.TotalPages;
            OnPropertyChanged(nameof(HasNoTransactions));
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ في تحميل البيانات");
        }
    }

    // ══════ Debounced Search ══════
    private async Task DebounceSearchAsync()
    {
        _searchCts?.Cancel();
        _searchCts = new CancellationTokenSource();
        try
        {
            await Task.Delay(400, _searchCts.Token);
            CurrentPage = 1;
            await LoadAsync();
        }
        catch (TaskCanceledException) { }
    }

    // ══════ Navigation ══════
    private async Task NavigateToAddTransactionAsync()
    {
        var vm = _viewModelAbstractFactory.CreateViewModel(ViewType.CreateTransaction);
        if (vm is IAsyncInitializable init) await init.InitializeAsync();
        _navigator.CurrentViewModel = vm;
    }

    private async Task NavigateToDisplayTransactionAsync(TransactionResponse transaction)
    {
        var vm = _viewModelAbstractFactory.CreateViewModel(ViewType.DisplayTransaction);
        if (vm is IAsyncInitializable<int> init) await init.InitializeAsync(transaction.Id);
        _navigator.CurrentViewModel = vm;
    }

    private async Task NavigateToUpdateTransactionAsync(TransactionResponse transaction)
    {
        var vm = _viewModelAbstractFactory.CreateViewModel(ViewType.UpdateTransaction);
        if (vm is IAsyncInitializable<int> init) await init.InitializeAsync(transaction.Id);
        _navigator.CurrentViewModel = vm;
    }

    // ══════ Delete ══════
    private async Task DeleteTransactionAsync(TransactionResponse transaction)
    {
        try
        {
            if (!await _dialogService.ShowConfirmAsync(
                $"هل أنت متأكد من حذف المعاملة \"{transaction.Number}\"؟", "تأكيد"))
                return;

            await _transactionService.RemoveAsync(transaction.Id);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ");
        }
    }
}