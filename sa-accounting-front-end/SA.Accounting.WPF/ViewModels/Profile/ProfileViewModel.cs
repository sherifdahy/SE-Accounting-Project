using Newtonsoft.Json;
using Refit;
using SA.Accounting.Core.Contracts.Account.Requests;
using SA.Accounting.Core.Contracts.Profile.Requests;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.Commands.Base;
using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using System.Windows.Input;

namespace SA.Accounting.WPF.ViewModels;

public class ProfileViewModel : ViewModelBase, IAsyncInitializable
{
    private readonly IAccountService _accountService;
    private readonly IDialogService _dialogService;

    public override ViewType Section => ViewType.Profile;

    // ══════ Tab Selection ══════
    private bool _isPersonalInfoSelected = true;
    public bool IsPersonalInfoSelected
    {
        get => _isPersonalInfoSelected;
        set
        {
            _isPersonalInfoSelected = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsSecuritySelected));
        }
    }

    public bool IsSecuritySelected => !IsPersonalInfoSelected;

    // ══════ Profile Data ══════
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
        private set { _email = value; OnPropertyChanged(); }
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

    private string _initials = string.Empty;
    public string Initials
    {
        get => _initials;
        private set { _initials = value; OnPropertyChanged(); }
    }

    // ══════ Change Password ══════
    private string _currentPassword = string.Empty;
    public string CurrentPassword
    {
        get => _currentPassword;
        set { _currentPassword = value; OnPropertyChanged(); }
    }

    private string _newPassword = string.Empty;
    public string NewPassword
    {
        get => _newPassword;
        set { _newPassword = value; OnPropertyChanged(); }
    }

    private string _confirmPassword = string.Empty;
    public string ConfirmPassword
    {
        get => _confirmPassword;
        set { _confirmPassword = value; OnPropertyChanged(); }
    }

    // ══════ Commands ══════
    public ICommand ShowPersonalInfoCommand { get; }
    public ICommand ShowSecurityCommand { get; }
    public ICommand SaveProfileCommand { get; }
    public ICommand ChangePasswordCommand { get; }

    public ProfileViewModel(
        IAccountService accountService,
        IDialogService dialogService)
    {
        _accountService = accountService;
        _dialogService = dialogService;

        ShowPersonalInfoCommand = new RelayCommand((_) => IsPersonalInfoSelected = true);
        ShowSecurityCommand = new RelayCommand((_) => IsPersonalInfoSelected = false);
        SaveProfileCommand = new AsyncRelayCommand(async (_) => await SaveProfileAsync());
        ChangePasswordCommand = new AsyncRelayCommand(async (_) => await ChangePasswordAsync());
    }

    public async Task InitializeAsync() => await LoadProfileAsync();

    // ══════ Load ══════
    private async Task LoadProfileAsync()
    {
        try
        {
            var profile = await _accountService.GetProfileAsync();

            Name = profile.Name;
            Email = profile.Email;
            SSN = profile.SSN;
            PhoneNumber = profile.PhoneNumber;
            Initials = GetInitials(profile.Name);
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ في تحميل البيانات");
        }
    }

    // ══════ Save Profile ══════
    private async Task SaveProfileAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await _dialogService.ShowWarningAsync("يرجى إدخال الاسم", "تنبيه");
                return;
            }

            var request = new UpdateProfileRequest
            {
                Name = Name,
                PhoneNumber = PhoneNumber,
                SSN = SSN
            };

            await _accountService.UpdateProfileAsync(request);
            Initials = GetInitials(Name);

            await _dialogService.ShowInfoAsync("تم تحديث البيانات بنجاح ✓", "نجح الحفظ");
        }
        catch (ApiException apiEx)
        {
            var errors = JsonConvert.DeserializeObject<ProblemDetails>(apiEx.Content!);
            await _dialogService.ShowErrorAsync(errors!.Errors.First().Value.First()!, "خطأ");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ أثناء الحفظ");
        }
    }

    // ══════ Change Password ══════
    private async Task ChangePasswordAsync()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(CurrentPassword)
                || string.IsNullOrWhiteSpace(NewPassword)
                || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                await _dialogService.ShowWarningAsync("يرجى ملء جميع الحقول", "تنبيه");
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                await _dialogService.ShowWarningAsync(
                    "كلمة المرور الجديدة وتأكيدها غير متطابقتين", "تنبيه");
                return;
            }

            var request = new ChangePasswordRequest
            {
                CurrentPassword = CurrentPassword,
                NewPassword = NewPassword
            };

            await _accountService.ChangePasswordAsync(request);

            CurrentPassword = string.Empty;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;

            await _dialogService.ShowInfoAsync("تم تغيير كلمة المرور بنجاح ✓", "نجح التغيير");
        }
        catch (ApiException apiEx)
        {
            var errors = JsonConvert.DeserializeObject<ProblemDetails>(apiEx.Content!);
            await _dialogService.ShowErrorAsync(errors!.Errors.First().Value.First()!, "خطأ");
        }
        catch (Exception ex)
        {
            await _dialogService.ShowErrorAsync(ex.Message, "خطأ أثناء تغيير كلمة المرور");
        }
    }

    // ══════ Helpers ══════
    private static string GetInitials(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "؟";
        var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2
            ? $"{parts[0][0]}{parts[1][0]}"
            : $"{parts[0][0]}";
    }
}