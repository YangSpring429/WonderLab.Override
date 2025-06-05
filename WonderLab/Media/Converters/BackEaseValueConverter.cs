using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace WonderLab.Media.Converters;

public sealed class BackEaseValueConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return System.Convert.ToDouble(value) + System.Convert.ToDouble(parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}