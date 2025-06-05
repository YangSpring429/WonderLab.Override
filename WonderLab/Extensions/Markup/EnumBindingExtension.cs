using Avalonia.Markup.Xaml;
using System;

namespace WonderLab.Extensions.Markup;

public sealed class EnumBindingExtension(Type enumType) : MarkupExtension {
    public override object ProvideValue(IServiceProvider serviceProvider) {
        return Enum.GetValues(enumType);
    }
}