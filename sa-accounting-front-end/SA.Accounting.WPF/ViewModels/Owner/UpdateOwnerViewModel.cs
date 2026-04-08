using CommunityToolkit.Mvvm.ComponentModel;
using SA.Accounting.Core.Contracts.Owner.Requests;
using SA.Accounting.Core.Contracts.Owner.Validators;
using SA.Accounting.Core;
using System.Xml.Linq;

namespace SA.Accounting.WPF.ViewModels.Owner;

public sealed partial class UpdateOwnerViewModel
    : ValidatableModel<UpdateOwnerRequest>
{
    // ─── Properties ──────────────────────────────
    [ObservableProperty] private int _id;
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _ssn = string.Empty;

    // ─── Constructor ─────────────────────────────
    public UpdateOwnerViewModel()
        : base(new UpdateOwnerRequestValidator()) { }

    // ─── Validation hooks ────────────────────────
    partial void OnNameChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(Name));

    partial void OnSsnChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(Ssn));

    // ─── Map to Request ──────────────────────────
    public UpdateOwnerRequest ToRequest() => new()
    {
        Id = Id,
        Name = Name,
        SSN = Ssn,
    };
}