using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using MinecraftLaunch.Classes.Models.Install;
using MinecraftLaunch.Components.Installer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WonderLab.Extensions;
using WonderLab.Infrastructure.Enums;
using WonderLab.ViewModels.Dialog.Download;
using WonderLab.Views.Dialog.Download;

namespace WonderLab.ViewModels.Page.Download;

public sealed partial class MinecraftListPageViewModel : ObservableObject {
    readonly struct MinecraftCache {
        public static List<VersionManifestEntry> MinecraftList { set; get; }
    }

    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ObservableCollection<VersionManifestEntry> _allMinecraftList = [];

    [ObservableProperty] private VersionType _activeVersion = VersionType.Old_Alpha;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsHasMinecraft))]
    private ReadOnlyObservableCollection<VersionManifestEntry> _minecraftList;

    public bool IsHasMinecraft => MinecraftList is null;

    public MinecraftListPageViewModel() {
        _cancellationTokenSource = new();
    }

    [RelayCommand]
    private Task OnLoaded() => Task.Run(async () => {
        await Dispatcher.UIThread.InvokeAsync(async () => {
            InstallMinecraftDialog dialog = new() {
                DataContext = App.Get<InstallMinecraftDialogViewModel>()
            };

            await DialogHost.Show(dialog, "PART_DialogHost");
        });

        if (MinecraftCache.MinecraftList?.Count > 0) {
            MinecraftList = new(_allMinecraftList);
            ActiveVersion = VersionType.Release;
            return;
        }

        var result = await VanlliaInstaller.EnumerableGameCoreAsync(_cancellationTokenSource.Token);
        MinecraftCache.MinecraftList = result.ToList();
        MinecraftList = new(_allMinecraftList);
        ActiveVersion = VersionType.Release;
    });

    [RelayCommand]
    private void OnUnloaded() {
        _cancellationTokenSource.Cancel();
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e) {
        base.OnPropertyChanged(e);

        if (e.PropertyName is nameof(ActiveVersion)) {
            _allMinecraftList.Clear();
            var list = ActiveVersion switch {
                VersionType.Release => MinecraftCache.MinecraftList.Where(x => x.Type is "release").ToObservableList(),
                VersionType.Snapshot => MinecraftCache.MinecraftList.Where(x => x.Type is "snapshot").ToObservableList(),
                VersionType.Old_Beta => MinecraftCache.MinecraftList.Where(x => x.Type is "old_beta").ToObservableList(),
                VersionType.Old_Alpha => MinecraftCache.MinecraftList.Where(x => x.Type is "old_alpha").ToObservableList(),
                _ => MinecraftCache.MinecraftList.ToObservableList()
            };

            foreach (var item in list)
                _allMinecraftList.Add(item);
        }
    }
}