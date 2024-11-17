using Avalonia.Controls;
using Avalonia.Platform;
using System.Diagnostics;

namespace WonderLab;

public sealed partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
        var r = TryGetPlatformHandle().Handle;
        Debug.WriteLine(r);
    }
}