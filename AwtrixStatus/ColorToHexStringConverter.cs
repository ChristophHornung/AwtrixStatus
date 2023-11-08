using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace AwtrixStatus;

public class ColorToHexStringConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not string hexColor)
		{
			return DependencyProperty.UnsetValue;
		}

		if (hexColor.StartsWith("#"))
		{
			hexColor = hexColor[1..];
		}

		if (hexColor.Length == 6)
		{
			byte r = byte.Parse(hexColor[..2], NumberStyles.HexNumber);
			byte g = byte.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);
			byte b = byte.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);

			return Color.FromRgb(r, g, b);
		}

		return DependencyProperty.UnsetValue;
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value == null)
		{
			return null;
		}

		return value is not Color color ? DependencyProperty.UnsetValue : $"#{color.R:X2}{color.G:X2}{color.B:X2}";
	}
}