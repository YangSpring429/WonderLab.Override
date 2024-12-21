using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftLaunch.Classes.Models.Install;
using MinecraftLaunch.Components.Installer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WonderLab.ViewModels.Page.Download;

public sealed partial class MinecraftListPageViewModel : ObservableObject {
    readonly struct MinecraftCache {
        public static List<VersionManifestEntry> MinecraftList { set; get; }
    }

    private CancellationTokenSource _cancellationTokenSource;
    private ObservableCollection<VersionManifestEntry> _allMinecraftList;

    [ObservableProperty]
    private ReadOnlyObservableCollection<VersionManifestEntry> _minecraftList;

    public MinecraftListPageViewModel() {
        _cancellationTokenSource = new();
    }

    [RelayCommand]
    private Task OnLoaded() => Task.Run(async () => {
        if (MinecraftCache.MinecraftList?.Count > 0) {
            MinecraftList = new(_allMinecraftList = new(MinecraftCache.MinecraftList));
            return;
        }

        var result = await VanlliaInstaller.EnumerableGameCoreAsync(_cancellationTokenSource.Token);
        MinecraftCache.MinecraftList = result.ToList();
        MinecraftList = new(_allMinecraftList = new(MinecraftCache.MinecraftList));
    });

    [RelayCommand]
    private void OnUnloaded() {
        _cancellationTokenSource.Cancel();
    }
}