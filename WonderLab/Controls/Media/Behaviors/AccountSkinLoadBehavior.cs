using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace WonderLab.Controls.Media.Behaviors;

public sealed class AccountSkinLoadBehavior : Behavior<Border> {
    protected override void OnAttached() {
        AssociatedObject.Loaded += OnLoaded;
        AssociatedObject.Unloaded += OnUnloaded;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change) {
        base.OnPropertyChanged(change);

        //if (change.Property is ) {

        //}
    }

    private void OnLoaded(object sender, Avalonia.Interactivity.RoutedEventArgs e) {
        throw new System.NotImplementedException();
    }

    private void OnUnloaded(object sender, Avalonia.Interactivity.RoutedEventArgs e) {
        throw new System.NotImplementedException();
    }
}