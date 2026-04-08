using CommunityToolkit.Mvvm.ComponentModel;
using Mapster; // <--- 1. إضافة using
using SA.Accounting.WPF.Contracts.Account.Requests;
using SA.Accounting.WPF.Contracts.Account.Validators;
using SA.Accounting.WPF.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Telerik.Windows.Controls.DataVisualization.Map.BingRest;

namespace SA.Accounting.WPF.ViewModels.Account;

public sealed partial class CreateAccountViewModel
    : ValidatableModel<CreateAccountRequest>
{
    // ─── Properties ──────────────────────────────
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private int _platformId;

    // ─── Constructor ─────────────────────────────
    public CreateAccountViewModel()
        : base(new CreateAccountRequestValidator()) { }

    // ─── Validation hooks ────────────────────────
    partial void OnEmailChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(Email));

    partial void OnPasswordChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(Password));

    partial void OnPlatformIdChanged(int value)
        => RunPropertyValidation(ToRequest(), nameof(PlatformId));

    // ─── Map to Request ──────────────────────────
    // 2. تم استبدال التحويل اليدوي بـ Mapster
    public CreateAccountRequest ToRequest() => this.Adapt<CreateAccountRequest>();
}