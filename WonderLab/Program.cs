using Avalonia;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace WonderLab;

internal sealed class Program {
    [STAThread]
    public static void Main(string[] args) {
        try {
            var app = BuildAvaloniaApp();
            app.StartWithClassicDesktopLifetime(args);
        } catch (Exception ex) {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"WonderLab", "logs", $"crash.log");
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Directory.Exists)
                fileInfo.Directory.Create();

            File.WriteAllText(fileInfo.FullName, ex.ToString());
        }
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .With(new Win32PlatformOptions {
                WinUICompositionBackdropCornerRadius = 8,
                RenderingMode = RuntimeInformation.ProcessArchitecture == Architecture.Arm || RuntimeInformation.ProcessArchitecture == Architecture.Arm64
                    ? [Win32RenderingMode.Wgl]
                    : [Win32RenderingMode.AngleEgl, Win32RenderingMode.Wgl, Win32RenderingMode.Software]!,
            }).With(new MacOSPlatformOptions {
                DisableAvaloniaAppDelegate = true,
                DisableDefaultApplicationMenuItems = true,
            }).With(new X11PlatformOptions {
                OverlayPopups = true,
            }).With(new SkiaOptions {
                MaxGpuResourceSizeBytes = null
            });
}