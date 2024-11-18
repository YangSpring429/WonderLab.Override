using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using System;

#if OSX
using WonderLab.Platform.MacOS;
#elif LINUX
using WonderLab.Platform.Linux;
#endif

namespace WonderLab.Controls.Media.Behaviors;

public sealed class WindowTitleBarBehavior : Behavior<Window> {
    protected override void OnAttached() {
        base.OnAttached();

        AssociatedObject.Opened += OnOpened;
        AssociatedObject.PropertyChanged += OnPropertyChanged;
    }

    private bool IsWayland() {
        string sessionType = Environment.GetEnvironmentVariable("XDG_SESSION_TYPE").ToLower();
        return sessionType is "wayland";
    }

    private void OnPropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e) {
#if OSX
        if (e.Property == Window.WindowStateProperty || e.Property == Window.BoundsProperty) {
            var platform = AssociatedObject.TryGetPlatformHandle();
            if (platform is not null) {
                var nsWindow = platform.Handle;
                if (nsWindow != IntPtr.Zero) {
                    try {
                        WindowHandler.RefreshTitleBarButtonPosition(nsWindow);
                        WindowHandler.HideZoomButton(nsWindow);
                    } catch (Exception exception) {
                        Console.WriteLine(exception);
                        throw;
                    }
                }
            }
        }
#elif Unix


#endif
    }

    private void OnOpened(object sender, EventArgs e) {
#if OSX
        var platform = AssociatedObject.TryGetPlatformHandle();
        if (platform is not null) {
            var nsWindow = platform.Handle;
            if (nsWindow != IntPtr.Zero) {
                try {
                    WindowHandler.RefreshTitleBarButtonPosition(nsWindow);
                    WindowHandler.HideZoomButton(nsWindow);
                }
                catch (Exception exception) {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }
#elif LINUX
        var platform = AssociatedObject.TryGetPlatformHandle();
        if (platform is not null) {
            if (IsWayland()) {
            } else {
                Platform.Linux.X11.WindowHandler.HideTitleBar(platform.Handle);
            }
        }
#endif
    }
}