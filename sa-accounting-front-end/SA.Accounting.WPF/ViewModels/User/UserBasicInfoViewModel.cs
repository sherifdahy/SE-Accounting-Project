using Mapster;
using SA.Accounting.Core.Contracts.Role.Responses;
using SA.Accounting.Core.Contracts.User.Requests;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public sealed class UserBasicInfoViewModel : ViewModelBase, IAsyncInitializable<int>
{
    private readonly IUserService _userService;
    private readonly IDialogService _dialogService;
    private readonly IRoleService _roleService;

    public override ViewType Section => ViewType.Users;

    // ══════ Properties ══════
    private int _userId;

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); }
    }

    private string _email = string.Empty;
    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(); }
    }

    private string _password = string.Empty;
    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    private string _ssn = string.Empty;
    public string SSN
    {
        get => _ssn;
        set { _ssn = value; OnPropertyChanged(); }
    }

    private string _phoneNumber = string.Empty;
    public string PhoneNumber
    {
        get => _phoneNumber;
        set { _phoneNumber = value; OnPropertyChanged(); }
    }

    private RoleResponse? _selectedRole;
    public RoleResponse? SelectedRole
    {
        get => _selectedRole;
        set { _selectedRole = value; OnPropertyChanged(); }
    }

    // ══════ Collections ══════
    public ObservableCollection<RoleResponse> Roles { get; } = [];

    // ══════ States ══════
    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set { _isLoading = value; OnPropertyChanged(); }
    }

    // ══════ Commands ══════
    public ICommand UpdateCurrentUserCommand { get; }

    // ══════ Constructor ══════
    public UserBasicInfoViewModel(
        IUserService userService,
        IDialogService dialogService,
        IRoleService roleService)
    {
        _userService = userService;
        _dialogService = dialogService;
        _roleService = roleService;

        UpdateCurrentUserCommand = new AsyncRelayCommand(async _ => await UpdateAsync());
    }

    // ══════ Init ══════
    public async Task InitializeAsync(int userId)
    {
        _userId = userId;
        await LoadRolesAsync();
        await LoadUserAsync();
    }

    // ══════ Load Roles ══════
    private async Task LoadRolesAsync()
    {
        try
        {
            IsLoading = true;
            var result = await _roleService.GetAllAsync(false);

            Roles.Clear();
            foreach (var r in result ?? []) Roles.Add(r);
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ في تحميل الصلاحيات");
        }
        finally
        {
            IsLoading = false;
        }
    }

    // ══════ Load User ══════
    private async Task LoadUserAsync()
    {
        try
        {
            IsLoading = true;
            var response = await _userService.GetByIdAsync(_userId);
            response.Adapt(this);

            if (Roles.Any())
                SelectedRole = Roles.FirstOrDefault(r => r.Name == response.Role);
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ في تحميل بيانات المستخدم");
        }
        finally
        {
            IsLoading = false;
        }
    }

    // ══════ Update ══════
    private async Task UpdateAsync()
    {
        try
        {
            IsBusy = true;

            var request = this.Adapt<UpdateUserRequest>();
            request.Role = SelectedRole?.Name ?? string.Empty;

            await _userService.UpdateAsync(_userId, request);
            await _dialogService.ShowInfoAsync("تم تحديث بيانات المستخدم بنجاح ✓", "نجح الحفظ");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ أثناء تحديث المستخدم");
        }
        finally
        {
            IsBusy = false;
        }
    }
}