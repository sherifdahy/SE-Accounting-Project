using SA.Accounting.Core.Contracts.Common;
using SA.Accounting.Core.Contracts.Company.Responses;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using SA.Accounting.WPF.ViewModels.Company;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public sealed class UserCompaniesViewModel : ViewModelBase, IAsyncInitializable<int>
{
    private int _userId;
    private bool _isBusy;
    private bool _isLoadingAvailable;
    private bool _isLoadingAssigned;
    private bool _isAllAvailableLoaded;
    private bool _isAllAssignedLoaded;

    private string _availableSearch = string.Empty;
    private string _assignedSearch = string.Empty;

    private ObservableCollection<SelectableCompanyViewModel> _allCompaniesResponse = new();
    private ObservableCollection<SelectableCompanyViewModel> _assignedCompaniesResponse = new();
    private ObservableCollection<SelectableCompanyViewModel> _filteredAvailable = new();
    private ObservableCollection<SelectableCompanyViewModel> _filteredAssigned = new();

    private readonly IUserService _userService;
    private readonly ICompanyService _companyService;
    private readonly IDialogService _dialogService;

    public UserCompaniesViewModel(
        IUserService userService,
        ICompanyService companyService,
        IDialogService dialogService)
    {
        _userService = userService;
        _companyService = companyService;
        _dialogService = dialogService;

        AddSelectedCommand = new AsyncRelayCommand(async _ => await AddSelectedAsync());
        RemoveSelectedCommand = new AsyncRelayCommand(async _ => await RemoveSelectedAsync());
        AddAllCommand = new AsyncRelayCommand(async _ => await AddAllAsync());
        RemoveAllCommand = new AsyncRelayCommand(async _ => await RemoveAllAsync());
        LoadMoreAvailableCommand = new AsyncRelayCommand(async _ => await LoadMoreAvailableAsync());
        LoadMoreAssignedCommand = new AsyncRelayCommand(async _ => await LoadMoreAssignedAsync());
    }

    #region Properties

    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged();
        }
    }

    public bool IsLoadingAvailable
    {
        get => _isLoadingAvailable;
        set
        {
            _isLoadingAvailable = value;
            OnPropertyChanged();
        }
    }

    public bool IsLoadingAssigned
    {
        get => _isLoadingAssigned;
        set
        {
            _isLoadingAssigned = value;
            OnPropertyChanged();
        }
    }

    public bool IsAllAvailableLoaded
    {
        get => _isAllAvailableLoaded;
        set
        {
            _isAllAvailableLoaded = value;
            OnPropertyChanged();
        }
    }

    public bool IsAllAssignedLoaded
    {
        get => _isAllAssignedLoaded;
        set
        {
            _isAllAssignedLoaded = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<SelectableCompanyViewModel> AllCompaniesResponse
    {
        get => _allCompaniesResponse;
        private set
        {
            _allCompaniesResponse = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(AvailableCount));
        }
    }

    public ObservableCollection<SelectableCompanyViewModel> AssignedCompaniesResponse
    {
        get => _assignedCompaniesResponse;
        private set
        {
            _assignedCompaniesResponse = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(AssignedCount));
        }
    }

    public ObservableCollection<SelectableCompanyViewModel> FilteredAvailable
    {
        get => _filteredAvailable;
        private set
        {
            _filteredAvailable = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsAvailableEmpty));
            OnPropertyChanged(nameof(AvailableCount));
        }
    }

    public ObservableCollection<SelectableCompanyViewModel> FilteredAssigned
    {
        get => _filteredAssigned;
        private set
        {
            _filteredAssigned = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsAssignedEmpty));
            OnPropertyChanged(nameof(AssignedCount));
        }
    }

    public string AvailableSearch
    {
        get => _availableSearch;
        set
        {
            _availableSearch = value;
            OnPropertyChanged();
            ApplyAvailableFilter();
        }
    }

    public string AssignedSearch
    {
        get => _assignedSearch;
        set
        {
            _assignedSearch = value;
            OnPropertyChanged();
            ApplyAssignedFilter();
        }
    }

    public int AvailableCount => FilteredAvailable?.Count ?? 0;
    public int AssignedCount => FilteredAssigned?.Count ?? 0;

    public bool IsAvailableEmpty => FilteredAvailable == null || FilteredAvailable.Count == 0;
    public bool IsAssignedEmpty => FilteredAssigned == null || FilteredAssigned.Count == 0;

    public bool HasAvailableSelection => AllCompaniesResponse.Any(x => x.IsSelected);
    public bool HasAssignedSelection => AssignedCompaniesResponse.Any(x => x.IsSelected);

    public string AvailableSelectionText
        => $"عدد المحدد: {AllCompaniesResponse.Count(x => x.IsSelected)}";

    public string AssignedSelectionText
        => $"عدد المحدد: {AssignedCompaniesResponse.Count(x => x.IsSelected)}";

    public RequestFilters AssignCompaniesFilters { get; set; } = new();
    public RequestFilters AllCompaniesFilters { get; set; } = new();

    #endregion

    #region Commands

    public ICommand AddSelectedCommand { get; }
    public ICommand RemoveSelectedCommand { get; }
    public ICommand AddAllCommand { get; }
    public ICommand RemoveAllCommand { get; }
    public ICommand LoadMoreAvailableCommand { get; }
    public ICommand LoadMoreAssignedCommand { get; }

    #endregion

    public async Task InitializeAsync(int userId)
    {
        _userId = userId;
        await RefreshAsync();
    }

    private async Task RefreshAsync()
    {
        IsBusy = true;
        try
        {
            // Reset pagination state
            IsAllAvailableLoaded = false;
            IsAllAssignedLoaded = false;
            AllCompaniesFilters = new RequestFilters { PageNumber = 1, PageSize = 10 };
            AssignCompaniesFilters = new RequestFilters { PageNumber = 1, PageSize = 10 };

            await LoadAssignedCompaniesAsync();
            await LoadAllCompaniesAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task LoadAssignedCompaniesAsync()
    {
        IsLoadingAssigned = true;
        try
        {
            var response = await _userService.GetUserCompaniesAsync(_userId, AssignCompaniesFilters);

            var companies = response?.Items ?? Enumerable.Empty<CompanyResponse>();

            AssignedCompaniesResponse = new ObservableCollection<SelectableCompanyViewModel>(
                companies.Select(MapToSelectable));

            SubscribeToSelectionChanges(AssignedCompaniesResponse);
            ApplyAssignedFilter();

            // Check if all data loaded
            if (companies.Count() < AssignCompaniesFilters.PageSize)
            {
                IsAllAssignedLoaded = true;
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ");
        }
        finally
        {
            IsLoadingAssigned = false;
        }
    }

    private async Task LoadAllCompaniesAsync()
    {
        IsLoadingAvailable = true;
        try
        {
            var response = await _companyService.GetAllAsync(AllCompaniesFilters, false);

            var allCompanies = response?.Items ?? Enumerable.Empty<CompanyResponse>();
            var assignedIds = AssignedCompaniesResponse.Select(x => x.Id).ToHashSet();

            var available = allCompanies
                .Where(x => !assignedIds.Contains(x.Id))
                .Select(MapToSelectable)
                .ToList();

            AllCompaniesResponse = new ObservableCollection<SelectableCompanyViewModel>(available);

            SubscribeToSelectionChanges(AllCompaniesResponse);
            ApplyAvailableFilter();

            // Check if all data loaded
            if (available.Count < AllCompaniesFilters.PageSize)
            {
                IsAllAvailableLoaded = true;
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ");
        }
        finally
        {
            IsLoadingAvailable = false;
        }
    }

    /// <summary>
    /// Load more available companies (pagination)
    /// </summary>
    private async Task LoadMoreAvailableAsync()
    {
        // Don't load more if already loading or all loaded
        if (IsLoadingAvailable || IsAllAvailableLoaded)
            return;

        IsLoadingAvailable = true;
        try
        {
            // Increment page number
            AllCompaniesFilters = AllCompaniesFilters with { PageNumber = AllCompaniesFilters.PageNumber + 1 };

            var response = await _companyService.GetAllAsync(AllCompaniesFilters, false);
            var newCompanies = response?.Items ?? Enumerable.Empty<CompanyResponse>();
            var assignedIds = AssignedCompaniesResponse.Select(x => x.Id).ToHashSet();

            var availableNew = newCompanies
                .Where(x => !assignedIds.Contains(x.Id))
                .Select(MapToSelectable)
                .ToList();

            // Append to existing collection
            foreach (var company in availableNew)
            {
                AllCompaniesResponse.Add(company);
                SubscribeToSelectionChanges(new[] { company });
            }

            ApplyAvailableFilter();

            // Check if all data loaded
            if (availableNew.Count < AllCompaniesFilters.PageSize)
            {
                IsAllAvailableLoaded = true;
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ");
            // Decrement page number on error
            AllCompaniesFilters = AllCompaniesFilters with { PageNumber = AllCompaniesFilters.PageNumber - 1 };
        }
        finally
        {
            IsLoadingAvailable = false;
        }
    }

    /// <summary>
    /// Load more assigned companies (pagination)
    /// </summary>
    private async Task LoadMoreAssignedAsync()
    {
        // Don't load more if already loading or all loaded
        if (IsLoadingAssigned || IsAllAssignedLoaded)
            return;

        IsLoadingAssigned = true;
        try
        {
            // Increment page number
            AssignCompaniesFilters = AssignCompaniesFilters with { PageNumber = AssignCompaniesFilters.PageNumber + 1 };

            var response = await _userService.GetUserCompaniesAsync(_userId, AssignCompaniesFilters);
            var newCompanies = response?.Items ?? Enumerable.Empty<CompanyResponse>();

            var newSelectableCompanies = newCompanies.Select(MapToSelectable).ToList();

            // Append to existing collection
            foreach (var company in newSelectableCompanies)
            {
                AssignedCompaniesResponse.Add(company);
                SubscribeToSelectionChanges(new[] { company });
            }

            ApplyAssignedFilter();

            // Check if all data loaded
            if (newSelectableCompanies.Count < AssignCompaniesFilters.PageSize)
            {
                IsAllAssignedLoaded = true;
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ");
            // Decrement page number on error
            AssignCompaniesFilters = AssignCompaniesFilters with { PageNumber = AssignCompaniesFilters.PageNumber - 1 };
        }
        finally
        {
            IsLoadingAssigned = false;
        }
    }

    private SelectableCompanyViewModel MapToSelectable(CompanyResponse company)
    {
        return new SelectableCompanyViewModel
        {
            Id = company.Id,
            Name = company.Name ?? string.Empty,
            TaxRegistrationNumber = company.TaxRegistrationNumber ?? string.Empty,
            IsSelected = false
        };
    }

    private void SubscribeToSelectionChanges(IEnumerable<SelectableCompanyViewModel> companies)
    {
        foreach (var company in companies)
        {
            company.PropertyChanged -= Company_PropertyChanged;
            company.PropertyChanged += Company_PropertyChanged;
        }
    }

    private void Company_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SelectableCompanyViewModel.IsSelected))
        {
            OnPropertyChanged(nameof(HasAvailableSelection));
            OnPropertyChanged(nameof(HasAssignedSelection));
            OnPropertyChanged(nameof(AvailableSelectionText));
            OnPropertyChanged(nameof(AssignedSelectionText));
        }
    }

    private void ApplyAvailableFilter()
    {
        IEnumerable<SelectableCompanyViewModel> query = AllCompaniesResponse;

        if (!string.IsNullOrWhiteSpace(AvailableSearch))
        {
            var search = AvailableSearch.Trim();

            query = query.Where(x =>
                (!string.IsNullOrWhiteSpace(x.Name) &&
                 x.Name.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrWhiteSpace(x.TaxRegistrationNumber) &&
                 x.TaxRegistrationNumber.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }

        FilteredAvailable = new ObservableCollection<SelectableCompanyViewModel>(query);

        OnPropertyChanged(nameof(HasAvailableSelection));
        OnPropertyChanged(nameof(AvailableSelectionText));
    }

    private void ApplyAssignedFilter()
    {
        IEnumerable<SelectableCompanyViewModel> query = AssignedCompaniesResponse;

        if (!string.IsNullOrWhiteSpace(AssignedSearch))
        {
            var search = AssignedSearch.Trim();

            query = query.Where(x =>
                (!string.IsNullOrWhiteSpace(x.Name) &&
                 x.Name.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrWhiteSpace(x.TaxRegistrationNumber) &&
                 x.TaxRegistrationNumber.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }

        FilteredAssigned = new ObservableCollection<SelectableCompanyViewModel>(query);

        OnPropertyChanged(nameof(HasAssignedSelection));
        OnPropertyChanged(nameof(AssignedSelectionText));
    }

    private async Task AddSelectedAsync()
    {
        var selectedCompanies = AllCompaniesResponse
            .Where(x => x.IsSelected)
            .ToList();

        if (!selectedCompanies.Any())
            return;

        IsBusy = true;
        try
        {
            foreach (var company in selectedCompanies)
            {
                await _userService.AssignCompanyToUserAsync(_userId, company.Id);
            }

            await RefreshAsync();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task RemoveSelectedAsync()
    {
        var selectedCompanies = AssignedCompaniesResponse
            .Where(x => x.IsSelected)
            .ToList();

        if (!selectedCompanies.Any())
            return;

        IsBusy = true;
        try
        {
            foreach (var company in selectedCompanies)
            {
                await _userService.RemoveCompanyFromUserAsync(_userId, company.Id);
            }

            await RefreshAsync();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task AddAllAsync()
    {
        IsBusy = true;
        try
        {
            await _userService.AssignAllCompaniesToUserAsync(_userId);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task RemoveAllAsync()
    {
        IsBusy = true;
        try
        {
            await _userService.RemoveAllCompaniesFromUserAsync(_userId);
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
