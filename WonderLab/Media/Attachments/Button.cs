using Avalonia;
using Avalonia.Controls;

namespace WonderLab.Media.Attachments;

public static class ButtonExtensions {
    public static readonly AttachedProperty<double> ValueProperty =
        AvaloniaProperty.RegisterAttached<Button, Control, double>("Value", 0.0);

    public static readonly AttachedProperty<object> ContentProperty =
        AvaloniaProperty.RegisterAttached<Button, Control, object>("Content", null);

    public static double GetValue(Button button) {
        return button.GetValue(ValueProperty);
    }

    public static void SetValue(Button button, double value) {
        button.SetValue(ValueProperty, value);
    }

    public static object GetContent(Button button) {
        return button.GetValue(ContentProperty);
    }

    public static object SetContent(Button button, object content) {
        return button.SetValue(ContentProperty, content);
    }
}

public static class ProgressBarExtensions {
    public static readonly AttachedProperty<object> ContentProperty =
        AvaloniaProperty.RegisterAttached<ProgressBar, Control, object>("Content", null);

    public static object GetContent(ProgressBar progressBar) {
        return progressBar.GetValue(ContentProperty);
    }

    public static object SetContent(ProgressBar progressBar, object content) {
        return progressBar.SetValue(ContentProperty, content);
    }
}