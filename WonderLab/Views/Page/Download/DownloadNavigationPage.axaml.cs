using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Threading.Tasks;
using WonderLab.Controls;

namespace WonderLab.Views.Page.Download;

public partial class DownloadNavigationPage : UserControl {
    public DownloadNavigationPage() {
        InitializeComponent();
    }

    protected override async void OnLoaded(RoutedEventArgs e) {
        base.OnLoaded(e);

        await Task.Delay(200);
        Tile.RunParentPanelAnimation(PART_TileListBox?.ItemsPanelRoot);
    }
}