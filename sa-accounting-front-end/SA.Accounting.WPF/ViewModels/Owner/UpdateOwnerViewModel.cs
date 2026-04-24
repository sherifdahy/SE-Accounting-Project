using SA.Accounting.Core.Contracts.Owner.Requests;
using SA.Accounting.Core.WPF;
using SA.Accounting.WPF.ViewModels.Base;

namespace SA.Accounting.WPF.ViewModels.Owner;

public class UpdateOwnerViewModel : ValidatableViewModel<UpdateOwnerViewModel>
{
    private readonly IValidator<UpdateOwnerViewModel> _validator;
    protected override IValidator<UpdateOwnerViewModel> Validator => _validator;

    public UpdateOwnerViewModel(IValidator<UpdateOwnerViewModel> validator)
    {
        _validator = validator;
    }

    // ══════ Properties ══════
    private int _id;
    public int Id
    {
        get => _id;
        set { _id = value; OnPropertyChanged(); }
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