using Avalonia;
using Avalonia.Controls.Primitives;

namespace WonderLab.Controls;

public sealed class FontIcon : TemplatedControl {
    public static readonly StyledProperty<string> GlyphProperty =
        AvaloniaProperty.Register<FontIcon, string>(nameof(Glyph), "\uE76E");

    public string Glyph {
        get => GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }
}