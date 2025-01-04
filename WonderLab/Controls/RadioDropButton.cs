using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Collections;

namespace WonderLab.Controls;

public sealed class RadioDropButton : RadioButton {
    public static readonly StyledProperty<IEnumerable> ItemsSourceProperty =
        AvaloniaProperty.Register<RadioDropButton, IEnumerable>(nameof(ItemsSource), new AvaloniaList<object>());

    public static readonly StyledProperty<IDataTemplate> ItemTemplateProperty =
        AvaloniaProperty.Register<RadioDropButton, IDataTemplate>(nameof(ItemTemplate));

    public static readonly StyledProperty<bool> IsDropDownOpenProperty =
        AvaloniaProperty.Register<RadioDropButton, bool>(nameof(IsDropDownOpen));

    public static readonly StyledProperty<object> SelectedItemProperty =
        AvaloniaProperty.Register<RadioDropButton, object>(nameof(SelectedItem));

    public IEnumerable ItemsSource {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public object SelectedItem {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public IDataTemplate ItemTemplate {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public bool IsDropDownOpen {
        get => GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e) {
        base.OnPointerPressed(e);
        //ListBox
        SetCurrentValue(IsDropDownOpenProperty, !IsDropDownOpen);
        e.Handled = true;
    }

    protected override void OnLostFocus(RoutedEventArgs e) {
        base.OnLostFocus(e);

        if (IsDropDownOpen) {
            SetCurrentValue(IsDropDownOpenProperty, false);
            e.Handled = true;
        }
    }
}