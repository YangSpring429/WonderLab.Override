using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace WonderLab.Controls.Media.Converters;

public sealed class ObjectToEnumerableConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        return value as IEnumerable<object>;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}