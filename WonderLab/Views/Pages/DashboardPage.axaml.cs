using Avalonia.Controls;
using WonderLab.Classes.Models;

namespace WonderLab;

public partial class DashboardPage : UserControl {
    public DashboardPage() {
        InitializeComponent();
    }

    private void OnSelectTemplateKey(object sender, SelectTemplateEventArgs e) {
        e.TemplateKey = e.DataContext is SinglePlayerSaveModel
            ? "SinglePlayer"
            : "MultiPlayer";
    }
}