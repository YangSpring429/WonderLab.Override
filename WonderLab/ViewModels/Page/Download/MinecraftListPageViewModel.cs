using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DialogHostAvalonia;
using MinecraftLaunch.Classes.Models.Install;
using MinecraftLaunch.Components.Installer;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WonderLab.Extensions;
using WonderLab.Infrastructure.Enums;
using WonderLab.Services.Download;
using WonderLab.ViewModels.Dialog.Download;
using WonderLab.Views.Dialog.Download;

namespace WonderLab.ViewModels.Page.Download;

public sealed partial class MinecraftListPageViewModel : ObservableObject {
    private readonly CacheService _cacheService;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly ObservableCollection<VersionManifestEntry> _allMinecraftList = [];

    [ObservableProperty] private VersionType _activeVersion = VersionType.Old_Alpha;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsHasMinecraft))]
    private ReadOnlyObservableCollection<VersionManifestEntry> _minecraftList;

    public bool IsHasMinecraft => MinecraftList is null;

    public MinecraftListPageViewModel(CacheService cacheService) {
        _cacheService = cacheService;
        _cancellationTokenSource = new();
    }

    [RelayCommand]
    private Task OnLoaded() => Task.Run(async () => {
        if (_cacheService.MinecraftList?.Count > 0) {
            MinecraftList = new(_allMinecraftList);
            ActiveVersion = VersionType.Release;
            return;
        }

        var result = await VanlliaInstaller.EnumerableGameCoreAsync(_cancellationTokenSource.Token);
        _cacheService.MinecraftList.AddRange(result.ToImmutableList());
        MinecraftList = new(_allMinecraftList);
        ActiveVersion = VersionType.Release;
    });

    [RelayCommand]
    private void OnUnloaded() {
        _cancellationTokenSource.Cancel();
    }

    [RelayCommand]
    private Task OpenInstallDialog(string id) => Dispatcher.UIThread.InvokeAsync(async () => {
        var dialogVM = App.Get<InstallMinecraftDialogViewModel>();
        dialogVM.GameCoreId = id;

        InstallMinecraftDialog dialog = new() {
            DataContext = dialogVM
        };

        await DialogHost.Show(dialog, "PART_DialogHost");
    });

    protected override void OnPropertyChanged(PropertyChangedEventArgs e) {
        base.OnPropertyChanged(e);

        if (e.PropertyName is nameof(ActiveVersion)) {
            _allMinecraftList.Clear();

            var mcList = _cacheService.MinecraftList;
            var list = ActiveVersion switch {
                VersionType.Release => mcList.Where(x => x.Type is "release").ToObservableList(),
                VersionType.Snapshot => mcList.Where(x => x.Type is "snapshot").ToObservableList(),
                VersionType.Old_Beta => mcList.Where(x => x.Type is "old_beta").ToObservableList(),
                VersionType.Old_Alpha => mcList.Where(x => x.Type is "old_alpha").ToObservableList(),
                _ => mcList.ToObservableList()
            };

            foreach (var item in list) _allMinecraftList.Add(item);
        }
    }
}