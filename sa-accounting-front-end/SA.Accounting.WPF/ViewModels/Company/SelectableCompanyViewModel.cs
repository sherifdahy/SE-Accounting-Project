using SA.Accounting.WPF.Models;
using SA.Accounting.WPF.ViewModels.Base;

namespace SA.Accounting.WPF.ViewModels.Company;

public class SelectableCompanyViewModel : ViewModelBase
{
    private int _id;
    private string _name = string.Empty;
    private string _taxRegistrationNumber = string.Empty;
    private bool _isSelected;

    public int Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged();
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged();
            }
        }
    }

    public string TaxRegistrationNumber
    {
        get => _taxRegistrationNumber;
        set
        {
            if (_taxRegistrationNumber != value)
            {
                _taxRegistrationNumber = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }
}