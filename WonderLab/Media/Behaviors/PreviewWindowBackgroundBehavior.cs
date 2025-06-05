using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Xaml.Interactivity;
using WonderLab.Controls;
using WonderLab.Services;

namespace WonderLab.Media.Behaviors;

public sealed class PreviewWindowBackgroundBehavior : Behavior<Border> {
    private ThemeService _themeService;

    protected override void OnAttached() {
        _themeService = App.Get<ThemeService>();
    }

    protected override void OnLoaded() {
        base.OnLoaded();
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime
                && lifetime.MainWindow is WonderWindow window) {
            if (AssociatedObject is null)
                return;

            AssociatedObject.Background = window.Background;
        }
    }

    protected override void OnAttachedToLogicalTree() {
        _themeService.BackgroundTypeChanged += OnBackgroundTypeChanged;
    }

    protected override void OnDetachedFromLogicalTree() {
        _themeService.BackgroundTypeChanged -= OnBackgroundTypeChanged;
    }

    private void OnBackgroundTypeChanged(object sender, System.EventArgs e) {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime
                && lifetime.MainWindow is WonderWindow window) {
            if (AssociatedObject is null)
                return;

            AssociatedObject.Background = window.Background;
        }
    }
}