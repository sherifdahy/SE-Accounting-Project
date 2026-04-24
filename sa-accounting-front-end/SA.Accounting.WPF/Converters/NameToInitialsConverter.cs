using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace SA.Accounting.WPF.Converters;

public class NameToInitialsConverter : IValueConverter
{
    public object Convert(object value, Type targetType,object parameter, CultureInfo culture)
    {
        if (value is string name && !string.IsNullOrEmpty(name))
        {
            var parts = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
                return $"{parts[0][0]}{parts[1][0]}".ToUpper();
            return name[0].ToString().ToUpper();
        }
        return "??";
    }

    public object ConvertBack(object value, Type targetType,object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
