using System.Globalization;
using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Media;
using Fruits.Domain.DB;
using System.Linq;

namespace Fruits.Wpf.Core.View.Converters;

public class ConverterForegroundTextError : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (parameter is string propName)
		{
			if (value is List<string> item)
			{
				if (item.Contains(propName))
				{
					return new SolidColorBrush(Colors.Crimson);
				}
			}

			if (value is List<StockFruits> stockFruits)
			{
				if (stockFruits.Any(y => y.ListHasErrorProperty.Any(x=>x == propName)))
				{
					return new SolidColorBrush(Colors.Crimson);
				}
			}
		}
	   
		return new SolidColorBrush(Colors.Black);
	}

	public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

}