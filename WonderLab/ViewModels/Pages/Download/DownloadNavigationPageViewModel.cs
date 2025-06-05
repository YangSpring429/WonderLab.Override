using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace WonderLab.ViewModels.Pages.Download;

public sealed partial class DownloadNavigationPageViewModel : ObservableObject {
    [ObservableProperty] private ObservableCollection<string> _headerItems = ["Minecraft"];
}