using SA.Accounting.Core.Contracts.Common;
using SA.Accounting.Core.Contracts.User.Responses;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public sealed class UsersViewModel : ViewModelBase, IAsyncInitializable
{
    private readonly IUserService _userService;
    private readonly INavigator _navigator;
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;
    private readonly IDialogService _dialogService;

    public override ViewType Section => ViewType.Users;

    // ══════ Data ══════
    public ObservableCollection<UserResponse> Users { get; } = [];

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

    public bool HasNoUsers => Users.Count == 0;

    // ══════ Commands ══════
    public ICommand AddUserCommand { get; }
    public ICommand UpdateUserCommand { get; }
    public ICommand DeleteUserCommand { get; }
    public ICommand NextPageCommand { get; }
    public ICommand PreviousPageCommand { get; }

    public UsersViewModel(
        IUserService userService,
        INavigator navigator,
        IViewModelAbstractFactory viewModelAbstractFactory,
        IDialogService dialogService)
    {
        _userService = userService;
        _navigator = navigator;
        _viewModelAbstractFactory = viewModelAbstractFactory;
        _dialogService = dialogService;

        AddUserCommand = new AsyncRelayCommand(async (_) => await AddUserAsync());
        UpdateUserCommand = new AsyncRelayCommand(async (u) => await UpdateUserAsync((UserResponse)u!));
        DeleteUserCommand = new AsyncRelayCommand(async (u) => await DeleteUserAsync((UserResponse)u!));

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
            var result = await _userService.GetAllAsync(new RequestFilters
            {
                PageNumber = CurrentPage,
                PageSize = 10,
                SearchValue = string.IsNullOrWhiteSpace(SearchText) ? null : SearchText.Trim()
            });

            Users.Clear();
            foreach (var u in result.Items) Users.Add(u);

            TotalCount = result.TotalCount;
            TotalPages = result.TotalPages;
            OnPropertyChanged(nameof(HasNoUsers));
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ في تحميل المستخدمين");
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

    // ══════ Add (Dialog) ══════
    private async Task AddUserAsync()
    {
        var vm = _viewModelAbstractFactory.CreateViewModel(ViewType.CreateUser);
        bool saved = await _dialogService.ShowDialogAsync(vm);
        if (saved) await LoadAsync();
    }

    // ══════ Update ══════
    private async Task UpdateUserAsync(UserResponse user)
    {
        var vm = (UpdateUserViewModel)_viewModelAbstractFactory.CreateViewModel(ViewType.UpdateUser);
        await vm.InitializeAsync(user.Id);
        _navigator.CurrentViewModel = vm;
    }

    // ══════ Delete ══════
    private async Task DeleteUserAsync(UserResponse user)
    {
        try
        {
            if (!await _dialogService.ShowConfirmAsync(
                $"هل أنت متأكد من حذف \"{user.Name}\"؟", "تأكيد"))
                return;

            await _userService.ToggleStatusAsync(user.Id);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ");
        }
    }
}