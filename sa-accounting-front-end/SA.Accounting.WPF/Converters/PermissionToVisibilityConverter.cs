using SA.Accounting.WPF.Interfaces;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SA.Accounting.WPF.Converters;

public class PermissionToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is List<string> permissions && parameter.ToString() != null)
        {
            bool hasPermission = permissions.Contains(parameter);

            return hasPermission ? Visibility.Visible : Visibility.Collapsed;
        }

        return Visibility.Hidden;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
