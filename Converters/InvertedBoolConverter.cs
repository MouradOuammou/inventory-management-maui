using System.Globalization;

namespace InventoryApp.Converters;

/// <summary>
/// Returns the logical negation of a boolean. Used to bind
/// Button.IsEnabled to "not IsBusy".
/// </summary>
public class InvertedBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b && !b;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b && !b;
}
