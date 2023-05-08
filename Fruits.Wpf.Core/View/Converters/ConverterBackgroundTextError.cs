using System.Globalization;
using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Media;

namespace Fruits.Wpf.Core.View.Converters;

public class ConverterBackgroundTextError : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is List<string> item && parameter is string propName)
		{
			if (item.Contains(propName))
			{
				return new SolidColorBrush(Colors.LightCoral);
			}
		}

		return new SolidColorBrush(Colors.Transparent);
	}

	public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

}