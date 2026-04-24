using SA.Accounting.Core.Contracts.Common;
using SA.Accounting.Core.Contracts.Company.Responses;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public class CompaniesViewModel : ViewModelBase, IAsyncInitializable
{
    private readonly ICompanyService _companyService;
    private readonly INavigator _navigator;
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;
    private readonly IDialogService _dialogService;

    public override ViewType Section => ViewType.Companies;

    // ══════ Data ══════
    public ObservableCollection<CompanyResponse> Companies { get; } = [];

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

    public bool HasNoCompanies => Companies.Count == 0;

    // ══════ Commands ══════
    public ICommand AddCompanyCommand { get; }
    public ICommand UpdateCompanyCommand { get; }
    public ICommand DeleteCompanyCommand { get; }
    public ICommand DisplayCompanyCommand { get; }
    public ICommand NextPageCommand { get; }
    public ICommand PreviousPageCommand { get; }

    public CompaniesViewModel(
        ICompanyService companyService,
        INavigator navigator,
        IViewModelAbstractFactory viewModelAbstractFactory,
        IDialogService dialogService)
    {
        _companyService = companyService;
        _navigator = navigator;
        _viewModelAbstractFactory = viewModelAbstractFactory;
        _dialogService = dialogService;

        AddCompanyCommand = new AsyncRelayCommand(async (_) => await NavigateToAddCompanyAsync());
        UpdateCompanyCommand = new AsyncRelayCommand(async (c) => await NavigateToUpdateCompanyAsync((CompanyResponse)c!));
        DeleteCompanyCommand = new AsyncRelayCommand(async (c) => await DeleteCompanyAsync((CompanyResponse)c!));
        DisplayCompanyCommand = new AsyncRelayCommand(async (c) => await NavigateToDisplayCompanyAsync((CompanyResponse)c!));

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
            var result = await _companyService.GetAllAsync(new RequestFilters
            {
                PageNumber = CurrentPage,
                PageSize = 10,
                SearchValue = string.IsNullOrWhiteSpace(SearchText) ? null : SearchText.Trim()
            });

            Companies.Clear();
            foreach (var c in result.Items) Companies.Add(c);

            TotalCount = result.TotalCount;
            TotalPages = result.TotalPages;
            OnPropertyChanged(nameof(HasNoCompanies));
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
    private async Task NavigateToAddCompanyAsync()
    {
        var vm = _viewModelAbstractFactory.CreateViewModel(ViewType.CreateCompany);
        if (vm is IAsyncInitializable init) await init.InitializeAsync();
        _navigator.CurrentViewModel = vm;
    }

    private async Task NavigateToUpdateCompanyAsync(CompanyResponse company)
    {
        var vm = _viewModelAbstractFactory.CreateViewModel(ViewType.UpdateCompany);
        if (vm is IAsyncInitializable<int> init) await init.InitializeAsync(company.Id);
        _navigator.CurrentViewModel = vm;
    }

    private async Task NavigateToDisplayCompanyAsync(CompanyResponse company)
    {
        var vm = _viewModelAbstractFactory.CreateViewModel(ViewType.DisplayCompany);
        if (vm is IAsyncInitializable<int> init) await init.InitializeAsync(company.Id);
        _navigator.CurrentViewModel = vm;
    }

    // ══════ Delete ══════
    private async Task DeleteCompanyAsync(CompanyResponse company)
    {
        try
        {
            if (!await _dialogService.ShowConfirmAsync(
                $"هل أنت متأكد من حذف \"{company.Name}\"؟", "تأكيد"))
                return;

            await _companyService.ToggleStatusAsync(company.Id);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ");
        }
    }
}