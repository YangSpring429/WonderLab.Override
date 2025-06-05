using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using System;
using System.Web;

namespace WonderLab.Media.Behaviors;

public sealed class DropYggdrasilUrlBehavior : Behavior<TextBox> {
    private const string URL_PREFIX = "authlib-injector:yggdrasil-server:";

    protected override void OnAttachedToVisualTree() {
        DragDrop.SetAllowDrop(AssociatedObject, true);
        AssociatedObject?.AddHandler(DragDrop.DropEvent, OnDrop);
    }

    protected override void OnDetachedFromVisualTree() {
        AssociatedObject?.RemoveHandler(DragDrop.DropEvent, OnDrop);
    }

    private void OnDrop(object sender, DragEventArgs args) {
        var text = args.Data.GetText();
        if (string.IsNullOrEmpty(text))
            return;

        if (text.StartsWith(URL_PREFIX, StringComparison.OrdinalIgnoreCase))
            AssociatedObject.Text = HttpUtility.UrlDecode(text[URL_PREFIX.Length..]); ;
    }
}