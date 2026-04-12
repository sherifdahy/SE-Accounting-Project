using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SA.Accounting.Core.Contracts.User.Requests;
using SA.Accounting.Core.Interfaces;
using SA.Accounting.Infrastructure.Handlers;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telerik.Windows.Controls.DataVisualization.Map.BingRest;
using Telerik.Windows.Documents.Spreadsheet.Expressions.Functions;

namespace SA.Accounting.WPF.ViewModels.User;

public sealed partial class UpdateUserViewModel : ObservableObject
{
    private readonly int _userId;
    private readonly IUserService _userService;
    private readonly IDialogService _dialogService;

    // ─── State ───────────────────────────────────
    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private bool _isLoading = true;

    // ─── Form Fields ─────────────────────────────
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _ssn = string.Empty;
    [ObservableProperty] private string _phoneNumber = string.Empty;
    [ObservableProperty] private string _role = string.Empty;

    // ─── Validation ──────────────────────────────
    [ObservableProperty] private string _nameError = string.Empty;
    [ObservableProperty] private string _emailError = string.Empty;
    [ObservableProperty] private string _passwordError = string.Empty;
    [ObservableProperty] private string _ssnError = string.Empty;
    [ObservableProperty] private string _phoneNumberError = string.Empty;
    [ObservableProperty] private string _roleError = string.Empty;

    // ─── Events ──────────────────────────────────
    public event Action? OnSaved;
    public event Action? OnCancelled;

    // ─── Constructor ─────────────────────────────
    public UpdateUserViewModel(
        int userId,
        IUserService userService,
        IDialogService dialogService)
    {
        _userId = userId;
        _userService = userService;
        _dialogService = dialogService;

        _ = LoadUserAsync();
    }

    // ─── Load Existing Data ──────────────────────
    private async Task LoadUserAsync()
    {
        IsLoading = true;
        try
        {
            var user = await _userService.GetUserByIdAsync(_userId);
            if (user is null) return;

            Name = user.Name;
            Email = user.Email;
            Ssn = user.SSN;
            PhoneNumber = user.PhoneNumber;
            Role = user.Role;
            Password = string.Empty; // لا نحمّل كلمة المرور
        }
        catch (Exception ex)
        {
        }
        finally { IsLoading = false; }
    }

    // ─── Validation ──────────────────────────────
    private bool Validate()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(Name))
        { NameError = "اسم المستخدم مطلوب"; isValid = false; }
        else NameError = string.Empty;

        if (string.IsNullOrWhiteSpace(Email))
        { EmailError = "البريد الإلكتروني مطلوب"; isValid = false; }
        else EmailError = string.Empty;

        // كلمة المرور اختيارية في التعديل
        if (!string.IsNullOrEmpty(Password) && Password.Length < 6)
        { PasswordError = "كلمة المرور يجب أن تكون 6 أحرف على الأقل"; isValid = false; }
        else PasswordError = string.Empty;

        if (string.IsNullOrWhiteSpace(Ssn))
        { SsnError = "الرقم القومي مطلوب"; isValid = false; }
        else SsnError = string.Empty;

        if (string.IsNullOrWhiteSpace(PhoneNumber))
        { PhoneNumberError = "رقم الهاتف مطلوب"; isValid = false; }
        else PhoneNumberError = string.Empty;

        if (string.IsNullOrWhiteSpace(Role))
        { RoleError = "الدور مطلوب"; isValid = false; }
        else RoleError = string.Empty;

        return isValid;
    }

    // ─── Commands ────────────────────────────────

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!Validate()) return;

        IsBusy = true;
        try
        {
            var request = new UpdateUserRequest
            {
                Name = Name.Trim(),
                Email = Email.Trim(),
                Password = string.IsNullOrWhiteSpace(Password) ? null : Password,
                SSN = Ssn.Trim(),
                PhoneNumber = PhoneNumber.Trim(),
                Role = Role.Trim()
            };

            await _userService.UpdateUserAsync(_userId, request);
            OnSaved?.Invoke();
        }
        catch (Exception ex)
        {
        }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    private void Cancel()
    {
        OnCancelled?.Invoke();
    }
}