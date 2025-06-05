using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace WonderLab.Media.Converters;

public sealed class OpactiyReverseConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return System.Convert.ToDouble(value) is 0 ? 1d : 0d;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}