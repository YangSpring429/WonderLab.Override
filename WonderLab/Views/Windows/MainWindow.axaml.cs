using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using WonderLab.Controls;

namespace WonderLab.Views.Windows;

public partial class MainWindow : WonderWindow {
    public MainWindow() {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e) {
        base.OnLoaded(e);

#if DEBUG
        Temp.IsVisible = false;
#endif
    }
}