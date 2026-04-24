using SA.Accounting.Core.Contracts.Transaction.Responses;
using SA.Accounting.Core.Contracts.TransactionItem.Responses;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public class DisplayTransactionViewModel : ViewModelBase, IAsyncInitializable<int>
{
    private readonly ITransactionService _transactionService;
    private readonly INavigator _navigator;
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;
    private readonly IDialogService _dialogService;

    public override ViewType Section => ViewType.Transactions;

    // ══════ Data ══════
    private TransactionDetailResponse? _transaction;
    public TransactionDetailResponse? Transaction
    {
        get => _transaction;
        private set { _transaction = value; OnPropertyChanged(); }
    }

    public ObservableCollection<TransactionItemResponse> Items { get; } = [];
    public ObservableCollection<TransactionHistoryResponse> Histories { get; } = [];

    public bool HasNoItems => Items.Count == 0;
    public bool HasNoHistories => Histories.Count == 0;

    // ══════ Computed ══════
    public decimal TotalAmount => Items.Sum(i => i.Amount);

    // ══════ Commands ══════
    public ICommand GoBackCommand { get; }
    public ICommand EditCommand { get; }

    public DisplayTransactionViewModel(
        ITransactionService transactionService,
        INavigator navigator,
        IViewModelAbstractFactory viewModelAbstractFactory,
        IDialogService dialogService)
    {
        _transactionService = transactionService;
        _navigator = navigator;
        _viewModelAbstractFactory = viewModelAbstractFactory;
        _dialogService = dialogService;

        GoBackCommand = new AsyncRelayCommand(async (_) => await NavigateBackAsync());
        EditCommand = new AsyncRelayCommand(async (_) => await NavigateToEditAsync());
    }

    public async Task InitializeAsync(int transactionId)
    {
        try
        {
            var result = await _transactionService.GetByIdAsync(transactionId);
            Transaction = result;

            Items.Clear();
            foreach (var item in result.Items) Items.Add(item);

            Histories.Clear();
            foreach (var h in result.Histories) Histories.Add(h);

            OnPropertyChanged(nameof(HasNoItems));
            OnPropertyChanged(nameof(HasNoHistories));
            OnPropertyChanged(nameof(TotalAmount));
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ في تحميل البيانات");
        }
    }

    private async Task NavigateBackAsync()
    {
        var vm = _viewModelAbstractFactory.CreateViewModel(ViewType.Transactions);
        if (vm is IAsyncInitializable init) await init.InitializeAsync();
        _navigator.CurrentViewModel = vm;
    }

    private async Task NavigateToEditAsync()
    {
        if (Transaction == null) return;
        var vm = _viewModelAbstractFactory.CreateViewModel(ViewType.UpdateTransaction);
        if (vm is IAsyncInitializable<int> init) await init.InitializeAsync(Transaction.Id);
        _navigator.CurrentViewModel = vm;
    }
}