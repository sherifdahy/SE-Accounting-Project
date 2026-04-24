using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;

namespace SA.Accounting.WPF.ViewModels;

public class CreateTransactionItemViewModel : ValidatableViewModel<CreateTransactionItemViewModel>
{
    private readonly IValidator<CreateTransactionItemViewModel> _validator;
    protected override IValidator<CreateTransactionItemViewModel> Validator => _validator;

    public CreateTransactionItemViewModel(IValidator<CreateTransactionItemViewModel> validator)
    {
        _validator = validator;
    }

    // ══════ Properties ══════
    private string _note = string.Empty;
    public string Note
    {
        get => _note;
        set { _note = value; OnPropertyChanged(); ValidateProperty(); }
    }

    private string? _fileUrl;
    public string? FileUrl
    {
        get => _fileUrl;
        set { _fileUrl = value; OnPropertyChanged(); }
    }

    private decimal _amount;
    public decimal Amount
    {
        get => _amount;
        set { _amount = value; OnPropertyChanged(); ValidateProperty(); }
    }

    private int _transactionCategoryId;
    public int TransactionCategoryId
    {
        get => _transactionCategoryId;
        set { _transactionCategoryId = value; OnPropertyChanged(); ValidateProperty(); }
    }

    private int _companyId;
    public int CompanyId
    {
        get => _companyId;
        set { _companyId = value; OnPropertyChanged(); ValidateProperty(); }
    }
}