using System.Globalization;
using System.Globalization;
using System;
using System.Windows.Data;
using System.Windows;

namespace Fruits.Wpf.Core.View.Converters;

public class ConverterVisibility : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool item)
		{
			if (item)
			{
				return Visibility.Visible;
			}
		}

		return Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

}