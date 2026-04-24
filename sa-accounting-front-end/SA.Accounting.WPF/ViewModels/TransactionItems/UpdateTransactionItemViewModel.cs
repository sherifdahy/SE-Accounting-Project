using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;

namespace SA.Accounting.WPF.ViewModels.TransactionItem;

public class UpdateTransactionItemViewModel : ValidatableViewModel<UpdateTransactionItemViewModel>
{
    private readonly IValidator<UpdateTransactionItemViewModel> _validator;
    protected override IValidator<UpdateTransactionItemViewModel> Validator => _validator;

    public UpdateTransactionItemViewModel(IValidator<UpdateTransactionItemViewModel> validator)
    {
        _validator = validator;
    }

    // ══════ Id (null = بند جديد) ══════
    private int? _id;
    public int? Id
    {
        get => _id;
        set { _id = value; OnPropertyChanged(); }
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