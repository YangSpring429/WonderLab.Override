using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;

namespace WonderLab.Controls;

[TemplatePart("PART_ProgressBar", typeof(ProgressBar), IsRequired = true)]
public sealed class ProgressButton : Button {
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<RangeBase, double>(nameof(Value), 0.0, false, BindingMode.TwoWay);

    public double Value {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e) {
        base.OnApplyTemplate(e);
    }
}