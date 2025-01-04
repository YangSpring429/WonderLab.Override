using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using System.Collections;

namespace WonderLab.Controls;

public sealed class ItemsFlyout : PopupFlyoutBase {
    public static readonly StyledProperty<IEnumerable> ItemsSourceProperty =
        AvaloniaProperty.Register<ItemsFlyout, IEnumerable>(nameof(ItemsSource), new AvaloniaList<object>());

    public static readonly StyledProperty<IDataTemplate> ItemTemplateProperty =
        AvaloniaProperty.Register<ItemsFlyout, IDataTemplate>(nameof(ItemTemplate));

    public IEnumerable ItemsSource {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public IDataTemplate ItemTemplate {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    protected override Control CreatePresenter() {
        return new ItemsFlyoutPresenter() {
            ItemsSource = ItemsSource,
            ItemTemplate = ItemTemplate,
        };
    }
}

public sealed class ItemsFlyoutPresenter : ItemsControl;