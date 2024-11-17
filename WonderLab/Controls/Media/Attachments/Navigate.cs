using Avalonia;
using Avalonia.Controls.Presenters;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System;
using WonderLab.Extensions.Hosting.UI;

namespace WonderLab.Controls.Media.Attachments;

[Obsolete("Replaced by new navigation component.")]
public sealed class Navigate : AvaloniaObject {
    private readonly static AvaloniaPageProvider _pageProvider = null;

    static Navigate() {
        KeyProperty.Changed.AddClassHandler<Interactive>(HandleKeyChanged);
    }

    public static readonly AttachedProperty<object> KeyProperty =
        AvaloniaProperty.RegisterAttached<Navigate, Interactive, object>("Key", default, false, BindingMode.OneWay, null);

    private static void HandleKeyChanged(Interactive interactive, AvaloniaPropertyChangedEventArgs args) {
        if (interactive is ContentPresenter presenter && args.Property == KeyProperty) {
            Dispatcher.UIThread.Post(() => presenter.Content = _pageProvider.GetPage(args.GetNewValue<string>()));
        }
    }
}