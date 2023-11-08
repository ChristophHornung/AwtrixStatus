using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace AwtrixStatus;

public class ColorToBrushConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not Color color)
		{
			return DependencyProperty.UnsetValue;
		}

		return new SolidColorBrush(color);
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not SolidColorBrush brush)
		{
			return DependencyProperty.UnsetValue;
		}

		return brush.Color;
	}
}