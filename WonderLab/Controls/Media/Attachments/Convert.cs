using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;

namespace WonderLab.Controls.Media.Attachments;
public sealed class Convert : AvaloniaObject {
    static Convert() {
        ObjectToEnumerableProperty.Changed.AddClassHandler<Interactive>(HandleObjectToEnumerableChanged);
    }

    public static readonly AttachedProperty<object> ObjectToEnumerableProperty =
        AvaloniaProperty.RegisterAttached<Convert, Interactive, object>("ObjectToEnumerable", default, false, BindingMode.OneWay, null);

    private static void HandleObjectToEnumerableChanged(Interactive interactive, AvaloniaPropertyChangedEventArgs args) {
        //var r = (Flyout)interactive;
    }
}