using SA.Accounting.Core.Contracts.Role.Responses;
using SA.Accounting.Core.Contracts.User.Requests;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public sealed class CreateUserViewModel : ViewModelBase, ICloseable
{
    private readonly IUserService _userService;
    private readonly IDialogService _dialogService;
    private readonly IRoleService _roleService;

    // ═══ State ═══
    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set { _isBusy = value; OnPropertyChanged(); }
    }

    private bool _isLoadingRoles;
    public bool IsLoadingRoles
    {
        get => _isLoadingRoles;
        set { _isLoadingRoles = value; OnPropertyChanged(); }
    }

    // ═══ Form Fields ═══
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
    public string Ssn
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

    // ✅ تم التغيير من string إلى RoleResponse كامل
    private RoleResponse? _selectedRole;
    public RoleResponse? SelectedRole
    {
        get => _selectedRole;
        set { _selectedRole = value; OnPropertyChanged(); }
    }

    // ═══ Roles Collection ═══
    private ObservableCollection<RoleResponse> _roles = new();
    public ObservableCollection<RoleResponse> Roles
    {
        get => _roles;
        set { _roles = value; OnPropertyChanged(); }
    }

    // ═══ ICloseable ═══
    public event Action? CloseRequested;

    private bool _dialogResult;
    public bool DialogResult
    {
        get => _dialogResult;
        private set { _dialogResult = value; OnPropertyChanged(); }
    }

    // ═══ Commands ═══
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand LoadRolesCommand { get; }

    // ═══ Constructor ═══
    public CreateUserViewModel(
        IUserService userService,
        IDialogService dialogService,
        IRoleService roleService)
    {
        _userService = userService;
        _dialogService = dialogService;
        _roleService = roleService;

        SaveCommand = new AsyncRelayCommand(async (_) => await SaveAsync());
        CancelCommand = new AsyncRelayCommand(async (_) => await CancelAsync());
        LoadRolesCommand = new AsyncRelayCommand(async (_) => await LoadRolesAsync());
    }

    // ═══ Load Roles ═══
    private async Task LoadRolesAsync()
    {
        IsLoadingRoles = true;
        try
        {
            var roles = await _roleService.GetAllAsync();

            Roles.Clear();
            foreach (var role in roles)
            {
                Roles.Add(role);
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(
                $"حدث خطأ أثناء جلب الصلاحيات: {ex.Message}",
                "خطأ");
        }
        finally
        {
            IsLoadingRoles = false;
        }
    }

    // ═══ Save ═══
    private async Task SaveAsync()
    {
        IsBusy = true;
        try
        {
            var request = new CreateUserRequest
            {
                Name = Name.Trim(),
                Email = Email.Trim(),
                Password = Password,
                SSN = Ssn.Trim(),
                PhoneNumber = PhoneNumber.Trim(),
                // ✅ إرسال اسم الدور (أو Id حسب الـ Backend)
                Role = SelectedRole?.Name ?? string.Empty
            };

            await _userService.CreateAsync(request);

            await _dialogService.ShowInfoAsync(
                "تم إنشاء المستخدم بنجاح", "نجاح");

            DialogResult = true;
            CloseRequested?.Invoke();
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(
                $"حدث خطأ أثناء إنشاء المستخدم: {ex.Message}",
                "خطأ");
        }
        finally
        {
            IsBusy = false;
        }
    }

    // ═══ Cancel ═══
    private async Task CancelAsync()
    {
        if (HasUnsavedChanges())
        {
            bool confirm = await _dialogService.ShowConfirmAsync(
                "هل أنت متأكد من الإلغاء؟ سيتم فقدان البيانات.",
                "تأكيد الإلغاء");
            if (!confirm) return;
        }

        DialogResult = false;
        CloseRequested?.Invoke();
    }

    private bool HasUnsavedChanges()
    {
        return !string.IsNullOrWhiteSpace(Name)
            || !string.IsNullOrWhiteSpace(Email)
            || !string.IsNullOrWhiteSpace(Password)
            || !string.IsNullOrWhiteSpace(Ssn)
            || !string.IsNullOrWhiteSpace(PhoneNumber)
            || SelectedRole is not null;
    }
}