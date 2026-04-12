using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mapster;                                         // ← أضف هذا
using SA.Accounting.Core.Contracts.Auth.Requests;
using SA.Accounting.Core.Contracts.Auth.Validators;
using SA.Accounting.Core;
using SA.Accounting.Infrastructure.Handlers;
using SA.Accounting.Core.Interfaces;

namespace SA.Accounting.WPF.ViewModels.Auth;

public sealed partial class GetTokenViewModel
    : ValidatableModel<GetTokenRequest>
{
    // ─── Services ────────────────────────────────
    private readonly IAuthService _authService;

    // ─── Properties ──────────────────────────────
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private bool _isBusy;

    // ─── Callback ────────────────────────────────
    public Action? OnLoginSuccess { get; set; }

    // ─── Constructor ─────────────────────────────
    public GetTokenViewModel(IAuthService authService)
        : base(new GetTokenValidator())
    {
        _authService = authService;
    }

    // ─── Validation hooks ────────────────────────
    partial void OnEmailChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(Email));

    partial void OnPasswordChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(Password));

    // ─── Login ───────────────────────────────────
    [RelayCommand]
    private async Task LoginAsync()
    {
        ValidateAll(ToRequest());
        if (!IsValid) return;

        IsBusy = true;
        try
        {
            var response = await _authService.GetTokenAsync(ToRequest());

            if (response is not null)
                OnLoginSuccess?.Invoke();
        }
        catch (Exception ex) { }
        finally { IsBusy = false; }
    }

    private GetTokenRequest ToRequest() => this.Adapt<GetTokenRequest>();  
}