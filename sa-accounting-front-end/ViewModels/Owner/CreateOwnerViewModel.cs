using CommunityToolkit.Mvvm.ComponentModel;
using SA.Accounting.WPF.Contracts.Owner.Requests;
using SA.Accounting.WPF.Contracts.Owner.Validators;
using SA.Accounting.WPF.Core;
using System.Xml.Linq;

namespace SA.Accounting.WPF.ViewModels.Owner;

public sealed partial class CreateOwnerViewModel
    : ValidatableModel<CreateOwnerRequest>
{
    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _ssn = string.Empty;

    public CreateOwnerViewModel()
        : base(new CreateOwnerRequestValidator()) { }

    partial void OnNameChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(Name));

    partial void OnSsnChanged(string value)
        => RunPropertyValidation(ToRequest(), nameof(Ssn));

    public CreateOwnerRequest ToRequest() => new()
    {
        Name = Name,
        SSN = Ssn,
    };
}