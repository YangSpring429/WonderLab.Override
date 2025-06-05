using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace WonderLab.Media.Converters;

public sealed class NullToVisibilityConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return value is not null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}