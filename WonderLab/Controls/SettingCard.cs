using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;
using System.Windows.Input;

namespace WonderLab.Controls;

public class SettingCard : ContentControl {
    public static readonly StyledProperty<string> IconProperty =
        AvaloniaProperty.Register<SettingCard, string>(nameof(Icon));

    public static readonly StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<SettingCard, string>(nameof(Header), "Title");

    public string Icon {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string Header {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }
}

public sealed class SettingExpandCard : ItemsControl {
    public static readonly StyledProperty<string> IconProperty =
        AvaloniaProperty.Register<SettingExpandCard, string>(nameof(Icon));

    public static readonly StyledProperty<bool> IsExpandedProperty =
        AvaloniaProperty.Register<SettingExpandCard, bool>(nameof(IsExpanded));

    public static readonly StyledProperty<object> HeaderProperty =
        AvaloniaProperty.Register<SettingExpandCard, object>(nameof(Header), "Title");

    public static readonly StyledProperty<bool> CanExpandedProperty =
        AvaloniaProperty.Register<SettingExpandCard, bool>(nameof(CanExpanded));

    public static readonly StyledProperty<object> FooterProperty =
        AvaloniaProperty.Register<SettingExpandCard, object>(nameof(Footer));

    public string Icon {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public object Header {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public object Footer {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }
    public bool IsExpanded {
        get => GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    public bool CanExpanded {
        get => GetValue(CanExpandedProperty);
        set => SetValue(CanExpandedProperty, value);
    }
}