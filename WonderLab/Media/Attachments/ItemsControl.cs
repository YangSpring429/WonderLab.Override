using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Rendering.Composition;
using WonderLab.Extensions;

namespace WonderLab.Media.Attachments;

public static class ListBoxExtensions {
    public static readonly AttachedProperty<bool> AnimatedScrollProperty =
        AvaloniaProperty.RegisterAttached<ListBox, bool>("AnimatedScroll", typeof(ListBox), defaultValue: false);

    static ListBoxExtensions() {
        AnimatedScrollProperty.Changed.AddClassHandler<ListBox>(HandleAnimatedScrollChanged);
    }

    private static void HandleAnimatedScrollChanged(ListBox interactElem, AvaloniaPropertyChangedEventArgs args) {
        if (GetAnimatedScroll(interactElem))
            interactElem.AttachedToVisualTree += (sender, args) =>
                ScrollableExtension.ApplyScrollAnimated(ElementComposition.GetElementVisual(interactElem));
    }

    public static bool GetAnimatedScroll(ListBox wrap) {
        return wrap.GetValue(AnimatedScrollProperty);
    }

    public static void SetAnimatedScroll(ListBox wrap, bool value) {
        wrap.SetValue(AnimatedScrollProperty, value);
    }
}

public static class ItemsControlExtensions {
    public static readonly AttachedProperty<bool> AnimatedScrollProperty =
        AvaloniaProperty.RegisterAttached<ItemsControl, bool>("AnimatedScroll", typeof(ItemsControl), defaultValue: false);

    static ItemsControlExtensions() {
        AnimatedScrollProperty.Changed.AddClassHandler<ItemsControl>(HandleAnimatedScrollChanged);
    }

    private static void HandleAnimatedScrollChanged(ItemsControl interactElem, AvaloniaPropertyChangedEventArgs args) {
        if (GetAnimatedScroll(interactElem))
            interactElem.AttachedToVisualTree += (sender, args) => 
                ScrollableExtension.ApplyScrollAnimated(ElementComposition.GetElementVisual(interactElem));
    }

    public static bool GetAnimatedScroll(ItemsControl wrap) {
        return wrap.GetValue(AnimatedScrollProperty);
    }

    public static void SetAnimatedScroll(ItemsControl wrap, bool value) {
        wrap.SetValue(AnimatedScrollProperty, value);
    }
}

public static class ItemsRepeaterExtensions {
    public static readonly AttachedProperty<bool> AnimatedScrollProperty =
        AvaloniaProperty.RegisterAttached<ItemsRepeater, bool>("AnimatedScroll", typeof(ItemsRepeater), defaultValue: false);
    
    static ItemsRepeaterExtensions() {
        AnimatedScrollProperty.Changed.AddClassHandler<ItemsRepeater>(HandleAnimatedScrollChanged);
    }

    private static void HandleAnimatedScrollChanged(ItemsRepeater interactElem, AvaloniaPropertyChangedEventArgs args) {
        if (GetAnimatedScroll(interactElem))
            interactElem.AttachedToVisualTree += (sender, args) =>
                ScrollableExtension.ApplyScrollAnimated(ElementComposition.GetElementVisual(interactElem));
    }

    public static bool GetAnimatedScroll(ItemsRepeater wrap) {
        return wrap.GetValue(AnimatedScrollProperty);
    }

    public static void SetAnimatedScroll(ItemsRepeater wrap, bool value) {
        wrap.SetValue(AnimatedScrollProperty, value);
    }
}

public static class ItemsPresenterExtensions {
    public static readonly AttachedProperty<bool> AnimatedScrollProperty =
        AvaloniaProperty.RegisterAttached<ItemsPresenter, bool>("AnimatedScroll", typeof(ItemsPresenter), defaultValue: false);

    static ItemsPresenterExtensions() {
        AnimatedScrollProperty.Changed.AddClassHandler<ItemsPresenter>(HandleAnimatedScrollChanged);
    }

    private static void HandleAnimatedScrollChanged(ItemsPresenter interactElem, AvaloniaPropertyChangedEventArgs args) {
        if (GetAnimatedScroll(interactElem))
            interactElem.AttachedToVisualTree += (sender, args) =>
                ScrollableExtension.ApplyScrollAnimated(ElementComposition.GetElementVisual(interactElem));
    }

    public static bool GetAnimatedScroll(ItemsPresenter wrap) {
        return wrap.GetValue(AnimatedScrollProperty);
    }

    public static void SetAnimatedScroll(ItemsPresenter wrap, bool value) {
        wrap.SetValue(AnimatedScrollProperty, value);
    }
}
