using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace ColorDesktop.CoreLib.View.Cov;

public class EnumToBooleanConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Equals(value, parameter);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isChecked && isChecked)
        {
            return parameter;
        }

        return BindingOperations.DoNothing;
    }
}
