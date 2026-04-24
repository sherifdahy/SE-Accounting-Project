using SA.Accounting.WPF.Interfaces;
using SA.Accounting.WPF.ViewModels.Base;
using System.Globalization;
using System.Windows.Data;

namespace SA.Accounting.WPF.Converters;

public class ViewModelToSelectedConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not ViewModelBase vm)
            return false;

        if (parameter is not ViewType target)
            return false;

        return vm.Section == target ? "Selected" : false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}