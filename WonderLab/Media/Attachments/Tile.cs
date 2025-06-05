using Avalonia;
using Avalonia.Controls;
using WonderLab.Controls;

namespace WonderLab.Media.Attachments;

public static class NavigationTileExtensions {
    public static readonly AttachedProperty<double> ValueProperty =
        AvaloniaProperty.RegisterAttached<NavigationTile, Control, double>("Value", 0d);

    public static readonly AttachedProperty<object> ContentProperty =
        AvaloniaProperty.RegisterAttached<NavigationTile, Control, object>("Content", string.Empty);

    public static object GetValue(NavigationTile tile) {
        return tile.GetValue(ValueProperty);
    }

    public static object GetContent(NavigationTile tile) {
        return tile.GetValue(ContentProperty);
    }

    public static object SetValue(NavigationTile tile, double value) {
        return tile.SetValue(ValueProperty, value);
    }

    public static object SetContent(NavigationTile tile, object value) {
        return tile.SetValue(ContentProperty, value);
    }
}