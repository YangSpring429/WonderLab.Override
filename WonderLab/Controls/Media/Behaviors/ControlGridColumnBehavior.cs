using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace WonderLab.Controls.Media.Behaviors;

public sealed class ControlGridColumnBehavior : Behavior<Control> {
    public static readonly StyledProperty<int> SourceColumnProperty =
        AvaloniaProperty.Register<ControlGridColumnBehavior, int>(nameof(SourceColumn));

    public static readonly StyledProperty<int> TargetColumnProperty =
        AvaloniaProperty.Register<ControlGridColumnBehavior, int>(nameof(TargetColumn));

    public static readonly StyledProperty<double> MaxValueProperty =
        AvaloniaProperty.Register<ControlGridColumnBehavior, double>(nameof(TargetColumn));

    public int SourceColumn {
        get => GetValue(SourceColumnProperty);
        set => SetValue(SourceColumnProperty, value);
    }

    public int TargetColumn {
        get => GetValue(TargetColumnProperty);
        set => SetValue(TargetColumnProperty, value);
    }

    public double MaxValue {
        get => GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }

    protected override void OnAttached() {
        base.OnAttached();
        AssociatedObject.PropertyChanged += OnAssociatedObjectPropertyChanged;
    }

    protected override void OnDetaching() {
        base.OnDetaching();
        AssociatedObject.PropertyChanged -= OnAssociatedObjectPropertyChanged;
    }

    private void OnAssociatedObjectPropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e) {
        if (e.Property == Visual.BoundsProperty && e.GetNewValue<Rect>().Width >= 1000) {
            Grid.SetColumn(AssociatedObject, TargetColumn);
        } else if (e.Property == Visual.BoundsProperty && e.GetNewValue<Rect>().Width < 1000) {
            Grid.SetColumn(AssociatedObject, SourceColumn);
        }
    }
}

public sealed class GridColumnBehavior : Behavior<Grid> {
    public static readonly StyledProperty<ColumnDefinition> SpacingColumnProperty =
        AvaloniaProperty.Register<GridColumnBehavior, ColumnDefinition>(nameof(SpacingColumn));

    public static readonly StyledProperty<ColumnDefinition> FirstColumnProperty =
        AvaloniaProperty.Register<GridColumnBehavior, ColumnDefinition>(nameof(FirstColumn));

    public static readonly StyledProperty<ColumnDefinition> LastColumnProperty =
        AvaloniaProperty.Register<GridColumnBehavior, ColumnDefinition>(nameof(LastColumn));

    public ColumnDefinition SpacingColumn {
        get => GetValue(SpacingColumnProperty);
        set => SetValue(SpacingColumnProperty, value);
    }

    public ColumnDefinition FirstColumn {
        get => GetValue(FirstColumnProperty);
        set => SetValue(FirstColumnProperty, value);
    }

    public ColumnDefinition LastColumn {
        get => GetValue(LastColumnProperty);
        set => SetValue(LastColumnProperty, value);
    }

    protected override void OnAttached() {
        base.OnAttached();
        AssociatedObject.PropertyChanged += OnAssociatedObjectPropertyChanged;
    }

    protected override void OnDetaching() {
        base.OnDetaching();
        AssociatedObject.PropertyChanged -= OnAssociatedObjectPropertyChanged;
    }

    private void OnAssociatedObjectPropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e) {
        if (e.Property == Visual.BoundsProperty && e.GetNewValue<Rect>().Width >= 800) {
            AssociatedObject.ColumnDefinitions = [FirstColumn, SpacingColumn, LastColumn];
        } else if (e.Property == Visual.BoundsProperty && e.GetNewValue<Rect>().Width < 800) {
            AssociatedObject.ColumnDefinitions = [FirstColumn];
        }
    }
}