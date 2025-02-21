using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Threading.Tasks;
using WonderLab.Controls;

namespace WonderLab.Views.Page.Setting;

public sealed partial class SettingNavigationPage : UserControl {
    public SettingNavigationPage() {
        InitializeComponent();
    }

    protected override async void OnLoaded(RoutedEventArgs e) {
        base.OnLoaded(e);

        await Task.Delay(200);
        Tile.RunParentPanelAnimation(PART_TileListBox?.ItemsPanelRoot, true);
    }
}