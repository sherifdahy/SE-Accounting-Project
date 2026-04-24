using SA.Accounting.Core.Contracts.Owner.Requests;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.ViewModels.Base;

namespace SA.Accounting.WPF.ViewModels.Owner;

public class CreateOwnerViewModel : ValidatableViewModel<CreateOwnerViewModel>
{
    private readonly IValidator<CreateOwnerViewModel> _validator;
    protected override IValidator<CreateOwnerViewModel> Validator => _validator;

    public CreateOwnerViewModel(IValidator<CreateOwnerViewModel> validator)
    {
        _validator = validator;
    }

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set { _name = value; OnPropertyChanged(); ValidateProperty(); }
    }

    private string _ssn = string.Empty;
    public string SSN
    {
        get => _ssn;
        set { _ssn = value; OnPropertyChanged(); ValidateProperty(); }
    }
}