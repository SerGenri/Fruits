using System.Globalization;
using System;
using System.Windows.Data;

namespace Fruits.Wpf.Core.View.Converters;

public class ConverterWidth : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool successParam = int.TryParse(parameter.ToString(), out var param);
		bool successValue = int.TryParse(value.ToString(), out var width);

		if (successValue && successParam)
		{
			return width - param;
		}

		return value;
	}

	public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

}