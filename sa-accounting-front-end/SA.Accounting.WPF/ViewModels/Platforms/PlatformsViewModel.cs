using SA.Accounting.Core.Contracts.Platform.Responses;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public sealed class PlatformsViewModel : ViewModelBase, IAsyncInitializable
{
    private readonly IPlatformService _platformService;
    private readonly IDialogService _dialogService;
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;
    private readonly INavigator _navigator;

    private const int PageSize = 10;

    public override ViewType Section => ViewType.Platforms;

    // ══════ Data ══════
    private List<PlatformResponse> _allPlatforms = [];
    private List<PlatformResponse> _filteredPlatforms = [];
    public ObservableCollection<PlatformResponse> Platforms { get; } = [];

    // ══════ Search ══════
    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            CurrentPage = 1;
            ApplyFilterAndPaginate();
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

    public int TotalPages => Math.Max(1, (int)Math.Ceiling(_filteredPlatforms.Count / (double)PageSize));
    public int TotalCount => _allPlatforms.Count;
    public bool HasNoPlatforms => Platforms.Count == 0;

    public string PaginationText =>
        $"صفحة {CurrentPage} من {TotalPages} — إجمالي {_filteredPlatforms.Count}";

    // ══════ Commands ══════
    public ICommand AddPlatformCommand { get; }
    public ICommand UpdatePlatformCommand { get; }
    public ICommand DeletePlatformCommand { get; }
    public ICommand OpenUrlCommand { get; }
    public ICommand NextPageCommand { get; }
    public ICommand PreviousPageCommand { get; }

    public PlatformsViewModel(
        IPlatformService platformService,
        IDialogService dialogService,
        IViewModelAbstractFactory viewModelAbstractFactory,
        INavigator navigator)
    {
        _platformService = platformService;
        _dialogService = dialogService;
        _viewModelAbstractFactory = viewModelAbstractFactory;
        _navigator = navigator;

        AddPlatformCommand = new RelayCommand((_) =>
        {
            _navigator.CurrentViewModel = _viewModelAbstractFactory.CreateViewModel(ViewType.CreatePlatform);
        });

        UpdatePlatformCommand = new AsyncRelayCommand(async (p) => await NavigateToUpdateAsync((PlatformResponse)p!));
        DeletePlatformCommand = new AsyncRelayCommand(async (p) => await DeletePlatformAsync((PlatformResponse)p!));

        OpenUrlCommand = new RelayCommand(
            execute: p =>
            {
                if (p is PlatformResponse pl && !string.IsNullOrWhiteSpace(pl.Url))
                    Process.Start(new ProcessStartInfo { FileName = pl.Url, UseShellExecute = true });
            },
            canExecute: p => p is PlatformResponse pl && !string.IsNullOrWhiteSpace(pl.Url));

        NextPageCommand = new RelayCommand((_) =>
        {
            if (CurrentPage < TotalPages) { CurrentPage++; Paginate(); }
        });

        PreviousPageCommand = new RelayCommand((_) =>
        {
            if (CurrentPage > 1) { CurrentPage--; Paginate(); }
        });
    }

    public async Task InitializeAsync() => await LoadAsync();

    // ══════ Load All From Server ══════
    private async Task LoadAsync()
    {
        try
        {
            _allPlatforms = await _platformService.GetAllAsync(false) ?? [];
            CurrentPage = 1;
            ApplyFilterAndPaginate();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ في تحميل المنصات");
        }
    }

    // ══════ Filter + Paginate ══════
    private void ApplyFilterAndPaginate()
    {
        // Filter
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            _filteredPlatforms = _allPlatforms;
        }
        else
        {
            var search = SearchText.Trim().ToLower();
            _filteredPlatforms = _allPlatforms
                .Where(x =>
                    (x.Name?.ToLower().Contains(search) == true) ||
                    (x.Url?.ToLower().Contains(search) == true))
                .ToList();
        }

        // Notify totals changed
        OnPropertyChanged(nameof(TotalCount));
        OnPropertyChanged(nameof(TotalPages));

        // Fix page if out of bounds
        if (CurrentPage > TotalPages) CurrentPage = TotalPages;

        Paginate();
    }

    private void Paginate()
    {
        Platforms.Clear();

        var page = _filteredPlatforms
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize);

        foreach (var p in page) Platforms.Add(p);

        OnPropertyChanged(nameof(HasNoPlatforms));
        OnPropertyChanged(nameof(PaginationText));
    }

    // ══════ Navigation ══════
    private async Task NavigateToUpdateAsync(PlatformResponse platform)
    {
        var vm = _viewModelAbstractFactory.CreateViewModel(ViewType.UpdatePlatform);
        if (vm is IAsyncInitializable<int> init) await init.InitializeAsync(platform.Id);
        _navigator.CurrentViewModel = vm;
    }

    // ══════ Delete ══════
    private async Task DeletePlatformAsync(PlatformResponse platform)
    {
        try
        {
            if (!await _dialogService.ShowConfirmAsync(
                $"هل أنت متأكد من حذف \"{platform.Name}\"؟", "تأكيد"))
                return;

            await _platformService.ToggleStatusAsync(platform.Id);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ");
        }
    }
}