using CommunityToolkit.Mvvm.ComponentModel;
using Mapster;
using SA.Accounting.Core.Contracts.Account.Requests;
using SA.Accounting.Core.Contracts.Account.Validators;
using SA.Accounting.Core;

namespace SA.Accounting.WPF.ViewModels.Account;

public sealed partial class UpdateAccountViewModel
    : ValidatableModel<UpdateAccountRequest>
{
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private int _platformId;

    public UpdateAccountViewModel()
        : base(new UpdateAccountRequestValidator()) { }

    partial void OnEmailChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(Email));

    partial void OnPasswordChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(Password));

    partial void OnPlatformIdChanged(int value)
        => RunPropertyValidation(ToRequest(), nameof(PlatformId));

    public UpdateAccountRequest ToRequest()
        => this.Adapt<UpdateAccountRequest>();
}