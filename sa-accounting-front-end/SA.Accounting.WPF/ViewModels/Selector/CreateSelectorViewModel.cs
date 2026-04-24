using SA.Accounting.Core.Enums;
using SA.Accounting.WPF.ViewModels.Base;

namespace SA.Accounting.WPF.ViewModels.Selector;

public class CreateSelectorViewModel : ViewModelBase
{
    private string _value = string.Empty;
    public string Value
    {
        get => _value;
        set { _value = value; OnPropertyChanged(); }
    }

    private SelectorContentType _contentType;
    public SelectorContentType ContentType
    {
        get => _contentType;
        set { _contentType = value; OnPropertyChanged(); }
    }

    private SelectorType _type;
    public SelectorType Type
    {
        get => _type;
        set { _type = value; OnPropertyChanged(); }
    }

    private int _priority;
    public int Priority
    {
        get => _priority;
        set { _priority = value; OnPropertyChanged(); }
    }

    private int _displayOrder;
    public int DisplayOrder
    {
        get => _displayOrder;
        set { _displayOrder = value; OnPropertyChanged(); }
    }
}