using Newtonsoft.Json;
using Refit;
using SA.Accounting.Core.Contracts.User.Requests;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using SA.Accounting.WPF.ViewModels.Permissions;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public class UserPermissionsViewModel : ViewModelBase, IAsyncInitializable<int>
{
    private int _userId;
    private readonly IUserService _userService;
    private readonly IDialogService _dialogService;
    private readonly IPermissionService _permissionService;
    private readonly INavigator _navigator;
    private readonly IViewModelAbstractFactory _viewModelAbstractFactory;

    public override ViewType Section => ViewType.Users;

    // ══════ Original Data (for dirty check) ══════
    private HashSet<string> _originalDenied = [];

    // ══════ Stats ══════
    private int _defaultCount;
    public int DefaultCount
    {
        get => _defaultCount;
        set { _defaultCount = value; OnPropertyChanged(); }
    }

    private int _overrideCount;
    public int OverrideCount
    {
        get => _overrideCount;
        set { _overrideCount = value; OnPropertyChanged(); }
    }

    private int _unassignedCount;
    public int UnassignedCount
    {
        get => _unassignedCount;
        set { _unassignedCount = value; OnPropertyChanged(); }
    }

    // ══════ Search ══════
    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            ApplyFilter();
        }
    }

    // ══════ Dirty State ══════
    private bool _hasUnsavedChanges;
    public bool HasUnsavedChanges
    {
        get => _hasUnsavedChanges;
        set { _hasUnsavedChanges = value; OnPropertyChanged(); }
    }

    private bool _isSaving;
    public bool IsSaving
    {
        get => _isSaving;
        set { _isSaving = value; OnPropertyChanged(); }
    }

    // ══════ Collections ══════
    public ObservableCollection<PermissionGroupViewModel> Groups { get; } = [];
    public ObservableCollection<PermissionGroupViewModel> FilteredGroups { get; } = [];

    // ══════ Commands ══════
    public ICommand SelectAllCommand { get; }
    public ICommand DeselectAllCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public UserPermissionsViewModel(
        IUserService userService,
        IDialogService dialogService,
        IPermissionService permissionService,
        INavigator navigator,
        IViewModelAbstractFactory viewModelAbstractFactory)
    {
        _userService = userService;
        _dialogService = dialogService;
        _permissionService = permissionService;
        _navigator = navigator;
        _viewModelAbstractFactory = viewModelAbstractFactory;

        SelectAllCommand = new RelayCommand((_) => SetAll(true));
        DeselectAllCommand = new RelayCommand((_) => SetAll(false));
        SaveCommand = new AsyncRelayCommand(async (_) => await SaveAsync());
        CancelCommand = new AsyncRelayCommand(async (_) => await CancelAsync());
    }

    // ══════ Initialize ══════
    public async Task InitializeAsync(int userId)
    {
        _userId = userId;
        await LoadDataAsync();
    }

    // ══════ Load ══════
    private async Task LoadDataAsync()
    {
        try
        {
            var systemPermissions = await _permissionService.GetAllAsync();
            var userPermissions = await _userService.GetUserPermissionsAsync(_userId);

            var defaultSet = userPermissions.Default.ToHashSet();
            var deniedSet = userPermissions.Denied.ToHashSet();

            _originalDenied = new HashSet<string>(deniedSet);

            Groups.Clear();

            // Group permissions by module (e.g., "Users.Create" → group "Users")
            var grouped = systemPermissions
                .GroupBy(p => p.Contains(':') ? p.Substring(0, p.IndexOf(':')) : p)
                .OrderBy(g => g.Key);

            foreach (var group in grouped)
            {
                var groupVm = new PermissionGroupViewModel
                {
                    GroupName = group.Key,
                    IsExpanded = false
                };

                foreach (var permission in group.OrderBy(p => p))
                {
                    var displayName = permission.Contains(':')
                        ? permission.Substring(permission.IndexOf(':') + 1)
                        : permission;

                    var isInRole = defaultSet.Contains(permission);
                    var isDenied = deniedSet.Contains(permission);

                    var item = new PermissionItemViewModel(() =>
                    {
                        groupVm.Refresh();
                        UpdateStats();
                        CheckDirty();
                    })
                    {
                        Permission = permission,
                        DisplayName = displayName,
                        FullKey = permission,
                        IsDefault = isInRole,
                        IsSelected = isInRole && !isDenied
                    };

                    groupVm.Permissions.Add(item);
                }

                groupVm.Refresh();
                Groups.Add(groupVm);
            }

            UpdateStats();
            ApplyFilter();
            HasUnsavedChanges = false;
        }
        catch (ApiException ex)
        {
            var errors = JsonConvert.DeserializeObject<ProblemDetails>(ex.Content!);
            await _dialogService.ShowErrorAsync(errors!.Errors.First().Value.First(), "خطأ");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ في تحميل البيانات");
        }
    }

    // ══════ Stats ══════
    private void UpdateStats()
    {
        var allPermissions = Groups.SelectMany(g => g.Permissions).ToList();

        DefaultCount = allPermissions.Count(p => p.IsDefault);
        OverrideCount = allPermissions.Count(p => p.IsSelected);
        UnassignedCount = DefaultCount - OverrideCount;

        if (UnassignedCount < 0) UnassignedCount = 0;
    }

    // ══════ Dirty Check ══════
    private void CheckDirty()
    {
        var currentDenied = GetCurrentDenied();
        HasUnsavedChanges = !_originalDenied.SetEquals(currentDenied);
    }

    private HashSet<string> GetCurrentDenied()
    {
        return Groups
            .SelectMany(g => g.Permissions)
            .Where(p => p.IsDefault && !p.IsSelected)
            .Select(p => p.Permission)
            .ToHashSet();
    }

    // ══════ Select/Deselect All ══════
    private void SetAll(bool selected)
    {
        foreach (var group in Groups)
        {
            foreach (var permission in group.Permissions)
            {
                if (permission.IsDefault)
                    permission.IsSelected = selected;
            }
            group.Refresh();
        }
        UpdateStats();
        CheckDirty();
    }

    // ══════ Filter ══════
    private void ApplyFilter()
    {
        FilteredGroups.Clear();

        var search = SearchText?.Trim().ToLower() ?? string.Empty;

        foreach (var group in Groups)
        {
            if (string.IsNullOrEmpty(search))
            {
                FilteredGroups.Add(group);
                continue;
            }

            if (group.GroupName.ToLower().Contains(search) ||
                group.Permissions.Any(p => p.Permission.ToLower().Contains(search) ||
                                           p.DisplayName.ToLower().Contains(search)))
            {
                FilteredGroups.Add(group);
            }
        }
    }

    // ══════ Save ══════
    private async Task SaveAsync()
    {
        try
        {
            IsSaving = true;

            var denied = GetCurrentDenied().ToList();

            var request = new UpdateUserPermissionOverridesRequest
            {
                DeniedPermissions = denied
            };

            await _userService.UpdateUserPermissionsAsync(_userId, request);

            _originalDenied = denied.ToHashSet();
            HasUnsavedChanges = false;

            await _dialogService.ShowInfoAsync("تم حفظ الصلاحيات بنجاح ✓", "تم الحفظ");
        }
        catch (ApiException ex)
        {
            var errors = JsonConvert.DeserializeObject<ProblemDetails>(ex.Content!);
            await _dialogService.ShowErrorAsync(errors!.Errors.First().Value.First(), "خطأ أثناء الحفظ");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ أثناء الحفظ");
        }
        finally
        {
            IsSaving = false;
        }
    }

    // ══════ Cancel ══════
    private async Task CancelAsync()
    {
        if (HasUnsavedChanges)
        {
            if (!await _dialogService.ShowConfirmAsync(
                "هل تريد إلغاء التعديلات؟ سيتم فقدان جميع التغييرات.", "تأكيد الإلغاء"))
                return;
        }

        var vm = _viewModelAbstractFactory.CreateViewModel(ViewType.Users);
        if (vm is IAsyncInitializable init) await init.InitializeAsync();
        _navigator.CurrentViewModel = vm;
    }
}